using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprComparisonTests
{
    [TestCase(true, true, true, Description = "true == true = true")]
    [TestCase(true, false, false, Description = "true == false = false")]
    [TestCase(false, true, false, Description = "false == true = false")]
    [TestCase(false, false, true, Description = "false == false = true")]
    public void Equality_BoolExprEqualsBool_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test equality operations with concrete boolean values
        var leftExpr = context.Bool(left);
        var rightExpr = context.Bool(right);

        // Test all variations of equality with bool literal
        var resultOperatorBool = leftExpr == rightExpr;           // BoolExpr == BoolExpr (operator)
        var resultOperatorRightLiteral = leftExpr == right;      // BoolExpr == bool (operator)
        var resultOperatorLeftLiteral = left == rightExpr;       // bool == BoolExpr (operator)
        var resultInstanceEq = leftExpr.Eq(rightExpr);           // BoolExpr.Eq(BoolExpr) (instance method)
        var resultInstanceEqLiteral = leftExpr.Eq(right);        // BoolExpr.Eq(bool) (instance method)
        var resultContextEqExpr = context.Eq(leftExpr, rightExpr); // Context.Eq(BoolExpr, BoolExpr) (method)
        var resultContextEqRightBool = context.Eq(leftExpr, right); // Context.Eq(BoolExpr, bool) (method)
        var resultContextEqLeftBool = context.Eq(left, rightExpr);  // Context.Eq(bool, BoolExpr) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorBool), Is.EqualTo(expectedResult), "BoolExpr == BoolExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightLiteral), Is.EqualTo(expectedResult), "BoolExpr == bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftLiteral), Is.EqualTo(expectedResult), "bool == BoolExpr operator failed");
            Assert.That(model.GetBoolValue(resultInstanceEq), Is.EqualTo(expectedResult), "BoolExpr.Eq(BoolExpr) instance method failed");
            Assert.That(model.GetBoolValue(resultInstanceEqLiteral), Is.EqualTo(expectedResult), "BoolExpr.Eq(bool) instance method failed");
            Assert.That(model.GetBoolValue(resultContextEqExpr), Is.EqualTo(expectedResult), "Context.Eq(BoolExpr, BoolExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextEqRightBool), Is.EqualTo(expectedResult), "Context.Eq(BoolExpr, bool) method failed");
            Assert.That(model.GetBoolValue(resultContextEqLeftBool), Is.EqualTo(expectedResult), "Context.Eq(bool, BoolExpr) method failed");
        });
    }

    [TestCase(true, true, false, Description = "true != true = false")]
    [TestCase(true, false, true, Description = "true != false = true")]
    [TestCase(false, true, true, Description = "false != true = true")]
    [TestCase(false, false, false, Description = "false != false = false")]
    public void Inequality_BoolExprNotEqualsBool_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test inequality operations with concrete boolean values
        var leftExpr = context.Bool(left);
        var rightExpr = context.Bool(right);

        // Test all variations of inequality with bool literal
        var resultOperatorBool = leftExpr != rightExpr;          // BoolExpr != BoolExpr (operator)
        var resultOperatorRightLiteral = leftExpr != right;     // BoolExpr != bool (operator)
        var resultOperatorLeftLiteral = left != rightExpr;      // bool != BoolExpr (operator)
        var resultInstanceNeq = leftExpr.Neq(rightExpr);        // BoolExpr.Neq(BoolExpr) (instance method)
        var resultInstanceNeqLiteral = leftExpr.Neq(right);     // BoolExpr.Neq(bool) (instance method)
        var resultContextNeqExpr = context.Neq(leftExpr, rightExpr); // Context.Neq(BoolExpr, BoolExpr) (method)
        var resultContextNeqRightBool = context.Neq(leftExpr, right); // Context.Neq(BoolExpr, bool) (method)
        var resultContextNeqLeftBool = context.Neq(left, rightExpr);  // Context.Neq(bool, BoolExpr) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorBool), Is.EqualTo(expectedResult), "BoolExpr != BoolExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightLiteral), Is.EqualTo(expectedResult), "BoolExpr != bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftLiteral), Is.EqualTo(expectedResult), "bool != BoolExpr operator failed");
            Assert.That(model.GetBoolValue(resultInstanceNeq), Is.EqualTo(expectedResult), "BoolExpr.Neq(BoolExpr) instance method failed");
            Assert.That(model.GetBoolValue(resultInstanceNeqLiteral), Is.EqualTo(expectedResult), "BoolExpr.Neq(bool) instance method failed");
            Assert.That(model.GetBoolValue(resultContextNeqExpr), Is.EqualTo(expectedResult), "Context.Neq(BoolExpr, BoolExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextNeqRightBool), Is.EqualTo(expectedResult), "Context.Neq(BoolExpr, bool) method failed");
            Assert.That(model.GetBoolValue(resultContextNeqLeftBool), Is.EqualTo(expectedResult), "Context.Neq(bool, BoolExpr) method failed");
        });
    }


    [Test]
    public void Equality_WithBoolConstants_AllCombinations_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test p == true (should be equivalent to p)
        var pEqualsTrue = p == true;
        var equivalentToP = pEqualsTrue.Iff(p);

        // Test p == false (should be equivalent to !p)
        var pEqualsFalse = p == false;
        var equivalentToNotP = pEqualsFalse.Iff(!p);

        solver.Assert(equivalentToP);
        solver.Assert(equivalentToNotP);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Inequality_WithBoolConstants_AllCombinations_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test p != true (should be equivalent to !p)
        var pNotEqualsTrue = p != true;
        var equivalentToNotP = pNotEqualsTrue.Iff(!p);

        // Test p != false (should be equivalent to p)
        var pNotEqualsFalse = p != false;
        var equivalentToP = pNotEqualsFalse.Iff(p);

        solver.Assert(equivalentToNotP);
        solver.Assert(equivalentToP);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Equality_ReverseOperands_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test that order doesn't matter for equality with literals
        var expr1 = p == true;
        var expr2 = true == p;
        var expr3 = p != false;
        var expr4 = false != p;

        // All should be equivalent to p
        solver.Assert(expr1.Iff(p));
        solver.Assert(expr2.Iff(p));
        solver.Assert(expr3.Iff(p));
        solver.Assert(expr4.Iff(p));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonOperations_WithVariables_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test scenario where p and q are both true
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(p == true);
        solver.Assert(q != false);
        solver.Assert(context.Eq(p, q));  // Both should be equal (both true)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(p), Is.True);
            Assert.That(model.GetBoolValue(q), Is.True);
        });
    }

    [Test]
    public void ComparisonOperations_WithMixedExpressions_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test complex comparison expressions
        var expr1 = (p & q) == true;     // Conjunction equals true
        var expr2 = (p | q) != false;    // Disjunction not equals false
        var expr3 = (!p) == false;       // Negation equals false (so p is true)

        solver.Assert(expr1);  // p & q must be true, so both p and q are true
        solver.Assert(expr2);  // p | q is not false, so at least one is true (already satisfied)
        solver.Assert(expr3);  // !p is false, so p is true (already satisfied)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(p), Is.True);
            Assert.That(model.GetBoolValue(q), Is.True);
        });
    }

    [Test]
    public void ComparisonChains_MultipleComparisons_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test chained comparisons
        var chain = (p == true) & (q != false) & (r == p) & (context.Eq(q, r));

        solver.Assert(chain);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // All variables should be true
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(p), Is.True);
            Assert.That(model.GetBoolValue(q), Is.True);
            Assert.That(model.GetBoolValue(r), Is.True);
        });
    }

    [Test]
    public void ComparisonNegation_DoubleNegative_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test that !!(p == true) is equivalent to (p == true)
        var expr1 = p == true;
        var expr2 = !!(p == true);
        var equivalence = expr1.Iff(expr2);

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonWithComplexExpressions_NestedLogic_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test complex nested comparisons that should be satisfiable
        // Set up: p=true, q=false, r=true
        solver.Assert(p);       // p is true
        solver.Assert(!q);      // q is false
        solver.Assert(r);       // r is true

        // With p=true, q=false, r=true:
        // - (p & q) == false: (true & false) == false = false == false ✓
        // - (r | !p) != false: (true | false) != false = true != false ✓
        // - (p ^ q) == p: (true ^ false) == true = true == true ✓
        var expr1 = (p & q) == false;
        var expr2 = (r | !p) != false;
        var expr3 = context.Eq(p ^ q, p);

        solver.Assert(expr1);
        solver.Assert(expr2);
        solver.Assert(expr3);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityReflexivity_SelfComparison_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test reflexivity of equality
        var reflexive = context.Eq(p, p);  // p == p should always be true
        solver.Assert(reflexive);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Should work regardless of p's value
        using var solver2 = context.CreateSolver();
        solver2.Assert(!p);
        solver2.Assert(reflexive);
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualitySymmetry_OrderIndependence_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test symmetry: p == q ≡ q == p
        var leftToRight = context.Eq(p, q);
        var rightToLeft = context.Eq(q, p);
        var symmetry = leftToRight.Iff(rightToLeft);

        solver.Assert(symmetry);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityTransitivity_ChainedEquality_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test transitivity: (p == q) & (q == r) → (p == r)
        var eq1 = context.Eq(p, q);
        var eq2 = context.Eq(q, r);
        var eq3 = context.Eq(p, r);
        var transitivity = (eq1 & eq2).Implies(eq3);

        solver.Assert(transitivity);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityProperties_Symmetry_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test symmetry for inequality: p != q ≡ q != p
        var leftToRight = context.Neq(p, q);
        var rightToLeft = context.Neq(q, p);
        var symmetry = leftToRight.Iff(rightToLeft);

        solver.Assert(symmetry);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonWithLogicalEquivalences_DeMorgansLaw_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test that comparison works with De Morgan's equivalences
        var expr1 = !(p & q) == (!p | !q);     // De Morgan's law as equality
        var expr2 = !(p | q) == (!p & !q);     // De Morgan's law as equality

        solver.Assert(expr1);
        solver.Assert(expr2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonEdgeCases_TautologiesAndContradictions_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test tautologies and contradictions
        var tautology1 = (p | !p) == true;         // Always true
        var tautology2 = (p & !p) == false;        // Always true
        var contradiction1 = (p | !p) != true;     // Always false
        var contradiction2 = (p & !p) != false;    // Always false

        solver.Assert(tautology1);
        solver.Assert(tautology2);
        solver.Assert(!contradiction1);  // Assert negation of contradiction
        solver.Assert(!contradiction2);  // Assert negation of contradiction

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}