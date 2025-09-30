using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverParametersTests
{
    [Test]
    public void SetParams_AppliesParametersToSolver()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var parameters = new Z3Params();

        parameters.Set("timeout", 1000u);
        solver.SetParams(parameters);

        var x = context.IntConst("x");
        solver.Assert(x == 42);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetParams_CanBeCalledMultipleTimes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var params1 = new Z3Params();
        params1.Set("timeout", 1000u);
        solver.SetParams(params1);

        var params2 = new Z3Params();
        params2.Set("timeout", 2000u);
        solver.SetParams(params2);

        var x = context.IntConst("x");
        solver.Assert(x == 42);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
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

        parameters.Set("timeout", 5000u).Set("max_memory", 1024u);

        solver.SetParams(parameters);

        var x = context.IntConst("x");
        solver.Assert(x > 0);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetParams_BeforeAndAfterAssertions_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");

        var params1 = new Z3Params();
        params1.Set("timeout", 1000u);
        solver.SetParams(params1);

        solver.Assert(x > 0);

        var params2 = new Z3Params();
        params2.Set("timeout", 2000u);
        solver.SetParams(params2);

        solver.Assert(x < 100);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
