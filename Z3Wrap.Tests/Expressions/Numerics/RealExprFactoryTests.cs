using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

/// <summary>
/// Tests for real expression creation methods.
/// </summary>
[TestFixture]
public class RealExprFactoryTests
{
    [TestCase(42.5)]
    [TestCase(0.0)]
    [TestCase(-10.75)]
    public void CreateReal_WithLiteralValue_ReturnsCorrectExpression(double val)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = (decimal)val;
        var realExpr = context.Real(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(realExpr).ToDecimal(), Is.EqualTo(value));
    }

    [Test]
    public void CreateRealConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realConst = context.RealConst("variableName");

        solver.Assert(realConst == 42.5m);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(realConst).ToDecimal(), Is.EqualTo(42.5m));
        Assert.That(realConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase(42.5)]
    [TestCase(0.0)]
    [TestCase(-10.75)]
    public void ImplicitConversion_FromDecimalToRealExpr_Works(double val)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = (decimal)val;
        RealExpr implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(implicitExpr).ToDecimal(), Is.EqualTo(value));
    }

    [Test]
    public void CreateReal_FromInt_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.Real(42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(realExpr).ToDecimal(), Is.EqualTo(42m));
    }

    [Test]
    public void CreateReal_FromLong_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.Real(9876543210L);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(realExpr).ToDecimal(), Is.EqualTo(9876543210m));
    }

    [Test]
    public void CreateReal_FromBigInteger_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("123456789012345678901234567890");
        var realExpr = context.Real(bigValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var result = model.GetRealValue(realExpr);
        Assert.That(result.Numerator, Is.EqualTo(bigValue));
        Assert.That(result.Denominator, Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void CreateReal_FromRational_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realExpr = context.Real(new Real(1, 3));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(realExpr), Is.EqualTo(new Real(1, 3)));
    }

    [Test]
    public void ImplicitConversion_FromIntToRealExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        RealExpr implicitExpr = 42;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(implicitExpr).ToDecimal(), Is.EqualTo(42m));
    }

    [Test]
    public void ImplicitConversion_FromLongToRealExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        RealExpr implicitExpr = 9876543210L;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(implicitExpr).ToDecimal(), Is.EqualTo(9876543210m));
    }

    [Test]
    public void ImplicitConversion_FromBigIntegerToRealExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("123456789012345678901234567890");
        RealExpr implicitExpr = bigValue;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var result = model.GetRealValue(implicitExpr);
        Assert.That(result.Numerator, Is.EqualTo(bigValue));
        Assert.That(result.Denominator, Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void ImplicitConversion_FromRealValueToRealExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realValue = new Real(1, 3);
        RealExpr implicitExpr = realValue;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetRealValue(implicitExpr), Is.EqualTo(new Real(1, 3)));
    }

    [Test]
    public void CreateMultipleRealConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var real1 = context.RealConst("var1");
        var real2 = context.RealConst("var2");
        var real3 = context.RealConst("var3");

        solver.Assert(real1 == 10.5m);
        solver.Assert(real2 == 20.5m);
        solver.Assert(real3 == 30.5m);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(real1).ToDecimal(), Is.EqualTo(10.5m));
            Assert.That(model.GetRealValue(real2).ToDecimal(), Is.EqualTo(20.5m));
            Assert.That(model.GetRealValue(real3).ToDecimal(), Is.EqualTo(30.5m));
        });
    }

    [Test]
    public void RealConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var real1 = context.RealConst("sameName");
        var real2 = context.RealConst("sameName");

        Assert.That(real1.Handle, Is.EqualTo(real2.Handle));
    }

    [Test]
    public void RealExpr_Sort_ReturnsRealSortKind()
    {
        using var context = new Z3Context();

        var sortHandle = context.GetSortForType<RealExpr>();
        var sortKind = context.Library.Z3GetSortKind(context.Handle, sortHandle);

        Assert.That(sortKind, Is.EqualTo(Z3SortKind.Real));
    }
}
