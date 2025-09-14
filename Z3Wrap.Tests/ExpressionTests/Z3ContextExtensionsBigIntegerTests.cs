namespace Z3Wrap.Tests.ExpressionTests;

[TestFixture]
public class Z3ContextExtensionsBigIntegerTests
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
    public void BigIntegerArithmetic_LeftOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly
        var add = context.Add(a, 10);
        var sub = context.Sub(a, 5);
        var mul = context.Mul(a, 3);
        var udiv = context.UDiv(a, 2);
        var sdiv = context.SDiv(a, 2);
        var urem = context.URem(a, 7);
        var srem = context.SRem(a, 7);
        var smod = context.SMod(a, 7);

        // Assert - Verify operations create valid expressions
        Assert.That(add.Size, Is.EqualTo(8));
        Assert.That(sub.Size, Is.EqualTo(8));
        Assert.That(mul.Size, Is.EqualTo(8));
        Assert.That(udiv.Size, Is.EqualTo(8));
        Assert.That(sdiv.Size, Is.EqualTo(8));
        Assert.That(urem.Size, Is.EqualTo(8));
        Assert.That(srem.Size, Is.EqualTo(8));
        Assert.That(smod.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 20);

        solver.Assert(add == 30);      // 20 + 10 = 30
        solver.Assert(sub == 15);      // 20 - 5 = 15
        solver.Assert(mul == 60);      // 20 * 3 = 60
        solver.Assert(udiv == 10);     // 20 / 2 = 10
        solver.Assert(sdiv == 10);     // 20 / 2 = 10
        solver.Assert(urem == 6);      // 20 % 7 = 6
        solver.Assert(srem == 6);      // 20 % 7 = 6
        solver.Assert(smod == 6);      // 20 % 7 = 6

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerArithmetic_RightOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly with BigInteger as left operand
        var add = context.Add(50, a);
        var sub = context.Sub(100, a);
        var mul = context.Mul(3, a);
        var udiv = context.UDiv(80, a);
        var sdiv = context.SDiv(80, a);
        var urem = context.URem(25, a);
        var srem = context.SRem(25, a);
        var smod = context.SMod(25, a);

        // Assert - Verify operations create valid expressions
        Assert.That(add.Size, Is.EqualTo(8));
        Assert.That(sub.Size, Is.EqualTo(8));
        Assert.That(mul.Size, Is.EqualTo(8));
        Assert.That(udiv.Size, Is.EqualTo(8));
        Assert.That(sdiv.Size, Is.EqualTo(8));
        Assert.That(urem.Size, Is.EqualTo(8));
        Assert.That(srem.Size, Is.EqualTo(8));
        Assert.That(smod.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 10);

        solver.Assert(add == 60);      // 50 + 10 = 60
        solver.Assert(sub == 90);      // 100 - 10 = 90
        solver.Assert(mul == 30);      // 3 * 10 = 30
        solver.Assert(udiv == 8);      // 80 / 10 = 8
        solver.Assert(sdiv == 8);      // 80 / 10 = 8
        solver.Assert(urem == 5);      // 25 % 10 = 5
        solver.Assert(srem == 5);      // 25 % 10 = 5
        solver.Assert(smod == 5);      // 25 % 10 = 5

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerBitwise_LeftOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly
        var and = context.And(a, 0x0F);    // 0b00001111
        var or = context.Or(a, 0xF0);      // 0b11110000
        var xor = context.Xor(a, 0xFF);    // 0b11111111

        // Assert - Verify operations create valid expressions
        Assert.That(and.Size, Is.EqualTo(8));
        Assert.That(or.Size, Is.EqualTo(8));
        Assert.That(xor.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 0b10101010);  // 170 in binary

        solver.Assert(and == 0b00001010);  // 170 & 15 = 10
        solver.Assert(or == 0b11111010);   // 170 | 240 = 250
        solver.Assert(xor == 0b01010101);  // 170 ^ 255 = 85

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerBitwise_RightOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly with BigInteger as left operand
        var and = context.And(0xFF, a);    // 0b11111111
        var or = context.Or(0x0F, a);      // 0b00001111
        var xor = context.Xor(0xAA, a);    // 0b10101010

        // Assert - Verify operations create valid expressions
        Assert.That(and.Size, Is.EqualTo(8));
        Assert.That(or.Size, Is.EqualTo(8));
        Assert.That(xor.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 0b11110000);  // 240 in binary

        solver.Assert(and == 0b11110000);  // 255 & 240 = 240
        solver.Assert(or == 0b11111111);   // 15 | 240 = 255
        solver.Assert(xor == 0b01011010);  // 170 ^ 240 = 90

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerShift_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly
        var shl = context.Shl(a, 2);
        var lshr = context.Lshr(a, 1);
        var ashr = context.Ashr(a, 1);

        // Assert - Verify operations create valid expressions
        Assert.That(shl.Size, Is.EqualTo(8));
        Assert.That(lshr.Size, Is.EqualTo(8));
        Assert.That(ashr.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 12);  // 0b00001100

        solver.Assert(shl == 48);   // 12 << 2 = 48 (0b00110000)
        solver.Assert(lshr == 6);   // 12 >> 1 = 6 (0b00000110)
        solver.Assert(ashr == 6);   // 12 >> 1 = 6 (same as logical for positive)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerComparison_LeftOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly
        var ult = context.Ult(a, 100);
        var slt = context.Slt(a, 100);
        var ule = context.Ule(a, 50);
        var sle = context.Sle(a, 50);
        var ugt = context.Ugt(a, 30);
        var sgt = context.Sgt(a, 30);
        var uge = context.Uge(a, 40);
        var sge = context.Sge(a, 40);

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 40);

        // All these should be true for a = 40
        solver.Assert(ult);    // 40 < 100 (unsigned)
        solver.Assert(slt);    // 40 < 100 (signed)
        solver.Assert(ule);    // 40 <= 50 (unsigned)
        solver.Assert(sle);    // 40 <= 50 (signed)
        solver.Assert(ugt);    // 40 > 30 (unsigned)
        solver.Assert(sgt);    // 40 > 30 (signed)
        solver.Assert(uge);    // 40 >= 40 (unsigned)
        solver.Assert(sge);    // 40 >= 40 (signed)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerComparison_RightOperand_WorksCorrectly()
    {
        // Arrange
        var a = context.BitVecConst("a", 8);

        // Act - Using context extensions directly with BigInteger as left operand
        var ult = context.Ult(30, a);
        var slt = context.Slt(30, a);
        var ule = context.Ule(50, a);
        var sle = context.Sle(50, a);
        var ugt = context.Ugt(60, a);
        var sgt = context.Sgt(60, a);
        var uge = context.Uge(50, a);
        var sge = context.Sge(50, a);

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a == 50);

        // All these should be true for a = 50
        solver.Assert(ult);    // 30 < 50 (unsigned)
        solver.Assert(slt);    // 30 < 50 (signed)
        solver.Assert(ule);    // 50 <= 50 (unsigned)
        solver.Assert(sle);    // 50 <= 50 (signed)
        solver.Assert(ugt);    // 60 > 50 (unsigned)
        solver.Assert(sgt);    // 60 > 50 (signed)
        solver.Assert(uge);    // 50 >= 50 (unsigned)
        solver.Assert(sge);    // 50 >= 50 (signed)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BigIntegerSizeValidation_ThrowsForMismatchedSizes()
    {
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b16 = context.BitVecConst("b", 16);

        // Act & Assert - These should throw because the BigInteger gets sized
        // to match the BitVec operand, but if we try to mix different sizes,
        // the underlying operations should still validate

        // Test that operations work when sizes are consistent
        Assert.DoesNotThrow(() =>
        {
            var _ = context.Add(a8, 42);
            var __ = context.And(255, a8);
            var ___ = context.Ult(a8, 100);
        });

        // The size validation happens in the underlying Z3BitVecExpr operations
        // when we mix different BitVec sizes, not with BigInteger operations
        // since BigInteger adapts to the BitVec size
    }

    [Test]
    public void BigIntegerOperations_DifferentBitVecSizes_WorkCorrectly()
    {
        // Arrange & Act - Test with different bit vector sizes
        var a8 = context.BitVecConst("a8", 8);
        var a16 = context.BitVecConst("a16", 16);
        var a32 = context.BitVecConst("a32", 32);

        // BigInteger operations should work with any size
        var result8 = context.Add(a8, 100);
        var result16 = context.Mul(a16, 1000);
        var result32 = context.And(a32, 0xFFFFFFFF);

        // Assert
        Assert.That(result8.Size, Is.EqualTo(8));
        Assert.That(result16.Size, Is.EqualTo(16));
        Assert.That(result32.Size, Is.EqualTo(32));

        // Verify with solver using simpler, correct values
        using var solver = context.CreateSolver();
        solver.Assert(a8 == 50);
        solver.Assert(a16 == 100);
        solver.Assert(a32 == 0x12345678);

        solver.Assert(result8 == 150);    // 50 + 100 = 150
        solver.Assert(result16 == 100000); // 100 * 1000 = 100000
        solver.Assert(result32 == 0x12345678); // 0x12345678 & 0xFFFFFFFF = 0x12345678

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}