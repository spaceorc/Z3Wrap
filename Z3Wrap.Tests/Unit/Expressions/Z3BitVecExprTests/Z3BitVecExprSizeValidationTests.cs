using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprSizeValidationTests
{
    [Test]
    public void ArithmeticOperators_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b16 = context.BitVecConst("b", 16);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 + b16;
        });
        var ex2 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 - b16;
        });
        var ex3 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 * b16;
        });
        var ex4 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 / b16;
        });
        var ex5 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 % b16;
        });

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex3!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex4!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex5!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
    }

    [Test]
    public void BitwiseOperators_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b32 = context.BitVecConst("b", 32);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 & b32;
        });
        var ex2 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 | b32;
        });
        var ex3 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8 ^ b32;
        });

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=8, right=32"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=8, right=32"));
        Assert.That(ex3!.Message, Contains.Substring("BitVector size mismatch: left=8, right=32"));
    }

    [Test]
    public void ShiftOperators_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a16 = context.BitVecConst("a", 16);
        var b8 = context.BitVecConst("b", 8);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a16 << b8;
        });
        var ex2 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a16 >> b8;
        });

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=16, right=8"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=16, right=8"));
    }

    [Test]
    public void ComparisonOperators_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a32 = context.BitVecConst("a", 32);
        var b64 = context.BitVecConst("b", 64);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a32 < b64;
        });
        var ex2 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a32 <= b64;
        });
        var ex3 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a32 > b64;
        });
        var ex4 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a32 >= b64;
        });

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=32, right=64"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=32, right=64"));
        Assert.That(ex3!.Message, Contains.Substring("BitVector size mismatch: left=32, right=64"));
        Assert.That(ex4!.Message, Contains.Substring("BitVector size mismatch: left=32, right=64"));
    }

    [Test]
    public void SignedOperations_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b16 = context.BitVecConst("b", 16);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8.Lt(b16, signed: true);
        });
        var ex2 = Assert.Throws<ArgumentException>(() =>
        {
            _ = context.Div(a8, b16, signed: true);
        });
        var ex3 = Assert.Throws<ArgumentException>(() =>
        {
            _ = a8.Shl(b16);
        });

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex3!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
    }

    [Test]
    public void ContextExtensions_WithMismatchedSizes_ThrowArgumentException()
    {
        using var context = new Z3Context();
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b16 = context.BitVecConst("b", 16);

        // Act & Assert
        var ex1 = Assert.Throws<ArgumentException>(() => context.Add(a8, b16));
        var ex2 = Assert.Throws<ArgumentException>(() => context.And(a8, b16));
        var ex3 = Assert.Throws<ArgumentException>(() => context.Shl(a8, b16));
        var ex4 = Assert.Throws<ArgumentException>(() => context.Lt(a8, b16));

        Assert.That(ex1!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex2!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex3!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
        Assert.That(ex4!.Message, Contains.Substring("BitVector size mismatch: left=8, right=16"));
    }

    [Test]
    public void SameSizeOperations_DoNotThrowExceptions()
    {
        using var context = new Z3Context();
        // Arrange
        var a8 = context.BitVecConst("a", 8);
        var b8 = context.BitVecConst("b", 8);

        // Act & Assert - These should NOT throw
        Assert.DoesNotThrow(() =>
        {
            _ = a8 + b8;
            _ = a8 & b8;
            _ = a8 << b8;
            _ = a8 < b8;
            _ = context.Div(a8, b8, signed: true);
            _ = context.Add(a8, b8);
        });
    }

    [Test]
    public void ImplicitConversion_FromBitVec_WorksCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        using var scope = context.SetUp();
        var bitVec = new BitVec(42, 8);

        // Act - This should work with implicit conversion
        Z3BitVecExpr expr = bitVec;

        // Assert
        Assert.That(expr.Size, Is.EqualTo(8));
        Assert.That(expr.ToString(), Contains.Substring("BitVec[8]"));
    }
}
