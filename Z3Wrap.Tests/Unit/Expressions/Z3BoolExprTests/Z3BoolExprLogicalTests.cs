using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprLogicalTests
{
    [TestCase(true, true, true, Description = "true AND true = true")]
    [TestCase(true, false, false, Description = "true AND false = false")]
    [TestCase(false, true, false, Description = "false AND true = false")]
    [TestCase(false, false, false, Description = "false AND false = false")]
    public void And_AllVariations_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Bool(left);
        var y = context.Bool(right);

        // Test all variations of AND operation
        var resultOperatorBoolExpr = x & y; // BoolExpr & BoolExpr (operator)
        var resultOperatorRightBool = x & right; // BoolExpr & bool (operator)
        var resultOperatorLeftBool = left & y; // bool & BoolExpr (operator)
        var resultMethodBoolExpr = x.And(y); // BoolExpr.And(BoolExpr) (method)
        var resultMethodBool = x.And(right); // BoolExpr.And(bool) (method)
        var resultContextBoolExpr = context.And(x, y); // Context.And(BoolExpr, BoolExpr) (method)
        var resultContextExprBool = context.And(x, right); // Context.And(BoolExpr, bool) (method)
        var resultContextBoolExpr2 = context.And(left, y); // Context.And(bool, BoolExpr) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = context.Bool(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr & BoolExpr operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBool),
                Is.EqualTo(expectedResult),
                "BoolExpr & bool operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBool),
                Is.EqualTo(expectedResult),
                "bool & BoolExpr operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr.And(BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBool),
                Is.EqualTo(expectedResult),
                "BoolExpr.And(bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr),
                Is.EqualTo(expectedResult),
                "Context.And(BoolExpr, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextExprBool),
                Is.EqualTo(expectedResult),
                "Context.And(BoolExpr, bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr2),
                Is.EqualTo(expectedResult),
                "Context.And(bool, BoolExpr) method failed"
            );
        });
    }

    [TestCase(true, true, true, Description = "true OR true = true")]
    [TestCase(true, false, true, Description = "true OR false = true")]
    [TestCase(false, true, true, Description = "false OR true = true")]
    [TestCase(false, false, false, Description = "false OR false = false")]
    public void Or_AllVariations_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Bool(left);
        var y = context.Bool(right);

        // Test all variations of OR operation
        var resultOperatorBoolExpr = x | y; // BoolExpr | BoolExpr (operator)
        var resultOperatorRightBool = x | right; // BoolExpr | bool (operator)
        var resultOperatorLeftBool = left | y; // bool | BoolExpr (operator)
        var resultMethodBoolExpr = x.Or(y); // BoolExpr.Or(BoolExpr) (method)
        var resultMethodBool = x.Or(right); // BoolExpr.Or(bool) (method)
        var resultContextBoolExpr = context.Or(x, y); // Context.Or(BoolExpr, BoolExpr) (method)
        var resultContextExprBool = context.Or(x, right); // Context.Or(BoolExpr, bool) (method)
        var resultContextBoolExpr2 = context.Or(left, y); // Context.Or(bool, BoolExpr) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr | BoolExpr operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBool),
                Is.EqualTo(expectedResult),
                "BoolExpr | bool operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBool),
                Is.EqualTo(expectedResult),
                "bool | BoolExpr operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBoolExpr),
                Is.EqualTo(expectedResult),
                "BoolExpr.Or(BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBool),
                Is.EqualTo(expectedResult),
                "BoolExpr.Or(bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr),
                Is.EqualTo(expectedResult),
                "Context.Or(BoolExpr, BoolExpr) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextExprBool),
                Is.EqualTo(expectedResult),
                "Context.Or(BoolExpr, bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBoolExpr2),
                Is.EqualTo(expectedResult),
                "Context.Or(bool, BoolExpr) method failed"
            );
        });
    }

    [TestCase(true, false, Description = "NOT true = false")]
    [TestCase(false, true, Description = "NOT false = true")]
    public void Not_AllVariations_ReturnsExpectedResult(bool value, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Bool(value);

        // Test all variations of NOT operation
        var resultOperator = !x; // !Bool (unary operator)
        var resultMethod = x.Not(); // Bool.Not() (method)
        var resultContextMethod = context.Not(x); // Context.Not(Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperator),
                Is.EqualTo(expectedResult),
                "!Bool unary operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethod),
                Is.EqualTo(expectedResult),
                "Bool.Not() method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextMethod),
                Is.EqualTo(expectedResult),
                "Context.Not(Bool) method failed"
            );
        });
    }

    [TestCase(true, true, false, Description = "true XOR true = false")]
    [TestCase(true, false, true, Description = "true XOR false = true")]
    [TestCase(false, true, true, Description = "false XOR true = true")]
    [TestCase(false, false, false, Description = "false XOR false = false")]
    public void Xor_AllVariations_ReturnsExpectedResult(bool left, bool right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Bool(left);
        var y = context.Bool(right);

        // Test all variations of XOR operation
        var resultOperatorBool = x ^ y; // Bool ^ Bool (operator)
        var resultOperatorRightBool = x ^ right; // Bool ^ bool (operator)
        var resultOperatorLeftBool = left ^ y; // bool ^ Bool (operator)
        var resultMethodBool = x.Xor(y); // Bool.Xor(Bool) (method)
        var resultContextMethod = context.Xor(x, y); // Context.Xor(Bool, Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBool),
                Is.EqualTo(expectedResult),
                "Bool ^ Bool operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBool),
                Is.EqualTo(expectedResult),
                "Bool ^ bool operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBool),
                Is.EqualTo(expectedResult),
                "bool ^ Bool operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBool),
                Is.EqualTo(expectedResult),
                "Bool.Xor(Bool) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextMethod),
                Is.EqualTo(expectedResult),
                "Context.Xor(Bool, Bool) method failed"
            );
        });
    }

    [Test]
    public void LogicalOperations_WithBoolConstants_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test mixed operations with constants and variables
        var expr1 = p & true; // Should be equivalent to p
        var expr2 = p | false; // Should be equivalent to p
        var expr3 = p ^ false; // Should be equivalent to p
        var expr4 = p & false; // Should be false

        solver.Assert(p);
        solver.Assert(expr1); // p & true = p (true)
        solver.Assert(expr2); // p | false = p (true)
        solver.Assert(expr3); // p ^ false = p (true)
        solver.Assert(!expr4); // p & false = false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_ShortCircuitEquivalence_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Test that true | anything = true
        var expr1 = true | p;
        solver.Assert(expr1);

        // Test that false & anything = false
        var expr2 = false & p;
        solver.Assert(!expr2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_Associativity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Test associativity of AND: (p & q) & r ≡ p & (q & r)
        var leftAssocAnd = (p & q) & r;
        var rightAssocAnd = p & (q & r);
        var andEquivalent = leftAssocAnd.Iff(rightAssocAnd);

        // Test associativity of OR: (p | q) | r ≡ p | (q | r)
        var leftAssocOr = (p | q) | r;
        var rightAssocOr = p | (q | r);
        var orEquivalent = leftAssocOr.Iff(rightAssocOr);

        // Test associativity of XOR: (p ^ q) ^ r ≡ p ^ (q ^ r)
        var leftAssocXor = (p ^ q) ^ r;
        var rightAssocXor = p ^ (q ^ r);
        var xorEquivalent = leftAssocXor.Iff(rightAssocXor);

        solver.Assert(andEquivalent);
        solver.Assert(orEquivalent);
        solver.Assert(xorEquivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_Commutativity_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test commutativity: p op q ≡ q op p
        var andCommutative = (p & q).Iff(q & p);
        var orCommutative = (p | q).Iff(q | p);
        var xorCommutative = (p ^ q).Iff(q ^ p);

        solver.Assert(andCommutative);
        solver.Assert(orCommutative);
        solver.Assert(xorCommutative);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_DistributiveLaws_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // AND distributes over OR: p & (q | r) ≡ (p & q) | (p & r)
        var andOverOr = (p & (q | r)).Iff((p & q) | (p & r));

        // OR distributes over AND: p | (q & r) ≡ (p | q) & (p | r)
        var orOverAnd = (p | (q & r)).Iff((p | q) & (p | r));

        solver.Assert(andOverOr);
        solver.Assert(orOverAnd);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_DeMorgansLaws_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // De Morgan's laws
        // !(p & q) ≡ (!p | !q)
        var deMorgan1 = (!(p & q)).Iff(!p | !q);

        // !(p | q) ≡ (!p & !q)
        var deMorgan2 = (!(p | q)).Iff(!p & !q);

        solver.Assert(deMorgan1);
        solver.Assert(deMorgan2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_DoubleNegation_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Double negation: !!p ≡ p
        var doubleNeg = (!!p).Iff(p);

        solver.Assert(doubleNeg);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void LogicalOperations_IdentityLaws_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");

        // Identity laws
        var andIdentity = (p & true).Iff(p); // p & true = p
        var orIdentity = (p | false).Iff(p); // p | false = p

        // Annihilation laws
        var andAnnihilation = (p & false).Iff(false); // p & false = false
        var orAnnihilation = (p | true).Iff(true); // p | true = true

        // XOR identity and self-annihilation
        var xorIdentity = (p ^ false).Iff(p); // p ^ false = p
        var xorSelfAnnihilation = (p ^ p).Iff(false); // p ^ p = false

        solver.Assert(andIdentity);
        solver.Assert(orIdentity);
        solver.Assert(andAnnihilation);
        solver.Assert(orAnnihilation);
        solver.Assert(xorIdentity);
        solver.Assert(xorSelfAnnihilation);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
