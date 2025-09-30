using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

/// <summary>
/// Tests for integer expression creation methods.
/// </summary>
[TestFixture]
public class IntExprFactoryTests
{
    [TestCase(42)]
    [TestCase(0)]
    [TestCase(-100)]
    public void CreateInt_WithLiteralValue_ReturnsCorrectExpression(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intExpr = context.Int(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intExpr), Is.EqualTo(new BigInteger(value)));
    }

    [Test]
    public void CreateIntConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intConst = context.IntConst("variableName");

        solver.Assert(intConst == 42);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intConst), Is.EqualTo(new BigInteger(42)));
        Assert.That(intConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase(42)]
    [TestCase(0)]
    [TestCase(-100)]
    public void ImplicitConversion_FromIntToIntExpr_Works(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        IntExpr implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(implicitExpr), Is.EqualTo(new BigInteger(value)));
    }

    [Test]
    public void CreateInt_FromLong_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intExpr = context.Int(9876543210L);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intExpr), Is.EqualTo(new BigInteger(9876543210L)));
    }

    [Test]
    public void CreateInt_FromBigInteger_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("123456789012345678901234567890");
        var intExpr = context.Int(bigValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(intExpr), Is.EqualTo(bigValue));
    }

    [Test]
    public void ImplicitConversion_FromLongToIntExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        IntExpr implicitExpr = 9876543210L;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(implicitExpr), Is.EqualTo(new BigInteger(9876543210L)));
    }

    [Test]
    public void ImplicitConversion_FromBigIntegerToIntExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("123456789012345678901234567890");
        IntExpr implicitExpr = bigValue;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(implicitExpr), Is.EqualTo(bigValue));
    }

    [Test]
    public void CreateMultipleIntConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var int1 = context.IntConst("var1");
        var int2 = context.IntConst("var2");
        var int3 = context.IntConst("var3");

        solver.Assert(int1 == 10);
        solver.Assert(int2 == 20);
        solver.Assert(int3 == 30);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(int1), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(int2), Is.EqualTo(new BigInteger(20)));
            Assert.That(model.GetIntValue(int3), Is.EqualTo(new BigInteger(30)));
        });
    }

    [Test]
    public void IntConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var int1 = context.IntConst("sameName");
        var int2 = context.IntConst("sameName");

        Assert.That(int1.Handle, Is.EqualTo(int2.Handle));
    }
}
