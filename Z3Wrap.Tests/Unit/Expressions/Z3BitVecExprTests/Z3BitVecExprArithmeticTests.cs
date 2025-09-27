using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprArithmeticTests
{
    [TestCase(5, 3, 8, Description = "Basic addition")]
    [TestCase(10, 5, 15, Description = "Another addition")]
    [TestCase(255, 1, 0, Description = "8-bit overflow wraps")]
    [TestCase(0, 0, 0, Description = "Adding zeros")]
    public void Add_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(left));
        var y = context.BitVec(new Bv<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of addition
        var resultOperatorBitVec = x + y; // BitVec + BitVec (operator)
        var resultOperatorRightBigInt = x + rightBigInt; // BitVec + BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt + y; // BigInteger + BitVec (operator)
        var resultMethodBitVec = x.Add(y); // BitVec.Add(BitVec) (method)
        var resultMethodBigInt = x.Add(rightBigInt); // BitVec.Add(BigInteger) (method)
        var resultContextBitVec = context.Add(x, y); // Context.Add(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Add(x, rightBigInt); // Context.Add(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Add(leftBigInt, y); // Context.Add(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new Bv<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec + BitVec operator failed");
            Assert.That(
                model.GetBitVec(resultOperatorRightBigInt),
                Is.EqualTo(expected),
                "BitVec + BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(resultOperatorLeftBigInt),
                Is.EqualTo(expected),
                "BigInteger + BitVec operator failed"
            );
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Add(BitVec) method failed");
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Add(BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Add(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Add(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Add(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(10, 3, 7, Description = "Basic subtraction")]
    [TestCase(5, 8, 253, Description = "Negative result wraps (5-8 = -3 = 253 unsigned)")]
    [TestCase(0, 1, 255, Description = "Zero minus one wraps")]
    [TestCase(100, 100, 0, Description = "Equal values result in zero")]
    public void Sub_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(left));
        var y = context.BitVec(new Bv<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of subtraction
        var resultOperatorBitVec = x - y; // BitVec - BitVec (operator)
        var resultOperatorRightBigInt = x - rightBigInt; // BitVec - BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt - y; // BigInteger - BitVec (operator)
        var resultMethodBitVec = x.Sub(y); // BitVec.Sub(BitVec) (method)
        var resultMethodBigInt = x.Sub(rightBigInt); // BitVec.Sub(BigInteger) (method)
        var resultContextBitVec = context.Sub(x, y); // Context.Sub(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Sub(x, rightBigInt); // Context.Sub(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Sub(leftBigInt, y); // Context.Sub(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new Bv<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec - BitVec operator failed");
            Assert.That(
                model.GetBitVec(resultOperatorRightBigInt),
                Is.EqualTo(expected),
                "BitVec - BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(resultOperatorLeftBigInt),
                Is.EqualTo(expected),
                "BigInteger - BitVec operator failed"
            );
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Sub(BitVec) method failed");
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Sub(BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Sub(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Sub(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Sub(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(5, 3, 15, Description = "Basic multiplication")]
    [TestCase(16, 16, 0, Description = "8-bit overflow wraps (256 % 256 = 0)")]
    [TestCase(0, 100, 0, Description = "Zero times anything")]
    [TestCase(1, 42, 42, Description = "One times value")]
    public void Mul_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(left));
        var y = context.BitVec(new Bv<Size8>(right));
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of multiplication
        var resultOperatorBitVec = x * y; // BitVec * BitVec (operator)
        var resultOperatorRightBigInt = x * rightBigInt; // BitVec * BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt * y; // BigInteger * BitVec (operator)
        var resultMethodBitVec = x.Mul(y); // BitVec.Mul(BitVec) (method)
        var resultMethodBigInt = x.Mul(rightBigInt); // BitVec.Mul(BigInteger) (method)
        var resultContextBitVec = context.Mul(x, y); // Context.Mul(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Mul(x, rightBigInt); // Context.Mul(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Mul(leftBigInt, y); // Context.Mul(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new Bv<Size8>(expectedResult);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec * BitVec operator failed");
            Assert.That(
                model.GetBitVec(resultOperatorRightBigInt),
                Is.EqualTo(expected),
                "BitVec * BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(resultOperatorLeftBigInt),
                Is.EqualTo(expected),
                "BigInteger * BitVec operator failed"
            );
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Mul(BitVec) method failed");
            Assert.That(
                model.GetBitVec(resultMethodBigInt),
                Is.EqualTo(expected),
                "BitVec.Mul(BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextBitVec),
                Is.EqualTo(expected),
                "Context.Mul(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Mul(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(resultContextLeftBigInt),
                Is.EqualTo(expected),
                "Context.Mul(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(15, 3, 5, 0, Description = "Basic unsigned division: 15/3=5, 15%3=0")]
    [TestCase(10, 2, 5, 0, Description = "Even division: 10/2=5, 10%2=0")]
    [TestCase(7, 3, 2, 1, Description = "Division with remainder: 7/3=2, 7%3=1")]
    [TestCase(100, 1, 100, 0, Description = "Division by one: 100/1=100, 100%1=0")]
    [TestCase(10, 3, 3, 1, Description = "Standard case: 10/3=3, 10%3=1")]
    [TestCase(5, 10, 0, 5, Description = "Dividend smaller than divisor: 5/10=0, 5%10=5")]
    public void UnsignedDivision_AllVariations_ReturnsExpectedResults(
        int dividend,
        int divisor,
        int expectedDiv,
        int expectedRem
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(dividend));
        var y = context.BitVec(new Bv<Size8>(divisor));
        var left = new BigInteger(dividend);
        var right = new BigInteger(divisor);

        // Test all variations of unsigned division operations
        var divResultOperatorBitVec = x / y; // BitVec / BitVec (operator)
        var divResultOperatorRightBigInt = x / right; // BitVec / BigInteger (operator)
        var divResultOperatorLeftBigInt = left / y; // BigInteger / BitVec (operator)
        var divResultBitVec = x.Div(y, signed: false); // BitVec.Div(BitVec, unsigned) (method)
        var divResultRightBigInt = x.Div(right, signed: false); // BitVec.Div(BigInteger, unsigned) (method)
        var divResultContextBitVec = context.Div(x, y, signed: false); // Context.Div(BitVec, BitVec, unsigned) (method)
        var divResultContextRightBigInt = context.Div(x, right, signed: false); // Context.Div(BitVec, BigInteger, unsigned) (method)
        var divResultLeftBigInt = context.Div(left, y, signed: false); // Context.Div(BigInteger, BitVec, unsigned) (method)

        var remResultOperatorBitVec = x % y; // BitVec % BitVec (operator)
        var remResultOperatorRightBigInt = x % right; // BitVec % BigInteger (operator)
        var remResultOperatorLeftBigInt = left % y; // BigInteger % BitVec (operator)
        var remResultBitVec = x.Rem(y, signed: false); // BitVec.Rem(BitVec, unsigned) (method)
        var remResultRightBigInt = x.Rem(right, signed: false); // BitVec.Rem(BigInteger, unsigned) (method)
        var remResultContextBitVec = context.Rem(x, y, signed: false); // Context.Rem(BitVec, BitVec, unsigned) (method)
        var remResultContextRightBigInt = context.Rem(x, right, signed: false); // Context.Rem(BitVec, BigInteger, unsigned) (method)
        var remResultLeftBigInt = context.Rem(left, y, signed: false); // Context.Rem(BigInteger, BitVec, unsigned) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var expectedDivBitVec = new Bv<Size8>(expectedDiv);
        var expectedRemBitVec = new Bv<Size8>(expectedRem);

        Assert.Multiple(() =>
        {
            // Test unsigned division operators
            Assert.That(
                model.GetBitVec(divResultOperatorBitVec),
                Is.EqualTo(expectedDivBitVec),
                "BitVec / BitVec operator failed"
            );
            Assert.That(
                model.GetBitVec(divResultOperatorRightBigInt),
                Is.EqualTo(expectedDivBitVec),
                "BitVec / BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(divResultOperatorLeftBigInt),
                Is.EqualTo(expectedDivBitVec),
                "BigInteger / BitVec operator failed"
            );

            // Test unsigned division methods
            Assert.That(
                model.GetBitVec(divResultBitVec),
                Is.EqualTo(expectedDivBitVec),
                "BitVec.Div(BitVec, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultRightBigInt),
                Is.EqualTo(expectedDivBitVec),
                "BitVec.Div(BigInteger, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultContextBitVec),
                Is.EqualTo(expectedDivBitVec),
                "Context.Div(BitVec, BitVec, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultContextRightBigInt),
                Is.EqualTo(expectedDivBitVec),
                "Context.Div(BitVec, BigInteger, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultLeftBigInt),
                Is.EqualTo(expectedDivBitVec),
                "Context.Div(BigInteger, BitVec, unsigned) method failed"
            );

            // Test unsigned remainder operators
            Assert.That(
                model.GetBitVec(remResultOperatorBitVec),
                Is.EqualTo(expectedRemBitVec),
                "BitVec % BitVec operator failed"
            );
            Assert.That(
                model.GetBitVec(remResultOperatorRightBigInt),
                Is.EqualTo(expectedRemBitVec),
                "BitVec % BigInteger operator failed"
            );
            Assert.That(
                model.GetBitVec(remResultOperatorLeftBigInt),
                Is.EqualTo(expectedRemBitVec),
                "BigInteger % BitVec operator failed"
            );

            // Test unsigned remainder methods
            Assert.That(
                model.GetBitVec(remResultBitVec),
                Is.EqualTo(expectedRemBitVec),
                "BitVec.Rem(BitVec, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultRightBigInt),
                Is.EqualTo(expectedRemBitVec),
                "BitVec.Rem(BigInteger, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultContextBitVec),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BitVec, BitVec, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultContextRightBigInt),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BitVec, BigInteger, unsigned) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultLeftBigInt),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BigInteger, BitVec, unsigned) method failed"
            );
        });
    }

    [TestCase(15, 3, 5, 0, 0, Description = "Positive / positive: 15/3=5, 15%3=0, smod=0")]
    [TestCase(-10, 3, 253, 255, 2, Description = "Negative / positive: -10/3=-3(253), -10%3=-1(255), smod=2")]
    [TestCase(10, -3, 253, 1, 254, Description = "Positive / negative: 10/(-3)=-3(253), 10%(-3)=1, smod=254")]
    [TestCase(-10, -3, 3, 255, 255, Description = "Negative / negative: -10/(-3)=3, -10%(-3)=-1(255), smod=-1(255)")]
    [TestCase(7, 7, 1, 0, 0, Description = "Equal values: 7/7=1, 7%7=0, smod=0")]
    [TestCase(-128, 1, 128, 0, 0, Description = "Most negative 8-bit: -128/1=-128(128), -128%1=0, smod=0")]
    public void SignedDivision_AllVariations_ReturnsExpectedResults(
        int dividend,
        int divisor,
        int expectedDiv,
        int expectedRem,
        int expectedSignedMod
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(dividend));
        var y = context.BitVec(new Bv<Size8>(divisor));
        var left = new BigInteger(dividend);
        var right = new BigInteger(divisor);
        var expected = new Bv<Size8>(expectedDiv);
        var expectedRemBitVec = new Bv<Size8>(expectedRem);
        var expectedSignedModBitVec = new Bv<Size8>(expectedSignedMod);

        // Test all variations of signed division operations
        var divResultBitVec = x.Div(y, signed: true); // BitVec.Div(BitVec, signed) (method)
        var divResultRightBigInt = x.Div(right, signed: true); // BitVec.Div(BigInteger, signed) (method)
        var divResultContextBitVec = context.Div(x, y, signed: true); // Context.Div(BitVec, BitVec, signed) (method)
        var divResultContextRightBigInt = context.Div(x, right, signed: true); // Context.Div(BitVec, BigInteger, signed) (method)
        var divResultLeftBigInt = context.Div(left, y, signed: true); // Context.Div(BigInteger, BitVec, signed) (method)

        var remResultBitVec = x.Rem(y, signed: true); // BitVec.Rem(BitVec, signed) (method)
        var remResultRightBigInt = x.Rem(right, signed: true); // BitVec.Rem(BigInteger, signed) (method)
        var remResultContextBitVec = context.Rem(x, y, signed: true); // Context.Rem(BitVec, BitVec, signed) (method)
        var remResultContextRightBigInt = context.Rem(x, right, signed: true); // Context.Rem(BitVec, BigInteger, signed) (method)
        var remResultLeftBigInt = context.Rem(left, y, signed: true); // Context.Rem(BigInteger, BitVec, signed) (method)

        var signedModResultBitVec = x.SignedMod(y); // BitVec.SignedMod(BitVec) (method)
        var signedModResultBigInt = x.SignedMod(right); // BitVec.SignedMod(BigInteger) (method)
        var signedModResultContextBitVec = context.SignedMod(x, y); // Context.SignedMod(BitVec, BitVec) (method)
        var signedModResultContextRightBigInt = context.SignedMod(x, right); // Context.SignedMod(BitVec, BigInteger) (method)
        var signedModResultLeftBigInt = context.SignedMod(left, y); // Context.SignedMod(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            // Test signed division methods
            Assert.That(
                model.GetBitVec(divResultBitVec),
                Is.EqualTo(expected),
                "BitVec.Div(BitVec, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultRightBigInt),
                Is.EqualTo(expected),
                "BitVec.Div(BigInteger, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultContextBitVec),
                Is.EqualTo(expected),
                "Context.Div(BitVec, BitVec, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultContextRightBigInt),
                Is.EqualTo(expected),
                "Context.Div(BitVec, BigInteger, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(divResultLeftBigInt),
                Is.EqualTo(expected),
                "Context.Div(BigInteger, BitVec, signed) method failed"
            );

            // Test signed remainder methods
            Assert.That(
                model.GetBitVec(remResultBitVec),
                Is.EqualTo(expectedRemBitVec),
                "BitVec.Rem(BitVec, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultRightBigInt),
                Is.EqualTo(expectedRemBitVec),
                "BitVec.Rem(BigInteger, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultContextBitVec),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BitVec, BitVec, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultContextRightBigInt),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BitVec, BigInteger, signed) method failed"
            );
            Assert.That(
                model.GetBitVec(remResultLeftBigInt),
                Is.EqualTo(expectedRemBitVec),
                "Context.Rem(BigInteger, BitVec, signed) method failed"
            );

            // Test signed modulo methods
            Assert.That(
                model.GetBitVec(signedModResultBitVec),
                Is.EqualTo(expectedSignedModBitVec),
                "BitVec.SignedMod(BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(signedModResultBigInt),
                Is.EqualTo(expectedSignedModBitVec),
                "BitVec.SignedMod(BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(signedModResultContextBitVec),
                Is.EqualTo(expectedSignedModBitVec),
                "Context.SignedMod(BitVec, BitVec) method failed"
            );
            Assert.That(
                model.GetBitVec(signedModResultContextRightBigInt),
                Is.EqualTo(expectedSignedModBitVec),
                "Context.SignedMod(BitVec, BigInteger) method failed"
            );
            Assert.That(
                model.GetBitVec(signedModResultLeftBigInt),
                Is.EqualTo(expectedSignedModBitVec),
                "Context.SignedMod(BigInteger, BitVec) method failed"
            );
        });
    }

    [TestCase(42, 214, Description = "Positive value negation")]
    [TestCase(0, 0, Description = "Zero negation")]
    [TestCase(-1, 1, Description = "Negative one")]
    [TestCase(-128, 128, Description = "Most negative 8-bit")]
    public void Neg_AllVariations_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new Bv<Size8>(value));
        var expected = new Bv<Size8>(expectedResult);

        // Test all variations of negation
        var resultOperator = -x; // -BitVec (unary operator)
        var resultMethod = x.Neg(); // BitVec.Neg() (method)
        var resultContext = context.Neg(x); // Context.Neg(BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperator), Is.EqualTo(expected), "-BitVec unary operator failed");
            Assert.That(model.GetBitVec(resultMethod), Is.EqualTo(expected), "BitVec.Neg() method failed");
            Assert.That(model.GetBitVec(resultContext), Is.EqualTo(expected), "Context.Neg(BitVec) method failed");
        });
    }
}
