using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecEdgeCasesTests
{
    [Test]
    public void SMod_ContextExtension_WorksCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        // Act - This was completely uncovered in the coverage report
        var smod = context.SignedMod(a, b);

        // Assert
        Assert.That(smod.Size, Is.EqualTo(8));

        // Verify with solver using signed modulo semantics
        using var solver = context.CreateSolver();
        solver.Assert(a == 7);
        solver.Assert(b == 3);
        solver.Assert(smod == 1); // 7 smod 3 = 1

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SMod_WithNegativeValues_WorksCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        // Act
        var smod = context.SignedMod(a, b);

        // Assert - Test signed modulo with negative dividend
        using var solver = context.CreateSolver();

        // In 8-bit signed, -7 is represented as 249 (256-7)
        solver.Assert(a == 249);  // -7 in 8-bit two's complement
        solver.Assert(b == 3);

        // Signed modulo of -7 mod 3 should be 2 (not -1)
        // This is because SMod in Z3 follows the mathematical definition
        solver.Assert(smod == 2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Resize_EdgeCases_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Test the uncovered branches in Resize method

        // Test same size (should return original)
        var a8 = context.BitVecConst("a", 8);
        var resized8 = context.Resize(a8, 8);
        Assert.That(resized8, Is.EqualTo(a8)); // Should be same instance

        // Test size reduction (extract lower bits) - this was uncovered
        var a16 = context.BitVecConst("a", 16);
        var resized8from16 = context.Resize(a16, 8);
        Assert.That(resized8from16.Size, Is.EqualTo(8));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a16 == 0x1234);
        solver.Assert(resized8from16 == 0x34); // Lower 8 bits

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedResize_EdgeCases_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Test the uncovered branches in SignedResize method

        // Test same size (should return original)
        var a16 = context.BitVecConst("a", 16);
        var resized16 = context.Resize(a16, 16, signed: true);
        Assert.That(resized16, Is.EqualTo(a16)); // Should be same instance

        // Test size reduction with sign extension - this was uncovered
        var a32 = context.BitVecConst("a", 32);
        var resized16from32 = context.Resize(a32, 16, signed: true);
        Assert.That(resized16from32.Size, Is.EqualTo(16));

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a32 == 0x8000ABCD); // Negative number in 32-bit
        solver.Assert(resized16from32 == 0xABCD); // Lower 16 bits

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SRem_SizeValidation_EdgeCase_WorksCorrectly()
    {
        using var context = new Z3Context();
        // This tests the partial coverage in SRem - the size validation branch

        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b8 = context.BitVecConst("b", 8);
        var b16 = context.BitVecConst("c", 16);

        // Act & Assert - Size validation should work
        Assert.DoesNotThrow(() => context.Rem(a8, b8, signed: true));

        // Size mismatch should throw
        var ex = Assert.Throws<ArgumentException>(() => context.Rem(a8, b16, signed: true));
        Assert.That(ex!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
    }

    [Test]
    public void ComplexBitVectorOperations_LargerSizes_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Test with larger bit vector sizes to ensure our operations work beyond 8-bit

        // Arrange
        var a64 = context.BitVecConst("a", 64);
        var b64 = context.BitVecConst("b", 64);

        // Act - Test all major operation types
        var add = context.Add(a64, b64);
        var smod = context.SignedMod(a64, b64);
        var and = context.And(a64, b64);
        var shl = context.Shl(a64, b64);
        var ult = context.Lt(a64, b64);

        // Assert
        Assert.That(add.Size, Is.EqualTo(64));
        Assert.That(smod.Size, Is.EqualTo(64));
        Assert.That(and.Size, Is.EqualTo(64));
        Assert.That(shl.Size, Is.EqualTo(64));

        // Verify with solver using large values
        using var solver = context.CreateSolver();
        solver.Assert(a64 == 0x123456789ABCDEFL);
        solver.Assert(b64 == 0x1000000000000000L);

        // Test that operations work with 64-bit values
        solver.Assert(add == 0x1123456789ABCDEFL);
        solver.Assert(ult); // a < b since a is smaller

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BitVecOperations_WithBoundaryValues_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Test operations with boundary values (0, max values)

        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b8 = context.BitVecConst("b", 8);

        // Act & Assert - Test with boundary values
        using var solver = context.CreateSolver();

        // Test with zero
        solver.Assert(a8 == 0);
        solver.Assert(b8 == 255); // Max 8-bit value

        var add = context.Add(a8, b8);
        var and = context.And(a8, b8);
        var or = context.Or(a8, b8);

        solver.Assert(add == 255); // 0 + 255 = 255
        solver.Assert(and == 0);   // 0 & 255 = 0
        solver.Assert(or == 255);  // 0 | 255 = 255

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BitVecOperations_OverflowBehavior_WorksCorrectly()
    {
        using var context = new Z3Context();
        // Test overflow behavior in bit vector operations

        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b8 = context.BitVecConst("b", 8);

        // Act & Assert - Test overflow wrapping
        using var solver = context.CreateSolver();

        solver.Assert(a8 == 200);
        solver.Assert(b8 == 100);

        var add = context.Add(a8, b8);    // 200 + 100 = 300, wraps to 44
        var mul = context.Mul(a8, b8);    // 200 * 100 = 20000, wraps significantly

        solver.Assert(add == 44);         // (200 + 100) % 256 = 44

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}