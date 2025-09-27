using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public static class ArithmeticOperationsContextExtensions
{
    public static T Add<T>(this Z3Context context, params T[] operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Add requires at least one operand. Z3 does not support empty addition."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAdd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    public static T Sub<T>(this Z3Context context, params T[] operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Sub requires at least one operand. Z3 does not support empty subtraction."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkSub(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    public static T Mul<T>(this Z3Context context, params T[] operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Mul requires at least one operand. Z3 does not support empty multiplication."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkMul(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    public static T Div<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    public static T UnaryMinus<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    public static IntExpr Mod(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }
}
