using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Z3IntExpr <-> Z3IntExpr operations
    public static Z3IntExpr Add(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, 2, args);
        return context.WrapIntExpr(resultHandle);
    }

    public static Z3IntExpr Sub(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(context.Handle, 2, args);
        return context.WrapIntExpr(resultHandle);
    }

    public static Z3IntExpr Mul(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(context.Handle, 2, args);
        return context.WrapIntExpr(resultHandle);
    }

    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return context.WrapIntExpr(resultHandle);
    }

    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return context.WrapIntExpr(resultHandle);
    }

    // Z3IntExpr <-> int operations
    public static Z3IntExpr Add(this Z3Context context, Z3IntExpr left, int right) => context.Add(left, context.Int(right));
    public static Z3IntExpr Add(this Z3Context context, int left, Z3IntExpr right) => context.Add(context.Int(left), right);
    public static Z3IntExpr Sub(this Z3Context context, Z3IntExpr left, int right) => context.Sub(left, context.Int(right));
    public static Z3IntExpr Sub(this Z3Context context, int left, Z3IntExpr right) => context.Sub(context.Int(left), right);
    public static Z3IntExpr Mul(this Z3Context context, Z3IntExpr left, int right) => context.Mul(left, context.Int(right));
    public static Z3IntExpr Mul(this Z3Context context, int left, Z3IntExpr right) => context.Mul(context.Int(left), right);
    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, int right) => context.Div(left, context.Int(right));
    public static Z3IntExpr Div(this Z3Context context, int left, Z3IntExpr right) => context.Div(context.Int(left), right);
    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, int right) => context.Mod(left, context.Int(right));
    public static Z3IntExpr Mod(this Z3Context context, int left, Z3IntExpr right) => context.Mod(context.Int(left), right);

    // Z3RealExpr <-> Z3RealExpr operations
    public static Z3RealExpr Add(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, 2, args);
        return context.WrapRealExpr(resultHandle);
    }

    public static Z3RealExpr Sub(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(context.Handle, 2, args);
        return context.WrapRealExpr(resultHandle);
    }

    public static Z3RealExpr Mul(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(context.Handle, 2, args);
        return context.WrapRealExpr(resultHandle);
    }

    public static Z3RealExpr Div(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return context.WrapRealExpr(resultHandle);
    }

    // Z3RealExpr <-> double operations
    public static Z3RealExpr Add(this Z3Context context, Z3RealExpr left, double right) => context.Add(left, context.Real(right));
    public static Z3RealExpr Add(this Z3Context context, double left, Z3RealExpr right) => context.Add(context.Real(left), right);
    public static Z3RealExpr Sub(this Z3Context context, Z3RealExpr left, double right) => context.Sub(left, context.Real(right));
    public static Z3RealExpr Sub(this Z3Context context, double left, Z3RealExpr right) => context.Sub(context.Real(left), right);
    public static Z3RealExpr Mul(this Z3Context context, Z3RealExpr left, double right) => context.Mul(left, context.Real(right));
    public static Z3RealExpr Mul(this Z3Context context, double left, Z3RealExpr right) => context.Mul(context.Real(left), right);
    public static Z3RealExpr Div(this Z3Context context, Z3RealExpr left, double right) => context.Div(left, context.Real(right));
    public static Z3RealExpr Div(this Z3Context context, double left, Z3RealExpr right) => context.Div(context.Real(left), right);

    // Unary operations
    public static Z3IntExpr UnaryMinus(this Z3Context context, Z3IntExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(context.Handle, expr.Handle);
        return context.WrapIntExpr(resultHandle);
    }

    public static Z3RealExpr UnaryMinus(this Z3Context context, Z3RealExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(context.Handle, expr.Handle);
        return context.WrapRealExpr(resultHandle);
    }

    public static Z3IntExpr Abs(this Z3Context context, Z3IntExpr expr) => context.Ite(context.Lt(expr, 0), context.UnaryMinus(expr), expr);
    public static Z3RealExpr Abs(this Z3Context context, Z3RealExpr expr) => context.Ite(context.Lt(expr, 0), context.UnaryMinus(expr), expr);
}