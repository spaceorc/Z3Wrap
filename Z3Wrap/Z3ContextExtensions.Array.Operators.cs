using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
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
}