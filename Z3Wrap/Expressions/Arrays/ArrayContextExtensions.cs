using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
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

        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, arraySort);

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

        var handle = SafeNativeMethods.Z3MkConstArray(context.Handle, indexSort, defaultValue.Handle);

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
        var handle = SafeNativeMethods.Z3MkStore(context.Handle, array.Handle, index.Handle, value.Handle);
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
        var handle = SafeNativeMethods.Z3MkSelect(context.Handle, array.Handle, index.Handle);
        return Z3Expr.Create<TValue>(context, handle);
    }
}
