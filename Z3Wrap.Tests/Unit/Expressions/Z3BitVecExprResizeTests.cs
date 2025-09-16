using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3BitVecExprResizeTests
{
    [TestCase(255, 8u, 16u, 255, Description = "Zero-extend 8-bit to 16-bit")]
    [TestCase(255, 8u, 4u, 15, Description = "Truncate 8-bit to 4-bit")]
    [TestCase(127, 8u, 16u, 127, Description = "Zero-extend positive value")]
    public void UnsignedResize_AllVariations_ReturnsExpectedResult(int value, uint fromSize, uint toSize, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, fromSize);
        var result = x.Resize(toSize, signed: false);

        Assert.That(result.Size, Is.EqualTo(toSize));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, toSize)));
    }

    [TestCase(200, 8u, 16u, 65480, Description = "Sign-extend negative value (200 = -56 signed, extends to -56 = 65480 in 16-bit)")]
    [TestCase(128, 8u, 16u, 65408, Description = "Sign-extend -128 to 16-bit (-128 = 65408 in 16-bit)")]
    [TestCase(255, 8u, 4u, 15, Description = "Truncate -1 to 4-bit (-1 truncated = 15)")]
    [TestCase(100, 8u, 16u, 100, Description = "Sign-extend positive value (same as zero-extend)")]
    [TestCase(127, 8u, 4u, 15, Description = "Truncate positive value")]
    public void SignedResize_AllVariations_ReturnsExpectedResult(int value, uint fromSize, uint toSize, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, fromSize);
        var result = x.Resize(toSize, signed: true);

        Assert.That(result.Size, Is.EqualTo(toSize));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, toSize)));
    }

    [TestCase(255, 7u, 0u, 255, 8u, Description = "Extract all 8 bits")]
    [TestCase(255, 3u, 0u, 15, 4u, Description = "Extract lower 4 bits")]
    [TestCase(255, 7u, 4u, 15, 4u, Description = "Extract upper 4 bits")]
    [TestCase(170, 3u, 0u, 10, 4u, Description = "Extract lower bits from alternating pattern")]
    public void Extract_AllVariations_ReturnsExpectedResult(int value, uint high, uint low, int expectedResult, uint expectedToSize)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);
        var result = x.Extract(high, low);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBitVec(result), Is.EqualTo(new BitVec(expectedResult, expectedToSize)));
    }
}