using System.Globalization;
using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3RealExprTests;

[TestFixture]
public class Z3RealExprComparisonTests
{
    [TestCase("2.1", "2.2", true, Description = "Basic less than - true")]
    [TestCase("5.5", "3.3", false, Description = "Basic less than - false")]
    [TestCase("0.0", "0.1", true, Description = "Zero less than positive")]
    [TestCase("-2.5", "-1.0", true, Description = "Negative less than negative")]
    [TestCase("10.0", "10.0", false, Description = "Equal values - false")]
    public void Lt_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of less than
        var resultOperatorRealExpr = x < y;                              // RealExpr < RealExpr (operator)
        var resultOperatorRightInt = x < rightInt;                       // RealExpr < int (operator)
        var resultOperatorLeftInt = leftInt < y;                         // int < RealExpr (operator)
        var resultOperatorRightDecimal = x < rightDecimal;               // RealExpr < decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal < y;                 // decimal < RealExpr (operator)
        var resultOperatorRightLong = x < rightLong;                     // RealExpr < long (operator)
        var resultOperatorLeftLong = leftLong < y;                       // long < RealExpr (operator)
        var resultOperatorRightBigInt = x < rightBigInt;                 // RealExpr < BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt < y;                   // BigInteger < RealExpr (operator)
        var resultMethodRealExpr = x.Lt(y);                              // RealExpr.Lt(RealExpr) (method)
        var resultMethodInt = x.Lt(rightInt);                            // RealExpr.Lt(int) (method)
        var resultMethodDecimal = x.Lt(rightDecimal);                    // RealExpr.Lt(decimal) (method)
        var resultMethodLong = x.Lt(rightLong);                          // RealExpr.Lt(long) (method)
        var resultMethodBigInt = x.Lt(rightBigInt);                      // RealExpr.Lt(BigInteger) (method)
        var resultContextRealExpr = context.Lt(x, y);                    // Context.Lt(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Lt(x, rightInt);             // Context.Lt(RealExpr, int) (method)
        var resultContextLeftInt = context.Lt(leftInt, y);               // Context.Lt(int, RealExpr) (method)
        var resultContextRightDecimal = context.Lt(x, rightDecimal);     // Context.Lt(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Lt(leftDecimal, y);       // Context.Lt(decimal, RealExpr) (method)
        var resultContextRightLong = context.Lt(x, rightLong);           // Context.Lt(RealExpr, long) (method)
        var resultContextLeftLong = context.Lt(leftLong, y);             // Context.Lt(long, RealExpr) (method)
        var resultContextRightBigInt = context.Lt(x, rightBigInt);       // Context.Lt(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Lt(leftBigInt, y);         // Context.Lt(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodInt);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultMethodLong);
            solver.Assert(resultMethodBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodInt));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultMethodLong));
            solver.Assert(context.Not(resultMethodBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("5.5", "5.5", true, Description = "Equal values - true")]
    [TestCase("2.1", "2.2", true, Description = "Less than - true")]
    [TestCase("5.5", "3.3", false, Description = "Greater than - false")]
    [TestCase("-2.5", "-1.0", true, Description = "Negative less than equal")]
    [TestCase("0.0", "0.0", true, Description = "Zero equal")]
    public void Le_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of less than or equal
        var resultOperatorRealExpr = x <= y;                             // RealExpr <= RealExpr (operator)
        var resultOperatorRightInt = x <= rightInt;                      // RealExpr <= int (operator)
        var resultOperatorLeftInt = leftInt <= y;                        // int <= RealExpr (operator)
        var resultOperatorRightDecimal = x <= rightDecimal;              // RealExpr <= decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal <= y;                // decimal <= RealExpr (operator)
        var resultOperatorRightLong = x <= rightLong;                    // RealExpr <= long (operator)
        var resultOperatorLeftLong = leftLong <= y;                      // long <= RealExpr (operator)
        var resultOperatorRightBigInt = x <= rightBigInt;                // RealExpr <= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt <= y;                  // BigInteger <= RealExpr (operator)
        var resultMethodRealExpr = x.Le(y);                              // RealExpr.Le(RealExpr) (method)
        var resultMethodInt = x.Le(rightInt);                            // RealExpr.Le(int) (method)
        var resultMethodDecimal = x.Le(rightDecimal);                    // RealExpr.Le(decimal) (method)
        var resultMethodLong = x.Le(rightLong);                          // RealExpr.Le(long) (method)
        var resultMethodBigInt = x.Le(rightBigInt);                      // RealExpr.Le(BigInteger) (method)
        var resultContextRealExpr = context.Le(x, y);                    // Context.Le(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Le(x, rightInt);             // Context.Le(RealExpr, int) (method)
        var resultContextLeftInt = context.Le(leftInt, y);               // Context.Le(int, RealExpr) (method)
        var resultContextRightDecimal = context.Le(x, rightDecimal);     // Context.Le(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Le(leftDecimal, y);       // Context.Le(decimal, RealExpr) (method)
        var resultContextRightLong = context.Le(x, rightLong);           // Context.Le(RealExpr, long) (method)
        var resultContextLeftLong = context.Le(leftLong, y);             // Context.Le(long, RealExpr) (method)
        var resultContextRightBigInt = context.Le(x, rightBigInt);       // Context.Le(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Le(leftBigInt, y);         // Context.Le(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodInt);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultMethodLong);
            solver.Assert(resultMethodBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodInt));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultMethodLong));
            solver.Assert(context.Not(resultMethodBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("9.7", "1.3", true, Description = "Basic greater than - true")]
    [TestCase("2.1", "2.2", false, Description = "Less than - false")]
    [TestCase("5.5", "5.5", false, Description = "Equal values - false")]
    [TestCase("-1.0", "-2.5", true, Description = "Negative greater than negative")]
    [TestCase("0.0", "-0.1", true, Description = "Zero greater than negative")]
    public void Gt_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of greater than
        var resultOperatorRealExpr = x > y;                              // RealExpr > RealExpr (operator)
        var resultOperatorRightInt = x > rightInt;                       // RealExpr > int (operator)
        var resultOperatorLeftInt = leftInt > y;                         // int > RealExpr (operator)
        var resultOperatorRightDecimal = x > rightDecimal;               // RealExpr > decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal > y;                 // decimal > RealExpr (operator)
        var resultOperatorRightLong = x > rightLong;                     // RealExpr > long (operator)
        var resultOperatorLeftLong = leftLong > y;                       // long > RealExpr (operator)
        var resultOperatorRightBigInt = x > rightBigInt;                 // RealExpr > BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt > y;                   // BigInteger > RealExpr (operator)
        var resultMethodRealExpr = x.Gt(y);                              // RealExpr.Gt(RealExpr) (method)
        var resultMethodInt = x.Gt(rightInt);                            // RealExpr.Gt(int) (method)
        var resultMethodDecimal = x.Gt(rightDecimal);                    // RealExpr.Gt(decimal) (method)
        var resultMethodLong = x.Gt(rightLong);                          // RealExpr.Gt(long) (method)
        var resultMethodBigInt = x.Gt(rightBigInt);                      // RealExpr.Gt(BigInteger) (method)
        var resultContextRealExpr = context.Gt(x, y);                    // Context.Gt(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Gt(x, rightInt);             // Context.Gt(RealExpr, int) (method)
        var resultContextLeftInt = context.Gt(leftInt, y);               // Context.Gt(int, RealExpr) (method)
        var resultContextRightDecimal = context.Gt(x, rightDecimal);     // Context.Gt(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Gt(leftDecimal, y);       // Context.Gt(decimal, RealExpr) (method)
        var resultContextRightLong = context.Gt(x, rightLong);           // Context.Gt(RealExpr, long) (method)
        var resultContextLeftLong = context.Gt(leftLong, y);             // Context.Gt(long, RealExpr) (method)
        var resultContextRightBigInt = context.Gt(x, rightBigInt);       // Context.Gt(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Gt(leftBigInt, y);         // Context.Gt(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodInt);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultMethodLong);
            solver.Assert(resultMethodBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodInt));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultMethodLong));
            solver.Assert(context.Not(resultMethodBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("7.5", "7.5", true, Description = "Equal values - true")]
    [TestCase("9.7", "1.3", true, Description = "Greater than - true")]
    [TestCase("2.1", "2.2", false, Description = "Less than - false")]
    [TestCase("-1.0", "-2.5", true, Description = "Negative greater than equal")]
    [TestCase("0.0", "0.0", true, Description = "Zero equal")]
    public void Ge_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of greater than or equal
        var resultOperatorRealExpr = x >= y;                             // RealExpr >= RealExpr (operator)
        var resultOperatorRightInt = x >= rightInt;                      // RealExpr >= int (operator)
        var resultOperatorLeftInt = leftInt >= y;                        // int >= RealExpr (operator)
        var resultOperatorRightDecimal = x >= rightDecimal;              // RealExpr >= decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal >= y;                // decimal >= RealExpr (operator)
        var resultOperatorRightLong = x >= rightLong;                    // RealExpr >= long (operator)
        var resultOperatorLeftLong = leftLong >= y;                      // long >= RealExpr (operator)
        var resultOperatorRightBigInt = x >= rightBigInt;                // RealExpr >= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt >= y;                  // BigInteger >= RealExpr (operator)
        var resultMethodRealExpr = x.Ge(y);                              // RealExpr.Ge(RealExpr) (method)
        var resultMethodInt = x.Ge(rightInt);                            // RealExpr.Ge(int) (method)
        var resultMethodDecimal = x.Ge(rightDecimal);                    // RealExpr.Ge(decimal) (method)
        var resultMethodLong = x.Ge(rightLong);                          // RealExpr.Ge(long) (method)
        var resultMethodBigInt = x.Ge(rightBigInt);                      // RealExpr.Ge(BigInteger) (method)
        var resultContextRealExpr = context.Ge(x, y);                    // Context.Ge(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Ge(x, rightInt);             // Context.Ge(RealExpr, int) (method)
        var resultContextLeftInt = context.Ge(leftInt, y);               // Context.Ge(int, RealExpr) (method)
        var resultContextRightDecimal = context.Ge(x, rightDecimal);     // Context.Ge(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Ge(leftDecimal, y);       // Context.Ge(decimal, RealExpr) (method)
        var resultContextRightLong = context.Ge(x, rightLong);           // Context.Ge(RealExpr, long) (method)
        var resultContextLeftLong = context.Ge(leftLong, y);             // Context.Ge(long, RealExpr) (method)
        var resultContextRightBigInt = context.Ge(x, rightBigInt);       // Context.Ge(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Ge(leftBigInt, y);         // Context.Ge(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultMethodRealExpr);
            solver.Assert(resultMethodInt);
            solver.Assert(resultMethodDecimal);
            solver.Assert(resultMethodLong);
            solver.Assert(resultMethodBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultMethodRealExpr));
            solver.Assert(context.Not(resultMethodInt));
            solver.Assert(context.Not(resultMethodDecimal));
            solver.Assert(context.Not(resultMethodLong));
            solver.Assert(context.Not(resultMethodBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("3.14", "3.14", true, Description = "Equal values - true")]
    [TestCase("2.718", "2.718", true, Description = "Equal values - true")]
    [TestCase("1.0", "2.0", false, Description = "Different values - false")]
    [TestCase("-5.5", "-5.5", true, Description = "Equal negative values - true")]
    [TestCase("0.0", "0.0", true, Description = "Zero equals zero")]
    public void Eq_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of equality
        var resultOperatorRealExpr = x == y;                             // RealExpr == RealExpr (operator)
        var resultOperatorRightInt = x == rightInt;                      // RealExpr == int (operator)
        var resultOperatorLeftInt = leftInt == y;                        // int == RealExpr (operator)
        var resultOperatorRightDecimal = x == rightDecimal;              // RealExpr == decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal == y;                // decimal == RealExpr (operator)
        var resultOperatorRightLong = x == rightLong;                    // RealExpr == long (operator)
        var resultOperatorLeftLong = leftLong == y;                      // long == RealExpr (operator)
        var resultOperatorRightBigInt = x == rightBigInt;                // RealExpr == BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt == y;                  // BigInteger == RealExpr (operator)
        var resultContextRealExpr = context.Eq(x, y);                    // Context.Eq(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Eq(x, rightInt);             // Context.Eq(RealExpr, int) (method)
        var resultContextLeftInt = context.Eq(leftInt, y);               // Context.Eq(int, RealExpr) (method)
        var resultContextRightDecimal = context.Eq(x, rightDecimal);     // Context.Eq(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Eq(leftDecimal, y);       // Context.Eq(decimal, RealExpr) (method)
        var resultContextRightLong = context.Eq(x, rightLong);           // Context.Eq(RealExpr, long) (method)
        var resultContextLeftLong = context.Eq(leftLong, y);             // Context.Eq(long, RealExpr) (method)
        var resultContextRightBigInt = context.Eq(x, rightBigInt);       // Context.Eq(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Eq(leftBigInt, y);         // Context.Eq(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all equality comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all equality comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
        }

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase("1.0", "0.0", true, Description = "Different values - true")]
    [TestCase("2.5", "2.7", true, Description = "Different values - true")]
    [TestCase("3.14", "3.14", false, Description = "Equal values - false")]
    [TestCase("-5.5", "-6.6", true, Description = "Different negative values - true")]
    [TestCase("0.0", "0.0", false, Description = "Zero not equals zero - false")]
    public void Neq_AllVariations_ReturnsExpectedResult(string leftStr, string rightStr, bool expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var leftDecimal = decimal.Parse(leftStr, CultureInfo.InvariantCulture);
        var rightDecimal = decimal.Parse(rightStr, CultureInfo.InvariantCulture);

        var x = context.Real(leftDecimal);
        var y = context.Real(rightDecimal);
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of inequality
        var resultOperatorRealExpr = x != y;                             // RealExpr != RealExpr (operator)
        var resultOperatorRightInt = x != rightInt;                      // RealExpr != int (operator)
        var resultOperatorLeftInt = leftInt != y;                        // int != RealExpr (operator)
        var resultOperatorRightDecimal = x != rightDecimal;              // RealExpr != decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal != y;                // decimal != RealExpr (operator)
        var resultOperatorRightLong = x != rightLong;                    // RealExpr != long (operator)
        var resultOperatorLeftLong = leftLong != y;                      // long != RealExpr (operator)
        var resultOperatorRightBigInt = x != rightBigInt;                // RealExpr != BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt != y;                  // BigInteger != RealExpr (operator)
        var resultContextRealExpr = context.Neq(x, y);                   // Context.Neq(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Neq(x, rightInt);            // Context.Neq(RealExpr, int) (method)
        var resultContextLeftInt = context.Neq(leftInt, y);              // Context.Neq(int, RealExpr) (method)
        var resultContextRightDecimal = context.Neq(x, rightDecimal);    // Context.Neq(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Neq(leftDecimal, y);      // Context.Neq(decimal, RealExpr) (method)
        var resultContextRightLong = context.Neq(x, rightLong);          // Context.Neq(RealExpr, long) (method)
        var resultContextLeftLong = context.Neq(leftLong, y);            // Context.Neq(long, RealExpr) (method)
        var resultContextRightBigInt = context.Neq(x, rightBigInt);      // Context.Neq(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Neq(leftBigInt, y);        // Context.Neq(BigInteger, RealExpr) (method)

        // Set up constraints
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        if (expectedResult)
        {
            // If we expect true, assert all inequality comparisons and check satisfiability
            solver.Assert(resultOperatorRealExpr);
            solver.Assert(resultOperatorRightInt);
            solver.Assert(resultOperatorLeftInt);
            solver.Assert(resultOperatorRightDecimal);
            solver.Assert(resultOperatorLeftDecimal);
            solver.Assert(resultOperatorRightLong);
            solver.Assert(resultOperatorLeftLong);
            solver.Assert(resultOperatorRightBigInt);
            solver.Assert(resultOperatorLeftBigInt);
            solver.Assert(resultContextRealExpr);
            solver.Assert(resultContextRightInt);
            solver.Assert(resultContextLeftInt);
            solver.Assert(resultContextRightDecimal);
            solver.Assert(resultContextLeftDecimal);
            solver.Assert(resultContextRightLong);
            solver.Assert(resultContextLeftLong);
            solver.Assert(resultContextRightBigInt);
            solver.Assert(resultContextLeftBigInt);
        }
        else
        {
            // If we expect false, assert negation of all inequality comparisons
            solver.Assert(context.Not(resultOperatorRealExpr));
            solver.Assert(context.Not(resultOperatorRightInt));
            solver.Assert(context.Not(resultOperatorLeftInt));
            solver.Assert(context.Not(resultOperatorRightDecimal));
            solver.Assert(context.Not(resultOperatorLeftDecimal));
            solver.Assert(context.Not(resultOperatorRightLong));
            solver.Assert(context.Not(resultOperatorLeftLong));
            solver.Assert(context.Not(resultOperatorRightBigInt));
            solver.Assert(context.Not(resultOperatorLeftBigInt));
            solver.Assert(context.Not(resultContextRealExpr));
            solver.Assert(context.Not(resultContextRightInt));
            solver.Assert(context.Not(resultContextLeftInt));
            solver.Assert(context.Not(resultContextRightDecimal));
            solver.Assert(context.Not(resultContextLeftDecimal));
            solver.Assert(context.Not(resultContextRightLong));
            solver.Assert(context.Not(resultContextLeftLong));
            solver.Assert(context.Not(resultContextRightBigInt));
            solver.Assert(context.Not(resultContextLeftBigInt));
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
    public void MixedTypeComparison_AllCombinations_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.RealConst("real");
        var intValue = 5;
        var decimalValue = 5.0m;
        var longValue = 5L;
        var bigIntValue = new BigInteger(5);

        // Test that all these comparisons work and produce consistent results
        var realVsInt = realExpr == intValue;
        var realVsDecimal = realExpr == decimalValue;
        var realVsLong = realExpr == longValue;
        var realVsBigInt = realExpr == bigIntValue;

        solver.Assert(context.Eq(realExpr, context.Real(5.0m)));
        solver.Assert(realVsInt);
        solver.Assert(realVsDecimal);
        solver.Assert(realVsLong);
        solver.Assert(realVsBigInt);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}