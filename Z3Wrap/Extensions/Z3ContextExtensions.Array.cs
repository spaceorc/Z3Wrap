using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
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
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var indexSort = GetSortForType<TIndex>(context);
        var valueSort = GetSortForType<TValue>(context);

        var arraySort = SafeNativeMethods.Z3MkArraySort(context.Handle, indexSort, valueSort);

        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, arraySort);

        return Z3ArrayExpr<TIndex, TValue>.Create(context, handle);
    }

    /// <summary>
    /// Creates a new array constant (variable) with integer indices and the specified value type.
    /// </summary>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the array constant.</param>
    /// <returns>A new Z3ArrayExpr with integer indices representing the array constant.</returns>
    public static Z3ArrayExpr<Z3IntExpr, TValue> ArrayConst<TValue>(
        this Z3Context context,
        string name
    )
        where TValue : Z3Expr
    {
        return context.ArrayConst<Z3IntExpr, TValue>(name);
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
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var indexSort = GetSortForType<TIndex>(context);
        var valueSort = GetSortForType<TValue>(context);

        var arraySort = SafeNativeMethods.Z3MkArraySort(context.Handle, indexSort, valueSort);
        var handle = SafeNativeMethods.Z3MkConstArray(
            context.Handle,
            indexSort,
            defaultValue.Handle
        );

        return Z3ArrayExpr<TIndex, TValue>.Create(context, handle);
    }

    /// <summary>
    /// Creates a constant array with integer indices where all indices map to the same default value.
    /// </summary>
    /// <typeparam name="TValue">The type of array value expressions.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="defaultValue">The default value for all array indices.</param>
    /// <returns>A new Z3ArrayExpr with integer indices representing the constant array.</returns>
    public static Z3ArrayExpr<Z3IntExpr, TValue> Array<TValue>(
        this Z3Context context,
        TValue defaultValue
    )
        where TValue : Z3Expr
    {
        return context.Array<Z3IntExpr, TValue>(defaultValue);
    }

    private static IntPtr GetSortForType<T>(Z3Context context)
        where T : Z3Expr
    {
        return typeof(T).Name switch
        {
            nameof(Z3BoolExpr) => SafeNativeMethods.Z3MkBoolSort(context.Handle),
            nameof(Z3IntExpr) => SafeNativeMethods.Z3MkIntSort(context.Handle),
            nameof(Z3RealExpr) => SafeNativeMethods.Z3MkRealSort(context.Handle),
            _ => throw new ArgumentException($"Unsupported array type: {typeof(T).Name}"),
        };
    }
}
