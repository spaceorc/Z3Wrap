using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Represents a string expression for constraint solving with Unicode strings.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class StringExpr : Z3Expr, IExprType<StringExpr>
{
    private StringExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static StringExpr IExprType<StringExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<StringExpr>.Sort(Z3Context context) => context.Library.MkStringSort(context.Handle);

    /// <summary>
    /// Implicit conversion from string literal to string expression.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>String expression representing the value.</returns>
    public static implicit operator StringExpr(string value) => Z3Context.Current.String(value);

    /// <summary>
    /// Gets the character at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>Character expression at the index.</returns>
    /// <remarks>Uses Z3's MkSeqNth - returns Char element. Matches C# string indexer semantics.</remarks>
    public CharExpr this[IntExpr index] => CharAt(index);

    /// <summary>
    /// Gets the character at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>Character expression at the index.</returns>
    /// <remarks>Uses Z3's MkSeqNth - returns Char element.</remarks>
    public CharExpr CharAt(IntExpr index)
    {
        var handle = Context.Library.MkSeqNth(Context.Handle, Handle, index.Handle);
        return Z3Expr.Create<CharExpr>(Context, handle);
    }

    /// <summary>
    /// Gets the unit string at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>String expression of length 1 at the index, or empty string if out of bounds.</returns>
    /// <remarks>Uses Z3's MkSeqAt - returns String of length 1.</remarks>
    public StringExpr At(IntExpr index)
    {
        var handle = Context.Library.MkSeqAt(Context.Handle, Handle, index.Handle);
        return Z3Expr.Create<StringExpr>(Context, handle);
    }

    /// <summary>
    /// Returns the length of this string.
    /// </summary>
    /// <returns>Integer expression representing the string length.</returns>
    public IntExpr Length()
    {
        var handle = Context.Library.MkSeqLength(Context.Handle, Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this string contains the specified substring.
    /// </summary>
    /// <param name="substring">The substring to search for.</param>
    /// <returns>Boolean expression that is true if this string contains the substring.</returns>
    public BoolExpr Contains(StringExpr substring)
    {
        var handle = Context.Library.MkSeqContains(Context.Handle, Handle, substring.Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this string starts with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix to check.</param>
    /// <returns>Boolean expression that is true if this string starts with the prefix.</returns>
    public BoolExpr StartsWith(StringExpr prefix)
    {
        var handle = Context.Library.MkSeqPrefix(Context.Handle, prefix.Handle, Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this string ends with the specified suffix.
    /// </summary>
    /// <param name="suffix">The suffix to check.</param>
    /// <returns>Boolean expression that is true if this string ends with the suffix.</returns>
    public BoolExpr EndsWith(StringExpr suffix)
    {
        var handle = Context.Library.MkSeqSuffix(Context.Handle, suffix.Handle, Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Extracts a substring from this string.
    /// </summary>
    /// <param name="offset">The starting position.</param>
    /// <param name="length">The length of the substring.</param>
    /// <returns>String expression representing the substring.</returns>
    public StringExpr Substring(IntExpr offset, IntExpr length)
    {
        var handle = Context.Library.MkSeqExtract(Context.Handle, Handle, offset.Handle, length.Handle);
        return Z3Expr.Create<StringExpr>(Context, handle);
    }

    /// <summary>
    /// Replaces the first occurrence of a substring with another string.
    /// </summary>
    /// <param name="source">The substring to replace.</param>
    /// <param name="destination">The replacement string.</param>
    /// <returns>String expression with the first occurrence replaced.</returns>
    public StringExpr Replace(StringExpr source, StringExpr destination)
    {
        var handle = Context.Library.MkSeqReplace(Context.Handle, Handle, source.Handle, destination.Handle);
        return Z3Expr.Create<StringExpr>(Context, handle);
    }

    /// <summary>
    /// Finds the first index of a substring starting from the specified offset.
    /// </summary>
    /// <param name="substring">The substring to find.</param>
    /// <param name="offset">The starting offset.</param>
    /// <returns>Integer expression representing the index, or -1 if not found.</returns>
    public IntExpr IndexOf(StringExpr substring, IntExpr offset)
    {
        var handle = Context.Library.MkSeqIndex(Context.Handle, Handle, substring.Handle, offset.Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Finds the last index of a substring.
    /// </summary>
    /// <param name="substring">The substring to find.</param>
    /// <returns>Integer expression representing the last index, or -1 if not found.</returns>
    public IntExpr LastIndexOf(StringExpr substring)
    {
        var handle = Context.Library.MkSeqLastIndex(Context.Handle, Handle, substring.Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Concatenates this string with another string.
    /// </summary>
    /// <param name="other">The string to append.</param>
    /// <returns>Concatenated string expression.</returns>
    public StringExpr Concat(StringExpr other)
    {
        var args = new[] { Handle, other.Handle };
        var handle = Context.Library.MkSeqConcat(Context.Handle, 2, args);
        return Z3Expr.Create<StringExpr>(Context, handle);
    }

    /// <summary>
    /// Converts this string to an integer.
    /// </summary>
    /// <returns>Integer expression representing the parsed string value.</returns>
    /// <remarks>Result is unspecified if the string does not represent a valid integer.</remarks>
    public IntExpr ToInt()
    {
        var handle = Context.Library.MkStrToInt(Context.Handle, Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Concatenation of two string expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Concatenated string expression.</returns>
    public static StringExpr operator +(StringExpr left, StringExpr right) => left.Concat(right);

    /// <summary>
    /// Lexicographic less than comparison.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for less than comparison.</returns>
    public static BoolExpr operator <(StringExpr left, StringExpr right)
    {
        var handle = left.Context.Library.MkStrLt(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Lexicographic less than or equal comparison.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for less than or equal comparison.</returns>
    public static BoolExpr operator <=(StringExpr left, StringExpr right)
    {
        var handle = left.Context.Library.MkStrLe(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Lexicographic greater than comparison.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for greater than comparison.</returns>
    public static BoolExpr operator >(StringExpr left, StringExpr right) => right < left;

    /// <summary>
    /// Lexicographic greater than or equal comparison.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for greater than or equal comparison.</returns>
    public static BoolExpr operator >=(StringExpr left, StringExpr right) => right <= left;

    /// <summary>
    /// Equality comparison of two string expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for equality.</returns>
    public static BoolExpr operator ==(StringExpr left, StringExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two string expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for inequality.</returns>
    public static BoolExpr operator !=(StringExpr left, StringExpr right) => !left.Eq(right);
}
