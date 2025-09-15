namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class RealExtensionsTests
{
    [Test]
    public void Add_RealExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var sum = context.Add(x, y);

        Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(sum.Context, Is.SameAs(context));

        // Test 2.5m + 3.7m == 6.2m
        var result = context.Add(context.Real(2.5m), context.Real(3.7m));
        solver.Assert(context.Eq(result, context.Real(6.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_RealExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var difference = context.Sub(x, y);

        Assert.That(difference.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(difference.Context, Is.SameAs(context));

        // Test 10.8m - 4.3m == 6.5m
        var result = context.Sub(context.Real(10.8m), context.Real(4.3m));
        solver.Assert(context.Eq(result, context.Real(6.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_RealExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var product = context.Mul(x, y);

        Assert.That(product.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(product.Context, Is.SameAs(context));

        // Test 2.5m * 4.0m == 10.0m
        var result = context.Mul(context.Real(2.5m), context.Real(4.0m));
        solver.Assert(context.Eq(result, context.Real(10.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_RealExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var quotient = context.Div(x, y);

        Assert.That(quotient.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quotient.Context, Is.SameAs(context));

        // Test 15.0m / 3.0m == 5.0m
        var result = context.Div(context.Real(15.0m), context.Real(3.0m));
        solver.Assert(context.Eq(result, context.Real(5.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Add_RealExprWithDecimal_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var addRight = context.Add(x, 1.5m);
        var addLeft = context.Add(2.5m, x);

        Assert.That(addRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(addLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x + 1.5m where x = 3.7m should be 5.2m
        solver.Assert(context.Eq(x, context.Real(3.7m)));
        solver.Assert(context.Eq(addRight, context.Real(5.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 2.5m + x where x = 4.1m should be 6.6m
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.1m)));
        solver.Assert(context.Eq(addLeft, context.Real(6.6m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_RealExprWithDecimal_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var subRight = context.Sub(x, 2.3m);
        var subLeft = context.Sub(10.7m, x);

        Assert.That(subRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(subLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x - 2.3m where x = 7.8m should be 5.5m
        solver.Assert(context.Eq(x, context.Real(7.8m)));
        solver.Assert(context.Eq(subRight, context.Real(5.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 10.7m - x where x = 4.2m should be 6.5m
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.2m)));
        solver.Assert(context.Eq(subLeft, context.Real(6.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_RealExprWithDecimal_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var mulRight = context.Mul(x, 3.0m);
        var mulLeft = context.Mul(1.5m, x);

        Assert.That(mulRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mulLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x * 3.0m where x = 2.5m should be 7.5m
        solver.Assert(context.Eq(x, context.Real(2.5m)));
        solver.Assert(context.Eq(mulRight, context.Real(7.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 1.5m * x where x = 4.0m should be 6.0m
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.0m)));
        solver.Assert(context.Eq(mulLeft, context.Real(6.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_RealExprWithDecimal_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var divRight = context.Div(x, 2.0m);
        var divLeft = context.Div(20.0m, x);

        Assert.That(divRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(divLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x / 2.0m where x = 9.0m should be 4.5m
        solver.Assert(context.Eq(x, context.Real(9.0m)));
        solver.Assert(context.Eq(divRight, context.Real(4.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 20.0m / x where x = 4.0m should be 5.0m
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.0m)));
        solver.Assert(context.Eq(divLeft, context.Real(5.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void UnaryMinus_RealExpr_CreatesNegationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var negation = context.UnaryMinus(x);

        Assert.That(negation.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negation.Context, Is.SameAs(context));

        // Test -3.14m == -3.14m
        var result = context.UnaryMinus(context.Real(3.14m));
        solver.Assert(context.Eq(result, context.Real(-3.14m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Abs_RealExpr_CreatesAbsoluteValueExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var absoluteValue = context.Abs(x);

        Assert.That(absoluteValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(absoluteValue.Context, Is.SameAs(context));

        // Test abs(-3.5m) == 3.5m
        var result = context.Abs(context.Real(-3.5m));
        solver.Assert(context.Eq(result, context.Real(3.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test abs(2.7m) == 2.7m
        solver.Reset();
        var result2 = context.Abs(context.Real(2.7m));
        solver.Assert(context.Eq(result2, context.Real(2.7m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}