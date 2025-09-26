using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BitVectors;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprCreationTests
{
    [Test]
    public void BitVecConst_Size32_CreatesVariableWithCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst<Size32>("x");

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bv.Context, Is.EqualTo(context));
        Assert.That(Z3BitVec<Size32>.Size, Is.EqualTo(32u));
    }

    [Test]
    public void BitVecConst_Size8_CreatesCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst<Size8>("x");

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void BitVecConst_Size16_CreatesCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst<Size16>("x");

        Assert.That(Z3BitVec<Size16>.Size, Is.EqualTo(16u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void BitVecConst_Size32_CreatesCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst<Size32>("x");

        Assert.That(Z3BitVec<Size32>.Size, Is.EqualTo(32u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void BitVecConst_Size64_CreatesCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst<Size64>("x");

        Assert.That(Z3BitVec<Size64>.Size, Is.EqualTo(64u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void BitVec_SmallPositiveValue_Size8_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec<Size8>(42));

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec<Size8>(42)));
    }

    [Test]
    public void BitVec_Zero_Size8_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec<Size8>(0));

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec<Size8>(0)));
    }

    [Test]
    public void BitVec_NegativeValue_Size8_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec<Size8>(-1));

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec<Size8>(-1)));
    }

    [Test]
    public void BitVec_Max8BitUnsigned_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec<Size8>(255));

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec<Size8>(255)));
    }

    [Test]
    public void BitVec_Min8BitSigned_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec<Size8>(-128));

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec<Size8>(-128)));
    }

    [Test]
    public void BitVecExpr_ImplicitConversionFromBitVec16_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bitVec = new BitVec<Size16>(100);
        Z3BitVec<Size16> bvExpr = bitVec; // Implicit conversion

        Assert.That(Z3BitVec<Size16>.Size, Is.EqualTo(16u));
        Assert.That(bvExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bvExpr.Context, Is.EqualTo(context));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bvExpr);
        Assert.That(actualValue, Is.EqualTo(bitVec));
    }

    [Test]
    public void BitVecExpr_ImplicitConversionFromBitVec8_Zero_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bitVec = new BitVec<Size8>(0);
        Z3BitVec<Size8> bvExpr = bitVec; // Implicit conversion

        Assert.That(Z3BitVec<Size8>.Size, Is.EqualTo(8u));
        Assert.That(bvExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bvExpr.Context, Is.EqualTo(context));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bvExpr);
        Assert.That(actualValue, Is.EqualTo(bitVec));
    }

    [Test]
    public void BitVecExpr_ImplicitConversionFromBitVec32_Negative_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bitVec = new BitVec<Size32>(-50);
        Z3BitVec<Size32> bvExpr = bitVec; // Implicit conversion

        Assert.That(Z3BitVec<Size32>.Size, Is.EqualTo(32u));
        Assert.That(bvExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bvExpr.Context, Is.EqualTo(context));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bvExpr);
        Assert.That(actualValue, Is.EqualTo(bitVec));
    }

    [Test]
    public void BitVecExpr_ImplicitConversionFromBitVec16_Max_CreatesCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bitVec = new BitVec<Size16>(65535);
        Z3BitVec<Size16> bvExpr = bitVec; // Implicit conversion

        Assert.That(Z3BitVec<Size16>.Size, Is.EqualTo(16u));
        Assert.That(bvExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bvExpr.Context, Is.EqualTo(context));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bvExpr);
        Assert.That(actualValue, Is.EqualTo(bitVec));
    }
}
