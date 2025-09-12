using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.ExpressionTests;

[TestFixture]
public class Z3BoolExprTests : IDisposable
{
    private Z3Context context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new Z3Context();
    }

    [TearDown]
    public void TearDown()
    {
        context?.Dispose();
    }

    public void Dispose() => context?.Dispose();

    #region Operator Tests

    [Test]
    public void AndOperator_LogicalConjunction_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p & q;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OrOperator_LogicalDisjunction_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p | q;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(!q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void XorOperator_ExclusiveOr_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p ^ q;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        // Test true XOR false = true
        solver.Assert(p);
        solver.Assert(!q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        using var solver2 = context.CreateSolver();
        // Test true XOR true = false
        solver2.Assert(p);
        solver2.Assert(q);
        solver2.Assert(result);
        
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void NotOperator_LogicalNegation_Works()
    {
        var p = context.BoolConst("p");
        
        var result = !p;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(result);
        
        // p and NOT p should be unsatisfiable
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    #endregion

    #region Instance Method Tests

    [Test]
    public void And_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p.And(q);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Or_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p.Or(q);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(!p);
        solver.Assert(q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Not_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        
        var result = p.Not();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Xor_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p.Xor(q);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        // Test true XOR false = true
        solver.Assert(p);
        solver.Assert(!q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        using var solver2 = context.CreateSolver();
        // Test false XOR false = false
        solver2.Assert(!p);
        solver2.Assert(!q);
        solver2.Assert(result);
        
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Implies_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p.Implies(q);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        // Test false implies anything = true
        solver.Assert(!p);
        solver.Assert(q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        using var solver2 = context.CreateSolver();
        // Test true implies false = false
        solver2.Assert(p);
        solver2.Assert(!q);
        solver2.Assert(result);
        
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Iff_InstanceMethod_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        var result = p.Iff(q);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        // Test true iff true = true
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(result);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        using var solver2 = context.CreateSolver();
        // Test true iff false = false
        solver2.Assert(p);
        solver2.Assert(!q);
        solver2.Assert(result);
        
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    #endregion

    #region If<T> Method Tests

    [Test]
    public void If_WithIntegerExpressions_Works()
    {
        var condition = context.BoolConst("condition");
        var thenValue = context.IntConst("thenValue");
        var elseValue = context.IntConst("elseValue");
        
        var result = condition.If(thenValue, elseValue);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3IntExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(condition);
        solver.Assert(thenValue == context.Int(42));
        solver.Assert(elseValue == context.Int(99));
        solver.Assert(result == context.Int(42)); // Should select thenValue
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithRealExpressions_Works()
    {
        var condition = context.BoolConst("condition");
        var thenValue = context.RealConst("thenValue");
        var elseValue = context.RealConst("elseValue");
        
        var result = condition.If(thenValue, elseValue);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3RealExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(!condition); // False condition
        solver.Assert(thenValue == context.Real(1.5m));
        solver.Assert(elseValue == context.Real(2.5m));
        solver.Assert(result == context.Real(2.5m)); // Should select elseValue
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithBooleanExpressions_Works()
    {
        var condition = context.BoolConst("condition");
        var thenValue = context.BoolConst("thenValue");
        var elseValue = context.BoolConst("elseValue");
        
        var result = condition.If(thenValue, elseValue);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        
        using var solver = context.CreateSolver();
        solver.Assert(condition);
        solver.Assert(thenValue);
        solver.Assert(!elseValue);
        solver.Assert(result); // Should be true (from thenValue)
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithLiteralValues_Works()
    {
        var condition = context.Bool(true);
        var result = condition.If(context.Int(100), context.Int(200));
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3IntExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(result == context.Int(100)); // Should select first value
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Complex Expression Tests

    [Test]
    public void ComplexBooleanLogic_CombinedOperators_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");
        
        // Test: (p & q) | (r ^ !p)
        var expr = (p & q) | (r ^ !p);
        
        Assert.That(expr, Is.Not.Null);
        Assert.That(expr, Is.TypeOf<Z3BoolExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(!p); // p is false, so !p = true
        solver.Assert(q);  // q is true (but p & q = false & true = false)
        solver.Assert(!r); // r is false
        // r ^ !p = false ^ true = true
        // (p & q) | (r ^ !p) = false | true = true
        solver.Assert(expr); // Should be true
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_DeMorgansLaw_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        // De Morgan's law: !(p & q) ≡ (!p | !q)
        var left = !(p & q);
        var right = !p | !q;
        var equivalent = left.Iff(right);
        
        using var solver = context.CreateSolver();
        solver.Assert(equivalent);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_Implication_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        // p → q ≡ !p | q
        var implication = p.Implies(q);
        var equivalent = !p | q;
        var isEquivalent = implication.Iff(equivalent);
        
        using var solver = context.CreateSolver();
        solver.Assert(isEquivalent);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_Biconditional_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        // p ↔ q ≡ (p → q) & (q → p)
        var biconditional = p.Iff(q);
        var equivalent = p.Implies(q) & q.Implies(p);
        var isEquivalent = biconditional.Iff(equivalent);
        
        using var solver = context.CreateSolver();
        solver.Assert(isEquivalent);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void XorProperties_CommutativityAndAssociativity_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");
        
        // Commutativity: p ^ q ≡ q ^ p
        var commutative = (p ^ q).Iff(q ^ p);
        
        // Associativity: (p ^ q) ^ r ≡ p ^ (q ^ r)  
        var associative = ((p ^ q) ^ r).Iff(p ^ (q ^ r));
        
        using var solver = context.CreateSolver();
        solver.Assert(commutative);
        solver.Assert(associative);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ConditionalExpressions_NestedIf_Works()
    {
        var cond1 = context.BoolConst("cond1");
        var cond2 = context.BoolConst("cond2");
        
        // Nested conditional: if cond1 then (if cond2 then 1 else 2) else 3
        var nested = cond1.If(
            cond2.If(context.Int(1), context.Int(2)),
            context.Int(3)
        );
        
        Assert.That(nested, Is.TypeOf<Z3IntExpr>());
        
        using var solver = context.CreateSolver();
        solver.Assert(cond1);
        solver.Assert(cond2);
        solver.Assert(nested == context.Int(1)); // Should select 1
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        using var solver2 = context.CreateSolver();
        solver2.Assert(!cond1);
        solver2.Assert(nested == context.Int(3)); // Should select 3
        
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Operator Precedence and Associativity Tests

    [Test]
    public void OperatorPrecedence_AndOrNot_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        
        // Test that !p & q is parsed as (!p) & q, not !(p & q)
        var expr1 = !p & q;
        var expr2 = (!p) & q;
        var equivalent = expr1.Iff(expr2);
        
        using var solver = context.CreateSolver();
        solver.Assert(equivalent);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorAssociativity_MultipleOrs_Works()
    {
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");
        
        // Test that p | q | r works correctly
        var expr = p | q | r;
        
        using var solver = context.CreateSolver();
        solver.Assert(!p);
        solver.Assert(!q);
        solver.Assert(r);
        solver.Assert(expr); // Should be true because r is true
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Truth Table Tests

    [Test]
    public void XorTruthTable_AllCombinations_Work()
    {
        var p = context.Bool(true);
        var q = context.Bool(false);
        
        // true ^ false = true
        using var solver1 = context.CreateSolver();
        solver1.Assert(p ^ q);
        Assert.That(solver1.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        // false ^ true = true
        using var solver2 = context.CreateSolver();
        solver2.Assert(context.Bool(false) ^ context.Bool(true));
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        // true ^ true = false
        using var solver3 = context.CreateSolver();
        solver3.Assert(context.Bool(true) ^ context.Bool(true));
        Assert.That(solver3.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
        
        // false ^ false = false
        using var solver4 = context.CreateSolver();
        solver4.Assert(context.Bool(false) ^ context.Bool(false));
        Assert.That(solver4.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    #endregion
}