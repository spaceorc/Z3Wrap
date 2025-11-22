using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Functions;

[TestFixture]
public class LambdaExprTests
{
    [Test]
    public void Lambda_SingleParameter_CreatesAndApplies()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: x => x + 1
        var x = context.IntConst("x");
        var lambda = context.Lambda(x, x + 1);

        // Apply lambda to value
        var result = lambda.Apply(5);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(6)));
    }

    [Test]
    public void Lambda_SingleParameter_VerifiesComputedValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: x => x * 2
        var x = context.IntConst("x");
        var doubleLambda = context.Lambda(x, x * context.Int(2));

        // Apply to value
        var result = doubleLambda.Apply(context.Int(7));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(14)));
    }

    [Test]
    public void Lambda_TwoParameters_CreatesAndApplies()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: (x, y) => x + y
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var addLambda = context.Lambda(x, y, x + y);

        // Apply lambda
        var a = context.Int(3);
        var b = context.Int(7);
        var sum = addLambda.Apply(a, b);

        // Verify sum is 10
        solver.Assert(sum == context.Int(10));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lambda_TwoParameters_VerifiesComputedValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: (x, y) => x * y
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var mulLambda = context.Lambda(x, y, x * y);

        // Apply lambda
        var result = mulLambda.Apply(context.Int(6), context.Int(7));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Lambda_ThreeParameters_CreatesAndApplies()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: (x, y, z) => x + y + z
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        var sumLambda = context.Lambda(x, y, z, x + y + z);

        // Apply lambda
        var result = sumLambda.Apply(context.Int(1), context.Int(2), context.Int(3));

        // Verify sum is 6
        solver.Assert(result == context.Int(6));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lambda_ThreeParameters_VerifiesComputedValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: (x, y, z) => (x + y) * z
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        var lambda = context.Lambda(x, y, z, (x + y) * z);

        // Apply lambda: (2 + 3) * 4 = 20
        var result = lambda.Apply(context.Int(2), context.Int(3), context.Int(4));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(20)));
    }

    [Test]
    public void Lambda_BooleanResult_CreatesPredicate()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create predicate: x => x > 0
        var x = context.IntConst("x");
        var isPositive = context.Lambda(x, x > context.Int(0));

        // Test with positive value
        var result = isPositive.Apply(context.Int(5));

        solver.Assert(result);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lambda_BooleanResult_FailsForNegative()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create predicate: x => x > 0
        var x = context.IntConst("x");
        var isPositive = context.Lambda(x, x > context.Int(0));

        // Test with negative value
        var result = isPositive.Apply(context.Int(-5));

        solver.Assert(result);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_ComplexExpression_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: x => (x * x) + (x * 2) + 1
        var x = context.IntConst("x");
        var quadratic = context.Lambda(x, (x * x) + (x * context.Int(2)) + context.Int(1));

        // Apply to x = 3: (3 * 3) + (3 * 2) + 1 = 9 + 6 + 1 = 16
        var result = quadratic.Apply(context.Int(3));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(16)));
    }

    [Test]
    public void Lambda_AppliedMultipleTimes_MaintainsIndependence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: x => x + 10
        var x = context.IntConst("x");
        var addTen = context.Lambda(x, x + context.Int(10));

        // Apply to different values
        var result1 = addTen.Apply(context.Int(5));
        var result2 = addTen.Apply(context.Int(20));
        var result3 = addTen.Apply(context.Int(100));

        solver.Assert(result1 == context.Int(15));
        solver.Assert(result2 == context.Int(30));
        solver.Assert(result3 == context.Int(110));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lambda_WithVariableArgument_Solves()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: x => x * 2
        var x = context.IntConst("x");
        var doubleLambda = context.Lambda(x, x * context.Int(2));

        // Apply to variable
        var input = context.IntConst("input");
        var output = doubleLambda.Apply(input);

        // Constrain: output = 20, so input must be 10
        solver.Assert(output == context.Int(20));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(input), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void Lambda_TwoParameters_ComplexOperation()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create lambda: (x, y) => (x > y) ? x : y  (max function using ite)
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var maxLambda = context.Lambda(x, y, context.Ite(x > y, x, y));

        // max(7, 3) should be 7
        var result1 = maxLambda.Apply(context.Int(7), context.Int(3));
        // max(2, 9) should be 9
        var result2 = maxLambda.Apply(context.Int(2), context.Int(9));

        solver.Assert(result1 == context.Int(7));
        solver.Assert(result2 == context.Int(9));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
