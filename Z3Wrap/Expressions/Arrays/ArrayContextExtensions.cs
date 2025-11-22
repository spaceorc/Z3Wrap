using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Arrays;

/// <summary>
/// Provides array creation methods for Z3Context.
/// </summary>
public static class ArrayContextExtensions
{
    /// <summary>
    /// Creates array constant with specified index and value types.
    /// </summary>
    /// <typeparam name="TIndex">Array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Array expression of type TIndex to TValue.</returns>
    public static ArrayExpr<TIndex, TValue> ArrayConst<TIndex, TValue>(this Z3Context context, string name)
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var arraySort = context.GetSortForType<ArrayExpr<TIndex, TValue>>();

        var handle = context.Library.MkConst(context.Handle, name, arraySort);

        return Z3Expr.Create<ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates array constant with integer index type.
    /// </summary>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Array expression from integer to TValue.</returns>
    public static ArrayExpr<IntExpr, TValue> ArrayConst<TValue>(this Z3Context context, string name)
        where TValue : Z3Expr, IExprType<TValue>
    {
        return context.ArrayConst<IntExpr, TValue>(name);
    }

    /// <summary>
    /// Creates 2D array constant with specified index and value types.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>2D array expression.</returns>
    public static ArrayExpr<TIndex1, TIndex2, TValue> ArrayConst<TIndex1, TIndex2, TValue>(
        this Z3Context context,
        string name
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var arraySort = context.GetSortForType<ArrayExpr<TIndex1, TIndex2, TValue>>();
        var handle = context.Library.MkConst(context.Handle, name, arraySort);
        return Z3Expr.Create<ArrayExpr<TIndex1, TIndex2, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates 3D array constant with specified index and value types.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TIndex3">Third array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>3D array expression.</returns>
    public static ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> ArrayConst<TIndex1, TIndex2, TIndex3, TValue>(
        this Z3Context context,
        string name
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TIndex3 : Z3Expr, IExprType<TIndex3>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var arraySort = context.GetSortForType<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>();
        var handle = context.Library.MkConst(context.Handle, name, arraySort);
        return Z3Expr.Create<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates constant array with default value for all indices.
    /// </summary>
    /// <typeparam name="TIndex">Array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="defaultValue">The default value for all array elements.</param>
    /// <returns>Array expression with constant default value.</returns>
    public static ArrayExpr<TIndex, TValue> Array<TIndex, TValue>(this Z3Context context, TValue defaultValue)
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var indexSort = context.GetSortForType<TIndex>();

        var handle = context.Library.MkConstArray(context.Handle, indexSort, defaultValue.Handle);

        return Z3Expr.Create<ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates constant array with integer index and default value for all indices.
    /// </summary>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="defaultValue">The default value for all array elements.</param>
    /// <returns>Array expression from integer with constant default value.</returns>
    public static ArrayExpr<IntExpr, TValue> Array<TValue>(this Z3Context context, TValue defaultValue)
        where TValue : Z3Expr, IExprType<TValue>
    {
        return context.Array<IntExpr, TValue>(defaultValue);
    }

    /// <summary>
    /// Creates array with value stored at specified index.
    /// </summary>
    /// <typeparam name="TIndex">Array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index">The index to store at.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>Array expression with value stored at index.</returns>
    public static ArrayExpr<TIndex, TValue> Store<TIndex, TValue>(
        this Z3Context context,
        ArrayExpr<TIndex, TValue> array,
        TIndex index,
        TValue value
    )
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkStore(context.Handle, array.Handle, index.Handle, value.Handle);
        return Z3Expr.Create<ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates expression selecting value at specified array index.
    /// </summary>
    /// <typeparam name="TIndex">Array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index">The index to select from.</param>
    /// <returns>Expression for value at the specified index.</returns>
    public static TValue Select<TIndex, TValue>(this Z3Context context, ArrayExpr<TIndex, TValue> array, TIndex index)
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkSelect(context.Handle, array.Handle, index.Handle);
        return Z3Expr.Create<TValue>(context, handle);
    }

    /// <summary>
    /// Creates expression selecting value at specified 2D array indices.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index1">The first index.</param>
    /// <param name="index2">The second index.</param>
    /// <returns>Expression for value at the specified indices.</returns>
    public static TValue Select<TIndex1, TIndex2, TValue>(
        this Z3Context context,
        ArrayExpr<TIndex1, TIndex2, TValue> array,
        TIndex1 index1,
        TIndex2 index2
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkSelectN(context.Handle, array.Handle, 2, new[] { index1.Handle, index2.Handle });
        return Z3Expr.Create<TValue>(context, handle);
    }

    /// <summary>
    /// Creates expression selecting value at specified 3D array indices.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TIndex3">Third array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index1">The first index.</param>
    /// <param name="index2">The second index.</param>
    /// <param name="index3">The third index.</param>
    /// <returns>Expression for value at the specified indices.</returns>
    public static TValue Select<TIndex1, TIndex2, TIndex3, TValue>(
        this Z3Context context,
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> array,
        TIndex1 index1,
        TIndex2 index2,
        TIndex3 index3
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TIndex3 : Z3Expr, IExprType<TIndex3>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkSelectN(
            context.Handle,
            array.Handle,
            3,
            new[] { index1.Handle, index2.Handle, index3.Handle }
        );
        return Z3Expr.Create<TValue>(context, handle);
    }

    /// <summary>
    /// Creates 2D array with value stored at specified indices.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index1">The first index to store at.</param>
    /// <param name="index2">The second index to store at.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>Array expression with value stored at indices.</returns>
    public static ArrayExpr<TIndex1, TIndex2, TValue> Store<TIndex1, TIndex2, TValue>(
        this Z3Context context,
        ArrayExpr<TIndex1, TIndex2, TValue> array,
        TIndex1 index1,
        TIndex2 index2,
        TValue value
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkStoreN(
            context.Handle,
            array.Handle,
            2,
            new[] { index1.Handle, index2.Handle },
            value.Handle
        );
        return Z3Expr.Create<ArrayExpr<TIndex1, TIndex2, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates 3D array with value stored at specified indices.
    /// </summary>
    /// <typeparam name="TIndex1">First array index type.</typeparam>
    /// <typeparam name="TIndex2">Second array index type.</typeparam>
    /// <typeparam name="TIndex3">Third array index type.</typeparam>
    /// <typeparam name="TValue">Array element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The array expression.</param>
    /// <param name="index1">The first index to store at.</param>
    /// <param name="index2">The second index to store at.</param>
    /// <param name="index3">The third index to store at.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>Array expression with value stored at indices.</returns>
    public static ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> Store<TIndex1, TIndex2, TIndex3, TValue>(
        this Z3Context context,
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> array,
        TIndex1 index1,
        TIndex2 index2,
        TIndex3 index3,
        TValue value
    )
        where TIndex1 : Z3Expr, IExprType<TIndex1>
        where TIndex2 : Z3Expr, IExprType<TIndex2>
        where TIndex3 : Z3Expr, IExprType<TIndex3>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = context.Library.MkStoreN(
            context.Handle,
            array.Handle,
            3,
            new[] { index1.Handle, index2.Handle, index3.Handle },
            value.Handle
        );
        return Z3Expr.Create<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>(context, handle);
    }
}
