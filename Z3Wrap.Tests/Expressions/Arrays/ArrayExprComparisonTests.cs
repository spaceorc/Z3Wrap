using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprComparisonTests
{
    [Test]
    public void Equality_SameArray_Equal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var array2 = array1;

        solver.Assert(array1 == array2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

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
    public void Inequality_DifferentArrays_CanBeDifferent()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");

        solver.Assert(array1 != array2);
        solver.Assert(array1[0] != array2[0]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Equality_ArraysWithSameElements_Equal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");

        var updated1 = array1.Store(0, 10).Store(1, 20);
        var updated2 = array2.Store(0, 10).Store(1, 20);

        solver.Assert(array1[0] == 10);
        solver.Assert(array1[1] == 20);
        solver.Assert(array2[0] == 10);
        solver.Assert(array2[1] == 20);
        solver.Assert(updated1 == updated2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Inequality_ArraysWithDifferentElements_NotEqual()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");

        solver.Assert(array1[0] == 10);
        solver.Assert(array2[0] == 20);
        solver.Assert(array1 != array2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Equality_ConstantArrays_SameDefault_Equal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.Array<IntExpr, IntExpr>(42);
        var array2 = context.Array<IntExpr, IntExpr>(42);

        solver.Assert(array1 == array2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Inequality_ConstantArrays_DifferentDefaults_NotEqual()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array1 = context.Array<IntExpr, IntExpr>(42);
        var array2 = context.Array<IntExpr, IntExpr>(99);

        solver.Assert(array1 != array2);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
