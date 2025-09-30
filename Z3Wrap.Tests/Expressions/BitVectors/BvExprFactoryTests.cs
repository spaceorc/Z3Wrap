using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprFactoryTests
{
    [TestCase(42u)]
    [TestCase(0u)]
    [TestCase(255u)]
    public void CreateBitVec_FromUint_ReturnsCorrectExpression(uint value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVec<Size32>(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(new BigInteger(value)));
    }

    [TestCase(42)]
    [TestCase(0)]
    [TestCase(-1)]
    public void CreateBitVec_FromInt_ReturnsCorrectExpression(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVec<Size32>(value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(new BigInteger(unchecked((uint)value))));
    }

    [Test]
    public void CreateBitVec_FromUlong_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVec<Size64>(9876543210UL);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(new BigInteger(9876543210UL)));
    }

    [Test]
    public void CreateBitVec_FromLong_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVec<Size64>(-42L);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(new BigInteger(unchecked((ulong)-42L))));
    }

    [Test]
    public void CreateBitVec_FromBigInteger_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("12345678901234567890");
        var bvExpr = context.BitVec<Size64>(bigValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        // BigInteger will be masked to 64 bits
        var expected = bigValue & ((BigInteger.One << 64) - 1);
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(expected));
    }

    [Test]
    public void CreateBitVec_FromBvValue_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = new Bv<Size32>(42u);
        var bvExpr = context.BitVec(bvValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvExpr).Value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void CreateBitVecConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvConst = context.BitVecConst<Size32>("variableName");

        solver.Assert(bvConst == 42u);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(bvConst).Value, Is.EqualTo(new BigInteger(42)));
        Assert.That(bvConst.ToString(), Does.Contain("variableName"));
    }

    [TestCase(42u)]
    [TestCase(0u)]
    [TestCase(255u)]
    public void ImplicitConversion_FromUintToBvExpr_Works(uint value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        BvExpr<Size32> implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(new BigInteger(value)));
    }

    [TestCase(42)]
    [TestCase(0)]
    [TestCase(-1)]
    public void ImplicitConversion_FromIntToBvExpr_Works(int value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        BvExpr<Size32> implicitExpr = value;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(new BigInteger(unchecked((uint)value))));
    }

    [Test]
    public void ImplicitConversion_FromUlongToBvExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        BvExpr<Size64> implicitExpr = 9876543210UL;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(new BigInteger(9876543210UL)));
    }

    [Test]
    public void ImplicitConversion_FromLongToBvExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        BvExpr<Size64> implicitExpr = -42L;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(new BigInteger(unchecked((ulong)-42L))));
    }

    [Test]
    public void ImplicitConversion_FromBigIntegerToBvExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bigValue = BigInteger.Parse("12345678901234567890");
        BvExpr<Size64> implicitExpr = bigValue;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = bigValue & ((BigInteger.One << 64) - 1);
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(expected));
    }

    [Test]
    public void ImplicitConversion_FromBvValueToBvExpr_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bvValue = new Bv<Size32>(42u);
        BvExpr<Size32> implicitExpr = bvValue;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(implicitExpr).Value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void CreateMultipleBitVecConstants_HaveIndependentValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv1 = context.BitVecConst<Size32>("var1");
        var bv2 = context.BitVecConst<Size32>("var2");
        var bv3 = context.BitVecConst<Size32>("var3");

        solver.Assert(bv1 == 10u);
        solver.Assert(bv2 == 20u);
        solver.Assert(bv3 == 30u);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(bv1).Value, Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetBitVec(bv2).Value, Is.EqualTo(new BigInteger(20)));
            Assert.That(model.GetBitVec(bv3).Value, Is.EqualTo(new BigInteger(30)));
        });
    }

    [Test]
    public void BitVecConstWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv1 = context.BitVecConst<Size32>("sameName");
        var bv2 = context.BitVecConst<Size32>("sameName");

        Assert.That(bv1.Handle, Is.EqualTo(bv2.Handle));
    }

    [TestCase(TypeArgs = [typeof(Size8)])]
    [TestCase(TypeArgs = [typeof(Size16)])]
    [TestCase(TypeArgs = [typeof(Size32)])]
    [TestCase(TypeArgs = [typeof(Size64)])]
    public void BvExpr_Sort_ReturnsBvSortKind<TSize>()
        where TSize : ISize
    {
        using var context = new Z3Context();

        var sortHandle = context.GetSortForType<BvExpr<TSize>>();
        var sortKind = context.Library.Z3GetSortKind(context.Handle, sortHandle);

        Assert.That(sortKind, Is.EqualTo(Z3SortKind.Bv));
    }

    [TestCase(TypeArgs = [typeof(Size8)], ExpectedResult = 8u)]
    [TestCase(TypeArgs = [typeof(Size16)], ExpectedResult = 16u)]
    [TestCase(TypeArgs = [typeof(Size32)], ExpectedResult = 32u)]
    [TestCase(TypeArgs = [typeof(Size64)], ExpectedResult = 64u)]
    public uint BvExpr_Size_ReturnsCorrectBitWidth<TSize>()
        where TSize : ISize
    {
        return BvExpr<TSize>.Size;
    }
}
