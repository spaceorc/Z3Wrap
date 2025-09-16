using System.Numerics;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3IntExprTests;

[TestFixture]
public class Z3IntExprCreationTests
{
    [TestCase(8u, Description = "8-bit conversion")]
    [TestCase(16u, Description = "16-bit conversion")]
    [TestCase(32u, Description = "32-bit conversion")]
    [TestCase(64u, Description = "64-bit conversion")]
    public void ToBitVec_ValidSize_CreatesBitVecExpr(uint size)
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var bitVecResult = x.ToBitVec(size);

        Assert.That(bitVecResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bitVecResult.Context, Is.SameAs(context));
        Assert.That(bitVecResult.Size, Is.EqualTo(size));

        // Test converting integer 42 to specified bit-vector size
        solver.Assert(context.Eq(x, context.Int(42)));
        var expected = context.BitVec(42, size);
        solver.Assert(context.Eq(bitVecResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(255, 8u, Description = "255 to 8-bit")]
    [TestCase(65535, 16u, Description = "65535 to 16-bit")]
    [TestCase(-1, 32u, Description = "-1 to 32-bit")]
    [TestCase(0, 64u, Description = "0 to 64-bit")]
    public void ToBitVec_SpecificValues_CreatesBitVecWithCorrectValue(int value, uint size)
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.Int(value);
        var bitVecResult = x.ToBitVec(size);

        Assert.That(bitVecResult.Size, Is.EqualTo(size));

        // Test that the bit-vector represents the same value
        var expected = context.BitVec(value, size);
        solver.Assert(context.Eq(bitVecResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(42, Description = "Positive integer")]
    [TestCase(-15, Description = "Negative integer")]
    [TestCase(0, Description = "Zero")]
    [TestCase(100, Description = "Another positive")]
    public void ToReal_IntExpr_CreatesRealExpr(int value)
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.Int(value);
        var realResult = x.ToReal();

        Assert.That(realResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realResult.Context, Is.SameAs(context));

        // Test converting integer to real
        var expected = context.Real(value);
        solver.Assert(context.Eq(realResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(9223372036854775807L, Description = "long.MaxValue")]
    [TestCase(-9223372036854775808L, Description = "long.MinValue")]
    [TestCase(0L, Description = "Zero as long")]
    [TestCase(1000000000000L, Description = "Large positive long")]
    public void ImplicitConversion_FromLong_CreatesIntExpr(long value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from long
        Z3IntExpr longResult = value;

        Assert.That(longResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(longResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Int(value);
        solver.Assert(context.Eq(longResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(100, Description = "Positive BigInteger")]
    [TestCase(-250, Description = "Negative BigInteger")]
    [TestCase(0, Description = "Zero BigInteger")]
    public void ImplicitConversion_FromBigInteger_CreatesIntExpr(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigIntValue = new BigInteger(value);

        // Test implicit conversion from BigInteger
        Z3IntExpr bigIntResult = bigIntValue;

        Assert.That(bigIntResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bigIntResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Int(value);
        solver.Assert(context.Eq(bigIntResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(42, Description = "Regular int")]
    [TestCase(-10, Description = "Negative int")]
    [TestCase(0, Description = "Zero")]
    public void ImplicitConversion_FromInt_CreatesIntExpr(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from int
        Z3IntExpr intResult = value;

        Assert.That(intResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(intResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Int(value);
        solver.Assert(context.Eq(intResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_LongInArithmetic_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        // Test long implicit conversion in arithmetic operations
        var result = x + 1000000000000L; // Large long value

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x + 1000000000000L where x = 5 equals 1000000000005L
        solver.Assert(context.Eq(x, context.Int(5)));
        solver.Assert(context.Eq(result, context.Int(1000000000005L)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_BigIntegerInArithmetic_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var largeBigInt = new BigInteger(999999999999);

        // Test BigInteger implicit conversion in arithmetic operations
        var result = x * largeBigInt;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x * 999999999999 where x = 2 equals 1999999999998
        solver.Assert(context.Eq(x, context.Int(2)));
        solver.Assert(context.Eq(result, context.Int(new BigInteger(1999999999998))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}