using System.Globalization;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.__old.Unit.Expressions.Z3RealExprTests;

[TestFixture]
public class Z3RealArithmeticTests
{
    [TestCase("2.5", "3.7", "6.2", Description = "Basic decimal addition")]
    [TestCase("10.8", "4.3", "15.1", Description = "Another decimal addition")]
    [TestCase("-5.5", "3.2", "-2.3", Description = "Negative plus positive")]
    [TestCase("0.0", "0.0", "0.0", Description = "Adding zeros")]
    [TestCase("100.75", "-50.25", "50.5", Description = "Positive plus negative")]
    [TestCase("-10.2", "-20.8", "-31.0", Description = "Two negatives")]
    public void Add_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of addition - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x + y; // RealExpr + RealExpr (operator)
        var resultOperatorRightDecimal = x + rightDecimal; // RealExpr + decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal + y; // decimal + RealExpr (operator)
        var resultMethodRealExpr = x.Add(y); // RealExpr.Add(RealExpr) (method)
        var resultMethodDecimal = x.Add(rightDecimal); // RealExpr.Add(decimal) (method)
        var resultContextRealExpr = context.Add(x, y); // Context.Add(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Add(x, rightDecimal); // Context.Add(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Add(leftDecimal, y); // Context.Add(decimal, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultOperatorRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr + RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorRightDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr + decimal operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "decimal + RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Add(RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Add(decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(new Real(expected)),
                "Context.Add(RealExpr, RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Add(RealExpr, decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Add(decimal, RealExpr) method failed"
            );
        });
    }

    [TestCase("10.8", "4.3", "6.5", Description = "Basic decimal subtraction")]
    [TestCase("5.0", "8.5", "-3.5", Description = "Negative result")]
    [TestCase("0.0", "1.7", "-1.7", Description = "Zero minus decimal")]
    [TestCase("100.25", "100.25", "0.0", Description = "Equal values result in zero")]
    [TestCase("-5.5", "-3.2", "-2.3", Description = "Negative minus negative")]
    [TestCase("-10.1", "5.9", "-16.0", Description = "Negative minus positive")]
    public void Sub_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of subtraction - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x - y; // RealExpr - RealExpr (operator)
        var resultOperatorRightDecimal = x - rightDecimal; // RealExpr - decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal - y; // decimal - RealExpr (operator)
        var resultMethodRealExpr = x.Sub(y); // RealExpr.Sub(RealExpr) (method)
        var resultMethodDecimal = x.Sub(rightDecimal); // RealExpr.Sub(decimal) (method)
        var resultContextRealExpr = context.Sub(x, y); // Context.Sub(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Sub(x, rightDecimal); // Context.Sub(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Sub(leftDecimal, y); // Context.Sub(decimal, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultOperatorRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr - RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorRightDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr - decimal operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "decimal - RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Sub(RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Sub(decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(new Real(expected)),
                "Context.Sub(RealExpr, RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Sub(RealExpr, decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Sub(decimal, RealExpr) method failed"
            );
        });
    }

    [TestCase("2.5", "4.0", "10.0", Description = "Basic decimal multiplication")]
    [TestCase("0.0", "100.5", "0.0", Description = "Zero times anything")]
    [TestCase("1.0", "42.7", "42.7", Description = "One times value")]
    [TestCase("-2.5", "3.0", "-7.5", Description = "Negative times positive")]
    [TestCase("-4.2", "-1.5", "6.3", Description = "Negative times negative")]
    [TestCase("10.0", "-2.3", "-23.0", Description = "Positive times negative")]
    public void Mul_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of multiplication - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x * y; // RealExpr * RealExpr (operator)
        var resultOperatorRightDecimal = x * rightDecimal; // RealExpr * decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal * y; // decimal * RealExpr (operator)
        var resultMethodRealExpr = x.Mul(y); // RealExpr.Mul(RealExpr) (method)
        var resultMethodDecimal = x.Mul(rightDecimal); // RealExpr.Mul(decimal) (method)
        var resultContextRealExpr = context.Mul(x, y); // Context.Mul(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Mul(x, rightDecimal); // Context.Mul(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Mul(leftDecimal, y); // Context.Mul(decimal, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultOperatorRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr * RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorRightDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr * decimal operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "decimal * RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Mul(RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Mul(decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(new Real(expected)),
                "Context.Mul(RealExpr, RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Mul(RealExpr, decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Mul(decimal, RealExpr) method failed"
            );
        });
    }

    [TestCase("15.0", "3.0", "5.0", Description = "Basic decimal division")]
    [TestCase("10.0", "2.0", "5.0", Description = "Even division")]
    [TestCase("7.5", "3.0", "2.5", Description = "Division with decimal result")]
    [TestCase("100.0", "1.0", "100.0", Description = "Division by one")]
    [TestCase("-15.0", "3.0", "-5.0", Description = "Negative divided by positive")]
    [TestCase("15.0", "-3.0", "-5.0", Description = "Positive divided by negative")]
    [TestCase("-15.0", "-3.0", "5.0", Description = "Negative divided by negative")]
    public void Div_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of division - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x / y; // RealExpr / RealExpr (operator)
        var resultOperatorRightDecimal = x / rightDecimal; // RealExpr / decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal / y; // decimal / RealExpr (operator)
        var resultMethodRealExpr = x.Div(y); // RealExpr.Div(RealExpr) (method)
        var resultMethodDecimal = x.Div(rightDecimal); // RealExpr.Div(decimal) (method)
        var resultContextRealExpr = context.Div(x, y); // Context.Div(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Div(x, rightDecimal); // Context.Div(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Div(leftDecimal, y); // Context.Div(decimal, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultOperatorRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr / RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorRightDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr / decimal operator failed"
            );
            Assert.That(
                model.GetRealValue(resultOperatorLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "decimal / RealExpr operator failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodRealExpr),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Div(RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultMethodDecimal),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Div(decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(new Real(expected)),
                "Context.Div(RealExpr, RealExpr) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Div(RealExpr, decimal) method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(new Real(expected)),
                "Context.Div(decimal, RealExpr) method failed"
            );
        });
    }

    [TestCase("42.7", "-42.7", Description = "Positive value negation")]
    [TestCase("0.0", "0.0", Description = "Zero negation")]
    [TestCase("-3.14", "3.14", Description = "Negative value")]
    [TestCase("-100.5", "100.5", Description = "Negative decimal")]
    public void UnaryMinus_AllVariations_ReturnsExpectedResult(string valueStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var valueDecimal = decimal.Parse(valueStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(valueDecimal);
        var expected = expectedDecimal;

        // Test all variations of negation
        var resultOperator = -x; // -RealExpr (unary operator)
        var resultMethod = x.UnaryMinus(); // RealExpr.UnaryMinus() (method)
        var resultContext = context.UnaryMinus(x); // Context.UnaryMinus(RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(valueDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultOperator),
                Is.EqualTo(new Real(expected)),
                "-RealExpr unary operator failed"
            );
            Assert.That(
                model.GetRealValue(resultMethod),
                Is.EqualTo(new Real(expected)),
                "RealExpr.UnaryMinus() method failed"
            );
            Assert.That(
                model.GetRealValue(resultContext),
                Is.EqualTo(new Real(expected)),
                "Context.UnaryMinus(RealExpr) method failed"
            );
        });
    }

    [TestCase("5.5", "5.5", Description = "Positive decimal value")]
    [TestCase("-5.7", "5.7", Description = "Negative decimal value")]
    [TestCase("0.0", "0.0", Description = "Zero")]
    [TestCase("-100.25", "100.25", Description = "Negative hundred")]
    [TestCase("42.42", "42.42", Description = "Positive decimal")]
    public void Abs_AllVariations_ReturnsExpectedResult(string valueStr, string expectedStr)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var valueDecimal = decimal.Parse(valueStr, CultureInfo.InvariantCulture);
        var expectedDecimal = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

        var x = context.Real(valueDecimal);
        var expected = expectedDecimal;

        // Test all variations of absolute value
        var resultMethod = x.Abs(); // RealExpr.Abs() (method)
        var resultContext = context.Abs(x); // Context.Abs(RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(valueDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultMethod),
                Is.EqualTo(new Real(expected)),
                "RealExpr.Abs() method failed"
            );
            Assert.That(
                model.GetRealValue(resultContext),
                Is.EqualTo(new Real(expected)),
                "Context.Abs(RealExpr) method failed"
            );
        });
    }

    [TestCase(10.5, 5.2, 5.2, Description = "Standard case")]
    [TestCase(-3.7, 7.1, -3.7, Description = "Negative minimum")]
    [TestCase(0.0, 0.0, 0.0, Description = "Equal values")]
    [TestCase(100.25, -50.75, -50.75, Description = "Positive vs negative")]
    [TestCase(-10.8, -20.3, -20.3, Description = "Two negatives")]
    public void Min_AllVariations_ReturnsExpectedResult(double leftDouble, double rightDouble, double expectedDouble)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var left = (decimal)leftDouble;
        var right = (decimal)rightDouble;
        var expected = (decimal)expectedDouble;

        var x = context.Real(left);
        var y = context.Real(right);
        var leftDecimal = left;
        var rightDecimal = right;

        // Test all variations of minimum (extension methods)
        var resultContextRealExpr = context.Min(x, y); // Context.Min(RealExpr, RealExpr) (extension method)
        var resultContextRightDecimal = context.Min(x, rightDecimal); // Context.Min(RealExpr, decimal) (extension method)
        var resultContextLeftDecimal = context.Min(leftDecimal, y); // Context.Min(decimal, RealExpr) (extension method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(left)));
        solver.Assert(context.Eq(y, context.Real(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedReal = new Real(expected);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(expectedReal),
                "Context.Min(RealExpr, RealExpr) extension method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(expectedReal),
                "Context.Min(RealExpr, decimal) extension method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(expectedReal),
                "Context.Min(decimal, RealExpr) extension method failed"
            );
        });
    }

    [TestCase(10.5, 5.2, 10.5, Description = "Standard case")]
    [TestCase(-3.7, 7.1, 7.1, Description = "Negative vs positive")]
    [TestCase(0.0, 0.0, 0.0, Description = "Equal values")]
    [TestCase(100.25, -50.75, 100.25, Description = "Positive vs negative")]
    [TestCase(-10.8, -20.3, -10.8, Description = "Two negatives")]
    public void Max_AllVariations_ReturnsExpectedResult(double leftDouble, double rightDouble, double expectedDouble)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var left = (decimal)leftDouble;
        var right = (decimal)rightDouble;
        var expected = (decimal)expectedDouble;

        var x = context.Real(left);
        var y = context.Real(right);
        var leftDecimal = left;
        var rightDecimal = right;

        // Test all variations of maximum (extension methods)
        var resultContextRealExpr = context.Max(x, y); // Context.Max(RealExpr, RealExpr) (extension method)
        var resultContextRightDecimal = context.Max(x, rightDecimal); // Context.Max(RealExpr, decimal) (extension method)
        var resultContextLeftDecimal = context.Max(leftDecimal, y); // Context.Max(decimal, RealExpr) (extension method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(left)));
        solver.Assert(context.Eq(y, context.Real(right)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedReal = new Real(expected);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(resultContextRealExpr),
                Is.EqualTo(expectedReal),
                "Context.Max(RealExpr, RealExpr) extension method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextRightDecimal),
                Is.EqualTo(expectedReal),
                "Context.Max(RealExpr, decimal) extension method failed"
            );
            Assert.That(
                model.GetRealValue(resultContextLeftDecimal),
                Is.EqualTo(expectedReal),
                "Context.Max(decimal, RealExpr) extension method failed"
            );
        });
    }

    [Test]
    public void OperatorMethodEquivalence_Add_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

        var operatorResult = x / y;
        var methodResult = x.Div(y);

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_UnaryMinus_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        var operatorResult = -x;
        var methodResult = x.UnaryMinus();

        solver.Assert(context.Eq(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MixedOperationScenarios_ComplexExpressions_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var z = context.RealConst("z");

        // Test complex expression: (x + y) * z - abs(x)
        var complexResult = (x + y) * z - x.Abs();

        // Set specific values: x = -3.5, y = 5.2, z = 2.0
        // Expected: (-3.5 + 5.2) * 2.0 - abs(-3.5) = 1.7 * 2.0 - 3.5 = 3.4 - 3.5 = -0.1
        solver.Assert(context.Eq(x, context.Real(-3.5m)));
        solver.Assert(context.Eq(y, context.Real(5.2m)));
        solver.Assert(context.Eq(z, context.Real(2.0m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(complexResult), Is.EqualTo(new Real(-0.1m)));
    }

    [Test]
    public void PrimitiveInteraction_RealExprWithDecimals_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // Test decimal interactions with RealExpr
        var result1 = x + 10.5m; // RealExpr + decimal
        var result2 = 20.7m + x; // decimal + RealExpr
        var result3 = x - 3.2m; // RealExpr - decimal
        var result4 = 100.8m - x; // decimal - RealExpr
        var result5 = x * 4.1m; // RealExpr * decimal
        var result6 = 7.3m * x; // decimal * RealExpr

        // Set x = 5.5 for testing
        solver.Assert(context.Eq(x, context.Real(5.5m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(result1), Is.EqualTo(new Real(16.0m)), "x + 10.5m failed");
            Assert.That(model.GetRealValue(result2), Is.EqualTo(new Real(26.2m)), "20.7m + x failed");
            Assert.That(model.GetRealValue(result3), Is.EqualTo(new Real(2.3m)), "x - 3.2m failed");
            Assert.That(model.GetRealValue(result4), Is.EqualTo(new Real(95.3m)), "100.8m - x failed");
            Assert.That(model.GetRealValue(result5), Is.EqualTo(new Real(22.55m)), "x * 4.1m failed");
            Assert.That(model.GetRealValue(result6), Is.EqualTo(new Real(40.15m)), "7.3m * x failed");
        });
    }

    #region Variadic Params Tests

    [Test]
    public void Add_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(42.5m);
        var result = context.Add(x);

        solver.Assert(context.Eq(x, context.Real(42.5m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(new Real(42.5m)), "Single operand Add failed");
    }

    [TestCase("5.5", "3.2", "2.1", "10.8", Description = "Three operands: 5.5 + 3.2 + 2.1 = 10.8")]
    [TestCase("1.0", "-1.5", "0.5", "0.0", Description = "Three operands with zero result")]
    [TestCase("-5.5", "-3.3", "-2.2", "-11.0", Description = "Three negative operands")]
    [TestCase("100.25", "-50.75", "25.50", "75.0", Description = "Mixed positive and negative")]
    public void Add_VariadicParams_ThreeOperands_ReturnsExpectedResult(
        string a,
        string b,
        string c,
        string expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(decimal.Parse(a, CultureInfo.InvariantCulture));
        var y = context.Real(decimal.Parse(b, CultureInfo.InvariantCulture));
        var z = context.Real(decimal.Parse(c, CultureInfo.InvariantCulture));

        var result = context.Add(x, y, z);

        solver.Assert(context.Eq(x, context.Real(decimal.Parse(a, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(y, context.Real(decimal.Parse(b, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(z, context.Real(decimal.Parse(c, CultureInfo.InvariantCulture))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(decimal.Parse(expectedResult, CultureInfo.InvariantCulture))),
            $"Three operands Add({a}, {b}, {c}) failed"
        );
    }

    [TestCase("1.1", "2.2", "3.3", "4.4", "11.0", Description = "Four operands: 1.1 + 2.2 + 3.3 + 4.4 = 11.0")]
    [TestCase("10.5", "-5.25", "3.75", "-2.0", "7.0", Description = "Four operands with mixed signs")]
    [TestCase("0.0", "0.0", "0.0", "0.0", "0.0", Description = "Four zeros")]
    public void Add_VariadicParams_FourOperands_ReturnsExpectedResult(
        string a,
        string b,
        string c,
        string d,
        string expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var w = context.Real(decimal.Parse(a, CultureInfo.InvariantCulture));
        var x = context.Real(decimal.Parse(b, CultureInfo.InvariantCulture));
        var y = context.Real(decimal.Parse(c, CultureInfo.InvariantCulture));
        var z = context.Real(decimal.Parse(d, CultureInfo.InvariantCulture));

        var result = context.Add(w, x, y, z);

        solver.Assert(context.Eq(w, context.Real(decimal.Parse(a, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(x, context.Real(decimal.Parse(b, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(y, context.Real(decimal.Parse(c, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(z, context.Real(decimal.Parse(d, CultureInfo.InvariantCulture))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(decimal.Parse(expectedResult, CultureInfo.InvariantCulture))),
            $"Four operands Add({a}, {b}, {c}, {d}) failed"
        );
    }

    [Test]
    public void Add_VariadicParams_FiveOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(1.1m);
        var b = context.Real(2.2m);
        var c = context.Real(3.3m);
        var d = context.Real(4.4m);
        var e = context.Real(5.5m);

        var result = context.Add(a, b, c, d, e);

        solver.Assert(context.Eq(a, context.Real(1.1m)));
        solver.Assert(context.Eq(b, context.Real(2.2m)));
        solver.Assert(context.Eq(c, context.Real(3.3m)));
        solver.Assert(context.Eq(d, context.Real(4.4m)));
        solver.Assert(context.Eq(e, context.Real(5.5m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(16.5m)),
            "Five operands Add(1.1,2.2,3.3,4.4,5.5) failed"
        );
    }

    [Test]
    public void Add_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Add(Array.Empty<RealExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void Sub_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(42.7m);
        var result = context.Sub(x);

        solver.Assert(context.Eq(x, context.Real(42.7m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(new Real(42.7m)), "Single operand Sub failed");
    }

    [TestCase("10.5", "3.2", "2.1", "5.2", Description = "Three operands: 10.5 - 3.2 - 2.1 = 5.2")]
    [TestCase("0.0", "1.5", "-1.5", "0.0", Description = "Three operands: 0.0 - 1.5 - (-1.5) = 0.0")]
    [TestCase("100.8", "20.3", "30.5", "50.0", Description = "Three operands: 100.8 - 20.3 - 30.5 = 50.0")]
    [TestCase("-5.5", "-3.3", "-2.2", "0.0", Description = "Three negative operands: -5.5 - (-3.3) - (-2.2) = 0.0")]
    public void Sub_VariadicParams_ThreeOperands_ReturnsExpectedResult(
        string a,
        string b,
        string c,
        string expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(decimal.Parse(a, CultureInfo.InvariantCulture));
        var y = context.Real(decimal.Parse(b, CultureInfo.InvariantCulture));
        var z = context.Real(decimal.Parse(c, CultureInfo.InvariantCulture));

        var result = context.Sub(x, y, z);

        solver.Assert(context.Eq(x, context.Real(decimal.Parse(a, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(y, context.Real(decimal.Parse(b, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(z, context.Real(decimal.Parse(c, CultureInfo.InvariantCulture))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(decimal.Parse(expectedResult, CultureInfo.InvariantCulture))),
            $"Three operands Sub({a}, {b}, {c}) failed"
        );
    }

    [Test]
    public void Sub_VariadicParams_FourOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(100.75m);
        var b = context.Real(20.25m);
        var c = context.Real(15.50m);
        var d = context.Real(10.00m);

        var result = context.Sub(a, b, c, d);

        solver.Assert(context.Eq(a, context.Real(100.75m)));
        solver.Assert(context.Eq(b, context.Real(20.25m)));
        solver.Assert(context.Eq(c, context.Real(15.50m)));
        solver.Assert(context.Eq(d, context.Real(10.00m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // 100.75 - 20.25 - 15.50 - 10.00 = 55.00
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(55.0m)),
            "Four operands Sub(100.75,20.25,15.50,10.00) failed"
        );
    }

    [Test]
    public void Sub_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Sub(Array.Empty<RealExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void Mul_VariadicParams_SingleOperand_ReturnsOperand()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(42.3m);
        var result = context.Mul(x);

        solver.Assert(context.Eq(x, context.Real(42.3m)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(new Real(42.3m)), "Single operand Mul failed");
    }

    [TestCase("2.5", "3.2", "4.0", "32.0", Description = "Three operands: 2.5 * 3.2 * 4.0 = 32.0")]
    [TestCase("1.0", "5.5", "7.2", "39.6", Description = "Three operands: 1.0 * 5.5 * 7.2 = 39.6")]
    [TestCase("-2.5", "3.0", "4.0", "-30.0", Description = "Three operands with negative: -2.5 * 3.0 * 4.0 = -30.0")]
    [TestCase("0.0", "100.5", "200.7", "0.0", Description = "Three operands with zero: 0.0 * 100.5 * 200.7 = 0.0")]
    [TestCase("-1.5", "-2.0", "-3.0", "-9.0", Description = "Three negative operands: -1.5 * -2.0 * -3.0 = -9.0")]
    public void Mul_VariadicParams_ThreeOperands_ReturnsExpectedResult(
        string a,
        string b,
        string c,
        string expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Real(decimal.Parse(a, CultureInfo.InvariantCulture));
        var y = context.Real(decimal.Parse(b, CultureInfo.InvariantCulture));
        var z = context.Real(decimal.Parse(c, CultureInfo.InvariantCulture));

        var result = context.Mul(x, y, z);

        solver.Assert(context.Eq(x, context.Real(decimal.Parse(a, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(y, context.Real(decimal.Parse(b, CultureInfo.InvariantCulture))));
        solver.Assert(context.Eq(z, context.Real(decimal.Parse(c, CultureInfo.InvariantCulture))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(decimal.Parse(expectedResult, CultureInfo.InvariantCulture))),
            $"Three operands Mul({a}, {b}, {c}) failed"
        );
    }

    [Test]
    public void Mul_VariadicParams_FourOperands_ReturnsExpectedResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(2.0m);
        var b = context.Real(3.5m);
        var c = context.Real(4.0m);
        var d = context.Real(5.0m);

        var result = context.Mul(a, b, c, d);

        solver.Assert(context.Eq(a, context.Real(2.0m)));
        solver.Assert(context.Eq(b, context.Real(3.5m)));
        solver.Assert(context.Eq(c, context.Real(4.0m)));
        solver.Assert(context.Eq(d, context.Real(5.0m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // 2.0 * 3.5 * 4.0 * 5.0 = 140.0
        Assert.That(
            model.GetRealValue(result),
            Is.EqualTo(new Real(140.0m)),
            "Four operands Mul(2.0,3.5,4.0,5.0) failed"
        );
    }

    [Test]
    public void Mul_VariadicParams_FiveOperands_WithZero_ReturnsZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(1.5m);
        var b = context.Real(2.7m);
        var c = context.Real(0.0m); // Zero multiplicand
        var d = context.Real(4.2m);
        var e = context.Real(5.8m);

        var result = context.Mul(a, b, c, d, e);

        solver.Assert(context.Eq(a, context.Real(1.5m)));
        solver.Assert(context.Eq(b, context.Real(2.7m)));
        solver.Assert(context.Eq(c, context.Real(0.0m)));
        solver.Assert(context.Eq(d, context.Real(4.2m)));
        solver.Assert(context.Eq(e, context.Real(5.8m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(new Real(0.0m)), "Five operands Mul with zero failed");
    }

    [Test]
    public void Mul_VariadicParams_EmptyOperands_ThrowsException()
    {
        using var context = new Z3Context();

        Assert.Throws<InvalidOperationException>(
            () => context.Mul(Array.Empty<RealExpr>()),
            "Empty operands should throw InvalidOperationException"
        );
    }

    [Test]
    public void VariadicOperations_WithVariables_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var a = context.RealConst("a");
        var b = context.RealConst("b");
        var c = context.RealConst("c");

        // Test variadic operations with variables
        var addResult = context.Add(a, b, c);
        var subResult = context.Sub(a, b, c);
        var mulResult = context.Mul(a, b, c);

        // Set specific values: a=10.5, b=3.2, c=2.0
        solver.Assert(context.Eq(a, context.Real(10.5m)));
        solver.Assert(context.Eq(b, context.Real(3.2m)));
        solver.Assert(context.Eq(c, context.Real(2.0m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetRealValue(addResult),
                Is.EqualTo(new Real(15.7m)),
                "Variadic Add(a,b,c) with variables failed"
            ); // 10.5+3.2+2.0=15.7
            Assert.That(
                model.GetRealValue(subResult),
                Is.EqualTo(new Real(5.3m)),
                "Variadic Sub(a,b,c) with variables failed"
            ); // 10.5-3.2-2.0=5.3
            Assert.That(
                model.GetRealValue(mulResult),
                Is.EqualTo(new Real(67.2m)),
                "Variadic Mul(a,b,c) with variables failed"
            ); // 10.5*3.2*2.0=67.2
        });
    }

    #endregion
}
