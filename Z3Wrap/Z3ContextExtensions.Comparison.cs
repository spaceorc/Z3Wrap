using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Z3IntExpr <-> Z3IntExpr operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    // Z3RealExpr <-> Z3RealExpr operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return context.WrapBoolExpr(resultHandle);
    }
}