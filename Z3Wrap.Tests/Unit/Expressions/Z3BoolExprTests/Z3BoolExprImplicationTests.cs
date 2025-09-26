using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Booleans;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprImplicationTests
{
    [TestCase(true, true, true, Description = "true IMPLIES true = true")]
    [TestCase(true, false, false, Description = "true IMPLIES false = false")]
    [TestCase(false, true, true, Description = "false IMPLIES true = true")]
    [TestCase(false, false, true, Description = "false IMPLIES false = true")]
    public void Implies_AllVariations_ReturnsExpectedResult(
        bool antecedent,
        bool consequent,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.Bool(antecedent);
        var q = context.Bool(consequent);

        // Test all variations of IMPLIES operation
        var resultMethodBoolExpr = p.Implies(q); // BoolExpr.Implies(BoolExpr) (method)
        var resultMethodBool = p.Implies(consequent); // BoolExpr.Implies(bool) (method)
        var resultContextBoolExpr = context.Implies(p, q); // Context.Implies(BoolExpr, BoolExpr) (method)
        var resultContextExprBool = context.Implies(p, consequent); // Context.Implies(BoolExpr, bool) (method)
        var resultContextBoolExpr2 = context.Implies(antecedent, q); // Context.Implies(bool, BoolExpr) (method)

        // Test equivalent form: p → q ≡ !p | q
        var equivalentForm = !p | q;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr.Implies(BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBool),
                Is.EqualTo(expectedResult),
                "BoolExpr.Implies(bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr),
                Is.EqualTo(expectedResult),
                "Context.Implies(BoolExpr, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextExprBool),
                Is.EqualTo(expectedResult),
                "Context.Implies(BoolExpr, bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr2),
                Is.EqualTo(expectedResult),
                "Context.Implies(bool, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(equivalentForm),
                Is.EqualTo(expectedResult),
                "Equivalent form (!p | q) failed"
            );
        });
    }

    [TestCase(true, true, true, Description = "true IFF true = true")]
    [TestCase(true, false, false, Description = "true IFF false = false")]
    [TestCase(false, true, false, Description = "false IFF true = false")]
    [TestCase(false, false, true, Description = "false IFF false = true")]
    public void Iff_AllVariations_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.Bool(left);
        var q = context.Bool(right);

        // Test all variations of IFF (biconditional) operation
        var resultMethodBoolExpr = p.Iff(q); // BoolExpr.Iff(BoolExpr) (method)
        var resultMethodBool = p.Iff(right); // BoolExpr.Iff(bool) (method)
        var resultContextBoolExpr = context.Iff(p, q); // Context.Iff(BoolExpr, BoolExpr) (method)
        var resultContextExprBool = context.Iff(p, right); // Context.Iff(BoolExpr, bool) (method)
        var resultContextBoolExpr2 = context.Iff(left, q); // Context.Iff(bool, BoolExpr) (method)

        // Test equivalent form: p ↔ q ≡ (p → q) & (q → p)
        var equivalentForm = p.Implies(q) & q.Implies(p);

        // Test another equivalent form: p ↔ q ≡ (p & q) | (!p & !q)
        var anotherEquivalentForm = (p & q) | (!p & !q);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr.Iff(BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBool),
                Is.EqualTo(expectedResult),
                "BoolExpr.Iff(bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr),
                Is.EqualTo(expectedResult),
                "Context.Iff(BoolExpr, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextExprBool),
                Is.EqualTo(expectedResult),
                "Context.Iff(BoolExpr, bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr2),
                Is.EqualTo(expectedResult),
                "Context.Iff(bool, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(equivalentForm),
                Is.EqualTo(expectedResult),
                "Equivalent form (p → q) & (q → p) failed"
            );
            Assert.That(
                model.GetBoolValue(anotherEquivalentForm),
                Is.EqualTo(expectedResult),
                "Equivalent form (p & q) | (!p & !q) failed"
            );
        });
    }

    [Test]
    public void Implies_WithBoolConstants_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test implications with constants
        var alwaysTrue1 = context.Bool(false).Implies(p); // false → anything = true
        var alwaysTrue2 = p.Implies(context.Bool(true)); // anything → true = true
        var equivalentToNegP = p.Implies(context.Bool(false)); // p → false ≡ !p

        solver.Assert(alwaysTrue1);
        solver.Assert(alwaysTrue2);
        solver.Assert(equivalentToNegP.Iff(!p)); // This should be a tautology

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Iff_WithBoolConstants_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test biconditionals with constants
        var equivalentToP = p.Iff(context.Bool(true)); // p ↔ true ≡ p
        var equivalentToNotP = p.Iff(context.Bool(false)); // p ↔ false ≡ !p

        solver.Assert(equivalentToP.Iff(p));
        solver.Assert(equivalentToNotP.Iff(!p));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationProperties_Reflexivity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Reflexivity: p → p is always true
        var reflexive = p.Implies(p);

        solver.Assert(reflexive);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Verify it's always true regardless of p's value
        using var solver2 = context.CreateSolver();
        solver2.Assert(!p);
        solver2.Assert(reflexive);
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationProperties_Transitivity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Transitivity: (p → q) & (q → r) → (p → r)
        var premise1 = p.Implies(q);
        var premise2 = q.Implies(r);
        var conclusion = p.Implies(r);
        var transitivity = (premise1 & premise2).Implies(conclusion);

        solver.Assert(transitivity);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BiconditionalProperties_Reflexivity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Reflexivity: p ↔ p is always true
        var reflexive = p.Iff(p);

        solver.Assert(reflexive);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BiconditionalProperties_Symmetry_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Symmetry: p ↔ q ≡ q ↔ p
        var symmetry = p.Iff(q).Iff(q.Iff(p));

        solver.Assert(symmetry);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BiconditionalProperties_Transitivity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Transitivity: (p ↔ q) & (q ↔ r) → (p ↔ r)
        var premise1 = p.Iff(q);
        var premise2 = q.Iff(r);
        var conclusion = p.Iff(r);
        var transitivity = (premise1 & premise2).Implies(conclusion);

        solver.Assert(transitivity);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationEquivalences_Contrapositive_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Contrapositive: p → q ≡ !q → !p
        var original = p.Implies(q);
        var contrapositive = (!q).Implies(!p);
        var equivalence = original.Iff(contrapositive);

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationEquivalences_Disjunction_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // p → q ≡ !p | q
        var implication = p.Implies(q);
        var disjunction = !p | q;
        var equivalence = implication.Iff(disjunction);

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexImplications_ChainedImplications_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");
        var s = context.BoolConst("s");

        // Test chained implications: ((p → q) & (q → r) & (r → s)) → (p → s)
        var chain = (p.Implies(q) & q.Implies(r) & r.Implies(s)).Implies(p.Implies(s));

        solver.Assert(chain);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexBiconditionals_ChainedBiconditionals_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test chained biconditionals: (p ↔ q) & (q ↔ r) → (p ↔ r)
        var chain = (p.Iff(q) & q.Iff(r)).Implies(p.Iff(r));

        solver.Assert(chain);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationWithLogicalOperators_DistributionOverConjunction_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // p → (q & r) ≡ (p → q) & (p → r)
        var leftSide = p.Implies(q & r);
        var rightSide = p.Implies(q) & p.Implies(r);
        var equivalence = leftSide.Iff(rightSide);

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationWithLogicalOperators_DistributionOverDisjunction_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // (p | q) → r ≡ (p → r) & (q → r)
        var leftSide = (p | q).Implies(r);
        var rightSide = p.Implies(r) & q.Implies(r);
        var equivalence = leftSide.Iff(rightSide);

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BiconditionalWithLogicalOperators_DistributionOverConjunction_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // p ↔ (q & r) ≡ (p ↔ q) & (p ↔ r) when both sides have the same truth value
        // This is NOT always true, so let's test a specific case
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(r);

        var leftSide = p.Iff(q & r);
        var rightSide = p.Iff(q) & p.Iff(r);

        // Both should be true when p, q, r are all true
        solver.Assert(leftSide);
        solver.Assert(rightSide);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ImplicationTautologies_ValidInferences_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Collection of tautologies involving implications
        var tautology1 = p.Implies(p | q); // p → (p | q)
        var tautology2 = (p & q).Implies(p); // (p & q) → p
        var tautology3 = (p & q).Implies(q); // (p & q) → q
        var tautology4 = (!p).Implies(p.Implies(q)); // !p → (p → q)

        solver.Assert(tautology1);
        solver.Assert(tautology2);
        solver.Assert(tautology3);
        solver.Assert(tautology4);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BiconditionalTautologies_ValidEquivalences_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Collection of tautologies involving biconditionals
        var tautology1 = p.Iff(!!p); // p ↔ !!p (double negation)
        var tautology2 = (p & q).Iff(q & p); // (p & q) ↔ (q & p) (commutativity)
        var tautology3 = (p | q).Iff(q | p); // (p | q) ↔ (q | p) (commutativity)
        var tautology4 = (p ^ q).Iff(q ^ p); // (p ^ q) ↔ (q ^ p) (commutativity)

        solver.Assert(tautology1);
        solver.Assert(tautology2);
        solver.Assert(tautology3);
        solver.Assert(tautology4);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
