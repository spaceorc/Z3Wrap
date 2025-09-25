namespace Spaceorc.Z3Wrap.Expressions;

public interface IZ3ExprType<out T>
    where T : Z3Expr
{
    internal static abstract T Create(Z3Context context, IntPtr handle);
    internal static abstract IntPtr GetSort(Z3Context context);
}
