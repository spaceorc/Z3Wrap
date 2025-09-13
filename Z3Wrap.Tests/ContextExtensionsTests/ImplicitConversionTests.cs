using System.Numerics;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.ContextExtensionsTests;

[TestFixture]
public class ImplicitConversionTests
{
    [Test]
    public void ImplicitIntConversion_WithoutSetUp_ThrowsException()
    {
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            Z3IntExpr _ = 42;        });

        Assert.That(ex.Message, Does.Contain("No Z3Context is currently set"));
        Assert.That(ex.Message, Does.Contain("context.SetUp()"));
    }

    [Test]
    public void ImplicitIntConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3IntExpr expr = 42;
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
        Assert.That(expr.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ImplicitLongConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3IntExpr expr = long.MaxValue;
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
        Assert.That(expr.ToString(), Is.EqualTo(long.MaxValue.ToString()));
    }

    [Test]
    public void ImplicitBigIntegerConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        BigInteger bigValue = new BigInteger(long.MaxValue) * 2;
        Z3IntExpr expr = bigValue;
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitConversion_ArithmeticOperations_CreatesValidExpressions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        // These should all work with implicit conversions
        var expr1 = x + 5;           // int -> Z3IntExpr
        var expr2 = 10 - x;          // int -> Z3IntExpr
        var expr3 = x * 3;           // int -> Z3IntExpr
        var expr4 = x == 42;         // int -> Z3IntExpr for comparison

        Assert.That(expr1, Is.Not.Null);
        Assert.That(expr2, Is.Not.Null);
        Assert.That(expr3, Is.Not.Null);
        Assert.That(expr4, Is.Not.Null);
    }

    [Test]
    public void ImplicitConversion_ArrayDefaultValue_CreatesSatisfiableArray()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        // This should work - 42 gets implicitly converted to Z3IntExpr
        var arr = context.Array<Z3IntExpr, Z3IntExpr>(42);

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the array works correctly
        var index = context.Int(0);
        var element = arr[index];

        using var solver = context.CreateSolver();
        solver.Assert(element == 42);
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicitConversion_NestedScopes_UsesCorrectContext()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        using (context1.SetUp())
        {
            Z3IntExpr expr1 = 100; // Uses context1
            Assert.That(expr1.Context, Is.EqualTo(context1));

            using (context2.SetUp())
            {
                Z3IntExpr expr2 = 200; // Uses context2
                Assert.That(expr2.Context, Is.EqualTo(context2));
                Assert.That(expr2.Context, Is.Not.EqualTo(expr1.Context));
            }

            // Back to context1
            Z3IntExpr expr3 = 300; // Uses context1 again
            Assert.That(expr3.Context, Is.EqualTo(context1));
        }

        // No context set
        Assert.Throws<InvalidOperationException>(() =>
        {
            Z3IntExpr _ = 400;        });
    }

    [Test]
    public void ImplicitConversion_AfterScopeDisposal_ThrowsException()
    {
        using var context = new Z3Context();

        {
            using var scope = context.SetUp();
            Z3IntExpr expr = 42;            Assert.That(expr, Is.Not.Null);
        } // scope disposed here

        // After scope disposal, should throw
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            Z3IntExpr _ = 42;        });

        Assert.That(ex.Message, Does.Contain("No Z3Context is currently set"));
    }

    [Test]
    public void ImplicitConversion_ComplexExpression_CreatesSatisfiableSolution()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Complex expression with multiple implicit conversions
        var constraint = (x + 5) * 2 == y - 10;

        using var solver = context.CreateSolver();
        solver.Assert(constraint);
        solver.Assert(x == 3); // x = 3, so (3 + 5) * 2 = 16, so y - 10 = 16, so y = 26

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetIntValue(y);

        Assert.That(xValue, Is.EqualTo(new BigInteger(3)));
        Assert.That(yValue, Is.EqualTo(new BigInteger(26)));
    }

    [Test]
    public void ImplicitConversion_MixedIntegerTypes_HandlesAllTypes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        // Mix of int, long, and BigInteger with implicit conversions
        var expr1 = x + 42;                              // int
        var expr2 = x + 9223372036854775807L;            // long (long.MaxValue)
        var expr3 = x + new BigInteger(long.MaxValue) * 2; // BigInteger

        Assert.That(expr1, Is.Not.Null);
        Assert.That(expr2, Is.Not.Null);
        Assert.That(expr3, Is.Not.Null);

        // Verify they can be used in constraints
        using var solver = context.CreateSolver();
        solver.Assert(expr1 > 0);
        solver.Assert(expr2 > 0);
        solver.Assert(expr3 > 0);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }
}