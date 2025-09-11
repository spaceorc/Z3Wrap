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

        // Test 2.5 + 3.7 == 6.2
        solver.Assert(context.Eq(context.Real(2.5) + context.Real(3.7), context.Real(6.2)));
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

        // Test 10.8 - 4.3 == 6.5
        solver.Assert(context.Eq(context.Real(10.8) - context.Real(4.3), context.Real(6.5)));
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

        // Test 2.5 * 4.0 == 10.0
        solver.Assert(context.Eq(context.Real(2.5) * context.Real(4.0), context.Real(10.0)));
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

        // Test 15.0 / 3.0 == 5.0
        solver.Assert(context.Eq(context.Real(15.0) / context.Real(3.0), context.Real(5.0)));
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

        // Test 2.1 < 2.2 is true
        solver.Assert(context.Real(2.1) < context.Real(2.2));
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

        // Test 5.5 <= 5.5 is true
        solver.Assert(context.Real(5.5) <= context.Real(5.5));
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

        // Test 9.7 > 1.3 is true
        solver.Assert(context.Real(9.7) > context.Real(1.3));
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

        // Test 7.5 >= 7.5 is true
        solver.Assert(context.Real(7.5) >= context.Real(7.5));
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

        // Test -3.14 == -3.14
        solver.Assert(context.Eq(-context.Real(3.14), context.Real(-3.14)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddOperator_RealExprToDouble_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x + 1.5;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x + 1.5 where x = 2.7 equals 4.2
        solver.Assert(context.Eq(x, context.Real(2.7)));
        solver.Assert(context.Eq(result, context.Real(4.2)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubOperator_RealExprToDouble_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x - 2.3;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x - 2.3 where x = 5.8 equals 3.5
        solver.Assert(context.Eq(x, context.Real(5.8)));
        solver.Assert(context.Eq(result, context.Real(3.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulOperator_RealExprToDouble_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x * 3.0;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x * 3.0 where x = 2.5 equals 7.5
        solver.Assert(context.Eq(x, context.Real(2.5)));
        solver.Assert(context.Eq(result, context.Real(7.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivOperator_RealExprToDouble_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x / 2.0;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x / 2.0 where x = 9.0 equals 4.5
        solver.Assert(context.Eq(x, context.Real(9.0)));
        solver.Assert(context.Eq(result, context.Real(4.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtOperator_RealExprToDouble_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x < 10.5;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x < 10.5 where x = 5.2 is true
        solver.Assert(context.Eq(x, context.Real(5.2)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeOperator_RealExprToDouble_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x <= 7.3;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x <= 7.3 where x = 7.3 is true
        solver.Assert(context.Eq(x, context.Real(7.3)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtOperator_RealExprToDouble_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x > 0.0;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x > 0.0 where x = 1.1 is true
        solver.Assert(context.Eq(x, context.Real(1.1)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeOperator_RealExprToDouble_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x >= -3.14;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x >= -3.14 where x = -3.14 is true
        solver.Assert(context.Eq(x, context.Real(-3.14)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddOperator_DoubleToRealExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 2.5 + x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.5 + x where x = 4.1 equals 6.6
        solver.Assert(context.Eq(x, context.Real(4.1)));
        solver.Assert(context.Eq(result, context.Real(6.6)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubOperator_DoubleToRealExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 10.7 - x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 10.7 - x where x = 4.2 equals 6.5
        solver.Assert(context.Eq(x, context.Real(4.2)));
        solver.Assert(context.Eq(result, context.Real(6.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulOperator_DoubleToRealExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 1.5 * x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 1.5 * x where x = 4.0 equals 6.0
        solver.Assert(context.Eq(x, context.Real(4.0)));
        solver.Assert(context.Eq(result, context.Real(6.0)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivOperator_DoubleToRealExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 20.0 / x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 20.0 / x where x = 4.0 equals 5.0
        solver.Assert(context.Eq(x, context.Real(4.0)));
        solver.Assert(context.Eq(result, context.Real(5.0)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtOperator_DoubleToRealExpr_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 5.5 < x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 5.5 < x where x = 10.0 is true
        solver.Assert(context.Eq(x, context.Real(10.0)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeOperator_DoubleToRealExpr_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 8.3 <= x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 8.3 <= x where x = 8.3 is true
        solver.Assert(context.Eq(x, context.Real(8.3)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtOperator_DoubleToRealExpr_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 15.2 > x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 15.2 > x where x = 10.1 is true
        solver.Assert(context.Eq(x, context.Real(10.1)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeOperator_DoubleToRealExpr_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 12.7 >= x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 12.7 >= x where x = 12.7 is true
        solver.Assert(context.Eq(x, context.Real(12.7)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqOperator_RealExprToDouble_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x == 3.14;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x == 3.14 where x = 3.14 is true
        solver.Assert(context.Eq(x, context.Real(3.14)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqOperator_RealExprToDouble_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x != 0.0;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test x != 0.0 where x = 1.0 is true
        solver.Assert(context.Eq(x, context.Real(1.0)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqOperator_DoubleToRealExpr_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = 2.718 == x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test 2.718 == x where x = 2.718 is true
        solver.Assert(context.Eq(x, context.Real(2.718)));
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqOperator_DoubleToRealExpr_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = -1.5 != x;

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test -1.5 != x where x = 0.0 is true
        solver.Assert(context.Eq(x, context.Real(0.0)));
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
        var result = x.Add(1.5);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x + 1.5;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubMethod_Double_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Sub(2.7);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x - 2.7;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulMethod_Double_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Mul(3.5);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x * 3.5;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void DivMethod_Double_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Div(2.0);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x / 2.0;
        solver.Assert(context.Eq(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LtMethod_Double_CreatesLessThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Lt(10.5);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x < 10.5;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LeMethod_Double_CreatesLessThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Le(7.8);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x <= 7.8;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GtMethod_Double_CreatesGreaterThanComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Gt(0.0);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x > 0.0;
        solver.Assert(context.Iff(result, operatorResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GeMethod_Double_CreatesGreaterThanOrEqualComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.Ge(-5.5);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));

        // Test equivalent to operator
        var operatorResult = x >= -5.5;
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

        // Test abs(-3.5) == 3.5
        solver.Assert(context.Eq(x, context.Real(-3.5)));
        solver.Assert(context.Eq(result, context.Real(3.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test abs(2.7) == 2.7
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(2.7)));
        solver.Assert(context.Eq(result, context.Real(2.7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}