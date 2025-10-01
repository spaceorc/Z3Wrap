using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprArithmeticTests
{
    [TestCase(10u, 32u, 42u)]
    [TestCase(5u, 7u, 12u)]
    public void Add_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaOperator = a + b;
        var resultViaOperatorUintLeft = aValue + b;
        var resultViaOperatorUintRight = a + bValue;
        var resultViaContext = context.Add(a, b);
        var resultViaContextUintLeft = context.Add(aValue, b);
        var resultViaContextUintRight = context.Add(a, bValue);
        var resultViaFunc = a.Add(b);
        var resultViaFuncUintRight = a.Add(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(50u, 8u, 42u)]
    [TestCase(20u, 8u, 12u)]
    public void Subtract_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaOperator = a - b;
        var resultViaOperatorUintLeft = aValue - b;
        var resultViaOperatorUintRight = a - bValue;
        var resultViaContext = context.Sub(a, b);
        var resultViaContextUintLeft = context.Sub(aValue, b);
        var resultViaContextUintRight = context.Sub(a, bValue);
        var resultViaFunc = a.Sub(b);
        var resultViaFuncUintRight = a.Sub(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(6u, 7u, 42u)]
    [TestCase(3u, 4u, 12u)]
    public void Multiply_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaOperator = a * b;
        var resultViaOperatorUintLeft = aValue * b;
        var resultViaOperatorUintRight = a * bValue;
        var resultViaContext = context.Mul(a, b);
        var resultViaContextUintLeft = context.Mul(aValue, b);
        var resultViaContextUintRight = context.Mul(a, bValue);
        var resultViaFunc = a.Mul(b);
        var resultViaFuncUintRight = a.Mul(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(84u, 2u, 42u)]
    [TestCase(48u, 4u, 12u)]
    public void Divide_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaOperator = a / b;
        var resultViaOperatorUintLeft = aValue / b;
        var resultViaOperatorUintRight = a / bValue;
        var resultViaContext = context.Div(a, b);
        var resultViaContextUintLeft = context.Div(aValue, b);
        var resultViaContextUintRight = context.Div(a, bValue);
        var resultViaFunc = a.Div(b);
        var resultViaFuncUintRight = a.Div(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(unchecked((uint)-84), unchecked((uint)-2), 42u)] // -84 / -2 = 42
    [TestCase(unchecked((uint)-48), unchecked((uint)-4), 12u)] // -48 / -4 = 12
    public void SignedDivide_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaContext = context.Div(a, b, signed: true);
        var resultViaContextUintLeft = context.Div(aValue, b, signed: true);
        var resultViaContextUintRight = context.Div(a, bValue, signed: true);
        var resultViaFunc = a.Div(b, signed: true);
        var resultViaFuncUintRight = a.Div(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(47u, 5u, 2u)] // 47 % 5 = 2
    [TestCase(50u, 8u, 2u)] // 50 % 8 = 2
    public void Rem_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaOperator = a % b;
        var resultViaOperatorUintLeft = aValue % b;
        var resultViaOperatorUintRight = a % bValue;
        var resultViaContext = context.Rem(a, b);
        var resultViaContextUintLeft = context.Rem(aValue, b);
        var resultViaContextUintRight = context.Rem(a, bValue);
        var resultViaFunc = a.Rem(b);
        var resultViaFuncUintRight = a.Rem(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaOperatorUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(unchecked((uint)-47), 5u, unchecked((uint)-2))] // -47 % 5 = -2 (signed remainder)
    [TestCase(unchecked((uint)-50), 8u, unchecked((uint)-2))] // -50 % 8 = -2 (signed remainder)
    public void SignedRem_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaContext = context.Rem(a, b, signed: true);
        var resultViaContextUintLeft = context.Rem(aValue, b, signed: true);
        var resultViaContextUintRight = context.Rem(a, bValue, signed: true);
        var resultViaFunc = a.Rem(b, signed: true);
        var resultViaFuncUintRight = a.Rem(bValue, signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(unchecked((uint)-47), 5u, 3u)] // -47 mod 5 = 3 (mathematical modulo, always non-negative)
    [TestCase(unchecked((uint)-50), 8u, 6u)] // -50 mod 8 = 6 (mathematical modulo, always non-negative)
    public void SignedMod_TwoValues_ComputesCorrectResult(uint aValue, uint bValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);
        var b = context.Bv<Size32>(bValue);

        var resultViaContext = context.SignedMod(a, b);
        var resultViaContextUintLeft = context.SignedMod(aValue, b);
        var resultViaContextUintRight = context.SignedMod(a, bValue);
        var resultViaFunc = a.SignedMod(b);
        var resultViaFuncUintRight = a.SignedMod(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expectedValue));
        });
    }

    [TestCase(42u, unchecked((uint)-42))]
    [TestCase(12u, unchecked((uint)-12))]
    public void UnaryMinus_SingleValue_ComputesCorrectResult(uint aValue, uint expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(aValue);

        var resultViaOperator = -a;
        var resultViaContext = context.Neg(a);
        var resultViaFunc = a.Neg();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expectedValue = new BigInteger(expected);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaOperator).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expectedValue));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expectedValue));
        });
    }
}
