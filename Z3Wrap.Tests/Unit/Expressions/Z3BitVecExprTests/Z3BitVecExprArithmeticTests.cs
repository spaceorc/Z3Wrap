using System.Numerics;
using Z3Wrap.DataTypes;

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

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of addition
        var resultOperatorBitVec = x + y;                    // BitVec + BitVec (operator)
        var resultOperatorRightBigInt = x + right;           // BitVec + BigInteger (operator)
        var resultOperatorLeftBigInt = left + y;             // BigInteger + BitVec (operator)
        var resultMethodBitVec = x.Add(y);                   // BitVec.Add(BitVec) (method)
        var resultMethodBigInt = x.Add(right);               // BitVec.Add(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec + BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec + BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger + BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Add(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Add(BigInteger) method failed");
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

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of subtraction
        var resultOperatorBitVec = x - y;                    // BitVec - BitVec (operator)
        var resultOperatorRightBigInt = x - right;           // BitVec - BigInteger (operator)
        var resultOperatorLeftBigInt = left - y;             // BigInteger - BitVec (operator)
        var resultMethodBitVec = x.Sub(y);                   // BitVec.Sub(BitVec) (method)
        var resultMethodBigInt = x.Sub(right);               // BitVec.Sub(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec - BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec - BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger - BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Sub(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Sub(BigInteger) method failed");
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

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of multiplication
        var resultOperatorBitVec = x * y;                    // BitVec * BitVec (operator)
        var resultOperatorRightBigInt = x * right;           // BitVec * BigInteger (operator)
        var resultOperatorLeftBigInt = left * y;             // BigInteger * BitVec (operator)
        var resultMethodBitVec = x.Mul(y);                   // BitVec.Mul(BitVec) (method)
        var resultMethodBigInt = x.Mul(right);               // BitVec.Mul(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec * BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec * BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger * BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Mul(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Mul(BigInteger) method failed");
        });
    }

    [TestCase(15, 3, 5, 0, Description = "Basic unsigned division: 15/3=5, 15%3=0")]
    [TestCase(10, 2, 5, 0, Description = "Even division: 10/2=5, 10%2=0")]
    [TestCase(7, 3, 2, 1, Description = "Division with remainder: 7/3=2, 7%3=1")]
    [TestCase(100, 1, 100, 0, Description = "Division by one: 100/1=100, 100%1=0")]
    [TestCase(10, 3, 3, 1, Description = "Standard case: 10/3=3, 10%3=1")]
    [TestCase(5, 10, 0, 5, Description = "Dividend smaller than divisor: 5/10=0, 5%10=5")]
    public void UnsignedDivision_AllVariations_ReturnsExpectedResults(int dividend, int divisor, int expectedDiv, int expectedRem)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(dividend, 8);
        var y = context.BitVec(divisor, 8);
        var divResult = x.Div(y, signed: false);
        var remResult = x.Rem(y, signed: false);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(divResult), Is.EqualTo(new BitVec(expectedDiv, 8)), "Division result mismatch");
            Assert.That(model.GetBitVec(remResult), Is.EqualTo(new BitVec(expectedRem, 8)), "Remainder result mismatch");
        });
    }

    [TestCase(15, 3, 5, 0, 0, Description = "Positive / positive: 15/3=5, 15%3=0, smod=0")]
    [TestCase(-10, 3, 253, 255, 2, Description = "Negative / positive: -10/3=-3(253), -10%3=-1(255), smod=2")]
    [TestCase(10, -3, 253, 1, 254, Description = "Positive / negative: 10/(-3)=-3(253), 10%(-3)=1, smod=254")]
    [TestCase(-10, -3, 3, 255, 255, Description = "Negative / negative: -10/(-3)=3, -10%(-3)=-1(255), smod=-1(255)")]
    [TestCase(7, 7, 1, 0, 0, Description = "Equal values: 7/7=1, 7%7=0, smod=0")]
    [TestCase(-128, 1, 128, 0, 0, Description = "Most negative 8-bit: -128/1=-128(128), -128%1=0, smod=0")]
    public void SignedDivision_AllVariations_ReturnsExpectedResults(int dividend, int divisor, int expectedDiv, int expectedRem, int expectedSignedMod)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(dividend, 8);
        var y = context.BitVec(divisor, 8);
        var left = new BigInteger(dividend);
        var right = new BigInteger(divisor);
        var expected = new BitVec(expectedDiv, 8);
        var expectedRemBitVec = new BitVec(expectedRem, 8);
        var expectedSignedModBitVec = new BitVec(expectedSignedMod, 8);

        // Test all variations of signed division operations
        // Note: Operators (/ and %) use unsigned semantics, so we test them separately for unsigned results
        var divResultOperatorBitVec = x / y;                         // BitVec / BitVec (operator - unsigned)
        var divResultOperatorRightBigInt = x / right;                // BitVec / BigInteger (operator - unsigned)
        var divResultOperatorLeftBigInt = left / y;                  // BigInteger / BitVec (operator - unsigned)
        var divResultBitVec = x.Div(y, signed: true);               // BitVec.Div(BitVec, signed) (method)
        var divResultRightBigInt = x.Div(right, signed: true);      // BitVec.Div(BigInteger, signed) (method)
        var divResultLeftBigInt = context.Div(left, y, signed: true); // Context.Div(BigInteger, BitVec, signed) (method)

        var remResultOperatorBitVec = x % y;                         // BitVec % BitVec (operator - unsigned)
        var remResultOperatorRightBigInt = x % right;                // BitVec % BigInteger (operator - unsigned)
        var remResultOperatorLeftBigInt = left % y;                  // BigInteger % BitVec (operator - unsigned)
        var remResultBitVec = x.Rem(y, signed: true);               // BitVec.Rem(BitVec, signed) (method)
        var remResultRightBigInt = x.Rem(right, signed: true);      // BitVec.Rem(BigInteger, signed) (method)
        var remResultLeftBigInt = context.Rem(left, y, signed: true); // Context.Rem(BigInteger, BitVec, signed) (method)

        var signedModResultBitVec = x.SignedMod(y);                  // BitVec.SignedMod(BitVec) (method)
        var signedModResultBigInt = x.SignedMod(right);              // BitVec.SignedMod(BigInteger) (method)
        var signedModResultLeftBigInt = context.SignedMod(left, y);  // Context.SignedMod(BigInteger, BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Calculate expected unsigned results for operators
        var unsignedDiv = context.Div(x, y, signed: false);
        var unsignedRem = context.Rem(x, y, signed: false);

        Assert.Multiple(() =>
        {
            // Test unsigned operators (/ and %)
            Assert.That(model.GetBitVec(divResultOperatorBitVec), Is.EqualTo(model.GetBitVec(unsignedDiv)), "BitVec / BitVec operator failed (unsigned)");
            Assert.That(model.GetBitVec(divResultOperatorRightBigInt), Is.EqualTo(model.GetBitVec(unsignedDiv)), "BitVec / BigInteger operator failed (unsigned)");
            Assert.That(model.GetBitVec(divResultOperatorLeftBigInt), Is.EqualTo(model.GetBitVec(unsignedDiv)), "BigInteger / BitVec operator failed (unsigned)");

            Assert.That(model.GetBitVec(remResultOperatorBitVec), Is.EqualTo(model.GetBitVec(unsignedRem)), "BitVec % BitVec operator failed (unsigned)");
            Assert.That(model.GetBitVec(remResultOperatorRightBigInt), Is.EqualTo(model.GetBitVec(unsignedRem)), "BitVec % BigInteger operator failed (unsigned)");
            Assert.That(model.GetBitVec(remResultOperatorLeftBigInt), Is.EqualTo(model.GetBitVec(unsignedRem)), "BigInteger % BitVec operator failed (unsigned)");

            // Test signed methods
            Assert.That(model.GetBitVec(divResultBitVec), Is.EqualTo(expected), "BitVec.Div(BitVec, signed) method failed");
            Assert.That(model.GetBitVec(divResultRightBigInt), Is.EqualTo(expected), "BitVec.Div(BigInteger, signed) method failed");
            Assert.That(model.GetBitVec(divResultLeftBigInt), Is.EqualTo(expected), "Context.Div(BigInteger, BitVec, signed) method failed");

            Assert.That(model.GetBitVec(remResultBitVec), Is.EqualTo(expectedRemBitVec), "BitVec.Rem(BitVec, signed) method failed");
            Assert.That(model.GetBitVec(remResultRightBigInt), Is.EqualTo(expectedRemBitVec), "BitVec.Rem(BigInteger, signed) method failed");
            Assert.That(model.GetBitVec(remResultLeftBigInt), Is.EqualTo(expectedRemBitVec), "Context.Rem(BigInteger, BitVec, signed) method failed");

            Assert.That(model.GetBitVec(signedModResultBitVec), Is.EqualTo(expectedSignedModBitVec), "BitVec.SignedMod(BitVec) method failed");
            Assert.That(model.GetBitVec(signedModResultBigInt), Is.EqualTo(expectedSignedModBitVec), "BitVec.SignedMod(BigInteger) method failed");
            Assert.That(model.GetBitVec(signedModResultLeftBigInt), Is.EqualTo(expectedSignedModBitVec), "Context.SignedMod(BigInteger, BitVec) method failed");
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

        var x = context.BitVec(value, 8);
        var expected = new BitVec(expectedResult, 8);

        // Test all variations of negation
        var resultOperator = -x;           // -BitVec (unary operator)
        var resultMethod = x.Neg();        // BitVec.Neg() (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperator), Is.EqualTo(expected), "-BitVec unary operator failed");
            Assert.That(model.GetBitVec(resultMethod), Is.EqualTo(expected), "BitVec.Neg() method failed");
        });
    }
}