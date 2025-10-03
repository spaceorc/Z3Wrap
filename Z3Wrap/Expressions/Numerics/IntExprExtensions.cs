namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides integer-specific operations for integer expressions.
/// </summary>
public static class IntExprExtensions
{
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
}
