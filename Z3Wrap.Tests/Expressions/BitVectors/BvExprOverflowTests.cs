using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprOverflowTests
{
    [TestCase(0, 0, false, Description = "0 + 0 = 0 < 256 → no overflow")]
    [TestCase(255, 0, false, Description = "255 + 0 = 255 < 256 → no overflow")]
    [TestCase(0, 255, false, Description = "0 + 255 = 255 < 256 → no overflow")]
    [TestCase(128, 127, false, Description = "128 + 127 = 255 < 256 → no overflow")]
    [TestCase(128, 128, true, Description = "128 + 128 = 256 ≥ 256 → overflow (boundary)")]
    [TestCase(255, 1, true, Description = "255 + 1 = 256 ≥ 256 → overflow")]
    [TestCase(200, 100, true, Description = "200 + 100 = 300 ≥ 256 → overflow")]
    [TestCase(255, 255, true, Description = "255 + 255 = 510 ≥ 256 → overflow")]
    [TestCase(100, 50, false, Description = "100 + 50 = 150 < 256 → no overflow")]
    [TestCase(127, 127, false, Description = "127 + 127 = 254 < 256 → no overflow")]
    [TestCase(129, 126, false, Description = "129 + 126 = 255 < 256 → no overflow")]
    [TestCase(129, 127, true, Description = "129 + 127 = 256 ≥ 256 → overflow")]
    public void UnsignedAddNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned add overflow detection
        var resultMethodBitVec = x.AddNoOverflow(y, false); // BitVec.AddNoOverflow(BitVec, signed) (method)
        var resultMethodBigInt = x.AddNoOverflow(rightBigInt, false); // BitVec.AddNoOverflow(BigInteger, signed) (method)
        var resultContextBitVec = context.AddNoOverflow(x, y, false); // Context.AddNoOverflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.AddNoOverflow(x, rightBigInt, false); // Context.AddNoOverflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.AddNoOverflow(leftBigInt, y, false); // Context.AddNoOverflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.AddNoOverflow(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.AddNoOverflow(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(127, 1, true, Description = "127 + 1 = 128 > 127 → positive overflow")]
    [TestCase(127, 0, false, Description = "127 + 0 = 127 → ok")]
    [TestCase(100, 50, true, Description = "100 + 50 = 150 > 127 → positive overflow")]
    [TestCase(127, 127, true, Description = "127 + 127 = 254 > 127 → positive overflow")]
    [TestCase(1, 127, true, Description = "1 + 127 = 128 > 127 → positive overflow")]
    [TestCase(2, 127, true, Description = "2 + 127 = 129 > 127 → positive overflow")]
    [TestCase(255, 1, false, Description = "-1 + 1 = 0 → ok")]
    [TestCase(255, 255, false, Description = "-1 + -1 = -2 → no positive overflow")]
    [TestCase(200, 200, false, Description = "-56 + -56 = -112 → no positive overflow")]
    [TestCase(128, 1, false, Description = "-128 + 1 = -127 → ok")]
    [TestCase(128, 128, false, Description = "-128 + -128 = -256 (wraps) → negative underflow (not flagged here)")]
    [TestCase(0, 128, false, Description = "0 + -128 = -128 → ok")]
    [TestCase(50, 200, false, Description = "50 + -56 = -6 → ok")]
    [TestCase(200, 100, false, Description = "-56 + 100 = 44 → ok")]
    public void SignedAddNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed add overflow detection
        var resultMethodBitVec = x.AddNoOverflow(y, true); // BitVec.AddNoOverflow(BitVec, signed) (method)
        var resultMethodBigInt = x.AddNoOverflow(rightBigInt, true); // BitVec.AddNoOverflow(BigInteger, signed) (method)
        var resultContextBitVec = context.AddNoOverflow(x, y, true); // Context.AddNoOverflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.AddNoOverflow(x, rightBigInt, true); // Context.AddNoOverflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.AddNoOverflow(leftBigInt, y, true); // Context.AddNoOverflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.AddNoOverflow(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.AddNoOverflow(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.AddNoOverflow(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(0, 0, false, Description = "0 * 0 = 0  < 256 → no overflow")]
    [TestCase(0, 255, false, Description = "0 * 255 = 0  < 256 → no overflow")]
    [TestCase(1, 255, false, Description = "1 * 255 = 255 < 256 → no overflow")]
    [TestCase(255, 1, false, Description = "255 * 1 = 255 < 256 → no overflow")]
    [TestCase(16, 16, true, Description = "16 * 16 = 256 ≥ 256 → overflow (boundary)")]
    [TestCase(32, 8, true, Description = "32 * 8 = 256  ≥ 256 → overflow")]
    [TestCase(64, 4, true, Description = "64 * 4 = 256  ≥ 256 → overflow")]
    [TestCase(128, 2, true, Description = "128 * 2 = 256 ≥ 256 → overflow")]
    [TestCase(15, 17, false, Description = "15 * 17 = 255 < 256 → no overflow")]
    [TestCase(3, 85, false, Description = "3 * 85 = 255  < 256 → no overflow")]
    [TestCase(5, 51, false, Description = "5 * 51 = 255  < 256 → no overflow")]
    [TestCase(5, 52, true, Description = "5 * 52 = 260  ≥ 256 → overflow")]
    [TestCase(3, 86, true, Description = "3 * 86 = 258  ≥ 256 → overflow")]
    [TestCase(200, 50, true, Description = "200 * 50 = 10000 ≥ 256 → overflow")]
    [TestCase(255, 2, true, Description = "255 * 2 = 510   ≥ 256 → overflow")]
    [TestCase(255, 255, true, Description = "255 * 255 ≫ 256 → overflow")]
    [TestCase(64, 3, false, Description = "64 * 3 = 192  < 256 → no overflow")]
    [TestCase(85, 3, false, Description = "85 * 3 = 255  < 256 → no overflow")]
    [TestCase(85, 4, true, Description = "85 * 4 = 340  ≥ 256 → overflow")]
    public void UnsignedMulNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned multiply overflow detection
        var resultMethodBitVec = x.MulNoOverflow(y, false); // BitVec.MulNoOverflow(BitVec, signed) (method)
        var resultMethodBigInt = x.MulNoOverflow(rightBigInt, false); // BitVec.MulNoOverflow(BigInteger, signed) (method)
        var resultContextBitVec = context.MulNoOverflow(x, y, false); // Context.MulNoOverflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.MulNoOverflow(x, rightBigInt, false); // Context.MulNoOverflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.MulNoOverflow(leftBigInt, y, false); // Context.MulNoOverflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.MulNoOverflow(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.MulNoOverflow(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(10, 12, false, Description = "Signed multiplication no overflow: 10 * 12 = 120 <= 127")]
    [TestCase(11, 12, true, Description = "Signed multiplication overflow: 11 * 12 = 132 > 127")]
    // [TestCase(200, 2, false, Description = "Signed no overflow: -56 * 2 = -112 >= -128")] // Platform-specific behavior between Z3 versions - disabled for CI compatibility
    [TestCase(1, 127, false, Description = "No overflow: 1 * 127")]
    [TestCase(0, 100, false, Description = "No overflow: 0 * 100")]
    public void SignedMulNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed multiply overflow detection
        var resultMethodBitVec = x.MulNoOverflow(y, true); // BitVec.MulNoOverflow(BitVec, signed) (method)
        var resultMethodBigInt = x.MulNoOverflow(rightBigInt, true); // BitVec.MulNoOverflow(BigInteger, signed) (method)
        var resultContextBitVec = context.MulNoOverflow(x, y, true); // Context.MulNoOverflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.MulNoOverflow(x, rightBigInt, true); // Context.MulNoOverflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.MulNoOverflow(leftBigInt, y, true); // Context.MulNoOverflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.MulNoOverflow(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.MulNoOverflow(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.MulNoOverflow(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(127, 200, true, Description = "127 - (-56) = 183 > 127 → overflow")]
    [TestCase(100, 50, false, Description = "100 - 50 = 50 → ok")]
    [TestCase(0, 128, true, Description = "0 - (-128) = 128 > 127 → overflow")]
    [TestCase(200, 100, false, Description = "-56 - 100 = -156 (too negative, but not upper-overflow)")]
    [TestCase(255, 255, false, Description = "-1 - (-1) = 0 → ok")]
    [TestCase(10, 255, false, Description = "10 - (-1) = 11 → ok")]
    [TestCase(127, 1, false, Description = "127 - 1 = 126 → ok")]
    [TestCase(127, 255, true, Description = "127 - (-1) = 128 > 127 → overflow")]
    public void SignedSubNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed subtract overflow detection
        var resultMethodBitVec = x.SignedSubNoOverflow(y); // BitVec.SignedSubNoOverflow(BitVec) (method)
        var resultMethodBigInt = x.SignedSubNoOverflow(rightBigInt); // BitVec.SignedSubNoOverflow(BigInteger) (method)
        var resultContextBitVec = context.SignedSubNoOverflow(x, y); // Context.SignedSubNoOverflow(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.SignedSubNoOverflow(x, rightBigInt); // Context.SignedSubNoOverflow(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.SignedSubNoOverflow(leftBigInt, y); // Context.SignedSubNoOverflow(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SignedSubNoOverflow(BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SignedSubNoOverflow(BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SignedSubNoOverflow(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedSubNoOverflow(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedSubNoOverflow(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(0, 0, false, Description = "0 - 0: x >= y → no underflow")]
    [TestCase(0, 1, true, Description = "0 - 1: x < y  → underflow (wraps to 255)")]
    [TestCase(1, 0, false, Description = "1 - 0: x >= y → no underflow")]
    [TestCase(255, 1, false, Description = "255 - 1: x >= y → no underflow")]
    [TestCase(0, 255, true, Description = "0 - 255: x < y → underflow (wraps to 1)")]
    [TestCase(128, 128, false, Description = "128 - 128: equal → no underflow")]
    [TestCase(127, 128, true, Description = "127 - 128: x < y → underflow")]
    [TestCase(128, 127, false, Description = "128 - 127: x >= y → no underflow")]
    [TestCase(200, 50, false, Description = "200 - 50: x >= y → no underflow")]
    [TestCase(50, 200, true, Description = "50 - 200: x < y → underflow")]
    [TestCase(255, 255, false, Description = "255 - 255: equal → no underflow")]
    [TestCase(1, 255, true, Description = "1 - 255: x < y → underflow")]
    public void UnsignedSubNoUnderflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedUnderflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of unsigned subtract underflow detection
        var resultMethodBitVec = x.SubNoUnderflow(y, false); // BitVec.SubNoUnderflow(BitVec, signed) (method)
        var resultMethodBigInt = x.SubNoUnderflow(rightBigInt, false); // BitVec.SubNoUnderflow(BigInteger, signed) (method)
        var resultContextBitVec = context.SubNoUnderflow(x, y, false); // Context.SubNoUnderflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.SubNoUnderflow(x, rightBigInt, false); // Context.SubNoUnderflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.SubNoUnderflow(leftBigInt, y, false); // Context.SubNoUnderflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedUnderflow; // Method returns true when NO underflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SubNoUnderflow(BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SubNoUnderflow(BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BitVec, BitVec, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BitVec, BigInteger, signed=false) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BigInteger, BitVec, signed=false) method failed"
            );
        });
    }

    [TestCase(128, 1, true, Description = "-128 - 1 = -129 < -128 → underflow")]
    [TestCase(200, 100, true, Description = "-56 - 100 = -156 < -128 → underflow")]
    [TestCase(128, 0, false, Description = "-128 - 0 = -128 (boundary) → ok")]
    [TestCase(0, 200, false, Description = "0 - (-56) = 56 → ok")]
    [TestCase(50, 200, false, Description = "50 - (-56) = 106 → ok")]
    [TestCase(0, 1, false, Description = "0 - 1 = -1 → ok")]
    [TestCase(129, 2, true, Description = "-127 - 2 = -129 < -128 → underflow")]
    [TestCase(255, 127, false, Description = "-1 - 127 = -128 (boundary) → ok")]
    public void SignedSubNoUnderflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedUnderflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed subtract underflow detection
        var resultMethodBitVec = x.SubNoUnderflow(y, true); // BitVec.SubNoUnderflow(BitVec, signed) (method)
        var resultMethodBigInt = x.SubNoUnderflow(rightBigInt, true); // BitVec.SubNoUnderflow(BigInteger, signed) (method)
        var resultContextBitVec = context.SubNoUnderflow(x, y, true); // Context.SubNoUnderflow(BitVec, BitVec, signed) (method)
        var resultContextRightBigInt = context.SubNoUnderflow(x, rightBigInt, true); // Context.SubNoUnderflow(BitVec, BigInteger, signed) (method)
        var resultContextLeftBigInt = context.SubNoUnderflow(leftBigInt, y, true); // Context.SubNoUnderflow(BigInteger, BitVec, signed) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedUnderflow; // Method returns true when NO underflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SubNoUnderflow(BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SubNoUnderflow(BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BitVec, BitVec, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BitVec, BigInteger, signed=true) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SubNoUnderflow(BigInteger, BitVec, signed=true) method failed"
            );
        });
    }

    [TestCase(200, 200, false, Description = "-56 * -56 = +3136 → positive (no underflow)")]
    [TestCase(128, 2, true, Description = "-128 * 2 = -256 < -128 → underflow")]
    [TestCase(100, 200, true, Description = "100 * -56 = -5600 < -128 → underflow")]
    [TestCase(1, 128, false, Description = "1 * -128 = -128 (boundary) → ok")]
    [TestCase(255, 127, false, Description = "-1 * 127 = -127 → ok")]
    [TestCase(255, 128, false, Description = "-1 * -128 = +128 (positive) → no underflow")]
    [TestCase(129, 2, true, Description = "-127 * 2 = -254 < -128 → underflow")]
    [TestCase(64, 4, false, Description = "64 * 4 = +256 (positive overflow, but no underflow)")]
    public void SignedMulNoUnderflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedUnderflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed multiply underflow detection
        var resultMethodBitVec = x.SignedMulNoUnderflow(y); // BitVec.SignedMulNoUnderflow(BitVec) (method)
        var resultMethodBigInt = x.SignedMulNoUnderflow(rightBigInt); // BitVec.SignedMulNoUnderflow(BigInteger) (method)
        var resultContextBitVec = context.SignedMulNoUnderflow(x, y); // Context.SignedMulNoUnderflow(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.SignedMulNoUnderflow(x, rightBigInt); // Context.SignedMulNoUnderflow(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.SignedMulNoUnderflow(leftBigInt, y); // Context.SignedMulNoUnderflow(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedUnderflow; // Method returns true when NO underflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SignedMulNoUnderflow(BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SignedMulNoUnderflow(BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SignedMulNoUnderflow(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedMulNoUnderflow(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedMulNoUnderflow(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(128, 255, true, Description = "−128 + (−1) = −129 < −128 → underflow")]
    [TestCase(128, 0, false, Description = "−128 + 0 = −128 (boundary) → ok")]
    [TestCase(255, 255, false, Description = "−1 + (−1) = −2 → ok")]
    [TestCase(156, 216, true, Description = "−100 + (−40) = −140 < −128 → underflow")]
    [TestCase(200, 184, false, Description = "−56 + (−72) = −128 (boundary) → ok")]
    [TestCase(0, 200, false, Description = "0 + (−56) = −56 → ok")]
    [TestCase(50, 200, false, Description = "50 + (−56) = −6 → ok")]
    [TestCase(129, 1, false, Description = "−127 + 1 = −126 → ok")]
    public void SignedAddNoUnderflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedUnderflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed add underflow detection
        var resultMethodBitVec = x.SignedAddNoUnderflow(y); // BitVec.SignedAddNoUnderflow(BitVec) (method)
        var resultMethodBigInt = x.SignedAddNoUnderflow(rightBigInt); // BitVec.SignedAddNoUnderflow(BigInteger) (method)
        var resultContextBitVec = context.SignedAddNoUnderflow(x, y); // Context.SignedAddNoUnderflow(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.SignedAddNoUnderflow(x, rightBigInt); // Context.SignedAddNoUnderflow(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.SignedAddNoUnderflow(leftBigInt, y); // Context.SignedAddNoUnderflow(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedUnderflow; // Method returns true when NO underflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SignedAddNoUnderflow(BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SignedAddNoUnderflow(BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SignedAddNoUnderflow(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedAddNoUnderflow(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedAddNoUnderflow(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(128, 255, true, Description = "−128 / (−1) → +128 (not representable) → overflow")]
    [TestCase(128, 1, false, Description = "−128 / 1 = −128 → ok")]
    [TestCase(128, 2, false, Description = "−128 / 2 = −64 → ok")]
    [TestCase(127, 255, false, Description = "127 / (−1) = −127 → ok")]
    [TestCase(255, 255, false, Description = "−1 / (−1) = 1 → ok")]
    [TestCase(200, 2, false, Description = "−56 / 2 = −28 → ok")]
    [TestCase(200, 128, false, Description = "−56 / (−128) = 0 (toward 0) → ok")]
    [TestCase(0, 200, false, Description = "0 / (−56) = 0 → ok")]
    public void SignedDivNoOverflow_AllVariations_ReturnsExpectedResult(int left, int right, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(left);
        var y = context.BitVec<Size8>(right);
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of signed division overflow detection
        var resultMethodBitVec = x.SignedDivNoOverflow(y); // BitVec.SignedDivNoOverflow(BitVec) (method)
        var resultMethodBigInt = x.SignedDivNoOverflow(rightBigInt); // BitVec.SignedDivNoOverflow(BigInteger) (method)
        var resultContextBitVec = context.SignedDivNoOverflow(x, y); // Context.SignedDivNoOverflow(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.SignedDivNoOverflow(x, rightBigInt); // Context.SignedDivNoOverflow(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.SignedDivNoOverflow(leftBigInt, y); // Context.SignedDivNoOverflow(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.Multiple(() =>
        {
            Assert.That(
                model.GetBoolValue(resultMethodBitVec),
                Is.EqualTo(expectedResult),
                "BitVec.SignedDivNoOverflow(BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultMethodBigInt),
                Is.EqualTo(expectedResult),
                "BitVec.SignedDivNoOverflow(BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextBitVec),
                Is.EqualTo(expectedResult),
                "Context.SignedDivNoOverflow(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextRightBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedDivNoOverflow(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBoolValue(resultContextLeftBigInt),
                Is.EqualTo(expectedResult),
                "Context.SignedDivNoOverflow(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(128, true, Description = "−128 neg → +128 (not representable) → overflow")]
    [TestCase(127, false, Description = "127 neg → −127 → ok")]
    [TestCase(255, false, Description = "−1 neg → +1 → ok")]
    [TestCase(0, false, Description = "0 neg → 0 → ok")]
    [TestCase(200, false, Description = "−56 neg → +56 → ok")]
    public void SignedNegNoOverflow_AllVariations_ReturnsExpectedResult(int value, bool expectedOverflow)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec<Size8>(value);

        // Test signed negation overflow detection (unary operation)
        var resultMethod = x.SignedNegNoOverflow(); // BitVec.SignedNegNoOverflow() (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expectedResult = !expectedOverflow; // Method returns true when NO overflow

        Assert.That(
            model.GetBoolValue(resultMethod),
            Is.EqualTo(expectedResult),
            "BitVec.SignedNegNoOverflow() method failed"
        );
    }
}
