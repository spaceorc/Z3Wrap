using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BoundaryChecks;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecBoundaryCheckBuilderTests
{
    private Z3Context context = null!;
    private Z3BitVecBoundaryCheckBuilder builder = null!;

    [SetUp]
    public void SetUp()
    {
        context = new Z3Context();
        builder = context.BitVecBoundaryCheck();
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public void BitVecBoundaryCheck_ReturnsBuilder()
    {
        var result = context.BitVecBoundaryCheck();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BitVecBoundaryCheckBuilder>());
    }

    [Test]
    public void Add_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Sub_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Mul_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Div_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Div(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Neg_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);

        var result = builder.Neg(a);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void AddNoOverflow_UnsignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b).NoOverflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void AddNoOverflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b).NoOverflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void SubNoOverflow_UnsignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b).NoOverflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() for unsigned subtraction
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void SubNoOverflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b).NoOverflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void MulNoOverflow_UnsignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b).NoOverflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void MulNoOverflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b).NoOverflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void DivNoOverflow_UnsignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Div(a, b).NoOverflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() for unsigned division
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void DivNoOverflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Div(a, b).NoOverflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void NegNoOverflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);

        var result = builder.Neg(a).NoOverflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void NegNoOverflow_UnsignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);

        var result = builder.Neg(a).NoOverflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() for unsigned negation (can't overflow)
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void AddNoUnderflow_UnsignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b).NoUnderflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() for unsigned addition
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void AddNoUnderflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b).NoUnderflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void SubNoUnderflow_UnsignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b).NoUnderflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void SubNoUnderflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b).NoUnderflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void MulNoUnderflow_UnsignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b).NoUnderflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() for unsigned multiplication
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void MulNoUnderflow_SignedCreatesValidExpression()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b).NoUnderflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void DivNoUnderflow_ReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var unsignedResult = builder.Div(a, b).NoUnderflow(signed: false);
        var signedResult = builder.Div(a, b).NoUnderflow(signed: true);

        Assert.That(unsignedResult, Is.Not.Null);
        Assert.That(signedResult, Is.Not.Null);

        // Both should be context.True() as division can't underflow
        using var solver = context.CreateSolver();
        solver.Assert(!unsignedResult);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        solver.Reset();
        solver.Assert(!signedResult);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void NegNoUnderflow_SignedReturnsTrue()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);

        var result = builder.Neg(a).NoUnderflow(signed: true);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());
        // Should be context.True() as signed negation can't underflow
        using var solver = context.CreateSolver();
        solver.Assert(!result);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void NegNoUnderflow_UnsignedChecksForZero()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);

        var result = builder.Neg(a).NoUnderflow(signed: false);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<Z3BoolExpr>());

        // Should be true only when a == 0 (unsigned negation of 0 is 0, no underflow)
        using var solver = context.CreateSolver();
        solver.Assert(result);
        solver.Assert(a == 0);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        solver.Reset();
        solver.Assert(result);
        solver.Assert(a == 1);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void FluentChain_CanCombineWithLogicalOperators()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);
        var c = context.BitVecConst("c", 8);

        var addCheck = builder.Add(a, b).NoOverflow();
        var mulCheck = builder.Mul(b, c).NoOverflow();
        var combined = addCheck & mulCheck;

        Assert.That(combined, Is.Not.Null);
        Assert.That(combined, Is.InstanceOf<Z3BoolExpr>());

        // Should be satisfiable - there exist values where both operations don't overflow
        using var solver = context.CreateSolver();
        solver.Assert(combined);
        solver.Assert(a == 1);
        solver.Assert(b == 2);
        solver.Assert(c == 3);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoundaryCheckBuilder_WorksWithSolver()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var noOverflow = builder.Add(a, b).NoOverflow();
        var noUnderflow = builder.Sub(a, b).NoUnderflow();

        using var solver = context.CreateSolver();
        solver.Assert(noOverflow);
        solver.Assert(noUnderflow);
        solver.Assert(a == 100);
        solver.Assert(b == 50);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(a).Value, Is.EqualTo(new BigInteger(100)));
        Assert.That(model.GetBitVec(b).Value, Is.EqualTo(new BigInteger(50)));
    }

    [Test]
    public void Add_WithBigIntegerRight_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = new BigInteger(50);

        var result = builder.Add(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Add_WithBigIntegerLeft_ReturnsOperationBuilder()
    {
        var a = new BigInteger(100);
        var b = context.BitVecConst("b", 8);

        var result = builder.Add(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Sub_WithBigIntegerRight_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = new BigInteger(50);

        var result = builder.Sub(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Sub_WithBigIntegerLeft_ReturnsOperationBuilder()
    {
        var a = new BigInteger(100);
        var b = context.BitVecConst("b", 8);

        var result = builder.Sub(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Mul_WithBigIntegerRight_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = new BigInteger(3);

        var result = builder.Mul(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Mul_WithBigIntegerLeft_ReturnsOperationBuilder()
    {
        var a = new BigInteger(5);
        var b = context.BitVecConst("b", 8);

        var result = builder.Mul(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Div_WithBigIntegerRight_ReturnsOperationBuilder()
    {
        var a = context.BitVecConst("a", 8);
        var b = new BigInteger(2);

        var result = builder.Div(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void Div_WithBigIntegerLeft_ReturnsOperationBuilder()
    {
        var a = new BigInteger(100);
        var b = context.BitVecConst("b", 8);

        var result = builder.Div(a, b);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<BitVecOperationBuilder>());
    }

    [Test]
    public void BigIntegerOverloads_WorkWithSolver()
    {
        using var scope = context.SetUp();
        var x = context.BitVecConst("x", 8);

        var addCheck = builder.Add(x, new BigInteger(10)).NoOverflow();
        var subCheck = builder.Sub(new BigInteger(200), x).NoUnderflow();
        var mulCheck = builder.Mul(x, new BigInteger(2)).NoOverflow();
        var divCheck = builder.Div(new BigInteger(100), x).NoOverflow(signed: true);

        using var solver = context.CreateSolver();
        solver.Assert(addCheck);
        solver.Assert(subCheck);
        solver.Assert(mulCheck);
        solver.Assert(divCheck);
        solver.Assert(x == 5);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBitVec(x).Value, Is.EqualTo(new BigInteger(5)));
    }

    [Test]
    public void Overflow_ReturnsNegationOfNoOverflow()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var noOverflow = builder.Add(a, b).NoOverflow();
        var overflow = builder.Add(a, b).Overflow();

        Assert.That(overflow, Is.Not.Null);
        Assert.That(overflow, Is.InstanceOf<Z3BoolExpr>());

        // Verify that overflow is logically equivalent to !noOverflow
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test that overflow == !noOverflow
        solver.Assert(overflow != (!noOverflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Underflow_ReturnsNegationOfNoUnderflow()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var noUnderflow = builder.Sub(a, b).NoUnderflow();
        var underflow = builder.Sub(a, b).Underflow();

        Assert.That(underflow, Is.Not.Null);
        Assert.That(underflow, Is.InstanceOf<Z3BoolExpr>());

        // Verify that underflow is logically equivalent to !noUnderflow
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test that underflow == !noUnderflow
        solver.Assert(underflow != (!noUnderflow));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void OverflowAndUnderflow_WorkWithSolver()
    {
        using var scope = context.SetUp();
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        // Test overflow condition
        var addOverflow = builder.Add(a, b).Overflow();

        using var solver = context.CreateSolver();
        solver.Assert(addOverflow);
        solver.Assert(a == 200);
        solver.Assert(b == 100);

        // This should be satisfiable as 200 + 100 = 300 which overflows 8-bit unsigned (max 255)
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Overflow_WithSignedParameter_WorksCorrectly()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var unsignedOverflow = builder.Add(a, b).Overflow(signed: false);
        var signedOverflow = builder.Add(a, b).Overflow(signed: true);

        Assert.That(unsignedOverflow, Is.Not.Null);
        Assert.That(signedOverflow, Is.Not.Null);
        Assert.That(unsignedOverflow, Is.InstanceOf<Z3BoolExpr>());
        Assert.That(signedOverflow, Is.InstanceOf<Z3BoolExpr>());
    }

    [Test]
    public void Underflow_WithSignedParameter_WorksCorrectly()
    {
        var a = context.BitVecConst("a", 8);
        var b = context.BitVecConst("b", 8);

        var unsignedUnderflow = builder.Sub(a, b).Underflow(signed: false);
        var signedUnderflow = builder.Sub(a, b).Underflow(signed: true);

        Assert.That(unsignedUnderflow, Is.Not.Null);
        Assert.That(signedUnderflow, Is.Not.Null);
        Assert.That(unsignedUnderflow, Is.InstanceOf<Z3BoolExpr>());
        Assert.That(signedUnderflow, Is.InstanceOf<Z3BoolExpr>());
    }
}