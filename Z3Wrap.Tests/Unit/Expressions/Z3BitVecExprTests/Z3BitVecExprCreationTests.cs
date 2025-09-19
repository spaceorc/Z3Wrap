using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprCreationTests
{
    [Test]
    public void BitVecConst_CreatesVariableWithCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst("x", 32);

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bv.Context, Is.EqualTo(context));
        Assert.That(bv.Size, Is.EqualTo(32));
    }

    [TestCase(8u, Description = "8-bit")]
    [TestCase(16u, Description = "16-bit")]
    [TestCase(32u, Description = "32-bit")]
    [TestCase(64u, Description = "64-bit")]
    public void BitVecConst_DifferentSizes_CreatesCorrectSize(uint size)
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst("x", size);

        Assert.That(bv.Size, Is.EqualTo(size));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [TestCase(42, 8u, Description = "Small positive value")]
    [TestCase(0, 8u, Description = "Zero value")]
    [TestCase(-1, 8u, Description = "Negative value")]
    [TestCase(255, 8u, Description = "Max 8-bit unsigned")]
    [TestCase(-128, 8u, Description = "Min 8-bit signed")]
    public void BitVec_FromInt_CreatesCorrectExpression(int value, uint size)
    {
        using var context = new Z3Context();
        var bv = context.BitVec(value, size);

        Assert.That(bv.Size, Is.EqualTo(size));
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify the actual value via model evaluation
        using var solver = context.CreateSolver();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var actualValue = model.GetBitVec(bv);
        Assert.That(actualValue, Is.EqualTo(new BitVec(value, size)));
    }

    [TestCase(100, 16u, Description = "Positive value")]
    [TestCase(0, 8u, Description = "Zero value")]
    [TestCase(-50, 32u, Description = "Negative value")]
    [TestCase(65535, 16u, Description = "Max 16-bit unsigned")]
    public void BitVecExpr_ImplicitConversionFromBitVec_CreatesCorrectExpression(int value, uint size)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bitVec = new BitVec(value, size);
        Z3BitVecExpr bvExpr = bitVec; // Implicit conversion

        Assert.That(bvExpr.Size, Is.EqualTo(size));
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