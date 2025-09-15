namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class BoolExtensionsTests
{
    [Test]
    public void And_TwoOperands_CreatesBooleanConjunction()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var conjunction = context.And(p, q);

        Assert.That(conjunction.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(conjunction.Context, Is.SameAs(context));

        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(conjunction);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Or_TwoOperands_CreatesBooleanDisjunction()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var disjunction = context.Or(p, q);

        Assert.That(disjunction.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(disjunction.Context, Is.SameAs(context));

        solver.Assert(context.Or(context.True(), context.False()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Not_SingleOperand_CreatesBooleanNegation()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var negation = context.Not(p);

        Assert.That(negation.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negation.Context, Is.SameAs(context));

        // Test that not(true) is false
        solver.Assert(context.Eq(context.Not(context.True()), context.False()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Implies_TwoOperands_CreatesImplication()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var implication = context.Implies(p, q);

        Assert.That(implication.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(implication.Context, Is.SameAs(context));

        // Test that false => anything is true
        solver.Assert(context.Implies(context.False(), context.True()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Iff_TwoOperands_CreatesBiconditional()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var biconditional = context.Iff(p, q);

        Assert.That(biconditional.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(biconditional.Context, Is.SameAs(context));

        // Test that true iff true is true
        solver.Assert(context.Iff(context.True(), context.True()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Xor_TwoOperands_CreatesExclusiveOr()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var exclusiveOr = context.Xor(p, q);

        Assert.That(exclusiveOr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(exclusiveOr.Context, Is.SameAs(context));

        // Test that true xor false is true
        solver.Assert(context.Xor(context.True(), context.False()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void True_CreatesConstantTrueValue()
    {
        using var context = new Z3Context();
        var trueExpr = context.True();

        Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(trueExpr.Context, Is.SameAs(context));
    }

    [Test]
    public void False_CreatesConstantFalseValue()
    {
        using var context = new Z3Context();
        var falseExpr = context.False();

        Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(falseExpr.Context, Is.SameAs(context));
    }

    [Test]
    public void BooleanOperators_LogicalLaws()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // De Morgan's laws
        solver.Assert(context.Iff(
            context.Not(context.And(p, q)),
            context.Or(context.Not(p), context.Not(q))
        ));

        solver.Assert(context.Iff(
            context.Not(context.Or(p, q)),
            context.And(context.Not(p), context.Not(q))
        ));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}