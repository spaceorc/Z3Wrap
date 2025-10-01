using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverParamsExtensionsTests
{
    [Test]
    public void SetParam_BooleanValue_SetsParameterCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 42);

        solver.SetParam("model", true);
        solver.Check();

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void SetParam_BooleanFalse_SetsParameterCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 42);

        solver.SetParam("model", false);
        solver.Check();

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void SetParam_UnsignedIntValue_SetsParameterCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetParam("timeout", 5000u);

        var x = context.IntConst("x");
        solver.Assert(x == 10);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetParam_DoubleValue_SetsParameterCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // qi.eager_threshold is a parameter that accepts double values
        solver.SetParam("qi.eager_threshold", 20.5);

        var x = context.IntConst("x");
        solver.Assert(x == 10);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetParam_StringValue_SetsParameterCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // smtlib2_log is a symbol parameter
        solver.SetParam("smtlib2_log", "test_log.smt2");

        var x = context.IntConst("x");
        solver.Assert(x == 10);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetParam_MultipleCallsWithDifferentTypes_AllWorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetParam("model", true);
        solver.SetParam("timeout", 10000u);
        solver.SetParam("qi.eager_threshold", 15.0);

        var x = context.IntConst("x");
        solver.Assert(x == 100);
        solver.Check();

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(100)));
    }

    [Test]
    public void SetParam_CalledBeforeAndAfterAssertions_ParametersApplyCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetParam("model", true);
        var x = context.IntConst("x");
        solver.Assert(x == 25);

        solver.SetParam("timeout", 5000u);
        solver.Check();

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(25)));
    }

    [Test]
    public void SetParam_OverwritePreviousParameter_LastValueWins()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 50);

        solver.SetParam("model", true);
        solver.SetParam("model", false);
        solver.Check();

        var model = solver.GetModel();
        // Last SetParam wins - model=false returns default value 0
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void SetTimeout_SetsTimeoutParameter()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetTimeout(TimeSpan.FromSeconds(5));

        var x = context.IntConst("x");
        solver.Assert(x == 10);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetTimeout_WithMilliseconds_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetTimeout(TimeSpan.FromMilliseconds(2500));

        var x = context.IntConst("x");
        solver.Assert(x == 20);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetTimeout_MultipleCallsWithDifferentValues_LastValueWins()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetTimeout(TimeSpan.FromSeconds(10));
        solver.SetTimeout(TimeSpan.FromSeconds(1));

        var x = context.IntConst("x");
        solver.Assert(x == 15);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SetTimeout_ZeroTimeout_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        solver.SetTimeout(TimeSpan.Zero);

        var x = context.IntConst("x");
        solver.Assert(x == 5);
        var status = solver.Check();

        // Zero timeout might return Unknown or Satisfiable depending on Z3's behavior
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable).Or.EqualTo(Z3Status.Unknown));
    }

    [Test]
    public void SetParam_AfterSolverDispose_ThrowsObjectDisposedException()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();
        solver.Dispose();

        Assert.Throws<ObjectDisposedException>(() => solver.SetParam("model", true));
    }

    [Test]
    public void SetTimeout_AfterSolverDispose_ThrowsObjectDisposedException()
    {
        using var context = new Z3Context();
        var solver = context.CreateSolver();
        solver.Dispose();

        Assert.Throws<ObjectDisposedException>(() => solver.SetTimeout(TimeSpan.FromSeconds(5)));
    }
}
