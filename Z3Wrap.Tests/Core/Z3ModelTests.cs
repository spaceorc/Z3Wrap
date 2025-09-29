using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3ModelTests
{
    [Test]
    public void Handle_ValidModel_ReturnsNonZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.Assert(context.True());
        solver.Check();

        var model = solver.GetModel();

        Assert.That(model.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void GetIntValue_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 42);
        solver.Check();

        var model = solver.GetModel();
        var value = model.GetIntValue(x);

        Assert.That(value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void GetRealValue_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.RealConst("x");
        solver.Assert(x == 3.14m);
        solver.Check();

        var model = solver.GetModel();
        var value = model.GetRealValue(x);

        Assert.That(value.ToDecimal(), Is.EqualTo(3.14m));
    }

    [Test]
    public void GetBoolValue_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var p = context.BoolConst("p");
        solver.Assert(p);
        solver.Check();

        var model = solver.GetModel();
        var value = model.GetBoolValue(p);

        Assert.That(value, Is.True);
    }

    [Test]
    public void ToString_ReturnsNonNull()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x == 42);
        solver.Check();

        var model = solver.GetModel();
        var str = model.ToString();

        Assert.That(str, Is.Not.Null);
    }
}
