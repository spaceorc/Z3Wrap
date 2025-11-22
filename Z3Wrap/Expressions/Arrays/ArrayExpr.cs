using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Arrays;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)

/// <summary>
/// Represents array expression with typed index and value for Z3 solving.
/// </summary>
/// <typeparam name="TIndex">The array index type.</typeparam>
/// <typeparam name="TValue">The array element type.</typeparam>
public sealed class ArrayExpr<TIndex, TValue> : Z3Expr, IExprType<ArrayExpr<TIndex, TValue>>
    where TIndex : Z3Expr, IExprType<TIndex>
    where TValue : Z3Expr, IExprType<TValue>
{
    private ArrayExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static ArrayExpr<TIndex, TValue> IExprType<ArrayExpr<TIndex, TValue>>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<ArrayExpr<TIndex, TValue>>.Sort(Z3Context context) =>
        context.Library.MkArraySort(context.Handle, TIndex.Sort(context), TValue.Sort(context));

    /// <summary>
    /// Gets the array element at the specified index.
    /// </summary>
    /// <param name="index">The array index.</param>
    /// <returns>The value at the specified index.</returns>
    public TValue this[TIndex index] => Context.Select(this, index);

    /// <summary>
    /// Creates a new array with the specified index updated to the given value.
    /// </summary>
    /// <param name="index">The array index to update.</param>
    /// <param name="value">The new value to store.</param>
    /// <returns>Array expression with updated value at index.</returns>
    public ArrayExpr<TIndex, TValue> Store(TIndex index, TValue value) => Context.Store(this, index, value);

    /// <summary>
    /// Equality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator ==(ArrayExpr<TIndex, TValue> left, ArrayExpr<TIndex, TValue> right) =>
        left.Eq(right);

    /// <summary>
    /// Inequality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator !=(ArrayExpr<TIndex, TValue> left, ArrayExpr<TIndex, TValue> right) =>
        left.Neq(right);
}

/// <summary>
/// Represents 2-dimensional array expression with typed indices and value for Z3 solving.
/// </summary>
/// <typeparam name="TIndex1">The first array index type.</typeparam>
/// <typeparam name="TIndex2">The second array index type.</typeparam>
/// <typeparam name="TValue">The array element type.</typeparam>
public sealed class ArrayExpr<TIndex1, TIndex2, TValue> : Z3Expr, IExprType<ArrayExpr<TIndex1, TIndex2, TValue>>
    where TIndex1 : Z3Expr, IExprType<TIndex1>
    where TIndex2 : Z3Expr, IExprType<TIndex2>
    where TValue : Z3Expr, IExprType<TValue>
{
    private ArrayExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static ArrayExpr<TIndex1, TIndex2, TValue> IExprType<ArrayExpr<TIndex1, TIndex2, TValue>>.Create(
        Z3Context context,
        IntPtr handle
    ) => new(context, handle);

    static IntPtr IExprType<ArrayExpr<TIndex1, TIndex2, TValue>>.Sort(Z3Context context)
    {
        var domains = new[] { TIndex1.Sort(context), TIndex2.Sort(context) };
        return context.Library.MkArraySortN(context.Handle, 2, domains, TValue.Sort(context));
    }

    /// <summary>
    /// Gets the array element at the specified indices.
    /// </summary>
    /// <param name="index1">The first array index.</param>
    /// <param name="index2">The second array index.</param>
    /// <returns>The value at the specified indices.</returns>
    public TValue this[TIndex1 index1, TIndex2 index2] => Context.Select(this, index1, index2);

    /// <summary>
    /// Creates a new array with the specified indices updated to the given value.
    /// </summary>
    /// <param name="index1">The first array index to update.</param>
    /// <param name="index2">The second array index to update.</param>
    /// <param name="value">The new value to store.</param>
    /// <returns>Array expression with updated value at indices.</returns>
    public ArrayExpr<TIndex1, TIndex2, TValue> Store(TIndex1 index1, TIndex2 index2, TValue value) =>
        Context.Store(this, index1, index2, value);

    /// <summary>
    /// Equality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator ==(
        ArrayExpr<TIndex1, TIndex2, TValue> left,
        ArrayExpr<TIndex1, TIndex2, TValue> right
    ) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator !=(
        ArrayExpr<TIndex1, TIndex2, TValue> left,
        ArrayExpr<TIndex1, TIndex2, TValue> right
    ) => left.Neq(right);
}

/// <summary>
/// Represents 3-dimensional array expression with typed indices and value for Z3 solving.
/// </summary>
/// <typeparam name="TIndex1">The first array index type.</typeparam>
/// <typeparam name="TIndex2">The second array index type.</typeparam>
/// <typeparam name="TIndex3">The third array index type.</typeparam>
/// <typeparam name="TValue">The array element type.</typeparam>
public sealed class ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>
    : Z3Expr,
        IExprType<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>
    where TIndex1 : Z3Expr, IExprType<TIndex1>
    where TIndex2 : Z3Expr, IExprType<TIndex2>
    where TIndex3 : Z3Expr, IExprType<TIndex3>
    where TValue : Z3Expr, IExprType<TValue>
{
    private ArrayExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> IExprType<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>.Create(
        Z3Context context,
        IntPtr handle
    ) => new(context, handle);

    static IntPtr IExprType<ArrayExpr<TIndex1, TIndex2, TIndex3, TValue>>.Sort(Z3Context context)
    {
        var domains = new[] { TIndex1.Sort(context), TIndex2.Sort(context), TIndex3.Sort(context) };
        return context.Library.MkArraySortN(context.Handle, 3, domains, TValue.Sort(context));
    }

    /// <summary>
    /// Gets the array element at the specified indices.
    /// </summary>
    /// <param name="index1">The first array index.</param>
    /// <param name="index2">The second array index.</param>
    /// <param name="index3">The third array index.</param>
    /// <returns>The value at the specified indices.</returns>
    public TValue this[TIndex1 index1, TIndex2 index2, TIndex3 index3] => Context.Select(this, index1, index2, index3);

    /// <summary>
    /// Creates a new array with the specified indices updated to the given value.
    /// </summary>
    /// <param name="index1">The first array index to update.</param>
    /// <param name="index2">The second array index to update.</param>
    /// <param name="index3">The third array index to update.</param>
    /// <param name="value">The new value to store.</param>
    /// <returns>Array expression with updated value at indices.</returns>
    public ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> Store(
        TIndex1 index1,
        TIndex2 index2,
        TIndex3 index3,
        TValue value
    ) => Context.Store(this, index1, index2, index3, value);

    /// <summary>
    /// Equality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator ==(
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> left,
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> right
    ) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two array expressions.
    /// </summary>
    public static BoolExpr operator !=(
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> left,
        ArrayExpr<TIndex1, TIndex2, TIndex3, TValue> right
    ) => left.Neq(right);
}
