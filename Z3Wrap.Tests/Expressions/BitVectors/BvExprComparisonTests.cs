using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprComparisonTests
{
    // LessThan unsigned tests - with operator overloads
    [TestCase(5u, 10u, true)] // unsigned: 5 < 10 = true
    [TestCase(10u, 5u, false)] // unsigned: 10 < 5 = false
    public void LessThan_UnsignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Lt(a, b);
        var resultViaContextUintLeft = context.Lt(aValue, b);
        var resultViaContextUintRight = context.Lt(a, bValue);
        var resultViaFunc = a.Lt(b);
        var resultViaFuncUintRight = a.Lt(bValue);
        var resultViaOperator = a < b;
        var resultViaOperatorUintLeft = aValue < b;
        var resultViaOperatorUintRight = a < bValue;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
        });
    }

    // LessThan signed tests - no operator overloads
    [TestCase(4294967291u, 10u, true)] // signed: -5 < 10 = true
    [TestCase(10u, 4294967291u, false)] // signed: 10 < -5 = false
    public void LessThan_SignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Lt(a, b, signed: true);
        var resultViaContextUintLeft = context.Lt(aValue, b, signed: true);
        var resultViaContextUintRight = context.Lt(a, bValue, signed: true);
        var resultViaFunc = a.Lt(b, signed: true);
        var resultViaFuncUintRight = a.Lt(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
        });
    }

    // LessOrEqual unsigned tests - with operator overloads
    [TestCase(5u, 10u, true)] // unsigned: 5 <= 10 = true
    [TestCase(10u, 5u, false)] // unsigned: 10 <= 5 = false
    public void LessOrEqual_UnsignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Le(a, b);
        var resultViaContextUintLeft = context.Le(aValue, b);
        var resultViaContextUintRight = context.Le(a, bValue);
        var resultViaFunc = a.Le(b);
        var resultViaFuncUintRight = a.Le(bValue);
        var resultViaOperator = a <= b;
        var resultViaOperatorUintLeft = aValue <= b;
        var resultViaOperatorUintRight = a <= bValue;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
        });
    }

    // LessOrEqual signed tests - no operator overloads
    [TestCase(4294967291u, 10u, true)] // signed: -5 <= 10 = true
    [TestCase(10u, 4294967291u, false)] // signed: 10 <= -5 = false
    public void LessOrEqual_SignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Le(a, b, signed: true);
        var resultViaContextUintLeft = context.Le(aValue, b, signed: true);
        var resultViaContextUintRight = context.Le(a, bValue, signed: true);
        var resultViaFunc = a.Le(b, signed: true);
        var resultViaFuncUintRight = a.Le(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
        });
    }

    // GreaterThan unsigned tests - with operator overloads
    [TestCase(10u, 5u, true)] // unsigned: 10 > 5 = true
    [TestCase(5u, 10u, false)] // unsigned: 5 > 10 = false
    public void GreaterThan_UnsignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Gt(a, b);
        var resultViaContextUintLeft = context.Gt(aValue, b);
        var resultViaContextUintRight = context.Gt(a, bValue);
        var resultViaFunc = a.Gt(b);
        var resultViaFuncUintRight = a.Gt(bValue);
        var resultViaOperator = a > b;
        var resultViaOperatorUintLeft = aValue > b;
        var resultViaOperatorUintRight = a > bValue;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
        });
    }

    // GreaterThan signed tests - no operator overloads
    [TestCase(10u, 4294967291u, true)] // signed: 10 > -5 = true
    [TestCase(4294967291u, 10u, false)] // signed: -5 > 10 = false
    public void GreaterThan_SignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Gt(a, b, signed: true);
        var resultViaContextUintLeft = context.Gt(aValue, b, signed: true);
        var resultViaContextUintRight = context.Gt(a, bValue, signed: true);
        var resultViaFunc = a.Gt(b, signed: true);
        var resultViaFuncUintRight = a.Gt(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
        });
    }

    // GreaterOrEqual unsigned tests - with operator overloads
    [TestCase(10u, 5u, true)] // unsigned: 10 >= 5 = true
    [TestCase(5u, 10u, false)] // unsigned: 5 >= 10 = false
    public void GreaterOrEqual_UnsignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Ge(a, b);
        var resultViaContextUintLeft = context.Ge(aValue, b);
        var resultViaContextUintRight = context.Ge(a, bValue);
        var resultViaFunc = a.Ge(b);
        var resultViaFuncUintRight = a.Ge(bValue);
        var resultViaOperator = a >= b;
        var resultViaOperatorUintLeft = aValue >= b;
        var resultViaOperatorUintRight = a >= bValue;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
        });
    }

    // GreaterOrEqual signed tests - no operator overloads
    [TestCase(10u, 4294967291u, true)] // signed: 10 >= -5 = true
    [TestCase(4294967291u, 10u, false)] // signed: -5 >= 10 = false
    public void GreaterOrEqual_SignedComparison_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaContext = context.Ge(a, b, signed: true);
        var resultViaContextUintLeft = context.Ge(aValue, b, signed: true);
        var resultViaContextUintRight = context.Ge(a, bValue, signed: true);
        var resultViaFunc = a.Ge(b, signed: true);
        var resultViaFuncUintRight = a.Ge(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
        });
    }

    // Equals tests - signedness doesn't matter for equality
    [TestCase(10u, 10u, true)]
    [TestCase(5u, 10u, false)]
    public void Equals_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaOperator = a == b;
        var resultViaOperatorUintLeft = aValue == b;
        var resultViaOperatorUintRight = a == bValue;
        var resultViaFunc = a.Eq(b);
        var resultViaFuncUintRight = a.Eq(bValue);
        var resultViaContext = context.Eq(a, b);
        var resultViaContextUintLeft = context.Eq(aValue, b);
        var resultViaContextUintRight = context.Eq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
        });
    }

    // NotEquals tests - signedness doesn't matter for inequality
    [TestCase(5u, 10u, true)]
    [TestCase(10u, 10u, false)]
    public void NotEquals_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(aValue);
        var b = context.BitVec<Size32>(bValue);

        var resultViaOperator = a != b;
        var resultViaOperatorUintLeft = aValue != b;
        var resultViaOperatorUintRight = a != bValue;
        var resultViaFunc = a.Neq(b);
        var resultViaFuncUintRight = a.Neq(bValue);
        var resultViaContext = context.Neq(a, b);
        var resultViaContextUintLeft = context.Neq(aValue, b);
        var resultViaContextUintRight = context.Neq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaOperator), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaOperatorUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.EqualTo(expected));
        });
    }
}
