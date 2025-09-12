namespace Z3Wrap.Tests.ExpressionTests;

[TestFixture]
public class Z3RealExprOperatorTests
{
    [Test]
    public void AddOperator_RealExprToRealExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x + y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.5m + 3.7m == 6.2m
        solver.Assert(context.Eq(context.Real(2.5m) + context.Real(3.7m), context.Real(6.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubOperator_RealExprToRealExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x - y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 10.8m - 4.3m == 6.5m
        solver.Assert(context.Eq(context.Real(10.8m) - context.Real(4.3m), context.Real(6.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulOperator_RealExprToRealExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x * y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.5m * 4.0m == 10.0m
        solver.Assert(context.Eq(context.Real(2.5m) * context.Real(4.0m), context.Real(10.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivOperator_RealExprToRealExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x / y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 15.0m / 3.0m == 5.0m
        solver.Assert(context.Eq(context.Real(15.0m) / context.Real(3.0m), context.Real(5.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtOperator_RealExprToRealExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x < y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.1m < 2.2m is true
        solver.Assert(context.Real(2.1m) < context.Real(2.2m));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeOperator_RealExprToRealExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x <= y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 5.5m <= 5.5m is true
        solver.Assert(context.Real(5.5m) <= context.Real(5.5m));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtOperator_RealExprToRealExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x > y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 9.7m > 1.3m is true
        solver.Assert(context.Real(9.7m) > context.Real(1.3m));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeOperator_RealExprToRealExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x >= y;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 7.5m >= 7.5m is true
        solver.Assert(context.Real(7.5m) >= context.Real(7.5m));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void UnaryMinusOperator_RealExpr_CreatesNegationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = -x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test -3.14m == -3.14m
        solver.Assert(context.Eq(-context.Real(3.14m), context.Real(-3.14m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddOperator_RealExprToDouble_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x + 1.5m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x + 1.5m where x = 2.7m equals 4.2m
        solver.Assert(context.Eq(x, context.Real(2.7m)));
        solver.Assert(context.Eq(result, context.Real(4.2m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubOperator_RealExprToDouble_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x - 2.3m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x - 2.3m where x = 5.8m equals 3.5m
        solver.Assert(context.Eq(x, context.Real(5.8m)));
        solver.Assert(context.Eq(result, context.Real(3.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulOperator_RealExprToDouble_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x * 3.0m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x * 3.0m where x = 2.5m equals 7.5m
        solver.Assert(context.Eq(x, context.Real(2.5m)));
        solver.Assert(context.Eq(result, context.Real(7.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivOperator_RealExprToDouble_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x / 2.0m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x / 2.0m where x = 9.0m equals 4.5m
        solver.Assert(context.Eq(x, context.Real(9.0m)));
        solver.Assert(context.Eq(result, context.Real(4.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtOperator_RealExprToDouble_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x < 10.5m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x < 10.5m where x = 5.2m is true
        solver.Assert(context.Eq(x, context.Real(5.2m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeOperator_RealExprToDouble_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x <= 7.3m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x <= 7.3m where x = 7.3m is true
        solver.Assert(context.Eq(x, context.Real(7.3m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtOperator_RealExprToDouble_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x > 0.0m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x > 0.0m where x = 1.1m is true
        solver.Assert(context.Eq(x, context.Real(1.1m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeOperator_RealExprToDouble_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x >= -3.14m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x >= -3.14m where x = -3.14m is true
        solver.Assert(context.Eq(x, context.Real(-3.14m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddOperator_DoubleToRealExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 2.5m + x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.5m + x where x = 4.1m equals 6.6m
        solver.Assert(context.Eq(x, context.Real(4.1m)));
        solver.Assert(context.Eq(result, context.Real(6.6m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubOperator_DoubleToRealExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 10.7m - x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 10.7m - x where x = 4.2m equals 6.5m
        solver.Assert(context.Eq(x, context.Real(4.2m)));
        solver.Assert(context.Eq(result, context.Real(6.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulOperator_DoubleToRealExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 1.5m * x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 1.5m * x where x = 4.0m equals 6.0m
        solver.Assert(context.Eq(x, context.Real(4.0m)));
        solver.Assert(context.Eq(result, context.Real(6.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivOperator_DoubleToRealExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 20.0m / x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 20.0m / x where x = 4.0m equals 5.0m
        solver.Assert(context.Eq(x, context.Real(4.0m)));
        solver.Assert(context.Eq(result, context.Real(5.0m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtOperator_DoubleToRealExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 5.5m < x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 5.5m < x where x = 10.0m is true
        solver.Assert(context.Eq(x, context.Real(10.0m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeOperator_DoubleToRealExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 8.3m <= x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 8.3m <= x where x = 8.3m is true
        solver.Assert(context.Eq(x, context.Real(8.3m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtOperator_DoubleToRealExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 15.2m > x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 15.2m > x where x = 10.1m is true
        solver.Assert(context.Eq(x, context.Real(10.1m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeOperator_DoubleToRealExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 12.7m >= x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 12.7m >= x where x = 12.7m is true
        solver.Assert(context.Eq(x, context.Real(12.7m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqOperator_RealExprToDouble_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x == 3.14m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x == 3.14m where x = 3.14m is true
        solver.Assert(context.Eq(x, context.Real(3.14m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqOperator_RealExprToDouble_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x != 0.0m;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x != 0.0m where x = 1.0m is true
        solver.Assert(context.Eq(x, context.Real(1.0m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqOperator_DoubleToRealExpr_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 2.718m == x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.718m == x where x = 2.718m is true
        solver.Assert(context.Eq(x, context.Real(2.718m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqOperator_DoubleToRealExpr_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = -1.5m != x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test -1.5m != x where x = 0.0m is true
        solver.Assert(context.Eq(x, context.Real(0.0m)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddMethod_RealExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Add(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x + y;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubMethod_RealExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Sub(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x - y;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulMethod_RealExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Mul(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x * y;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivMethod_RealExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Div(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x / y;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtMethod_RealExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Lt(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x < y;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeMethod_RealExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Le(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x <= y;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtMethod_RealExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Gt(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x > y;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeMethod_RealExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var result = x.Ge(y);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x >= y;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddMethod_Double_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Add(1.5m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x + 1.5m;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubMethod_Double_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Sub(2.7m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x - 2.7m;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulMethod_Double_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Mul(3.5m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x * 3.5m;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivMethod_Double_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Div(2.0m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x / 2.0m;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtMethod_Double_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Lt(10.5m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x < 10.5m;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeMethod_Double_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Le(7.8m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x <= 7.8m;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtMethod_Double_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Gt(0.0m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x > 0.0m;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeMethod_Double_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Ge(-5.5m);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x >= -5.5m;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void UnaryMinusMethod_CreatesNegationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.UnaryMinus();

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = -x;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AbsMethod_CreatesAbsoluteValueExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Abs();

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test abs(-3.5m) == 3.5m
        solver.Assert(context.Eq(x, context.Real(-3.5m)));
        solver.Assert(context.Eq(result, context.Real(3.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test abs(2.7m) == 2.7m
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(2.7m)));
        solver.Assert(context.Eq(result, context.Real(2.7m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}