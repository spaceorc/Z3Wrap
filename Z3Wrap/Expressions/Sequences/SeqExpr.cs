using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Sequences;

/// <summary>
/// Represents a sequence expression for constraint solving with ordered collections.
/// </summary>
/// <typeparam name="T">The element type of the sequence.</typeparam>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class SeqExpr<T> : Z3Expr, IExprType<SeqExpr<T>>
    where T : Z3Expr, IExprType<T>
{
    private SeqExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static SeqExpr<T> IExprType<SeqExpr<T>>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<SeqExpr<T>>.Sort(Z3Context context) =>
        context.Library.MkSeqSort(context.Handle, T.Sort(context));

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>Element expression at the index.</returns>
    public T this[IntExpr index] => Nth(index);

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>Element expression at the index.</returns>
    public T Nth(IntExpr index)
    {
        var handle = Context.Library.MkSeqNth(Context.Handle, Handle, index.Handle);
        return Z3Expr.Create<T>(Context, handle);
    }

    /// <summary>
    /// Gets the unit sequence at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index.</param>
    /// <returns>Sequence of length 1 at the index, or empty sequence if out of bounds.</returns>
    public SeqExpr<T> At(IntExpr index)
    {
        var handle = Context.Library.MkSeqAt(Context.Handle, Handle, index.Handle);
        return Z3Expr.Create<SeqExpr<T>>(Context, handle);
    }

    /// <summary>
    /// Returns the length of this sequence.
    /// </summary>
    /// <returns>Integer expression representing the sequence length.</returns>
    public IntExpr Length()
    {
        var handle = Context.Library.MkSeqLength(Context.Handle, Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this sequence contains the specified subsequence.
    /// </summary>
    /// <param name="subsequence">The subsequence to search for.</param>
    /// <returns>Boolean expression that is true if this sequence contains the subsequence.</returns>
    public BoolExpr Contains(SeqExpr<T> subsequence)
    {
        var handle = Context.Library.MkSeqContains(Context.Handle, Handle, subsequence.Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this sequence starts with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix to check.</param>
    /// <returns>Boolean expression that is true if this sequence starts with the prefix.</returns>
    public BoolExpr StartsWith(SeqExpr<T> prefix)
    {
        var handle = Context.Library.MkSeqPrefix(Context.Handle, prefix.Handle, Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Checks if this sequence ends with the specified suffix.
    /// </summary>
    /// <param name="suffix">The suffix to check.</param>
    /// <returns>Boolean expression that is true if this sequence ends with the suffix.</returns>
    public BoolExpr EndsWith(SeqExpr<T> suffix)
    {
        var handle = Context.Library.MkSeqSuffix(Context.Handle, suffix.Handle, Handle);
        return Z3Expr.Create<BoolExpr>(Context, handle);
    }

    /// <summary>
    /// Extracts a subsequence from this sequence.
    /// </summary>
    /// <param name="offset">The starting position.</param>
    /// <param name="length">The length of the subsequence.</param>
    /// <returns>Sequence expression representing the subsequence.</returns>
    public SeqExpr<T> Extract(IntExpr offset, IntExpr length)
    {
        var handle = Context.Library.MkSeqExtract(Context.Handle, Handle, offset.Handle, length.Handle);
        return Z3Expr.Create<SeqExpr<T>>(Context, handle);
    }

    /// <summary>
    /// Replaces the first occurrence of a subsequence with another sequence.
    /// </summary>
    /// <param name="source">The subsequence to replace.</param>
    /// <param name="destination">The replacement sequence.</param>
    /// <returns>Sequence expression with the first occurrence replaced.</returns>
    public SeqExpr<T> Replace(SeqExpr<T> source, SeqExpr<T> destination)
    {
        var handle = Context.Library.MkSeqReplace(Context.Handle, Handle, source.Handle, destination.Handle);
        return Z3Expr.Create<SeqExpr<T>>(Context, handle);
    }

    /// <summary>
    /// Finds the first index of a subsequence starting from the specified offset.
    /// </summary>
    /// <param name="subsequence">The subsequence to find.</param>
    /// <param name="offset">The starting offset.</param>
    /// <returns>Integer expression representing the index, or -1 if not found.</returns>
    public IntExpr IndexOf(SeqExpr<T> subsequence, IntExpr offset)
    {
        var handle = Context.Library.MkSeqIndex(Context.Handle, Handle, subsequence.Handle, offset.Handle);
        return Z3Expr.Create<IntExpr>(Context, handle);
    }

    /// <summary>
    /// Finds the last index of a subsequence.
    /// </summary>
    /// <param name="subsequence">The subsequence to find.</param>
    /// <returns>Integer expression representing the last index, or -1 if not found.</returns>
    [Obsolete("LastIndexOf is not supported in Z3 (unstable behavior). This method will throw NotSupportedException.")]
    public IntExpr LastIndexOf(SeqExpr<T> subsequence)
    {
        throw new NotSupportedException(
            "Z3's seq.last_indexof has unstable behavior and is not supported in Z3Wrap. " +
            "Use IndexOf with manual iteration if you need to find the last occurrence.");
    }

    /// <summary>
    /// Concatenation of two sequence expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Concatenated sequence expression.</returns>
    public static SeqExpr<T> operator +(SeqExpr<T> left, SeqExpr<T> right) => left.Concat(right);

    /// <summary>
    /// Equality comparison of two sequence expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for equality.</returns>
    public static BoolExpr operator ==(SeqExpr<T> left, SeqExpr<T> right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two sequence expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression for inequality.</returns>
    public static BoolExpr operator !=(SeqExpr<T> left, SeqExpr<T> right) => !left.Eq(right);
}
