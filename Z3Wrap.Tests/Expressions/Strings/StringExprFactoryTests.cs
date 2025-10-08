using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprFactoryTests
{
    [TestCase("hello")]
    [TestCase("")]
    [TestCase("Hello, World!")]
    public void CreateString_WithLiteralValue_ReturnsCorrectExpression(string value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var stringExpr = context.String(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetStringValue(stringExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateStringConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var stringConst = context.StringConst("variableName");

        solver.Assert(stringConst == "hello");
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetStringValue(stringConst), Is.EqualTo("hello"));
        Assert.That(stringConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase("test")]
    [TestCase("")]
    [TestCase("Hello")]
    public void ImplicitConversion_FromStringToStringExpr_Works(string value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        StringExpr implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetStringValue(implicitExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateMultipleStringConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.StringConst("var1");
        var str2 = context.StringConst("var2");
        var str3 = context.StringConst("var3");

        solver.Assert(str1 == "hello");
        solver.Assert(str2 == "world");
        solver.Assert(str3 == "test");

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(str1), Is.EqualTo("hello"));
            Assert.That(model.GetStringValue(str2), Is.EqualTo("world"));
            Assert.That(model.GetStringValue(str3), Is.EqualTo("test"));
        });
    }

    [Test]
    public void StringConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var str1 = context.StringConst("sameName");
        var str2 = context.StringConst("sameName");

        Assert.That(str1.Handle, Is.EqualTo(str2.Handle));
    }

    [Test]
    public void StringExpr_Sort_ReturnsStringSortKind()
    {
        using var context = new Z3Context();

        var sortHandle = context.GetSortForType<StringExpr>();
        var sortKind = context.Library.GetSortKind(context.Handle, sortHandle);

        Assert.That(sortKind, Is.EqualTo(Z3Library.SortKind.Z3_SEQ_SORT));
    }

    [Test]
    public void CreateString_WithUnicodeCharacters_ReturnsEscapedString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var stringExpr = context.String("你好");

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // Z3 returns Unicode as escape sequences in string representation
        var result = model.GetStringValue(stringExpr);
        Assert.That(result, Does.Contain("\\u{"));
    }
}
