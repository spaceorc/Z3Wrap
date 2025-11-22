using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Represents a regular expression for string constraint solving.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class RegexExpr : Z3Expr, IExprType<RegexExpr>
{
    private RegexExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static RegexExpr IExprType<RegexExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<RegexExpr>.Sort(Z3Context context)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        return context.Library.MkReSort(context.Handle, stringSort);
    }

    /// <summary>
    /// Concatenation of two regular expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Concatenated regex.</returns>
    public static RegexExpr operator +(RegexExpr left, RegexExpr right) => left.Context.RegexConcat(left, right);

    /// <summary>
    /// Union (alternation) of two regular expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Union regex.</returns>
    public static RegexExpr operator |(RegexExpr left, RegexExpr right) => left.Context.RegexUnion(left, right);

    /// <summary>
    /// Complement of a regular expression.
    /// </summary>
    /// <param name="regex">The regex to complement.</param>
    /// <returns>Complemented regex.</returns>
    public static RegexExpr operator ~(RegexExpr regex) => regex.Context.RegexComplement(regex);

    /// <summary>
    /// Intersection of two regular expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Intersection regex.</returns>
    public static RegexExpr operator &(RegexExpr left, RegexExpr right) => left.Context.RegexIntersect(left, right);

    /// <summary>
    /// Difference of two regular expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Difference regex.</returns>
    public static RegexExpr operator -(RegexExpr left, RegexExpr right) => left.Context.RegexDiff(left, right);

    /// <summary>
    /// Equality comparison of two regex expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for equality.</returns>
    public static BoolExpr operator ==(RegexExpr left, RegexExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two regex expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for inequality.</returns>
    public static BoolExpr operator !=(RegexExpr left, RegexExpr right) => !left.Eq(right);
}
#pragma warning restore CS0660, CS0661
