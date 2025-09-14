using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3ArrayExpr<TIndex, TValue> ArrayConst<TIndex, TValue>(this Z3Context context, string name)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var indexSort = GetSortForType<TIndex>(context);
        var valueSort = GetSortForType<TValue>(context);

        var arraySort = NativeMethods.Z3MkArraySort(context.Handle, indexSort, valueSort);

        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, arraySort);

        return Z3ArrayExpr<TIndex, TValue>.Create(context, handle);
    }

    // Type-inferred overload for common array pattern (Int index by default)
    public static Z3ArrayExpr<Z3IntExpr, TValue> ArrayConst<TValue>(this Z3Context context, string name)
        where TValue : Z3Expr
    {
        return context.ArrayConst<Z3IntExpr, TValue>(name);
    }

    public static Z3ArrayExpr<TIndex, TValue> Array<TIndex, TValue>(this Z3Context context, TValue defaultValue)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var indexSort = GetSortForType<TIndex>(context);
        var valueSort = GetSortForType<TValue>(context);

        var arraySort = NativeMethods.Z3MkArraySort(context.Handle, indexSort, valueSort);
        var handle = NativeMethods.Z3MkConstArray(context.Handle, indexSort, defaultValue.Handle);

        return Z3ArrayExpr<TIndex, TValue>.Create(context, handle);
    }

    // Type-inferred overload for common array pattern (Int index by default)
    public static Z3ArrayExpr<Z3IntExpr, TValue> Array<TValue>(this Z3Context context, TValue defaultValue)
        where TValue : Z3Expr
    {
        return context.Array<Z3IntExpr, TValue>(defaultValue);
    }

    public static Z3ArrayExpr<TIndex, TValue> Store<TIndex, TValue>(this Z3Context context, Z3ArrayExpr<TIndex, TValue> array, TIndex index, TValue value)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var handle = NativeMethods.Z3MkStore(context.Handle, array.Handle, index.Handle, value.Handle);
        return Z3ArrayExpr<TIndex, TValue>.Create(context, handle);
    }

    public static TValue Select<TIndex, TValue>(this Z3Context context, Z3ArrayExpr<TIndex, TValue> array, TIndex index)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var handle = NativeMethods.Z3MkSelect(context.Handle, array.Handle, index.Handle);
        return (TValue)Z3Expr.Create(context, handle);
    }

    private static IntPtr GetSortForType<T>(Z3Context context) where T : Z3Expr
    {
        return typeof(T).Name switch
        {
            nameof(Z3BoolExpr) => NativeMethods.Z3MkBoolSort(context.Handle),
            nameof(Z3IntExpr) => NativeMethods.Z3MkIntSort(context.Handle),
            nameof(Z3RealExpr) => NativeMethods.Z3MkRealSort(context.Handle),
            _ => throw new ArgumentException($"Unsupported array type: {typeof(T).Name}")
        };
    }
}