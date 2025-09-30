using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprArithmeticTests
{
    [Test]
    public void Add_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(10u);
        var b = context.BitVec<Size32>(32u);

        var sum = a + b;
        var sumViaUintLeft = 10u + b;
        var sumViaUintRight = a + 32u;
        var sumViaContext = context.Add(a, b);
        var sumViaContextUintLeft = context.Add<Size32>(10u, b);
        var sumViaContextUintRight = context.Add(a, 32u);
        var sumViaFunc = a.Add(b);
        var sumViaFuncUintRight = a.Add(32u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(sum).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaContext).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaContextUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaContextUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaFunc).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(sumViaFuncUintRight).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Subtract_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(50u);
        var b = context.BitVec<Size32>(8u);

        var difference = a - b;
        var differenceViaUintLeft = 50u - b;
        var differenceViaUintRight = a - 8u;
        var differenceViaContext = context.Sub(a, b);
        var differenceViaContextUintLeft = context.Sub<Size32>(50u, b);
        var differenceViaContextUintRight = context.Sub(a, 8u);
        var differenceViaFunc = a.Sub(b);
        var differenceViaFuncUintRight = a.Sub(8u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(difference).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaContext).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaContextUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaContextUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaFunc).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(differenceViaFuncUintRight).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Multiply_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(6u);
        var b = context.BitVec<Size32>(7u);

        var product = a * b;
        var productViaUintLeft = 6u * b;
        var productViaUintRight = a * 7u;
        var productViaContext = context.Mul(a, b);
        var productViaContextUintLeft = context.Mul<Size32>(6u, b);
        var productViaContextUintRight = context.Mul(a, 7u);
        var productViaFunc = a.Mul(b);
        var productViaFuncUintRight = a.Mul(7u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(product).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaContext).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaContextUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaContextUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaFunc).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(productViaFuncUintRight).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Divide_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(84u);
        var b = context.BitVec<Size32>(2u);

        var quotient = a / b;
        var quotientViaUintLeft = 84u / b;
        var quotientViaUintRight = a / 2u;
        var quotientViaContext = context.Div(a, b);
        var quotientViaContextUintLeft = context.Div<Size32>(84u, b);
        var quotientViaContextUintRight = context.Div(a, 2u);
        var quotientViaFunc = a.Div(b);
        var quotientViaFuncUintRight = a.Div(2u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(quotient).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaContext).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaContextUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaContextUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaFunc).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaFuncUintRight).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void SignedDivide_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(unchecked((uint)-84));
        var b = context.BitVec<Size32>(unchecked((uint)-2));

        var quotient = context.Div(a, b, signed: true);
        var quotientViaContextUintLeft = context.Div<Size32>(unchecked((uint)-84), b, signed: true);
        var quotientViaContextUintRight = context.Div(a, unchecked((uint)-2), signed: true);
        var quotientViaFunc = a.Div(b, signed: true);
        var quotientViaFuncUintRight = a.Div(unchecked((uint)-2), signed: true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(quotient).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaContextUintLeft).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaContextUintRight).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaFunc).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(quotientViaFuncUintRight).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Mod_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(47u);
        var b = context.BitVec<Size32>(5u);

        var remainder = a % b;
        var remainderViaUintLeft = 47u % b;
        var remainderViaUintRight = a % 5u;
        var remainderViaContext = context.Rem(a, b);
        var remainderViaContextUintLeft = context.Rem<Size32>(47u, b);
        var remainderViaContextUintRight = context.Rem(a, 5u);
        var remainderViaFunc = a.Rem(b);
        var remainderViaFuncUintRight = a.Rem(5u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(remainder).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaUintLeft).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaUintRight).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaContext).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaContextUintLeft).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaContextUintRight).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaFunc).Value, Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetBitVec(remainderViaFuncUintRight).Value, Is.EqualTo(new BigInteger(2)));
        });
    }

    [Test]
    public void SignedMod_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(unchecked((uint)-47));
        var b = context.BitVec<Size32>(5u);

        var remainder = context.SignedMod(a, b);
        var remainderViaContextUintLeft = context.SignedMod<Size32>(unchecked((uint)-47), b);
        var remainderViaContextUintRight = context.SignedMod(a, 5u);
        var remainderViaFunc = a.SignedMod(b);
        var remainderViaFuncUintRight = a.SignedMod(5u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Z3's signed modulo returns a non-negative result: -47 mod 5 = 3 (mathematical modulo)
        var expected = new BigInteger(3);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(remainder).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(remainderViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(remainderViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(remainderViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(remainderViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void UnaryMinus_SingleValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(42u);

        var negation = -a;
        var negationViaContext = context.Neg(a);
        var negationViaFunc = a.Neg();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(unchecked((uint)-42));
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(negation).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(negationViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(negationViaFunc).Value, Is.EqualTo(expected));
        });
    }
}
