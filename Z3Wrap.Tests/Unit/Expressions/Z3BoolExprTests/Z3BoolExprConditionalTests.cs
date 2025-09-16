using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprConditionalTests
{
    [TestCase(true, 10, 20, 10, Description = "true ? 10 : 20 = 10")]
    [TestCase(false, 10, 20, 20, Description = "false ? 10 : 20 = 20")]
    [TestCase(true, -5, 100, -5, Description = "true ? -5 : 100 = -5")]
    [TestCase(false, -5, 100, 100, Description = "false ? -5 : 100 = 100")]
    public void If_WithIntegerExpressions_AllVariations_ReturnsExpectedResult(bool conditionValue, int thenValue, int elseValue, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(conditionValue);
        var thenExpr = context.Int(thenValue);
        var elseExpr = context.Int(elseValue);

        // Test all variations of conditional operations with integers
        var resultInstanceMethod = condition.If(thenExpr, elseExpr);    // Bool.If<T>(T, T) (method)
        var resultContextMethod = context.Ite(condition, thenExpr, elseExpr); // Context.Ite(Bool, T, T) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = context.Int(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(resultInstanceMethod), Is.EqualTo(new BigInteger(expectedResult)), "Bool.If<Z3IntExpr> method failed");
            Assert.That(model.GetIntValue(resultContextMethod), Is.EqualTo(new BigInteger(expectedResult)), "Context.Ite method failed");
        });
    }

    [TestCase(true, 1.5, 2.5, 1.5, Description = "true ? 1.5 : 2.5 = 1.5")]
    [TestCase(false, 1.5, 2.5, 2.5, Description = "false ? 1.5 : 2.5 = 2.5")]
    [TestCase(true, -3.14, 3.14, -3.14, Description = "true ? -3.14 : 3.14 = -3.14")]
    [TestCase(false, -3.14, 3.14, 3.14, Description = "false ? -3.14 : 3.14 = 3.14")]
    public void If_WithRealExpressions_AllVariations_ReturnsExpectedResult(bool conditionValue, double thenValue, double elseValue, double expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(conditionValue);
        var thenExpr = context.Real((decimal)thenValue);
        var elseExpr = context.Real((decimal)elseValue);

        // Test all variations of conditional operations with reals
        var resultInstanceMethod = condition.If(thenExpr, elseExpr);    // Bool.If<T>(T, T) (method)
        var resultContextMethod = context.Ite(condition, thenExpr, elseExpr); // Context.Ite(Bool, T, T) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultInstanceMethod), Is.EqualTo(new Real((decimal)expectedResult)), "Bool.If<Z3RealExpr> method failed");
            Assert.That(model.GetRealValue(resultContextMethod), Is.EqualTo(new Real((decimal)expectedResult)), "Context.Ite method failed");
        });
    }

    [TestCase(true, true, false, true, Description = "true ? true : false = true")]
    [TestCase(false, true, false, false, Description = "false ? true : false = false")]
    [TestCase(true, false, true, false, Description = "true ? false : true = false")]
    [TestCase(false, false, true, true, Description = "false ? false : true = true")]
    public void If_WithBooleanExpressions_AllVariations_ReturnsExpectedResult(bool conditionValue, bool thenValue, bool elseValue, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(conditionValue);
        var thenExpr = context.Bool(thenValue);
        var elseExpr = context.Bool(elseValue);

        // Test all variations of conditional operations with booleans
        var resultInstanceMethod = condition.If(thenExpr, elseExpr);    // Bool.If<T>(T, T) (method)
        var resultContextMethod = context.Ite(condition, thenExpr, elseExpr); // Context.Ite(Bool, T, T) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultInstanceMethod), Is.EqualTo(expectedResult), "Bool.If<Z3BoolExpr> method failed");
            Assert.That(model.GetBoolValue(resultContextMethod), Is.EqualTo(expectedResult), "Context.Ite method failed");
        });
    }

    [Test]
    public void If_WithBitVecExpressions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(true);
        var thenExpr = context.BitVec(42, 8);
        var elseExpr = context.BitVec(99, 8);

        var result = condition.If(thenExpr, elseExpr);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3BitVecExpr>());

        solver.Assert(result == context.BitVec(42, 8));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithArrayExpressions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(false);
        var arr1 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr1");
        var arr2 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr2");

        var result = condition.If(arr1, arr2);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3IntExpr>>());

        solver.Assert(arr1[0] == context.Int(10));
        solver.Assert(arr2[0] == context.Int(20));
        solver.Assert(result[0] == context.Int(20)); // Should select arr2 since condition is false

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithVariableConditions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.BoolConst("condition");
        var thenValue = context.IntConst("thenValue");
        var elseValue = context.IntConst("elseValue");

        var result = condition.If(thenValue, elseValue);

        // Test when condition is true
        solver.Assert(condition);
        solver.Assert(thenValue == context.Int(100));
        solver.Assert(elseValue == context.Int(200));
        solver.Assert(result == context.Int(100));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test when condition is false
        using var solver2 = context.CreateSolver();
        solver2.Assert(!condition);
        solver2.Assert(thenValue == context.Int(100));
        solver2.Assert(elseValue == context.Int(200));
        solver2.Assert(result == context.Int(200));

        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_NestedConditionals_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var cond1 = context.BoolConst("cond1");
        var cond2 = context.BoolConst("cond2");
        var cond3 = context.BoolConst("cond3");

        // Nested: if cond1 then (if cond2 then 1 else 2) else (if cond3 then 3 else 4)
        var nested = cond1.If(
            cond2.If(context.Int(1), context.Int(2)),
            cond3.If(context.Int(3), context.Int(4))
        );

        // Test case: cond1=true, cond2=true -> result should be 1
        solver.Assert(cond1);
        solver.Assert(cond2);
        solver.Assert(!cond3); // Shouldn't matter since we take the first branch
        solver.Assert(nested == context.Int(1));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: cond1=false, cond3=true -> result should be 3
        using var solver2 = context.CreateSolver();
        solver2.Assert(!cond1);
        solver2.Assert(cond3);
        solver2.Assert(!cond2); // Shouldn't matter since we take the second branch
        solver2.Assert(nested == context.Int(3));

        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: cond1=false, cond3=false -> result should be 4
        using var solver3 = context.CreateSolver();
        solver3.Assert(!cond1);
        solver3.Assert(!cond3);
        solver3.Assert(nested == context.Int(4));

        Assert.That(solver3.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_ConditionalChains_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var cond1 = x > context.Int(10);
        var cond2 = x > context.Int(5);
        var cond3 = x > context.Int(0);

        // Chained conditionals simulating if-else-if logic
        var result = cond1.If(
            context.Int(100),  // if x > 10 then 100
            cond2.If(
                context.Int(50), // else if x > 5 then 50
                cond3.If(
                    context.Int(10), // else if x > 0 then 10
                    context.Int(0)   // else 0
                )
            )
        );

        // Test x = 15 -> should return 100
        solver.Assert(x == context.Int(15));
        solver.Assert(result == context.Int(100));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test x = 8 -> should return 50
        using var solver2 = context.CreateSolver();
        solver2.Assert(x == context.Int(8));
        solver2.Assert(result == context.Int(50));
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test x = 3 -> should return 10
        using var solver3 = context.CreateSolver();
        solver3.Assert(x == context.Int(3));
        solver3.Assert(result == context.Int(10));
        Assert.That(solver3.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test x = -5 -> should return 0
        using var solver4 = context.CreateSolver();
        solver4.Assert(x == context.Int(-5));
        solver4.Assert(result == context.Int(0));
        Assert.That(solver4.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_WithComplexBooleanConditions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        // Complex condition: (p & q) | (!p & r)
        var complexCondition = (p & q) | (!p & r);
        var result = complexCondition.If(context.Int(42), context.Int(99));

        // Test case where condition should be true: p=true, q=true, r=false
        solver.Assert(p);
        solver.Assert(q);
        solver.Assert(!r);
        solver.Assert(result == context.Int(42));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case where condition should be true: p=false, q=false, r=true
        using var solver2 = context.CreateSolver();
        solver2.Assert(!p);
        solver2.Assert(!q);
        solver2.Assert(r);
        solver2.Assert(result == context.Int(42));

        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case where condition should be false: p=false, q=true, r=false
        using var solver3 = context.CreateSolver();
        solver3.Assert(!p);
        solver3.Assert(q);
        solver3.Assert(!r);
        solver3.Assert(result == context.Int(99));

        Assert.That(solver3.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_ConditionalExpressionAsCondition_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Use a comparison as the condition
        var condition = x > y;
        var result = condition.If(context.Int(100), context.Int(200));

        solver.Assert(x == context.Int(10));
        solver.Assert(y == context.Int(5));
        solver.Assert(result == context.Int(100));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test the opposite case
        using var solver2 = context.CreateSolver();
        solver2.Assert(x == context.Int(3));
        solver2.Assert(y == context.Int(7));
        solver2.Assert(result == context.Int(200));

        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_SameTypeValidation_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var condition = context.Bool(true);

        // Both branches must be the same type
        var intResult = condition.If(context.Int(1), context.Int(2));
        var boolResult = condition.If(context.Bool(true), context.Bool(false));
        var realResult = condition.If(context.Real(1.5m), context.Real(2.5m));

        Assert.Multiple(() =>
        {
            Assert.That(intResult, Is.TypeOf<Z3IntExpr>());
            Assert.That(boolResult, Is.TypeOf<Z3BoolExpr>());
            Assert.That(realResult, Is.TypeOf<Z3RealExpr>());
        });
    }

    [Test]
    public void If_ConditionalEquivalences_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var a = context.IntConst("a");
        var b = context.IntConst("b");

        var conditional = p.If(a, b);

        // Test equivalence: if p then a else b ≡ (p & (result == a)) | (!p & (result == b))
        var equivalence = (p & (conditional == a)) | (!p & (conditional == b));

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_IdentityConditions_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.IntConst("a");
        var b = context.IntConst("b");

        // if true then a else b should be equivalent to a
        var trueCondition = context.Bool(true).If(a, b);
        var trueEquivalence = trueCondition == a;

        // if false then a else b should be equivalent to b
        var falseCondition = context.Bool(false).If(a, b);
        var falseEquivalence = falseCondition == b;

        solver.Assert(trueEquivalence);
        solver.Assert(falseEquivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_ConditionalWithSameBranches_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var a = context.IntConst("a");

        // if p then a else a should be equivalent to a (regardless of p)
        var conditional = p.If(a, a);
        var equivalence = conditional == a;

        solver.Assert(equivalence);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void If_ConditionalComposition_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var a = context.IntConst("a");
        var b = context.IntConst("b");
        var c = context.IntConst("c");

        // Composition: if p then (if q then a else b) else c
        var inner = q.If(a, b);
        var outer = p.If(inner, c);

        // Should be equivalent to: if (p & q) then a else if p then b else c
        var equivalent = (p & q).If(a, p.If(b, c));

        solver.Assert(outer == equivalent);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}