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
}
