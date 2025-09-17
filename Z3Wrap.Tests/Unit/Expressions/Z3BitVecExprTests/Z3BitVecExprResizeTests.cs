using Spaceorc.Z3Wrap;
#pragma warning disable NUnit2021 // Comparison of value types is allowed here for BitVec tests

using Spaceorc.Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprResizeTests
{
    [TestCase(255, 8u, 16u, 255, Description = "Zero-extend 8-bit to 16-bit")]
    [TestCase(255, 8u, 4u, 15, Description = "Truncate 8-bit to 4-bit")]
    [TestCase(127, 8u, 16u, 127, Description = "Zero-extend positive value")]
    public void UnsignedResize_AllVariations_ReturnsExpectedResult(int value, uint fromSize, uint toSize, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, fromSize);
        var result = x.Resize(toSize, signed: false);

        Assert.That(result.Size, Is.EqualTo(toSize));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, toSize)));
    }

    [TestCase(200, 8u, 16u, 65480, Description = "Sign-extend negative value (200 = -56 signed, extends to -56 = 65480 in 16-bit)")]
    [TestCase(128, 8u, 16u, 65408, Description = "Sign-extend -128 to 16-bit (-128 = 65408 in 16-bit)")]
    [TestCase(255, 8u, 4u, 15, Description = "Truncate -1 to 4-bit (-1 truncated = 15)")]
    [TestCase(100, 8u, 16u, 100, Description = "Sign-extend positive value (same as zero-extend)")]
    [TestCase(127, 8u, 4u, 15, Description = "Truncate positive value")]
    public void SignedResize_AllVariations_ReturnsExpectedResult(int value, uint fromSize, uint toSize, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, fromSize);
        var result = x.Resize(toSize, signed: true);

        Assert.That(result.Size, Is.EqualTo(toSize));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, toSize)));
    }

    [TestCase(255, 7u, 0u, 255, 8u, Description = "Extract all 8 bits")]
    [TestCase(255, 3u, 0u, 15, 4u, Description = "Extract lower 4 bits")]
    [TestCase(255, 7u, 4u, 15, 4u, Description = "Extract upper 4 bits")]
    [TestCase(170, 3u, 0u, 10, 4u, Description = "Extract lower bits from alternating pattern")]
    public void Extract_AllVariations_ReturnsExpectedResult(int value, uint high, uint low, int expectedResult, uint expectedToSize)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);
        var result = x.Extract(high, low);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, expectedToSize)));
    }

    [Test]
    public void Extend_ContextMethod_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(42, 8));
        var bv16 = context.Extend(bv8, 8); // Add 8 more bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(42, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extend_FluentAPI_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(170, 8)); // 0b10101010
        var bv16 = bv8.Extend(8); // Add 8 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(170, 16))); // Should be 0b00000000_10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedExtend_ContextMethod_PositiveValue()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(85, 8)); // 0b01010101 (positive)
        var bv16 = context.Extend(bv8, 8, signed: true);

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(85, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedExtend_FluentAPI_NegativeValue()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(170, 8)); // 0b10101010 (negative in signed interpretation)
        var bv16 = bv8.Extend(8, signed: true);

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        // In signed interpretation, 170 in 8-bit is -86, so sign extension should preserve this
        var expected = new BitVec(-86, 16); // Should be properly sign-extended
        solver.Assert(bv16 == context.BitVec(expected));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extract_ContextMethod_MiddleBits()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(0b11010011, 8)); // 211
        var extracted = context.Extract(bv8, 5, 2); // Extract bits 5-2

        Assert.That(extracted.Size, Is.EqualTo(4));

        using var solver = context.CreateSolver();
        solver.Assert(extracted == context.BitVec(new BitVec(0b0100, 4))); // Should be 4

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extract_FluentAPI_SingleBit()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(0b10101010, 8));
        var bit7 = bv8.Extract(7, 7); // MSB
        var bit0 = bv8.Extract(0, 0); // LSB

        Assert.That(bit7.Size, Is.EqualTo(1));
        Assert.That(bit0.Size, Is.EqualTo(1));

        using var solver = context.CreateSolver();
        solver.Assert(bit7 == context.BitVec(new BitVec(1, 1)));
        solver.Assert(bit0 == context.BitVec(new BitVec(0, 1)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Int2BitVec_ContextMethod_CreatesCorrectBitVec()
    {
        using var context = new Z3Context();
        var intExpr = context.IntConst("x");
        var bvExpr = context.ToBitVec(intExpr, 16);

        Assert.That(bvExpr.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(intExpr == context.Int(42));

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);
            var bvValue = model.GetBitVec(bvExpr);

            Assert.That((int)intValue, Is.EqualTo(42));
            Assert.That((int)bvValue.Value, Is.EqualTo(42));
            Assert.That(bvValue.Size, Is.EqualTo(16));
        }
    }

    [Test]
    public void BitVec2Int_ContextMethod_UnsignedConversion()
    {
        using var context = new Z3Context();
        var bvExpr = context.BitVec(new BitVec(200, 8));
        var intExpr = context.ToInt(bvExpr); // unsigned

        using var solver = context.CreateSolver();
        solver.Assert(intExpr >= context.Int(0));

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            Assert.That((int)intValue, Is.EqualTo(200));
        }
    }

    [Test]
    public void BitVec2Int_FluentAPI_SignedConversion()
    {
        using var context = new Z3Context();
        var bvExpr = context.BitVec(new BitVec(200, 8)); // 200 unsigned, -56 signed
        var intExpr = bvExpr.ToInt(signed: true);

        using var solver = context.CreateSolver();

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            // 200 in 8-bit unsigned is 11001000 binary, which in signed is -56
            Assert.That((int)intValue, Is.EqualTo(-56));
        }
    }

    [Test]
    public void ToInt_FluentAPI_ReturnsUnsignedValue()
    {
        using var context = new Z3Context();
        var bvExpr = context.BitVec(new BitVec(200, 8));
        var intExpr = bvExpr.ToInt();

        using var solver = context.CreateSolver();

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            Assert.That((int)intValue, Is.EqualTo(200));
        }
    }

    [Test]
    public void ChainedOperations_WorkCorrectly()
    {
        using var context = new Z3Context();
        var bv8 = context.BitVec(new BitVec(0b11110000, 8));
        var extracted = bv8.Extract(7, 4); // Get upper 4 bits -> 1111
        var extended = extracted.Extend(4); // Extend to 8 bits -> 00001111

        Assert.That(extracted.Size, Is.EqualTo(4));
        Assert.That(extended.Size, Is.EqualTo(8));

        using var solver = context.CreateSolver();
        solver.Assert(extended == context.BitVec(new BitVec(0b00001111, 8)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(0b1010, 4u, 2u, 0b10101010, 8u, Description = "Repeat 4-bit pattern twice")]
    [TestCase(0b11, 2u, 3u, 0b111111, 6u, Description = "Repeat 2-bit pattern three times")]
    [TestCase(0b1, 1u, 8u, 0b11111111, 8u, Description = "Repeat single bit eight times")]
    [TestCase(0b101, 3u, 1u, 0b101, 3u, Description = "Repeat once (no change)")]
    public void Repeat_AllVariations_ReturnsExpectedResult(int value, uint originalSize, uint repeatCount, int expectedResult, uint expectedSize)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, originalSize);
        var result = x.Repeat(repeatCount);

        Assert.That(result.Size, Is.EqualTo(expectedSize));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, expectedSize)));
    }

    [Test]
    public void Repeat_ContextMethod_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv4 = context.BitVec(new BitVec(0b1100, 4));
        var bv12 = context.Repeat(bv4, 3); // Repeat three times

        Assert.That(bv12.Size, Is.EqualTo(12));

        using var solver = context.CreateSolver();
        solver.Assert(bv12 == context.BitVec(new BitVec(0b110011001100, 12)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Repeat_FluentAPI_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv3 = context.BitVec(new BitVec(0b110, 3));
        var bv9 = bv3.Repeat(3); // Repeat three times

        Assert.That(bv9.Size, Is.EqualTo(9));

        using var solver = context.CreateSolver();
        solver.Assert(bv9 == context.BitVec(new BitVec(0b110110110, 9)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexResizingScenario_WithModel()
    {
        using var context = new Z3Context();
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 16);

        // Extend x to 16 bits and add to y
        var xExtended = x.Extend(8);
        var sum = context.BitVec(new BitVec(100, 16));

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(42, 8)));
        solver.Assert(y == context.BitVec(new BitVec(58, 16)));
        // Note: arithmetic operators for Z3BitVecExpr are not implemented yet
        // solver.Assert(xExtended + y == sum);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetBitVec(x);
        var yValue = model.GetBitVec(y);

        Assert.That((int)xValue.Value, Is.EqualTo(42));
        Assert.That((int)yValue.Value, Is.EqualTo(58));
    }
}