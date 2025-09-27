using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class RealExprContextExtensions
{
    /// <summary>
    /// Adds multiple real expressions together.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The real expressions to add together.</param>
    /// <returns>A Z3Real representing the sum of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static RealExpr Add(this Z3Context context, params RealExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Add requires at least one operand. Z3 does not support empty addition."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAdd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RealExpr>(context, resultHandle);
    }

    /// <summary>
    /// Subtracts multiple real expressions. For multiple operands, performs left-associative subtraction.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The real expressions to subtract. The first operand is the minuend, subsequent operands are subtracted from it.</param>
    /// <returns>A Z3Real representing the result of the subtraction.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static RealExpr Sub(this Z3Context context, params RealExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Sub requires at least one operand. Z3 does not support empty subtraction."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkSub(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RealExpr>(context, resultHandle);
    }

    /// <summary>
    /// Multiplies multiple real expressions together.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The real expressions to multiply together.</param>
    /// <returns>A Z3Real representing the product of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static RealExpr Mul(this Z3Context context, params RealExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Mul requires at least one operand. Z3 does not support empty multiplication."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkMul(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RealExpr>(context, resultHandle);
    }

    /// <summary>
    /// Divides one real expression by another.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend (numerator).</param>
    /// <param name="right">The divisor (denominator).</param>
    /// <returns>A Z3Real representing the quotient of left divided by right.</returns>
    public static RealExpr Div(this Z3Context context, RealExpr left, RealExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<RealExpr>(context, resultHandle);
    }

    /// <summary>
    /// Negates a real expression (unary minus operation).
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The real expression to negate.</param>
    /// <returns>A Z3Real representing the negated value of the operand.</returns>
    public static RealExpr UnaryMinus(this Z3Context context, RealExpr operand)
    {
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<RealExpr>(context, resultHandle);
    }
}
