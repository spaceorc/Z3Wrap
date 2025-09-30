using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverTests
{
    [Test]
    public void CreateSolver_ReturnsValidHandle()
    {
        using var context = new Z3Context();

        using var solver = context.CreateSolver();

        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Check_WithoutConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Assert_SimpleConstraint_DoesNotThrow()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var trueExpr = context.True();

        Assert.DoesNotThrow(() => solver.Assert(trueExpr));
    }

    [Test]
    public void Reset_ClearsConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.Assert(context.False());

        solver.Reset();
        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Push_Pop_ManagesScopes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x > 0);

        solver.Push();
        solver.Assert(x < 0);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        solver.Pop();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GetModel_AfterSatisfiableCheck_ReturnsModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.Assert(context.True());

        solver.Check();
        var model = solver.GetModel();

        Assert.That(model, Is.Not.Null);
    }

    [Test]
    public void GetReasonUnknown_ReturnsNonNull()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        solver.Check();

        var reason = solver.GetReasonUnknown();

        Assert.That(reason, Is.Not.Null);
    }

    [Test]
    public void SetParams_ModelTrue_GeneratesModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var parameters = new Z3Params();

        parameters.Set("model", true);
        solver.SetParams(parameters);

        var x = context.IntConst("x");
        solver.Assert(x == 42);
        solver.Check();

        var model = solver.GetModel();
        var value = model.GetIntValue(x);

        Assert.That(value, Is.EqualTo(new BigInteger(42)));
        Assert.That(model.ToString(), Does.Contain("x"));
    }

    [Test]
    public void SetParams_ModelFalse_ReturnsDefaultValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var parameters = new Z3Params();

        parameters.Set("model", false);
        solver.SetParams(parameters);

        var x = context.IntConst("x");
        solver.Assert(x == 42);
        solver.Check();

        var model = solver.GetModel();

        // When model=false, Z3 returns a model with default values (not the actual solution)
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void SetParams_CanBeCalledMultipleTimes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 42);

        var params1 = new Z3Params();
        params1.Set("model", false);
        solver.SetParams(params1);

        solver.Check();
        var model1 = solver.GetModel();
        Assert.That(model1.ToString(), Is.Empty);

        // Change parameter and check again
        var params2 = new Z3Params();
        params2.Set("model", true);
        solver.SetParams(params2);

        solver.Check();
        var model2 = solver.GetModel();
        Assert.That(model2.GetIntValue(x), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void SetParams_AfterSolverDispose_ThrowsObjectDisposedException()
    {
        using var context = new Z3Context();
        var parameters = new Z3Params();
        var solver = context.CreateSolver();
        solver.Dispose();

        Assert.Throws<ObjectDisposedException>(() => solver.SetParams(parameters));
    }

    [Test]
    public void SetParams_MultipleParameterTypes_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var parameters = new Z3Params();

        parameters.Set("model", true).Set("timeout", 5000u);

        solver.SetParams(parameters);

        var x = context.IntConst("x");
        solver.Assert(x == 10);
        solver.Check();

        var model = solver.GetModel();
        var value = model.GetIntValue(x);

        // Verify model=true actually works (returns real value, not 0)
        Assert.That(value, Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void SetParams_BeforeAndAfterAssertions_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");

        var params1 = new Z3Params();
        params1.Set("model", true);
        solver.SetParams(params1);

        solver.Assert(x == 50);

        var params2 = new Z3Params();
        params2.Set("model", false);
        solver.SetParams(params2);

        solver.Assert(x < 100);
        solver.Check();

        var model = solver.GetModel();

        // Last parameter wins - model=false returns default value 0, not actual solution 50
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(0)));
    }
}
