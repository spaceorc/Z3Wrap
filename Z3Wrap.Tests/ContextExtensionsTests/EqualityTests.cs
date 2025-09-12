namespace Z3Wrap.Tests.ContextExtensionsTests;

[TestFixture]
public class EqualityTests
{
    [Test]
    public void Eq_GenericExpr_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var equality = context.Eq(x, y);

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        // Test 42 == 42 is satisfiable
        solver.Assert(context.Eq(context.Int(42), context.Int(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Neq_GenericExpr_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var inequality = context.Neq(x, y);

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        // Test 5 != 7 is satisfiable
        solver.Assert(context.Neq(context.Int(5), context.Int(7)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Eq_IntExprWithInt_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var eqRight = context.Eq(x, 100);
        var eqLeft = context.Eq(50, x);

        Assert.That(eqRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(eqLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x == 100 is satisfiable
        solver.Assert(eqRight);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset solver and test 50 == x is satisfiable
        solver.Reset();
        solver.Assert(eqLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Neq_IntExprWithInt_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var neqRight = context.Neq(x, 0);
        var neqLeft = context.Neq(-1, x);

        Assert.That(neqRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(neqLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x != 0 && -1 != x is satisfiable
        solver.Assert(neqRight);
        solver.Assert(neqLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Eq_RealExprWithDouble_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var eqRight = context.Eq(x, 3.14m);
        var eqLeft = context.Eq(2.718m, x);

        Assert.That(eqRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(eqLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x == 3.14m is satisfiable
        solver.Assert(eqRight);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Reset solver and test 2.718m == x is satisfiable
        solver.Reset();
        solver.Assert(eqLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Neq_RealExprWithDouble_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var neqRight = context.Neq(x, 0.0m);
        var neqLeft = context.Neq(-1.5m, x);

        Assert.That(neqRight.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(neqLeft.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test x != 0.0m && -1.5m != x is satisfiable
        solver.Assert(neqRight);
        solver.Assert(neqLeft);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Eq_BooleanExpressions_CreatesEqualityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var equality = context.Eq(p, q);

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        // Test true == true is satisfiable
        solver.Assert(context.Eq(context.True(), context.True()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Neq_BooleanExpressions_CreatesInequalityComparison()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var inequality = context.Neq(p, q);

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        // Test true != false is satisfiable
        solver.Assert(context.Neq(context.True(), context.False()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityOperators_ReflexivityTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        
        // Test reflexivity: x == x is always true
        solver.Assert(context.Eq(x, x));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityOperators_SymmetryTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test symmetry: x == y iff y == x
        var leftRight = context.Eq(x, y);
        var rightLeft = context.Eq(y, x);
        var symmetry = context.Iff(leftRight, rightLeft);
        
        solver.Assert(symmetry);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityOperators_TransitivityTest()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        
        // Test transitivity: x == y && y == z implies x == z
        solver.Assert(context.Eq(x, y));
        solver.Assert(context.Eq(y, z));
        solver.Assert(context.Eq(x, z));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqIsNotEq_LogicalEquivalence()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test that Neq is equivalent to Not(Eq)
        var neq = context.Neq(x, y);
        var notEq = context.Not(context.Eq(x, y));
        var equivalence = context.Iff(neq, notEq);
        
        solver.Assert(equivalence);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}