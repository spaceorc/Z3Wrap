using System.Numerics;

namespace Z3Wrap.Tests.ExpressionTests;

[TestFixture]
public class Z3BitVecExprBigIntegerTests
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
    public void ArithmeticOperators_WithIntLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 10);

        // Test basic arithmetic with int literals
        solver.Assert(a + 5 == 15);           // a + int
        solver.Assert(a - 3 == 7);            // a - int
        solver.Assert(a * 2 == 20);           // a * int
        solver.Assert(a / 2 == 5);            // a / int
        solver.Assert(a % 3 == 1);            // a % int

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(10));
    }

    [Test]
    public void ArithmeticOperators_WithLongLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 32);

        using var solver = context.CreateSolver();
        solver.Assert(a == 1000000L);

        // Test arithmetic with long literals
        solver.Assert(a + 500000L == 1500000L);
        solver.Assert(a - 250000L == 750000L);
        solver.Assert(a / 4L == 250000L);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToLong(), Is.EqualTo(1000000L));
    }

    [Test]
    public void ArithmeticOperators_ReverseOrder_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 6);

        // Test reverse order arithmetic (literal op BitVec)
        solver.Assert(20 - a == 14);          // int - a
        solver.Assert(3 * a == 18);           // int * a
        solver.Assert(24 / a == 4);           // int / a
        solver.Assert(20 % a == 2);           // int % a

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(6));
    }

    [Test]
    public void BitwiseOperators_WithHexLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 0b11001100); // 204 in decimal, 0xCC in hex

        // Test bitwise operations with hex literals
        solver.Assert((a & 0xFF) == 0xCC);         // Should equal itself
        solver.Assert((a & 0x0F) == 0x0C);         // Lower 4 bits: 1100 & 1111 = 1100
        solver.Assert((a | 0x03) == 0xCF);         // 11001100 | 00000011 = 11001111
        solver.Assert((a ^ 0xFF) == 0x33);         // XOR with all 1s = bitwise NOT

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(0xCC));
    }

    [Test]
    public void BitwiseOperators_WithBinaryLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 0b10101010); // 170 in decimal

        // Test bitwise operations with binary literals
        solver.Assert((a & 0b11110000) == 0b10100000);  // Mask upper 4 bits
        solver.Assert((a | 0b00000101) == 0b10101111);  // Set bits 0 and 2
        solver.Assert((a ^ 0b01010101) == 0b11111111);  // XOR to get all 1s

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(0b10101010));
    }

    [Test]
    public void BitwiseOperators_ReverseOrder_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 0b11001100);

        // Test reverse order bitwise operations (literal op BitVec)
        solver.Assert((0xFF & a) == 0xCC);         // Should equal a
        solver.Assert((0x0F & a) == 0x0C);         // Lower 4 bits of a
        solver.Assert((0x03 | a) == 0xCF);         // 00000011 | 11001100 = 11001111

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(0xCC));
    }

    [Test]
    public void ComparisonOperators_WithLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();

        // Test comparison operations
        solver.Assert(a == 100);
        solver.Assert(a > 50);             // a > int
        solver.Assert(a >= 100);           // a >= int (equal)
        solver.Assert(a < 150);            // a < int
        solver.Assert(a <= 100);           // a <= int (equal)
        solver.Assert(a != 99);            // a != int

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(100));
    }

    [Test]
    public void ComparisonOperators_ReverseOrder_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();

        // Test reverse order comparisons (literal op BitVec)
        solver.Assert(a == 75);
        solver.Assert(50 < a);             // int < a
        solver.Assert(75 <= a);            // int <= a (equal)
        solver.Assert(100 > a);            // int > a
        solver.Assert(75 >= a);            // int >= a (equal)
        solver.Assert(74 != a);            // int != a

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(75));
    }

    [Test]
    public void ShiftOperators_WithLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 0b00001100); // 12 in decimal

        // Test shift operations with literals
        solver.Assert((a << 2) == 0b00110000);     // Left shift by 2: 12 -> 48
        solver.Assert((a >> 1) == 0b00000110);     // Right shift by 1: 12 -> 6

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(12));
    }

    [Test]
    public void ComplexExpressions_WithMixedLiterals_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        using var solver = context.CreateSolver();
        solver.Assert(a == 10);
        solver.Assert(b == 20);

        // Test complex mixed expressions
        solver.Assert((a + 5) * 2 == 30);                    // (10 + 5) * 2 = 30
        solver.Assert((a << 1) + b == 40);                   // (10 << 1) + 20 = 20 + 20 = 40
        // Let me recalculate: a=10=0x0A, b=20=0x14
        // (0x0A & 0x0F) | (0x14 & 0xF0) = 0x0A | 0x10 = 0x1A = 26
        solver.Assert(((a & 0x0F) | (b & 0xF0)) == 26);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(10));
        Assert.That(model.GetBitVec(b).ToInt(), Is.EqualTo(20));
    }

    [Test]
    public void DifferentBitVecSizes_AutoSizeLiterals_WorkCorrectly()
    {
        // Arrange
        var a8 = context.BitVecConst("a8", 8);    // 8-bit
        var a16 = context.BitVecConst("a16", 16); // 16-bit
        var a32 = context.BitVecConst("a32", 32); // 32-bit

        using var solver = context.CreateSolver();

        // Same literal value should adapt to different sizes
        solver.Assert(a8 == 255);           // 255 as 8-bit: 0xFF
        solver.Assert(a16 == 255);          // 255 as 16-bit: 0x00FF
        solver.Assert(a32 == 255);          // 255 as 32-bit: 0x000000FF

        // Test that operations work with auto-sizing
        solver.Assert((a8 & 0xFF) == 255);
        solver.Assert((a16 & 0xFFFF) == 255);
        solver.Assert((a32 & 0xFFFFFFFF) == 255);

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a8).ToInt(), Is.EqualTo(255));
        Assert.That(model.GetBitVec(a16).ToInt(), Is.EqualTo(255));
        Assert.That(model.GetBitVec(a32).ToInt(), Is.EqualTo(255));
    }

    [Test]
    public void EdgeCases_LargeNumbers_WorkCorrectly()
    {
        // Arrange
        var a32 = context.BitVecConst("a32", 32);

        using var solver = context.CreateSolver();

        // Test with large numbers that fit in 32-bit
        var largeNum = 0x80000000U; // 2^31, maximum value + 1 for signed 32-bit
        solver.Assert(a32 == largeNum);
        solver.Assert(a32 > 0x7FFFFFFF);    // Greater than max signed int
        solver.Assert(a32 + 0x7FFFFFFF == 0xFFFFFFFF); // Should wrap to max uint

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a32).ToUInt(), Is.EqualTo(0x80000000U));
    }

    [Test]
    public void EdgeCases_NegativeNumbers_WorkCorrectly()
    {
        // Arrange
        var a8 = context.BitVecConst("a8", 8);

        using var solver = context.CreateSolver();

        // Test negative numbers (will be converted to positive via two's complement)
        solver.Assert(a8 == -1);  // -1 should become 255 (0xFF) in 8-bit unsigned
        solver.Assert(a8 > 200);  // Verify it's treated as large positive number

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a8).ToInt(), Is.EqualTo(255)); // -1 becomes 0xFF = 255
    }

    [Test]
    public void MixedNumericTypes_AllConvertToBigInteger_WorkCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 16);

        using var solver = context.CreateSolver();
        solver.Assert(a == 1000);

        // Test different numeric types that all implicitly convert to BigInteger
        solver.Assert(a + (byte)50 == 1050);      // byte
        solver.Assert(a + (short)100 == 1100);    // short
        solver.Assert(a + 200 == 1200);           // int
        solver.Assert(a + 300L == 1300);          // long
        solver.Assert(a + 400U == 1400);          // uint
        solver.Assert(a + 500UL == 1500);         // ulong

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).ToInt(), Is.EqualTo(1000));
    }

    [Test]
    public void BigIntegerLiterals_ExplicitBigInteger_WorkCorrectly()
    {
        // Arrange
        var a64 = context.BitVecConst("a64", 64);

        using var solver = context.CreateSolver();

        // Test with explicit BigInteger values
        var bigValue = BigInteger.Pow(2, 40); // 2^40 = 1099511627776
        solver.Assert(a64 == bigValue);
        solver.Assert(a64 > BigInteger.Pow(2, 39));
        solver.Assert(a64 < BigInteger.Pow(2, 41));

        // Act & Assert
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetBitVec(a64).Value;
        Assert.That(result, Is.EqualTo(BigInteger.Pow(2, 40)));
    }
}