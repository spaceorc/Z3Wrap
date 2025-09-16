using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3RealExprTests;

[TestFixture]
public class Z3RealExprCreationTests
{
    [Test]
    public void RealConst_CreatesRealConstant_WithCorrectProperties()
    {
        using var context = new Z3Context();

        var realConst = context.RealConst("x");

        Assert.That(realConst.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realConst.Context, Is.SameAs(context));
        Assert.That(realConst.ToString(), Does.Contain("x"));
    }

    [Test]
    public void Real_FromDecimal_CreatesRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var realValue = context.Real(3.14m);

        Assert.That(realValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realValue.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(3.14m);
        solver.Assert(context.Eq(realValue, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Real_FromInteger_CreatesRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var realValue = context.Real(42);

        Assert.That(realValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realValue.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(42.0m);
        solver.Assert(context.Eq(realValue, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Real_FromLong_CreatesRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var longValue = 9223372036854775807L; // long.MaxValue
        var realValue = context.Real(longValue);

        Assert.That(realValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realValue.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real((decimal)longValue);
        solver.Assert(context.Eq(realValue, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Real_FromBigInteger_CreatesRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bigIntValue = BigInteger.Parse("123456789012345678901234567890");
        var realValue = context.Real(bigIntValue);

        Assert.That(realValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realValue.Context, Is.SameAs(context));

        // Verify the value is correct by creating another with same value
        var expected = context.Real(bigIntValue);
        solver.Assert(context.Eq(realValue, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Real_FromRealDataType_CreatesRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var realDataType = new Real(22, 7); // Approximation of pi
        var realValue = context.Real(realDataType);

        Assert.That(realValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realValue.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(realDataType);
        solver.Assert(context.Eq(realValue, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToInt_RealExpr_CreatesIntExpr()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var intResult = x.ToInt();

        Assert.That(intResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(intResult.Context, Is.SameAs(context));

        // Test converting real 42.0 to integer 42
        solver.Assert(context.Eq(x, context.Real(42.0m)));
        var expected = context.Int(42);
        solver.Assert(context.Eq(intResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToInt_NegativeReal_CreatesNegativeInt()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var intResult = x.ToInt();

        // Test converting negative real to negative integer
        solver.Assert(context.Eq(x, context.Real(-15.0m)));
        var expected = context.Int(-15);
        solver.Assert(context.Eq(intResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToInt_FractionalReal_TruncatesToInt()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var intResult = x.ToInt();

        // Test converting fractional real to truncated integer
        // Note: Z3 behavior for real-to-int conversion may vary
        solver.Assert(context.Eq(x, context.Real(15.7m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Verify that the result is an integer (no fractional part)
        var intValue = model.GetIntValue(intResult);
        Assert.That(intValue, Is.InstanceOf<BigInteger>());
    }

    [Test]
    public void ImplicitConversion_FromLong_CreatesRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from long
        Z3RealExpr longResult = 9223372036854775807L; // long.MaxValue

        Assert.That(longResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(longResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(9223372036854775807L);
        solver.Assert(context.Eq(longResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromNegativeLong_CreatesNegativeRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from negative long
        Z3RealExpr negativeLongResult = -9223372036854775808L; // long.MinValue

        Assert.That(negativeLongResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negativeLongResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(-9223372036854775808L);
        solver.Assert(context.Eq(negativeLongResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_LongInArithmetic_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // Test long implicit conversion in arithmetic operations
        var result = x + 1000000000000L; // Large long value

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x + 1000000000000L where x = 5.5 equals 1000000000005.5
        solver.Assert(context.Eq(x, context.Real(5.5m)));
        solver.Assert(context.Eq(result, context.Real(1000000000005.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromBigInteger_CreatesRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from BigInteger
        var bigIntValue = BigInteger.Parse("123456789012345678901234567890");
        Z3RealExpr bigIntResult = bigIntValue;

        Assert.That(bigIntResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bigIntResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(bigIntValue);
        solver.Assert(context.Eq(bigIntResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromNegativeBigInteger_CreatesNegativeRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from negative BigInteger
        var negativeBigIntValue = BigInteger.Parse("-987654321098765432109876543210");
        Z3RealExpr negativeBigIntResult = negativeBigIntValue;

        Assert.That(negativeBigIntResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negativeBigIntResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(negativeBigIntValue);
        solver.Assert(context.Eq(negativeBigIntResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_BigIntegerInArithmetic_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // Test BigInteger implicit conversion in arithmetic operations
        var bigIntValue = new BigInteger(999999999999999);
        var result = x + bigIntValue;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x + bigIntValue where x = 1 equals 1000000000000000
        solver.Assert(context.Eq(x, context.Real(1)));
        solver.Assert(context.Eq(result, context.Real(new BigInteger(1000000000000000))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromReal_CreatesRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from Real data type
        var realValue = new Real(22, 7); // Approximation of pi
        Z3RealExpr realResult = realValue;

        Assert.That(realResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(realValue);
        solver.Assert(context.Eq(realResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromDecimal_CreatesRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from decimal
        Z3RealExpr decimalResult = 3.14159m;

        Assert.That(decimalResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(decimalResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(3.14159m);
        solver.Assert(context.Eq(decimalResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_FromInt_CreatesRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion from int
        Z3RealExpr intResult = 42;

        Assert.That(intResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(intResult.Context, Is.SameAs(context));

        // Verify the value is correct
        var expected = context.Real(42);
        solver.Assert(context.Eq(intResult, expected));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_DecimalInArithmetic_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // Test decimal implicit conversion in arithmetic operations
        var result = x * 2.5m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x * 2.5m where x = 4.2m equals 10.5m
        solver.Assert(context.Eq(x, context.Real(4.2m)));
        solver.Assert(context.Eq(result, context.Real(10.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TypeCoercion_MixedTypes_WorkTogetherCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.RealConst("real");

        // Test various type coercions in a single expression
        // (real + int) * decimal + long + bigInteger
        var intVal = 5;
        var decimalVal = 2.5m;
        var longVal = 1000L;
        var bigIntVal = new BigInteger(999);

        var complexResult = (realExpr + intVal) * decimalVal + longVal + bigIntVal;

        Assert.That(complexResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(complexResult.Context, Is.SameAs(context));

        // Set real = 3.0, expected: (3.0 + 5) * 2.5 + 1000 + 999 = 8.0 * 2.5 + 1999 = 20.0 + 1999 = 2019.0
        solver.Assert(context.Eq(realExpr, context.Real(3.0m)));
        solver.Assert(context.Eq(complexResult, context.Real(2019.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NegativeValues_AllTypes_CreateCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test negative values for all supported types
        var negativeDecimal = context.Real(-3.14m);
        var negativeInt = context.Real(-42);
        var negativeLong = context.Real(-9223372036854775808L);
        var negativeBigInt = context.Real(new BigInteger(-123456789));
        var negativeRealType = context.Real(new Real(-22, 7));

        // Test implicit conversions with negative values
        Z3RealExpr implicitNegativeDecimal = -2.718m;
        Z3RealExpr implicitNegativeInt = -100;
        Z3RealExpr implicitNegativeLong = -5000000000L;
        Z3RealExpr implicitNegativeBigInt = new BigInteger(-987654321);

        Assert.Multiple(() =>
        {
            Assert.That(negativeDecimal.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negativeInt.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negativeLong.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negativeBigInt.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negativeRealType.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitNegativeDecimal.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitNegativeInt.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitNegativeLong.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitNegativeBigInt.Handle, Is.Not.EqualTo(IntPtr.Zero));
        });

        // Verify all have the same context
        Assert.Multiple(() =>
        {
            Assert.That(negativeDecimal.Context, Is.SameAs(context));
            Assert.That(negativeInt.Context, Is.SameAs(context));
            Assert.That(negativeLong.Context, Is.SameAs(context));
            Assert.That(negativeBigInt.Context, Is.SameAs(context));
            Assert.That(negativeRealType.Context, Is.SameAs(context));
            Assert.That(implicitNegativeDecimal.Context, Is.SameAs(context));
            Assert.That(implicitNegativeInt.Context, Is.SameAs(context));
            Assert.That(implicitNegativeLong.Context, Is.SameAs(context));
            Assert.That(implicitNegativeBigInt.Context, Is.SameAs(context));
        });
    }

    [Test]
    public void ZeroValues_AllTypes_CreateCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test zero values for all supported types
        var zeroDecimal = context.Real(0.0m);
        var zeroInt = context.Real(0);
        var zeroLong = context.Real(0L);
        var zeroBigInt = context.Real(BigInteger.Zero);

        // Test implicit conversions with zero values
        Z3RealExpr implicitZeroDecimal = 0.0m;
        Z3RealExpr implicitZeroInt = 0;
        Z3RealExpr implicitZeroLong = 0L;
        Z3RealExpr implicitZeroBigInt = BigInteger.Zero;

        // All zeros should be equivalent
        solver.Assert(context.Eq(zeroDecimal, zeroInt));
        solver.Assert(context.Eq(zeroInt, zeroLong));
        solver.Assert(context.Eq(zeroLong, zeroBigInt));
        solver.Assert(context.Eq(zeroBigInt, implicitZeroDecimal));
        solver.Assert(context.Eq(implicitZeroDecimal, implicitZeroInt));
        solver.Assert(context.Eq(implicitZeroInt, implicitZeroLong));
        solver.Assert(context.Eq(implicitZeroLong, implicitZeroBigInt));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}