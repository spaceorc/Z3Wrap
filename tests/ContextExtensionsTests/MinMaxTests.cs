namespace z3lib.Tests.ContextExtensionsTests;

[TestFixture]
public class MinMaxTests
{
    [Test]
    public void Min_IntExpr_ReturnsMinimumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var minimum = context.Min(x, y);

        Assert.That(minimum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minimum.Context, Is.SameAs(context));

        // Test min(5, 10) == 5
        var result = context.Min(context.Int(5), context.Int(10));
        solver.Assert(context.Eq(result, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_IntExpr_ReturnsMaximumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var maximum = context.Max(x, y);

        Assert.That(maximum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maximum.Context, Is.SameAs(context));

        // Test max(5, 10) == 10
        var result = context.Max(context.Int(5), context.Int(10));
        solver.Assert(context.Eq(result, context.Int(10)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Min_RealExpr_ReturnsMinimumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var minimum = context.Min(x, y);

        Assert.That(minimum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minimum.Context, Is.SameAs(context));

        // Test min(2.5, 7.3) == 2.5
        var result = context.Min(context.Real(2.5), context.Real(7.3));
        solver.Assert(context.Eq(result, context.Real(2.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_RealExpr_ReturnsMaximumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var maximum = context.Max(x, y);

        Assert.That(maximum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maximum.Context, Is.SameAs(context));

        // Test max(2.5, 7.3) == 7.3
        var result = context.Max(context.Real(2.5), context.Real(7.3));
        solver.Assert(context.Eq(result, context.Real(7.3)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Min_IntExprWithInt_ReturnsMinimumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var minRight = context.Min(x, 20);
        var minLeft = context.Min(15, x);

        Assert.That(minRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test min(x, 20) where x = 30 should be 20
        solver.Assert(context.Eq(x, context.Int(30)));
        solver.Assert(context.Eq(minRight, context.Int(20)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test min(15, x) where x = 10 should be 10
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(10)));
        solver.Assert(context.Eq(minLeft, context.Int(10)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_IntExprWithInt_ReturnsMaximumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var maxRight = context.Max(x, 20);
        var maxLeft = context.Max(25, x);

        Assert.That(maxRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maxLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test max(x, 20) where x = 15 should be 20
        solver.Assert(context.Eq(x, context.Int(15)));
        solver.Assert(context.Eq(maxRight, context.Int(20)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test max(25, x) where x = 30 should be 30
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(30)));
        solver.Assert(context.Eq(maxLeft, context.Int(30)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Min_RealExprWithDouble_ReturnsMinimumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var minRight = context.Min(x, 5.5);
        var minLeft = context.Min(3.3, x);

        Assert.That(minRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(minLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test min(x, 5.5) where x = 7.2 should be 5.5
        solver.Assert(context.Eq(x, context.Real(7.2)));
        solver.Assert(context.Eq(minRight, context.Real(5.5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test min(3.3, x) where x = 2.1 should be 2.1
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(2.1)));
        solver.Assert(context.Eq(minLeft, context.Real(2.1)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Max_RealExprWithDouble_ReturnsMaximumValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var maxRight = context.Max(x, 4.4);
        var maxLeft = context.Max(6.6, x);

        Assert.That(maxRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(maxLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test max(x, 4.4) where x = 2.2 should be 4.4
        solver.Assert(context.Eq(x, context.Real(2.2)));
        solver.Assert(context.Eq(maxRight, context.Real(4.4)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset and test max(6.6, x) where x = 8.8 should be 8.8
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(8.8)));
        solver.Assert(context.Eq(maxLeft, context.Real(8.8)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MinMax_EqualValues_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // When x == y, both min(x, y) and max(x, y) should equal x (and y)
        solver.Assert(context.Eq(x, y));
        solver.Assert(context.Eq(context.Min(x, y), x));
        solver.Assert(context.Eq(context.Max(x, y), x));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MinMax_Idempotent_PropertyTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        
        // Test idempotency: min(x, x) == x and max(x, x) == x
        solver.Assert(context.Eq(context.Min(x, x), x));
        solver.Assert(context.Eq(context.Max(x, x), x));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MinMax_Commutative_PropertyTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test commutativity: min(x, y) == min(y, x) and max(x, y) == max(y, x)
        solver.Assert(context.Eq(context.Min(x, y), context.Min(y, x)));
        solver.Assert(context.Eq(context.Max(x, y), context.Max(y, x)));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MinMax_Associative_PropertyTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        
        // Test associativity: min(min(x, y), z) == min(x, min(y, z))
        var minLeft = context.Min(context.Min(x, y), z);
        var minRight = context.Min(x, context.Min(y, z));
        solver.Assert(context.Eq(minLeft, minRight));
        
        // Test associativity: max(max(x, y), z) == max(x, max(y, z))
        var maxLeft = context.Max(context.Max(x, y), z);
        var maxRight = context.Max(x, context.Max(y, z));
        solver.Assert(context.Eq(maxLeft, maxRight));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MinMax_BoundingProperty_Test()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test bounding properties: 
        // min(x, y) <= x and min(x, y) <= y
        // x <= max(x, y) and y <= max(x, y)
        var minimum = context.Min(x, y);
        var maximum = context.Max(x, y);
        
        solver.Assert(context.Le(minimum, x));
        solver.Assert(context.Le(minimum, y));
        solver.Assert(context.Le(x, maximum));
        solver.Assert(context.Le(y, maximum));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}