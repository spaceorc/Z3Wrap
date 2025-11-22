using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Sequences;

/// <summary>
/// Provides sequence-specific operations for sequence expressions.
/// </summary>
public static class SeqExprExtensions
{
    /// <summary>
    /// Concatenates this sequence with additional sequences.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional sequences to concatenate.</param>
    /// <returns>Concatenated sequence expression.</returns>
    public static SeqExpr<T> Concat<T>(this SeqExpr<T> left, params ReadOnlySpan<SeqExpr<T>> others)
        where T : Z3Expr, IExprType<T>
    {
        return left.Context.SeqConcat([left, .. others]);
    }
}
