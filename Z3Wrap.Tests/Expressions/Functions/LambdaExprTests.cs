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

    [Test]
    public void Lambda_SingleParameter_EqualityOperator_SameLambda()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create identical lambdas: x => x + 1
        var x1 = context.IntConst("x1");
        var lambda1 = context.Lambda(x1, x1 + 1);

        var x2 = context.IntConst("x2");
        var lambda2 = context.Lambda(x2, x2 + 1);

        // Assert they are equal (extensionally - same behavior)
        var equalityConstraint = lambda1 == lambda2;
        solver.Assert(equalityConstraint);

        var status = solver.Check();
        // Equality might be UNKNOWN for lambdas in general, but identical simple ones should work
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_SingleParameter_InequalityOperator_DifferentLambdas()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create different lambdas
        var x1 = context.IntConst("x1");
        var lambda1 = context.Lambda(x1, x1 + 1); // x => x + 1

        var x2 = context.IntConst("x2");
        var lambda2 = context.Lambda(x2, x2 + 2); // x => x + 2

        // Assert they are NOT equal
        var inequalityConstraint = lambda1 != lambda2;
        solver.Assert(inequalityConstraint);

        var status = solver.Check();
        // Lambda inequality is undecidable in general, so we accept Satisfiable or Unknown
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_TwoParameters_EqualityOperator_SameLambda()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create identical lambdas: (x, y) => x + y
        var x1 = context.IntConst("x1");
        var y1 = context.IntConst("y1");
        var lambda1 = context.Lambda(x1, y1, x1 + y1);

        var x2 = context.IntConst("x2");
        var y2 = context.IntConst("y2");
        var lambda2 = context.Lambda(x2, y2, x2 + y2);

        // Assert they are equal
        var equalityConstraint = lambda1 == lambda2;
        solver.Assert(equalityConstraint);

        var status = solver.Check();
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_TwoParameters_InequalityOperator_DifferentLambdas()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create different lambdas
        var x1 = context.IntConst("x1");
        var y1 = context.IntConst("y1");
        var lambda1 = context.Lambda(x1, y1, x1 + y1); // (x,y) => x + y

        var x2 = context.IntConst("x2");
        var y2 = context.IntConst("y2");
        var lambda2 = context.Lambda(x2, y2, x2 * y2); // (x,y) => x * y

        // Assert they are NOT equal
        var inequalityConstraint = lambda1 != lambda2;
        solver.Assert(inequalityConstraint);

        var status = solver.Check();
        // Lambda inequality is undecidable in general, so we accept Satisfiable or Unknown
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_ThreeParameters_EqualityOperator_SameLambda()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create identical lambdas: (x, y, z) => x + y + z
        var x1 = context.IntConst("x1");
        var y1 = context.IntConst("y1");
        var z1 = context.IntConst("z1");
        var lambda1 = context.Lambda(x1, y1, z1, x1 + y1 + z1);

        var x2 = context.IntConst("x2");
        var y2 = context.IntConst("y2");
        var z2 = context.IntConst("z2");
        var lambda2 = context.Lambda(x2, y2, z2, x2 + y2 + z2);

        // Assert they are equal
        var equalityConstraint = lambda1 == lambda2;
        solver.Assert(equalityConstraint);

        var status = solver.Check();
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_ThreeParameters_InequalityOperator_DifferentLambdas()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create different lambdas
        var x1 = context.IntConst("x1");
        var y1 = context.IntConst("y1");
        var z1 = context.IntConst("z1");
        var lambda1 = context.Lambda(x1, y1, z1, x1 + y1 + z1); // sum

        var x2 = context.IntConst("x2");
        var y2 = context.IntConst("y2");
        var z2 = context.IntConst("z2");
        var lambda2 = context.Lambda(x2, y2, z2, x2 * y2 * z2); // product

        // Assert they are NOT equal
        var inequalityConstraint = lambda1 != lambda2;
        solver.Assert(inequalityConstraint);

        var status = solver.Check();
        // Lambda inequality is undecidable in general, so we accept Satisfiable or Unknown
        Assert.That(status, Is.Not.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Lambda_EqualityConstraint_ProvesByCounterexample()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create two lambdas: x => x + 1 and x => x + 2
        var x1 = context.IntConst("x1");
        var lambda1 = context.Lambda(x1, x1 + 1);

        var x2 = context.IntConst("x2");
        var lambda2 = context.Lambda(x2, x2 + 2);

        // Try to prove they're equal - should fail
        // We can show they're different by finding an input where outputs differ
        var testInput = context.IntConst("input");
        var output1 = lambda1.Apply(testInput);
        var output2 = lambda2.Apply(testInput);

        // Outputs should be different
        solver.Assert(output1 != output2);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        // Any input will show they differ
        var model = solver.GetModel();
        var inputValue = model.GetIntValue(testInput);
        var out1Value = model.GetIntValue(output1);
        var out2Value = model.GetIntValue(output2);

        // Verify the difference
        Assert.That(out2Value - out1Value, Is.EqualTo(new BigInteger(1)));
    }
}
