using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public static class EqualityContextExtensions
{
    public static BoolExpr Eq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Neq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IExprType<T>
    {
        var eqHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
