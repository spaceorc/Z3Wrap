using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Real arithmetic operations
    public static Z3RealExpr Add(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Sub(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Mul(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Div(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr UnaryMinus(this Z3Context context, Z3RealExpr operand)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3RealExpr.Create(context, resultHandle);
    }

    // Real comparison operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }
}