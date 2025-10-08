using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class CharExprFactoryTests
{
    [TestCase('A')]
    [TestCase('z')]
    [TestCase('0')]
    [TestCase(' ')]
    public void CreateChar_WithCharValue_ReturnsCorrectExpression(char value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetCharValue(charExpr), Is.EqualTo(value));
    }

    [TestCase(0x41u, 'A')] // Latin A
    [TestCase(0x0030u, '0')] // Digit 0
    public void CreateChar_WithCodepoint_ReturnsCorrectExpression(uint codepoint, char expectedChar)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(codepoint);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetCharValue(charExpr), Is.EqualTo(expectedChar));
    }

    [Test]
    public void CreateCharConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charConst = context.CharConst("variableName");

        solver.Assert(charConst == 'X');
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetCharValue(charConst), Is.EqualTo('X'));
        Assert.That(charConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase('A')]
    [TestCase('z')]
    [TestCase('5')]
    public void ImplicitConversion_FromCharToCharExpr_Works(char value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        CharExpr implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetCharValue(implicitExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateMultipleCharConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.CharConst("var1");
        var char2 = context.CharConst("var2");
        var char3 = context.CharConst("var3");

        solver.Assert(char1 == 'A');
        solver.Assert(char2 == 'B');
        solver.Assert(char3 == 'C');

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetCharValue(char1), Is.EqualTo('A'));
            Assert.That(model.GetCharValue(char2), Is.EqualTo('B'));
            Assert.That(model.GetCharValue(char3), Is.EqualTo('C'));
        });
    }

    [Test]
    public void CharConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var char1 = context.CharConst("sameName");
        var char2 = context.CharConst("sameName");

        Assert.That(char1.Handle, Is.EqualTo(char2.Handle));
    }

    [Test]
    public void CharExpr_Sort_ReturnsCharSortKind()
    {
        using var context = new Z3Context();

        var sortHandle = context.GetSortForType<CharExpr>();
        var sortKind = context.Library.GetSortKind(context.Handle, sortHandle);

        Assert.That(sortKind, Is.EqualTo(Z3Library.SortKind.Z3_CHAR_SORT));
    }

    [Test]
    public void CreateChar_BothOverloads_ProduceSameResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charFromChar = context.Char('A');
        var charFromCodepoint = context.Char(0x41u);

        solver.Assert(charFromChar == charFromCodepoint);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
