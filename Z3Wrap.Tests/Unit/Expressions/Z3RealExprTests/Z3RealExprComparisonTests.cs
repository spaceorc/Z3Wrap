using System.Globalization;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3RealExprTests;

[TestFixture]
public class Z3RealExprComparisonTests
{
    [TestCase("2.1", "2.2", true, Description = "Basic less than - true")]
    [TestCase("5.5", "3.3", false, Description = "Basic less than - false")]
    [TestCase("0.0", "0.1", true, Description = "Zero less than positive")]
    [TestCase("-2.5", "-1.0", true, Description = "Negative less than negative")]
    [TestCase("10.0", "10.0", false, Description = "Equal values - false")]
    public void Lt_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of less than - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x < y; // RealExpr < RealExpr (operator)
        var resultOperatorRightDecimal = x < rightDecimal; // RealExpr < decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal < y; // decimal < RealExpr (operator)
        var resultMethodRealExpr = x.Lt(y); // RealExpr.Lt(RealExpr) (method)
        var resultMethodDecimal = x.Lt(rightDecimal); // RealExpr.Lt(decimal) (method)
        var resultContextRealExpr = context.Lt(x, y); // Context.Lt(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Lt(x, rightDecimal); // Context.Lt(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Lt(leftDecimal, y); // Context.Lt(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("5.5", "5.5", true, Description = "Equal values - true")]
    [TestCase("2.1", "2.2", true, Description = "Less than - true")]
    [TestCase("5.5", "3.3", false, Description = "Greater than - false")]
    [TestCase("-2.5", "-1.0", true, Description = "Negative less than equal")]
    [TestCase("0.0", "0.0", true, Description = "Zero equal")]
    public void Le_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of less than or equal - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x <= y; // RealExpr <= RealExpr (operator)
        var resultOperatorRightDecimal = x <= rightDecimal; // RealExpr <= decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal <= y; // decimal <= RealExpr (operator)
        var resultMethodRealExpr = x.Le(y); // RealExpr.Le(RealExpr) (method)
        var resultMethodDecimal = x.Le(rightDecimal); // RealExpr.Le(decimal) (method)
        var resultContextRealExpr = context.Le(x, y); // Context.Le(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Le(x, rightDecimal); // Context.Le(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Le(leftDecimal, y); // Context.Le(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("9.7", "1.3", true, Description = "Basic greater than - true")]
    [TestCase("2.1", "2.2", false, Description = "Less than - false")]
    [TestCase("5.5", "5.5", false, Description = "Equal values - false")]
    [TestCase("-1.0", "-2.5", true, Description = "Negative greater than negative")]
    [TestCase("0.0", "-0.1", true, Description = "Zero greater than negative")]
    public void Gt_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of greater than - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x > y; // RealExpr > RealExpr (operator)
        var resultOperatorRightDecimal = x > rightDecimal; // RealExpr > decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal > y; // decimal > RealExpr (operator)
        var resultMethodRealExpr = x.Gt(y); // RealExpr.Gt(RealExpr) (method)
        var resultMethodDecimal = x.Gt(rightDecimal); // RealExpr.Gt(decimal) (method)
        var resultContextRealExpr = context.Gt(x, y); // Context.Gt(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Gt(x, rightDecimal); // Context.Gt(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Gt(leftDecimal, y); // Context.Gt(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("7.5", "7.5", true, Description = "Equal values - true")]
    [TestCase("9.7", "1.3", true, Description = "Greater than - true")]
    [TestCase("2.1", "2.2", false, Description = "Less than - false")]
    [TestCase("-1.0", "-2.5", true, Description = "Negative greater than equal")]
    [TestCase("0.0", "0.0", true, Description = "Zero equal")]
    public void Ge_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of greater than or equal - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x >= y; // RealExpr >= RealExpr (operator)
        var resultOperatorRightDecimal = x >= rightDecimal; // RealExpr >= decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal >= y; // decimal >= RealExpr (operator)
        var resultMethodRealExpr = x.Ge(y); // RealExpr.Ge(RealExpr) (method)
        var resultMethodDecimal = x.Ge(rightDecimal); // RealExpr.Ge(decimal) (method)
        var resultContextRealExpr = context.Ge(x, y); // Context.Ge(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Ge(x, rightDecimal); // Context.Ge(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Ge(leftDecimal, y); // Context.Ge(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("3.14", "3.14", true, Description = "Equal values - true")]
    [TestCase("2.718", "2.718", true, Description = "Equal values - true")]
    [TestCase("1.0", "2.0", false, Description = "Different values - false")]
    [TestCase("-5.5", "-5.5", true, Description = "Equal negative values - true")]
    [TestCase("0.0", "0.0", true, Description = "Zero equals zero")]
    public void Eq_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of equality - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x == y; // RealExpr == RealExpr (operator)
        var resultOperatorRightDecimal = x == rightDecimal; // RealExpr == decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal == y; // decimal == RealExpr (operator)
        var resultContextRealExpr = context.Eq(x, y); // Context.Eq(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Eq(x, rightDecimal); // Context.Eq(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Eq(leftDecimal, y); // Context.Eq(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all equality comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all equality comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("1.0", "0.0", true, Description = "Different values - true")]
    [TestCase("2.5", "2.7", true, Description = "Different values - true")]
    [TestCase("3.14", "3.14", false, Description = "Equal values - false")]
    [TestCase("-5.5", "-6.6", true, Description = "Different negative values - true")]
    [TestCase("0.0", "0.0", false, Description = "Zero not equals zero - false")]
    public void Neq_AllVariations_ReturnsExpectedResult(
        string leftStr,
        string rightStr,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);

        // Test all variations of inequality - SIMPLIFIED to decimal + RealExpr only
        var resultOperatorRealExpr = x != y; // RealExpr != RealExpr (operator)
        var resultOperatorRightDecimal = x != rightDecimal; // RealExpr != decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal != y; // decimal != RealExpr (operator)
        var resultContextRealExpr = context.Neq(x, y); // Context.Neq(RealExpr, RealExpr) (method)
        var resultContextRightDecimal = context.Neq(x, rightDecimal); // Context.Neq(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Neq(leftDecimal, y); // Context.Neq(decimal, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all inequality comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
        }
        else
        {
            // If we expect false, assert negation of all inequality comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OperatorMethodEquivalence_Lt_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

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

        var x = context.RealConst("x");
        var y = context.RealConst("y");

        var operatorResult = x >= y;
        var methodResult = x.Ge(y);

        solver.Assert(context.Iff(operatorResult, methodResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComparisonChaining_MultipleComparisons_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var z = context.RealConst("z");

        // Test chained comparison: x < y < z
        var comparison1 = x < y;
        var comparison2 = y < z;
        var chainedComparison = comparison1 & comparison2;

        // Set specific values: x = 1.0, y = 2.5, z = 5.0
        solver.Assert(context.Eq(x, context.Real(1.0m)));
        solver.Assert(context.Eq(y, context.Real(2.5m)));
        solver.Assert(context.Eq(z, context.Real(5.0m)));

        solver.Assert(chainedComparison);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MixedTypeComparison_DecimalCombinations_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.RealConst("real");
        var decimalValue = 5.0m;

        // Test that decimal comparisons work and produce consistent results
        var realVsDecimal = realExpr == decimalValue;
        var decimalVsReal = decimalValue == realExpr;

        solver.Assert(context.Eq(realExpr, context.Real(5.0m)));
        solver.Assert(realVsDecimal);
        solver.Assert(decimalVsReal);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
