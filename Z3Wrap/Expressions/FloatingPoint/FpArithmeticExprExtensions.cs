using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Extension methods for floating-point arithmetic operations.
/// </summary>
public static class FpArithmeticExprExtensions
{
    /// <summary>
    /// Addition of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Sum of the operands.</returns>
    public static FpExpr<TFormat> Add<TFormat>(
        this FpExpr<TFormat> left,
        FpExpr<TFormat> right,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaAdd(left.Context.Handle, roundingMode.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Subtraction of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Difference of the operands.</returns>
    public static FpExpr<TFormat> Sub<TFormat>(
        this FpExpr<TFormat> left,
        FpExpr<TFormat> right,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaSub(left.Context.Handle, roundingMode.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Multiplication of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Product of the operands.</returns>
    public static FpExpr<TFormat> Mul<TFormat>(
        this FpExpr<TFormat> left,
        FpExpr<TFormat> right,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaMul(left.Context.Handle, roundingMode.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Division of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Quotient of the operands.</returns>
    public static FpExpr<TFormat> Div<TFormat>(
        this FpExpr<TFormat> left,
        FpExpr<TFormat> right,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaDiv(left.Context.Handle, roundingMode.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Remainder of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Remainder of the operands.</returns>
    public static FpExpr<TFormat> Rem<TFormat>(this FpExpr<TFormat> left, FpExpr<TFormat> right)
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaRem(left.Context.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Negation of floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Negated value.</returns>
    public static FpExpr<TFormat> Neg<TFormat>(this FpExpr<TFormat> operand)
        where TFormat : IFloatFormat
    {
        var handle = operand.Context.Library.MkFpaNeg(operand.Context.Handle, operand.Handle);
        return new FpExpr<TFormat>(operand.Context, handle);
    }

    /// <summary>
    /// Absolute value of floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Absolute value.</returns>
    public static FpExpr<TFormat> Abs<TFormat>(this FpExpr<TFormat> operand)
        where TFormat : IFloatFormat
    {
        var handle = operand.Context.Library.MkFpaAbs(operand.Context.Handle, operand.Handle);
        return new FpExpr<TFormat>(operand.Context, handle);
    }

    /// <summary>
    /// Square root of floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Square root of the operand.</returns>
    public static FpExpr<TFormat> Sqrt<TFormat>(this FpExpr<TFormat> operand, RoundingModeExpr roundingMode)
        where TFormat : IFloatFormat
    {
        var handle = operand.Context.Library.MkFpaSqrt(operand.Context.Handle, roundingMode.Handle, operand.Handle);
        return new FpExpr<TFormat>(operand.Context, handle);
    }

    /// <summary>
    /// Round to integral value using specified rounding mode.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Rounded integral value.</returns>
    public static FpExpr<TFormat> RoundToIntegral<TFormat>(this FpExpr<TFormat> operand, RoundingModeExpr roundingMode)
        where TFormat : IFloatFormat
    {
        var handle = operand.Context.Library.MkFpaRoundToIntegral(
            operand.Context.Handle,
            roundingMode.Handle,
            operand.Handle
        );
        return new FpExpr<TFormat>(operand.Context, handle);
    }

    /// <summary>
    /// Fused multiply-add: x * y + z with single rounding.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="x">The first multiplicand.</param>
    /// <param name="y">The second multiplicand.</param>
    /// <param name="z">The addend.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Result of x * y + z.</returns>
    public static FpExpr<TFormat> Fma<TFormat>(
        this FpExpr<TFormat> x,
        FpExpr<TFormat> y,
        FpExpr<TFormat> z,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var handle = x.Context.Library.MkFpaFma(x.Context.Handle, roundingMode.Handle, x.Handle, y.Handle, z.Handle);
        return new FpExpr<TFormat>(x.Context, handle);
    }

    /// <summary>
    /// Minimum of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Minimum of the operands.</returns>
    public static FpExpr<TFormat> Min<TFormat>(this FpExpr<TFormat> left, FpExpr<TFormat> right)
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaMin(left.Context.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }

    /// <summary>
    /// Maximum of two floating-point values.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Maximum of the operands.</returns>
    public static FpExpr<TFormat> Max<TFormat>(this FpExpr<TFormat> left, FpExpr<TFormat> right)
        where TFormat : IFloatFormat
    {
        var handle = left.Context.Library.MkFpaMax(left.Context.Handle, left.Handle, right.Handle);
        return new FpExpr<TFormat>(left.Context, handle);
    }
}
