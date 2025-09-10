using z3lib;

namespace tests;

public class Z3DisposalTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void ContextThrowsAfterDisposal()
    {
        var context = new Z3Context();
        context.Dispose();

        // All context operations should throw ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(() => _ = context.Handle);
        Assert.Throws<ObjectDisposedException>(() => context.SetParameter("timeout", "1000"));
        Assert.Throws<ObjectDisposedException>(() => context.MkInt(5));
        Assert.Throws<ObjectDisposedException>(() => context.MkIntConst("x"));
        Assert.Throws<ObjectDisposedException>(() => context.MkReal(3.14));
        Assert.Throws<ObjectDisposedException>(() => context.MkRealConst("y"));
        Assert.Throws<ObjectDisposedException>(() => context.MkTrue());
        Assert.Throws<ObjectDisposedException>(() => context.MkFalse());
        Assert.Throws<ObjectDisposedException>(() => context.MkSolver());
        Assert.Throws<ObjectDisposedException>(() => context.MkSimpleSolver());
    }

    [Test]
    public void SolverThrowsAfterDisposal()
    {
        using var context = new Z3Context();
        var solver = context.MkSolver();
        
        solver.Dispose();

        // All solver operations should throw ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(() => _ = solver.Handle);
        Assert.Throws<ObjectDisposedException>(() => solver.Assert(context.MkTrue()));
        Assert.Throws<ObjectDisposedException>(() => solver.Check());
        Assert.Throws<ObjectDisposedException>(() => solver.GetReasonUnknown());
        Assert.Throws<ObjectDisposedException>(() => solver.Push());
        Assert.Throws<ObjectDisposedException>(() => solver.Pop());
    }

    [Test]
    public void SolverThrowsWhenContextDisposed()
    {
        Z3Solver solver;
        
        using (var context = new Z3Context())
        {
            solver = context.MkSolver();
            
            // Solver should work while context is alive
            Assert.DoesNotThrow(() => solver.Check());
        } // Context is disposed here
        
        // Now solver operations should throw ObjectDisposedException when accessing context
        Assert.Throws<ObjectDisposedException>(() => solver.Assert(CreateBoolExprFromAnotherContext()));
        Assert.Throws<ObjectDisposedException>(() => solver.Check());
        Assert.Throws<ObjectDisposedException>(() => solver.GetReasonUnknown());
        Assert.Throws<ObjectDisposedException>(() => solver.Push());
        Assert.Throws<ObjectDisposedException>(() => solver.Pop());
        
        // Clean up
        solver.Dispose();
    }

    [Test]
    public void ContextDisposalOrder_ContextFirst()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        // Add some constraints to make it interesting
        solver.Assert(context.MkTrue());
        
        // Dispose context first
        context.Dispose();
        
        // Solver should still be disposable without crashing
        Assert.DoesNotThrow(() => solver.Dispose());
    }

    [Test]
    public void ContextDisposalOrder_SolverFirst()
    {
        using var context = new Z3Context();
        var solver = context.MkSolver();
        
        // Add some constraints
        solver.Assert(context.MkTrue());
        
        // Dispose solver first
        solver.Dispose();
        
        // Context should still work
        Assert.DoesNotThrow(() => context.MkInt(5));
        Assert.DoesNotThrow(() => context.SetParameter("timeout", "1000"));
    }

    [Test]
    public void MultipleSolversDisposalOrder()
    {
        using var context = new Z3Context();
        var solver1 = context.MkSolver();
        var solver2 = context.MkSimpleSolver();
        var solver3 = context.MkSolver();
        
        // Add constraints to each
        solver1.Assert(context.MkTrue());
        solver2.Assert(context.MkFalse());
        solver3.Assert(context.MkInt(1) == context.MkInt(1));
        
        // Dispose in different order
        solver2.Dispose();
        solver1.Dispose();
        solver3.Dispose();
        
        // Context should still work
        Assert.DoesNotThrow(() => context.MkInt(10));
    }

    [Test]
    public void ContextDisposalCleansUpExpressions()
    {
        var context = new Z3Context();
        
        // Create many expressions to test cleanup
        var expressions = new List<Z3Expr>();
        for (int i = 0; i < 100; i++)
        {
            expressions.Add(context.MkInt(i));
            expressions.Add(context.MkIntConst($"x{i}"));
        }
        
        // Should not throw when disposing context with many tracked expressions
        Assert.DoesNotThrow(() => context.Dispose());
    }

    [Test]
    public void DoubleDisposalIsSafe()
    {
        // Context double disposal
        var context = new Z3Context();
        context.Dispose();
        Assert.DoesNotThrow(() => context.Dispose()); // Should not throw
        
        // Solver double disposal
        using var context2 = new Z3Context();
        var solver = context2.MkSolver();
        solver.Dispose();
        Assert.DoesNotThrow(() => solver.Dispose()); // Should not throw
    }

    [Test]
    public void CreateSolverFromDisposedContextThrows()
    {
        var context = new Z3Context();
        context.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() => context.MkSolver());
        Assert.Throws<ObjectDisposedException>(() => context.MkSimpleSolver());
    }

    [Test]
    public void ComplexDisposalScenario()
    {
        var context = new Z3Context();
        var solvers = new List<Z3Solver>();
        
        // Create multiple solvers with complex constraints
        for (int i = 0; i < 10; i++)
        {
            var solver = context.MkSolver();
            solvers.Add(solver);
            
            var x = context.MkIntConst($"x{i}");
            var y = context.MkIntConst($"y{i}");
            
            solver.Assert(x > context.MkInt(0));
            solver.Assert(y < context.MkInt(100));
            solver.Assert((x + y) == context.MkInt(50));
            
            solver.Push();
            solver.Assert(x == context.MkInt(i * 5));
            
            var result = solver.Check();
            Assert.That(result, Is.EqualTo(Z3Status.Satisfiable).Or.EqualTo(Z3Status.Unsatisfiable));
            
            solver.Pop();
        }
        
        // Dispose some solvers
        for (int i = 0; i < 5; i++)
        {
            solvers[i].Dispose();
        }
        
        // Context should still work with remaining solvers
        for (int i = 5; i < 10; i++)
        {
            Assert.DoesNotThrow(() => solvers[i].Check());
        }
        
        // Clean up remaining solvers
        for (int i = 5; i < 10; i++)
        {
            solvers[i].Dispose();
        }
        
        // Context disposal should not crash
        Assert.DoesNotThrow(() => context.Dispose());
    }

    [Test]
    public void FinalizerDoesNotCrash()
    {
        // Create objects and let them go out of scope to test finalizers
        CreateAndAbandonObjects();
        
        // Force garbage collection to trigger finalizers
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        // If we get here without crashing, finalizers worked correctly
        Assert.Pass("Finalizers completed without crashing");
    }

    private static void CreateAndAbandonObjects()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        solver.Assert(context.MkTrue());
        solver.Check();
        
        // Let objects go out of scope without explicit disposal
        // Finalizers should handle cleanup
    }

    private static Z3BoolExpr CreateBoolExprFromAnotherContext()
    {
        using var tempContext = new Z3Context();
        return tempContext.MkTrue();
    }
}