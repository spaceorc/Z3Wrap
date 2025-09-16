using System.Globalization;
using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3RealExprTests;

[TestFixture]
public class Z3RealExprArithmeticTests
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
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100); // Scale for testing
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of addition
        var resultOperatorRealExpr = x + y;                              // RealExpr + RealExpr (operator)
        var resultOperatorRightInt = x + rightInt;                       // RealExpr + int (operator)
        var resultOperatorLeftInt = leftInt + y;                         // int + RealExpr (operator)
        var resultOperatorRightDecimal = x + rightDecimal;               // RealExpr + decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal + y;                 // decimal + RealExpr (operator)
        var resultOperatorRightLong = x + rightLong;                     // RealExpr + long (operator)
        var resultOperatorLeftLong = leftLong + y;                       // long + RealExpr (operator)
        var resultOperatorRightBigInt = x + rightBigInt;                 // RealExpr + BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt + y;                   // BigInteger + RealExpr (operator)
        var resultMethodRealExpr = x.Add(y);                             // RealExpr.Add(RealExpr) (method)
        var resultMethodInt = x.Add(rightInt);                           // RealExpr.Add(int) (method)
        var resultMethodDecimal = x.Add(rightDecimal);                   // RealExpr.Add(decimal) (method)
        var resultMethodLong = x.Add(rightLong);                         // RealExpr.Add(long) (method)
        var resultMethodBigInt = x.Add(rightBigInt);                     // RealExpr.Add(BigInteger) (method)
        var resultContextRealExpr = context.Add(x, y);                   // Context.Add(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Add(x, rightInt);            // Context.Add(RealExpr, int) (method)
        var resultContextLeftInt = context.Add(leftInt, y);              // Context.Add(int, RealExpr) (method)
        var resultContextRightDecimal = context.Add(x, rightDecimal);    // Context.Add(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Add(leftDecimal, y);      // Context.Add(decimal, RealExpr) (method)
        var resultContextRightLong = context.Add(x, rightLong);          // Context.Add(RealExpr, long) (method)
        var resultContextLeftLong = context.Add(leftLong, y);            // Context.Add(long, RealExpr) (method)
        var resultContextRightBigInt = context.Add(x, rightBigInt);      // Context.Add(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Add(leftBigInt, y);        // Context.Add(BigInteger, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultOperatorRealExpr), Is.EqualTo(new Real(expected)), "RealExpr + RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightInt), Is.EqualTo(new Real(leftDecimal + rightInt)), "RealExpr + int operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftInt), Is.EqualTo(new Real(leftInt + rightDecimal)), "int + RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightDecimal), Is.EqualTo(new Real(expected)), "RealExpr + decimal operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftDecimal), Is.EqualTo(new Real(expected)), "decimal + RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightLong), Is.EqualTo(new Real(leftDecimal + rightLong)), "RealExpr + long operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftLong), Is.EqualTo(new Real(leftLong + rightDecimal)), "long + RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightBigInt), Is.EqualTo(new Real(leftDecimal + (decimal)rightBigInt)), "RealExpr + BigInteger operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt + rightDecimal)), "BigInteger + RealExpr operator failed");
            Assert.That(model.GetRealValue(resultMethodRealExpr), Is.EqualTo(new Real(expected)), "RealExpr.Add(RealExpr) method failed");
            Assert.That(model.GetRealValue(resultMethodInt), Is.EqualTo(new Real(leftDecimal + rightInt)), "RealExpr.Add(int) method failed");
            Assert.That(model.GetRealValue(resultMethodDecimal), Is.EqualTo(new Real(expected)), "RealExpr.Add(decimal) method failed");
            Assert.That(model.GetRealValue(resultMethodLong), Is.EqualTo(new Real(leftDecimal + rightLong)), "RealExpr.Add(long) method failed");
            Assert.That(model.GetRealValue(resultMethodBigInt), Is.EqualTo(new Real(leftDecimal + (decimal)rightBigInt)), "RealExpr.Add(BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextRealExpr), Is.EqualTo(new Real(expected)), "Context.Add(RealExpr, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightInt), Is.EqualTo(new Real(leftDecimal + rightInt)), "Context.Add(RealExpr, int) method failed");
            Assert.That(model.GetRealValue(resultContextLeftInt), Is.EqualTo(new Real(leftInt + rightDecimal)), "Context.Add(int, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightDecimal), Is.EqualTo(new Real(expected)), "Context.Add(RealExpr, decimal) method failed");
            Assert.That(model.GetRealValue(resultContextLeftDecimal), Is.EqualTo(new Real(expected)), "Context.Add(decimal, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightLong), Is.EqualTo(new Real(leftDecimal + rightLong)), "Context.Add(RealExpr, long) method failed");
            Assert.That(model.GetRealValue(resultContextLeftLong), Is.EqualTo(new Real(leftLong + rightDecimal)), "Context.Add(long, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightBigInt), Is.EqualTo(new Real(leftDecimal + (decimal)rightBigInt)), "Context.Add(RealExpr, BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt + rightDecimal)), "Context.Add(BigInteger, RealExpr) method failed");
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
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of subtraction
        var resultOperatorRealExpr = x - y;                              // RealExpr - RealExpr (operator)
        var resultOperatorRightInt = x - rightInt;                       // RealExpr - int (operator)
        var resultOperatorLeftInt = leftInt - y;                         // int - RealExpr (operator)
        var resultOperatorRightDecimal = x - rightDecimal;               // RealExpr - decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal - y;                 // decimal - RealExpr (operator)
        var resultOperatorRightLong = x - rightLong;                     // RealExpr - long (operator)
        var resultOperatorLeftLong = leftLong - y;                       // long - RealExpr (operator)
        var resultOperatorRightBigInt = x - rightBigInt;                 // RealExpr - BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt - y;                   // BigInteger - RealExpr (operator)
        var resultMethodRealExpr = x.Sub(y);                             // RealExpr.Sub(RealExpr) (method)
        var resultMethodInt = x.Sub(rightInt);                           // RealExpr.Sub(int) (method)
        var resultMethodDecimal = x.Sub(rightDecimal);                   // RealExpr.Sub(decimal) (method)
        var resultMethodLong = x.Sub(rightLong);                         // RealExpr.Sub(long) (method)
        var resultMethodBigInt = x.Sub(rightBigInt);                     // RealExpr.Sub(BigInteger) (method)
        var resultContextRealExpr = context.Sub(x, y);                   // Context.Sub(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Sub(x, rightInt);            // Context.Sub(RealExpr, int) (method)
        var resultContextLeftInt = context.Sub(leftInt, y);              // Context.Sub(int, RealExpr) (method)
        var resultContextRightDecimal = context.Sub(x, rightDecimal);    // Context.Sub(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Sub(leftDecimal, y);      // Context.Sub(decimal, RealExpr) (method)
        var resultContextRightLong = context.Sub(x, rightLong);          // Context.Sub(RealExpr, long) (method)
        var resultContextLeftLong = context.Sub(leftLong, y);            // Context.Sub(long, RealExpr) (method)
        var resultContextRightBigInt = context.Sub(x, rightBigInt);      // Context.Sub(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Sub(leftBigInt, y);        // Context.Sub(BigInteger, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultOperatorRealExpr), Is.EqualTo(new Real(expected)), "RealExpr - RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightInt), Is.EqualTo(new Real(leftDecimal - rightInt)), "RealExpr - int operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftInt), Is.EqualTo(new Real(leftInt - rightDecimal)), "int - RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightDecimal), Is.EqualTo(new Real(expected)), "RealExpr - decimal operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftDecimal), Is.EqualTo(new Real(expected)), "decimal - RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightLong), Is.EqualTo(new Real(leftDecimal - rightLong)), "RealExpr - long operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftLong), Is.EqualTo(new Real(leftLong - rightDecimal)), "long - RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightBigInt), Is.EqualTo(new Real(leftDecimal - (decimal)rightBigInt)), "RealExpr - BigInteger operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt - rightDecimal)), "BigInteger - RealExpr operator failed");
            Assert.That(model.GetRealValue(resultMethodRealExpr), Is.EqualTo(new Real(expected)), "RealExpr.Sub(RealExpr) method failed");
            Assert.That(model.GetRealValue(resultMethodInt), Is.EqualTo(new Real(leftDecimal - rightInt)), "RealExpr.Sub(int) method failed");
            Assert.That(model.GetRealValue(resultMethodDecimal), Is.EqualTo(new Real(expected)), "RealExpr.Sub(decimal) method failed");
            Assert.That(model.GetRealValue(resultMethodLong), Is.EqualTo(new Real(leftDecimal - rightLong)), "RealExpr.Sub(long) method failed");
            Assert.That(model.GetRealValue(resultMethodBigInt), Is.EqualTo(new Real(leftDecimal - (decimal)rightBigInt)), "RealExpr.Sub(BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextRealExpr), Is.EqualTo(new Real(expected)), "Context.Sub(RealExpr, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightInt), Is.EqualTo(new Real(leftDecimal - rightInt)), "Context.Sub(RealExpr, int) method failed");
            Assert.That(model.GetRealValue(resultContextLeftInt), Is.EqualTo(new Real(leftInt - rightDecimal)), "Context.Sub(int, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightDecimal), Is.EqualTo(new Real(expected)), "Context.Sub(RealExpr, decimal) method failed");
            Assert.That(model.GetRealValue(resultContextLeftDecimal), Is.EqualTo(new Real(expected)), "Context.Sub(decimal, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightLong), Is.EqualTo(new Real(leftDecimal - rightLong)), "Context.Sub(RealExpr, long) method failed");
            Assert.That(model.GetRealValue(resultContextLeftLong), Is.EqualTo(new Real(leftLong - rightDecimal)), "Context.Sub(long, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightBigInt), Is.EqualTo(new Real(leftDecimal - (decimal)rightBigInt)), "Context.Sub(RealExpr, BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt - rightDecimal)), "Context.Sub(BigInteger, RealExpr) method failed");
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
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of multiplication
        var resultOperatorRealExpr = x * y;                              // RealExpr * RealExpr (operator)
        var resultOperatorRightInt = x * rightInt;                       // RealExpr * int (operator)
        var resultOperatorLeftInt = leftInt * y;                         // int * RealExpr (operator)
        var resultOperatorRightDecimal = x * rightDecimal;               // RealExpr * decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal * y;                 // decimal * RealExpr (operator)
        var resultOperatorRightLong = x * rightLong;                     // RealExpr * long (operator)
        var resultOperatorLeftLong = leftLong * y;                       // long * RealExpr (operator)
        var resultOperatorRightBigInt = x * rightBigInt;                 // RealExpr * BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt * y;                   // BigInteger * RealExpr (operator)
        var resultMethodRealExpr = x.Mul(y);                             // RealExpr.Mul(RealExpr) (method)
        var resultMethodInt = x.Mul(rightInt);                           // RealExpr.Mul(int) (method)
        var resultMethodDecimal = x.Mul(rightDecimal);                   // RealExpr.Mul(decimal) (method)
        var resultMethodLong = x.Mul(rightLong);                         // RealExpr.Mul(long) (method)
        var resultMethodBigInt = x.Mul(rightBigInt);                     // RealExpr.Mul(BigInteger) (method)
        var resultContextRealExpr = context.Mul(x, y);                   // Context.Mul(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Mul(x, rightInt);            // Context.Mul(RealExpr, int) (method)
        var resultContextLeftInt = context.Mul(leftInt, y);              // Context.Mul(int, RealExpr) (method)
        var resultContextRightDecimal = context.Mul(x, rightDecimal);    // Context.Mul(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Mul(leftDecimal, y);      // Context.Mul(decimal, RealExpr) (method)
        var resultContextRightLong = context.Mul(x, rightLong);          // Context.Mul(RealExpr, long) (method)
        var resultContextLeftLong = context.Mul(leftLong, y);            // Context.Mul(long, RealExpr) (method)
        var resultContextRightBigInt = context.Mul(x, rightBigInt);      // Context.Mul(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Mul(leftBigInt, y);        // Context.Mul(BigInteger, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultOperatorRealExpr), Is.EqualTo(new Real(expected)), "RealExpr * RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightInt), Is.EqualTo(new Real(leftDecimal * rightInt)), "RealExpr * int operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftInt), Is.EqualTo(new Real(leftInt * rightDecimal)), "int * RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightDecimal), Is.EqualTo(new Real(expected)), "RealExpr * decimal operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftDecimal), Is.EqualTo(new Real(expected)), "decimal * RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightLong), Is.EqualTo(new Real(leftDecimal * rightLong)), "RealExpr * long operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftLong), Is.EqualTo(new Real(leftLong * rightDecimal)), "long * RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightBigInt), Is.EqualTo(new Real(leftDecimal * (decimal)rightBigInt)), "RealExpr * BigInteger operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt * rightDecimal)), "BigInteger * RealExpr operator failed");
            Assert.That(model.GetRealValue(resultMethodRealExpr), Is.EqualTo(new Real(expected)), "RealExpr.Mul(RealExpr) method failed");
            Assert.That(model.GetRealValue(resultMethodInt), Is.EqualTo(new Real(leftDecimal * rightInt)), "RealExpr.Mul(int) method failed");
            Assert.That(model.GetRealValue(resultMethodDecimal), Is.EqualTo(new Real(expected)), "RealExpr.Mul(decimal) method failed");
            Assert.That(model.GetRealValue(resultMethodLong), Is.EqualTo(new Real(leftDecimal * rightLong)), "RealExpr.Mul(long) method failed");
            Assert.That(model.GetRealValue(resultMethodBigInt), Is.EqualTo(new Real(leftDecimal * (decimal)rightBigInt)), "RealExpr.Mul(BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextRealExpr), Is.EqualTo(new Real(expected)), "Context.Mul(RealExpr, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightInt), Is.EqualTo(new Real(leftDecimal * rightInt)), "Context.Mul(RealExpr, int) method failed");
            Assert.That(model.GetRealValue(resultContextLeftInt), Is.EqualTo(new Real(leftInt * rightDecimal)), "Context.Mul(int, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightDecimal), Is.EqualTo(new Real(expected)), "Context.Mul(RealExpr, decimal) method failed");
            Assert.That(model.GetRealValue(resultContextLeftDecimal), Is.EqualTo(new Real(expected)), "Context.Mul(decimal, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightLong), Is.EqualTo(new Real(leftDecimal * rightLong)), "Context.Mul(RealExpr, long) method failed");
            Assert.That(model.GetRealValue(resultContextLeftLong), Is.EqualTo(new Real(leftLong * rightDecimal)), "Context.Mul(long, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightBigInt), Is.EqualTo(new Real(leftDecimal * (decimal)rightBigInt)), "Context.Mul(RealExpr, BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextLeftBigInt), Is.EqualTo(new Real((decimal)leftBigInt * rightDecimal)), "Context.Mul(BigInteger, RealExpr) method failed");
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
        var leftInt = (int)leftDecimal;
        var rightInt = (int)rightDecimal;
        var leftLong = (long)(leftDecimal * 100);
        var rightLong = (long)(rightDecimal * 100);
        var leftBigInt = new BigInteger(leftDecimal);
        var rightBigInt = new BigInteger(rightDecimal);

        // Test all variations of division
        var resultOperatorRealExpr = x / y;                              // RealExpr / RealExpr (operator)
        var resultOperatorRightInt = x / rightInt;                       // RealExpr / int (operator)
        var resultOperatorLeftInt = leftInt / y;                         // int / RealExpr (operator)
        var resultOperatorRightDecimal = x / rightDecimal;               // RealExpr / decimal (operator)
        var resultOperatorLeftDecimal = leftDecimal / y;                 // decimal / RealExpr (operator)
        var resultOperatorRightLong = x / rightLong;                     // RealExpr / long (operator)
        var resultOperatorLeftLong = leftLong / y;                       // long / RealExpr (operator)
        var resultOperatorRightBigInt = x / rightBigInt;                 // RealExpr / BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt / y;                   // BigInteger / RealExpr (operator)
        var resultMethodRealExpr = x.Div(y);                             // RealExpr.Div(RealExpr) (method)
        var resultMethodInt = x.Div(rightInt);                           // RealExpr.Div(int) (method)
        var resultMethodDecimal = x.Div(rightDecimal);                   // RealExpr.Div(decimal) (method)
        var resultMethodLong = x.Div(rightLong);                         // RealExpr.Div(long) (method)
        var resultMethodBigInt = x.Div(rightBigInt);                     // RealExpr.Div(BigInteger) (method)
        var resultContextRealExpr = context.Div(x, y);                   // Context.Div(RealExpr, RealExpr) (method)
        var resultContextRightInt = context.Div(x, rightInt);            // Context.Div(RealExpr, int) (method)
        var resultContextLeftInt = context.Div(leftInt, y);              // Context.Div(int, RealExpr) (method)
        var resultContextRightDecimal = context.Div(x, rightDecimal);    // Context.Div(RealExpr, decimal) (method)
        var resultContextLeftDecimal = context.Div(leftDecimal, y);      // Context.Div(decimal, RealExpr) (method)
        var resultContextRightLong = context.Div(x, rightLong);          // Context.Div(RealExpr, long) (method)
        var resultContextLeftLong = context.Div(leftLong, y);            // Context.Div(long, RealExpr) (method)
        var resultContextRightBigInt = context.Div(x, rightBigInt);      // Context.Div(RealExpr, BigInteger) (method)
        var resultContextLeftBigInt = context.Div(leftBigInt, y);        // Context.Div(BigInteger, RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(leftDecimal)));
        solver.Assert(context.Eq(y, context.Real(rightDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = expectedDecimal;

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultOperatorRealExpr), Is.EqualTo(new Real(expected)), "RealExpr / RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightInt), Is.EqualTo(new Real(leftDecimal / rightInt)), "RealExpr / int operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftInt), Is.EqualTo(new Real(leftInt) / rightDecimal), "int / RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightDecimal), Is.EqualTo(new Real(expected)), "RealExpr / decimal operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftDecimal), Is.EqualTo(new Real(expected)), "decimal / RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightLong), Is.EqualTo(new Real(leftDecimal) / rightLong), "RealExpr / long operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftLong), Is.EqualTo(new Real(leftLong) / rightDecimal), "long / RealExpr operator failed");
            Assert.That(model.GetRealValue(resultOperatorRightBigInt), Is.EqualTo(new Real(leftDecimal) / rightBigInt), "RealExpr / BigInteger operator failed");
            Assert.That(model.GetRealValue(resultOperatorLeftBigInt), Is.EqualTo(new Real(leftBigInt) / rightDecimal), "BigInteger / RealExpr operator failed");
            Assert.That(model.GetRealValue(resultMethodRealExpr), Is.EqualTo(new Real(expected)), "RealExpr.Div(RealExpr) method failed");
            Assert.That(model.GetRealValue(resultMethodInt), Is.EqualTo(new Real(leftDecimal) / rightInt), "RealExpr.Div(int) method failed");
            Assert.That(model.GetRealValue(resultMethodDecimal), Is.EqualTo(new Real(expected)), "RealExpr.Div(decimal) method failed");
            Assert.That(model.GetRealValue(resultMethodLong), Is.EqualTo(new Real(leftDecimal) / rightLong), "RealExpr.Div(long) method failed");
            Assert.That(model.GetRealValue(resultMethodBigInt), Is.EqualTo(new Real(leftDecimal) / rightBigInt), "RealExpr.Div(BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextRealExpr), Is.EqualTo(new Real(expected)), "Context.Div(RealExpr, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightInt), Is.EqualTo(new Real(leftDecimal) / rightInt), "Context.Div(RealExpr, int) method failed");
            Assert.That(model.GetRealValue(resultContextLeftInt), Is.EqualTo(new Real(leftInt) / rightDecimal), "Context.Div(int, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightDecimal), Is.EqualTo(new Real(expected)), "Context.Div(RealExpr, decimal) method failed");
            Assert.That(model.GetRealValue(resultContextLeftDecimal), Is.EqualTo(new Real(expected)), "Context.Div(decimal, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightLong), Is.EqualTo(new Real(leftDecimal) / rightLong), "Context.Div(RealExpr, long) method failed");
            Assert.That(model.GetRealValue(resultContextLeftLong), Is.EqualTo(new Real(leftLong) / rightDecimal), "Context.Div(long, RealExpr) method failed");
            Assert.That(model.GetRealValue(resultContextRightBigInt), Is.EqualTo(new Real(leftDecimal) / rightBigInt), "Context.Div(RealExpr, BigInteger) method failed");
            Assert.That(model.GetRealValue(resultContextLeftBigInt), Is.EqualTo(new Real(leftBigInt) / rightDecimal), "Context.Div(BigInteger, RealExpr) method failed");
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
        var resultOperator = -x;                        // -RealExpr (unary operator)
        var resultMethod = x.UnaryMinus();              // RealExpr.UnaryMinus() (method)
        var resultContext = context.UnaryMinus(x);     // Context.UnaryMinus(RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(valueDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultOperator), Is.EqualTo(new Real(expected)), "-RealExpr unary operator failed");
            Assert.That(model.GetRealValue(resultMethod), Is.EqualTo(new Real(expected)), "RealExpr.UnaryMinus() method failed");
            Assert.That(model.GetRealValue(resultContext), Is.EqualTo(new Real(expected)), "Context.UnaryMinus(RealExpr) method failed");
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
        var resultMethod = x.Abs();            // RealExpr.Abs() (method)
        var resultContext = context.Abs(x);    // Context.Abs(RealExpr) (method)

        // Set up constraints to get specific values for evaluation
        solver.Assert(context.Eq(x, context.Real(valueDecimal)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(resultMethod), Is.EqualTo(new Real(expected)), "RealExpr.Abs() method failed");
            Assert.That(model.GetRealValue(resultContext), Is.EqualTo(new Real(expected)), "Context.Abs(RealExpr) method failed");
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
    public void PrimitiveInteraction_RealExprWithLiterals_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // Test various primitive interactions
        var result1 = x + 10.5m;       // RealExpr + decimal
        var result2 = 20.7m + x;       // decimal + RealExpr
        var result3 = x - 3.2m;        // RealExpr - decimal
        var result4 = 100.8m - x;      // decimal - RealExpr
        var result5 = x * 4.1m;        // RealExpr * decimal
        var result6 = 7.3m * x;        // decimal * RealExpr

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
}