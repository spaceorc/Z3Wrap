using System.Numerics;
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

    // Z3IntExpr <-> BigInteger operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Lt(left, context.Int(right));
    public static Z3BoolExpr Lt(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Lt(context.Int(left), right);
    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Le(left, context.Int(right));
    public static Z3BoolExpr Le(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Le(context.Int(left), right);
    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Gt(left, context.Int(right));
    public static Z3BoolExpr Gt(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Gt(context.Int(left), right);
    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Ge(left, context.Int(right));
    public static Z3BoolExpr Ge(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Ge(context.Int(left), right);

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

    // Z3RealExpr <-> Real operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Real right) => context.Lt(left, context.Real(right));
    public static Z3BoolExpr Lt(this Z3Context context, Real left, Z3RealExpr right) => context.Lt(context.Real(left), right);
    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Real right) => context.Le(left, context.Real(right));
    public static Z3BoolExpr Le(this Z3Context context, Real left, Z3RealExpr right) => context.Le(context.Real(left), right);
    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Real right) => context.Gt(left, context.Real(right));
    public static Z3BoolExpr Gt(this Z3Context context, Real left, Z3RealExpr right) => context.Gt(context.Real(left), right);
    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Real right) => context.Ge(left, context.Real(right));
    public static Z3BoolExpr Ge(this Z3Context context, Real left, Z3RealExpr right) => context.Ge(context.Real(left), right);
}