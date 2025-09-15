using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3BoolExprTests
{
    #region Operator Tests

    [Test]
    public void AndOperator_LogicalConjunction_Works()
    {
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        solver.Assert(result == context.Int(42));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithRealExpressions_Works()
    {
        using var context = new Z3Context();
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
        solver.Assert(result == context.Real(2.5m));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithBooleanExpressions_Works()
    {
        using var context = new Z3Context();
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
        solver.Assert(result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithLiteralValues_Works()
    {
        using var context = new Z3Context();
        var condition = context.Bool(true);
        var result = condition.If(context.Int(100), context.Int(200));

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3IntExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(result == context.Int(100));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Complex Expression Tests

    [Test]
    public void ComplexBooleanLogic_CombinedOperators_Works()
    {
        using var context = new Z3Context();
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test: (p & q) | (r ^ !p)
        var expr = (p & q) | (r ^ !p);

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(!p); // p is false, so !p = true
        solver.Assert(q); // q is true (but p & q = false & true = false)
        solver.Assert(!r); // r is false
        // r ^ !p = false ^ true = true
        // (p & q) | (r ^ !p) = false | true = true
        solver.Assert(expr);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_DeMorgansLaw_Works()
    {
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        using var context = new Z3Context();
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
        solver.Assert(nested == context.Int(1));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        using var solver2 = context.CreateSolver();
        solver2.Assert(!cond1);
        solver2.Assert(nested == context.Int(3));
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithArrayExpressions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var condition = context.BoolConst("condition");
        var arr1 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr1");
        var arr2 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr2");

        // This should now work with the ExpressionWrapper handling array types
        var result = condition.If(arr1, arr2);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3IntExpr>>());

        using var solver = context.CreateSolver();
        solver.Assert(condition);
        solver.Assert(arr1[0] == context.Int(10));
        solver.Assert(arr2[0] == context.Int(20));
        solver.Assert(result[0] == context.Int(10)); // Should select arr1

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // This should now work without throwing an exception
        var model = solver.GetModel();
        Assert.That(model, Is.Not.Null);

        // Test that the conditional logic works correctly
        using var solver2 = context.CreateSolver();
        solver2.Assert(!condition); // condition is false
        solver2.Assert(arr1[0] == context.Int(10));
        solver2.Assert(arr2[0] == context.Int(20));
        solver2.Assert(result[0] == context.Int(20)); // Should select arr2

        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Operator Precedence and Associativity Tests

    [Test]
    public void OperatorPrecedence_AndOrNot_Works()
    {
        using var context = new Z3Context();
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

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
        using var context = new Z3Context();
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        var expr = p | q | r;

        using var solver = context.CreateSolver();
        solver.Assert(!p);
        solver.Assert(!q);
        solver.Assert(r);
        solver.Assert(expr);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Truth Table Tests

    [Test]
    public void XorTruthTable_AllCombinations_Work()
    {
        using var context = new Z3Context();
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

    #region Bool Comparison Operator Tests

    [Test]
    public void EqualityOperator_BoolExprEqualTrue_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = p == true;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(result);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityOperator_BoolExprEqualFalse_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = p == false;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p); // p is true
        solver.Assert(result); // p == false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void EqualityOperator_TrueEqualBoolExpr_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = true == p;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p);
        solver.Assert(result);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityOperator_BoolExprNotEqualTrue_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = p != true;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p); // p is true
        solver.Assert(result); // p != true

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void InequalityOperator_BoolExprNotEqualFalse_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = p != false;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p); // p is true
        solver.Assert(result); // p != false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityOperator_FalseNotEqualBoolExpr_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");

        var result = false != p;

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());

        using var solver = context.CreateSolver();
        solver.Assert(p); // p is true
        solver.Assert(result); // false != p

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolComparison_ComplexExpression_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test: (p & q) == true and p == true and q != false
        var expr1 = (p & q) == true;
        var expr2 = p == true;
        var expr3 = q != false;

        using var solver = context.CreateSolver();
        solver.Assert(expr1);
        solver.Assert(expr2);
        solver.Assert(expr3);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion
}