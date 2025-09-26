using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BitVecTheory;
using Spaceorc.Z3Wrap.Values;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprComparisonTests
{
    [TestCase(5, 10, true, Description = "5 < 10: true")]
    [TestCase(10, 5, false, Description = "10 < 5: false")]
    [TestCase(7, 7, false, Description = "7 < 7: false")]
    [TestCase(0, 255, true, Description = "0 < 255: true (unsigned)")]
    public void UnsignedLessThan_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned less than
        var resultOperatorBitVec = x < y; // BitVec < BitVec (operator)
        var resultOperatorRightBigInt = x < rightBigInt; // BitVec < BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt < y; // BigInteger < BitVec (operator)
        var resultMethodBitVec = x.Lt(y, signed: false); // BitVec.Lt(BitVec, signed) (method)
        var resultMethodBigInt = x.Lt(rightBigInt, signed: false); // BitVec.Lt(BigInteger, signed) (method)
        var resultContextBitVec = context.Lt(x, y, signed: false); // Context.Lt(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Lt(x, rightBigInt, signed: false); // Context.Lt(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Lt(leftBigInt, y, signed: false); // Context.Lt(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBitVec),
                Is.EqualTo(expectedResult),
                "BitVec < BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBigInt),
                Is.EqualTo(expectedResult),
                "BitVec < BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBigInt),
                Is.EqualTo(expectedResult),
                "BigInteger < BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.Lt(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.Lt(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.Lt(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.Lt(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.Lt(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(
        200,
        100,
        false,
        Description = "200 < 100: false (200 = -56 signed, -56 < 100 = true)"
    )]
    [TestCase(
        100,
        200,
        true,
        Description = "100 < 200: true (200 = -56 signed, 100 < -56 = false)"
    )]
    [TestCase(
        128,
        100,
        false,
        Description = "-128 < 100: true (signed), 128 < 100: false (unsigned)"
    )]
    [TestCase(
        127,
        255,
        true,
        Description = "127 < 255: true (127 < -1 = false signed, 127 < 255 = true unsigned)"
    )]
    public void SignedLessThan_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedUnsigned
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test signed less than (only methods support signed parameter)
        var resultMethodBitVec = x.Lt(y, signed: true); // BitVec.Lt(BitVec, signed) (method)
        var resultMethodBigInt = x.Lt(rightBigInt, signed: true); // BitVec.Lt(BigInteger, signed) (method)
        var resultContextBitVec = context.Lt(x, y, signed: true); // Context.Lt(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Lt(x, rightBigInt, signed: true); // Context.Lt(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Lt(leftBigInt, y, signed: true); // Context.Lt(BigInteger, BitVec, signed) (method)

        // Calculate expected signed result
        var leftSigned = left > 127 ? left - 256 : left; // Convert to signed
        var rightSigned = right > 127 ? right - 256 : right; // Convert to signed
        var expectedSigned = leftSigned < rightSigned;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedSigned),
                "BitVec.Lt(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedSigned),
                "BitVec.Lt(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedSigned),
                "Context.Lt(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Lt(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Lt(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(5, 10, true, Description = "5 <= 10: true")]
    [TestCase(10, 5, false, Description = "10 <= 5: false")]
    [TestCase(7, 7, true, Description = "7 <= 7: true")]
    [TestCase(0, 255, true, Description = "0 <= 255: true (unsigned)")]
    public void UnsignedLessThanOrEqual_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned less than or equal
        var resultOperatorBitVec = x <= y; // BitVec <= BitVec (operator)
        var resultOperatorRightBigInt = x <= rightBigInt; // BitVec <= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt <= y; // BigInteger <= BitVec (operator)
        var resultMethodBitVec = x.Le(y, signed: false); // BitVec.Le(BitVec, signed) (method)
        var resultMethodBigInt = x.Le(rightBigInt, signed: false); // BitVec.Le(BigInteger, signed) (method)
        var resultContextBitVec = context.Le(x, y, signed: false); // Context.Le(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Le(x, rightBigInt, signed: false); // Context.Le(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Le(leftBigInt, y, signed: false); // Context.Le(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBitVec),
                Is.EqualTo(expectedResult),
                "BitVec <= BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBigInt),
                Is.EqualTo(expectedResult),
                "BitVec <= BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBigInt),
                Is.EqualTo(expectedResult),
                "BigInteger <= BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.Le(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.Le(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.Le(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.Le(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.Le(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(
        200,
        100,
        false,
        Description = "200 <= 100: false unsigned (200 = -56 signed, -56 <= 100 = true)"
    )]
    [TestCase(
        100,
        200,
        true,
        Description = "100 <= 200: true unsigned (200 = -56 signed, 100 <= -56 = false)"
    )]
    [TestCase(128, 128, true, Description = "-128 <= -128: true")]
    public void SignedLessThanOrEqual_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedUnsigned
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test signed less than or equal (only methods support signed parameter)
        var resultMethodBitVec = x.Le(y, signed: true); // BitVec.Le(BitVec, signed) (method)
        var resultMethodBigInt = x.Le(rightBigInt, signed: true); // BitVec.Le(BigInteger, signed) (method)
        var resultContextBitVec = context.Le(x, y, signed: true); // Context.Le(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Le(x, rightBigInt, signed: true); // Context.Le(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Le(leftBigInt, y, signed: true); // Context.Le(BigInteger, BitVec, signed) (method)

        // Calculate expected signed result
        var leftSigned = left > 127 ? left - 256 : left; // Convert to signed
        var rightSigned = right > 127 ? right - 256 : right; // Convert to signed
        var expectedSigned = leftSigned <= rightSigned;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedSigned),
                "BitVec.Le(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedSigned),
                "BitVec.Le(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedSigned),
                "Context.Le(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Le(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Le(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(10, 5, true, Description = "10 > 5: true")]
    [TestCase(5, 10, false, Description = "5 > 10: false")]
    [TestCase(7, 7, false, Description = "7 > 7: false")]
    [TestCase(255, 0, true, Description = "255 > 0: true (unsigned)")]
    public void UnsignedGreaterThan_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned greater than
        var resultOperatorBitVec = x > y; // BitVec > BitVec (operator)
        var resultOperatorRightBigInt = x > rightBigInt; // BitVec > BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt > y; // BigInteger > BitVec (operator)
        var resultMethodBitVec = x.Gt(y, signed: false); // BitVec.Gt(BitVec, signed) (method)
        var resultMethodBigInt = x.Gt(rightBigInt, signed: false); // BitVec.Gt(BigInteger, signed) (method)
        var resultContextBitVec = context.Gt(x, y, signed: false); // Context.Gt(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Gt(x, rightBigInt, signed: false); // Context.Gt(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Gt(leftBigInt, y, signed: false); // Context.Gt(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBitVec),
                Is.EqualTo(expectedResult),
                "BitVec > BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBigInt),
                Is.EqualTo(expectedResult),
                "BitVec > BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBigInt),
                Is.EqualTo(expectedResult),
                "BigInteger > BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.Gt(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.Gt(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.Gt(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.Gt(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.Gt(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(
        100,
        200,
        false,
        Description = "100 > 200: false unsigned (200 = -56 signed, 100 > -56 = true)"
    )]
    [TestCase(
        200,
        100,
        true,
        Description = "200 > 100: true unsigned (200 = -56 signed, -56 > 100 = false)"
    )]
    [TestCase(
        255,
        128,
        false,
        Description = "-1 > -128: true (signed), 255 > 128: true (unsigned)"
    )]
    public void SignedGreaterThan_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedUnsigned
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test signed greater than (only methods support signed parameter)
        var resultMethodBitVec = x.Gt(y, signed: true); // BitVec.Gt(BitVec, signed) (method)
        var resultMethodBigInt = x.Gt(rightBigInt, signed: true); // BitVec.Gt(BigInteger, signed) (method)
        var resultContextBitVec = context.Gt(x, y, signed: true); // Context.Gt(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Gt(x, rightBigInt, signed: true); // Context.Gt(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Gt(leftBigInt, y, signed: true); // Context.Gt(BigInteger, BitVec, signed) (method)

        // Calculate expected signed result
        var leftSigned = left > 127 ? left - 256 : left; // Convert to signed
        var rightSigned = right > 127 ? right - 256 : right; // Convert to signed
        var expectedSigned = leftSigned > rightSigned;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedSigned),
                "BitVec.Gt(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedSigned),
                "BitVec.Gt(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedSigned),
                "Context.Gt(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Gt(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Gt(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(10, 5, true, Description = "10 >= 5: true")]
    [TestCase(5, 10, false, Description = "5 >= 10: false")]
    [TestCase(7, 7, true, Description = "7 >= 7: true")]
    [TestCase(255, 0, true, Description = "255 >= 0: true (unsigned)")]
    public void UnsignedGreaterThanOrEqual_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned greater than or equal
        var resultOperatorBitVec = x >= y; // BitVec >= BitVec (operator)
        var resultOperatorRightBigInt = x >= rightBigInt; // BitVec >= BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt >= y; // BigInteger >= BitVec (operator)
        var resultMethodBitVec = x.Ge(y, signed: false); // BitVec.Ge(BitVec, signed) (method)
        var resultMethodBigInt = x.Ge(rightBigInt, signed: false); // BitVec.Ge(BigInteger, signed) (method)
        var resultContextBitVec = context.Ge(x, y, signed: false); // Context.Ge(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Ge(x, rightBigInt, signed: false); // Context.Ge(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Ge(leftBigInt, y, signed: false); // Context.Ge(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultOperatorBitVec),
                Is.EqualTo(expectedResult),
                "BitVec >= BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorRightBigInt),
                Is.EqualTo(expectedResult),
                "BitVec >= BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultOperatorLeftBigInt),
                Is.EqualTo(expectedResult),
                "BigInteger >= BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.Ge(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.Ge(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.Ge(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.Ge(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.Ge(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(
        100,
        200,
        false,
        Description = "100 >= 200: false unsigned (200 = -56 signed, 100 >= -56 = true)"
    )]
    [TestCase(
        200,
        100,
        true,
        Description = "200 >= 100: true unsigned (200 = -56 signed, -56 >= 100 = false)"
    )]
    [TestCase(128, 128, true, Description = "-128 >= -128: true")]
    public void SignedGreaterThanOrEqual_AllVariations_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedUnsigned
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test signed greater than or equal (only methods support signed parameter)
        var resultMethodBitVec = x.Ge(y, signed: true); // BitVec.Ge(BitVec, signed) (method)
        var resultMethodBigInt = x.Ge(rightBigInt, signed: true); // BitVec.Ge(BigInteger, signed) (method)
        var resultContextBitVec = context.Ge(x, y, signed: true); // Context.Ge(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.Ge(x, rightBigInt, signed: true); // Context.Ge(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.Ge(leftBigInt, y, signed: true); // Context.Ge(BigInteger, BitVec, signed) (method)

        // Calculate expected signed result
        var leftSigned = left > 127 ? left - 256 : left; // Convert to signed
        var rightSigned = right > 127 ? right - 256 : right; // Convert to signed
        var expectedSigned = leftSigned >= rightSigned;

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedSigned),
                "BitVec.Ge(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedSigned),
                "BitVec.Ge(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedSigned),
                "Context.Ge(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Ge(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedSigned),
                "Context.Ge(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(42, 42, true, Description = "Equal values: 42 == 42")]
    [TestCase(42, 100, false, Description = "Different values: 42 == 100")]
    [TestCase(255, 255, true, Description = "Max values: 255 == 255")]
    [TestCase(0, 0, true, Description = "Zero values: 0 == 0")]
    public void Equality_BigIntegerOperators_ReturnsExpectedResult(
        int left,
        int right,
        bool expectedResult
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec<Size8>(left));
        var y = context.BitVec(new BitVec<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test BigInteger equality operators (these are the missing ones)
        var resultBitVecEqBigInt = x == rightBigInt; // BitVec == BigInteger (operator)
        var resultBigIntEqBitVec = leftBigInt == y; // BigInteger == BitVec (operator)

        // Test BigInteger inequality operators (these are also missing)
        var resultBitVecNeqBigInt = x != rightBigInt; // BitVec != BigInteger (operator)
        var resultBigIntNeqBitVec = leftBigInt != y; // BigInteger != BitVec (operator)

        // Also test regular BitVec equality for comparison
        var resultBitVecEqBitVec = x == y; // BitVec == BitVec (operator)
        var resultBitVecNeqBitVec = x != y; // BitVec != BitVec (operator)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultBitVecEqBigInt),
                Is.EqualTo(expectedResult),
                "BitVec == BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultBigIntEqBitVec),
                Is.EqualTo(expectedResult),
                "BigInteger == BitVec operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultBitVecNeqBigInt),
                Is.EqualTo(!expectedResult),
                "BitVec != BigInteger operator failed"
            );
            Assert.That(
                model.GetBoolValue(resultBigIntNeqBitVec),
                Is.EqualTo(!expectedResult),
                "BigInteger != BitVec operator failed"
            );

            // Verify consistency with BitVec == BitVec operators
            Assert.That(
                model.GetBoolValue(resultBitVecEqBitVec),
                Is.EqualTo(expectedResult),
                "BitVec == BitVec operator consistency check failed"
            );
            Assert.That(
                model.GetBoolValue(resultBitVecNeqBitVec),
                Is.EqualTo(!expectedResult),
                "BitVec != BitVec operator consistency check failed"
            );
        });
    }
}
