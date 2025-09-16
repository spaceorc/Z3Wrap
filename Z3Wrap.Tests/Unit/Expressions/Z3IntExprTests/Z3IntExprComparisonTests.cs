using System.Numerics;

namespace Z3Wrap.Tests.Unit.Expressions.Z3IntExprTests;

[TestFixture]
public class Z3IntExprComparisonTests
{
    [TestCase(3, 5, true, Description = "3 < 5")]
    [TestCase(5, 3, false, Description = "5 not < 3")]
    [TestCase(5, 5, false, Description = "5 not < 5")]
    [TestCase(-5, 3, true, Description = "-5 < 3")]
    [TestCase(-10, -5, true, Description = "-10 < -5")]
    [TestCase(0, 1, true, Description = "0 < 1")]
    public void Lt_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of less than comparison
        var resultOperatorIntExpr = x < y;                           // IntExpr < IntExpr (operator)
        var resultOperatorRightInt = x < rightInt;                   // IntExpr < int (operator)
        var resultOperatorLeftInt = leftInt < y;                     // int < IntExpr (operator)
        var resultOperatorRightBigInt = x < rightBigInt;             // IntExpr < BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt < y;               // BigInteger < IntExpr (operator)
        var resultMethodIntExpr = x.Lt(y);                           // IntExpr.Lt(IntExpr) (method)
        var resultMethodInt = x.Lt(rightInt);                        // IntExpr.Lt(int) (method)
        var resultMethodBigInt = x.Lt(rightBigInt);                  // IntExpr.Lt(BigInteger) (method)
        var resultContextIntExpr = context.Lt(x, y);                 // Context.Lt(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Lt(x, rightInt);         // Context.Lt(IntExpr, int) (method)
        var resultContextLeftInt = context.Lt(leftInt, y);           // Context.Lt(int, IntExpr) (method)
        var resultContextRightBigInt = context.Lt(x, rightBigInt);   // Context.Lt(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Lt(leftBigInt, y);     // Context.Lt(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr < IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr < int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int < IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr < BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger < IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultMethodIntExpr), Is.EqualTo(expected), "IntExpr.Lt(IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultMethodInt), Is.EqualTo(expected), "IntExpr.Lt(int) method failed");
            Assert.That(model.GetBoolValue(resultMethodBigInt), Is.EqualTo(expected), "IntExpr.Lt(BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextIntExpr), Is.EqualTo(expected), "Context.Lt(IntExpr, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightInt), Is.EqualTo(expected), "Context.Lt(IntExpr, int) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftInt), Is.EqualTo(expected), "Context.Lt(int, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightBigInt), Is.EqualTo(expected), "Context.Lt(IntExpr, BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Lt(BigInteger, IntExpr) method failed");
        });
    }

    [TestCase(3, 5, true, Description = "3 <= 5")]
    [TestCase(5, 5, true, Description = "5 <= 5")]
    [TestCase(5, 3, false, Description = "5 not <= 3")]
    [TestCase(-5, 3, true, Description = "-5 <= 3")]
    [TestCase(-10, -5, true, Description = "-10 <= -5")]
    [TestCase(0, 0, true, Description = "0 <= 0")]
    public void Le_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of less than or equal comparison
        var resultOperatorIntExpr = x <= y;                          // IntExpr <= IntExpr (operator)
        var resultOperatorRightInt = x <= rightInt;                  // IntExpr <= int (operator)
        var resultOperatorLeftInt = leftInt <= y;                    // int <= IntExpr (operator)
        var resultOperatorRightBigInt = x <= rightBigInt;            // IntExpr <= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt <= y;              // BigInteger <= IntExpr (operator)
        var resultMethodIntExpr = x.Le(y);                           // IntExpr.Le(IntExpr) (method)
        var resultMethodInt = x.Le(rightInt);                        // IntExpr.Le(int) (method)
        var resultMethodBigInt = x.Le(rightBigInt);                  // IntExpr.Le(BigInteger) (method)
        var resultContextIntExpr = context.Le(x, y);                 // Context.Le(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Le(x, rightInt);         // Context.Le(IntExpr, int) (method)
        var resultContextLeftInt = context.Le(leftInt, y);           // Context.Le(int, IntExpr) (method)
        var resultContextRightBigInt = context.Le(x, rightBigInt);   // Context.Le(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Le(leftBigInt, y);     // Context.Le(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr <= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr <= int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int <= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr <= BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger <= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultMethodIntExpr), Is.EqualTo(expected), "IntExpr.Le(IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultMethodInt), Is.EqualTo(expected), "IntExpr.Le(int) method failed");
            Assert.That(model.GetBoolValue(resultMethodBigInt), Is.EqualTo(expected), "IntExpr.Le(BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextIntExpr), Is.EqualTo(expected), "Context.Le(IntExpr, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightInt), Is.EqualTo(expected), "Context.Le(IntExpr, int) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftInt), Is.EqualTo(expected), "Context.Le(int, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightBigInt), Is.EqualTo(expected), "Context.Le(IntExpr, BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Le(BigInteger, IntExpr) method failed");
        });
    }

    [TestCase(5, 3, true, Description = "5 > 3")]
    [TestCase(3, 5, false, Description = "3 not > 5")]
    [TestCase(5, 5, false, Description = "5 not > 5")]
    [TestCase(3, -5, true, Description = "3 > -5")]
    [TestCase(-5, -10, true, Description = "-5 > -10")]
    [TestCase(1, 0, true, Description = "1 > 0")]
    public void Gt_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of greater than comparison
        var resultOperatorIntExpr = x > y;                           // IntExpr > IntExpr (operator)
        var resultOperatorRightInt = x > rightInt;                   // IntExpr > int (operator)
        var resultOperatorLeftInt = leftInt > y;                     // int > IntExpr (operator)
        var resultOperatorRightBigInt = x > rightBigInt;             // IntExpr > BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt > y;               // BigInteger > IntExpr (operator)
        var resultMethodIntExpr = x.Gt(y);                           // IntExpr.Gt(IntExpr) (method)
        var resultMethodInt = x.Gt(rightInt);                        // IntExpr.Gt(int) (method)
        var resultMethodBigInt = x.Gt(rightBigInt);                  // IntExpr.Gt(BigInteger) (method)
        var resultContextIntExpr = context.Gt(x, y);                 // Context.Gt(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Gt(x, rightInt);         // Context.Gt(IntExpr, int) (method)
        var resultContextLeftInt = context.Gt(leftInt, y);           // Context.Gt(int, IntExpr) (method)
        var resultContextRightBigInt = context.Gt(x, rightBigInt);   // Context.Gt(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Gt(leftBigInt, y);     // Context.Gt(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr > IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr > int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int > IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr > BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger > IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultMethodIntExpr), Is.EqualTo(expected), "IntExpr.Gt(IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultMethodInt), Is.EqualTo(expected), "IntExpr.Gt(int) method failed");
            Assert.That(model.GetBoolValue(resultMethodBigInt), Is.EqualTo(expected), "IntExpr.Gt(BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextIntExpr), Is.EqualTo(expected), "Context.Gt(IntExpr, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightInt), Is.EqualTo(expected), "Context.Gt(IntExpr, int) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftInt), Is.EqualTo(expected), "Context.Gt(int, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightBigInt), Is.EqualTo(expected), "Context.Gt(IntExpr, BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Gt(BigInteger, IntExpr) method failed");
        });
    }

    [TestCase(5, 3, true, Description = "5 >= 3")]
    [TestCase(5, 5, true, Description = "5 >= 5")]
    [TestCase(3, 5, false, Description = "3 not >= 5")]
    [TestCase(3, -5, true, Description = "3 >= -5")]
    [TestCase(-5, -10, true, Description = "-5 >= -10")]
    [TestCase(0, 0, true, Description = "0 >= 0")]
    public void Ge_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of greater than or equal comparison
        var resultOperatorIntExpr = x >= y;                          // IntExpr >= IntExpr (operator)
        var resultOperatorRightInt = x >= rightInt;                  // IntExpr >= int (operator)
        var resultOperatorLeftInt = leftInt >= y;                    // int >= IntExpr (operator)
        var resultOperatorRightBigInt = x >= rightBigInt;            // IntExpr >= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt >= y;              // BigInteger >= IntExpr (operator)
        var resultMethodIntExpr = x.Ge(y);                           // IntExpr.Ge(IntExpr) (method)
        var resultMethodInt = x.Ge(rightInt);                        // IntExpr.Ge(int) (method)
        var resultMethodBigInt = x.Ge(rightBigInt);                  // IntExpr.Ge(BigInteger) (method)
        var resultContextIntExpr = context.Ge(x, y);                 // Context.Ge(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Ge(x, rightInt);         // Context.Ge(IntExpr, int) (method)
        var resultContextLeftInt = context.Ge(leftInt, y);           // Context.Ge(int, IntExpr) (method)
        var resultContextRightBigInt = context.Ge(x, rightBigInt);   // Context.Ge(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Ge(leftBigInt, y);     // Context.Ge(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr >= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr >= int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int >= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr >= BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger >= IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultMethodIntExpr), Is.EqualTo(expected), "IntExpr.Ge(IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultMethodInt), Is.EqualTo(expected), "IntExpr.Ge(int) method failed");
            Assert.That(model.GetBoolValue(resultMethodBigInt), Is.EqualTo(expected), "IntExpr.Ge(BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextIntExpr), Is.EqualTo(expected), "Context.Ge(IntExpr, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightInt), Is.EqualTo(expected), "Context.Ge(IntExpr, int) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftInt), Is.EqualTo(expected), "Context.Ge(int, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightBigInt), Is.EqualTo(expected), "Context.Ge(IntExpr, BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Ge(BigInteger, IntExpr) method failed");
        });
    }

    [TestCase(5, 5, true, Description = "5 == 5")]
    [TestCase(5, 3, false, Description = "5 != 3")]
    [TestCase(-5, -5, true, Description = "-5 == -5")]
    [TestCase(0, 0, true, Description = "0 == 0")]
    [TestCase(-10, 10, false, Description = "-10 != 10")]
    [TestCase(42, 42, true, Description = "42 == 42")]
    public void Eq_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of equality comparison
        var resultOperatorIntExpr = x == y;                          // IntExpr == IntExpr (operator)
        var resultOperatorRightInt = x == rightInt;                  // IntExpr == int (operator)
        var resultOperatorLeftInt = leftInt == y;                    // int == IntExpr (operator)
        var resultOperatorRightBigInt = x == rightBigInt;            // IntExpr == BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt == y;              // BigInteger == IntExpr (operator)
        var resultContextIntExpr = context.Eq(x, y);                 // Context.Eq(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Eq(x, rightInt);         // Context.Eq(IntExpr, int) (method)
        var resultContextLeftInt = context.Eq(leftInt, y);           // Context.Eq(int, IntExpr) (method)
        var resultContextRightBigInt = context.Eq(x, rightBigInt);   // Context.Eq(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Eq(leftBigInt, y);     // Context.Eq(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr == IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr == int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int == IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr == BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger == IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultContextIntExpr), Is.EqualTo(expected), "Context.Eq(IntExpr, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightInt), Is.EqualTo(expected), "Context.Eq(IntExpr, int) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftInt), Is.EqualTo(expected), "Context.Eq(int, IntExpr) method failed");
            Assert.That(model.GetBoolValue(resultContextRightBigInt), Is.EqualTo(expected), "Context.Eq(IntExpr, BigInteger) method failed");
            Assert.That(model.GetBoolValue(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Eq(BigInteger, IntExpr) method failed");
        });
    }

    [TestCase(5, 3, true, Description = "5 != 3")]
    [TestCase(5, 5, false, Description = "5 == 5 (not !=)")]
    [TestCase(-5, 5, true, Description = "-5 != 5")]
    [TestCase(0, 0, false, Description = "0 == 0 (not !=)")]
    [TestCase(-10, 10, true, Description = "-10 != 10")]
    [TestCase(42, 24, true, Description = "42 != 24")]
    public void Ne_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(left);
        var y = context.Int(right);
        var leftInt = left;
        var rightInt = right;
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of inequality comparison
        var resultOperatorIntExpr = x != y;                          // IntExpr != IntExpr (operator)
        var resultOperatorRightInt = x != rightInt;                  // IntExpr != int (operator)
        var resultOperatorLeftInt = leftInt != y;                    // int != IntExpr (operator)
        var resultOperatorRightBigInt = x != rightBigInt;            // IntExpr != BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt != y;              // BigInteger != IntExpr (operator)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultOperatorIntExpr), Is.EqualTo(expected), "IntExpr != IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightInt), Is.EqualTo(expected), "IntExpr != int operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftInt), Is.EqualTo(expected), "int != IntExpr operator failed");
            Assert.That(model.GetBoolValue(resultOperatorRightBigInt), Is.EqualTo(expected), "IntExpr != BigInteger operator failed");
            Assert.That(model.GetBoolValue(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger != IntExpr operator failed");
        });
    }

    [Test]
    public void OperatorMethodEquivalence_Lt_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x < y;
        var methodResult = x.Lt(y);

        // Test that operator and method produce equivalent results
        solver.Assert(context.Iff(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Le_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x <= y;
        var methodResult = x.Le(y);

        solver.Assert(context.Iff(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Gt_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x > y;
        var methodResult = x.Gt(y);

        solver.Assert(context.Iff(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Ge_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x >= y;
        var methodResult = x.Ge(y);

        solver.Assert(context.Iff(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Eq_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x == y;
        var contextResult = context.Eq(x, y);

        solver.Assert(context.Iff(operatorResult, contextResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Ne_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x != y;
        var negatedEq = !(x == y);

        // Test that != is equivalent to !(==)
        solver.Assert(context.Iff(operatorResult, negatedEq));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void PrimitiveComparisons_IntExprWithLiterals_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        // Test various primitive comparison interactions
        var result1 = x < 10;     // IntExpr < int
        var result2 = 5 < x;      // int < IntExpr
        var result3 = x == 42;    // IntExpr == int
        var result4 = 25 == x;    // int == IntExpr
        var result5 = x != 0;     // IntExpr != int
        var result6 = -1 != x;    // int != IntExpr

        // Set x = 7 for testing
        solver.Assert(context.Eq(x, context.Int(7)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result1), Is.True, "x < 10 failed"); // 7 < 10 = true
            Assert.That(model.GetBoolValue(result2), Is.True, "5 < x failed");  // 5 < 7 = true
            Assert.That(model.GetBoolValue(result3), Is.False, "x == 42 failed"); // 7 == 42 = false
            Assert.That(model.GetBoolValue(result4), Is.False, "25 == x failed"); // 25 == 7 = false
            Assert.That(model.GetBoolValue(result5), Is.True, "x != 0 failed");   // 7 != 0 = true
            Assert.That(model.GetBoolValue(result6), Is.True, "-1 != x failed");  // -1 != 7 = true
        });
    }
}