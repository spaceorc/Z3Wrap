namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3BoolExpr Eq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }
    
    public static Z3BoolExpr Eq(this Z3Context context, Z3IntExpr left, int right) => context.Eq(left, context.Int(right));
    public static Z3BoolExpr Eq(this Z3Context context, int left, Z3IntExpr right) => context.Eq(context.Int(left), right);
    public static Z3BoolExpr Eq(this Z3Context context, Z3RealExpr left, double right) => context.Eq(left, context.Real(right));
    public static Z3BoolExpr Eq(this Z3Context context, double left, Z3RealExpr right) => context.Eq(context.Real(left), right);
    public static Z3BoolExpr Eq(this Z3Context context, Z3RealExpr left, int right) => context.Eq(left, context.Real(right));
    public static Z3BoolExpr Eq(this Z3Context context, int left, Z3RealExpr right) => context.Eq(context.Real(left), right);

    public static Z3BoolExpr Neq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        return context.Not(context.Eq(left, right));
    }
    
    public static Z3BoolExpr Neq(this Z3Context context, Z3IntExpr left, int right) => context.Neq(left, context.Int(right));
    public static Z3BoolExpr Neq(this Z3Context context, int left, Z3IntExpr right) => context.Neq(context.Int(left), right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3RealExpr left, double right) => context.Neq(left, context.Real(right));
    public static Z3BoolExpr Neq(this Z3Context context, double left, Z3RealExpr right) => context.Neq(context.Real(left), right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3RealExpr left, int right) => context.Neq(left, context.Real(right));
    public static Z3BoolExpr Neq(this Z3Context context, int left, Z3RealExpr right) => context.Neq(context.Real(left), right);
}