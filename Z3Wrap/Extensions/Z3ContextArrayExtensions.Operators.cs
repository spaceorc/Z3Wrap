using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextArrayExtensions
{
    /// <summary>
    /// Creates a new array that is the same as the given array except that the value at the specified index is updated.
    /// </summary>
    /// <typeparam name="TIndex">The type of array index expressions.</typeparam>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The source array.</param>
    /// <param name="index">The index to update.</param>
    /// <param name="value">The new value at the specified index.</param>
    /// <returns>A new Z3ArrayExpr with the updated value at the specified index.</returns>
    public static Z3ArrayExpr<TIndex, TValue> Store<TIndex, TValue>(
        this Z3Context context,
        Z3ArrayExpr<TIndex, TValue> array,
        TIndex index,
        TValue value
    )
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = SafeNativeMethods.Z3MkStore(
            context.Handle,
            array.Handle,
            index.Handle,
            value.Handle
        );
        return Z3Expr.Create<Z3ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Selects the value at the specified index from an array.
    /// </summary>
    /// <typeparam name="TIndex">The type of array index expressions.</typeparam>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="array">The source array.</param>
    /// <param name="index">The index to select from.</param>
    /// <returns>The value at the specified index in the array.</returns>
    public static TValue Select<TIndex, TValue>(
        this Z3Context context,
        Z3ArrayExpr<TIndex, TValue> array,
        TIndex index
    )
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = SafeNativeMethods.Z3MkSelect(context.Handle, array.Handle, index.Handle);
        return Z3Expr.Create<TValue>(context, handle);
    }
}
