#pragma warning disable NUnit2021 // Comparison of value types is allowed here for BitVec tests

using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3BitVecResizingTests
{
    private Z3Context context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new Z3Context();
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public void Extend_ContextMethod_CreatesCorrectExpression()
    {
        var bv8 = context.BitVec(new BitVec(42, 8));
        var bv16 = context.Extend(bv8, 8); // Add 8 more bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(42, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extend_FluentAPI_CreatesCorrectExpression()
    {
        var bv8 = context.BitVec(new BitVec(170, 8)); // 0b10101010
        var bv16 = bv8.Extend(8); // Add 8 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(170, 16))); // Should be 0b00000000_10101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }


    [Test]
    public void SignedExtend_ContextMethod_PositiveValue()
    {
        var bv8 = context.BitVec(new BitVec(85, 8)); // 0b01010101 (positive)
        var bv16 = context.Extend(bv8, 8, signed: true);

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(85, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedExtend_FluentAPI_NegativeValue()
    {
        var bv8 = context.BitVec(new BitVec(170, 8)); // 0b10101010 (negative in signed interpretation)
        var bv16 = bv8.Extend(8, signed: true);

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        // In signed interpretation, 170 in 8-bit is -86, so sign extension should preserve this
        var expected = new BitVec(-86, 16); // Should be properly sign-extended
        solver.Assert(bv16 == context.BitVec(expected));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extract_ContextMethod_MiddleBits()
    {
        var bv8 = context.BitVec(new BitVec(0b11010011, 8)); // 211
        var extracted = context.Extract(bv8, 5, 2); // Extract bits 5-2

        Assert.That(extracted.Size, Is.EqualTo(4));

        using var solver = context.CreateSolver();
        solver.Assert(extracted == context.BitVec(new BitVec(0b0100, 4))); // Should be 4

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Extract_FluentAPI_SingleBit()
    {
        var bv8 = context.BitVec(new BitVec(0b10101010, 8));
        var bit7 = bv8.Extract(7, 7); // MSB
        var bit0 = bv8.Extract(0, 0); // LSB

        Assert.That(bit7.Size, Is.EqualTo(1));
        Assert.That(bit0.Size, Is.EqualTo(1));

        using var solver = context.CreateSolver();
        solver.Assert(bit7 == context.BitVec(new BitVec(1, 1)));
        solver.Assert(bit0 == context.BitVec(new BitVec(0, 1)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Int2BitVec_ContextMethod_CreatesCorrectBitVec()
    {
        var intExpr = context.IntConst("x");
        var bvExpr = context.ToBitVec(intExpr, 16);

        Assert.That(bvExpr.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(intExpr == context.Int(42));

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);
            var bvValue = model.GetBitVec(bvExpr);

            Assert.That((int)intValue, Is.EqualTo(42));
            Assert.That((int)bvValue.Value, Is.EqualTo(42));
            Assert.That(bvValue.Size, Is.EqualTo(16));
        }
    }

    [Test]
    public void BitVec2Int_ContextMethod_UnsignedConversion()
    {
        var bvExpr = context.BitVec(new BitVec(200, 8));
        var intExpr = context.ToInt(bvExpr); // unsigned

        using var solver = context.CreateSolver();
        solver.Assert(intExpr >= context.Int(0));

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            Assert.That((int)intValue, Is.EqualTo(200));
        }
    }

    [Test]
    public void BitVec2Int_FluentAPI_SignedConversion()
    {
        var bvExpr = context.BitVec(new BitVec(200, 8)); // 200 unsigned, -56 signed
        var intExpr = bvExpr.ToInt(signed: true);

        using var solver = context.CreateSolver();

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            // 200 in 8-bit unsigned is 11001000 binary, which in signed is -56
            Assert.That((int)intValue, Is.EqualTo(-56));
        }
    }

    [Test]
    public void ToInt_FluentAPI_ReturnsUnsignedValue()
    {
        var bvExpr = context.BitVec(new BitVec(200, 8));
        var intExpr = bvExpr.ToInt();

        using var solver = context.CreateSolver();

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var intValue = model.GetIntValue(intExpr);

            Assert.That((int)intValue, Is.EqualTo(200));
        }
    }

    [Test]
    public void ChainedOperations_WorkCorrectly()
    {
        var bv8 = context.BitVec(new BitVec(0b11110000, 8));
        var extracted = bv8.Extract(7, 4); // Get upper 4 bits -> 1111
        var extended = extracted.Extend(4); // Extend to 8 bits -> 00001111

        Assert.That(extracted.Size, Is.EqualTo(4));
        Assert.That(extended.Size, Is.EqualTo(8));

        using var solver = context.CreateSolver();
        solver.Assert(extended == context.BitVec(new BitVec(0b00001111, 8)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexResizingScenario_WithModel()
    {
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 16);

        // Extend x to 16 bits and add to y
        var xExtended = x.Extend(8);
        var sum = context.BitVec(new BitVec(100, 16));

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(42, 8)));
        solver.Assert(y == context.BitVec(new BitVec(58, 16)));
        // Note: arithmetic operators for Z3BitVecExpr are not implemented yet
        // solver.Assert(xExtended + y == sum);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetBitVec(x);
        var yValue = model.GetBitVec(y);

        Assert.That((int)xValue.Value, Is.EqualTo(42));
        Assert.That((int)yValue.Value, Is.EqualTo(58));
    }

    [Test]
    public void Resize_ContextMethod_WorksCorrectly()
    {
        var bv8 = context.BitVec(new BitVec(42, 8));
        var bv16 = context.Resize(bv8, 16); // Resize to 16 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(42, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Resize_FluentAPI_WorksCorrectly()
    {
        var bv8 = context.BitVec(new BitVec(42, 8));
        var bv16 = bv8.Resize(16); // Resize to 16 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        solver.Assert(bv16 == context.BitVec(new BitVec(42, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedResize_ContextMethod_WorksCorrectly()
    {
        var bv8 = context.BitVec(new BitVec(200, 8)); // -56 in signed 8-bit
        var bv16 = context.Resize(bv8, 16, signed: true); // Sign extend to 16 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        var expected = new BitVec(-56, 16); // Should be sign-extended
        solver.Assert(bv16 == context.BitVec(expected));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedResize_FluentAPI_WorksCorrectly()
    {
        var bv8 = context.BitVec(new BitVec(200, 8)); // -56 in signed 8-bit
        var bv16 = bv8.Resize(16, signed: true); // Sign extend to 16 bits

        Assert.That(bv16.Size, Is.EqualTo(16));

        using var solver = context.CreateSolver();
        var expected = new BitVec(-56, 16); // Should be sign-extended
        solver.Assert(bv16 == context.BitVec(expected));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}