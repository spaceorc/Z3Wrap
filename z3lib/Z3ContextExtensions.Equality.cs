namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3BoolExpr Eq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Neq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        return context.Not(context.Eq(left, right));
    }
}