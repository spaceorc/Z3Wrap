namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides integer-specific operations for integer expressions.
/// </summary>
public static class IntExprExtensions
{
    /// <summary>
    /// Creates power expression for this integer expression.
    /// </summary>
    /// <param name="base">The base operand.</param>
    /// <param name="exponent">The exponent operand (int or real).</param>
    /// <returns>Real expression representing base^exponent.</returns>
    /// <remarks>
    /// Z3's power operation always returns Real sort, even for integer inputs.
    /// Z3 support for exponentiation is limited: works best with constant exponents,
    /// may return unknown for variable exponents or complex nonlinear arithmetic.
    /// </remarks>
    public static RealExpr Power(this IntExpr @base, ArithmeticExpr exponent) => @base.Context.Power(@base, exponent);

    /// <summary>
    /// Creates modulo operation for this integer expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Integer expression representing the modulo operation.</returns>
    public static IntExpr Mod(this IntExpr left, IntExpr right) => left.Context.Mod(left, right);

    /// <summary>
    /// Creates modulo operation with integer literal as right operand.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The integer literal right operand.</param>
    /// <returns>Integer expression representing the modulo operation.</returns>
    public static IntExpr Mod(this IntExpr left, int right) => left.Context.Mod(left, right);

    /// <summary>
    /// Creates remainder operation for this integer expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Integer expression representing the remainder operation.</returns>
    public static IntExpr Rem(this IntExpr left, IntExpr right) => left.Context.Rem(left, right);

    /// <summary>
    /// Creates remainder operation with integer literal as right operand.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The integer literal right operand.</param>
    /// <returns>Integer expression representing the remainder operation.</returns>
    public static IntExpr Rem(this IntExpr left, int right) => left.Context.Rem(left, right);

    /// <summary>
    /// Creates divisibility check for this integer expression.
    /// </summary>
    /// <param name="divisor">The divisor.</param>
    /// <param name="dividend">The dividend.</param>
    /// <returns>Boolean expression that is true when divisor divides dividend.</returns>
    public static Logic.BoolExpr Divides(this IntExpr divisor, IntExpr dividend) =>
        divisor.Context.Divides(divisor, dividend);

    /// <summary>
    /// Creates divisibility check with integer literal as dividend.
    /// </summary>
    /// <param name="divisor">The divisor.</param>
    /// <param name="dividend">The integer literal dividend.</param>
    /// <returns>Boolean expression that is true when divisor divides dividend.</returns>
    public static Logic.BoolExpr Divides(this IntExpr divisor, int dividend) =>
        divisor.Context.Divides(divisor, dividend);
}
