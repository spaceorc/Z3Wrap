using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BitVecTheory;

#pragma warning disable NUnit2021 // Comparison of value types is allowed here for BitVec tests

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprResizeTests
{
    /// <summary>
    /// Represents a 1-bit bitvector size.
    /// </summary>
    public readonly struct Size1 : ISize
    {
        /// <summary>
        /// Gets the bit width (1 bit).
        /// </summary>
        public static uint Size => 1;
    }

    /// <summary>
    /// Represents a 2-bit bitvector size.
    /// </summary>
    public readonly struct Size2 : ISize
    {
        /// <summary>
        /// Gets the bit width (2 bits).
        /// </summary>
        public static uint Size => 2;
    }

    /// <summary>
    /// Represents a 3-bit bitvector size.
    /// </summary>
    public readonly struct Size3 : ISize
    {
        /// <summary>
        /// Gets the bit width (3 bits).
        /// </summary>
        public static uint Size => 3;
    }

    /// <summary>
    /// Represents a 4-bit bitvector size.
    /// </summary>
    public readonly struct Size4 : ISize
    {
        /// <summary>
        /// Gets the bit width (4 bits).
        /// </summary>
        public static uint Size => 4;
    }

    /// <summary>
    /// Represents a 6-bit bitvector size.
    /// </summary>
    public readonly struct Size6 : ISize
    {
        /// <summary>
        /// Gets the bit width (6 bits).
        /// </summary>
        public static uint Size => 6;
    }

    /// <summary>
    /// Represents a 9-bit bitvector size.
    /// </summary>
    public readonly struct Size9 : ISize
    {
        /// <summary>
        /// Gets the bit width (9 bits).
        /// </summary>
        public static uint Size => 9;
    }

    /// <summary>
    /// Represents a 12-bit bitvector size.
    /// </summary>
    public readonly struct Size12 : ISize
    {
        /// <summary>
        /// Gets the bit width (12 bits).
        /// </summary>
        public static uint Size => 12;
    }

    [TestCase(255, 255, Description = "Zero-extend 8-bit to 16-bit")]
    [TestCase(127, 127, Description = "Zero-extend positive value")]
    [TestCase(200, 200, Description = "Zero-extend large value")]
    public void UnsignedResize_From8To16_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        var result = x.Resize<Size16>(signed: false);

        solver.Assert(x == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var resultValue = model.GetBitVec(result);
        Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [TestCase(255, 15, Description = "Truncate 8-bit to 4-bit")]
    [TestCase(170, 10, Description = "Truncate alternating pattern")]
    [TestCase(127, 15, Description = "Truncate positive value")]
    public void UnsignedResize_From8To4_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        var result = x.Resize<Size4>(signed: false);

        solver.Assert(x == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var resultValue = model.GetBitVec(result);
        Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
        Assert.That(Size4.Size, Is.EqualTo(4U));
    }

    [TestCase(
        200,
        65480,
        Description = "Sign-extend negative value (200 = -56 signed, extends to -56 = 65480 in 16-bit)"
    )]
    [TestCase(128, 65408, Description = "Sign-extend -128 to 16-bit (-128 = 65408 in 16-bit)")]
    [TestCase(100, 100, Description = "Sign-extend positive value (same as zero-extend)")]
    public void SignedResize_From8To16_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        var result = x.Resize<Size16>(signed: true);

        solver.Assert(x == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var resultValue = model.GetBitVec(result);
        Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [TestCase(255, 15, Description = "Truncate -1 to 4-bit (-1 truncated = 15)")]
    [TestCase(127, 15, Description = "Truncate positive value")]
    public void SignedResize_From8To4_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        var result = x.Resize<Size4>(signed: true);

        solver.Assert(x == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var resultValue = model.GetBitVec(result);
        Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
        Assert.That(Size4.Size, Is.EqualTo(4U));
    }

    [TestCase(255, 0U, 15, Description = "Extract lower 4 bits")]
    [TestCase(255, 4U, 15, Description = "Extract upper 4 bits")]
    [TestCase(170, 0U, 10, Description = "Extract lower bits from alternating pattern")]
    public void Extract_AllVariations_ReturnsExpectedResult(
        int value,
        uint startBit,
        int expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        solver.Assert(x == value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Test different extraction sizes based on startBit
        if (startBit == 0)
        {
            var result4 = x.Extract<Size4>(startBit);
            var resultValue4 = model.GetBitVec(result4);
            Assert.That((int)resultValue4.Value, Is.EqualTo(expectedResult));
            Assert.That(Size4.Size, Is.EqualTo(4U));
        }
        else
        {
            var result4 = x.Extract<Size4>(startBit);
            var resultValue4 = model.GetBitVec(result4);
            Assert.That((int)resultValue4.Value, Is.EqualTo(expectedResult));
            Assert.That(Size4.Size, Is.EqualTo(4U));
        }
    }

    [Test]
    public void Resize_ContextMethod_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bv16 = context.Resize<Size8, Size16>(bv8); // Extend from 8 to 16 bits

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 42);
        solver.Assert(bv16 == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv8Value = model.GetBitVec(bv8);
        var bv16Value = model.GetBitVec(bv16);

        Assert.That((int)bv8Value.Value, Is.EqualTo(42));
        Assert.That((int)bv16Value.Value, Is.EqualTo(42));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [Test]
    public void Resize_FluentAPI_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bv16 = bv8.Resize<Size16>(); // Extend from 8 to 16 bits

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 170); // 0b10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv8Value = model.GetBitVec(bv8);
        var bv16Value = model.GetBitVec(bv16);

        Assert.That((int)bv8Value.Value, Is.EqualTo(170));
        Assert.That((int)bv16Value.Value, Is.EqualTo(170)); // Should be 0b00000000_10101010
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [Test]
    public void SignedResize_ContextMethod_PositiveValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bv16 = context.Resize<Size8, Size16>(bv8, signed: true);

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 85); // 0b01010101 (positive)
        solver.Assert(bv16 == 85);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv16Value = model.GetBitVec(bv16);
        Assert.That((int)bv16Value.Value, Is.EqualTo(85));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [Test]
    public void SignedResize_FluentAPI_NegativeValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bv16 = bv8.Resize<Size16>(signed: true);

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 170); // 0b10101010 (negative in signed interpretation)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv16Value = model.GetBitVec(bv16);

        // In signed interpretation, 170 in 8-bit is -86, so sign extension should preserve this
        // -86 in 16-bit is 65536 - 86 = 65450
        Assert.That((int)bv16Value.Value, Is.EqualTo(65450));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }

    [Test]
    public void Extract_ContextMethod_MiddleBits()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var extracted = context.Extract<Size8, Size4>(bv8, 2); // Extract 4 bits starting from bit 2

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 0b11010011); // 211

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var extractedValue = model.GetBitVec(extracted);

        Assert.That(Size4.Size, Is.EqualTo(4U));
        // Bits [5:2] of 0b11010011 are 0b0100 = 4
        Assert.That((int)extractedValue.Value, Is.EqualTo(4));
    }

    [Test]
    public void Extract_FluentAPI_SingleBit()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bit7 = context.Extract<Size8, Size1>(bv8, 7); // MSB
        var bit0 = context.Extract<Size8, Size1>(bv8, 0); // LSB

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 0b10101010);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bit7Value = model.GetBitVec(bit7);
        var bit0Value = model.GetBitVec(bit0);

        Assert.That(Size1.Size, Is.EqualTo(1U));
        Assert.That(Size1.Size, Is.EqualTo(1U));
        Assert.That((int)bit7Value.Value, Is.EqualTo(1));
        Assert.That((int)bit0Value.Value, Is.EqualTo(0));
    }

    [Test]
    public void BitVec2Int_ContextMethod_UnsignedConversion()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bvExpr = context.BitVecConst<Size8>("bv");
        var intExpr = context.ToInt(bvExpr); // unsigned

        using var solver = context.CreateSolver();
        solver.Assert(bvExpr == 200);
        solver.Assert(intExpr >= 0);

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
        using var scope = context.SetUp();

        var bvExpr = context.BitVecConst<Size8>("bv");
        var intExpr = bvExpr.ToInt(signed: true);

        using var solver = context.CreateSolver();
        solver.Assert(bvExpr == 200); // 200 unsigned, -56 signed

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
        using var scope = context.SetUp();

        var bvExpr = context.BitVecConst<Size8>("bv");
        var intExpr = bvExpr.ToInt();

        using var solver = context.CreateSolver();
        solver.Assert(bvExpr == 200);

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
        using var scope = context.SetUp();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var extracted = bv8.Extract<Size4>(4); // Get upper 4 bits -> 1111
        var extended = extracted.Resize<Size8>(); // Extend to 8 bits -> 00001111

        using var solver = context.CreateSolver();
        solver.Assert(bv8 == 0b11110000);
        solver.Assert(extended == 0b00001111);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var extendedValue = model.GetBitVec(extended);
        Assert.That((int)extendedValue.Value, Is.EqualTo(0b00001111));
        Assert.That(Size8.Size, Is.EqualTo(8U));
    }

    [TestCase(0b1010, 0b10101010, Description = "Repeat 4-bit pattern twice")]
    [TestCase(0b11, 0b111111, Description = "Repeat 2-bit pattern three times")]
    [TestCase(0b1, 0b11111111, Description = "Repeat single bit eight times")]
    public void Repeat_AllVariations_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Different test cases for different input sizes
        if (value == 0b1010) // 4-bit to 8-bit
        {
            var x = context.BitVecConst<Size4>("x");
            var result = x.Repeat<Size8>();

            solver.Assert(x == value);
            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            var model = solver.GetModel();
            var resultValue = model.GetBitVec(result);
            Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
            Assert.That(Size8.Size, Is.EqualTo(8U));
        }
        else if (value == 0b11) // 2-bit to 6-bit
        {
            var x = context.BitVecConst<Size2>("x");
            var result = x.Repeat<Size6>();

            solver.Assert(x == value);
            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            var model = solver.GetModel();
            var resultValue = model.GetBitVec(result);
            Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
            Assert.That(Size6.Size, Is.EqualTo(6U));
        }
        else if (value == 0b1) // 1-bit to 8-bit
        {
            var x = context.BitVecConst<Size1>("x");
            var result = x.Repeat<Size8>();

            solver.Assert(x == value);
            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            var model = solver.GetModel();
            var resultValue = model.GetBitVec(result);
            Assert.That((int)resultValue.Value, Is.EqualTo(expectedResult));
            Assert.That(Size8.Size, Is.EqualTo(8U));
        }
    }

    [Test]
    public void Repeat_ContextMethod_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv4 = context.BitVecConst<Size4>("bv4");
        var bv12 = context.Repeat<Size4, Size12>(bv4); // Repeat three times

        using var solver = context.CreateSolver();
        solver.Assert(bv4 == 0b1100);
        solver.Assert(bv12 == 0b110011001100);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv12Value = model.GetBitVec(bv12);
        Assert.That((int)bv12Value.Value, Is.EqualTo(0b110011001100));
        Assert.That(Size12.Size, Is.EqualTo(12U));
    }

    [Test]
    public void Repeat_FluentAPI_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv3 = context.BitVecConst<Size3>("bv3");
        var bv9 = bv3.Repeat<Size9>(); // Repeat three times

        using var solver = context.CreateSolver();
        solver.Assert(bv3 == 0b110);
        solver.Assert(bv9 == 0b110110110);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bv9Value = model.GetBitVec(bv9);
        Assert.That((int)bv9Value.Value, Is.EqualTo(0b110110110));
        Assert.That(Size9.Size, Is.EqualTo(9U));
    }

    [Test]
    public void ComplexResizingScenario_WithModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.BitVecConst<Size8>("x");
        var y = context.BitVecConst<Size16>("y");

        // Extend x to 16 bits and add to y
        var xExtended = x.Resize<Size16>();

        using var solver = context.CreateSolver();
        solver.Assert(x == 42);
        solver.Assert(y == 58);
        // Note: arithmetic operators for Z3BitVecExpr are implemented now
        solver.Assert(xExtended + y == 100);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetBitVec(x);
        var yValue = model.GetBitVec(y);
        var xExtendedValue = model.GetBitVec(xExtended);

        Assert.That((int)xValue.Value, Is.EqualTo(42));
        Assert.That((int)yValue.Value, Is.EqualTo(58));
        Assert.That((int)xExtendedValue.Value, Is.EqualTo(42));
        Assert.That(Size16.Size, Is.EqualTo(16U));
    }
}
