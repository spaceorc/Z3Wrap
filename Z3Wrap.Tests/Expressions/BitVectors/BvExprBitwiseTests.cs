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

        var a = context.Bv<Size32>(0b101010u);
        var b = context.Bv<Size32>(0b110011u);

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
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Or_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(0b101010u);
        var b = context.Bv<Size32>(0b110011u);

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
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Xor_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(0b101010u);
        var b = context.Bv<Size32>(0b110011u);

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
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void Not_SingleValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(0b101010u);

        var result = ~a;
        var resultViaContext = context.Not(a);
        var resultViaFunc = a.Not();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var expected = new BigInteger(~0b101010u); // Bitwise NOT
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void ShiftLeft_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(5u);
        var b = context.Bv<Size32>(3u);

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
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void LogicalShiftRight_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size32>(40u);
        var b = context.Bv<Size32>(3u);

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
            Assert.That(model.GetBv(result).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void ArithmeticShiftRight_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Use a negative number (represented as unsigned)
        var a = context.Bv<Size32>(unchecked((uint)-40));
        var b = context.Bv<Size32>(3u);

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
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void RotateLeft_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size8>(0b00001010u); // 10
        var b = context.Bv<Size8>(2u);

        // 5 variants (no operator for rotate)
        var resultViaContext = context.RotateLeft(a, b);
        var resultViaContextUintLeft = context.RotateLeft(0b00001010u, b);
        var resultViaContextUintRight = context.RotateLeft(a, 2u);
        var resultViaFunc = a.RotateLeft(b);
        var resultViaFuncUintRight = a.RotateLeft(2u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // 0b00001010 rotated left by 2 = 0b00101000 = 40
        var expected = new BigInteger(0b00101000);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void RotateRight_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size8>(0b00101000u); // 40
        var b = context.Bv<Size8>(2u);

        // 5 variants (no operator for rotate)
        var resultViaContext = context.RotateRight(a, b);
        var resultViaContextUintLeft = context.RotateRight(0b00101000u, b);
        var resultViaContextUintRight = context.RotateRight(a, 2u);
        var resultViaFunc = a.RotateRight(b);
        var resultViaFuncUintRight = a.RotateRight(2u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // 0b00101000 rotated right by 2 = 0b00001010 = 10
        var expected = new BigInteger(0b00001010);
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBv(resultViaContext).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintLeft).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaContextUintRight).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFunc).Value, Is.EqualTo(expected));
            Assert.That(model.GetBv(resultViaFuncUintRight).Value, Is.EqualTo(expected));
        });
    }

    [Test]
    public void RotateLeft_ByZero_ReturnsOriginalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size8>(0b10101010u);

        var result = a.RotateLeft(0u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBv(result).Value, Is.EqualTo(new BigInteger(0b10101010)));
    }

    [Test]
    public void RotateLeft_ByFullWidth_ReturnsOriginalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bv<Size8>(0b10101010u);

        // Rotation by 8 on Size8 should return original value
        var result = a.RotateLeft(8u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBv(result).Value, Is.EqualTo(new BigInteger(0b10101010)));
    }

    [Test]
    public void RotateLeft_WithWrapAround_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // High bits should wrap to low bits
        var a = context.Bv<Size8>(0b10000001u);

        var result = a.RotateLeft(1u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // 0b10000001 rotated left by 1 = 0b00000011
        Assert.That(model.GetBv(result).Value, Is.EqualTo(new BigInteger(0b00000011)));
    }

    [Test]
    public void RotateRight_WithWrapAround_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Low bits should wrap to high bits
        var a = context.Bv<Size8>(0b00000011u);

        var result = a.RotateRight(1u);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // 0b00000011 rotated right by 1 = 0b10000001
        Assert.That(model.GetBv(result).Value, Is.EqualTo(new BigInteger(0b10000001)));
    }
}
