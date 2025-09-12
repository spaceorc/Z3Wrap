namespace Z3Wrap.Expressions;

public class Z3ArrayExpr<TIndex, TValue>(Z3Context context, IntPtr handle) : Z3Expr(context, handle)
    where TIndex : Z3Expr
    where TValue : Z3Expr
{
    public TValue this[TIndex index] => Context.Select(this, index);

    public Z3ArrayExpr<TIndex, TValue> Store(TIndex index, TValue value) => Context.Store(this, index, value);
}