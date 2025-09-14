using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // If-then-else operation
    public static T Ite<T>(this Z3Context context, Z3BoolExpr condition, T thenExpr, T elseExpr) where T : Z3Expr
    {
        var resultHandle = NativeMethods.Z3MkIte(context.Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return (T)Z3Expr.Create(context, resultHandle);
    }
}