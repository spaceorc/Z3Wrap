using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class BitVecOverflowChecksTests
{
    [Test]
    public void AddNoOverflow_UnsignedMode_DetectsOverflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var overflowCheck = context.AddNoOverflow(x, y, isSigned: false);

        Assert.That(overflowCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(overflowCheck.Context, Is.SameAs(context));

        // Test case: 255 + 1 should overflow in unsigned 8-bit arithmetic
        var maxVal = context.BitVec(new BitVec(255, 8)); // Max unsigned 8-bit value
        var one = context.BitVec(new BitVec(1, 8));
        var wouldOverflow = context.AddNoOverflow(maxVal, one, isSigned: false);

        // The overflow check should return false (meaning it WOULD overflow)
        solver.Assert(context.Not(wouldOverflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: 100 + 50 should NOT overflow in unsigned 8-bit arithmetic
        solver.Reset();
        var val1 = context.BitVec(new BitVec(100, 8));
        var val2 = context.BitVec(new BitVec(50, 8));
        var wouldNotOverflow = context.AddNoOverflow(val1, val2, isSigned: false);

        // The overflow check should return true (meaning it would NOT overflow)
        solver.Assert(wouldNotOverflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void AddNoOverflow_SignedMode_DetectsOverflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        // Test case: 127 + 1 should overflow in signed 8-bit arithmetic
        var maxSigned = context.BitVec(new BitVec(127, 8)); // Max signed 8-bit value
        var one = context.BitVec(new BitVec(1, 8));
        var wouldOverflow = !context.AddNoOverflow(maxSigned, one, isSigned: true);

        // The overflow check should return false (meaning it WOULD overflow)
        solver.Assert(wouldOverflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: 60 + 60 should NOT overflow in signed 8-bit arithmetic
        solver.Reset();
        var val1 = context.BitVec(new BitVec(60, 8));
        var val2 = context.BitVec(new BitVec(60, 8));
        var wouldNotOverflow = context.AddNoOverflow(val1, val2, isSigned: true);

        // The overflow check should return true (meaning it would NOT overflow)
        solver.Assert(wouldNotOverflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubNoOverflow_DetectsOverflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var overflowCheck = context.SubNoOverflow(x, y);

        Assert.That(overflowCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(overflowCheck.Context, Is.SameAs(context));

        // Test case: 10 - 5 should NOT overflow in 8-bit arithmetic
        var val1 = context.BitVec(new BitVec(10, 8));
        var val2 = context.BitVec(new BitVec(5, 8));
        var wouldNotOverflow = context.SubNoOverflow(val1, val2);

        // The overflow check should return true (meaning it would NOT overflow)
        solver.Assert(wouldNotOverflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubNoUnderflow_SignedMode_DetectsUnderflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        // Test case: -128 - 1 should underflow in signed 8-bit arithmetic
        var minSigned = context.BitVec(new BitVec(128, 8)); // -128 in 8-bit two's complement
        var one = context.BitVec(new BitVec(1, 8));
        var wouldUnderflow = context.SubNoUnderflow(minSigned, one, isSigned: true);

        // The underflow check should return false (meaning it WOULD underflow)
        solver.Assert(context.Not(wouldUnderflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: -100 - 20 should NOT underflow in signed 8-bit arithmetic
        solver.Reset();
        var val1 = context.BitVec(new BitVec(156, 8)); // -100 in 8-bit two's complement
        var val2 = context.BitVec(new BitVec(20, 8));
        var wouldNotUnderflow = context.SubNoUnderflow(val1, val2, isSigned: true);

        // The underflow check should return true (meaning it would NOT underflow)
        solver.Assert(wouldNotUnderflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulNoOverflow_UnsignedMode_DetectsOverflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        // Test case: 16 * 16 = 256 should overflow in unsigned 8-bit arithmetic
        var val1 = context.BitVec(new BitVec(16, 8));
        var val2 = context.BitVec(new BitVec(16, 8));
        var wouldOverflow = context.MulNoOverflow(val1, val2, isSigned: false);

        // The overflow check should return false (meaning it WOULD overflow)
        solver.Assert(context.Not(wouldOverflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test case: 10 * 10 = 100 should NOT overflow in unsigned 8-bit arithmetic
        solver.Reset();
        val1 = context.BitVec(new BitVec(10, 8));
        val2 = context.BitVec(new BitVec(10, 8));
        var wouldNotOverflow = context.MulNoOverflow(val1, val2, isSigned: false);

        // The overflow check should return true (meaning it would NOT overflow)
        solver.Assert(wouldNotOverflow);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulNoOverflow_SignedMode_DetectsOverflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        // Test case: 12 * 12 = 144 should overflow in signed 8-bit arithmetic (max is 127)
        var val1 = context.BitVec(new BitVec(12, 8));
        var val2 = context.BitVec(new BitVec(12, 8));
        var wouldOverflow = context.MulNoOverflow(val1, val2, isSigned: true);

        // The overflow check should return false (meaning it WOULD overflow)
        solver.Assert(context.Not(wouldOverflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulNoUnderflow_DetectsUnderflow()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);
        var underflowCheck = context.MulNoUnderflow(x, y);

        Assert.That(underflowCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(underflowCheck.Context, Is.SameAs(context));

        // Test case: -128 * 2 should underflow in signed 8-bit arithmetic
        var minSigned = context.BitVec(new BitVec(128, 8)); // -128 in 8-bit two's complement
        var two = context.BitVec(new BitVec(2, 8));
        var wouldUnderflow = context.MulNoUnderflow(minSigned, two);

        // The underflow check should return false (meaning it WOULD underflow)
        solver.Assert(context.Not(wouldUnderflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void OverflowChecks_SizeMismatch_ThrowsException()
    {
        using var context = new Z3Context();

        var x8 = context.BitVecConst("x", 8);
        var y16 = context.BitVecConst("y", 16);

        Assert.Throws<ArgumentException>(() => context.AddNoOverflow(x8, y16));
        Assert.Throws<ArgumentException>(() => context.SubNoOverflow(x8, y16));
        Assert.Throws<ArgumentException>(() => context.SubNoUnderflow(x8, y16));
        Assert.Throws<ArgumentException>(() => context.MulNoOverflow(x8, y16));
        Assert.Throws<ArgumentException>(() => context.MulNoUnderflow(x8, y16));
    }

    [Test]
    public void OverflowChecks_WithConstraints_ProvidesBounds()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        // Use overflow checks to constrain values
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        // Constrain that x + y doesn't overflow (unsigned)
        solver.Assert(context.AddNoOverflow(x, y, isSigned: false));

        // Also constrain x > 200 and y > 200
        solver.Assert(context.Gt(x, context.BitVec(new BitVec(200, 8))));
        solver.Assert(context.Gt(y, context.BitVec(new BitVec(200, 8))));

        // This should be unsatisfiable because both > 200 would cause overflow
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void AddNoOverflow_BigIntegerOverloads_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);

        // Test Z3BitVecExpr + BigInteger
        var check1 = context.AddNoOverflow(x, 100, isSigned: false);
        Assert.That(check1.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test BigInteger + Z3BitVecExpr
        var check2 = context.AddNoOverflow(100, x, isSigned: false);
        Assert.That(check2.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test practical constraint: x = 200, x + 100 should overflow
        solver.Assert(context.Eq(x, context.BitVec(new BitVec(200, 8))));
        solver.Assert(context.Not(context.AddNoOverflow(x, 100, isSigned: false)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MulNoOverflow_BigIntegerOverloads_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);

        // Test Z3BitVecExpr * BigInteger
        var check1 = context.MulNoOverflow(x, 20, isSigned: false);
        Assert.That(check1.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test BigInteger * Z3BitVecExpr
        var check2 = context.MulNoOverflow(20, x, isSigned: false);
        Assert.That(check2.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test practical constraint: x = 20, x * 20 = 400 should overflow in 8-bit
        solver.Assert(context.Eq(x, context.BitVec(new BitVec(20, 8))));
        solver.Assert(context.Not(context.MulNoOverflow(x, 20, isSigned: false)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SubNoOverflow_BigIntegerOverloads_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);

        // Test Z3BitVecExpr - BigInteger
        var check1 = context.SubNoOverflow(x, 50);
        Assert.That(check1.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test BigInteger - Z3BitVecExpr
        var check2 = context.SubNoOverflow(50, x);
        Assert.That(check2.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test practical constraint: x = 100, x - 50 should not overflow
        solver.Assert(context.Eq(x, context.BitVec(new BitVec(100, 8))));
        solver.Assert(context.SubNoOverflow(x, 50));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_OverflowChecks_WorkCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        // Test fluent API for overflow checks
        var addCheck = x.AddNoOverflow(y, isSigned: false);
        var subCheck = x.SubNoOverflow(y);
        var mulCheck = x.MulNoOverflow(y, isSigned: false);

        Assert.That(addCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(subCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mulCheck.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test fluent API with BigInteger
        var addCheckBig = x.AddNoOverflow(100, isSigned: false);
        var subCheckBig = x.SubNoOverflow(50);
        var mulCheckBig = x.MulNoOverflow(10, isSigned: false);

        Assert.That(addCheckBig.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(subCheckBig.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mulCheckBig.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Test practical constraint: x = 200, x + 100 should overflow
        solver.Assert(context.Eq(x, context.BitVec(new BitVec(200, 8))));
        solver.Assert(context.Not(x.AddNoOverflow(100, isSigned: false)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_Repeat_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVec(new BitVec(5, 4)); // 0101 in binary
        var repeated = x.Repeat(2);

        Assert.That(repeated.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(repeated.Size, Is.EqualTo(8)); // 4 * 2 = 8 bits

        // 0101 repeated twice should be 01010101 = 85 in decimal
        solver.Assert(context.Eq(repeated, context.BitVec(new BitVec(85, 8))));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}