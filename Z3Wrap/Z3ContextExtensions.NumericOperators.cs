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

    public static Z3IntExpr Abs(this Z3Context context, Z3IntExpr expr) => context.Ite(expr < 0, context.UnaryMinus(expr), expr);
    public static Z3RealExpr Abs(this Z3Context context, Z3RealExpr expr) => context.Ite(expr < 0, context.UnaryMinus(expr), expr);

    // Type conversion operations
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return context.WrapRealExpr(resultHandle);
    }

    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return context.WrapIntExpr(resultHandle);
    }
}