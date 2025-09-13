namespace Z3Wrap.Tests.ContextExtensionsTests;

[TestFixture]
public class ComparisonTests
{
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
    public void Lt_IntExprWithInt_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var ltRight = context.Lt(x, 10);
        var ltLeft = context.Lt(5, x);

        Assert.That(ltRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ltLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x < 10 && 5 < x is satisfiable (x = 6, 7, 8, or 9)
        solver.Assert(ltRight);
        solver.Assert(ltLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_IntExprWithInt_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var leRight = context.Le(x, 5);
        var leLeft = context.Le(5, x);

        Assert.That(leRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(leLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x <= 5 && 5 <= x implies x = 5
        solver.Assert(leRight);
        solver.Assert(leLeft);
        solver.Assert(context.Eq(x, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Gt_IntExprWithInt_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var gtRight = context.Gt(x, 0);
        var gtLeft = context.Gt(100, x);

        Assert.That(gtRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(gtLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x > 0 && 100 > x is satisfiable (x can be 1-99)
        solver.Assert(gtRight);
        solver.Assert(gtLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ge_IntExprWithInt_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var geRight = context.Ge(x, -5);
        var geLeft = context.Ge(5, x);

        Assert.That(geRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(geLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x >= -5 && 5 >= x is satisfiable (x can be -5 to 5)
        solver.Assert(geRight);
        solver.Assert(geLeft);
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
    public void Gt_RealExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var comparison = context.Gt(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 9.8m > 1.2m is satisfiable
        solver.Assert(context.Gt(context.Real(9.8m), context.Real(1.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ge_RealExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var comparison = context.Ge(x, y);

        Assert.That(comparison.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(comparison.Context, Is.SameAs(context));

        // Test 6.5m >= 6.5m is satisfiable
        solver.Assert(context.Ge(context.Real(6.5m), context.Real(6.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lt_RealExprWithDouble_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var ltRight = context.Lt(x, 10.5m);
        var ltLeft = context.Lt(5.5m, x);

        Assert.That(ltRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ltLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x < 10.5m && 5.5m < x is satisfiable
        solver.Assert(ltRight);
        solver.Assert(ltLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_RealExprWithDouble_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var leRight = context.Le(x, 7.3m);
        var leLeft = context.Le(7.3m, x);

        Assert.That(leRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(leLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x <= 7.3m && 7.3m <= x implies x = 7.3m
        solver.Assert(leRight);
        solver.Assert(leLeft);
        solver.Assert(context.Eq(x, context.Real(7.3m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Gt_RealExprWithDouble_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var gtRight = context.Gt(x, 0.0m);
        var gtLeft = context.Gt(100.0m, x);

        Assert.That(gtRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(gtLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x > 0 && 100 > x is satisfiable
        solver.Assert(gtRight);
        solver.Assert(gtLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ge_RealExprWithDouble_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var geRight = context.Ge(x, -3.14m);
        var geLeft = context.Ge(3.14m, x);

        Assert.That(geRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(geLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x >= -3.14m && 3.14m >= x is satisfiable
        solver.Assert(geRight);
        solver.Assert(geLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonOperators_TransitivityTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        
        // Test transitivity: x < y && y < z implies x < z
        solver.Assert(context.Lt(x, y));
        solver.Assert(context.Lt(y, z));
        solver.Assert(context.Lt(x, z));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}