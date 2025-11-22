using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides character expression creation methods for Z3Context.
/// </summary>
public static class CharContextExtensions
{
    /// <summary>
    /// Creates character expression from Unicode codepoint.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="codepoint">The Unicode codepoint.</param>
    /// <returns>Character expression representing the codepoint.</returns>
    public static CharExpr Char(this Z3Context context, uint codepoint)
    {
        var handle = context.Library.MkChar(context.Handle, codepoint);
        return Z3Expr.Create<CharExpr>(context, handle);
    }

    /// <summary>
    /// Creates character expression from char value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The character value.</param>
    /// <returns>Character expression representing the character.</returns>
    public static CharExpr Char(this Z3Context context, char value)
    {
        return context.Char((uint)value);
    }

    /// <summary>
    /// Creates character constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Character expression constant.</returns>
    public static CharExpr CharConst(this Z3Context context, string name)
    {
        var charSort = context.Library.MkCharSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, charSort);
        return Z3Expr.Create<CharExpr>(context, handle);
    }

    /// <summary>
    /// Converts character expression to its Unicode codepoint as an integer.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="ch">The character expression.</param>
    /// <returns>Integer expression representing the Unicode codepoint.</returns>
    public static IntExpr ToInt(this Z3Context context, CharExpr ch)
    {
        var handle = context.Library.MkCharToInt(context.Handle, ch.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Checks if character expression is a digit (0-9).
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="ch">The character expression.</param>
    /// <returns>Boolean expression that is true if the character is a digit.</returns>
    public static BoolExpr IsDigit(this Z3Context context, CharExpr ch)
    {
        var handle = context.Library.MkCharIsDigit(context.Handle, ch.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates less-than-or-equal comparison for character expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for less-than-or-equal comparison.</returns>
    public static BoolExpr Le(this Z3Context context, CharExpr left, CharExpr right)
    {
        var handle = context.Library.MkCharLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates greater-than-or-equal comparison for character expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge(this Z3Context context, CharExpr left, CharExpr right) => context.Le(right, left);

    /// <summary>
    /// Creates less-than comparison for character expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for less-than comparison.</returns>
    public static BoolExpr Lt(this Z3Context context, CharExpr left, CharExpr right) =>
        (context.Le(left, right)) & !(left == right);

    /// <summary>
    /// Creates greater-than comparison for character expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for greater-than comparison.</returns>
    public static BoolExpr Gt(this Z3Context context, CharExpr left, CharExpr right) => context.Lt(right, left);
}
