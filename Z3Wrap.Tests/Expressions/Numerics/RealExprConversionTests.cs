using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

[TestFixture]
public class RealExprConversionTests
{
    [TestCase(42.7, 42)]
    [TestCase(-42.7, -43)]
    [TestCase(42.0, 42)]
    public void ToInt_ConvertsRealToInteger(double realVal, int expectedInt)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realValue = context.Real((decimal)realVal);
        var intValue = realValue.ToInt();
        var intValueViaContext = context.ToInt(realValue);

        solver.Check();
        var model = solver.GetModel();

        // Z3's real2int floors the value (rounds towards negative infinity)
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(expectedInt)));
            Assert.That(model.GetIntValue(intValueViaContext), Is.EqualTo(new BigInteger(expectedInt)));
        });
    }

    [Test]
    public void ToInt_WithSymbolicVariable_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        solver.Assert(x == 42.5m);

        var intX = x.ToInt();
        solver.Assert(intX == 42);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(x).ToDecimal(), Is.EqualTo(42.5m));
        Assert.That(model.GetIntValue(intX), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void ToInt_WithRationalValue_TruncatesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realValue = context.Real(new Real(10, 3)); // 10/3 = 3.333...
        var intValue = realValue.ToInt();
        var intValueViaContext = context.ToInt(realValue);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(intValue), Is.EqualTo(new BigInteger(3)));
            Assert.That(model.GetIntValue(intValueViaContext), Is.EqualTo(new BigInteger(3)));
        });
    }

    [Test]
    public void ToInt_InConstraint_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        solver.Assert(x > 5.0m);
        solver.Assert(x < 6.0m);
        solver.Assert(x.ToInt() == 5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetRealValue(x);
        Assert.That(value.ToDecimal(), Is.GreaterThan(5.0m));
        Assert.That(value.ToDecimal(), Is.LessThan(6.0m));
    }

    [Test]
    public void IsInt_WithIntegerValue_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realValue = context.Real(42);

        var resultViaContext = context.IsInt(realValue);
        var resultViaFunc = realValue.IsInt();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.True);
        });
    }

    [Test]
    public void IsInt_WithNonIntegerValue_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var realValue = context.Real(new Real(42, 10)); // 42/10 = 4.2

        var resultViaContext = context.IsInt(realValue);
        var resultViaFunc = realValue.IsInt();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaFunc), Is.False);
        });
    }

    [Test]
    public void IsInt_InConstraint_CanSolveForIntegerRealValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        // x must be a real number that is also an integer, between 5 and 7
        solver.Assert(x.IsInt());
        solver.Assert(x > context.Real(5));
        solver.Assert(x < context.Real(7));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetRealValue(x);
        // Should be 6 (the only integer between 5 and 7)
        Assert.That(value.Numerator, Is.EqualTo(new BigInteger(6)));
        Assert.That(value.Denominator, Is.EqualTo(new BigInteger(1)));
    }
}
