using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprBitwiseTests
{
    [TestCase(5, 3, 1, Description = "Basic AND (101 & 011 = 001)")]
    [TestCase(15, 7, 7, Description = "AND with subset (1111 & 0111 = 0111)")]
    [TestCase(0, 255, 0, Description = "AND with zero")]
    [TestCase(255, 255, 255, Description = "AND with self")]
    public void BitwiseAnd_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of bitwise AND
        var resultOperatorBitVec = x & y;                    // BitVec & BitVec (operator)
        var resultOperatorRightBigInt = x & right;           // BitVec & BigInteger (operator)
        var resultOperatorLeftBigInt = left & y;             // BigInteger & BitVec (operator)
        var resultMethodBitVec = x.And(y);                   // BitVec.And(BitVec) (method)
        var resultMethodBigInt = x.And(right);               // BitVec.And(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec & BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec & BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger & BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.And(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.And(BigInteger) method failed");
        });
    }

    [TestCase(5, 3, 7, Description = "Basic OR (101 | 011 = 111)")]
    [TestCase(8, 4, 12, Description = "OR different bits")]
    [TestCase(0, 255, 255, Description = "OR with all bits")]
    [TestCase(0, 0, 0, Description = "OR with zeros")]
    public void BitwiseOr_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of bitwise OR
        var resultOperatorBitVec = x | y;                    // BitVec | BitVec (operator)
        var resultOperatorRightBigInt = x | right;           // BitVec | BigInteger (operator)
        var resultOperatorLeftBigInt = left | y;             // BigInteger | BitVec (operator)
        var resultMethodBitVec = x.Or(y);                    // BitVec.Or(BitVec) (method)
        var resultMethodBigInt = x.Or(right);                // BitVec.Or(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec | BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec | BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger | BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Or(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Or(BigInteger) method failed");
        });
    }

    [TestCase(5, 3, 6, Description = "Basic XOR (101 ^ 011 = 110)")]
    [TestCase(15, 15, 0, Description = "XOR with self")]
    [TestCase(0, 255, 255, Description = "XOR with all bits")]
    [TestCase(170, 85, 255, Description = "Alternating patterns")]
    public void BitwiseXor_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(left, 8);
        var y = context.BitVec(right, 8);

        // Test all variations of bitwise XOR
        var resultOperatorBitVec = x ^ y;                    // BitVec ^ BitVec (operator)
        var resultOperatorRightBigInt = x ^ right;           // BitVec ^ BigInteger (operator)
        var resultOperatorLeftBigInt = left ^ y;             // BigInteger ^ BitVec (operator)
        var resultMethodBitVec = x.Xor(y);                   // BitVec.Xor(BitVec) (method)
        var resultMethodBigInt = x.Xor(right);               // BitVec.Xor(BigInteger) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec ^ BitVec operator failed");
            Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec ^ BigInteger operator failed");
            Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger ^ BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Xor(BitVec) method failed");
            Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Xor(BigInteger) method failed");
        });
    }

    [TestCase(5, 250, Description = "Basic NOT (00000101 -> 11111010)")]
    [TestCase(0, 255, Description = "NOT zero")]
    [TestCase(255, 0, Description = "NOT all ones")]
    [TestCase(170, 85, Description = "NOT alternating pattern")]
    public void BitwiseNot_AllVariations_ReturnsExpectedResult(int value, int expectedResult)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVec(value, 8);

        // Test all variations of bitwise NOT (unary operation)
        var resultOperator = ~x;                             // ~BitVec (operator)
        var resultMethod = x.Not();                          // BitVec.Not() (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperator), Is.EqualTo(expected), "~BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethod), Is.EqualTo(expected), "BitVec.Not() method failed");
        });
    }
}