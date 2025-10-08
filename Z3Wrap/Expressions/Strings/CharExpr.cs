using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Represents a character expression for Unicode character operations.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class CharExpr : Z3Expr, IExprType<CharExpr>
{
    private CharExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static CharExpr IExprType<CharExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<CharExpr>.Sort(Z3Context context) => context.Library.MkCharSort(context.Handle);

    /// <summary>
    /// Implicit conversion from C# char to character expression.
    /// </summary>
    /// <param name="value">The character value.</param>
    /// <returns>Character expression representing the value.</returns>
    public static implicit operator CharExpr(char value) => Z3Context.Current.Char(value);

    /// <summary>
    /// Converts this character to its Unicode codepoint as an integer.
    /// </summary>
    /// <returns>Integer expression representing the Unicode codepoint.</returns>
    public IntExpr ToInt()
    {
        var handle = Context.Library.MkCharToInt(Context.Handle, Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Converts this character to an 18-bit bitvector representation.
    /// </summary>
    /// <returns>18-bit bitvector containing the Unicode codepoint.</returns>
    /// <remarks>Z3 uses exactly 18-bit bitvectors for character representation.</remarks>
    public BvExpr<Size18> ToBv()
    {
        var handle = Context.Library.MkCharToBv(Context.Handle, Handle);
        return Z3Expr.Create<BvExpr<Size18>>(Context, handle);
    }

    /// <summary>
    /// Converts this character to a bitvector representation with custom size type.
    /// </summary>
    /// <typeparam name="TSize">Bitvector size type (must be 18 bits).</typeparam>
    /// <returns>Bitvector containing the Unicode codepoint.</returns>
    /// <exception cref="ArgumentException">Thrown if TSize is not 18 bits.</exception>
    /// <remarks>Z3 requires exactly 18-bit bitvectors for character-to-bitvector conversions.</remarks>
    public BvExpr<TSize> ToBv<TSize>()
        where TSize : ISize
    {
        if (TSize.Size != 18)
        {
            throw new ArgumentException(
                $"Character to bitvector conversion requires exactly 18-bit bitvector, but got {TSize.Size} bits.",
                nameof(TSize)
            );
        }

        var handle = Context.Library.MkCharToBv(Context.Handle, Handle);
        return Z3Expr.Create<BvExpr<TSize>>(Context, handle);
    }

    /// <summary>
    /// Checks if this character is a digit (0-9).
    /// </summary>
    /// <returns>Boolean expression that is true if this character is a digit.</returns>
    public BoolExpr IsDigit()
    {
        var handle = Context.Library.MkCharIsDigit(Context.Handle, Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Less than or equal comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for less than or equal comparison.</returns>
    public static BoolExpr operator <=(CharExpr left, CharExpr right)
    {
        var handle = left.Context.Library.MkCharLe(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Greater than or equal comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for greater than or equal comparison.</returns>
    public static BoolExpr operator >=(CharExpr left, CharExpr right) => right <= left;

    /// <summary>
    /// Less than comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for less than comparison.</returns>
    public static BoolExpr operator <(CharExpr left, CharExpr right) => (left <= right) & !(left == right);

    /// <summary>
    /// Greater than comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for greater than comparison.</returns>
    public static BoolExpr operator >(CharExpr left, CharExpr right) => right < left;

    /// <summary>
    /// Equality comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for equality.</returns>
    public static BoolExpr operator ==(CharExpr left, CharExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two character expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for inequality.</returns>
    public static BoolExpr operator !=(CharExpr left, CharExpr right) => !left.Eq(right);
}
