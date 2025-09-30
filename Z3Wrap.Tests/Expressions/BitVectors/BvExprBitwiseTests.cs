using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.BitVectors;

[TestFixture]
public class BvExprBitwiseTests
{
    [Test]
    public void And_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(0b101010u);
        var b = context.BitVec<Size32>(0b110011u);

        var result = a & b;
        var resultViaUintLeft = 0b101010u & b;
        var resultViaUintRight = a & 0b110011u;
        var resultViaContext = context.And(a, b);
        var resultViaContextUintLeft = context.And(0b101010u, b);
        var resultViaContextUintRight = context.And(a, 0b110011u);
        var resultViaFunc = a.And(b);
        var resultViaFuncUintRight = a.And(0b110011u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(0b100010); // 0b101010 & 0b110011 = 0b100010
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Or_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(0b101010u);
        var b = context.BitVec<Size32>(0b110011u);

        var result = a | b;
        var resultViaUintLeft = 0b101010u | b;
        var resultViaUintRight = a | 0b110011u;
        var resultViaContext = context.Or(a, b);
        var resultViaContextUintLeft = context.Or(0b101010u, b);
        var resultViaContextUintRight = context.Or(a, 0b110011u);
        var resultViaFunc = a.Or(b);
        var resultViaFuncUintRight = a.Or(0b110011u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(0b111011); // 0b101010 | 0b110011 = 0b111011
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Xor_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(0b101010u);
        var b = context.BitVec<Size32>(0b110011u);

        var result = a ^ b;
        var resultViaUintLeft = 0b101010u ^ b;
        var resultViaUintRight = a ^ 0b110011u;
        var resultViaContext = context.Xor(a, b);
        var resultViaContextUintLeft = context.Xor(0b101010u, b);
        var resultViaContextUintRight = context.Xor(a, 0b110011u);
        var resultViaFunc = a.Xor(b);
        var resultViaFuncUintRight = a.Xor(0b110011u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(0b011001); // 0b101010 ^ 0b110011 = 0b011001
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Not_SingleValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(0b101010u);

        var result = ~a;
        var resultViaContext = context.Not(a);
        var resultViaFunc = a.Not();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(~0b101010u); // Bitwise NOT
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void ShiftLeft_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(5u);
        var b = context.BitVec<Size32>(3u);

        // Note: C# shift operators don't support uint << BvExpr (left operand must be the defining type)
        var result = a << b;
        var resultViaUintRight = a << 3u;
        var resultViaContext = context.Shl(a, b);
        var resultViaContextUintLeft = context.Shl(5u, b);
        var resultViaContextUintRight = context.Shl(a, 3u);
        var resultViaFunc = a.Shl(b);
        var resultViaFuncUintRight = a.Shl(3u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(40); // 5 << 3 = 40
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void LogicalShiftRight_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BitVec<Size32>(40u);
        var b = context.BitVec<Size32>(3u);

        // Note: C# shift operators don't support uint >> BvExpr (left operand must be the defining type)
        var result = a >> b;
        var resultViaUintRight = a >> 3u;
        var resultViaContext = context.Shr(a, b);
        var resultViaContextUintLeft = context.Shr(40u, b);
        var resultViaContextUintRight = context.Shr(a, 3u);
        var resultViaFunc = a.Shr(b);
        var resultViaFuncUintRight = a.Shr(3u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(5); // 40 >> 3 = 5
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void ArithmeticShiftRight_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Use a negative number (represented as unsigned)
        var a = context.BitVec<Size32>(unchecked((uint)-40));
        var b = context.BitVec<Size32>(3u);

        // Arithmetic shift right (signed) - no operator, only context and expr methods
        var resultViaContext = context.Shr(a, b, true);
        var resultViaContextUintLeft = context.Shr(unchecked((uint)-40), b, true);
        var resultViaContextUintRight = context.Shr(a, 3u, true);
        var resultViaFunc = a.Shr(b, true);
        var resultViaFuncUintRight = a.Shr(3u, true);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Arithmetic shift right preserves sign bit
        var expected = new BigInteger(unchecked((uint)(-40 >> 3)));
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBitVec(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }
}
