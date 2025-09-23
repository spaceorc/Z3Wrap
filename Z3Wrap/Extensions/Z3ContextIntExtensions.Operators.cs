using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextIntExtensions
{
    /// <summary>
    /// Creates an addition expression from multiple integer operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to add together.</param>
    /// <returns>A new integer expression representing the sum of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static Z3IntExpr Add(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Add requires at least one operand. Z3 does not support empty addition."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAdd(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a subtraction expression from multiple integer operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to subtract (first - second - third - ...).</param>
    /// <returns>A new integer expression representing the result of the subtraction.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static Z3IntExpr Sub(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Sub requires at least one operand. Z3 does not support empty subtraction."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkSub(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a multiplication expression from multiple integer operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The integer expressions to multiply together.</param>
    /// <returns>A new integer expression representing the product of all operands.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no operands are provided.</exception>
    public static Z3IntExpr Mul(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException(
                "Mul requires at least one operand. Z3 does not support empty multiplication."
            );

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkMul(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a division expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend (left operand).</param>
    /// <param name="right">The divisor (right operand).</param>
    /// <returns>A new integer expression representing left divided by right.</returns>
    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a modulo expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend (left operand).</param>
    /// <param name="right">The divisor (right operand).</param>
    /// <returns>A new integer expression representing left modulo right.</returns>
    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a unary minus expression for an integer operand.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The integer expression to negate.</param>
    /// <returns>A new integer expression representing the negated operand.</returns>
    public static Z3IntExpr UnaryMinus(this Z3Context context, Z3IntExpr operand)
    {
        var args = new[] { operand.Handle };
        var resultHandle = SafeNativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a less-than comparison expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than comparison expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates an addition expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new integer expression representing left + right.</returns>
    public static Z3IntExpr Add(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Add(left, context.Int(right));

    /// <summary>
    /// Creates an addition expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new integer expression representing left + right.</returns>
    public static Z3IntExpr Add(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Add(context.Int(left), right);

    /// <summary>
    /// Creates a subtraction expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new integer expression representing left - right.</returns>
    public static Z3IntExpr Sub(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Sub(left, context.Int(right));

    /// <summary>
    /// Creates a subtraction expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new integer expression representing left - right.</returns>
    public static Z3IntExpr Sub(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Sub(context.Int(left), right);

    /// <summary>
    /// Creates a multiplication expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new integer expression representing left * right.</returns>
    public static Z3IntExpr Mul(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Mul(left, context.Int(right));

    /// <summary>
    /// Creates a multiplication expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new integer expression representing left * right.</returns>
    public static Z3IntExpr Mul(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Mul(context.Int(left), right);

    /// <summary>
    /// Creates a division expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new integer expression representing left / right.</returns>
    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Div(left, context.Int(right));

    /// <summary>
    /// Creates a division expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new integer expression representing left / right.</returns>
    public static Z3IntExpr Div(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Div(context.Int(left), right);

    /// <summary>
    /// Creates a modulo expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new integer expression representing left % right.</returns>
    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Mod(left, context.Int(right));

    /// <summary>
    /// Creates a modulo expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new integer expression representing left % right.</returns>
    public static Z3IntExpr Mod(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Mod(context.Int(left), right);

    /// <summary>
    /// Creates a less-than comparison expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Lt(left, context.Int(right));

    /// <summary>
    /// Creates a less-than comparison expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr Lt(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Lt(context.Int(left), right);

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Le(left, context.Int(right));

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr Le(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Le(context.Int(left), right);

    /// <summary>
    /// Creates a greater-than comparison expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Gt(left, context.Int(right));

    /// <summary>
    /// Creates a greater-than comparison expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr Gt(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Gt(context.Int(left), right);

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between an integer expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The integer expression (left operand).</param>
    /// <param name="right">The BigInteger value (right operand).</param>
    /// <returns>A new boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, BigInteger right) =>
        context.Ge(left, context.Int(right));

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between a BigInteger value and an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value (left operand).</param>
    /// <param name="right">The integer expression (right operand).</param>
    /// <returns>A new boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr Ge(this Z3Context context, BigInteger left, Z3IntExpr right) =>
        context.Ge(context.Int(left), right);
}
