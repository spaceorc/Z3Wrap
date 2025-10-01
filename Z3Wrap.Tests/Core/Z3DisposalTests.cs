using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3DisposalTests
{
    [Test]
    public void Solver_AfterDisposal_ThrowsObjectDisposedException()
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
    public void Solver_WhenContextDisposed_ThrowsObjectDisposedException()
    {
        Z3Solver solver;

        using (var context = new Z3Context())
        {
            solver = context.CreateSolver();

            // Solver should work while context is alive
            Assert.DoesNotThrow(() => solver.Check());
        } // Context is disposed here

        // Now solver operations should throw ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(() => solver.Check());
        Assert.Throws<ObjectDisposedException>(() => solver.GetReasonUnknown());
        Assert.Throws<ObjectDisposedException>(() => solver.Push());
        Assert.Throws<ObjectDisposedException>(() => solver.Pop());

        // Clean up
        solver.Dispose();
    }

    [Test]
    public void DisposalOrder_ContextFirst_DoesNotCrash()
    {
        var context = new Z3Context();
        var solver = context.CreateSolver();

        // Add some constraints
        solver.Assert(context.True());

        // Dispose context first
        context.Dispose();

        // Solver should still be disposable without crashing
        Assert.DoesNotThrow(() => solver.Dispose());
    }

    [Test]
    public void DisposalOrder_SolverFirst_ContextStillWorks()
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
    public void MultipleSolvers_DifferentDisposalOrder_DoesNotCrash()
    {
        using var context = new Z3Context();
        var solver1 = context.CreateSolver();
        var solver2 = context.CreateSimpleSolver();
        var solver3 = context.CreateSolver();

        // Add constraints to each
        solver1.Assert(context.True());
        solver2.Assert(context.False());
        solver3.Assert(context.Int(1) == context.Int(1));

        // Dispose in different order than creation
        solver2.Dispose();
        solver1.Dispose();
        solver3.Dispose();

        // Context should still work
        Assert.DoesNotThrow(() => context.Int(10));
    }

    [Test]
    public void ManyExpressions_ContextDisposal_DoesNotCrash()
    {
        var context = new Z3Context();
        using var scope = context.SetUp();

        // Create many expressions to test cleanup
        for (var i = 0; i < 100; i++)
        {
            _ = context.Int(i);
            _ = context.IntConst($"x{i}");
        }

        // Should not crash when disposing context with many tracked expressions
        Assert.DoesNotThrow(() => context.Dispose());
    }

    [Test]
    public void Solver_DoubleDisposal_DoesNotThrow()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();

        solver.Dispose();
        Assert.DoesNotThrow(() => solver.Dispose()); // Should not throw
    }

    [Test]
    public void CreateSolver_FromDisposedContext_ThrowsObjectDisposedException()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => context.CreateSolver());
        Assert.Throws<ObjectDisposedException>(() => context.CreateSimpleSolver());
    }

    [Test]
    public void ComplexDisposalScenario_MultipleSolversWithConstraints()
    {
        var context = new Z3Context();
        using var scope = context.SetUp();
        var solvers = new List<Z3Solver>();

        // Create multiple solvers with complex constraints
        for (var i = 0; i < 10; i++)
        {
            var solver = context.CreateSolver();
            solvers.Add(solver);

            var x = context.IntConst($"x{i}");
            var y = context.IntConst($"y{i}");

            solver.Assert(x > 0);
            solver.Assert(y < 100);
            solver.Assert(x + y == 50);

            solver.Push();
            solver.Assert(x == i * 5);

            var result = solver.Check();
            Assert.That(result, Is.EqualTo(Z3Status.Satisfiable).Or.EqualTo(Z3Status.Unsatisfiable));

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
    public void Finalizer_DoesNotCrash()
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

    [Test]
    public void Context_DisposesTrackedSolversAutomatically()
    {
        var context = new Z3Context();
        using var scope = context.SetUp();
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
    public void Solver_ExplicitDisposal_IsTrackedByContext()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
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

    private static void CreateAndAbandonObjects()
    {
        var context = new Z3Context();
        using var scope = context.SetUp();
        var solver = context.CreateSolver();

        solver.Assert(context.True());
        solver.Check();

        // Let objects go out of scope without explicit disposal
        // Finalizers should handle cleanup
    }
}
