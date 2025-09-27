using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3ArrayExprTests
{
    private Z3Context context = null!;
    private Z3Context.SetUpScope setUpScope = null!;

    [SetUp]
    public void SetUp()
    {
        context = new Z3Context();
        setUpScope = context.SetUp();
    }

    [TearDown]
    public void TearDown()
    {
        setUpScope.Dispose();
        context.Dispose();
    }

    #region Array Creation Tests

    [TestCase("intToBool")]
    [TestCase("test_array")]
    [TestCase("arr")]
    public void ArrayConst_IntToBool_CreatesVariable(string name)
    {
        var array = context.ArrayConst<IntExpr, BoolExpr>(name);

        AssertValidArrayExpression(array, name);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, BoolExpr>>());
    }

    [TestCase("prices")]
    [TestCase("values")]
    public void ArrayConst_IntToReal_CreatesVariable(string name)
    {
        var array = context.ArrayConst<IntExpr, RealExpr>(name);

        AssertValidArrayExpression(array, name);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, RealExpr>>());
    }

    [TestCase("flags")]
    [TestCase("conditions")]
    public void ArrayConst_BoolToInt_CreatesVariable(string name)
    {
        var array = context.ArrayConst<BoolExpr, IntExpr>(name);

        AssertValidArrayExpression(array, name);
        Assert.That(array, Is.TypeOf<ArrayExpr<BoolExpr, IntExpr>>());
    }

    [TestCase("arr")]
    [TestCase("data")]
    public void ArrayConst_SingleGeneric_IntToBool_CreatesVariable(string name)
    {
        var array = context.ArrayConst<BoolExpr>(name);

        AssertValidArrayExpression(array, name);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, BoolExpr>>());
    }

    [TestCase("prices")]
    [TestCase("measurements")]
    public void ArrayConst_SingleGeneric_IntToReal_CreatesVariable(string name)
    {
        var array = context.ArrayConst<RealExpr>(name);

        AssertValidArrayExpression(array, name);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, RealExpr>>());
    }

    [Test]
    public void Array_BooleanDefault_CreatesConstantArray()
    {
        var defaultValue = context.Bool(true);
        var array = context.Array<IntExpr, BoolExpr>(defaultValue);

        AssertValidArrayExpression(array);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, BoolExpr>>());
    }

    [Test]
    public void Array_IntegerDefault_CreatesConstantArray()
    {
        var defaultValue = context.Int(42);
        var array = context.Array<BoolExpr, IntExpr>(defaultValue);

        AssertValidArrayExpression(array);
        Assert.That(array, Is.TypeOf<ArrayExpr<BoolExpr, IntExpr>>());
    }

    [Test]
    public void Array_SingleGeneric_BooleanDefault_CreatesConstantArray()
    {
        var defaultValue = context.Bool(true);
        var array = context.Array(defaultValue);

        AssertValidArrayExpression(array);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, BoolExpr>>());
    }

    [Test]
    public void Array_SingleGeneric_IntegerDefault_CreatesConstantArray()
    {
        var defaultValue = context.Int(42);
        var array = context.Array(defaultValue);

        AssertValidArrayExpression(array);
        Assert.That(array, Is.TypeOf<ArrayExpr<IntExpr, IntExpr>>());
    }

    #endregion

    #region Array Access Tests

    [TestCase(5)]
    [TestCase(0)]
    [TestCase(-1)]
    public void IndexerAccess_IntegerLiteral_WorksWithImplicitConversion(int index)
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");

        var element = array[index];

        AssertValidExpression(element);
        Assert.That(element, Is.TypeOf<IntExpr>());

        // Verify it works in solver context
        using var solver = context.CreateSolver();
        solver.Assert(array[index] == 42);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void IndexerAccess_Z3IntExpr_Works()
    {
        var array = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var index = context.Int(5);

        var element = array[index];

        AssertValidExpression(element);
        Assert.That(element, Is.TypeOf<BoolExpr>());
    }

    [Test]
    public void Select_ExtensionMethod_Z3IntConstIndex_Works()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.IntConst("i");

        var element = context.Select(array, index);

        AssertValidExpression(element);
        Assert.That(element, Is.TypeOf<IntExpr>());
    }

    [Test]
    public void Select_ExtensionMethodVsIndexer_ProduceEquivalentResults()
    {
        var array = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var index = context.Int(42);

        var elementViaExtension = context.Select(array, index);
        var elementViaIndexer = array[index];

        AssertValidExpression(elementViaExtension);
        AssertValidExpression(elementViaIndexer);
        Assert.That(elementViaExtension, Is.TypeOf<BoolExpr>());
        Assert.That(elementViaIndexer, Is.TypeOf<BoolExpr>());

        // Both should work in solver contexts
        using var solver = context.CreateSolver();
        solver.Assert(elementViaExtension == true);
        solver.Assert(elementViaIndexer == true);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Array Update Tests

    [TestCase(10, true)]
    [TestCase(0, false)]
    [TestCase(-5, true)]
    public void Store_IntegerLiteralIndexAndValue_ReturnsNewArray(int index, bool value)
    {
        var array = context.ArrayConst<IntExpr, BoolExpr>("arr");

        var updatedArray = array.Store(index, value);

        AssertValidArrayExpression(updatedArray);
        Assert.That(updatedArray, Is.TypeOf<ArrayExpr<IntExpr, BoolExpr>>());
        Assert.That(updatedArray.Handle, Is.Not.EqualTo(array.Handle)); // Functional update

        // Verify the stored value can be retrieved
        using var solver = context.CreateSolver();
        solver.Assert(updatedArray[index] == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Store_Z3Expressions_ReturnsNewArray()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.Int(7);
        var value = context.Int(555);

        var updatedArray = array.Store(index, value);

        AssertValidArrayExpression(updatedArray);
        Assert.That(updatedArray.Handle, Is.Not.EqualTo(array.Handle));

        // Verify store-select property
        using var solver = context.CreateSolver();
        solver.Assert(updatedArray[index] == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Store_ExtensionMethod_Works()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.Int(5);
        var value = context.Int(100);

        var updatedArrayViaExtension = context.Store(array, index, value);
        var updatedArrayViaInstance = array.Store(index, value);

        AssertValidArrayExpression(updatedArrayViaExtension);
        AssertValidArrayExpression(updatedArrayViaInstance);

        // Both should satisfy the same constraint
        using var solver = context.CreateSolver();
        solver.Assert(updatedArrayViaExtension[index] == value);
        solver.Assert(updatedArrayViaInstance[index] == value);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Array Constraint Solving Tests

    [Test]
    public void ArrayConstraints_MultipleIndices_Independent()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var i = context.Int(1);
        var j = context.Int(2);
        var value1 = context.Int(111);
        var value2 = context.Int(222);

        var array1 = array.Store(i, value1);
        var array2 = array1.Store(j, value2);

        using var solver = context.CreateSolver();
        solver.Assert(array2[i] == value1);
        solver.Assert(array2[j] == value2);
        solver.Assert(value1 != value2);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConstraints_ConstantArrayAllIndices_SameValue()
    {
        var array = context.Array<IntExpr, IntExpr>(99);

        using var solver = context.CreateSolver();
        solver.Assert(array[100] == 99);
        solver.Assert(array[200] == 99);
        solver.Assert(100 != 200);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConstraints_StoreSelectProperty_DifferentIndices()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index1 = context.Int(0);
        var index2 = context.Int(1);
        var value = context.Int(100);

        var storedArray = context.Store(array, index1, value);
        var select1 = context.Select(storedArray, index1);
        var select2 = context.Select(storedArray, index2);
        var originalAtIndex2 = context.Select(array, index2);

        using var solver = context.CreateSolver();
        solver.Assert(context.Eq<IntExpr>(select1, value)); // Should equal stored value
        solver.Assert(context.Eq<IntExpr>(select2, originalAtIndex2)); // Should equal original value

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayConstraints_ComplexNestedStoreSelect_Works()
    {
        var array = context.ArrayConst<IntExpr, IntExpr>("arr");
        var i = context.Int(1);
        var j = context.Int(2);

        var array1 = array.Store(i, context.Int(10));
        var array2 = array1.Store(j, context.Int(20));

        using var solver = context.CreateSolver();
        solver.Assert(array2[i] == context.Int(10)); // Should still be 10
        solver.Assert(array2[j] == context.Int(20)); // Should be 20

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var valI = model.GetIntValue(array2[i]);
        var valJ = model.GetIntValue(array2[j]);
        Assert.That(valI, Is.EqualTo(new BigInteger(10)));
        Assert.That(valJ, Is.EqualTo(new BigInteger(20)));
    }

    #endregion

    #region Array Type-Specific Tests

    [Test]
    public void RealArrays_DecimalConstraints_Works()
    {
        var prices = context.ArrayConst<IntExpr, RealExpr>("prices");
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

        // Z3 may return equivalent fraction representations
        Assert.That(price1Str, Is.EqualTo("1999/100"));
        Assert.That(price2Str, Is.EqualTo("599/20"));
    }

    [Test]
    public void BooleanArrays_LogicalConstraints_Works()
    {
        var flags = context.ArrayConst<IntExpr, BoolExpr>("flags");
        var idx0 = context.Int(0);
        var idx1 = context.Int(1);

        using var solver = context.CreateSolver();
        solver.Assert(flags[idx0] == context.Bool(true));
        solver.Assert(flags[idx1] == context.Bool(false));
        solver.Assert(flags[idx0] & !flags[idx1]); // true & !false = true

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        // Verify the negation is unsatisfiable
        using var solver2 = context.CreateSolver();
        solver2.Assert(flags[idx0] == context.Bool(true));
        solver2.Assert(flags[idx1] == context.Bool(false));
        solver2.Assert(!(flags[idx0] & !flags[idx1])); // Negate the constraint

        var result2 = solver2.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    #endregion

    #region Array Equality Tests

    [Test]
    public void ArrayEquality_SameReference_Equal()
    {
        var array1 = context.ArrayConst<IntExpr, BoolExpr>("arr");
        var array2 = array1; // Same reference

        using var solver = context.CreateSolver();
        solver.Assert(array1 == array2);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArrayEquality_DifferentArrays_CanBeEqual()
    {
        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");
        var index = context.Int(0);

        using var solver = context.CreateSolver();
        solver.Assert(array1 == array2);
        solver.Assert(array1[index] == context.Int(42));

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var val1 = model.GetIntValue(array1[index]);
        var val2 = model.GetIntValue(array2[index]);
        Assert.That(val1, Is.EqualTo(val2));
        Assert.That(val1, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void ArrayInequality_ExplicitlyDifferent_Works()
    {
        var array1 = context.ArrayConst<IntExpr, IntExpr>("arr1");
        var array2 = context.ArrayConst<IntExpr, IntExpr>("arr2");
        var index = context.Int(0);

        using var solver = context.CreateSolver();
        solver.Assert(array1 != array2);
        solver.Assert(array1[index] != array2[index]); // Make them provably different

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    #endregion

    #region Array String Representation Tests

    [TestCase("test_array")]
    [TestCase("my_array")]
    [TestCase("data")]
    public void ArrayToString_ValidOutput_ContainsArrayName(string arrayName)
    {
        var array = context.ArrayConst<IntExpr, BoolExpr>(arrayName);
        var stringRepresentation = array.ToString();

        Assert.That(stringRepresentation, Is.Not.Null);
        Assert.That(stringRepresentation, Is.Not.Empty);
        Assert.That(stringRepresentation, Is.EqualTo(arrayName));
    }

    #endregion

    #region Helper Methods

    private void AssertValidArrayExpression<TIndex, TValue>(
        ArrayExpr<TIndex, TValue> array,
        string? expectedName = null
    )
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        Assert.That(array, Is.Not.Null);
        Assert.That(array.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(array.Context, Is.SameAs(context));

        if (expectedName != null)
        {
            Assert.That(array.ToString(), Is.EqualTo(expectedName));
        }
    }

    private void AssertValidExpression(Z3Expr expression)
    {
        Assert.That(expression, Is.Not.Null);
        Assert.That(expression.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(expression.Context, Is.SameAs(context));
    }

    #endregion
}
