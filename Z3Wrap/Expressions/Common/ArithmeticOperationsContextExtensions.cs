using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides basic arithmetic operation methods for Z3Context.
/// </summary>
public static class ArithmeticOperationsContextExtensions
{
    /// <summary>
    /// Creates addition expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to add.</param>
    /// <returns>Expression representing sum of operands.</returns>
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

    /// <summary>
    /// Creates subtraction expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to subtract.</param>
    /// <returns>Expression representing sequential subtraction of operands.</returns>
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

    /// <summary>
    /// Creates multiplication expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to multiply.</param>
    /// <returns>Expression representing product of operands.</returns>
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

    /// <summary>
    /// Creates division expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>Expression representing left / right.</returns>
    public static T Div<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Creates unary minus expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>Expression representing -operand.</returns>
    public static T UnaryMinus<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

}
