using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.Numerics;

[TestFixture]
public class IntExprConversionTests
{
    [Test]
    public void ToReal_ConvertsIntegerToReal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intValue = context.Int(42);
        var realValue = intValue.ToReal();
        var realValueViaContext = context.ToReal(intValue);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(realValue).ToDecimal(), Is.EqualTo(42m));
            Assert.That(model.GetRealValue(realValueViaContext).ToDecimal(), Is.EqualTo(42m));
        });
    }

    [TestCase(TypeArgs = [typeof(Size8)])]
    [TestCase(TypeArgs = [typeof(Size16)])]
    [TestCase(TypeArgs = [typeof(Size32)])]
    [TestCase(TypeArgs = [typeof(Size64)])]
    public void ToBitVec_ConvertsIntegerToBitVector<TSize>()
        where TSize : ISize
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var intValue = context.Int(42);
        var bvValue = intValue.ToBv<TSize>();
        var bvValueViaContext = context.ToBv<TSize>(intValue);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBv(bvValue).Value, Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBv(bvValueViaContext).Value, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void ToReal_WithSymbolicVariable_PreservesConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == 10);

        var realX = x.ToReal();
        solver.Assert(realX > 5.0m);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetRealValue(realX).ToDecimal(), Is.EqualTo(10m));
        });
    }

    [Test]
    public void ToBitVec_WithSymbolicVariable_PreservesConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x >= 0);
        solver.Assert(x <= 255);

        var bvX = x.ToBv<Size8>();
        solver.Assert(bvX == 42);

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetBv(bvX).Value, Is.EqualTo(new BigInteger(42)));
        });
    }
}
