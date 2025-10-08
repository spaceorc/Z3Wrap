using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprFactoryTests
{
    [TestCase(
        Z3Library.SortKind.Z3_INT_SORT,
        Z3Library.SortKind.Z3_INT_SORT,
        TypeArgs = [typeof(IntExpr), typeof(IntExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_INT_SORT,
        Z3Library.SortKind.Z3_REAL_SORT,
        TypeArgs = [typeof(IntExpr), typeof(RealExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_INT_SORT,
        Z3Library.SortKind.Z3_BOOL_SORT,
        TypeArgs = [typeof(IntExpr), typeof(BoolExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_INT_SORT,
        Z3Library.SortKind.Z3_BV_SORT,
        TypeArgs = [typeof(IntExpr), typeof(BvExpr<Size32>)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BOOL_SORT,
        Z3Library.SortKind.Z3_INT_SORT,
        TypeArgs = [typeof(BoolExpr), typeof(IntExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BOOL_SORT,
        Z3Library.SortKind.Z3_REAL_SORT,
        TypeArgs = [typeof(BoolExpr), typeof(RealExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BOOL_SORT,
        Z3Library.SortKind.Z3_BOOL_SORT,
        TypeArgs = [typeof(BoolExpr), typeof(BoolExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_REAL_SORT,
        Z3Library.SortKind.Z3_INT_SORT,
        TypeArgs = [typeof(RealExpr), typeof(IntExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_REAL_SORT,
        Z3Library.SortKind.Z3_REAL_SORT,
        TypeArgs = [typeof(RealExpr), typeof(RealExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_REAL_SORT,
        Z3Library.SortKind.Z3_BOOL_SORT,
        TypeArgs = [typeof(RealExpr), typeof(BoolExpr)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BV_SORT,
        Z3Library.SortKind.Z3_BV_SORT,
        TypeArgs = [typeof(BvExpr<Size8>), typeof(BvExpr<Size32>)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BV_SORT,
        Z3Library.SortKind.Z3_BV_SORT,
        TypeArgs = [typeof(BvExpr<Size32>), typeof(BvExpr<Size32>)]
    )]
    [TestCase(
        Z3Library.SortKind.Z3_BV_SORT,
        Z3Library.SortKind.Z3_INT_SORT,
        TypeArgs = [typeof(BvExpr<Size32>), typeof(IntExpr)]
    )]
    public void ArrayConst_VariousTypes_CreatesVariable<TIndex, TValue>(
        Z3Library.SortKind expectedIndexSortKind,
        Z3Library.SortKind expectedValueSortKind
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
        var indexSortKind = context.Library.GetSortKind(context.Handle, indexSort);
        var valueSortKind = context.Library.GetSortKind(context.Handle, valueSort);

        Assert.Multiple(() =>
        {
            Assert.That(array.ToString(), Is.EqualTo("arr"));
            Assert.That(indexSortKind, Is.EqualTo(expectedIndexSortKind));
            Assert.That(valueSortKind, Is.EqualTo(expectedValueSortKind));
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(Z3Library.SortKind.Z3_INT_SORT, TypeArgs = [typeof(IntExpr)])]
    [TestCase(Z3Library.SortKind.Z3_REAL_SORT, TypeArgs = [typeof(RealExpr)])]
    [TestCase(Z3Library.SortKind.Z3_BOOL_SORT, TypeArgs = [typeof(BoolExpr)])]
    [TestCase(Z3Library.SortKind.Z3_BV_SORT, TypeArgs = [typeof(BvExpr<Size32>)])]
    [TestCase(Z3Library.SortKind.Z3_BV_SORT, TypeArgs = [typeof(BvExpr<Size64>)])]
    public void ArrayConst_SingleGeneric_CreatesVariable<TValue>(Z3Library.SortKind expectedValueSortKind)
        where TValue : Z3Expr, IExprType<TValue>
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<TValue>("arr");

        var indexSort = context.GetSortForType<IntExpr>();
        var valueSort = context.GetSortForType<TValue>();
        var indexSortKind = context.Library.GetSortKind(context.Handle, indexSort);
        var valueSortKind = context.Library.GetSortKind(context.Handle, valueSort);

        Assert.Multiple(() =>
        {
            Assert.That(array.ToString(), Is.EqualTo("arr"));
            Assert.That(indexSortKind, Is.EqualTo(Z3Library.SortKind.Z3_INT_SORT));
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

    [Test]
    public void Array_SingleGeneric_IntDefaultValue_AllElementsHaveDefaultValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array(context.Int(42));

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
    public void Array_SingleGeneric_BoolDefaultValue_AllElementsHaveDefaultValue(bool defaultValue)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array(context.Bool(defaultValue));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(array[0]), Is.EqualTo(defaultValue));
            Assert.That(model.GetBoolValue(array[50]), Is.EqualTo(defaultValue));
        });
    }

    [Test]
    public void Array_SingleGeneric_RealDefaultValue_AllElementsHaveDefaultValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array(context.Real(new Real(3, 2)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(array[0]), Is.EqualTo(new Real(3, 2)));
            Assert.That(model.GetRealValue(array[99]), Is.EqualTo(new Real(3, 2)));
        });
    }

    [Test]
    public void Array_SingleGeneric_BitVecDefaultValue_AllElementsHaveDefaultValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.Array(context.Bv<Size32>(255u));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBv(array[0]).Value, Is.EqualTo(new BigInteger(255)));
            Assert.That(model.GetBv(array[10]).Value, Is.EqualTo(new BigInteger(255)));
        });
    }
}
