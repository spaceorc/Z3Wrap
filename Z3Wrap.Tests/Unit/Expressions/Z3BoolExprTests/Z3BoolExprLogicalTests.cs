using Z3Wrap.Expressions;

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
        var resultOperatorBool = x & y;                      // Bool & Bool (operator)
        var resultOperatorRightBool = x & right;             // Bool & bool (operator)
        var resultOperatorLeftBool = left & y;               // bool & Bool (operator)
        var resultMethodBool = x.And(y);                     // Bool.And(Bool) (method)
        var resultContextMethod = context.And(x, y);         // Context.And(Bool, Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = context.Bool(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorBool), Is.EqualTo(expectedResult), "Bool & Bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBool), Is.EqualTo(expectedResult), "Bool & bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBool), Is.EqualTo(expectedResult), "bool & Bool operator failed");
            Assert.That(model.GetBoolValue(resultMethodBool), Is.EqualTo(expectedResult), "Bool.And(Bool) method failed");
            Assert.That(model.GetBoolValue(resultContextMethod), Is.EqualTo(expectedResult), "Context.And(Bool, Bool) method failed");
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
        var resultOperatorBool = x | y;                      // Bool | Bool (operator)
        var resultOperatorRightBool = x | right;             // Bool | bool (operator)
        var resultOperatorLeftBool = left | y;               // bool | Bool (operator)
        var resultMethodBool = x.Or(y);                      // Bool.Or(Bool) (method)
        var resultContextMethod = context.Or(x, y);          // Context.Or(Bool, Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorBool), Is.EqualTo(expectedResult), "Bool | Bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBool), Is.EqualTo(expectedResult), "Bool | bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBool), Is.EqualTo(expectedResult), "bool | Bool operator failed");
            Assert.That(model.GetBoolValue(resultMethodBool), Is.EqualTo(expectedResult), "Bool.Or(Bool) method failed");
            Assert.That(model.GetBoolValue(resultContextMethod), Is.EqualTo(expectedResult), "Context.Or(Bool, Bool) method failed");
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
        var resultOperator = !x;                             // !Bool (unary operator)
        var resultMethod = x.Not();                          // Bool.Not() (method)
        var resultContextMethod = context.Not(x);            // Context.Not(Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperator), Is.EqualTo(expectedResult), "!Bool unary operator failed");
            Assert.That(model.GetBoolValue(resultMethod), Is.EqualTo(expectedResult), "Bool.Not() method failed");
            Assert.That(model.GetBoolValue(resultContextMethod), Is.EqualTo(expectedResult), "Context.Not(Bool) method failed");
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
        var resultOperatorBool = x ^ y;                      // Bool ^ Bool (operator)
        var resultOperatorRightBool = x ^ right;             // Bool ^ bool (operator)
        var resultOperatorLeftBool = left ^ y;               // bool ^ Bool (operator)
        var resultMethodBool = x.Xor(y);                     // Bool.Xor(Bool) (method)
        var resultContextMethod = context.Xor(x, y);         // Context.Xor(Bool, Bool) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorBool), Is.EqualTo(expectedResult), "Bool ^ Bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBool), Is.EqualTo(expectedResult), "Bool ^ bool operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBool), Is.EqualTo(expectedResult), "bool ^ Bool operator failed");
            Assert.That(model.GetBoolValue(resultMethodBool), Is.EqualTo(expectedResult), "Bool.Xor(Bool) method failed");
            Assert.That(model.GetBoolValue(resultContextMethod), Is.EqualTo(expectedResult), "Context.Xor(Bool, Bool) method failed");
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
        var expr1 = p & true;    // Should be equivalent to p
        var expr2 = p | false;   // Should be equivalent to p
        var expr3 = p ^ false;   // Should be equivalent to p
        var expr4 = p & false;   // Should be false

        solver.Assert(p);
        solver.Assert(expr1);  // p & true = p (true)
        solver.Assert(expr2);  // p | false = p (true)
        solver.Assert(expr3);  // p ^ false = p (true)
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
        var andIdentity = (p & true).Iff(p);   // p & true = p
        var orIdentity = (p | false).Iff(p);   // p | false = p

        // Annihilation laws
        var andAnnihilation = (p & false).Iff(false);  // p & false = false
        var orAnnihilation = (p | true).Iff(true);     // p | true = true

        // XOR identity and self-annihilation
        var xorIdentity = (p ^ false).Iff(p);          // p ^ false = p
        var xorSelfAnnihilation = (p ^ p).Iff(false);  // p ^ p = false

        solver.Assert(andIdentity);
        solver.Assert(orIdentity);
        solver.Assert(andAnnihilation);
        solver.Assert(orAnnihilation);
        solver.Assert(xorIdentity);
        solver.Assert(xorSelfAnnihilation);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}