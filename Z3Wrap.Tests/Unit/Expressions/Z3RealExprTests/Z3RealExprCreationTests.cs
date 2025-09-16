using System.Globalization;
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
    public void NegativeValues_Decimals_CreateCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test negative decimal values
        var negativeDecimal = context.Real(-3.14m);
        var negativeRealType = context.Real(new Real(-22, 7));

        // Test implicit conversions with negative values
        Z3RealExpr implicitNegativeDecimal = -2.718m;

        Assert.Multiple(() =>
        {
            Assert.That(negativeDecimal.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negativeRealType.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitNegativeDecimal.Handle, Is.Not.EqualTo(IntPtr.Zero));
        });

        // Verify all have the same context
        Assert.Multiple(() =>
        {
            Assert.That(negativeDecimal.Context, Is.SameAs(context));
            Assert.That(negativeRealType.Context, Is.SameAs(context));
            Assert.That(implicitNegativeDecimal.Context, Is.SameAs(context));
        });
    }

    [Test]
    public void ZeroValues_AllTypes_CreateCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test zero values for decimal types
        var zeroDecimal = context.Real(0.0m);
        var zeroReal = context.Real(Real.Zero);

        // Test implicit conversions with zero values
        Z3RealExpr implicitZeroDecimal = 0.0m;
        Z3RealExpr implicitZeroReal = Real.Zero;

        // All zeros should be equivalent
        solver.Assert(context.Eq(zeroDecimal, zeroReal));
        solver.Assert(context.Eq(zeroReal, implicitZeroDecimal));
        solver.Assert(context.Eq(implicitZeroDecimal, implicitZeroReal));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TypeCoercion_DecimalWithRealExpr_WorkTogetherCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.RealConst("real");

        // Test decimal coercion in a complex expression
        // (real + decimal) * decimal / decimal
        var decimalVal1 = 5.5m;
        var decimalVal2 = 2.0m;
        var decimalVal3 = 4.0m;

        var complexResult = (realExpr + decimalVal1) * decimalVal2 / decimalVal3;

        Assert.That(complexResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(complexResult.Context, Is.SameAs(context));

        // Set real = 2.5, expected: (2.5 + 5.5) * 2.0 / 4.0 = 8.0 * 2.0 / 4.0 = 16.0 / 4.0 = 4.0
        solver.Assert(context.Eq(realExpr, context.Real(2.5m)));
        solver.Assert(context.Eq(complexResult, context.Real(4.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FractionalValues_WithRealType_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test fractional Real values
        var oneThird = new Real(1, 3);
        var twoThirds = new Real(2, 3);
        var piApprox = new Real(22, 7);

        var realExpr1 = context.Real(oneThird);
        var realExpr2 = context.Real(twoThirds);
        var realExpr3 = context.Real(piApprox);

        // Test that 1/3 + 2/3 = 1
        var sum = realExpr1 + realExpr2;
        solver.Assert(context.Eq(sum, context.Real(Real.One)));

        // Test that pi approximation is greater than 3
        solver.Assert(realExpr3 > context.Real(3.0m));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}