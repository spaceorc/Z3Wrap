using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprAccessTests
{
    [TestCase(0, 100)]
    [TestCase(5, 42)]
    [TestCase(-1, 999)]
    [TestCase(100, 777)]
    public void IndexerAccess_IntegerIndex_Works(int index, int expectedValue)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        solver.Assert(array[index] == expectedValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(array[index]), Is.EqualTo(new BigInteger(expectedValue)));
    }

    [Test]
    public void IndexerAccess_SymbolicIndex_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.IntConst("i");

        solver.Assert(index == 10);
        solver.Assert(array[index] == 555);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(array[index]), Is.EqualTo(new BigInteger(555)));
            Assert.That(model.GetIntValue(array[10]), Is.EqualTo(new BigInteger(555)));
        });
    }

    [Test]
    public void Select_ExtensionMethodAndIndexer_ProduceEquivalentResults()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.IntConst("i");

        var elementViaIndexer = array[index];
        var elementViaExtension = context.Select(array, index);

        solver.Assert(index == 7);
        solver.Assert(elementViaIndexer == 888);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(7)));
            Assert.That(model.GetIntValue(elementViaIndexer), Is.EqualTo(new BigInteger(888)));
            Assert.That(model.GetIntValue(elementViaExtension), Is.EqualTo(new BigInteger(888)));
        });
    }

    [Test]
    public void Select_BoolArrayIndexerAndExtension_ProduceEquivalentResults()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var index = context.Int(42);

        var elementViaExtension = context.Select(array, index);
        var elementViaIndexer = array[index];

        solver.Assert(elementViaIndexer);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(elementViaIndexer), Is.True);
            Assert.That(model.GetBoolValue(elementViaExtension), Is.True);
        });
    }

    [Test]
    public void IndexerAccess_MultipleIndices_Independent()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        solver.Assert(array[1] == 111);
        solver.Assert(array[2] == 222);
        solver.Assert(array[1] != array[2]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array[1]), Is.EqualTo(new BigInteger(111)));
            Assert.That(model.GetIntValue(array[2]), Is.EqualTo(new BigInteger(222)));
        });
    }

    [Test]
    public void IndexerAccess_ConstantArray_AllIndicesSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array<IntExpr, IntExpr>(99);

        solver.Assert(array[100] == 99);
        solver.Assert(array[200] == 99);
        solver.Assert(100 != 200);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array[100]), Is.EqualTo(new BigInteger(99)));
            Assert.That(model.GetIntValue(array[200]), Is.EqualTo(new BigInteger(99)));
        });
    }
}
