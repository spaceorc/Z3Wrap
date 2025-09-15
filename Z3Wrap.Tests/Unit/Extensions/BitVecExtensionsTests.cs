using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class BitVecExtensionsTests
{
    [Test]
    public void Add_BitVecExprs_CreatesAdditionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var sum = context.Add(x, y);

        Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(sum.Context, Is.SameAs(context));
        Assert.That(sum.Size, Is.EqualTo(8));

        // Test 5 + 3 == 8 in 8-bit arithmetic
        var result = context.Add(context.BitVec(new BitVec(5, 8)), context.BitVec(new BitVec(3, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(8, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Sub_BitVecExprs_CreatesSubtractionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var difference = context.Sub(x, y);

        Assert.That(difference.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(difference.Context, Is.SameAs(context));
        Assert.That(difference.Size, Is.EqualTo(8));

        // Test 10 - 4 == 6 in 8-bit arithmetic
        var result = context.Sub(context.BitVec(new BitVec(10, 8)), context.BitVec(new BitVec(4, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(6, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mul_BitVecExprs_CreatesMultiplicationExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var product = context.Mul(x, y);

        Assert.That(product.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(product.Context, Is.SameAs(context));
        Assert.That(product.Size, Is.EqualTo(8));

        // Test 3 * 4 == 12 in 8-bit arithmetic
        var result = context.Mul(context.BitVec(new BitVec(3, 8)), context.BitVec(new BitVec(4, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(12, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Div_BitVecExprs_CreatesUnsignedDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var quotient = context.Div(x, y);

        Assert.That(quotient.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quotient.Context, Is.SameAs(context));
        Assert.That(quotient.Size, Is.EqualTo(8));

        // Test 15 / 3 == 5 in 8-bit arithmetic
        var result = context.Div(context.BitVec(new BitVec(15, 8)), context.BitVec(new BitVec(3, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(5, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedDiv_BitVecExprs_CreatesSignedDivisionExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var quotient = context.SignedDiv(x, y);

        Assert.That(quotient.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quotient.Context, Is.SameAs(context));
        Assert.That(quotient.Size, Is.EqualTo(8));
    }

    [Test]
    public void And_BitVecExprs_CreatesBitwiseAndExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var andResult = context.And(x, y);

        Assert.That(andResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(andResult.Context, Is.SameAs(context));
        Assert.That(andResult.Size, Is.EqualTo(8));

        // Test 0b1100 & 0b1010 == 0b1000 (12 & 10 == 8)
        var result = context.And(context.BitVec(new BitVec(12, 8)), context.BitVec(new BitVec(10, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(8, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Or_BitVecExprs_CreatesBitwiseOrExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var orResult = context.Or(x, y);

        Assert.That(orResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(orResult.Context, Is.SameAs(context));
        Assert.That(orResult.Size, Is.EqualTo(8));

        // Test 0b1100 | 0b1010 == 0b1110 (12 | 10 == 14)
        var result = context.Or(context.BitVec(new BitVec(12, 8)), context.BitVec(new BitVec(10, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(14, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Xor_BitVecExprs_CreatesBitwiseXorExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var xorResult = context.Xor(x, y);

        Assert.That(xorResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(xorResult.Context, Is.SameAs(context));
        Assert.That(xorResult.Size, Is.EqualTo(8));

        // Test 0b1100 ^ 0b1010 == 0b0110 (12 ^ 10 == 6)
        var result = context.Xor(context.BitVec(new BitVec(12, 8)), context.BitVec(new BitVec(10, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(6, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Not_BitVecExpr_CreatesBitwiseNotExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var notResult = context.Not(x);

        Assert.That(notResult.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(notResult.Context, Is.SameAs(context));
        Assert.That(notResult.Size, Is.EqualTo(8));

        // Test ~0b00001111 == 0b11110000 in 8-bit (~15 == 240)
        var result = context.Not(context.BitVec(new BitVec(15, 8)));
        solver.Assert(context.Eq(result, context.BitVec(new BitVec(240, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }


    [Test]
    public void BitVecOperations_SizeMismatch_ThrowsException()
    {
        using var context = new Z3Context();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 16);

        Assert.Throws<ArgumentException>(() => context.Add(x, y));
        Assert.Throws<ArgumentException>(() => context.Sub(x, y));
        Assert.Throws<ArgumentException>(() => context.Mul(x, y));
        Assert.Throws<ArgumentException>(() => context.And(x, y));
    }

}