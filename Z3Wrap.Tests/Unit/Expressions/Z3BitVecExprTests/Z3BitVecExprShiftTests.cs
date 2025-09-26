using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BitVecTheory;
using Spaceorc.Z3Wrap.Values;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprShiftTests
{
    [TestCase(5, 1, 10, Description = "Left shift by 1 (101 << 1 = 1010)")]
    [TestCase(1, 3, 8, Description = "Left shift by 3")]
    [TestCase(255, 1, 254, Description = "Left shift with overflow")]
    [TestCase(0, 5, 0, Description = "Left shift zero")]
    public void LeftShift_AllVariations_ReturnsExpectedResult(
        int value,
        int shiftAmount,
        int expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(value));
        var shift = context.BitVec(new BitVec<Size8>(shiftAmount));
        var valueBigInt = new BigInteger(value);
        var shiftAmountBigInt = new BigInteger(shiftAmount);

        // Test all variations of left shift
        var resultOperatorBitVec = x << shift; // BitVec << BitVec (operator)
        var resultOperatorRightBigInt = x << shiftAmountBigInt; // BitVec << BigInteger (operator)
        var resultMethodBitVec = x.Shl(shift); // BitVec.Shl(BitVec) (method)
        var resultMethodBigInt = x.Shl(shiftAmountBigInt); // BitVec.Shl(BigInteger) (method)
        var resultContextBitVec = context.Shl(x, shift); // Context.Shl(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Shl(x, shiftAmountBigInt); // Context.Shl(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Shl(valueBigInt, shift); // Context.Shl(BigInteger, BitVec) (method)

        // Note: C# constraint CS0564 prevents BigInteger << Z3BitVecExpr operators
        // (first operand must be same type as containing type for shift operators)
        // So we test 7 variations instead of 8 for shift operations

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBitVec(resultOperatorBitVec),
                Is.EqualTo(expected),
                "BitVec << BitVec operator failed"
            );
            Assert.That(
                model.GetBitVec(resultOperatorRightBigInt),
                Is.EqualTo(expected),
                "BitVec << BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(resultMethodBitVec),
                Is.EqualTo(expected),
                "BitVec.Shl(BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Shl(BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Shl(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Shl(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Shl(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(10, 1, 5, Description = "Logical right shift by 1")]
    [TestCase(16, 2, 4, Description = "Right shift by 2")]
    [TestCase(255, 4, 15, Description = "Right shift all bits")]
    [TestCase(0, 3, 0, Description = "Right shift zero")]
    public void UnsignedRightShift_AllVariations_ReturnsExpectedResult(
        int value,
        int shiftAmount,
        int expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(value));
        var shift = context.BitVec(new BitVec<Size8>(shiftAmount));
        var valueBigInt = new BigInteger(value);
        var shiftAmountBigInt = new BigInteger(shiftAmount);

        // Test all variations of logical (unsigned) right shift
        var resultOperatorBitVec = x >> shift; // BitVec >> BitVec (operator)
        var resultOperatorRightBigInt = x >> shiftAmountBigInt; // BitVec >> BigInteger (operator)
        var resultMethodBitVec = x.Shr(shift, signed: false); // BitVec.Shr(BitVec, signed) (method)
        var resultMethodBigInt = x.Shr(shiftAmountBigInt, signed: false); // BitVec.Shr(BigInteger, signed) (method)
        var resultContextBitVec = context.Shr(x, shift, signed: false); // Context.Shr(BitVec, BitVec, signed: false) (method)
        var resultContextRightBigInt = context.Shr(x, shiftAmountBigInt, signed: false); // Context.Shr(BitVec, BigInteger, signed: false) (method)
        var resultContextLeftBigInt = context.Shr(valueBigInt, shift, signed: false); // Context.Shr(BigInteger, BitVec, signed: false) (method)

        // Note: BigInteger >> Z3BitVecExpr operator doesn't exist (makes no semantic sense)
        // so we only test 7 variations instead of 8 for shift operations

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBitVec(resultOperatorBitVec),
                Is.EqualTo(expected),
                "BitVec >> BitVec operator failed"
            );
            Assert.That(
                model.GetBitVec(resultOperatorRightBigInt),
                Is.EqualTo(expected),
                "BitVec >> BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(resultMethodBitVec),
                Is.EqualTo(expected),
                "BitVec.Shr(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Shr(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BitVec, signed: false) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BigInteger, signed: false) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BigInteger, BitVec, signed: false) method failed"
            );
        });
    }

    [TestCase(
        200,
        1,
        228,
        Description = "Signed right shift negative value by 1 (200 = -56 signed, -56>>1 = -28 = 228)"
    )]
    [TestCase(128, 2, 224, Description = "Signed right shift -128 by 2 (-128>>2 = -32 = 224)")]
    [TestCase(255, 3, 255, Description = "Signed right shift -1 by 3 (-1>>3 = -1 = 255)")]
    [TestCase(100, 1, 50, Description = "Signed right shift positive value by 1")]
    [TestCase(0, 3, 0, Description = "Signed right shift zero")]
    public void SignedRightShift_AllVariations_ReturnsExpectedResult(
        int value,
        int shiftAmount,
        int expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(value));
        var shift = context.BitVec(new BitVec<Size8>(shiftAmount));
        var valueBigInt = new BigInteger(value);
        var shiftAmountBigInt = new BigInteger(shiftAmount);

        // Test all variations of arithmetic (signed) right shift
        // Note: Operators >> always do logical shift, so we only test methods for signed shift
        var resultMethodBitVec = x.Shr(shift, signed: true); // BitVec.Shr(BitVec, signed) (method)
        var resultMethodBigInt = x.Shr(shiftAmountBigInt, signed: true); // BitVec.Shr(BigInteger, signed) (method)
        var resultContextBitVec = context.Shr(x, shift, signed: true); // Context.Shr(BitVec, BitVec, signed: true) (method)
        var resultContextRightBigInt = context.Shr(x, shiftAmountBigInt, signed: true); // Context.Shr(BitVec, BigInteger, signed: true) (method)
        var resultContextLeftBigInt = context.Shr(valueBigInt, shift, signed: true); // Context.Shr(BigInteger, BitVec, signed: true) (method)

        // For completeness, we also test the general Shr method with signed=true
        var resultContextShrBitVec = context.Shr(x, shift, signed: true); // Context.Shr(BitVec, BitVec, signed=true) (method)
        var resultContextShrRightBigInt = context.Shr(x, shiftAmountBigInt, signed: true); // Context.Shr(BitVec, BigInteger, signed=true) (method)
        var resultContextShrLeftBigInt = context.Shr(valueBigInt, shift, signed: true); // Context.Shr(BigInteger, BitVec, signed=true) (method)

        // Note: There are no BigInteger >> Z3BitVecExpr operators for signed shift (makes no semantic sense)
        // so we test 8 variations but only method calls, no operators with BigInteger on left side

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBitVec(resultMethodBitVec),
                Is.EqualTo(expected),
                "BitVec.Shr(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Shr(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BitVec, signed: true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BigInteger, signed: true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BigInteger, BitVec, signed: true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextShrBitVec),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextShrRightBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextShrLeftBigInt),
                Is.EqualTo(expected),
                "Context.Shr(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }
}
