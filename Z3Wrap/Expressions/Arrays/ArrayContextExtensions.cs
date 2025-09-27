using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Arrays;

public static class ArrayContextExtensions
{
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

    public static ArrayExpr<IntExpr, TValue> ArrayConst<TValue>(this Z3Context context, string name)
        where TValue : Z3Expr, IExprType<TValue>
    {
        return context.ArrayConst<IntExpr, TValue>(name);
    }

    public static ArrayExpr<TIndex, TValue> Array<TIndex, TValue>(this Z3Context context, TValue defaultValue)
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var indexSort = context.GetSortForType<TIndex>();

        var handle = SafeNativeMethods.Z3MkConstArray(context.Handle, indexSort, defaultValue.Handle);

        return Z3Expr.Create<ArrayExpr<TIndex, TValue>>(context, handle);
    }

    public static ArrayExpr<IntExpr, TValue> Array<TValue>(this Z3Context context, TValue defaultValue)
        where TValue : Z3Expr, IExprType<TValue>
    {
        return context.Array<IntExpr, TValue>(defaultValue);
    }

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

    public static TValue Select<TIndex, TValue>(this Z3Context context, ArrayExpr<TIndex, TValue> array, TIndex index)
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        var handle = SafeNativeMethods.Z3MkSelect(context.Handle, array.Handle, index.Handle);
        return Z3Expr.Create<TValue>(context, handle);
    }
}
