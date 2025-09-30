using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprConversionTests
{
    [TestCase(TypeArgs = [typeof(Size32)])]
    [TestCase(TypeArgs = [typeof(Size64)])]
    public void ToInt_UnsignedConversion_ConvertsCorrectly<TSize>()
        where TSize : ISize
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<TSize>(42u);
        var intValue = bvValue.ToInt();
        var intValueViaContext = context.ToInt(bvValue);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(intValueViaContext), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void ToInt_SignedConversion_ConvertsNegativeValueCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // -42 in 32-bit two's complement
        var bvValue = context.BitVec<Size32>(unchecked((uint)-42));
        var intValue = bvValue.ToInt(true);
        var intValueViaContext = context.ToInt(bvValue, true);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(-42)));
            Assert.That(model.GetIntValue(intValueViaContext), Is.EqualTo(new BigInteger(-42)));
        });
    }

    [Test]
    public void ToInt_UnsignedConversion_ConvertsLargeValueCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // This would be negative if interpreted as signed
        var bvValue = context.BitVec<Size32>(0xFFFFFF00u);
        var intValue = bvValue.ToInt(false);

        solver.Check();
        var model = solver.GetModel();

        Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(0xFFFFFF00u)));
    }

    [Test]
    public void ToInt_WithSymbolicVariable_PreservesConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size32>("x");
        solver.Assert(x == 42u);

        var intX = x.ToInt();
        solver.Assert(intX > 40);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(x).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(intX), Is.EqualTo(new BigInteger(42)));
        });
    }

    [TestCase(TypeArgs = [typeof(Size8), typeof(Size16)])]
    [TestCase(TypeArgs = [typeof(Size16), typeof(Size32)])]
    [TestCase(TypeArgs = [typeof(Size32), typeof(Size64)])]
    public void Resize_ZeroExtend_WorksCorrectly<TInputSize, TOutputSize>()
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<TInputSize>(42u);
        var resizedValue = bvValue.Resize<TOutputSize>(false);
        var resizedValueViaContext = context.Resize<TInputSize, TOutputSize>(bvValue, false);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resizedValue).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(resizedValueViaContext).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Resize_SignExtend_PreservesNegativeValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // -42 in 8-bit two's complement (0xD6 = 214)
        var bvValue = context.BitVec<Size8>((uint)unchecked((byte)-42));
        var resizedValue = bvValue.Resize<Size32>(true);

        solver.Check();
        var model = solver.GetModel();

        // Should sign-extend to 32-bit: -42
        Assert.That(model.GetBitVec(resizedValue).Value, Is.EqualTo(new BigInteger(unchecked((uint)-42))));
    }

    [Test]
    public void Resize_ZeroExtend_DoesNotPreserveNegativeValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // -42 in 8-bit two's complement (0xD6 = 214)
        var bvValue = context.BitVec<Size8>((uint)unchecked((byte)-42));
        var resizedValue = bvValue.Resize<Size32>(false);

        solver.Check();
        var model = solver.GetModel();

        // Should zero-extend to 32-bit: 214
        Assert.That(model.GetBitVec(resizedValue).Value, Is.EqualTo(new BigInteger(214)));
    }

    [TestCase(TypeArgs = [typeof(Size32), typeof(Size16)])]
    [TestCase(TypeArgs = [typeof(Size64), typeof(Size32)])]
    public void Resize_Truncate_WorksCorrectly<TInputSize, TOutputSize>()
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<TInputSize>(0x12345678u);
        var truncatedValue = bvValue.Resize<TOutputSize>();

        solver.Check();
        var model = solver.GetModel();

        // Should truncate to lower bits
        var outputBits = TOutputSize.Size;
        var mask = (BigInteger.One << (int)outputBits) - 1;
        var expected = new BigInteger(0x12345678u) & mask;
        Assert.That(model.GetBitVec(truncatedValue).Value, Is.EqualTo(expected));
    }

    [TestCase(0u)]
    [TestCase(4u)]
    public void Extract_ExtractsCorrectBits(uint startBit)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<Size16>(0xABCDu); // 1010101111001101
        var extractedValue = bvValue.Extract<Size8>(startBit);
        var extractedValueViaContext = context.Extract<Size16, Size8>(bvValue, startBit);

        solver.Check();
        var model = solver.GetModel();

        // Extract bits [startBit+7:startBit]
        var expectedValue = (0xABCDu >> (int)startBit) & 0xFFu;
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(extractedValue).Value, Is.EqualTo(new BigInteger(expectedValue)));
            Assert.That(model.GetBitVec(extractedValueViaContext).Value, Is.EqualTo(new BigInteger(expectedValue)));
        });
    }

    [Test]
    public void Repeat_RepeatsPatternCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<Size8>(0xABu); // 10101011
        var repeatedValue = bvValue.Repeat<Size16>();
        var repeatedValueViaContext = context.Repeat<Size8, Size16>(bvValue);

        solver.Check();
        var model = solver.GetModel();

        // Should be 0xABAB
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(repeatedValue).Value, Is.EqualTo(new BigInteger(0xABABu)));
            Assert.That(model.GetBitVec(repeatedValueViaContext).Value, Is.EqualTo(new BigInteger(0xABABu)));
        });
    }

    [Test]
    public void Repeat_MultipleRepetitions_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = context.BitVec<Size8>(0x12u);
        var repeatedValue = bvValue.Repeat<Size32>();

        solver.Check();
        var model = solver.GetModel();

        // Should be 0x12121212
        Assert.That(model.GetBitVec(repeatedValue).Value, Is.EqualTo(new BigInteger(0x12121212u)));
    }

    [Test]
    public void ConversionChain_BitVecToIntToBitVec_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var originalBv = context.BitVec<Size32>(42u);
        var intValue = originalBv.ToInt();
        var convertedBackBv = intValue.ToBitVec<Size32>();

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(originalBv).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(convertedBackBv).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Resize_WithSymbolicVariable_PreservesConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        solver.Assert(x < 100u);

        var resizedX = x.Resize<Size32>();
        solver.Assert(resizedX == 42u);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(x).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBitVec(resizedX).Value, Is.EqualTo(new BigInteger(42)));
        });
    }
}
