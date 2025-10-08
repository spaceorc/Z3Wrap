using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class CharExprOperationsTests
{
    [TestCase('A', 65)]
    [TestCase('0', 48)]
    [TestCase('z', 122)]
    public void ToInt_ReturnsCorrectCodepoint(char ch, int expectedCodepoint)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(ch);
        var intResult = charExpr.ToInt();
        var intResultViaContext = context.ToInt(charExpr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var evaluatedResult = model.Evaluate(intResult);
        var evaluatedResultViaContext = model.Evaluate(intResultViaContext);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(evaluatedResult), Is.EqualTo(new BigInteger(expectedCodepoint)));
            Assert.That(model.GetIntValue(evaluatedResultViaContext), Is.EqualTo(new BigInteger(expectedCodepoint)));
        });
    }

    // TODO: Discover actual bitvector size from Z3 and update this test
    // [Test]
    // public void ToBv_ReturnsCorrectBitvector()
    // {
    //     using var context = new Z3Context();
    //     using var scope = context.SetUp();
    //     using var solver = context.CreateSolver();
    //
    //     var charExpr = context.Char('A');
    //     var bvResult = charExpr.ToBv<???>();  // Need to discover size
    //     var bvResultViaContext = context.ToBv<???>(charExpr);
    //
    //     var status = solver.Check();
    //     Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    //
    //     var model = solver.GetModel();
    //     var evaluatedResult = model.Evaluate(bvResult);
    //     var evaluatedResultViaContext = model.Evaluate(bvResultViaContext);
    //
    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(model.GetBv(evaluatedResult).Value, Is.EqualTo(new BigInteger(65)));
    //         Assert.That(model.GetBv(evaluatedResultViaContext).Value, Is.EqualTo(new BigInteger(65)));
    //     });
    // }

    [TestCase('0')]
    [TestCase('5')]
    [TestCase('9')]
    public void IsDigit_WithDigits_ReturnsTrue(char digit)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(digit);
        var result = charExpr.IsDigit();
        var resultViaContext = context.IsDigit(charExpr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.True);
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
        });
    }

    [TestCase('A')]
    [TestCase('z')]
    [TestCase(' ')]
    [TestCase('!')]
    public void IsDigit_WithNonDigits_ReturnsFalse(char nonDigit)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(nonDigit);
        var result = charExpr.IsDigit();
        var resultViaContext = context.IsDigit(charExpr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.False);
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
        });
    }

    [TestCase('A', 'B', true)]
    [TestCase('B', 'A', false)]
    [TestCase('X', 'X', true)]
    public void Le_TwoCharacters_ComputesCorrectResult(char char1Value, char char2Value, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char(char1Value);
        var char2 = context.Char(char2Value);

        var result = char1 <= char2;
        var resultViaCharLeft = char1Value <= char2;
        var resultViaCharRight = char1 <= char2Value;
        var resultViaContext = context.Le(char1, char2);
        var resultViaContextCharLeft = context.Le(char1Value, char2);
        var resultViaContextCharRight = context.Le(char1, char2Value);
        var resultViaFunc = char1.Le(char2);
        var resultViaFuncCharRight = char1.Le(char2Value);

        solver.Assert(result == expected);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncCharRight), Is.EqualTo(expected));
        });
    }

    [TestCase('Z', 'A', true)]
    [TestCase('A', 'Z', false)]
    [TestCase('M', 'M', true)]
    public void Ge_TwoCharacters_ComputesCorrectResult(char char1Value, char char2Value, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char(char1Value);
        var char2 = context.Char(char2Value);

        var result = char1 >= char2;
        var resultViaCharLeft = char1Value >= char2;
        var resultViaCharRight = char1 >= char2Value;
        var resultViaContext = context.Ge(char1, char2);
        var resultViaContextCharLeft = context.Ge(char1Value, char2);
        var resultViaContextCharRight = context.Ge(char1, char2Value);
        var resultViaFunc = char1.Ge(char2);
        var resultViaFuncCharRight = char1.Ge(char2Value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncCharRight), Is.EqualTo(expected));
        });
    }

    [TestCase('A', 'B', true)]
    [TestCase('B', 'A', false)]
    [TestCase('M', 'M', false)]
    public void Lt_TwoCharacters_ComputesCorrectResult(char char1Value, char char2Value, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char(char1Value);
        var char2 = context.Char(char2Value);

        var result = char1 < char2;
        var resultViaCharLeft = char1Value < char2;
        var resultViaCharRight = char1 < char2Value;
        var resultViaContext = context.Lt(char1, char2);
        var resultViaContextCharLeft = context.Lt(char1Value, char2);
        var resultViaContextCharRight = context.Lt(char1, char2Value);
        var resultViaFunc = char1.Lt(char2);
        var resultViaFuncCharRight = char1.Lt(char2Value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncCharRight), Is.EqualTo(expected));
        });
    }

    [TestCase('Z', 'A', true)]
    [TestCase('A', 'Z', false)]
    [TestCase('K', 'K', false)]
    public void Gt_TwoCharacters_ComputesCorrectResult(char char1Value, char char2Value, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char(char1Value);
        var char2 = context.Char(char2Value);

        var result = char1 > char2;
        var resultViaCharLeft = char1Value > char2;
        var resultViaCharRight = char1 > char2Value;
        var resultViaContext = context.Gt(char1, char2);
        var resultViaContextCharLeft = context.Gt(char1Value, char2);
        var resultViaContextCharRight = context.Gt(char1, char2Value);
        var resultViaFunc = char1.Gt(char2);
        var resultViaFuncCharRight = char1.Gt(char2Value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextCharRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncCharRight), Is.EqualTo(expected));
        });
    }

    [Test]
    public void Eq_EqualCharacters_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('T');
        var char2 = context.Char('T');
        var result = char1 == char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Eq_DifferentCharacters_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('X');
        var char2 = context.Char('Y');
        var result = char1 == char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Ne_DifferentCharacters_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('A');
        var char2 = context.Char('B');
        var result = char1 != char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Ne_EqualCharacters_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('K');
        var char2 = context.Char('K');
        var result = char1 != char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }
}
