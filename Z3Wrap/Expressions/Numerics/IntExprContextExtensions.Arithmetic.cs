using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class IntExprContextExtensions
{
    /// <summary>
    /// Adds multiple integer expressions together.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to add together.</param>
    /// <returns>A Z3Int representing the sum of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static IntExpr Add(this Z3Context context, params IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Add requires at least one operand. Z3 does not support empty addition."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAdd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }

    /// <summary>
    /// Subtracts multiple integer expressions. For multiple operands, performs left-associative subtraction.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to subtract. The first operand is the minuend, subsequent operands are subtracted from it.</param>
    /// <returns>A Z3Int representing the result of the subtraction.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static IntExpr Sub(this Z3Context context, params IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Sub requires at least one operand. Z3 does not support empty subtraction."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkSub(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }

    /// <summary>
    /// Multiplies multiple integer expressions together.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to multiply together.</param>
    /// <returns>A Z3Int representing the product of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static IntExpr Mul(this Z3Context context, params IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Mul requires at least one operand. Z3 does not support empty multiplication."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkMul(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }

    /// <summary>
    /// Divides one integer expression by another (integer division).
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>A Z3Int representing the quotient of left divided by right.</returns>
    public static IntExpr Div(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }

    /// <summary>
    /// Computes the modulo of one integer expression with another.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>A Z3Int representing left modulo right.</returns>
    public static IntExpr Mod(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }

    /// <summary>
    /// Negates an integer expression (unary minus operation).
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The integer expression to negate.</param>
    /// <returns>A Z3Int representing the negated value of the operand.</returns>
    public static IntExpr UnaryMinus(this Z3Context context, IntExpr operand)
    {
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }
}
