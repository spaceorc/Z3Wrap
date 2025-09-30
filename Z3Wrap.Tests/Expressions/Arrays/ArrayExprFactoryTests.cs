using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprFactoryTests
{
    [TestCase(Z3SortKind.Int, Z3SortKind.Int, TypeArgs = [typeof(IntExpr), typeof(IntExpr)])]
    [TestCase(Z3SortKind.Int, Z3SortKind.Real, TypeArgs = [typeof(IntExpr), typeof(RealExpr)])]
    [TestCase(Z3SortKind.Int, Z3SortKind.Bool, TypeArgs = [typeof(IntExpr), typeof(BoolExpr)])]
    [TestCase(Z3SortKind.Int, Z3SortKind.Bv, TypeArgs = [typeof(IntExpr), typeof(BvExpr<Size32>)])]
    [TestCase(Z3SortKind.Bool, Z3SortKind.Int, TypeArgs = [typeof(BoolExpr), typeof(IntExpr)])]
    [TestCase(Z3SortKind.Bool, Z3SortKind.Real, TypeArgs = [typeof(BoolExpr), typeof(RealExpr)])]
    [TestCase(Z3SortKind.Bool, Z3SortKind.Bool, TypeArgs = [typeof(BoolExpr), typeof(BoolExpr)])]
    [TestCase(Z3SortKind.Real, Z3SortKind.Int, TypeArgs = [typeof(RealExpr), typeof(IntExpr)])]
    [TestCase(Z3SortKind.Real, Z3SortKind.Real, TypeArgs = [typeof(RealExpr), typeof(RealExpr)])]
    [TestCase(Z3SortKind.Real, Z3SortKind.Bool, TypeArgs = [typeof(RealExpr), typeof(BoolExpr)])]
    [TestCase(Z3SortKind.Bv, Z3SortKind.Bv, TypeArgs = [typeof(BvExpr<Size8>), typeof(BvExpr<Size32>)])]
    [TestCase(Z3SortKind.Bv, Z3SortKind.Bv, TypeArgs = [typeof(BvExpr<Size32>), typeof(BvExpr<Size32>)])]
    [TestCase(Z3SortKind.Bv, Z3SortKind.Int, TypeArgs = [typeof(BvExpr<Size32>), typeof(IntExpr)])]
    public void ArrayConst_VariousTypes_CreatesVariable<TIndex, TValue>(
        Z3SortKind expectedIndexSortKind,
        Z3SortKind expectedValueSortKind
    )
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<TIndex, TValue>("arr");

        var indexSort = context.GetSortForType<TIndex>();
        var valueSort = context.GetSortForType<TValue>();
        var indexSortKind = context.Library.Z3GetSortKind(context.Handle, indexSort);
        var valueSortKind = context.Library.Z3GetSortKind(context.Handle, valueSort);

        Assert.Multiple(() =>
        {
            Assert.That(array.ToString(), Is.EqualTo("arr"));
            Assert.That(indexSortKind, Is.EqualTo(expectedIndexSortKind));
            Assert.That(valueSortKind, Is.EqualTo(expectedValueSortKind));
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(Z3SortKind.Int, TypeArgs = [typeof(IntExpr)])]
    [TestCase(Z3SortKind.Real, TypeArgs = [typeof(RealExpr)])]
    [TestCase(Z3SortKind.Bool, TypeArgs = [typeof(BoolExpr)])]
    [TestCase(Z3SortKind.Bv, TypeArgs = [typeof(BvExpr<Size32>)])]
    [TestCase(Z3SortKind.Bv, TypeArgs = [typeof(BvExpr<Size64>)])]
    public void ArrayConst_SingleGeneric_CreatesVariable<TValue>(Z3SortKind expectedValueSortKind)
        where TValue : Z3Expr, IExprType<TValue>
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<TValue>("arr");

        var indexSort = context.GetSortForType<IntExpr>();
        var valueSort = context.GetSortForType<TValue>();
        var indexSortKind = context.Library.Z3GetSortKind(context.Handle, indexSort);
        var valueSortKind = context.Library.Z3GetSortKind(context.Handle, valueSort);

        Assert.Multiple(() =>
        {
            Assert.That(array.ToString(), Is.EqualTo("arr"));
            Assert.That(indexSortKind, Is.EqualTo(Z3SortKind.Int));
            Assert.That(valueSortKind, Is.EqualTo(expectedValueSortKind));
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Array_IntDefaultValue_AllElementsHaveDefaultValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array<IntExpr, IntExpr>(42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(array[0]), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(array[100]), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(array[-5]), Is.EqualTo(new BigInteger(42)));
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Array_BoolDefaultValue_AllElementsHaveDefaultValue(bool defaultValue)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array<IntExpr, BoolExpr>(defaultValue);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(array[0]), Is.EqualTo(defaultValue));
            Assert.That(model.GetBoolValue(array[50]), Is.EqualTo(defaultValue));
        });
    }
}
