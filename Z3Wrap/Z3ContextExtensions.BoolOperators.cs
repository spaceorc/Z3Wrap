namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3BoolExpr And(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAnd(context.Handle, 2, args);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Or(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkOr(context.Handle, 2, args);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Not(this Z3Context context, Z3BoolExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkNot(context.Handle, expr.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Xor(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Implies(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Iff(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static T Ite<T>(this Z3Context context, Z3BoolExpr condition, T thenExpr, T elseExpr) where T : Z3Expr
    {
        return (T)context.Ite(condition, (Z3Expr)thenExpr, elseExpr);
    }

    public static Z3Expr Ite(this Z3Context context, Z3BoolExpr condition, Z3Expr thenExpr, Z3Expr elseExpr)
    {
        var resultHandle = NativeMethods.Z3MkIte(context.Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return context.WrapExpr(resultHandle);
    }
}