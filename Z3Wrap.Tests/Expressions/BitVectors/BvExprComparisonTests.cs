using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

/// <summary>
/// Tests for bitvector comparison operations with comprehensive syntax variant coverage.
/// </summary>
[TestFixture]
public class BvExprComparisonTests
{
    [Test]
    public void UnsignedLessThan_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a < b;
        var resultViaUintLeft = 5u < b;
        var resultViaUintRight = a < 10u;
        var resultViaContext = context.Lt(a, b, signed: false);
        var resultViaContextUintLeft = context.Lt(5u, b, signed: false);
        var resultViaContextUintRight = context.Lt(a, 10u, signed: false);
        var resultViaFunc = a.Lt(b, signed: false);
        var resultViaFuncUintRight = a.Lt(10u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void UnsignedLessThan_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(5u);

        var result = a < b;
        var resultViaUintLeft = 10u < b;
        var resultViaUintRight = a < 5u;
        var resultViaContext = context.Lt(a, b, signed: false);
        var resultViaContextUintLeft = context.Lt(10u, b, signed: false);
        var resultViaContextUintRight = context.Lt(a, 5u, signed: false);
        var resultViaFunc = a.Lt(b, signed: false);
        var resultViaFuncUintRight = a.Lt(5u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void SignedLessThan_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 4294967291 = -5 in signed 32-bit, 10 = 10 in signed 32-bit, -5 < 10 = true
        var a = context.BitVec<Size32>(4294967291u);
        var b = context.BitVec<Size32>(10u);

        var resultViaContext = context.Lt(a, b, signed: true);
        var resultViaContextUintLeft = context.Lt(4294967291u, b, signed: true);
        var resultViaContextUintRight = context.Lt(a, 10u, signed: true);
        var resultViaFunc = a.Lt(b, signed: true);
        var resultViaFuncUintRight = a.Lt(10u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void SignedLessThan_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 10 = 10 in signed 32-bit, 4294967291 = -5 in signed 32-bit, 10 < -5 = false
        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(4294967291u);

        var resultViaContext = context.Lt(a, b, signed: true);
        var resultViaContextUintLeft = context.Lt(10u, b, signed: true);
        var resultViaContextUintRight = context.Lt(a, 4294967291u, signed: true);
        var resultViaFunc = a.Lt(b, signed: true);
        var resultViaFuncUintRight = a.Lt(4294967291u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void UnsignedLessOrEqual_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a <= b;
        var resultViaUintLeft = 5u <= b;
        var resultViaUintRight = a <= 10u;
        var resultViaContext = context.Le(a, b, signed: false);
        var resultViaContextUintLeft = context.Le(5u, b, signed: false);
        var resultViaContextUintRight = context.Le(a, 10u, signed: false);
        var resultViaFunc = a.Le(b, signed: false);
        var resultViaFuncUintRight = a.Le(10u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void UnsignedLessOrEqual_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(5u);

        var result = a <= b;
        var resultViaUintLeft = 10u <= b;
        var resultViaUintRight = a <= 5u;
        var resultViaContext = context.Le(a, b, signed: false);
        var resultViaContextUintLeft = context.Le(10u, b, signed: false);
        var resultViaContextUintRight = context.Le(a, 5u, signed: false);
        var resultViaFunc = a.Le(b, signed: false);
        var resultViaFuncUintRight = a.Le(5u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void SignedLessOrEqual_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 4294967291 = -5 in signed 32-bit, 10 = 10 in signed 32-bit, -5 <= 10 = true
        var a = context.BitVec<Size32>(4294967291u);
        var b = context.BitVec<Size32>(10u);

        var resultViaContext = context.Le(a, b, signed: true);
        var resultViaContextUintLeft = context.Le(4294967291u, b, signed: true);
        var resultViaContextUintRight = context.Le(a, 10u, signed: true);
        var resultViaFunc = a.Le(b, signed: true);
        var resultViaFuncUintRight = a.Le(10u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void SignedLessOrEqual_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 10 = 10 in signed 32-bit, 4294967291 = -5 in signed 32-bit, 10 <= -5 = false
        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(4294967291u);

        var resultViaContext = context.Le(a, b, signed: true);
        var resultViaContextUintLeft = context.Le(10u, b, signed: true);
        var resultViaContextUintRight = context.Le(a, 4294967291u, signed: true);
        var resultViaFunc = a.Le(b, signed: true);
        var resultViaFuncUintRight = a.Le(4294967291u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void UnsignedGreaterThan_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(5u);

        var result = a > b;
        var resultViaUintLeft = 10u > b;
        var resultViaUintRight = a > 5u;
        var resultViaContext = context.Gt(a, b, signed: false);
        var resultViaContextUintLeft = context.Gt(10u, b, signed: false);
        var resultViaContextUintRight = context.Gt(a, 5u, signed: false);
        var resultViaFunc = a.Gt(b, signed: false);
        var resultViaFuncUintRight = a.Gt(5u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void UnsignedGreaterThan_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a > b;
        var resultViaUintLeft = 5u > b;
        var resultViaUintRight = a > 10u;
        var resultViaContext = context.Gt(a, b, signed: false);
        var resultViaContextUintLeft = context.Gt(5u, b, signed: false);
        var resultViaContextUintRight = context.Gt(a, 10u, signed: false);
        var resultViaFunc = a.Gt(b, signed: false);
        var resultViaFuncUintRight = a.Gt(10u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void SignedGreaterThan_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 10 = 10 in signed 32-bit, 4294967291 = -5 in signed 32-bit, 10 > -5 = true
        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(4294967291u);

        var resultViaContext = context.Gt(a, b, signed: true);
        var resultViaContextUintLeft = context.Gt(10u, b, signed: true);
        var resultViaContextUintRight = context.Gt(a, 4294967291u, signed: true);
        var resultViaFunc = a.Gt(b, signed: true);
        var resultViaFuncUintRight = a.Gt(4294967291u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void SignedGreaterThan_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 4294967291 = -5 in signed 32-bit, 10 = 10 in signed 32-bit, -5 > 10 = false
        var a = context.BitVec<Size32>(4294967291u);
        var b = context.BitVec<Size32>(10u);

        var resultViaContext = context.Gt(a, b, signed: true);
        var resultViaContextUintLeft = context.Gt(4294967291u, b, signed: true);
        var resultViaContextUintRight = context.Gt(a, 10u, signed: true);
        var resultViaFunc = a.Gt(b, signed: true);
        var resultViaFuncUintRight = a.Gt(10u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void UnsignedGreaterOrEqual_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(5u);

        var result = a >= b;
        var resultViaUintLeft = 10u >= b;
        var resultViaUintRight = a >= 5u;
        var resultViaContext = context.Ge(a, b, signed: false);
        var resultViaContextUintLeft = context.Ge(10u, b, signed: false);
        var resultViaContextUintRight = context.Ge(a, 5u, signed: false);
        var resultViaFunc = a.Ge(b, signed: false);
        var resultViaFuncUintRight = a.Ge(5u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void UnsignedGreaterOrEqual_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a >= b;
        var resultViaUintLeft = 5u >= b;
        var resultViaUintRight = a >= 10u;
        var resultViaContext = context.Ge(a, b, signed: false);
        var resultViaContextUintLeft = context.Ge(5u, b, signed: false);
        var resultViaContextUintRight = context.Ge(a, 10u, signed: false);
        var resultViaFunc = a.Ge(b, signed: false);
        var resultViaFuncUintRight = a.Ge(10u, signed: false);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void SignedGreaterOrEqual_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 10 = 10 in signed 32-bit, 4294967291 = -5 in signed 32-bit, 10 >= -5 = true
        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(4294967291u);

        var resultViaContext = context.Ge(a, b, signed: true);
        var resultViaContextUintLeft = context.Ge(10u, b, signed: true);
        var resultViaContextUintRight = context.Ge(a, 4294967291u, signed: true);
        var resultViaFunc = a.Ge(b, signed: true);
        var resultViaFuncUintRight = a.Ge(4294967291u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.True);
        });
    }

    [Test]
    public void SignedGreaterOrEqual_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 4294967291 = -5 in signed 32-bit, 10 = 10 in signed 32-bit, -5 >= 10 = false
        var a = context.BitVec<Size32>(4294967291u);
        var b = context.BitVec<Size32>(10u);

        var resultViaContext = context.Ge(a, b, signed: true);
        var resultViaContextUintLeft = context.Ge(4294967291u, b, signed: true);
        var resultViaContextUintRight = context.Ge(a, 10u, signed: true);
        var resultViaFunc = a.Ge(b, signed: true);
        var resultViaFuncUintRight = a.Ge(10u, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
            Assert.That(model.GetBoolValue(resultViaFuncUintRight), Is.False);
        });
    }

    [Test]
    public void Equals_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(10u);

        var result = a == b;
        var resultViaUintLeft = 10u == b;
        var resultViaUintRight = a == 10u;
        var resultViaContext = context.Eq(a, b);
        var resultViaContextUintLeft = context.Eq(10u, b);
        var resultViaContextUintRight = context.Eq(a, 10u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.True);
        });
    }

    [Test]
    public void Equals_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a == b;
        var resultViaUintLeft = 5u == b;
        var resultViaUintRight = a == 10u;
        var resultViaContext = context.Eq(a, b);
        var resultViaContextUintLeft = context.Eq(5u, b);
        var resultViaContextUintRight = context.Eq(a, 10u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaContextUintRight), Is.False);
        });
    }

    [Test]
    public void NotEquals_TwoValues_ComputesCorrectResult_WhenTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(10u);

        var result = a != b;
        var resultViaUintLeft = 5u != b;
        var resultViaUintRight = a != 10u;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.True);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.True);
        });
    }

    [Test]
    public void NotEquals_TwoValues_ComputesCorrectResult_WhenFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(10u);

        var result = a != b;
        var resultViaUintLeft = 10u != b;
        var resultViaUintRight = a != 10u;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintLeft), Is.False);
            Assert.That(model.GetBoolValue(resultViaUintRight), Is.False);
        });
    }
}
