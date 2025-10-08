using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides character-specific operations for character expressions.
/// </summary>
public static class CharExprExtensions
{
    /// <summary>
    /// Creates less-than-or-equal comparison for this character expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than-or-equal comparison.</returns>
    public static BoolExpr Le(this CharExpr left, CharExpr right) => left.Context.Le(left, right);

    /// <summary>
    /// Creates greater-than-or-equal comparison for this character expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge(this CharExpr left, CharExpr right) => left.Context.Ge(left, right);

    /// <summary>
    /// Creates less-than comparison for this character expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than comparison.</returns>
    public static BoolExpr Lt(this CharExpr left, CharExpr right) => left.Context.Lt(left, right);

    /// <summary>
    /// Creates greater-than comparison for this character expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than comparison.</returns>
    public static BoolExpr Gt(this CharExpr left, CharExpr right) => left.Context.Gt(left, right);
}
