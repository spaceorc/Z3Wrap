using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprEdgeCasesTests
{
    [Test]
    public void ComplexBooleanLogic_CombinedOperators_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Complex expression: (p & q) | (r ^ !p)
        var expr = (p & q) | (r ^ !p);

        Assert.That(expr, Is.Not.Null);
        Assert.That(expr, Is.TypeOf<BoolExpr>());

        // Test case: p=false, q=true, r=false
        // !p = true, so r ^ !p = false ^ true = true
        // p & q = false & true = false
        // (p & q) | (r ^ !p) = false | true = true
        solver.Assert(!p);
        solver.Assert(q);
        solver.Assert(!r);
        solver.Assert(expr);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_DeMorgansLaw_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // De Morgan's law: !(p & q) ≡ (!p | !q)
        var left = !(p & q);
        var right = !p | !q;
        var equivalent = left.Iff(right);

        solver.Assert(equivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_Implication_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // p → q ≡ !p | q
        var implication = p.Implies(q);
        var equivalent = !p | q;
        var isEquivalent = implication.Iff(equivalent);

        solver.Assert(isEquivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalEquivalences_Biconditional_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // p ↔ q ≡ (p → q) & (q → p)
        var biconditional = p.Iff(q);
        var equivalent = p.Implies(q) & q.Implies(p);
        var isEquivalent = biconditional.Iff(equivalent);

        solver.Assert(isEquivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void XorProperties_CommutativityAndAssociativity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Commutativity: p ^ q ≡ q ^ p
        var commutative = (p ^ q).Iff(q ^ p);

        // Associativity: (p ^ q) ^ r ≡ p ^ (q ^ r)
        var associative = ((p ^ q) ^ r).Iff(p ^ (q ^ r));

        solver.Assert(commutative);
        solver.Assert(associative);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ConditionalExpressions_NestedIf_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var cond1 = context.BoolConst("cond1");
        var cond2 = context.BoolConst("cond2");

        // Nested conditional: if cond1 then (if cond2 then 1 else 2) else 3
        var nested = cond1.Ite(cond2.Ite(context.Int(1), context.Int(2)), context.Int(3));

        Assert.That(nested, Is.TypeOf<IntExpr>());

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
    public void OperatorPrecedence_AndOrNot_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test that negation has higher precedence than conjunction
        var expr1 = !p & q;
        var expr2 = (!p) & q;
        var equivalent = expr1.Iff(expr2);

        solver.Assert(equivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorAssociativity_MultipleOrs_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        var expr = p | q | r;

        solver.Assert(!p);
        solver.Assert(!q);
        solver.Assert(r);
        solver.Assert(expr);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void XorTruthTable_AllCombinations_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

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

    [Test]
    public void QuantifierSimulation_ForAllPattern_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Simulate "for all x in {true, false}, P(x)" pattern
        var p_true = context.BoolConst("P_true");
        var p_false = context.BoolConst("P_false");

        // "For all boolean values, P holds" ≡ P(true) & P(false)
        var forAll = p_true & p_false;

        solver.Assert(forAll);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(p_true), Is.True);
            Assert.That(model.GetBoolValue(p_false), Is.True);
        });
    }

    [Test]
    public void QuantifierSimulation_ExistsPattern_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Simulate "exists x in {true, false}, P(x)" pattern
        var p_true = context.BoolConst("P_true");
        var p_false = context.BoolConst("P_false");

        // "There exists a boolean value for which P holds" ≡ P(true) | P(false)
        var exists = p_true | p_false;

        solver.Assert(exists);
        solver.Assert(!p_true); // Force P(true) to be false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(p_true), Is.False);
            Assert.That(model.GetBoolValue(p_false), Is.True); // P(false) must be true for exists to hold
        });
    }

    [Test]
    public void BooleanSatisfiabilityProblems_3SAT_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x1 = context.BoolConst("x1");
        var x2 = context.BoolConst("x2");
        var x3 = context.BoolConst("x3");

        // 3-SAT problem: (x1 | x2 | !x3) & (!x1 | x2 | x3) & (x1 | !x2 | x3)
        var clause1 = x1 | x2 | !x3;
        var clause2 = !x1 | x2 | x3;
        var clause3 = x1 | !x2 | x3;

        var formula = clause1 & clause2 & clause3;

        solver.Assert(formula);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var val1 = model.GetBoolValue(x1);
        var val2 = model.GetBoolValue(x2);
        var val3 = model.GetBoolValue(x3);

        // Verify the solution manually
        var c1Result = val1 || val2 || !val3;
        var c2Result = !val1 || val2 || val3;
        var c3Result = val1 || !val2 || val3;

        Assert.That(c1Result && c2Result && c3Result, Is.True);
    }

    [Test]
    public void BooleanSatisfiabilityProblems_UnsatisfiableSAT_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Unsatisfiable: p & !p
        var contradiction = p & !p;

        solver.Assert(contradiction);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void ComplexLogicalCircuits_HalfAdder_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BoolConst("a");
        var b = context.BoolConst("b");

        // Half adder logic
        var sum = a ^ b; // XOR for sum
        var carry = a & b; // AND for carry

        // Test all combinations
        var testCases = new[]
        {
            (false, false, false, false), // 0+0=0, carry=0
            (false, true, true, false), // 0+1=1, carry=0
            (true, false, true, false), // 1+0=1, carry=0
            (true, true, false, true), // 1+1=0, carry=1
        };

        foreach (var (aVal, bVal, expectedSum, expectedCarry) in testCases)
        {
            using var testSolver = context.CreateSolver();
            testSolver.Assert(a == aVal);
            testSolver.Assert(b == bVal);
            testSolver.Assert(sum == expectedSum);
            testSolver.Assert(carry == expectedCarry);

            Assert.That(
                testSolver.Check(),
                Is.EqualTo(Z3Status.Satisfiable),
                $"Half adder failed for a={aVal}, b={bVal}"
            );
        }
    }

    [Test]
    public void ComplexLogicalCircuits_Multiplexer2To1_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var input0 = context.BoolConst("input0");
        var input1 = context.BoolConst("input1");
        var select = context.BoolConst("select");

        // 2-to-1 multiplexer: output = select ? input1 : input0
        var output = select.Ite(input1, input0);

        // Test all combinations
        var testCases = new[]
        {
            (false, false, false, false), // select=0, in0=0, in1=0 -> out=0 (select in0)
            (true, false, false, true), // select=0, in0=1, in1=0 -> out=1 (select in0)
            (false, true, false, false), // select=0, in0=0, in1=1 -> out=0 (select in0)
            (true, true, false, true), // select=0, in0=1, in1=1 -> out=1 (select in0)
            (false, false, true, false), // select=1, in0=0, in1=0 -> out=0 (select in1)
            (true, false, true, false), // select=1, in0=1, in1=0 -> out=0 (select in1)
            (false, true, true, true), // select=1, in0=0, in1=1 -> out=1 (select in1)
            (true, true, true, true), // select=1, in0=1, in1=1 -> out=1 (select in1)
        };

        foreach (var (in0Val, in1Val, selVal, expectedOut) in testCases)
        {
            using var testSolver = context.CreateSolver();
            testSolver.Assert(input0 == in0Val);
            testSolver.Assert(input1 == in1Val);
            testSolver.Assert(select == selVal);
            testSolver.Assert(output == expectedOut);

            Assert.That(
                testSolver.Check(),
                Is.EqualTo(Z3Status.Satisfiable),
                $"Multiplexer failed for in0={in0Val}, in1={in1Val}, sel={selVal}"
            );
        }
    }

    [Test]
    public void BooleanFunctionTautologies_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Collection of tautologies (always true formulas)
        var tautology1 = p | !p; // Law of excluded middle
        var tautology2 = !(p & !p); // Law of non-contradiction
        var tautology3 = p.Implies(p | context.Bool(true)); // Weakening
        var tautology4 = (p & context.Bool(true)).Iff(p); // Identity

        solver.Assert(tautology1);
        solver.Assert(tautology2);
        solver.Assert(tautology3);
        solver.Assert(tautology4);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BooleanFunctionContradictions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var p = context.BoolConst("p");

        // Test individual contradictions separately
        using var solver1 = context.CreateSolver();
        solver1.Assert(p & !p); // Basic contradiction
        Assert.That(solver1.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        using var solver2 = context.CreateSolver();
        solver2.Assert(!(p | !p)); // Negation of tautology
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void BooleanAlgebraSimplification_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test various algebraic simplifications
        var simplification1 = (p & (p | q)).Iff(p); // Absorption
        var simplification2 = (p | (p & q)).Iff(p); // Absorption
        var simplification3 = ((p | q) & (p | !q)).Iff(p); // Resolution
        var simplification4 = (p & q | p & !q).Iff(p); // Distributivity + simplification

        solver.Assert(simplification1);
        solver.Assert(simplification2);
        solver.Assert(simplification3);
        solver.Assert(simplification4);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BooleanPuzzle_KnightsAndKnaves_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Knights always tell the truth, knaves always lie
        var aIsKnight = context.BoolConst("aIsKnight");
        var bIsKnight = context.BoolConst("bIsKnight");

        // A says: "B is a knave"
        var aStatement = !bIsKnight;
        var aStatementTrue = aIsKnight.Iff(aStatement);

        // B says: "A and I are of the same type"
        var bStatement = aIsKnight.Iff(bIsKnight);
        var bStatementTrue = bIsKnight.Iff(bStatement);

        solver.Assert(aStatementTrue);
        solver.Assert(bStatementTrue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var aKnight = model.GetBoolValue(aIsKnight);
        var bKnight = model.GetBoolValue(bIsKnight);

        // A is a knight (tells truth), B is a knave (lies)
        Assert.Multiple(() =>
        {
            Assert.That(aKnight, Is.True, "A should be a knight");
            Assert.That(bKnight, Is.False, "B should be a knave");
        });
    }

    [Test]
    public void RecursiveDefinitionSimulation_BooleanRecurrence_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Simulate recursive definition: f(0) = true, f(n) = !f(n-1) for small n
        var f0 = context.BoolConst("f0");
        var f1 = context.BoolConst("f1");
        var f2 = context.BoolConst("f2");
        var f3 = context.BoolConst("f3");

        // Base case and recurrence
        solver.Assert(f0 == true); // f(0) = true
        solver.Assert(f1 == !f0); // f(1) = !f(0) = false
        solver.Assert(f2 == !f1); // f(2) = !f(1) = true
        solver.Assert(f3 == !f2); // f(3) = !f(2) = false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(f0), Is.True);
            Assert.That(model.GetBoolValue(f1), Is.False);
            Assert.That(model.GetBoolValue(f2), Is.True);
            Assert.That(model.GetBoolValue(f3), Is.False);
        });
    }

    [Test]
    public void BooleanMatrixOperations_LogicalMatrix_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 2x2 boolean matrix operations
        var a11 = context.BoolConst("a11");
        var a12 = context.BoolConst("a12");
        var a21 = context.BoolConst("a21");
        var a22 = context.BoolConst("a22");

        var b11 = context.BoolConst("b11");
        var b12 = context.BoolConst("b12");
        var b21 = context.BoolConst("b21");
        var b22 = context.BoolConst("b22");

        // Logical "multiplication" (AND operation)
        var c11 = a11 & b11;
        var c12 = a12 & b12;
        var c21 = a21 & b21;
        var c22 = a22 & b22;

        // Set up a specific case
        solver.Assert(a11 & !a12 & a21 & !a22); // Matrix A = [[T,F],[T,F]]
        solver.Assert(b11 & b12 & !b21 & b22); // Matrix B = [[T,T],[F,T]]

        // Expected result: C = [[T,F],[F,F]]
        solver.Assert(c11 & !c12 & !c21 & !c22);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
