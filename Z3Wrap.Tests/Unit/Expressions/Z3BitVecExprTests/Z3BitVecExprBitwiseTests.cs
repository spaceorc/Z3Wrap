using Spaceorc.Z3Wrap;
using System.Numerics;
using Spaceorc.Z3Wrap.DataTypes;

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
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of bitwise AND
        var resultOperatorBitVec = x & y;                    // BitVec & BitVec (operator)
        var resultOperatorRightBigInt = x & rightBigInt;     // BitVec & BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt & y;       // BigInteger & BitVec (operator)
        var resultMethodBitVec = x.And(y);                   // BitVec.And(BitVec) (method)
        var resultMethodBigInt = x.And(rightBigInt);         // BitVec.And(BigInteger) (method)
        var resultContextBitVec = context.And(x, y);         // Context.And(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.And(x, rightBigInt); // Context.And(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.And(leftBigInt, y);   // Context.And(BigInteger, BitVec) (method)

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
            Assert.That(model.GetBitVec(resultContextBitVec), Is.EqualTo(expected), "Context.And(BitVec, BitVec) method failed");
            Assert.That(model.GetBitVec(resultContextRightBigInt), Is.EqualTo(expected), "Context.And(BitVec, BigInteger) method failed");
            Assert.That(model.GetBitVec(resultContextLeftBigInt), Is.EqualTo(expected), "Context.And(BigInteger, BitVec) method failed");
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
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of bitwise OR
        var resultOperatorBitVec = x | y;                        // BitVec | BitVec (operator)
        var resultOperatorRightBigInt = x | rightBigInt;         // BitVec | BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt | y;           // BigInteger | BitVec (operator)
        var resultMethodBitVec = x.Or(y);                        // BitVec.Or(BitVec) (method)
        var resultMethodBigInt = x.Or(rightBigInt);              // BitVec.Or(BigInteger) (method)
        var resultContextBitVec = context.Or(x, y);              // Context.Or(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Or(x, rightBigInt); // Context.Or(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Or(leftBigInt, y);   // Context.Or(BigInteger, BitVec) (method)

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
            Assert.That(model.GetBitVec(resultContextBitVec), Is.EqualTo(expected), "Context.Or(BitVec, BitVec) method failed");
            Assert.That(model.GetBitVec(resultContextRightBigInt), Is.EqualTo(expected), "Context.Or(BitVec, BigInteger) method failed");
            Assert.That(model.GetBitVec(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Or(BigInteger, BitVec) method failed");
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
        var leftBigInt = new BigInteger(left);
        var rightBigInt = new BigInteger(right);

        // Test all variations of bitwise XOR
        var resultOperatorBitVec = x ^ y;                        // BitVec ^ BitVec (operator)
        var resultOperatorRightBigInt = x ^ rightBigInt;         // BitVec ^ BigInteger (operator)
        var resultOperatorLeftBigInt = leftBigInt ^ y;           // BigInteger ^ BitVec (operator)
        var resultMethodBitVec = x.Xor(y);                       // BitVec.Xor(BitVec) (method)
        var resultMethodBigInt = x.Xor(rightBigInt);             // BitVec.Xor(BigInteger) (method)
        var resultContextBitVec = context.Xor(x, y);             // Context.Xor(BitVec, BitVec) (method)
        var resultContextRightBigInt = context.Xor(x, rightBigInt); // Context.Xor(BitVec, BigInteger) (method)
        var resultContextLeftBigInt = context.Xor(leftBigInt, y);   // Context.Xor(BigInteger, BitVec) (method)

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
            Assert.That(model.GetBitVec(resultContextBitVec), Is.EqualTo(expected), "Context.Xor(BitVec, BitVec) method failed");
            Assert.That(model.GetBitVec(resultContextRightBigInt), Is.EqualTo(expected), "Context.Xor(BitVec, BigInteger) method failed");
            Assert.That(model.GetBitVec(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Xor(BigInteger, BitVec) method failed");
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
        var resultContext = context.Not(x);                  // Context.Not(BitVec) (method)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var expected = new BitVec(expectedResult, 8);

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultOperator), Is.EqualTo(expected), "~BitVec operator failed");
            Assert.That(model.GetBitVec(resultMethod), Is.EqualTo(expected), "BitVec.Not() method failed");
            Assert.That(model.GetBitVec(resultContext), Is.EqualTo(expected), "Context.Not(BitVec) method failed");
        });
    }
}