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
        context.Library.Z3MkArraySort(context.Handle, TIndex.Sort(context), TValue.Sort(context));

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
