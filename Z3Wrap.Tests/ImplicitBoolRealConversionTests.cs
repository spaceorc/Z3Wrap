using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests;

[TestFixture]
public class ImplicitBoolRealConversionTests
{
    [Test]
    public void ImplicitBoolConversion_WithoutSetUp_ThrowsException()
    {
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            Z3BoolExpr _ = true; // Should throw
        });

        Assert.That(ex.Message, Does.Contain("No Z3Context is currently set"));
        Assert.That(ex.Message, Does.Contain("context.SetUp()"));
    }

    [Test]
    public void ImplicitBoolConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3BoolExpr trueExpr = true;   // Should work with implicit conversion
        Z3BoolExpr falseExpr = false; // Should work with implicit conversion

        Assert.That(trueExpr, Is.Not.Null);
        Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(trueExpr.Context, Is.EqualTo(context));

        Assert.That(falseExpr, Is.Not.Null);
        Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(falseExpr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_IntValue_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3RealExpr expr = 42; // int -> Z3RealExpr

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_LongValue_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3RealExpr expr = long.MaxValue; // long -> Z3RealExpr

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_DecimalValue_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3RealExpr expr = 3.14159m; // decimal -> Z3RealExpr

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_BigIntegerValue_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        BigInteger bigValue = new BigInteger(long.MaxValue) * 3;
        Z3RealExpr expr = bigValue; // BigInteger -> Z3RealExpr

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_RealValue_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Real realValue = new Real(22, 7); // 22/7 approximation of pi
        Z3RealExpr expr = realValue; // Real -> Z3RealExpr

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitBoolConversion_InLogicalOperations_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.BoolConst("x");

        // These should all work with implicit conversions
        var expr1 = x & true;        // bool -> Z3BoolExpr
        var expr2 = false | x;       // bool -> Z3BoolExpr
        var expr3 = x ^ true;        // bool -> Z3BoolExpr

        Assert.That(expr1, Is.Not.Null);
        Assert.That(expr2, Is.Not.Null);
        Assert.That(expr3, Is.Not.Null);

        // Verify they work in solver
        using var solver = context.CreateSolver();
        solver.Assert(expr1); // x & true, so x must be true
        solver.Assert(!expr2); // !(false | x), so !x, so x must be false

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable)); // Contradiction
    }

    [Test]
    public void ImplicitRealConversion_InArithmetic_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.RealConst("x");

        // These should all work with implicit conversions
        var expr1 = x + 5;           // int -> Z3RealExpr
        var expr2 = x - 10L;         // long -> Z3RealExpr
        var expr3 = x * 3.14159m;    // decimal -> Z3RealExpr
        var expr4 = x / new BigInteger(7); // BigInteger -> Z3RealExpr

        Assert.That(expr1, Is.Not.Null);
        Assert.That(expr2, Is.Not.Null);
        Assert.That(expr3, Is.Not.Null);
        Assert.That(expr4, Is.Not.Null);

        // Verify they work in solver
        using var solver = context.CreateSolver();
        solver.Assert(expr1 > 0);
        solver.Assert(expr2 > 0);
        solver.Assert(expr3 > 0);
        solver.Assert(expr4 > 0);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_ArrayCreation_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        // Bool array with implicit default
        var boolArray = context.Array<Z3IntExpr, Z3BoolExpr>(defaultValue: true);

        // Real arrays with different implicit defaults
        var realArray1 = context.Array<Z3IntExpr, Z3RealExpr>(defaultValue: 42);      // int
        var realArray2 = context.Array<Z3IntExpr, Z3RealExpr>(defaultValue: 42L);     // long
        var realArray3 = context.Array<Z3IntExpr, Z3RealExpr>(defaultValue: 3.14m);   // decimal

        Assert.That(boolArray, Is.Not.Null);
        Assert.That(realArray1, Is.Not.Null);
        Assert.That(realArray2, Is.Not.Null);
        Assert.That(realArray3, Is.Not.Null);

        // Verify they work correctly
        var index = context.Int(0);
        using var solver = context.CreateSolver();

        Z3BoolExpr trueValue = true;   // Implicit conversion
        Z3RealExpr intValue = 42;      // Implicit conversion
        Z3RealExpr longValue = 42L;    // Implicit conversion

        solver.Assert(boolArray[index] == trueValue);
        solver.Assert(realArray1[index] == intValue);
        solver.Assert(realArray2[index] == longValue);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_MixedTypes_ComplexExpression_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.RealConst("x");
        var b = context.BoolConst("b");

        // Complex expression mixing real arithmetic and boolean logic with implicit conversions
        var realConstraint = (x + 1) * 2.5m == x * 3 - 0.5m; // Mix of Real operations with decimals
        var boolConstraint = b & true | false;                 // Boolean logic with implicit values

        using var solver = context.CreateSolver();
        solver.Assert(realConstraint);
        solver.Assert(boolConstraint); // This simplifies to just 'b'

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValueStr = model.GetRealValueAsString(x);

        // The equation (x + 1) * 2.5 = x * 3 - 0.5
        // Expands to: 2.5x + 2.5 = 3x - 0.5
        // Solving: 2.5 + 0.5 = 3x - 2.5x => 3 = 0.5x => x = 6
        Assert.That(xValueStr, Is.EqualTo("6"));
    }

    [Test]
    public void ImplicitConversion_NestedScopes_WorksForAllTypes()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        using (context1.SetUp())
        {
            Z3BoolExpr b1 = true;
            Z3RealExpr r1 = 3.14m;
            Assert.That(b1.Context, Is.EqualTo(context1));
            Assert.That(r1.Context, Is.EqualTo(context1));

            using (context2.SetUp())
            {
                Z3BoolExpr b2 = false;
                Z3RealExpr r2 = 2.71m;
                Assert.That(b2.Context, Is.EqualTo(context2));
                Assert.That(r2.Context, Is.EqualTo(context2));
                Assert.That(b2.Context, Is.Not.EqualTo(b1.Context));
                Assert.That(r2.Context, Is.Not.EqualTo(r1.Context));
            }

            // Back to context1
            Z3BoolExpr b3 = true;
            Z3RealExpr r3 = 1.41m;
            Assert.That(b3.Context, Is.EqualTo(context1));
            Assert.That(r3.Context, Is.EqualTo(context1));
        }

        // No context set - should throw
        Assert.Throws<InvalidOperationException>(() => { Z3BoolExpr _ = true; });
        Assert.Throws<InvalidOperationException>(() => { Z3RealExpr _ = 2.5m; });
    }
}