using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprShiftTests
{
    [TestCase(5, 1, 10, Description = "Left shift by 1 (101 << 1 = 1010)")]
    [TestCase(1, 3, 8, Description = "Left shift by 3")]
    [TestCase(255, 1, 254, Description = "Left shift with overflow")]
    [TestCase(0, 5, 0, Description = "Left shift zero")]
    public void LeftShift_AllVariations_ReturnsExpectedResult(int value, int shiftAmount, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);
        var shift = context.BitVec(shiftAmount, 8);

        // Test all variations of left shift
        var resultOperatorBitVec = x << shift;               // BitVec << BitVec (operator)
        var resultOperatorBigInt = x << shiftAmount;         // BitVec << BigInteger (operator)
        var resultMethodBitVec = x.Shl(shift);               // BitVec.Shl(BitVec) (method)
        var resultMethodBigInt = x.Shl(shiftAmount);         // BitVec.Shl(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec << BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorBigInt), Is.EqualTo(expected), "BitVec << BigInteger operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Shl(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Shl(BigInteger) method failed");
        });
    }

    [TestCase(10, 1, 5, Description = "Logical right shift by 1")]
    [TestCase(16, 2, 4, Description = "Right shift by 2")]
    [TestCase(255, 4, 15, Description = "Right shift all bits")]
    [TestCase(0, 3, 0, Description = "Right shift zero")]
    public void UnsignedRightShift_AllVariations_ReturnsExpectedResult(int value, int shiftAmount, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);
        var shift = context.BitVec(shiftAmount, 8);

        // Test all variations of logical (unsigned) right shift
        var resultOperatorBitVec = x >> shift;                          // BitVec >> BitVec (operator)
        var resultOperatorBigInt = x >> shiftAmount;                    // BitVec >> BigInteger (operator)
        var resultMethodBitVec = x.Shr(shift, signed: false);          // BitVec.Shr(BitVec, signed) (method)
        var resultMethodBigInt = x.Shr(shiftAmount, signed: false);    // BitVec.Shr(BigInteger, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec >> BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorBigInt), Is.EqualTo(expected), "BitVec >> BigInteger operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Shr(BitVec, signed=false) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Shr(BigInteger, signed=false) method failed");
        });
    }

    [TestCase(200, 1, 228, Description = "Signed right shift negative value by 1 (200 = -56 signed, -56>>1 = -28 = 228)")]
    [TestCase(128, 2, 224, Description = "Signed right shift -128 by 2 (-128>>2 = -32 = 224)")]
    [TestCase(255, 3, 255, Description = "Signed right shift -1 by 3 (-1>>3 = -1 = 255)")]
    [TestCase(100, 1, 50, Description = "Signed right shift positive value by 1")]
    [TestCase(0, 3, 0, Description = "Signed right shift zero")]
    public void SignedRightShift_AllVariations_ReturnsExpectedResult(int value, int shiftAmount, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);
        var shift = context.BitVec(shiftAmount, 8);

        // Test all variations of arithmetic (signed) right shift
        // Note: Operators >> always do logical shift, so we test methods for signed
        var resultMethodBitVec = x.Shr(shift, signed: true);           // BitVec.Shr(BitVec, signed) (method)
        var resultMethodBigInt = x.Shr(shiftAmount, signed: true);     // BitVec.Shr(BigInteger, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Shr(BitVec, signed=true) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Shr(BigInteger, signed=true) method failed");
        });
    }
}