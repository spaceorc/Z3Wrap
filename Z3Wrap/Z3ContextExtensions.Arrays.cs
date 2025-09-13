using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3ArrayExpr<TIndex, TValue> ArrayConst<TIndex, TValue>(this Z3Context context, string name)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var domainSort = GetSortForType<TIndex>(context);
        var rangeSort = GetSortForType<TValue>(context);
        var arraySort = NativeMethods.Z3MkArraySort(context.Handle, domainSort, rangeSort);

        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, arraySort);

        return context.WrapArrayExpr<TIndex, TValue>(handle);
    }

    public static Z3ArrayExpr<Z3IntExpr, TValue> ArrayConst<TValue>(this Z3Context context, string name)
        where TValue : Z3Expr
    {
        return context.ArrayConst<Z3IntExpr, TValue>(name);
    }

    public static Z3ArrayExpr<TIndex, TValue> Array<TIndex, TValue>(this Z3Context context, TValue defaultValue)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var domainSort = GetSortForType<TIndex>(context);
        var handle = NativeMethods.Z3MkConstArray(context.Handle, domainSort, defaultValue.Handle);

        return context.WrapArrayExpr<TIndex, TValue>(handle);
    }

    public static Z3ArrayExpr<Z3IntExpr, TValue> Array<TValue>(this Z3Context context, TValue defaultValue)
        where TValue : Z3Expr
    {
        return context.Array<Z3IntExpr, TValue>(defaultValue);
    }

    public static TValue Select<TIndex, TValue>(this Z3Context context, Z3ArrayExpr<TIndex, TValue> array, TIndex index)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var handle = NativeMethods.Z3MkSelect(context.Handle, array.Handle, index.Handle);
        return context.WrapExprForType<TValue>(handle);
    }

    public static Z3ArrayExpr<TIndex, TValue> Store<TIndex, TValue>(this Z3Context context, Z3ArrayExpr<TIndex, TValue> array, TIndex index, TValue value)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        var handle = NativeMethods.Z3MkStore(context.Handle, array.Handle, index.Handle, value.Handle);
        return context.WrapArrayExpr<TIndex, TValue>(handle);
    }

    private static IntPtr GetSortForType<T>(Z3Context context) where T : Z3Expr
    {
        return typeof(T).Name switch
        {
            nameof(Z3BoolExpr) => NativeMethods.Z3MkBoolSort(context.Handle),
            nameof(Z3IntExpr) => NativeMethods.Z3MkIntSort(context.Handle),
            nameof(Z3RealExpr) => NativeMethods.Z3MkRealSort(context.Handle),
            _ => throw new InvalidOperationException($"Unsupported expression type: {typeof(T).Name}")
        };
    }

    private static T WrapExprForType<T>(this Z3Context context, IntPtr handle) where T : Z3Expr
    {
        return typeof(T).Name switch
        {
            nameof(Z3BoolExpr) => (T)(object)context.WrapBoolExpr(handle),
            nameof(Z3IntExpr) => (T)(object)context.WrapIntExpr(handle),
            nameof(Z3RealExpr) => (T)(object)context.WrapRealExpr(handle),
            _ => throw new InvalidOperationException($"Unsupported expression type: {typeof(T).Name}")
        };
    }
}