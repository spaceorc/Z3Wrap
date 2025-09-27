using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Arrays;

public class ArrayExpr<TIndex, TValue> : Z3Expr, IExprType<ArrayExpr<TIndex, TValue>>
    where TIndex : Z3Expr, IExprType<TIndex>
    where TValue : Z3Expr, IExprType<TValue>
{
    private ArrayExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static ArrayExpr<TIndex, TValue> IExprType<ArrayExpr<TIndex, TValue>>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<ArrayExpr<TIndex, TValue>>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkArraySort(context.Handle, TIndex.Sort(context), TValue.Sort(context));

    public TValue this[TIndex index] => Context.Select(this, index);

    public ArrayExpr<TIndex, TValue> Store(TIndex index, TValue value) => Context.Store(this, index, value);

    public static BoolExpr operator ==(ArrayExpr<TIndex, TValue> left, ArrayExpr<TIndex, TValue> right) =>
        left.Eq(right);

    public static BoolExpr operator !=(ArrayExpr<TIndex, TValue> left, ArrayExpr<TIndex, TValue> right) =>
        left.Neq(right);
}
