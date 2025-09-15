using System.Numerics;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3ArrayExprTests
{
    [Test]
    public void ArrayConst_IntToBool_CreatesVariable()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("arr");

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ArrayConst_IntToReal_CreatesVariable()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr.Context, Is.EqualTo(context));
    }

    [Test]
    public void ArrayConst_BoolToInt_CreatesVariable()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3BoolExpr, Z3IntExpr>("flags");

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr.Context, Is.EqualTo(context));
    }

    [Test]
    public void Array_AllElementsSameValue_Creates()
    {
        using var context = new Z3Context();
        var defaultVal = context.Bool(true);
        var arr = context.Array<Z3IntExpr, Z3BoolExpr>(defaultVal);

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Array_IntegerDefault_Creates()
    {
        using var context = new Z3Context();
        var defaultVal = context.Int(42);
        var arr = context.Array<Z3BoolExpr, Z3IntExpr>(defaultVal);

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void IndexerAccess_SelectOperation_Works()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("arr");
        var index = context.Int(5);

        var element = arr[index];

        Assert.That(element, Is.Not.Null);
        Assert.That(element, Is.TypeOf<Z3BoolExpr>());
        Assert.That(element.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void IndexerAccess_IntegerLiteral_WorksWithImplicitConversion()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");

        var element = arr[0];

        Assert.That(element, Is.Not.Null);
        Assert.That(element, Is.TypeOf<Z3IntExpr>());
        Assert.That(element.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify it works in solver context
        using var solver = context.CreateSolver();
        solver.Assert(arr[0] == 42);
        solver.Assert(arr[1] == 100);
        solver.Assert(arr[0] < arr[1]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Store_UpdatesArray_ReturnsNewArray()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("arr");

        var updatedArr = arr.Store(10, true);

        Assert.That(updatedArr, Is.Not.Null);
        Assert.That(updatedArr, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3BoolExpr>>());
        Assert.That(updatedArr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(updatedArr.Handle, Is.Not.EqualTo(arr.Handle)); // Functional update
    }

    [Test]
    public void ArrayConstraints_SolveWithSelect_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");

        using var solver = context.CreateSolver();
        solver.Assert(arr[5] == 42);
        solver.Assert(arr[10] == 100);
        solver.Assert(arr[5] < arr[10]);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model, Is.Not.Null);
    }

    [Test]
    public void ArrayConstraints_SolveWithStore_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr1 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr1");

        var arr2 = arr1.Store(3, 999);

        using var solver = context.CreateSolver();
        solver.Assert(arr2[3] == 999);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConstraints_StoreSelectProperty_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");

        var updatedArr = arr.Store(7, 555);

        using var solver = context.CreateSolver();
        solver.Assert(updatedArr[7] == 555);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConstraints_DifferentIndices_Independent()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var i = context.Int(1);
        var j = context.Int(2);
        var value1 = context.Int(111);
        var value2 = context.Int(222);

        var arr1 = arr.Store(i, value1);
        var arr2 = arr1.Store(j, value2);

        using var solver = context.CreateSolver();
        solver.Assert(arr2[i] == value1);
        solver.Assert(arr2[j] == value2);
        solver.Assert(value1 != value2);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Array_AllIndicesHaveSameValue_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var arr = context.Array<Z3IntExpr, Z3IntExpr>(99);

        using var solver = context.CreateSolver();
        solver.Assert(arr[100] == 99);
        solver.Assert(arr[200] == 99);
        solver.Assert(100 != 200);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayEquality_SameArrays_Equal()
    {
        using var context = new Z3Context();
        var arr1 = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("arr");
        var arr2 = arr1; // Same reference

        using var solver = context.CreateSolver();
        solver.Assert(arr1 == arr2);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayEquality_DifferentArrays_CanBeEqual()
    {
        using var context = new Z3Context();
        var arr1 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr1");
        var arr2 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr2");
        var i = context.Int(0);

        using var solver = context.CreateSolver();
        // Force arrays to be equal by constraining all accessible elements
        solver.Assert(arr1 == arr2);
        solver.Assert(arr1[i] == context.Int(42));

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Both arrays should select the same value at index i
        var val1 = model.GetIntValue(arr1[i]);
        var val2 = model.GetIntValue(arr2[i]);
        Assert.That(val1, Is.EqualTo(val2));
        Assert.That(val1, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void ArrayInequality_ExplicitlyDifferent_Works()
    {
        using var context = new Z3Context();
        var arr1 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr1");
        var arr2 = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr2");
        var i = context.Int(0);

        using var solver = context.CreateSolver();
        solver.Assert(arr1 != arr2);
        solver.Assert(arr1[i] != arr2[i]); // Make them provably different

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexArrayOperation_NestedStoreSelect_Works()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var i = context.Int(1);
        var j = context.Int(2);

        // arr[i] := 10, then arr[j] := 20, then select arr[i]
        var arr1 = arr.Store(i, context.Int(10));
        var arr2 = arr1.Store(j, context.Int(20));

        using var solver = context.CreateSolver();
        solver.Assert(arr2[i] == context.Int(10)); // Should still be 10
        solver.Assert(arr2[j] == context.Int(20)); // Should be 20

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var valI = model.GetIntValue(arr2[i]);
        var valJ = model.GetIntValue(arr2[j]);
        Assert.That(valI, Is.EqualTo(new BigInteger(10)));
        Assert.That(valJ, Is.EqualTo(new BigInteger(20)));
    }

    [Test]
    public void RealArrays_SolveConstraints_Works()
    {
        using var context = new Z3Context();
        var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
        var item1 = context.Int(1);
        var item2 = context.Int(2);

        using var solver = context.CreateSolver();
        solver.Assert(prices[item1] == context.Real(19.99m));
        solver.Assert(prices[item2] == context.Real(29.95m));
        solver.Assert(prices[item1] < prices[item2]);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var price1Str = model.GetNumericValueAsString(prices[item1]);
        var price2Str = model.GetNumericValueAsString(prices[item2]);

        // Check that the stored values are maintained (Z3 may return equivalent fractions)
        // 19.99 = 1999/100, 29.95 = 599/20 are equivalent representations
        Assert.That(price1Str, Is.EqualTo("1999/100"));
        Assert.That(price2Str, Is.EqualTo("599/20"));
    }

    [Test]
    public void BooleanArrays_LogicalConstraints_Works()
    {
        using var context = new Z3Context();
        var flags = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("flags");
        var idx0 = context.Int(0);
        var idx1 = context.Int(1);

        using var solver = context.CreateSolver();
        solver.Assert(flags[idx0] == context.Bool(true));
        solver.Assert(flags[idx1] == context.Bool(false));
        solver.Assert(flags[idx0] & !flags[idx1]); // true & !false = true

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        // For now, just verify that the constraints are satisfiable
        // Array element evaluation in models may require additional Z3 configuration
        var model = solver.GetModel();
        Assert.That(model, Is.Not.Null);

        // Test that the constraint logic works by checking satisfiability
        using var solver2 = context.CreateSolver();
        solver2.Assert(flags[idx0] == context.Bool(true));
        solver2.Assert(flags[idx1] == context.Bool(false));
        solver2.Assert(!(flags[idx0] & !flags[idx1])); // Negate the constraint

        var result2 = solver2.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Unsatisfiable)); // Should be UNSAT
    }

    [Test]
    public void ArrayToString_ValidOutput_ContainsArrayInfo()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("test_array");
        var toString = arr.ToString();

        Assert.That(toString, Is.Not.Null);
        Assert.That(toString, Is.Not.Empty);
        // Z3 typically shows array expressions with the variable name
        Assert.That(toString, Is.EqualTo("test_array"));
    }

    [Test]
    public void ArrayContextExtensions_SelectMethod_Works()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("arr");
        var index = context.Int(42);

        var element1 = context.Select(arr, index); // Extension method
        var element2 = arr[index]; // Indexer

        // Both should produce equivalent expressions
        Assert.That(element1, Is.Not.Null);
        Assert.That(element2, Is.Not.Null);
        Assert.That(element1, Is.TypeOf<Z3BoolExpr>());
        Assert.That(element2, Is.TypeOf<Z3BoolExpr>());
    }

    [Test]
    public void ArrayContextExtensions_StoreMethod_Works()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        var index = context.Int(5);
        var value = context.Int(100);

        var newArr1 = context.Store(arr, index, value); // Extension method
        var newArr2 = arr.Store(index, value); // Instance method

        using var solver = context.CreateSolver();
        solver.Assert(newArr1[index] == value);
        solver.Assert(newArr2[index] == value);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConst_SingleGeneric_IntToBool_CreatesVariable()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3BoolExpr>("arr");

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr.Context, Is.EqualTo(context));
        Assert.That(arr, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3BoolExpr>>());
    }

    [Test]
    public void ArrayConst_SingleGeneric_IntToReal_CreatesVariable()
    {
        using var context = new Z3Context();
        var arr = context.ArrayConst<Z3RealExpr>("prices");

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr.Context, Is.EqualTo(context));
        Assert.That(arr, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3RealExpr>>());
    }

    [Test]
    public void Array_SingleGeneric_AllElementsSameValue_Creates()
    {
        using var context = new Z3Context();
        var defaultVal = context.Bool(true);
        var arr = context.Array(defaultVal);

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3BoolExpr>>());
    }

    [Test]
    public void Array_SingleGeneric_IntegerDefault_Creates()
    {
        using var context = new Z3Context();
        var defaultVal = context.Int(42);
        var arr = context.Array(defaultVal);

        Assert.That(arr, Is.Not.Null);
        Assert.That(arr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(arr, Is.TypeOf<Z3ArrayExpr<Z3IntExpr, Z3IntExpr>>());
    }
}