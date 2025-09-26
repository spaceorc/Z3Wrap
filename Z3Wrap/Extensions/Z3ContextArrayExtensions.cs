using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.IntTheory;

namespace Spaceorc.Z3Wrap.Extensions;

/// <summary>
/// Provides extension methods for Z3Context to work with array expressions and constants.
/// Arrays in Z3 are functions from indices to values with support for select and store operations.
/// </summary>
public static partial class Z3ContextArrayExtensions
{
    /// <summary>
    /// Creates a new array constant (variable) with the specified name and generic index/value types.
    /// </summary>
    /// <typeparam name="TIndex">The type of array index expressions.</typeparam>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the array constant.</param>
    /// <returns>A new Z3ArrayExpr representing the array constant.</returns>
    public static Z3ArrayExpr<TIndex, TValue> ArrayConst<TIndex, TValue>(
        this Z3Context context,
        string name
    )
        where TIndex : Z3Expr, IZ3ExprType<TIndex>
        where TValue : Z3Expr, IZ3ExprType<TValue>
    {
        var arraySort = context.GetSortForType<Z3ArrayExpr<TIndex, TValue>>();

        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, arraySort);

        return Z3Expr.Create<Z3ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates a new array constant (variable) with integer indices and the specified value type.
    /// </summary>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the array constant.</param>
    /// <returns>A new Z3ArrayExpr with integer indices representing the array constant.</returns>
    public static Z3ArrayExpr<Z3Int, TValue> ArrayConst<TValue>(
        this Z3Context context,
        string name
    )
        where TValue : Z3Expr, IZ3ExprType<TValue>
    {
        return context.ArrayConst<Z3Int, TValue>(name);
    }

    /// <summary>
    /// Creates a constant array where all indices map to the same default value.
    /// </summary>
    /// <typeparam name="TIndex">The type of array index expressions.</typeparam>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="defaultValue">The default value for all array indices.</param>
    /// <returns>A new Z3ArrayExpr representing the constant array.</returns>
    public static Z3ArrayExpr<TIndex, TValue> Array<TIndex, TValue>(
        this Z3Context context,
        TValue defaultValue
    )
        where TIndex : Z3Expr, IZ3ExprType<TIndex>
        where TValue : Z3Expr, IZ3ExprType<TValue>
    {
        var indexSort = context.GetSortForType<TIndex>();

        var handle = SafeNativeMethods.Z3MkConstArray(
            context.Handle,
            indexSort,
            defaultValue.Handle
        );

        return Z3Expr.Create<Z3ArrayExpr<TIndex, TValue>>(context, handle);
    }

    /// <summary>
    /// Creates a constant array with integer indices where all indices map to the same default value.
    /// </summary>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="defaultValue">The default value for all array indices.</param>
    /// <returns>A new Z3ArrayExpr with integer indices representing the constant array.</returns>
    public static Z3ArrayExpr<Z3Int, TValue> Array<TValue>(
        this Z3Context context,
        TValue defaultValue
    )
        where TValue : Z3Expr, IZ3ExprType<TValue>
    {
        return context.Array<Z3Int, TValue>(defaultValue);
    }
}
