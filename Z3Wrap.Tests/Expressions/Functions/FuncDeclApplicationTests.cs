using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Functions;

/// <summary>
/// Tests for function application and solving with uninterpreted functions.
/// Validates function application syntax, constraint satisfaction, and model extraction.
/// </summary>
[TestFixture]
public class FuncDeclApplicationTests
{
    [Test]
    public void ApplyFunc_WithNoArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr>("f");
        var result = func.Apply();

        solver.Assert(result == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void ApplyFunc_WithOneArg_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.Int(5);
        var result = func.Apply(x);

        solver.Assert(result == 10);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void ApplyFunc_WithTwoArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr, IntExpr>("f");
        var x = context.Int(3);
        var y = context.Int(7);
        var result = func.Apply(x, y);

        solver.Assert(result == 21);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(21)));
    }

    [Test]
    public void ApplyFunc_WithThreeArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr, IntExpr, IntExpr>("f");
        var x = context.Int(2);
        var y = context.Int(3);
        var z = context.Int(4);
        var result = func.Apply(x, y, z);

        solver.Assert(result == 24);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(24)));
    }

    [Test]
    public void ApplyFunc_IntToReal_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, RealExpr>("f");
        var x = context.Int(5);
        var result = func.Apply(x);

        solver.Assert(result == 2.5m);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result).ToDecimal(), Is.EqualTo(2.5m));
    }

    [Test]
    public void ApplyFunc_RealIntToBool_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<RealExpr, IntExpr, BoolExpr>("f");
        var x = context.Real(3.5m);
        var y = context.Int(10);
        var result = func.Apply(x, y);

        solver.Assert(result);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void ApplyFunc_WithConstraints_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.IntConst("x");

        // f(x) == 10 and x > 0
        solver.Assert(func.Apply(x) == 10);
        solver.Assert(x > 0);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(func.Apply(x)), Is.EqualTo(new BigInteger(10)));
        Assert.That(model.GetIntValue(x) > BigInteger.Zero, Is.True);
    }

    [Test]
    public void ApplyFunc_SameInputs_ProduceSameOutput()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.Int(5);

        // f(5) == f(5) should always be true (function consistency)
        var result1 = func.Apply(x);
        var result2 = func.Apply(x);
        solver.Assert(result1 == result2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result1), Is.EqualTo(model.GetIntValue(result2)));
    }

    [Test]
    public void ApplyFunc_DifferentInputs_CanProduceDifferentOutputs()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.Int(5);
        var y = context.Int(7);

        // f(5) != f(7) should be satisfiable (functions can map different inputs to different outputs)
        var result1 = func.Apply(x);
        var result2 = func.Apply(y);
        solver.Assert(result1 != result2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ApplyFunc_DifferentInputs_CanProduceSameOutputs()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.Int(5);
        var y = context.Int(7);

        // f(5) == f(7) should also be satisfiable (functions can map different inputs to same output)
        var result1 = func.Apply(x);
        var result2 = func.Apply(y);
        solver.Assert(result1 == result2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ApplyFunc_ViaContextApply_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr, IntExpr>("f");
        var x = context.Int(3);
        var y = context.Int(7);

        // Test context.Apply() method
        var result = context.Apply(func, [x, y]);

        solver.Assert(result == 21);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(21)));
    }

    [Test]
    public void ApplyFunc_WithVariable_AllowsMultipleSolutions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.IntConst("x");

        // f(x) > 10 should have many solutions
        solver.Assert(func.Apply(x) > 10);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var result = model.GetIntValue(func.Apply(x));
        Assert.That(result > BigInteger.Parse("10"), Is.True);
    }

    [Test]
    public void ApplyFunc_ComposedFunctions_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var f = context.Func<IntExpr, IntExpr>("f");
        var g = context.Func<IntExpr, IntExpr>("g");
        var x = context.Int(5);

        // g(f(x)) == 20
        var fx = f.Apply(x);
        var gfx = g.Apply(fx);
        solver.Assert(gfx == 20);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(gfx), Is.EqualTo(new BigInteger(20)));
    }

    [Test]
    public void ApplyFunc_ViaContextApplyWrongArgCount_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var func = context.Func<IntExpr, IntExpr, IntExpr>("f");
        var x = context.Int(5);

        // Try to apply 2-arg function with only 1 arg via context.Apply
        var ex = Assert.Throws<ArgumentException>(() => context.Apply(func, [x]));
        Assert.That(ex.Message, Does.Contain("arity"));
        Assert.That(ex.Message, Does.Contain("2"));
        Assert.That(ex.Message, Does.Contain("1"));
    }
}
