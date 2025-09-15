using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class CoreExtensionsTests
{
    [Test]
    public void Eq_IntExpr_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var equality = context.Eq(x, y);

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        // Test 5 == 5 is satisfiable
        solver.Assert(context.Eq(context.Int(5), context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lt_IntExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var comparison = context.Lt(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 3 < 5 is satisfiable
        solver.Assert(context.Lt(context.Int(3), context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_IntExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var comparison = context.Le(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 5 <= 5 is satisfiable
        solver.Assert(context.Le(context.Int(5), context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Gt_IntExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var comparison = context.Gt(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 10 > 3 is satisfiable
        solver.Assert(context.Gt(context.Int(10), context.Int(3)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ge_IntExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var comparison = context.Ge(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 7 >= 7 is satisfiable
        solver.Assert(context.Ge(context.Int(7), context.Int(7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lt_RealExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var comparison = context.Lt(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 2.5m < 3.7m is satisfiable
        solver.Assert(context.Lt(context.Real(2.5m), context.Real(3.7m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_RealExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var comparison = context.Le(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 4.2m <= 4.2m is satisfiable
        solver.Assert(context.Le(context.Real(4.2m), context.Real(4.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }


    [Test]
    public void Min_IntExpr_CreatesMinimumExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var minExpr = context.Min(x, y);

        Assert.That(minExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minExpr.Context, Is.SameAs(context));

        // Test min(7, 3) == 3
        var result = context.Min(context.Int(7), context.Int(3));
        solver.Assert(context.Eq(result, context.Int(3)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_IntExpr_CreatesMaximumExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var maxExpr = context.Max(x, y);

        Assert.That(maxExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maxExpr.Context, Is.SameAs(context));

        // Test max(7, 3) == 7
        var result = context.Max(context.Int(7), context.Int(3));
        solver.Assert(context.Eq(result, context.Int(7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Min_RealExpr_CreatesMinimumExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var minExpr = context.Min(x, y);

        Assert.That(minExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minExpr.Context, Is.SameAs(context));

        // Test min(5.5m, 2.3m) == 2.3m
        var result = context.Min(context.Real(5.5m), context.Real(2.3m));
        solver.Assert(context.Eq(result, context.Real(2.3m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_RealExpr_CreatesMaximumExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var maxExpr = context.Max(x, y);

        Assert.That(maxExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maxExpr.Context, Is.SameAs(context));

        // Test max(5.5m, 2.3m) == 5.5m
        var result = context.Max(context.Real(5.5m), context.Real(2.3m));
        solver.Assert(context.Eq(result, context.Real(5.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

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
    public void ImplicitIntConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3IntExpr intExpr = 42; // Should work with implicit conversion

        Assert.That(intExpr, Is.Not.Null);
        Assert.That(intExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(intExpr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ImplicitRealConversion_WithSetUp_CreatesExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Z3RealExpr realExpr = 3.14m; // Should work with implicit conversion

        Assert.That(realExpr, Is.Not.Null);
        Assert.That(realExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(realExpr.Context, Is.EqualTo(context));
    }
}