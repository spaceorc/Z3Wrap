namespace z3lib.Tests.ContextExtensionsTests;

[TestFixture]
public class NumericOperatorsTests
{
    [Test]
    public void Add_IntExpr_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = context.Add(x, y);

        Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(sum.Context, Is.SameAs(context));

        // Test 5 + 3 == 8
        var result = context.Add(context.Int(5), context.Int(3));
        solver.Assert(context.Eq(result, context.Int(8)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_IntExpr_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var difference = context.Sub(x, y);

        Assert.That(difference.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(difference.Context, Is.SameAs(context));

        // Test 10 - 4 == 6
        var result = context.Sub(context.Int(10), context.Int(4));
        solver.Assert(context.Eq(result, context.Int(6)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_IntExpr_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var product = context.Mul(x, y);

        Assert.That(product.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(product.Context, Is.SameAs(context));

        // Test 7 * 6 == 42
        var result = context.Mul(context.Int(7), context.Int(6));
        solver.Assert(context.Eq(result, context.Int(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_IntExpr_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var quotient = context.Div(x, y);

        Assert.That(quotient.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quotient.Context, Is.SameAs(context));

        // Test 15 / 3 == 5
        var result = context.Div(context.Int(15), context.Int(3));
        solver.Assert(context.Eq(result, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mod_IntExpr_CreatesModuloExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var remainder = context.Mod(x, y);

        Assert.That(remainder.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(remainder.Context, Is.SameAs(context));

        // Test 17 % 5 == 2
        var result = context.Mod(context.Int(17), context.Int(5));
        solver.Assert(context.Eq(result, context.Int(2)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Add_IntExprWithInt_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var addRight = context.Add(x, 10);
        var addLeft = context.Add(5, x);

        Assert.That(addRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(addLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x + 10 where x = 20 should be 30
        solver.Assert(context.Eq(x, context.Int(20)));
        solver.Assert(context.Eq(addRight, context.Int(30)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 5 + x where x = 15 should be 20
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(15)));
        solver.Assert(context.Eq(addLeft, context.Int(20)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_IntExprWithInt_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var subRight = context.Sub(x, 7);
        var subLeft = context.Sub(20, x);

        Assert.That(subRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(subLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x - 7 where x = 12 should be 5
        solver.Assert(context.Eq(x, context.Int(12)));
        solver.Assert(context.Eq(subRight, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 20 - x where x = 8 should be 12
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(8)));
        solver.Assert(context.Eq(subLeft, context.Int(12)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_IntExprWithInt_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var mulRight = context.Mul(x, 4);
        var mulLeft = context.Mul(3, x);

        Assert.That(mulRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mulLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x * 4 where x = 6 should be 24
        solver.Assert(context.Eq(x, context.Int(6)));
        solver.Assert(context.Eq(mulRight, context.Int(24)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 3 * x where x = 9 should be 27
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(9)));
        solver.Assert(context.Eq(mulLeft, context.Int(27)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_IntExprWithInt_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var divRight = context.Div(x, 2);
        var divLeft = context.Div(100, x);

        Assert.That(divRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(divLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x / 2 where x = 14 should be 7
        solver.Assert(context.Eq(x, context.Int(14)));
        solver.Assert(context.Eq(divRight, context.Int(7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 100 / x where x = 10 should be 10
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(10)));
        solver.Assert(context.Eq(divLeft, context.Int(10)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mod_IntExprWithInt_CreatesModuloExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var modRight = context.Mod(x, 3);
        var modLeft = context.Mod(23, x);

        Assert.That(modRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(modLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x % 3 where x = 11 should be 2
        solver.Assert(context.Eq(x, context.Int(11)));
        solver.Assert(context.Eq(modRight, context.Int(2)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 23 % x where x = 7 should be 2
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(7)));
        solver.Assert(context.Eq(modLeft, context.Int(2)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

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

        // Test 2.5 + 3.7 == 6.2
        var result = context.Add(context.Real(2.5), context.Real(3.7));
        solver.Assert(context.Eq(result, context.Real(6.2)));
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

        // Test 10.8 - 4.3 == 6.5
        var result = context.Sub(context.Real(10.8), context.Real(4.3));
        solver.Assert(context.Eq(result, context.Real(6.5)));
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

        // Test 2.5 * 4.0 == 10.0
        var result = context.Mul(context.Real(2.5), context.Real(4.0));
        solver.Assert(context.Eq(result, context.Real(10.0)));
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

        // Test 15.0 / 3.0 == 5.0
        var result = context.Div(context.Real(15.0), context.Real(3.0));
        solver.Assert(context.Eq(result, context.Real(5.0)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Add_RealExprWithDouble_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var addRight = context.Add(x, 1.5);
        var addLeft = context.Add(2.5, x);

        Assert.That(addRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(addLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x + 1.5 where x = 3.7 should be 5.2
        solver.Assert(context.Eq(x, context.Real(3.7)));
        solver.Assert(context.Eq(addRight, context.Real(5.2)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 2.5 + x where x = 4.1 should be 6.6
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.1)));
        solver.Assert(context.Eq(addLeft, context.Real(6.6)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_RealExprWithDouble_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var subRight = context.Sub(x, 2.3);
        var subLeft = context.Sub(10.7, x);

        Assert.That(subRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(subLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x - 2.3 where x = 7.8 should be 5.5
        solver.Assert(context.Eq(x, context.Real(7.8)));
        solver.Assert(context.Eq(subRight, context.Real(5.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 10.7 - x where x = 4.2 should be 6.5
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.2)));
        solver.Assert(context.Eq(subLeft, context.Real(6.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_RealExprWithDouble_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var mulRight = context.Mul(x, 3.0);
        var mulLeft = context.Mul(1.5, x);

        Assert.That(mulRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mulLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x * 3.0 where x = 2.5 should be 7.5
        solver.Assert(context.Eq(x, context.Real(2.5)));
        solver.Assert(context.Eq(mulRight, context.Real(7.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 1.5 * x where x = 4.0 should be 6.0
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.0)));
        solver.Assert(context.Eq(mulLeft, context.Real(6.0)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_RealExprWithDouble_CreatesDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var divRight = context.Div(x, 2.0);
        var divLeft = context.Div(20.0, x);

        Assert.That(divRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(divLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x / 2.0 where x = 9.0 should be 4.5
        solver.Assert(context.Eq(x, context.Real(9.0)));
        solver.Assert(context.Eq(divRight, context.Real(4.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test 20.0 / x where x = 4.0 should be 5.0
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(4.0)));
        solver.Assert(context.Eq(divLeft, context.Real(5.0)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void UnaryMinus_IntExpr_CreatesNegationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var negation = context.UnaryMinus(x);

        Assert.That(negation.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negation.Context, Is.SameAs(context));

        // Test -5 == -5
        var result = context.UnaryMinus(context.Int(5));
        solver.Assert(context.Eq(result, context.Int(-5)));
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

        // Test -3.14 == -3.14
        var result = context.UnaryMinus(context.Real(3.14));
        solver.Assert(context.Eq(result, context.Real(-3.14)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Abs_IntExpr_CreatesAbsoluteValueExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var absoluteValue = context.Abs(x);

        Assert.That(absoluteValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(absoluteValue.Context, Is.SameAs(context));

        // Test abs(-7) == 7
        var result = context.Abs(context.Int(-7));
        solver.Assert(context.Eq(result, context.Int(7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test abs(5) == 5
        solver.Reset();
        var result2 = context.Abs(context.Int(5));
        solver.Assert(context.Eq(result2, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Abs_RealExpr_CreatesAbsoluteValueExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var absoluteValue = context.Abs(x);

        Assert.That(absoluteValue.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(absoluteValue.Context, Is.SameAs(context));

        // Test abs(-3.5) == 3.5
        var result = context.Abs(context.Real(-3.5));
        solver.Assert(context.Eq(result, context.Real(3.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test abs(2.7) == 2.7
        solver.Reset();
        var result2 = context.Abs(context.Real(2.7));
        solver.Assert(context.Eq(result2, context.Real(2.7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArithmeticOperators_CommutativeProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test commutativity: x + y == y + x and x * y == y * x
        solver.Assert(context.Eq(context.Add(x, y), context.Add(y, x)));
        solver.Assert(context.Eq(context.Mul(x, y), context.Mul(y, x)));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArithmeticOperators_AssociativeProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        
        // Test associativity: (x + y) + z == x + (y + z)
        var addLeft = context.Add(context.Add(x, y), z);
        var addRight = context.Add(x, context.Add(y, z));
        solver.Assert(context.Eq(addLeft, addRight));
        
        // Test associativity: (x * y) * z == x * (y * z)
        var mulLeft = context.Mul(context.Mul(x, y), z);
        var mulRight = context.Mul(x, context.Mul(y, z));
        solver.Assert(context.Eq(mulLeft, mulRight));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArithmeticOperators_DistributiveProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        
        // Test distributivity: x * (y + z) == (x * y) + (x * z)
        var left = context.Mul(x, context.Add(y, z));
        var right = context.Add(context.Mul(x, y), context.Mul(x, z));
        solver.Assert(context.Eq(left, right));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void UnaryMinus_DoubleNegation()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        
        // Test double negation: -(-x) == x
        var doubleNegation = context.UnaryMinus(context.UnaryMinus(x));
        solver.Assert(context.Eq(doubleNegation, x));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}