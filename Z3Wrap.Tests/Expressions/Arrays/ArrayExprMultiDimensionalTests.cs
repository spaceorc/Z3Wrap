using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprMultiDimensionalTests
{
    [Test]
    public void TwoDimensional_IndexerAccess_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix");
        var updatedArray = array.Store(context.Int(0), context.Int(1), context.Int(42));
        var value = updatedArray[context.Int(0), context.Int(1)];

        solver.Assert(value == context.Int(42));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TwoDimensional_StoreMethod_UpdatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix");
        var row = context.Int(2);
        var col = context.Int(3);
        var value = context.Int(99);

        var updatedArray = array.Store(row, col, value);
        var retrievedValue = updatedArray[row, col];

        solver.Assert(retrievedValue == value);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(retrievedValue), Is.EqualTo(new BigInteger(99)));
    }

    [Test]
    public void TwoDimensional_MultipleStores_MaintainSeparateValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix");
        var array1 = array.Store(context.Int(0), context.Int(0), context.Int(10));
        var array2 = array1.Store(context.Int(0), context.Int(1), context.Int(20));
        var array3 = array2.Store(context.Int(1), context.Int(0), context.Int(30));

        var val00 = array3[context.Int(0), context.Int(0)];
        var val01 = array3[context.Int(0), context.Int(1)];
        var val10 = array3[context.Int(1), context.Int(0)];

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(val00), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(val01), Is.EqualTo(new BigInteger(20)));
            Assert.That(model.GetIntValue(val10), Is.EqualTo(new BigInteger(30)));
        });
    }

    [Test]
    public void ThreeDimensional_IndexerAccess_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube");
        var updatedArray = array.Store(context.Int(1), context.Int(2), context.Int(3), context.Int(123));
        var value = updatedArray[context.Int(1), context.Int(2), context.Int(3)];

        solver.Assert(value == context.Int(123));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ThreeDimensional_StoreMethod_UpdatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube");
        var x = context.Int(5);
        var y = context.Int(6);
        var z = context.Int(7);
        var value = context.Int(567);

        var updatedArray = array.Store(x, y, z, value);
        var retrievedValue = updatedArray[x, y, z];

        solver.Assert(retrievedValue == value);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(retrievedValue), Is.EqualTo(new BigInteger(567)));
    }

    [Test]
    public void ThreeDimensional_MultipleStores_MaintainSeparateValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube");
        var array1 = array.Store(context.Int(0), context.Int(0), context.Int(0), context.Int(100));
        var array2 = array1.Store(context.Int(0), context.Int(0), context.Int(1), context.Int(200));
        var array3 = array2.Store(context.Int(0), context.Int(1), context.Int(0), context.Int(300));
        var array4 = array3.Store(context.Int(1), context.Int(0), context.Int(0), context.Int(400));

        var val000 = array4[context.Int(0), context.Int(0), context.Int(0)];
        var val001 = array4[context.Int(0), context.Int(0), context.Int(1)];
        var val010 = array4[context.Int(0), context.Int(1), context.Int(0)];
        var val100 = array4[context.Int(1), context.Int(0), context.Int(0)];

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(val000), Is.EqualTo(new BigInteger(100)));
            Assert.That(model.GetIntValue(val001), Is.EqualTo(new BigInteger(200)));
            Assert.That(model.GetIntValue(val010), Is.EqualTo(new BigInteger(300)));
            Assert.That(model.GetIntValue(val100), Is.EqualTo(new BigInteger(400)));
        });
    }

    [Test]
    public void TwoDimensional_EqualityOperator_ComparesArrays()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix1");
        var array2 = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix2");

        var array1Updated = array1.Store(context.Int(0), context.Int(0), context.Int(42));
        var array2Updated = array2.Store(context.Int(0), context.Int(0), context.Int(42));

        solver.Assert(array1Updated == array2Updated);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TwoDimensional_InequalityOperator_DetectsDifference()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix1");
        var array2 = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix2");

        var array1Updated = array1.Store(context.Int(0), context.Int(0), context.Int(42));
        var array2Updated = array2.Store(context.Int(0), context.Int(0), context.Int(99));

        solver.Assert(array1Updated != array2Updated);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ThreeDimensional_EqualityOperator_ComparesArrays()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube1");
        var array2 = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube2");

        var array1Updated = array1.Store(context.Int(1), context.Int(2), context.Int(3), context.Int(123));
        var array2Updated = array2.Store(context.Int(1), context.Int(2), context.Int(3), context.Int(123));

        solver.Assert(array1Updated == array2Updated);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ThreeDimensional_InequalityOperator_DetectsDifference()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube1");
        var array2 = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube2");

        var array1Updated = array1.Store(context.Int(1), context.Int(2), context.Int(3), context.Int(123));
        var array2Updated = array2.Store(context.Int(1), context.Int(2), context.Int(3), context.Int(456));

        solver.Assert(array1Updated != array2Updated);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TwoDimensional_ContextSelect_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix");
        var updatedArray = context.Store(array, context.Int(3), context.Int(4), context.Int(34));
        var value = context.Select(updatedArray, context.Int(3), context.Int(4));

        solver.Assert(value == context.Int(34));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ThreeDimensional_ContextSelect_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("cube");
        var updatedArray = context.Store(array, context.Int(2), context.Int(3), context.Int(4), context.Int(234));
        var value = context.Select(updatedArray, context.Int(2), context.Int(3), context.Int(4));

        solver.Assert(value == context.Int(234));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void TwoDimensional_MixedIndexTypes_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, RealExpr, IntExpr>("mixed");
        var realIndex = context.Real(new Real(3, 2));
        var updatedArray = array.Store(context.Int(5), realIndex, context.Int(99));
        var value = updatedArray[context.Int(5), realIndex];

        solver.Assert(value == context.Int(99));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ThreeDimensional_MixedIndexTypes_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<IntExpr, RealExpr, IntExpr, IntExpr>("mixed");
        var realIndex = context.Real(new Real(2, 1));
        var updatedArray = array.Store(context.Int(1), realIndex, context.Int(3), context.Int(777));
        var value = updatedArray[context.Int(1), realIndex, context.Int(3)];

        solver.Assert(value == context.Int(777));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
