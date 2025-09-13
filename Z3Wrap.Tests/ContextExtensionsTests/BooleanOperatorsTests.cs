using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.ContextExtensionsTests;

[TestFixture]
public class BooleanOperatorsTests
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

        var notTrue = context.Not(context.True());
        solver.Assert(context.Eq(notTrue, context.False()));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Xor_TwoOperands_CreatesBooleanExclusiveOr()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var exclusiveOr = context.Xor(p, q);

        Assert.That(exclusiveOr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(exclusiveOr.Context, Is.SameAs(context));

        var trueXorFalse = context.Xor(context.True(), context.False());
        solver.Assert(trueXorFalse);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Implies_TwoOperands_CreatesBooleanImplication()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var implication = context.Implies(p, q);

        Assert.That(implication.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(implication.Context, Is.SameAs(context));

        var falseImpliesTrue = context.Implies(context.False(), context.True());
        solver.Assert(falseImpliesTrue);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Iff_TwoOperands_CreatesBooleanBiconditional()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var biconditional = context.Iff(p, q);

        Assert.That(biconditional.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(biconditional.Context, Is.SameAs(context));

        var trueIffTrue = context.Iff(context.True(), context.True());
        solver.Assert(trueIffTrue);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ite_GenericVersion_CreatesConditionalExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var condition = context.BoolConst("condition");
        var thenValue = context.IntConst("thenValue");
        var elseValue = context.IntConst("elseValue");
        
        var conditional = context.Ite(condition, thenValue, elseValue);

        Assert.That(conditional.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(conditional.Context, Is.SameAs(context));
        Assert.That(conditional, Is.InstanceOf<Z3IntExpr>());

        var result = context.Ite(context.True(), context.Int(5), context.Int(10));
        solver.Assert(context.Eq(result, context.Int(5)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ite_BaseVersion_CreatesConditionalExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var condition = context.BoolConst("condition");
        var thenValue = context.IntConst("thenValue");
        var elseValue = context.IntConst("elseValue");
        
        var conditional = context.Ite(condition, (Z3Expr)thenValue, elseValue);

        Assert.That(conditional.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(conditional.Context, Is.SameAs(context));

        var result = context.Ite(context.False(), context.Int(5), context.Int(10));
        solver.Assert(context.Eq(result, context.Int(10)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolOperators_CombinedOperations_CreatesSatisfiableSolution()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");
        
        var leftSide = context.Not(context.And(p, q));
        var rightSide = context.Or(context.Not(p), context.Not(q));
        var deMorgan = context.Iff(leftSide, rightSide);
        
        solver.Assert(deMorgan);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}