using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3DisposalTests
{
    [Test]
    public void ContextThrowsAfterDisposal()
    {
        var context = new Z3Context();
        context.Dispose();

        // All context operations should throw ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(() => _ = context.Handle);
        Assert.Throws<ObjectDisposedException>(() => context.SetParameter("timeout", "1000"));
        Assert.Throws<ObjectDisposedException>(() => context.Int(5));
        Assert.Throws<ObjectDisposedException>(() => context.IntConst("x"));
        Assert.Throws<ObjectDisposedException>(() => context.Real(3.14m));
        Assert.Throws<ObjectDisposedException>(() => context.RealConst("y"));
        Assert.Throws<ObjectDisposedException>(() => context.True());
        Assert.Throws<ObjectDisposedException>(() => context.False());
        Assert.Throws<ObjectDisposedException>(() => context.CreateSolver());
        Assert.Throws<ObjectDisposedException>(() => context.CreateSimpleSolver());
    }

    [Test]
    public void SolverThrowsAfterDisposal()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();

        solver.Dispose();

        // All solver operations should throw ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(() => _ = solver.Handle);
        Assert.Throws<ObjectDisposedException>(() => solver.Assert(context.True()));
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
            solver = context.CreateSolver();

            // Solver should work while context is alive
            Assert.DoesNotThrow(() => solver.Check());
        } // Context is disposed here

        // Now solver operations should throw ObjectDisposedException when accessing context
        Assert.Throws<ObjectDisposedException>(() =>
            solver.Assert(CreateBoolExprFromAnotherContext())
        );
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
        var solver = context.CreateSolver();

        // Add some constraints to make it interesting
        solver.Assert(context.True());

        // Dispose context first
        context.Dispose();

        // Solver should still be disposable without crashing
        Assert.DoesNotThrow(() => solver.Dispose());
    }

    [Test]
    public void ContextDisposalOrder_SolverFirst()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();

        // Add some constraints
        solver.Assert(context.True());

        // Dispose solver first
        solver.Dispose();

        // Context should still work
        Assert.DoesNotThrow(() => context.Int(5));
        Assert.DoesNotThrow(() => context.SetParameter("timeout", "1000"));
    }

    [Test]
    public void MultipleSolversDisposalOrder()
    {
        using var context = new Z3Context();
        var solver1 = context.CreateSolver();
        var solver2 = context.CreateSimpleSolver();
        var solver3 = context.CreateSolver();

        // Add constraints to each
        solver1.Assert(context.True());
        solver2.Assert(context.False());
        solver3.Assert(context.Int(1) == context.Int(1));

        // Dispose in different order
        solver2.Dispose();
        solver1.Dispose();
        solver3.Dispose();

        // Context should still work
        Assert.DoesNotThrow(() => context.Int(10));
    }

    [Test]
    public void ContextDisposalCleansUpExpressions()
    {
        var context = new Z3Context();

        // Create many expressions to test cleanup
        var expressions = new List<Z3Expr>();
        for (var i = 0; i < 100; i++)
        {
            expressions.Add(context.Int(i));
            expressions.Add(context.IntConst($"x{i}"));
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
        var solver = context2.CreateSolver();
        solver.Dispose();
        Assert.DoesNotThrow(() => solver.Dispose()); // Should not throw
    }

    [Test]
    public void CreateSolverFromDisposedContextThrows()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => context.CreateSolver());
        Assert.Throws<ObjectDisposedException>(() => context.CreateSimpleSolver());
    }

    [Test]
    public void ComplexDisposalScenario()
    {
        var context = new Z3Context();
        var solvers = new List<Z3Solver>();

        // Create multiple solvers with complex constraints
        for (var i = 0; i < 10; i++)
        {
            var solver = context.CreateSolver();
            solvers.Add(solver);

            var x = context.IntConst($"x{i}");
            var y = context.IntConst($"y{i}");

            solver.Assert(x > context.Int(0));
            solver.Assert(y < context.Int(100));
            solver.Assert((x + y) == context.Int(50));

            solver.Push();
            solver.Assert(x == context.Int(i * 5));

            var result = solver.Check();
            Assert.That(
                result,
                Is.EqualTo(Z3Status.Satisfiable).Or.EqualTo(Z3Status.Unsatisfiable)
            );

            solver.Pop();
        }

        // Dispose some solvers
        for (var i = 0; i < 5; i++)
        {
            solvers[i].Dispose();
        }

        // Context should still work with remaining solvers
        for (var i = 5; i < 10; i++)
        {
            Assert.DoesNotThrow(() => solvers[i].Check());
        }

        // Clean up remaining solvers
        for (var i = 5; i < 10; i++)
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
        var solver = context.CreateSolver();

        solver.Assert(context.True());
        solver.Check();

        // Let objects go out of scope without explicit disposal
        // Finalizers should handle cleanup
    }

    [Test]
    public void ContextDisposesTrackedSolversAutomatically()
    {
        var context = new Z3Context();
        var solver1 = context.CreateSolver();
        var solver2 = context.CreateSimpleSolver();

        // Add constraints to verify solvers are working
        solver1.Assert(context.True());
        solver2.Assert(context.False());

        // Dispose context - should automatically dispose both solvers
        context.Dispose();

        // Solvers should now be disposed and throw when accessed
        Assert.Throws<ObjectDisposedException>(() => _ = solver1.Handle);
        Assert.Throws<ObjectDisposedException>(() => _ = solver2.Handle);
        Assert.Throws<ObjectDisposedException>(() => solver1.Check());
        Assert.Throws<ObjectDisposedException>(() => solver2.Check());
    }

    [Test]
    public void SolverDisposalIsTrackedByContext()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();

        // Solver should work initially
        solver.Assert(context.True());
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Dispose solver explicitly - context should handle it
        solver.Dispose();

        // Solver should now be disposed
        Assert.Throws<ObjectDisposedException>(() => _ = solver.Handle);
        Assert.Throws<ObjectDisposedException>(() => solver.Check());

        // Context should still work fine
        Assert.DoesNotThrow(() => context.Int(5));
    }

    private static Z3Bool CreateBoolExprFromAnotherContext()
    {
        using var tempContext = new Z3Context();
        return tempContext.True();
    }
}
