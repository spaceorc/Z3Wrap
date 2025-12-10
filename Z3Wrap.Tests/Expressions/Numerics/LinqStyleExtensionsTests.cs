using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

/// <summary>
/// Tests for LINQ-style arithmetic aggregation methods (Sum, Product).
/// </summary>
[TestFixture]
public class LinqStyleExtensionsTests
{
    [Test]
    public void Sum_IntExprs_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(10);
        var b = context.Int(20);
        var c = context.Int(12);

        var exprs = new[] { a, b, c };
        var result = exprs.Sum();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Sum_IntExprs_EmptySequence_ReturnsZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var exprs = Array.Empty<IntExpr>();
        var result = exprs.Sum();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void Sum_RealExprs_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(10.5m);
        var b = context.Real(20.5m);
        var c = context.Real(11.0m);

        var exprs = new[] { a, b, c };
        var result = exprs.Sum();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result).ToDecimal(), Is.EqualTo(42.0m));
    }

    [Test]
    public void Sum_WithSelector_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var items = new[]
        {
            (Value: 10, Expr: context.Int(10)),
            (Value: 20, Expr: context.Int(20)),
            (Value: 12, Expr: context.Int(12)),
        };

        var result = items.Sum(x => x.Expr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Product_IntExprs_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(2);
        var b = context.Int(3);
        var c = context.Int(7);

        var exprs = new[] { a, b, c };
        var result = exprs.Product();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Product_IntExprs_EmptySequence_ReturnsOne()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var exprs = Array.Empty<IntExpr>();
        var result = exprs.Product();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void Product_WithSelector_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var items = new[]
        {
            (Value: 2, Expr: context.Int(2)),
            (Value: 3, Expr: context.Int(3)),
            (Value: 7, Expr: context.Int(7)),
        };

        var result = items.Product(x => x.Expr);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }
}
