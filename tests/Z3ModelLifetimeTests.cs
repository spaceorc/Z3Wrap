using z3lib;

namespace tests;

public class Z3ModelLifetimeTests
{
    [Test]
    public void ModelCreationAndBasicUse()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        solver.Assert(x + y == context.MkInt(10));
        solver.Assert(x > context.MkInt(0));
        solver.Assert(y > context.MkInt(0));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        Assert.That(model, Is.Not.Null);
        
        var modelString = model.ToString();
        Assert.That(modelString, Is.Not.Null.And.Not.Empty);
        Assert.That(modelString, Does.Not.Contain("<error>"));
        Assert.That(modelString, Does.Not.Contain("<invalid>"));
        Assert.That(modelString, Does.Not.Contain("<invalidated>"));
    }

    [Test]
    public void ModelInvalidatedAfterSolverDisposal()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(5));
        
        solver.Check();
        var model = solver.GetModel();
        
        // Model should work initially
        Assert.That(model.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.DoesNotThrow(() => _ = model.Handle);
        
        // Dispose solver
        solver.Dispose();
        
        // Model should be invalidated
        Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        Assert.Throws<ObjectDisposedException>(() => _ = model.Handle);
        
        // Clean up context
        context.Dispose();
    }

    [Test]
    public void ModelInvalidatedAfterContextDisposal()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(42));
        
        solver.Check();
        var model = solver.GetModel();
        
        // Model should work initially
        Assert.That(model.ToString(), Is.Not.Null.And.Not.Empty);
        
        // Dispose context (cascades to solver then model)
        context.Dispose();
        
        // Model should be invalidated
        Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        Assert.Throws<ObjectDisposedException>(() => _ = model.Handle);
    }

    [Test]
    public void ModelInvalidatedAfterNewAssertion()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x > context.MkInt(0));
        
        solver.Check();
        var firstModel = solver.GetModel();
        
        // First model should be valid
        Assert.That(firstModel.ToString(), Is.Not.Null.And.Not.Empty);
        
        // Add new assertion - should invalidate first model
        solver.Assert(x < context.MkInt(100));
        
        // First model should now be invalidated
        Assert.That(firstModel.ToString(), Is.EqualTo("<invalidated>"));
        Assert.Throws<ObjectDisposedException>(() => _ = firstModel.Handle);
        
        // Getting model again should return new instance
        solver.Check();
        var secondModel = solver.GetModel();
        
        Assert.That(secondModel, Is.Not.Null);
        Assert.That(ReferenceEquals(firstModel, secondModel), Is.False);
        Assert.That(secondModel.ToString(), Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void ModelInvalidatedAfterPushPop()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(10));
        
        solver.Check();
        var modelBeforePush = solver.GetModel();
        Assert.That(modelBeforePush.ToString(), Is.Not.Null.And.Not.Empty);
        
        // Push should invalidate model
        solver.Push();
        Assert.That(modelBeforePush.ToString(), Is.EqualTo("<invalidated>"));
        
        solver.Assert(x > context.MkInt(5));
        solver.Check();
        var modelAfterPush = solver.GetModel();
        Assert.That(modelAfterPush.ToString(), Is.Not.Null.And.Not.Empty);
        
        // Pop should invalidate model
        solver.Pop();
        Assert.That(modelAfterPush.ToString(), Is.EqualTo("<invalidated>"));
    }

    [Test]
    public void DisposalOrderDoesntMatter()
    {
        var context = new Z3Context();
        var solvers = new List<Z3Solver>();
        var models = new List<Z3Model>();
        
        // Create multiple solvers with models
        for (int i = 0; i < 5; i++)
        {
            var solver = context.MkSolver();
            solvers.Add(solver);
            
            var x = context.MkIntConst($"x{i}");
            solver.Assert(x == context.MkInt(i));
            solver.Check();
            
            var model = solver.GetModel();
            models.Add(model);
        }
        
        // All models should work initially
        foreach (var model in models)
        {
            Assert.That(model.ToString(), Is.Not.Null.And.Not.Empty);
        }
        
        // Dispose in different orders - context first (cascades to all)
        context.Dispose();
        
        // All models should be invalidated
        foreach (var model in models)
        {
            Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        }
        
        // Additional disposal calls should be safe
        Assert.DoesNotThrow(() =>
        {
            foreach (var solver in solvers)
            {
                solver.Dispose();
            }
        });
    }

    [Test]
    public void MultipleGetModelCallsReturnSameInstance()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(123));
        
        solver.Check();
        
        var model1 = solver.GetModel();
        var model2 = solver.GetModel();
        var model3 = solver.GetModel();
        
        // Should return same cached instance
        Assert.That(ReferenceEquals(model1, model2), Is.True);
        Assert.That(ReferenceEquals(model2, model3), Is.True);
        Assert.That(ReferenceEquals(model1, model3), Is.True);
    }

    [Test]
    public void GetModelThrowsWhenNotSatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        // Create unsatisfiable constraints
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(1));
        solver.Assert(x == context.MkInt(2)); // Contradiction
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
        
        var ex = Assert.Throws<InvalidOperationException>(() => solver.GetModel());
        Assert.That(ex.Message, Does.Contain("Cannot get model when solver status is Unsatisfiable"));
    }

    [Test]
    public void GetModelThrowsWhenUnknown()
    {
        using var context = new Z3Context();
        
        // Set very low timeout to force unknown result
        context.SetParameter("timeout", "1");
        
        using var solver = context.MkSolver();
        
        // Create a complex constraint that might timeout
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var z = context.MkIntConst("z");
        
        // Add complex constraints
        for (int i = 0; i < 50; i++)
        {
            var xi = context.MkIntConst($"x{i}");
            solver.Assert((x * y * z + xi) == context.MkInt(i * 17 + 13));
        }
        
        var result = solver.Check();
        
        if (result == Z3Status.Unknown)
        {
            var ex = Assert.Throws<InvalidOperationException>(() => solver.GetModel());
            Assert.That(ex.Message, Does.Contain("Cannot get model when solver status is Unknown"));
        }
        else
        {
            // If it didn't timeout, just verify it works normally
            if (result == Z3Status.Satisfiable)
            {
                Assert.DoesNotThrow(() => solver.GetModel());
            }
        }
    }

    [Test]
    public void GetModelThrowsWithoutCheck()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(5));
        
        // Don't call Check()
        var ex = Assert.Throws<InvalidOperationException>(() => solver.GetModel());
        Assert.That(ex.Message, Does.Contain("Must call Check() before GetModel()"));
    }

    [Test]
    public void ComplexDisposalWithManyModels()
    {
        var context = new Z3Context();
        var solversAndModels = new List<(Z3Solver solver, Z3Model model)>();
        
        // Create many solvers with models
        for (int i = 0; i < 20; i++)
        {
            var solver = context.MkSolver();
            var x = context.MkIntConst($"x{i}");
            var y = context.MkIntConst($"y{i}");
            
            solver.Assert(x + y == context.MkInt(i * 2));
            solver.Assert(x > context.MkInt(0));
            solver.Assert(y > context.MkInt(0));
            
            if (solver.Check() == Z3Status.Satisfiable)
            {
                var model = solver.GetModel();
                solversAndModels.Add((solver, model));
            }
        }
        
        // Verify all models work initially
        foreach (var (solver, model) in solversAndModels)
        {
            Assert.That(model.ToString(), Is.Not.Null.And.Not.Empty);
            Assert.DoesNotThrow(() => _ = model.Handle);
        }
        
        // Dispose some solvers randomly
        var random = new Random(42);
        var solversToDispose = solversAndModels
            .Take(10)
            .OrderBy(x => random.Next())
            .ToList();
            
        foreach (var (solver, model) in solversToDispose)
        {
            solver.Dispose();
        }
        
        // Verify models from disposed solvers are invalidated
        foreach (var (solver, model) in solversToDispose)
        {
            Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        }
        
        // Context disposal should clean up everything remaining
        Assert.DoesNotThrow(() => context.Dispose());
        
        // All models should now be invalidated
        foreach (var (solver, model) in solversAndModels)
        {
            Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        }
    }

    [Test]
    public void ModelHandleValidityAfterInvalidation()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(999));
        solver.Check();
        
        var model = solver.GetModel();
        
        // Handle should work initially
        Assert.DoesNotThrow(() => _ = model.Handle);
        
        // Invalidate by adding assertion
        solver.Assert(x > context.MkInt(0));
        
        // Handle should throw after invalidation
        var ex = Assert.Throws<ObjectDisposedException>(() => _ = model.Handle);
        Assert.That(ex.ObjectName, Is.EqualTo("Z3Model"));
        Assert.That(ex.Message, Does.Contain("invalidated due to solver state change"));
        
        context.Dispose();
    }

    [Test]
    public void ModelToStringNeverThrows()
    {
        var context = new Z3Context();
        var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(777));
        solver.Check();
        
        var model = solver.GetModel();
        
        // ToString should work normally
        Assert.DoesNotThrow(() => model.ToString());
        
        // ToString should still work after invalidation
        solver.Assert(x > context.MkInt(0));
        Assert.DoesNotThrow(() => model.ToString());
        Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
        
        // ToString should still work after context disposal
        context.Dispose();
        Assert.DoesNotThrow(() => model.ToString());
        Assert.That(model.ToString(), Is.EqualTo("<invalidated>"));
    }
}