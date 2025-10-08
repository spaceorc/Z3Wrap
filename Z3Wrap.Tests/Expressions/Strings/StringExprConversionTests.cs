using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprConversionTests
{
    [TestCase("42", 42)]
    [TestCase("0", 0)]
    [TestCase("999", 999)]
    public void ToInt_ValidNumericString_ReturnsCorrectInteger(string strValue, int expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String(strValue);
        var intResult = str.ToInt();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intResult), Is.EqualTo(new BigInteger(expected)));
    }

    [Test]
    public void ToInt_WithVariable_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.StringConst("numStr");
        var intResult = str.ToInt();

        solver.Assert(intResult == 123);
        solver.Assert(str.Length() == 3);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(str), Is.EqualTo("123"));
    }

    [TestCase(42, "42")]
    [TestCase(0, "0")]
    [TestCase(999, "999")]
    public void ToStr_Integer_ReturnsCorrectString(int intValue, string expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intExpr = context.Int(intValue);
        var strResult = intExpr.ToStr();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(strResult), Is.EqualTo(expected));
    }

    [Test]
    public void ToStr_WithVariable_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intVar = context.IntConst("num");
        var strResult = intVar.ToStr();

        solver.Assert(strResult == "456");
        solver.Assert(intVar >= 0);
        solver.Assert(intVar <= 1000);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intVar), Is.EqualTo(new BigInteger(456)));
    }

    [Test]
    public void ToInt_ToStr_RoundTrip_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var original = context.Int(123);
        var asString = original.ToStr();
        var backToInt = asString.ToInt();

        solver.Assert(backToInt == 123);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(asString), Is.EqualTo("123"));
            Assert.That(model.GetIntValue(backToInt), Is.EqualTo(new BigInteger(123)));
        });
    }

    [Test]
    public void ToStr_ToInt_RoundTrip_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var original = context.String("789");
        var asInt = original.ToInt();
        var backToStr = asInt.ToStr();

        solver.Assert(backToStr == "789");

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(asInt), Is.EqualTo(new BigInteger(789)));
            Assert.That(model.GetStringValue(backToStr), Is.EqualTo("789"));
        });
    }

    [Test]
    public void ToInt_LargeNumber_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = new BigInteger(123456789);
        var intExpr = context.Int(bigValue);
        var strResult = intExpr.ToStr();
        var backToInt = strResult.ToInt();

        solver.Assert(backToInt == intExpr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(backToInt), Is.EqualTo(bigValue));
    }

    [Test]
    public void ToInt_NegativeNumberString_ReturnsMinusOne()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("-100");
        var intResult = str.ToInt();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Z3's str.to.int returns -1 for strings that don't represent non-negative integers
        Assert.That(model.GetIntValue(intResult), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void ToStr_NegativeNumber_ReturnsEmptyString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var negativeInt = context.Int(-42);
        var strResult = negativeInt.ToStr();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Z3's int.to.str returns empty string for negative numbers
        Assert.That(model.GetStringValue(strResult), Is.EqualTo(""));
    }

    [Test]
    public void ToInt_ContextMethod_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("567");
        var intResult = context.StrToInt(str);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intResult), Is.EqualTo(new BigInteger(567)));
    }

    [Test]
    public void ToStr_ContextMethod_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intExpr = context.Int(890);
        var strResult = context.ToStr(intExpr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(strResult), Is.EqualTo("890"));
    }
}
