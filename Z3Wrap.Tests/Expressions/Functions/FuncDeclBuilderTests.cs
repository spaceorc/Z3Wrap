using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Functions;

/// <summary>
/// Tests for dynamic function builder API (FuncBuilder).
/// Validates builder pattern for creating functions with arbitrary arity.
/// </summary>
[TestFixture]
public class FuncDeclBuilderTests
{
    [Test]
    public void BuildFunc_WithNoArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.FuncBuilder<IntExpr>("f").Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(0));
        });
    }

    [Test]
    public void BuildFunc_WithOneArg_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.FuncBuilder<IntExpr>("f").WithArg<IntExpr>().Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(1));
        });
    }

    [Test]
    public void BuildFunc_WithTwoArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.FuncBuilder<IntExpr>("f").WithArg<IntExpr>().WithArg<IntExpr>().Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(2));
        });
    }

    [Test]
    public void BuildFunc_WithThreeArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(3));
        });
    }

    [Test]
    public void BuildFunc_WithFourArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(4));
        });
    }

    [Test]
    public void BuildFunc_WithFiveArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(5));
        });
    }

    [Test]
    public void BuildFunc_IntToReal_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.FuncBuilder<RealExpr>("f").WithArg<IntExpr>().Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(1));
        });
    }

    [Test]
    public void BuildFunc_WithMixedTypes_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context
            .FuncBuilder<BoolExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<BoolExpr>()
            .WithArg<RealExpr>()
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(3));
        });
    }

    [Test]
    public void BuildFunc_ApplyWithNoArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.FuncBuilder<IntExpr>("f").Build();
        var result = func.Apply();

        solver.Assert(result == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void BuildFunc_ApplyWithOneArg_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context.FuncBuilder<IntExpr>("f").WithArg<IntExpr>().Build();
        var x = context.Int(5);
        var result = func.Apply([x]);

        solver.Assert(result == 10);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void BuildFunc_ApplyWithTwoArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();
        var x = context.Int(3);
        var y = context.Int(7);
        var result = func.Apply([x, y]);

        solver.Assert(result == 21);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(21)));
    }

    [Test]
    public void BuildFunc_ApplyWithThreeArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();
        var x = context.Int(2);
        var y = context.Int(3);
        var z = context.Int(4);
        var result = func.Apply([x, y, z]);

        solver.Assert(result == 24);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(24)));
    }

    [Test]
    public void BuildFunc_ApplyWithFourArgs_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context
            .FuncBuilder<IntExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();
        var a = context.Int(2);
        var b = context.Int(3);
        var c = context.Int(4);
        var d = context.Int(5);
        var result = func.Apply([a, b, c, d]);

        solver.Assert(result == 120);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(120)));
    }

    [Test]
    public void BuildFunc_ApplyWithMixedTypes_ProducesValidExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var func = context
            .FuncBuilder<BoolExpr>("f")
            .WithArg<IntExpr>()
            .WithArg<BoolExpr>()
            .WithArg<RealExpr>()
            .Build();
        var x = context.Int(5);
        var y = context.Bool(true);
        var z = context.Real(2.5m);
        var result = func.Apply([x, y, z]);

        solver.Assert(result);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void BuildFunc_ApplyWrongArgCount_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var func = context.FuncBuilder<IntExpr>("f").WithArg<IntExpr>().Build();
        var x = context.Int(5);
        var y = context.Int(7);

        Assert.Throws<ArgumentException>(() => func.Apply([x, y]));
    }

    [Test]
    public void BuildFunc_ApplyWithNoArgsToOneArgFunc_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var func = context.FuncBuilder<IntExpr>("f").WithArg<IntExpr>().Build();

        Assert.Throws<ArgumentException>(() => func.Apply([]));
    }

    [Test]
    public void BuildFunc_EquivalentToStaticFunc_ProducesSameResults()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var funcStatic = context.Func<IntExpr, IntExpr, IntExpr>("f");
        var funcBuilder = context
            .FuncBuilder<IntExpr>("g")
            .WithArg<IntExpr>()
            .WithArg<IntExpr>()
            .Build();

        var x = context.Int(3);
        var y = context.Int(7);

        var resultStatic = funcStatic.Apply(x, y);
        var resultBuilder = funcBuilder.Apply([x, y]);

        solver.Assert(resultStatic == 21);
        solver.Assert(resultBuilder == 21);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(resultStatic), Is.EqualTo(new BigInteger(21)));
            Assert.That(model.GetIntValue(resultBuilder), Is.EqualTo(new BigInteger(21)));
        });
    }
}
