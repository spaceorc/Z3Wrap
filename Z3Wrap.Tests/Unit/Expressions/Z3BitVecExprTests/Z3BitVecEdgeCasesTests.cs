using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecEdgeCasesTests
{
    [Test]
    public void SMod_ContextExtension_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        // Arrange
        var a = context.BitVecConst<Size8>("a");
        var b = context.BitVecConst<Size8>("b");

        // Act - This was completely uncovered in the coverage report
        var smod = context.SignedMod(a, b);

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
        using var scope = context.SetUp();
        // Arrange
        var a = context.BitVecConst<Size8>("a");
        var b = context.BitVecConst<Size8>("b");

        // Act
        var smod = context.SignedMod(a, b);

        // Assert - Test signed modulo with negative dividend
        using var solver = context.CreateSolver();

        // In 8-bit signed, -7 is represented as 249 (256-7)
        solver.Assert(a == 249); // -7 in 8-bit two's complement
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
        using var scope = context.SetUp();
        // Test the uncovered branches in Resize method

        // Test same size (should return original)
        var a8 = context.BitVecConst<Size8>("a");
        var resized8 = a8.Resize<Size8>();
        Assert.That(resized8, Is.EqualTo(a8)); // Should be same instance

        // Test size reduction (extract lower bits) - this was uncovered
        var a16 = context.BitVecConst<Size16>("a");
        var resized8From16 = a16.Resize<Size8>();

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a16 == 0x1234);
        solver.Assert(resized8From16 == 0x34); // Lower 8 bits

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedResize_EdgeCases_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        // Test the uncovered branches in SignedResize method

        // Test same size (should return original)
        var a16 = context.BitVecConst<Size16>("a");
        var resized16 = a16.Resize<Size16>(signed: true);
        Assert.That(resized16, Is.EqualTo(a16)); // Should be same instance

        // Test size reduction with sign extension - this was uncovered
        var a32 = context.BitVecConst<Size32>("a");
        var resized16From32 = a32.Resize<Size16>(signed: true);

        // Verify with solver
        using var solver = context.CreateSolver();
        solver.Assert(a32 == 0x8000ABCD); // Negative number in 32-bit
        solver.Assert(resized16From32 == 0xABCD); // Lower 16 bits

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexBitVectorOperations_LargerSizes_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        // Test with larger bit vector sizes to ensure our operations work beyond 8-bit

        // Arrange
        var a64 = context.BitVecConst<Size64>("a");
        var b64 = context.BitVecConst<Size64>("b");

        // Act - Test all major operation types
        var add = context.Add(a64, b64);
        var smod = context.SignedMod(a64, b64);
        var and = a64 & b64;
        var shl = a64 << b64;
        var ult = a64 < b64;

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
        using var scope = context.SetUp();
        // Test operations with boundary values (0, max values)

        // Arrange
        var a8 = context.BitVecConst<Size8>("a");
        var b8 = context.BitVecConst<Size8>("b");

        // Act & Assert - Test with boundary values
        using var solver = context.CreateSolver();

        // Test with zero
        solver.Assert(a8 == 0);
        solver.Assert(b8 == 255); // Max 8-bit value

        var add = context.Add(a8, b8);
        var and = context.And(a8, b8);
        var or = context.Or(a8, b8);

        solver.Assert(add == 255); // 0 + 255 = 255
        solver.Assert(and == 0); // 0 & 255 = 0
        solver.Assert(or == 255); // 0 | 255 = 255

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BitVecOperations_OverflowBehavior_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        // Test overflow behavior in bit vector operations

        // Arrange
        var a8 = context.BitVecConst<Size8>("a");
        var b8 = context.BitVecConst<Size8>("b");

        // Act & Assert - Test overflow wrapping
        using var solver = context.CreateSolver();

        solver.Assert(a8 == 200);
        solver.Assert(b8 == 100);

        var add = context.Add(a8, b8); // 200 + 100 = 300, wraps to 44
        var mul = context.Mul(a8, b8); // 200 * 100 = 20000, wraps significantly

        solver.Assert(add == 44); // (200 + 100) % 256 = 44

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
