namespace Spaceorc.Z3Wrap.Expressions;

internal interface IZ3ExprCreate<out T>
    where T : Z3Expr
{
    static abstract T Create(Z3Context context, IntPtr handle);
    static abstract IntPtr GetSort(Z3Context context);
}
