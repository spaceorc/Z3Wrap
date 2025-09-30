using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprStoreTests
{
    [TestCase(10, true)]
    [TestCase(0, false)]
    [TestCase(-5, true)]
    public void Store_IntegerIndexAndBoolValue_Works(int index, bool value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var updatedArrayViaInstance = array.Store(index, value);
        var updatedArrayViaExtension = context.Store(array, index, value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(updatedArrayViaInstance[index]), Is.EqualTo(value));
            Assert.That(model.GetBoolValue(updatedArrayViaExtension[index]), Is.EqualTo(value));
        });
    }

    [Test]
    public void Store_IntegerIndexAndValue_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var updatedArrayViaInstance = array.Store(7, 555);
        var updatedArrayViaExtension = context.Store(array, 7, 555);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(updatedArrayViaInstance[7]), Is.EqualTo(new BigInteger(555)));
            Assert.That(model.GetIntValue(updatedArrayViaExtension[7]), Is.EqualTo(new BigInteger(555)));
        });
    }

    [Test]
    public void Store_MultipleStores_ChainCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var array1 = array.Store(1, 111);
        var array2 = array1.Store(2, 222);

        solver.Assert(array2[1] == 111);
        solver.Assert(array2[2] == 222);
        solver.Assert(111 != 222);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array2[1]), Is.EqualTo(new BigInteger(111)));
            Assert.That(model.GetIntValue(array2[2]), Is.EqualTo(new BigInteger(222)));
        });
    }

    [Test]
    public void Store_StoreSelectProperty_SameIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.Int(0);
        var value = context.Int(100);

        var storedArray = context.Store(array, index, value);
        var selected = context.Select(storedArray, index);

        solver.Assert(selected == value);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Store_StoreSelectProperty_DifferentIndices()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index1 = context.Int(0);
        var index2 = context.Int(1);
        var value = context.Int(100);

        var storedArray = context.Store(array, index1, value);
        var select1 = context.Select(storedArray, index1);
        var select2 = context.Select(storedArray, index2);
        var originalAtIndex2 = context.Select(array, index2);

        solver.Assert(select1 == value);
        solver.Assert(select2 == originalAtIndex2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Store_ComplexNestedStores_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var array1 = array.Store(1, 10);
        var array2 = array1.Store(2, 20);

        solver.Assert(array2[1] == 10);
        solver.Assert(array2[2] == 20);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array2[1]), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(array2[2]), Is.EqualTo(new BigInteger(20)));
        });
    }
}
