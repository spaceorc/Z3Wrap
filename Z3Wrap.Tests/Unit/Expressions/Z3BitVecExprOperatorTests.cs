using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3BitVecExprOperatorTests
{
    private Z3Context context = null!;

    [SetUp]
    public void Setup()
    {
        context = new Z3Context();
    }

    [TearDown]
    public void Cleanup()
    {
        context.Dispose();
    }

    [Test]
    public void ArithmeticOperators_BasicOperations_SolveCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var five = context.BitVec(new BitVec(5, 8));
        var three = context.BitVec(new BitVec(3, 8));
        var fifteen = context.BitVec(new BitVec(15, 8));
        var two = context.BitVec(new BitVec(2, 8));
        var one = context.BitVec(new BitVec(1, 8));

        using var solver = context.CreateSolver();

        // Test addition: x + y == 8 where x = 5, y = 3
        solver.Assert(x == five);
        solver.Assert(y == three);
        solver.Assert(x + y == context.BitVec(new BitVec(8, 8)));

        // Test subtraction: x - y == 2 where x = 5, y = 3
        solver.Assert(x - y == two);

        // Test multiplication: x * y == 15 where x = 5, y = 3
        solver.Assert(x * y == fifteen);

        // Test division: x / y == 1 where x = 5, y = 3 (unsigned division)
        solver.Assert(x / y == one);

        // Test remainder: x % y == 2 where x = 5, y = 3
        solver.Assert(x % y == two);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(5));
        Assert.That(model.GetBitVec(y).ToInt(), Is.EqualTo(3));
    }

    [Test]
    public void UnaryMinusOperator_NegatesValue_WorksCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var five = context.BitVec(new BitVec(5, 8));

        using var solver = context.CreateSolver();
        solver.Assert(x == five);
        solver.Assert(-x == context.BitVec(new BitVec(251, 8))); // -5 in 8-bit two's complement = 251

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BitwiseOperators_AllOperations_WorkCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var val12 = context.BitVec(new BitVec(12, 8));  // 0b00001100
        var val10 = context.BitVec(new BitVec(10, 8));  // 0b00001010
        var val8 = context.BitVec(new BitVec(8, 8));    // 0b00001000 (12 & 10)
        var val14 = context.BitVec(new BitVec(14, 8));  // 0b00001110 (12 | 10)
        var val6 = context.BitVec(new BitVec(6, 8));    // 0b00000110 (12 ^ 10)
        var val243 = context.BitVec(new BitVec(243, 8)); // 0b11110011 (~12)

        using var solver = context.CreateSolver();

        solver.Assert(x == val12);
        solver.Assert(y == val10);

        // Test bitwise AND
        solver.Assert((x & y) == val8);

        // Test bitwise OR
        solver.Assert((x | y) == val14);

        // Test bitwise XOR
        solver.Assert((x ^ y) == val6);

        // Test bitwise NOT
        solver.Assert(~x == val243);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(12));
        Assert.That(model.GetBitVec(y).ToInt(), Is.EqualTo(10));
    }

    [Test]
    public void ShiftOperators_LeftAndRightShift_WorkCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var shift = context.BitVecConst("shift", 8);
        var val12 = context.BitVec(new BitVec(12, 8));  // 0b00001100
        var val2 = context.BitVec(new BitVec(2, 8));    // shift amount
        var val48 = context.BitVec(new BitVec(48, 8));  // 12 << 2 = 0b00110000
        var val3 = context.BitVec(new BitVec(3, 8));    // 12 >> 2 = 0b00000011

        using var solver = context.CreateSolver();

        solver.Assert(x == val12);
        solver.Assert(shift == val2);

        // Test left shift
        solver.Assert((x << shift) == val48);

        // Test logical right shift
        solver.Assert((x >> shift) == val3);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(12));
        Assert.That(model.GetBitVec(shift).ToInt(), Is.EqualTo(2));
    }

    [Test]
    public void ComparisonOperators_UnsignedComparisons_WorkCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var val5 = context.BitVec(new BitVec(5, 8));
        var val10 = context.BitVec(new BitVec(10, 8));

        using var solver = context.CreateSolver();

        solver.Assert(x == val5);
        solver.Assert(y == val10);

        // Test unsigned comparisons
        solver.Assert(x < y);     // 5 < 10
        solver.Assert(x <= y);    // 5 <= 10
        solver.Assert(y > x);     // 10 > 5
        solver.Assert(y >= x);    // 10 >= 5
        solver.Assert(x <= val5); // 5 <= 5
        solver.Assert(x >= val5); // 5 >= 5

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(5));
        Assert.That(model.GetBitVec(y).ToInt(), Is.EqualTo(10));
    }

    [Test]
    public void SignedOperations_ExplicitSignedMethods_WorkCorrectly()
    {
        // Arrange - using 4-bit values to easily test signed behavior
        var x = context.BitVecConst("x", 4);
        var y = context.BitVecConst("y", 4);
        var negOne = context.BitVec(new BitVec(15, 4)); // -1 in 4-bit two's complement = 0b1111 = 15
        var posTwo = context.BitVec(new BitVec(2, 4));  // +2 = 0b0010 = 2

        using var solver = context.CreateSolver();

        solver.Assert(x == negOne);
        solver.Assert(y == posTwo);

        // Test signed comparisons (-1 < 2 in signed, but 15 > 2 in unsigned)
        solver.Assert(x.Lt(y, signed: true));  // -1 < 2 (signed)
        solver.Assert(x.Le(y, signed: true));  // -1 <= 2 (signed)
        solver.Assert(y.Gt(x, signed: true));  // 2 > -1 (signed)
        solver.Assert(y.Ge(x, signed: true));  // 2 >= -1 (signed)

        // But unsigned comparison should be opposite
        solver.Assert(x > y);  // 15 > 2 (unsigned)

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(15));
        Assert.That(model.GetBitVec(y).ToInt(), Is.EqualTo(2));
    }

    [Test]
    public void SignedDivisionOperations_SignedDivRemMod_WorkCorrectly()
    {
        // Arrange - test signed division with negative numbers
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var negTen = context.BitVec(new BitVec(246, 8)); // -10 in 8-bit two's complement
        var three = context.BitVec(new BitVec(3, 8));
        var negThree = context.BitVec(new BitVec(253, 8)); // -3 in 8-bit two's complement
        var negOne = context.BitVec(new BitVec(255, 8)); // -1 in 8-bit two's complement

        using var solver = context.CreateSolver();

        solver.Assert(x == negTen);  // x = -10
        solver.Assert(y == three);   // y = 3

        // Test signed division: -10 / 3 = -3
        solver.Assert(context.Div(x, y, signed: true) == negThree);

        // Test signed remainder: -10 % 3 = -1
        solver.Assert(context.Rem(x, y, signed: true) == negOne);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(246));
        Assert.That(model.GetBitVec(y).ToInt(), Is.EqualTo(3));
    }

    [Test]
    public void ArithmeticShiftRight_PreservesSignBit_WorksCorrectly()
    {
        // Arrange
        var x = context.BitVecConst("x", 8);
        var shift = context.BitVecConst("shift", 8);
        var negValue = context.BitVec(new BitVec(240, 8)); // 0b11110000 = -16 in signed
        var one = context.BitVec(new BitVec(1, 8));
        var expected = context.BitVec(new BitVec(248, 8)); // 0b11111000 = -8 (arithmetic shift right)

        using var solver = context.CreateSolver();

        solver.Assert(x == negValue);
        solver.Assert(shift == one);

        // Test arithmetic shift right (preserves sign bit)
        solver.Assert(x.Shr(shift, signed: true) == expected);

        // Compare with logical shift right (doesn't preserve sign bit)
        var logicalResult = context.BitVec(new BitVec(120, 8)); // 0b01111000 = 120
        solver.Assert((x >> shift) == logicalResult);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).ToInt(), Is.EqualTo(240));
        Assert.That(model.GetBitVec(shift).ToInt(), Is.EqualTo(1));
    }

    [Test]
    public void OperatorChaining_ComplexExpressions_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);
        var c = context.BitVecConst("c", 8);

        var val2 = context.BitVec(new BitVec(2, 8));
        var val3 = context.BitVec(new BitVec(3, 8));
        var val4 = context.BitVec(new BitVec(4, 8));
        var val14 = context.BitVec(new BitVec(14, 8));

        using var solver = context.CreateSolver();

        solver.Assert(a == val2);
        solver.Assert(b == val3);
        solver.Assert(c == val4);

        // Test complex expression: (a + b) * c - (a & b) == 14
        // (2 + 3) * 4 - (2 & 3) = 5 * 4 - 2 = 20 - 2 = 18... wait, let me recalculate
        // Actually: (2 + 3) * 4 - (2 & 3) = 5 * 4 - 2 = 20 - 2 = 18
        // Let me adjust the expected value
        var val18 = context.BitVec(new BitVec(18, 8));
        solver.Assert((a + b) * c - (a & b) == val18);

        // Test another complex expression with shifts and bitwise ops
        // (a << val2) | (b ^ c) where a=2, b=3, c=4
        // (2 << 2) | (3 ^ 4) = 8 | 7 = 15
        var val15 = context.BitVec(new BitVec(15, 8));
        solver.Assert(((a << val2) | (b ^ c)) == val15);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(2));
        Assert.That(model.GetBitVec(b).ToInt(), Is.EqualTo(3));
        Assert.That(model.GetBitVec(c).ToInt(), Is.EqualTo(4));
    }
}