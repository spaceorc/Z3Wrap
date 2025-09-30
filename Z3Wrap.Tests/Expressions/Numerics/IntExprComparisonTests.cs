using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

[TestFixture]
public class IntExprComparisonTests
{
    [TestCase(5, 10, true)]
    [TestCase(10, 5, false)]
    [TestCase(10, 10, false)]
    public void LessThan_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a < b;
        var resultViaIntLeft = aValue < b;
        var resultViaIntRight = a < bValue;
        var resultViaContext = context.Lt(a, b);
        var resultViaContextIntLeft = context.Lt(aValue, b);
        var resultViaContextIntRight = context.Lt(a, bValue);
        var resultViaFunc = a.Lt(b);
        var resultViaFuncIntRight = a.Lt(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncIntRight), Is.EqualTo(expected));
        });
    }

    [TestCase(5, 10, true)]
    [TestCase(10, 5, false)]
    [TestCase(10, 10, true)]
    public void LessOrEqual_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a <= b;
        var resultViaIntLeft = aValue <= b;
        var resultViaIntRight = a <= bValue;
        var resultViaContext = context.Le(a, b);
        var resultViaContextIntLeft = context.Le(aValue, b);
        var resultViaContextIntRight = context.Le(a, bValue);
        var resultViaFunc = a.Le(b);
        var resultViaFuncIntRight = a.Le(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncIntRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10, 5, true)]
    [TestCase(5, 10, false)]
    [TestCase(10, 10, false)]
    public void GreaterThan_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a > b;
        var resultViaIntLeft = aValue > b;
        var resultViaIntRight = a > bValue;
        var resultViaContext = context.Gt(a, b);
        var resultViaContextIntLeft = context.Gt(aValue, b);
        var resultViaContextIntRight = context.Gt(a, bValue);
        var resultViaFunc = a.Gt(b);
        var resultViaFuncIntRight = a.Gt(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncIntRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10, 5, true)]
    [TestCase(5, 10, false)]
    [TestCase(10, 10, true)]
    public void GreaterOrEqual_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a >= b;
        var resultViaIntLeft = aValue >= b;
        var resultViaIntRight = a >= bValue;
        var resultViaContext = context.Ge(a, b);
        var resultViaContextIntLeft = context.Ge(aValue, b);
        var resultViaContextIntRight = context.Ge(a, bValue);
        var resultViaFunc = a.Ge(b);
        var resultViaFuncIntRight = a.Ge(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncIntRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10, 10, true)]
    [TestCase(5, 10, false)]
    [TestCase(42, 42, true)]
    public void Equals_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a == b;
        var resultViaIntLeft = aValue == b;
        var resultViaIntRight = a == bValue;
        var resultViaContext = context.Eq(a, b);
        var resultViaContextIntLeft = context.Eq(aValue, b);
        var resultViaContextIntRight = context.Eq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
        });
    }

    [TestCase(5, 10, true)]
    [TestCase(10, 10, false)]
    [TestCase(42, 43, true)]
    public void NotEquals_TwoValues_ComputesCorrectResult(int aValue, int bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var result = a != b;
        var resultViaIntLeft = aValue != b;
        var resultViaIntRight = a != bValue;
        var resultViaContext = context.Neq(a, b);
        var resultViaContextIntLeft = context.Neq(aValue, b);
        var resultViaContextIntRight = context.Neq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaIntRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
        });
    }
}
