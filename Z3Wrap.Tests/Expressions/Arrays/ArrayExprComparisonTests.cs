using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprComparisonTests
{
    [Test]
    public void Equality_DifferentArrays_CanBeEqual()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");

        solver.Assert(array1 == array2);
        solver.Assert(array1[0] == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var val1 = model.GetIntValue(array1[0]);
        var val2 = model.GetIntValue(array2[0]);

        Assert.Multiple(() =>
        {
            Assert.That(val1, Is.EqualTo(val2));
            Assert.That(val1, Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Inequality_DifferentArrays_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");

        solver.Assert(array1 != array2);
        solver.Assert(array1[0] == 10);
        solver.Assert(array2[0] == 20);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array1[0]), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(array2[0]), Is.EqualTo(new BigInteger(20)));
        });
    }

    [Test]
    public void Equality_ConstantArrays_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.Array<IntExpr, IntExpr>(42);
        var array2 = context.Array<IntExpr, IntExpr>(42);

        solver.Assert(array1 == array2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array1[0]), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(array2[0]), Is.EqualTo(new BigInteger(42)));
        });
    }
}
