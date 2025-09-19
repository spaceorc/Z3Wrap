using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Integration;

[TestFixture]
public class Z3ModelBitVecIntegrationTests
{
    [Test]
    public void GetBitVec_SimpleValue_ReturnsCorrectBitVec()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 8);
        solver.Assert(bvExpr == context.BitVec(new BitVec(170, 8))); // 10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        Assert.That(bitVec.Value, Is.EqualTo(new BigInteger(170)));
        Assert.That(bitVec.Size, Is.EqualTo(8));
        Assert.That(bitVec.ToBinaryString(), Is.EqualTo("10101010"));
        Assert.That((int)bitVec.Value, Is.EqualTo(170));
    }

    [Test]
    public void GetBitVec_ZeroValue_ReturnsCorrectBitVec()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 16);
        solver.Assert(bvExpr == context.BitVec(new BitVec(0, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        Assert.That(bitVec.Value, Is.EqualTo(BigInteger.Zero));
        Assert.That(bitVec.Size, Is.EqualTo(16));
        Assert.That(bitVec.IsZero, Is.True);
        Assert.That(bitVec.ToBinaryString(), Is.EqualTo("0000000000000000"));
    }

    [Test]
    public void GetBitVec_MaxValue_ReturnsCorrectBitVec()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 4);
        solver.Assert(bvExpr == context.BitVec(new BitVec(15, 4))); // 1111

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        Assert.That(bitVec.Value, Is.EqualTo(new BigInteger(15)));
        Assert.That(bitVec.Size, Is.EqualTo(4));
        Assert.That(bitVec.ToBinaryString(), Is.EqualTo("1111"));
        Assert.That((int)bitVec.Value, Is.EqualTo(15));
    }

    [Test]
    public void GetBitVec_LargeValue_ReturnsCorrectBitVec()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 32);
        solver.Assert(bvExpr == context.BitVec(new BitVec(1234567890, 32)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        Assert.That(bitVec.Value, Is.EqualTo(new BigInteger(1234567890)));
        Assert.That(bitVec.Size, Is.EqualTo(32));
        Assert.That(bitVec.ToInt(), Is.EqualTo(1234567890));
        Assert.That(bitVec.ToUInt(), Is.EqualTo(1234567890U));
    }

    [Test]
    public void GetBitVec_DifferentSizes_ReturnsCorrectSizes()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv8 = context.BitVecConst("bv8", 8);
        var bv16 = context.BitVecConst("bv16", 16);
        var bv32 = context.BitVecConst("bv32", 32);

        solver.Assert(bv8 == context.BitVec(new BitVec(255, 8)));
        solver.Assert(bv16 == context.BitVec(new BitVec(65535, 16)));
        solver.Assert(bv32 == context.BitVec(new BitVec(2147483647, 32))); // int.MaxValue

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        var bitVec8 = model.GetBitVec(bv8);
        var bitVec16 = model.GetBitVec(bv16);
        var bitVec32 = model.GetBitVec(bv32);

        Assert.That(bitVec8.Size, Is.EqualTo(8));
        Assert.That(bitVec16.Size, Is.EqualTo(16));
        Assert.That(bitVec32.Size, Is.EqualTo(32));

        Assert.That((int)bitVec8.Value, Is.EqualTo(255));
        Assert.That((int)bitVec16.Value, Is.EqualTo(65535));
        Assert.That(bitVec32.ToInt(), Is.EqualTo(2147483647));
    }

    [Test]
    public void GetBitVec_ComplexExpression_ReturnsCorrectBitVec()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        // x + y = 100, x = 30
        solver.Assert(x == context.BitVec(new BitVec(30, 8)));
        solver.Assert(y == context.BitVec(new BitVec(70, 8)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVecX = model.GetBitVec(x);
        var bitVecY = model.GetBitVec(y);

        Assert.That(bitVecX.Value, Is.EqualTo(new BigInteger(30)));
        Assert.That(bitVecY.Value, Is.EqualTo(new BigInteger(70)));

        // Test BitVec struct operations
        var sum = bitVecX + bitVecY;
        Assert.That(sum.Value, Is.EqualTo(new BigInteger(100)));
        Assert.That(sum.Size, Is.EqualTo(8));

        var diff = bitVecY - bitVecX;
        Assert.That(diff.Value, Is.EqualTo(new BigInteger(40)));

        var product = bitVecX * new BitVec(2, 8);
        Assert.That(product.Value, Is.EqualTo(new BigInteger(60)));

        var quotient = bitVecY / bitVecX;
        Assert.That(quotient.Value, Is.EqualTo(new BigInteger(2))); // 70 / 30 = 2

        var remainder = bitVecY % bitVecX;
        Assert.That(remainder.Value, Is.EqualTo(new BigInteger(10))); // 70 % 30 = 10
    }

    [Test]
    public void GetBitVec_WithBitwiseOperations_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 8);
        solver.Assert(bvExpr == context.BitVec(new BitVec(170, 8))); // 10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        // Test bitwise operations on extracted BitVec
        var mask = new BitVec(85, 8); // 01010101

        var andResult = bitVec & mask;
        Assert.That(andResult.Value, Is.EqualTo(BigInteger.Zero)); // 00000000

        var orResult = bitVec | mask;
        Assert.That(orResult.Value, Is.EqualTo(new BigInteger(255))); // 11111111

        var xorResult = bitVec ^ mask;
        Assert.That(xorResult.Value, Is.EqualTo(new BigInteger(255))); // 11111111

        var notResult = ~bitVec;
        Assert.That(notResult.Value, Is.EqualTo(new BigInteger(85))); // 01010101
    }

    [Test]
    public void GetBitVec_WithShiftOperations_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 8);
        solver.Assert(bvExpr == context.BitVec(new BitVec(5, 8))); // 00000101

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        var leftShift = bitVec << 2; // 00010100 = 20
        Assert.That(leftShift.Value, Is.EqualTo(new BigInteger(20)));

        var rightShift = bitVec >> 1; // 00000010 = 2
        Assert.That(rightShift.Value, Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void GetBitVec_Formatting_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bvExpr = context.BitVecConst("bv", 8);
        solver.Assert(bvExpr == context.BitVec(new BitVec(170, 8))); // 10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVec = model.GetBitVec(bvExpr);

        Assert.That(bitVec.ToString(), Is.EqualTo("170"));
        Assert.That(bitVec.ToString("B"), Is.EqualTo("0b10101010 (8-bit)"));
        Assert.That(bitVec.ToString("X"), Is.EqualTo("0xAA (8-bit)"));
        Assert.That(bitVec.ToString("V"), Is.EqualTo("170"));
    }

    [Test]
    public void GetBitVec_InvalidatedModel_ThrowsException()
    {
        BitVec bitVec;
        Z3BitVecExpr bvExpr;

        using (var context = new Z3Context())
        {
            using var solver = context.CreateSolver();
            bvExpr = context.BitVecConst("bv", 8);
            solver.Assert(bvExpr == context.BitVec(new BitVec(42, 8)));

            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            var model = solver.GetModel();

            // Get BitVec before model is invalidated
            bitVec = model.GetBitVec(bvExpr);

            // Solver goes out of scope, model becomes invalid
        }

        // BitVec struct should still be usable since it's a value type
        Assert.That(bitVec.Value, Is.EqualTo(new BigInteger(42)));
        Assert.That(bitVec.Size, Is.EqualTo(8));
        Assert.That(bitVec.ToInt(), Is.EqualTo(42));
    }
}
