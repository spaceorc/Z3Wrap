using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides extension methods for Z3Context to create arithmetic operations for numeric expressions.
/// Supports basic arithmetic operations (+, -, *, /, unary-) for integer and real expressions.
/// </summary>
public static class ArithmeticOperationsContextExtensions
{
    /// <summary>
    /// Adds multiple arithmetic expressions together (operand₁ + operand₂ + ... + operandₙ).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="operands">Arithmetic expressions to add.</param>
    /// <returns>Expression representing the sum of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
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
    /// Subtracts multiple arithmetic expressions with left-associative evaluation (((a - b) - c) - d).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="operands">Arithmetic expressions to subtract.</param>
    /// <returns>Expression representing the result of the subtraction.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
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
    /// Multiplies multiple arithmetic expressions together (operand₁ × operand₂ × ... × operandₙ).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="operands">Arithmetic expressions to multiply.</param>
    /// <returns>Expression representing the product of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
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
    /// Divides one arithmetic expression by another (left ÷ right).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Dividend.</param>
    /// <param name="right">Divisor.</param>
    /// <returns>Expression representing left divided by right.</returns>
    public static T Div<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Negates an arithmetic expression (-operand).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="operand">Arithmetic expression to negate.</param>
    /// <returns>Expression representing the negated value.</returns>
    public static T UnaryMinus<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Computes the modulo of two integer expressions (left mod right). Integer-specific operation.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Dividend.</param>
    /// <param name="right">Divisor.</param>
    /// <returns>IntExpr representing left modulo right.</returns>
    public static IntExpr Mod(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }
}
