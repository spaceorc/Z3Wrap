using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class ArrayExtensionsTests
{
    [Test]
    public void ArrayConst_Generic_CreatesArrayExpression()
    {
        using var context = new Z3Context();

        var array = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");

        Assert.That(array.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(array.Context, Is.SameAs(context));
    }

    [Test]
    public void ArrayConst_TypeInferred_CreatesIntIndexedArrayExpression()
    {
        using var context = new Z3Context();

        var array = context.ArrayConst<Z3IntExpr>("arr");

        Assert.That(array.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(array.Context, Is.SameAs(context));
    }

    [Test]
    public void Select_ArrayExpr_CreatesSelectExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var index = context.IntConst("i");
        var selected = context.Select(array, index);

        Assert.That(selected.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(selected.Context, Is.SameAs(context));
    }

    [Test]
    public void Store_ArrayExpr_CreatesStoreExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var index = context.Int(0);
        var value = context.Int(42);
        var stored = context.Store(array, index, value);

        Assert.That(stored.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(stored.Context, Is.SameAs(context));

        // Test that storing and then selecting gives back the stored value
        var retrieved = context.Select(stored, index);
        solver.Assert(context.Eq(retrieved, value));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayOperations_StoreSelectProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var index1 = context.Int(0);
        var index2 = context.Int(1);
        var value = context.Int(100);

        // Store value at index1, then select from different indices
        var stored = context.Store(array, index1, value);
        var select1 = context.Select(stored, index1);
        var select2 = context.Select(stored, index2);

        // select1 should equal the stored value
        solver.Assert(context.Eq(select1, value));

        // select2 should equal the original array value at index2
        var originalValue = context.Select(array, index2);
        solver.Assert(context.Eq(select2, originalValue));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}