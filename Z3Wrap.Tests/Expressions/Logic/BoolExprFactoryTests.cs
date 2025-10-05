using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Z3Wrap.Tests.Expressions.Logic;

[TestFixture]
public class BoolExprFactoryTests
{
    [TestCase(true)]
    [TestCase(false)]
    public void CreateBool_WithLiteralValue_ReturnsCorrectExpression(bool value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var boolExpr = context.Bool(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(boolExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateBoolConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var boolConst = context.BoolConst("variableName");

        solver.Assert(boolConst);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(boolConst), Is.True);
        Assert.That(boolConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ImplicitConversion_FromBoolToBoolExpr_Works(bool value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        BoolExpr implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(implicitExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateTrueAndFalse_ReturnCorrectExpressions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var trueExpr = context.True();
        var falseExpr = context.False();

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(trueExpr), Is.True);
            Assert.That(model.GetBoolValue(falseExpr), Is.False);
        });
    }

    [Test]
    public void CreateMultipleBoolConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bool1 = context.BoolConst("var1");
        var bool2 = context.BoolConst("var2");
        var bool3 = context.BoolConst("var3");

        solver.Assert(bool1);
        solver.Assert(!bool2);
        solver.Assert(bool3);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(bool1), Is.True);
            Assert.That(model.GetBoolValue(bool2), Is.False);
            Assert.That(model.GetBoolValue(bool3), Is.True);
        });
    }

    [Test]
    public void BoolConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bool1 = context.BoolConst("sameName");
        var bool2 = context.BoolConst("sameName");

        Assert.That(bool1.Handle, Is.EqualTo(bool2.Handle));
    }

    [Test]
    public void BoolExpr_Sort_ReturnsBoolSortKind()
    {
        using var context = new Z3Context();

        var sortHandle = context.GetSortForType<BoolExpr>();
        var sortKind = context.Library.GetSortKind(context.Handle, sortHandle);

        Assert.That(sortKind, Is.EqualTo(Z3Library.SortKind.Z3_BOOL_SORT));
    }
}
