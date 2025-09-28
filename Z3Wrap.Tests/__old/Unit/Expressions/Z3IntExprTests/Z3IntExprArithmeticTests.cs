using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.__old.Unit.Expressions.Z3IntExprTests;

[TestFixture]
public class Z3IntExprArithmeticTests
{
    [TestCase(5, 3, 8, Description = "Basic addition")]
    [TestCase(10, 5, 15, Description = "Another addition")]
    [TestCase(-5, 3, -2, Description = "Negative plus positive")]
    [TestCase(0, 0, 0, Description = "Adding zeros")]
    [TestCase(100, -50, 50, Description = "Positive plus negative")]
    [TestCase(-10, -20, -30, Description = "Two negatives")]
    public void Add_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of addition
        var resultOperatorIntExpr = x + y; // IntExpr + IntExpr (operator)
        var resultOperatorRightInt = x + rightInt; // IntExpr + int (operator)
        var resultOperatorLeftInt = leftInt + y; // int + IntExpr (operator)
        var resultOperatorRightBigInt = x + rightBigInt; // IntExpr + BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt + y; // BigInteger + IntExpr (operator)
        var resultMethodIntExpr = x.Add(y); // IntExpr.Add(IntExpr) (method)
        var resultMethodInt = x.Add(rightInt); // IntExpr.Add(int) (method)
        var resultMethodBigInt = x.Add(rightBigInt); // IntExpr.Add(BigInteger) (method)
        var resultContextIntExpr = context.Add(x, y); // Context.Add(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Add(x, rightInt); // Context.Add(IntExpr, int) (method)
        var resultContextLeftInt = context.Add(leftInt, y); // Context.Add(int, IntExpr) (method)
        var resultContextRightBigInt = context.Add(x, rightBigInt); // Context.Add(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Add(leftBigInt, y); // Context.Add(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperatorIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr + IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr + int operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "int + IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr + BigInteger operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "BigInteger + IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Add(IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Add(int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Add(BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Add(IntExpr, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Add(IntExpr, int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Add(int, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Add(IntExpr, BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Add(BigInteger, IntExpr) method failed"
            );
        });
    }

    [TestCase(10, 3, 7, Description = "Basic subtraction")]
    [TestCase(5, 8, -3, Description = "Negative result")]
    [TestCase(0, 1, -1, Description = "Zero minus one")]
    [TestCase(100, 100, 0, Description = "Equal values result in zero")]
    [TestCase(-5, -3, -2, Description = "Negative minus negative")]
    [TestCase(-10, 5, -15, Description = "Negative minus positive")]
    public void Sub_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of subtraction
        var resultOperatorIntExpr = x - y; // IntExpr - IntExpr (operator)
        var resultOperatorRightInt = x - rightInt; // IntExpr - int (operator)
        var resultOperatorLeftInt = leftInt - y; // int - IntExpr (operator)
        var resultOperatorRightBigInt = x - rightBigInt; // IntExpr - BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt - y; // BigInteger - IntExpr (operator)
        var resultMethodIntExpr = x.Sub(y); // IntExpr.Sub(IntExpr) (method)
        var resultMethodInt = x.Sub(rightInt); // IntExpr.Sub(int) (method)
        var resultMethodBigInt = x.Sub(rightBigInt); // IntExpr.Sub(BigInteger) (method)
        var resultContextIntExpr = context.Sub(x, y); // Context.Sub(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Sub(x, rightInt); // Context.Sub(IntExpr, int) (method)
        var resultContextLeftInt = context.Sub(leftInt, y); // Context.Sub(int, IntExpr) (method)
        var resultContextRightBigInt = context.Sub(x, rightBigInt); // Context.Sub(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Sub(leftBigInt, y); // Context.Sub(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperatorIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr - IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr - int operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "int - IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr - BigInteger operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "BigInteger - IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Sub(IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Sub(int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Sub(BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Sub(IntExpr, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Sub(IntExpr, int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Sub(int, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Sub(IntExpr, BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Sub(BigInteger, IntExpr) method failed"
            );
        });
    }

    [TestCase(5, 3, 15, Description = "Basic multiplication")]
    [TestCase(0, 100, 0, Description = "Zero times anything")]
    [TestCase(1, 42, 42, Description = "One times value")]
    [TestCase(-5, 3, -15, Description = "Negative times positive")]
    [TestCase(-4, -7, 28, Description = "Negative times negative")]
    [TestCase(10, -2, -20, Description = "Positive times negative")]
    public void Mul_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of multiplication
        var resultOperatorIntExpr = x * y; // IntExpr * IntExpr (operator)
        var resultOperatorRightInt = x * rightInt; // IntExpr * int (operator)
        var resultOperatorLeftInt = leftInt * y; // int * IntExpr (operator)
        var resultOperatorRightBigInt = x * rightBigInt; // IntExpr * BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt * y; // BigInteger * IntExpr (operator)
        var resultMethodIntExpr = x.Mul(y); // IntExpr.Mul(IntExpr) (method)
        var resultMethodInt = x.Mul(rightInt); // IntExpr.Mul(int) (method)
        var resultMethodBigInt = x.Mul(rightBigInt); // IntExpr.Mul(BigInteger) (method)
        var resultContextIntExpr = context.Mul(x, y); // Context.Mul(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Mul(x, rightInt); // Context.Mul(IntExpr, int) (method)
        var resultContextLeftInt = context.Mul(leftInt, y); // Context.Mul(int, IntExpr) (method)
        var resultContextRightBigInt = context.Mul(x, rightBigInt); // Context.Mul(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Mul(leftBigInt, y); // Context.Mul(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperatorIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr * IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr * int operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "int * IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr * BigInteger operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "BigInteger * IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mul(IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mul(int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mul(BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mul(IntExpr, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mul(IntExpr, int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mul(int, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mul(IntExpr, BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mul(BigInteger, IntExpr) method failed"
            );
        });
    }

    [TestCase(15, 3, 5, Description = "Basic division")]
    [TestCase(10, 2, 5, Description = "Even division")]
    [TestCase(7, 3, 2, Description = "Division with truncation")]
    [TestCase(100, 1, 100, Description = "Division by one")]
    [TestCase(-15, 3, -5, Description = "Negative divided by positive")]
    [TestCase(15, -3, -5, Description = "Positive divided by negative")]
    [TestCase(-15, -3, 5, Description = "Negative divided by negative")]
    public void Div_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of division
        var resultOperatorIntExpr = x / y; // IntExpr / IntExpr (operator)
        var resultOperatorRightInt = x / rightInt; // IntExpr / int (operator)
        var resultOperatorLeftInt = leftInt / y; // int / IntExpr (operator)
        var resultOperatorRightBigInt = x / rightBigInt; // IntExpr / BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt / y; // BigInteger / IntExpr (operator)
        var resultMethodIntExpr = x.Div(y); // IntExpr.Div(IntExpr) (method)
        var resultMethodInt = x.Div(rightInt); // IntExpr.Div(int) (method)
        var resultMethodBigInt = x.Div(rightBigInt); // IntExpr.Div(BigInteger) (method)
        var resultContextIntExpr = context.Div(x, y); // Context.Div(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Div(x, rightInt); // Context.Div(IntExpr, int) (method)
        var resultContextLeftInt = context.Div(leftInt, y); // Context.Div(int, IntExpr) (method)
        var resultContextRightBigInt = context.Div(x, rightBigInt); // Context.Div(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Div(leftBigInt, y); // Context.Div(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperatorIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr / IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr / int operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "int / IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr / BigInteger operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "BigInteger / IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Div(IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Div(int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Div(BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Div(IntExpr, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Div(IntExpr, int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Div(int, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Div(IntExpr, BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Div(BigInteger, IntExpr) method failed"
            );
        });
    }

    [TestCase(17, 5, 2, Description = "Basic modulo")]
    [TestCase(10, 3, 1, Description = "Standard case")]
    [TestCase(5, 10, 5, Description = "Dividend smaller than divisor")]
    [TestCase(15, 3, 0, Description = "Exact division")]
    [TestCase(-17, 5, 3, Description = "Negative modulo positive (Z3 uses Euclidean division)")]
    [TestCase(17, -5, 2, Description = "Positive modulo negative (Z3 uses Euclidean division)")]
    [TestCase(-17, -5, 3, Description = "Negative modulo negative (Z3 uses Euclidean division)")]
    public void Mod_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of modulo
        var resultOperatorIntExpr = x % y; // IntExpr % IntExpr (operator)
        var resultOperatorRightInt = x % rightInt; // IntExpr % int (operator)
        var resultOperatorLeftInt = leftInt % y; // int % IntExpr (operator)
        var resultOperatorRightBigInt = x % rightBigInt; // IntExpr % BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt % y; // BigInteger % IntExpr (operator)
        var resultMethodIntExpr = x.Mod(y); // IntExpr.Mod(IntExpr) (method)
        var resultMethodInt = x.Mod(rightInt); // IntExpr.Mod(int) (method)
        var resultMethodBigInt = x.Mod(rightBigInt); // IntExpr.Mod(BigInteger) (method)
        var resultContextIntExpr = context.Mod(x, y); // Context.Mod(IntExpr, IntExpr) (method)
        var resultContextRightInt = context.Mod(x, rightInt); // Context.Mod(IntExpr, int) (method)
        var resultContextLeftInt = context.Mod(leftInt, y); // Context.Mod(int, IntExpr) (method)
        var resultContextRightBigInt = context.Mod(x, rightBigInt); // Context.Mod(IntExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Mod(leftBigInt, y); // Context.Mod(BigInteger, IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperatorIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr % IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr % int operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "int % IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr % BigInteger operator failed"
            );
            Assert.That(
                model.GetIntValue(resultOperatorLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "BigInteger % IntExpr operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mod(IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mod(int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultMethodBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Mod(BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mod(IntExpr, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mod(IntExpr, int) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mod(int, IntExpr) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mod(IntExpr, BigInteger) method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Mod(BigInteger, IntExpr) method failed"
            );
        });
    }

    [TestCase(42, -42, Description = "Positive value negation")]
    [TestCase(0, 0, Description = "Zero negation")]
    [TestCase(-1, 1, Description = "Negative one")]
    [TestCase(-100, 100, Description = "Negative value")]
    public void Neg_AllVariations_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(value);
        var expected = expectedResult;

        // Test all variations of negation
        var resultOperator = -x; // -IntExpr (unary operator)
        var resultMethod = x.UnaryMinus(); // IntExpr.UnaryMinus() (method)
        var resultContext = context.UnaryMinus(x); // Context.UnaryMinus(IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(value)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultOperator),
                Is.EqualTo(new BigInteger(expected)),
                "-IntExpr unary operator failed"
            );
            Assert.That(
                model.GetIntValue(resultMethod),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.UnaryMinus() method failed"
            );
            Assert.That(
                model.GetIntValue(resultContext),
                Is.EqualTo(new BigInteger(expected)),
                "Context.UnaryMinus(IntExpr) method failed"
            );
        });
    }

    [TestCase(5, 5, Description = "Positive value")]
    [TestCase(-5, 5, Description = "Negative value")]
    [TestCase(0, 0, Description = "Zero")]
    [TestCase(-100, 100, Description = "Negative hundred")]
    [TestCase(42, 42, Description = "Positive answer")]
    public void Abs_AllVariations_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(value);
        var expected = expectedResult;

        // Test all variations of absolute value
        var resultMethod = x.Abs(); // IntExpr.Abs() (method)
        var resultContext = context.Abs(x); // Context.Abs(IntExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(value)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultMethod),
                Is.EqualTo(new BigInteger(expected)),
                "IntExpr.Abs() method failed"
            );
            Assert.That(
                model.GetIntValue(resultContext),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Abs(IntExpr) method failed"
            );
        });
    }

    [Test]
    public void OperatorMethodEquivalence_Add_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x + y;
        var methodResult = x.Add(y);

        // Test that operator and method produce equivalent results
        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Sub_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x - y;
        var methodResult = x.Sub(y);

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Mul_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x * y;
        var methodResult = x.Mul(y);

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Div_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x / y;
        var methodResult = x.Div(y);

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Mod_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var operatorResult = x % y;
        var methodResult = x.Mod(y);

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_UnaryMinus_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        var operatorResult = -x;
        var methodResult = x.UnaryMinus();

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(5, 5, Description = "Positive value - enhanced")]
    [TestCase(-5, 5, Description = "Negative value - enhanced")]
    [TestCase(0, 0, Description = "Zero - enhanced")]
    [TestCase(-100, 100, Description = "Large negative")]
    [TestCase(42, 42, Description = "Positive answer")]
    [TestCase(-1, 1, Description = "Negative one")]
    [TestCase(2147483647, 2147483647, Description = "Max int")]
    [TestCase(-2147483647, 2147483647, Description = "Large negative int")]
    public void Abs_EnhancedCoverage_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(value);
        var result = x.Abs();

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(value)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(expectedResult)));

        // Additional verification: abs should always be non-negative
        solver.Assert(result >= 0);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(10, 5, 5, Description = "Standard case")]
    [TestCase(-3, 7, -3, Description = "Negative minimum")]
    [TestCase(0, 0, 0, Description = "Equal values")]
    [TestCase(100, -50, -50, Description = "Positive vs negative")]
    [TestCase(-10, -20, -20, Description = "Two negatives")]
    public void Min_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of minimum (extension methods)
        var resultContextIntExpr = context.Min(x, y); // Context.Min(IntExpr, IntExpr) (extension method)
        var resultContextRightInt = context.Min(x, rightInt); // Context.Min(IntExpr, int) (extension method)
        var resultContextLeftInt = context.Min(leftInt, y); // Context.Min(int, IntExpr) (extension method)
        var resultContextRightBigInt = context.Min(x, rightBigInt); // Context.Min(IntExpr, BigInteger) (extension method)
        var resultContextLeftBigInt = context.Min(leftBigInt, y); // Context.Min(BigInteger, IntExpr) (extension method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Min(IntExpr, IntExpr) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Min(IntExpr, int) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Min(int, IntExpr) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Min(IntExpr, BigInteger) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Min(BigInteger, IntExpr) extension method failed"
            );
        });
    }

    [TestCase(10, 5, 10, Description = "Standard case")]
    [TestCase(-3, 7, 7, Description = "Negative vs positive")]
    [TestCase(0, 0, 0, Description = "Equal values")]
    [TestCase(100, -50, 100, Description = "Positive vs negative")]
    [TestCase(-10, -20, -10, Description = "Two negatives")]
    public void Max_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
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

        // Test all variations of maximum (extension methods)
        var resultContextIntExpr = context.Max(x, y); // Context.Max(IntExpr, IntExpr) (extension method)
        var resultContextRightInt = context.Max(x, rightInt); // Context.Max(IntExpr, int) (extension method)
        var resultContextLeftInt = context.Max(leftInt, y); // Context.Max(int, IntExpr) (extension method)
        var resultContextRightBigInt = context.Max(x, rightBigInt); // Context.Max(IntExpr, BigInteger) (extension method)
        var resultContextLeftBigInt = context.Max(leftBigInt, y); // Context.Max(BigInteger, IntExpr) (extension method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Int(left)));
        solver.Assert(context.Eq(y, context.Int(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedResult;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(resultContextIntExpr),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Max(IntExpr, IntExpr) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Max(IntExpr, int) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Max(int, IntExpr) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextRightBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Max(IntExpr, BigInteger) extension method failed"
            );
            Assert.That(
                model.GetIntValue(resultContextLeftBigInt),
                Is.EqualTo(new BigInteger(expected)),
                "Context.Max(BigInteger, IntExpr) extension method failed"
            );
        });
    }

    [Test]
    public void MixedOperationScenarios_ComplexExpressions_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");

        // Test complex expression: (x + y) * z - abs(x)
        var complexResult = (x + y) * z - x.Abs();

        // Set specific values: x = -3, y = 5, z = 2
        // Expected: (-3 + 5) * 2 - abs(-3) = 2 * 2 - 3 = 4 - 3 = 1
        solver.Assert(context.Eq(x, context.Int(-3)));
        solver.Assert(context.Eq(y, context.Int(5)));
        solver.Assert(context.Eq(z, context.Int(2)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(complexResult), Is.EqualTo(new BigInteger(1)));
    }

    [Test]
    public void PrimitiveInteraction_IntExprWithLiterals_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        // Test various primitive interactions from the original tests
        var result1 = x + 10; // IntExpr + int
        var result2 = 20 + x; // int + IntExpr
        var result3 = x - 3; // IntExpr - int
        var result4 = 100 - x; // int - IntExpr
        var result5 = x * 4; // IntExpr * int
        var result6 = 7 * x; // int * IntExpr

        // Set x = 5 for testing
        solver.Assert(context.Eq(x, context.Int(5)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(result1), Is.EqualTo(new BigInteger(15)), "x + 10 failed");
            Assert.That(model.GetIntValue(result2), Is.EqualTo(new BigInteger(25)), "20 + x failed");
            Assert.That(model.GetIntValue(result3), Is.EqualTo(new BigInteger(2)), "x - 3 failed");
            Assert.That(model.GetIntValue(result4), Is.EqualTo(new BigInteger(95)), "100 - x failed");
            Assert.That(model.GetIntValue(result5), Is.EqualTo(new BigInteger(20)), "x * 4 failed");
            Assert.That(model.GetIntValue(result6), Is.EqualTo(new BigInteger(35)), "7 * x failed");
        });
    }

    #region Variadic Params Tests

    [Test]
    public void Add_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(42);
        var result = context.Add(x);

        solver.Assert(context.Eq(x, context.Int(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)), "Single operand Add failed");
    }

    [TestCase(5, 3, 2, 10, Description = "Three operands: 5 + 3 + 2 = 10")]
    [TestCase(1, -1, 0, 0, Description = "Three operands with zero result")]
    [TestCase(-5, -3, -2, -10, Description = "Three negative operands")]
    [TestCase(100, -50, 25, 75, Description = "Mixed positive and negative")]
    public void Add_VariadicParams_ThreeOperands_ReturnsExpectedResult(int a, int b, int c, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(a);
        var y = context.Int(b);
        var z = context.Int(c);

        var result = context.Add(x, y, z);

        solver.Assert(context.Eq(x, context.Int(a)));
        solver.Assert(context.Eq(y, context.Int(b)));
        solver.Assert(context.Eq(z, context.Int(c)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetIntValue(result),
            Is.EqualTo(new BigInteger(expectedResult)),
            $"Three operands Add({a}, {b}, {c}) failed"
        );
    }

    [TestCase(1, 2, 3, 4, 10, Description = "Four operands: 1 + 2 + 3 + 4 = 10")]
    [TestCase(10, -5, 3, -2, 6, Description = "Four operands with mixed signs")]
    [TestCase(0, 0, 0, 0, 0, Description = "Four zeros")]
    public void Add_VariadicParams_FourOperands_ReturnsExpectedResult(int a, int b, int c, int d, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var w = context.Int(a);
        var x = context.Int(b);
        var y = context.Int(c);
        var z = context.Int(d);

        var result = context.Add(w, x, y, z);

        solver.Assert(context.Eq(w, context.Int(a)));
        solver.Assert(context.Eq(x, context.Int(b)));
        solver.Assert(context.Eq(y, context.Int(c)));
        solver.Assert(context.Eq(z, context.Int(d)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetIntValue(result),
            Is.EqualTo(new BigInteger(expectedResult)),
            $"Four operands Add({a}, {b}, {c}, {d}) failed"
        );
    }

    [Test]
    public void Add_VariadicParams_FiveOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(1);
        var b = context.Int(2);
        var c = context.Int(3);
        var d = context.Int(4);
        var e = context.Int(5);

        var result = context.Add(a, b, c, d, e);

        solver.Assert(context.Eq(a, context.Int(1)));
        solver.Assert(context.Eq(b, context.Int(2)));
        solver.Assert(context.Eq(c, context.Int(3)));
        solver.Assert(context.Eq(d, context.Int(4)));
        solver.Assert(context.Eq(e, context.Int(5)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(15)), "Five operands Add(1,2,3,4,5) failed");
    }

    [Test]
    public void Add_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Add(Array.Empty<IntExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void Sub_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(42);
        var result = context.Sub(x);

        solver.Assert(context.Eq(x, context.Int(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)), "Single operand Sub failed");
    }

    [TestCase(10, 3, 2, 5, Description = "Three operands: 10 - 3 - 2 = 5")]
    [TestCase(0, 1, -1, 0, Description = "Three operands: 0 - 1 - (-1) = 0")]
    [TestCase(100, 20, 30, 50, Description = "Three operands: 100 - 20 - 30 = 50")]
    [TestCase(-5, -3, -2, 0, Description = "Three negative operands: -5 - (-3) - (-2) = 0")]
    public void Sub_VariadicParams_ThreeOperands_ReturnsExpectedResult(int a, int b, int c, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(a);
        var y = context.Int(b);
        var z = context.Int(c);

        var result = context.Sub(x, y, z);

        solver.Assert(context.Eq(x, context.Int(a)));
        solver.Assert(context.Eq(y, context.Int(b)));
        solver.Assert(context.Eq(z, context.Int(c)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetIntValue(result),
            Is.EqualTo(new BigInteger(expectedResult)),
            $"Three operands Sub({a}, {b}, {c}) failed"
        );
    }

    [Test]
    public void Sub_VariadicParams_FourOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(100);
        var b = context.Int(20);
        var c = context.Int(15);
        var d = context.Int(10);

        var result = context.Sub(a, b, c, d);

        solver.Assert(context.Eq(a, context.Int(100)));
        solver.Assert(context.Eq(b, context.Int(20)));
        solver.Assert(context.Eq(c, context.Int(15)));
        solver.Assert(context.Eq(d, context.Int(10)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // 100 - 20 - 15 - 10 = 55
        Assert.That(
            model.GetIntValue(result),
            Is.EqualTo(new BigInteger(55)),
            "Four operands Sub(100,20,15,10) failed"
        );
    }

    [Test]
    public void Sub_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Sub(Array.Empty<IntExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void Mul_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(42);
        var result = context.Mul(x);

        solver.Assert(context.Eq(x, context.Int(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)), "Single operand Mul failed");
    }

    [TestCase(2, 3, 4, 24, Description = "Three operands: 2 * 3 * 4 = 24")]
    [TestCase(1, 5, 7, 35, Description = "Three operands: 1 * 5 * 7 = 35")]
    [TestCase(-2, 3, 4, -24, Description = "Three operands with negative: -2 * 3 * 4 = -24")]
    [TestCase(0, 100, 200, 0, Description = "Three operands with zero: 0 * 100 * 200 = 0")]
    [TestCase(-1, -2, -3, -6, Description = "Three negative operands: -1 * -2 * -3 = -6")]
    public void Mul_VariadicParams_ThreeOperands_ReturnsExpectedResult(int a, int b, int c, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Int(a);
        var y = context.Int(b);
        var z = context.Int(c);

        var result = context.Mul(x, y, z);

        solver.Assert(context.Eq(x, context.Int(a)));
        solver.Assert(context.Eq(y, context.Int(b)));
        solver.Assert(context.Eq(z, context.Int(c)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetIntValue(result),
            Is.EqualTo(new BigInteger(expectedResult)),
            $"Three operands Mul({a}, {b}, {c}) failed"
        );
    }

    [Test]
    public void Mul_VariadicParams_FourOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(2);
        var b = context.Int(3);
        var c = context.Int(4);
        var d = context.Int(5);

        var result = context.Mul(a, b, c, d);

        solver.Assert(context.Eq(a, context.Int(2)));
        solver.Assert(context.Eq(b, context.Int(3)));
        solver.Assert(context.Eq(c, context.Int(4)));
        solver.Assert(context.Eq(d, context.Int(5)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // 2 * 3 * 4 * 5 = 120
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(120)), "Four operands Mul(2,3,4,5) failed");
    }

    [Test]
    public void Mul_VariadicParams_FiveOperands_WithZero_ReturnsZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(1);
        var b = context.Int(2);
        var c = context.Int(0); // Zero multiplicand
        var d = context.Int(4);
        var e = context.Int(5);

        var result = context.Mul(a, b, c, d, e);

        solver.Assert(context.Eq(a, context.Int(1)));
        solver.Assert(context.Eq(b, context.Int(2)));
        solver.Assert(context.Eq(c, context.Int(0)));
        solver.Assert(context.Eq(d, context.Int(4)));
        solver.Assert(context.Eq(e, context.Int(5)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(0)), "Five operands Mul with zero failed");
    }

    [Test]
    public void Mul_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Mul(Array.Empty<IntExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void VariadicOperations_WithVariables_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var a = context.IntConst("a");
        var b = context.IntConst("b");
        var c = context.IntConst("c");

        // Test variadic operations with variables
        var addResult = context.Add(a, b, c);
        var subResult = context.Sub(a, b, c);
        var mulResult = context.Mul(a, b, c);

        // Set specific values: a=10, b=3, c=2
        solver.Assert(context.Eq(a, context.Int(10)));
        solver.Assert(context.Eq(b, context.Int(3)));
        solver.Assert(context.Eq(c, context.Int(2)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetIntValue(addResult),
                Is.EqualTo(new BigInteger(15)),
                "Variadic Add(a,b,c) with variables failed"
            ); // 10+3+2=15
            Assert.That(
                model.GetIntValue(subResult),
                Is.EqualTo(new BigInteger(5)),
                "Variadic Sub(a,b,c) with variables failed"
            ); // 10-3-2=5
            Assert.That(
                model.GetIntValue(mulResult),
                Is.EqualTo(new BigInteger(60)),
                "Variadic Mul(a,b,c) with variables failed"
            ); // 10*3*2=60
        });
    }

    #endregion
}
