using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

/// <summary>
/// Tests for real comparison operations with comprehensive syntax variant coverage.
/// </summary>
[TestFixture]
public class RealExprComparisonTests
{
    [TestCase(5.5, 10.5, true)]
    [TestCase(10.5, 5.5, false)]
    [TestCase(10.5, 10.5, false)]
    public void LessThan_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a < b;
        var resultViaDecimalLeft = aValue < b;
        var resultViaDecimalRight = a < bValue;
        var resultViaContext = context.Lt(a, b);
        var resultViaContextDecimalLeft = context.Lt(aValue, b);
        var resultViaContextDecimalRight = context.Lt(a, bValue);
        var resultViaFunc = a.Lt(b);
        var resultViaFuncDecimalRight = a.Lt(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncDecimalRight), Is.EqualTo(expected));
        });
    }

    [TestCase(5.5, 10.5, true)]
    [TestCase(10.5, 5.5, false)]
    [TestCase(10.5, 10.5, true)]
    public void LessOrEqual_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a <= b;
        var resultViaDecimalLeft = aValue <= b;
        var resultViaDecimalRight = a <= bValue;
        var resultViaContext = context.Le(a, b);
        var resultViaContextDecimalLeft = context.Le(aValue, b);
        var resultViaContextDecimalRight = context.Le(a, bValue);
        var resultViaFunc = a.Le(b);
        var resultViaFuncDecimalRight = a.Le(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncDecimalRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10.5, 5.5, true)]
    [TestCase(5.5, 10.5, false)]
    [TestCase(10.5, 10.5, false)]
    public void GreaterThan_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a > b;
        var resultViaDecimalLeft = aValue > b;
        var resultViaDecimalRight = a > bValue;
        var resultViaContext = context.Gt(a, b);
        var resultViaContextDecimalLeft = context.Gt(aValue, b);
        var resultViaContextDecimalRight = context.Gt(a, bValue);
        var resultViaFunc = a.Gt(b);
        var resultViaFuncDecimalRight = a.Gt(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncDecimalRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10.5, 5.5, true)]
    [TestCase(5.5, 10.5, false)]
    [TestCase(10.5, 10.5, true)]
    public void GreaterOrEqual_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a >= b;
        var resultViaDecimalLeft = aValue >= b;
        var resultViaDecimalRight = a >= bValue;
        var resultViaContext = context.Ge(a, b);
        var resultViaContextDecimalLeft = context.Ge(aValue, b);
        var resultViaContextDecimalRight = context.Ge(a, bValue);
        var resultViaFunc = a.Ge(b);
        var resultViaFuncDecimalRight = a.Ge(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncDecimalRight), Is.EqualTo(expected));
        });
    }

    [TestCase(10.5, 10.5, true)]
    [TestCase(5.5, 10.5, false)]
    [TestCase(42.5, 42.5, true)]
    public void Equals_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a == b;
        var resultViaDecimalLeft = aValue == b;
        var resultViaDecimalRight = a == bValue;
        var resultViaContext = context.Eq(a, b);
        var resultViaContextDecimalLeft = context.Eq(aValue, b);
        var resultViaContextDecimalRight = context.Eq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextDecimalRight), Is.EqualTo(expected));
        });
    }

    [TestCase(5.5, 10.5, true)]
    [TestCase(10.5, 10.5, false)]
    [TestCase(42.5, 43.5, true)]
    public void NotEquals_TwoValues_ComputesCorrectResult(double aVal, double bVal, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var aValue = (decimal)aVal;
        var bValue = (decimal)bVal;
        var a = context.Real(aValue);
        var b = context.Real(bValue);

        var result = a != b;
        var resultViaDecimalLeft = aValue != b;
        var resultViaDecimalRight = a != bValue;
        var resultViaContext = context.Neq(a, b);
        var resultViaContextIntLeft = context.Neq(aValue, b);
        var resultViaContextIntRight = context.Neq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaDecimalRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextIntRight), Is.EqualTo(expected));
        });
    }
}
