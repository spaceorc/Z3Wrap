using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions;

/// <summary>
/// Represents a typed array expression in Z3 with strongly-typed index and value types.
/// Provides natural indexing syntax and functional update operations for working with Z3 array theory.
/// </summary>
/// <typeparam name="TIndex">The type of expressions used for array indices.</typeparam>
/// <typeparam name="TValue">The type of expressions used for array values.</typeparam>
public class Z3ArrayExpr<TIndex, TValue> : Z3Expr, IZ3ExprType<Z3ArrayExpr<TIndex, TValue>>
    where TIndex : Z3Expr, IZ3ExprType<TIndex>
    where TValue : Z3Expr, IZ3ExprType<TValue>
{
    private Z3ArrayExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3ArrayExpr<TIndex, TValue> IZ3ExprType<Z3ArrayExpr<TIndex, TValue>>.Create(
        Z3Context context,
        IntPtr handle
    ) => new(context, handle);

    static IntPtr IZ3ExprType<Z3ArrayExpr<TIndex, TValue>>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkArraySort(
            context.Handle,
            TIndex.GetSort(context),
            TValue.GetSort(context)
        );

    /// <summary>
    /// Gets the value at the specified index using natural indexing syntax.
    /// Equivalent to the Z3 array select operation.
    /// </summary>
    /// <param name="index">The index expression.</param>
    /// <returns>The value expression at the given index.</returns>
    public TValue this[TIndex index] => Context.Select(this, index);

    /// <summary>
    /// Creates a new array expression with the value at the specified index updated.
    /// This is a functional update that does not modify the original array.
    /// </summary>
    /// <param name="index">The index where the value should be stored.</param>
    /// <param name="value">The value to store at the index.</param>
    /// <returns>A new array expression with the updated value.</returns>
    public Z3ArrayExpr<TIndex, TValue> Store(TIndex index, TValue value) =>
        Context.Store(this, index, value);
}
