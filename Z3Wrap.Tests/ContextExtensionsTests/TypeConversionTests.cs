using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.ContextExtensionsTests;

[TestFixture]
public class TypeConversionTests
{
    [Test]
    public void ToReal_IntExpr_ReturnsRealExpr()
    {
        using var context = new Z3Context();
        
        var intExpr = context.IntConst("x");
        var realExpr = context.ToReal(intExpr);

        Assert.That(realExpr, Is.InstanceOf<Z3RealExpr>());
        Assert.That(realExpr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ToInt_RealExpr_ReturnsIntExpr()
    {
        using var context = new Z3Context();
        
        var realExpr = context.RealConst("y");
        var intExpr = context.ToInt(realExpr);

        Assert.That(intExpr, Is.InstanceOf<Z3IntExpr>());
        Assert.That(intExpr.Context, Is.EqualTo(context));
    }

    [Test]
    public void IntExprToReal_EquivalentToContextMethod()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var result = x.ToReal();

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
        Assert.That(result, Is.InstanceOf<Z3RealExpr>());

        // Test equivalent to context extension method
        var contextResult = context.ToReal(x);
        solver.Assert(context.Eq(result, contextResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test that int 42 becomes real 42
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(42)));
        solver.Assert(context.Eq(result, context.Real(42)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void RealExprToInt_EquivalentToContextMethod()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.RealConst("x");
        var result = x.ToInt();

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
        Assert.That(result, Is.InstanceOf<Z3IntExpr>());

        // Test equivalent to context extension method
        var contextResult = context.ToInt(x);
        solver.Assert(context.Eq(result, contextResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test that real 17.0 becomes int 17
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(17)));
        solver.Assert(context.Eq(result, context.Int(17)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        // Test truncation: real 17.9 becomes int 17
        solver.Reset();
        solver.Assert(context.Eq(x, context.Real(17.9m)));
        solver.Assert(context.Eq(result, context.Int(17)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToReal_MixedArithmetic_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.RealConst("y");

        solver.Assert(context.Add(context.ToReal(x), y).Eq(context.Real(5.5m)));
        solver.Assert(x.Eq(context.Int(3)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetNumericValueAsString(y);

        Assert.That(xValue, Is.EqualTo(BigInteger.Parse("3")));
        Assert.That(yValue, Is.EqualTo("5/2")); // 5.5 - 3 = 2.5 = 5/2
    }

    [Test]
    public void ToInt_MixedArithmetic_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.IntConst("y");

        solver.Assert(context.Add(context.ToInt(x), y).Eq(context.Int(10)));
        solver.Assert(x.Eq(context.Real(7)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetNumericValueAsString(x);
        var yValue = model.GetIntValue(y);

        Assert.That(xValue, Is.EqualTo("7"));
        Assert.That(yValue, Is.EqualTo(BigInteger.Parse("3"))); // 10 - 7 = 3
    }

    [Test]
    public void ToReal_ChainedOperations_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.RealConst("z");

        solver.Assert(context.Add(context.ToReal(x), context.Add(context.ToReal(y), z)).Eq(context.Real(15)));
        solver.Assert(x.Eq(context.Int(5)));
        solver.Assert(y.Eq(context.Int(4)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var zValue = model.GetNumericValueAsString(z);

        Assert.That(zValue, Is.EqualTo("6")); // 15 - 5 - 4 = 6
    }

    [Test]
    public void ToInt_WithRealArithmetic_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.IntConst("y");

        solver.Assert(x.Eq(context.Real(new Real(7, 2)))); // 3.5
        solver.Assert(context.ToInt(x).Eq(context.Int(3))); // Should truncate to 3
        solver.Assert(y.Eq(context.Add(context.ToInt(x), context.Int(2))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var yValue = model.GetIntValue(y);

        Assert.That(yValue, Is.EqualTo(BigInteger.Parse("5"))); // 3 + 2 = 5
    }

    [Test]
    public void ToReal_WithComparison_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.RealConst("y");

        solver.Assert(context.Gt(context.ToReal(x), y));
        solver.Assert(x.Eq(context.Int(10)));
        solver.Assert(y.Eq(context.Real(9.5m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetNumericValueAsString(y);

        Assert.That(xValue, Is.EqualTo(BigInteger.Parse("10")));
        Assert.That(yValue, Is.EqualTo("19/2")); // 9.5 = 19/2
    }

    [Test]
    public void ToInt_WithModulo_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var result = context.IntConst("result");

        solver.Assert(x.Eq(context.Real(17)));
        solver.Assert(result.Eq(context.Mod(context.ToInt(x), context.Int(5))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var resultValue = model.GetIntValue(result);

        Assert.That(resultValue, Is.EqualTo(BigInteger.Parse("2"))); // 17 % 5 = 2
    }

    [Test]
    public void TypeConversions_BothDirections_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.RealConst("y");

        solver.Assert(x.Eq(context.Int(42)));
        solver.Assert(y.Eq(context.Add(context.ToReal(x), context.Real(0.5m))));
        solver.Assert(context.ToInt(y).Eq(x)); // Should convert back to original value

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetNumericValueAsString(y);

        Assert.That(xValue, Is.EqualTo(BigInteger.Parse("42")));
        Assert.That(yValue, Is.EqualTo("85/2")); // 42.5 = 85/2
    }

    [Test]
    public void ToInt_FractionalValue_UnsatisfiableConstraint()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");

        solver.Assert(x.Eq(context.Real(2.7m)));
        solver.Assert(context.ToInt(x).Eq(context.Int(3))); // Real 2.7 truncated should be 2, not 3

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void IntExprToBitVec_EquivalentToContextMethod()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var result = x.ToBitVec(32);

        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
        Assert.That(result, Is.InstanceOf<Z3BitVecExpr>());
        Assert.That(result.Size, Is.EqualTo(32));

        // Test equivalent to context extension method
        var contextResult = context.ToBitVec(x, 32);
        solver.Assert(context.Eq(result, contextResult));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Test that int 42 becomes 32-bit bitvector 42
        solver.Reset();
        solver.Assert(context.Eq(x, context.Int(42)));
        solver.Assert(context.Eq(result, context.BitVec(42, 32)));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var resultValue = model.GetBitVec(result);

        Assert.That(xValue, Is.EqualTo(new BigInteger(42)));
        Assert.That(resultValue.Value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void IntExprToBitVec_WithLargeValue_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var bv = x.ToBitVec(64);

        // Test with a large value that fits in 64 bits
        var largeValue = BigInteger.Parse("1234567890123456789");
        solver.Assert(context.Eq(x, context.Int(largeValue)));
        solver.Assert(context.Eq(bv, context.BitVec(largeValue, 64)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var bvValue = model.GetBitVec(bv);

        Assert.That(xValue, Is.EqualTo(largeValue));
        Assert.That(bvValue.Value, Is.EqualTo(largeValue));
    }

    [Test]
    public void IntExprToBitVec_WithNegativeValue_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var bv = x.ToBitVec(8);

        // Test with negative value that becomes positive in bitvector representation
        solver.Assert(context.Eq(x, context.Int(-1)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var bvValue = model.GetBitVec(bv);

        Assert.That(xValue, Is.EqualTo(new BigInteger(-1)));
        Assert.That(bvValue.Value, Is.EqualTo(new BigInteger(255))); // -1 in 8-bit two's complement is 255
    }
}