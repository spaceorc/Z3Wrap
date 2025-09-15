namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class IntExtensionsTests
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
        using var scope = context.SetUp();
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
    public void Abs_IntExpr_CreatesAbsoluteValueExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
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