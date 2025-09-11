namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, int right) => context.Lt(left, context.Int(right));
    public static Z3BoolExpr Lt(this Z3Context context, int left, Z3IntExpr right) => context.Lt(context.Int(left), right);

    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, double right) => context.Lt(left, context.Real(right));
    public static Z3BoolExpr Lt(this Z3Context context, double left, Z3RealExpr right) => context.Lt(context.Real(left), right);
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, int right) => context.Lt(left, context.Real(right));
    public static Z3BoolExpr Lt(this Z3Context context, int left, Z3RealExpr right) => context.Lt(context.Real(left), right);

    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, int right) => context.Le(left, context.Int(right));
    public static Z3BoolExpr Le(this Z3Context context, int left, Z3IntExpr right) => context.Le(context.Int(left), right);

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, double right) => context.Le(left, context.Real(right));
    public static Z3BoolExpr Le(this Z3Context context, double left, Z3RealExpr right) => context.Le(context.Real(left), right);
    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, int right) => context.Le(left, context.Real(right));
    public static Z3BoolExpr Le(this Z3Context context, int left, Z3RealExpr right) => context.Le(context.Real(left), right);

    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, int right) => context.Gt(left, context.Int(right));
    public static Z3BoolExpr Gt(this Z3Context context, int left, Z3IntExpr right) => context.Gt(context.Int(left), right);

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, double right) => context.Gt(left, context.Real(right));
    public static Z3BoolExpr Gt(this Z3Context context, double left, Z3RealExpr right) => context.Gt(context.Real(left), right);
    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, int right) => context.Gt(left, context.Real(right));
    public static Z3BoolExpr Gt(this Z3Context context, int left, Z3RealExpr right) => context.Gt(context.Real(left), right);
    
    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, int right) => context.Ge(left, context.Int(right));
    public static Z3BoolExpr Ge(this Z3Context context, int left, Z3IntExpr right) => context.Ge(context.Int(left), right);

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, double right) => context.Ge(left, context.Real(right));
    public static Z3BoolExpr Ge(this Z3Context context, double left, Z3RealExpr right) => context.Ge(context.Real(left), right);
    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, int right) => context.Ge(left, context.Real(right));
    public static Z3BoolExpr Ge(this Z3Context context, int left, Z3RealExpr right) => context.Ge(context.Real(left), right);
}