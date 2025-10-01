using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Functions;

/// <summary>
/// Tests for function declaration creation methods.
/// Validates function creation APIs, property correctness, and type combinations.
/// </summary>
[TestFixture]
public class FuncDeclFactoryTests
{
    [Test]
    public void CreateFunc_WithNoArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(0));
        });
    }

    [Test]
    public void CreateFunc_WithOneArg_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr, IntExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(1));
        });
    }

    [Test]
    public void CreateFunc_WithTwoArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr, IntExpr, IntExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(2));
        });
    }

    [Test]
    public void CreateFunc_WithThreeArgs_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr, IntExpr, IntExpr, IntExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(3));
        });
    }

    [Test]
    public void CreateFunc_IntToReal_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr, RealExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(1));
        });
    }

    [Test]
    public void CreateFunc_RealIntToBool_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<RealExpr, IntExpr, BoolExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(2));
        });
    }

    [Test]
    public void CreateFunc_IntBoolRealToInt_ReturnsCorrectDeclaration()
    {
        using var context = new Z3Context();

        var func = context.Func<IntExpr, BoolExpr, RealExpr, IntExpr>("f");

        Assert.Multiple(() =>
        {
            Assert.That(func.Name, Is.EqualTo("f"));
            Assert.That(func.Arity, Is.EqualTo(3));
        });
    }

    [Test]
    public void FuncDeclsWithSameName_ReturnSameHandle()
    {
        using var context = new Z3Context();

        var func1 = context.Func<IntExpr, IntExpr>("f");
        var func2 = context.Func<IntExpr, IntExpr>("f");

        Assert.That(func1.Handle, Is.EqualTo(func2.Handle));
    }

    [Test]
    public void FuncDeclsWithDifferentNames_ReturnDifferentHandles()
    {
        using var context = new Z3Context();

        var func1 = context.Func<IntExpr, IntExpr>("f");
        var func2 = context.Func<IntExpr, IntExpr>("g");

        Assert.That(func1.Handle, Is.Not.EqualTo(func2.Handle));
    }

    [Test]
    public void FuncDeclsWithSameNameDifferentArity_ReturnDifferentHandles()
    {
        using var context = new Z3Context();

        var func1 = context.Func<IntExpr, IntExpr>("f");
        var func2 = context.Func<IntExpr, IntExpr, IntExpr>("f");

        Assert.That(func1.Handle, Is.Not.EqualTo(func2.Handle));
    }

    [Test]
    public void FuncDeclsWithSameNameDifferentTypes_ReturnDifferentHandles()
    {
        using var context = new Z3Context();

        var func1 = context.Func<IntExpr, IntExpr>("f");
        var func2 = context.Func<RealExpr, RealExpr>("f");

        Assert.That(func1.Handle, Is.Not.EqualTo(func2.Handle));
    }
}
