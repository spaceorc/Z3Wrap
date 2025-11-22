using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides string-specific operations for string expressions.
/// </summary>
public static class StringExprExtensions
{
    /// <summary>
    /// Creates less-than comparison for this string expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than comparison.</returns>
    public static BoolExpr Lt(this StringExpr left, StringExpr right) => left.Context.Lt(left, right);

    /// <summary>
    /// Creates less-than-or-equal comparison for this string expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than-or-equal comparison.</returns>
    public static BoolExpr Le(this StringExpr left, StringExpr right) => left.Context.Le(left, right);

    /// <summary>
    /// Creates greater-than comparison for this string expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than comparison.</returns>
    public static BoolExpr Gt(this StringExpr left, StringExpr right) => left.Context.Gt(left, right);

    /// <summary>
    /// Creates greater-than-or-equal comparison for this string expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge(this StringExpr left, StringExpr right) => left.Context.Ge(left, right);

    /// <summary>
    /// Creates concatenation operation for this string expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional operands to concatenate.</param>
    /// <returns>Expression representing the concatenation.</returns>
    public static StringExpr Concat(this StringExpr left, params ReadOnlySpan<StringExpr> others) =>
        left.Context.Concat([left, .. others]);

    /// <summary>
    /// Checks if this string matches a regular expression pattern.
    /// </summary>
    /// <param name="str">The string expression to match.</param>
    /// <param name="regex">The regular expression pattern.</param>
    /// <returns>Boolean expression that is true if the string matches the pattern.</returns>
    public static BoolExpr Matches(this StringExpr str, RegexExpr regex) => str.Context.InRegex(str, regex);

    /// <summary>
    /// Converts this string expression to a regular expression.
    /// </summary>
    /// <param name="str">The string expression to convert.</param>
    /// <returns>Regular expression matching the string.</returns>
    public static RegexExpr ToRegex(this StringExpr str) => str.Context.Regex(str);
}
