using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Numerics;

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

    /// <summary>
    /// Maps lambda function over sequence elements.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TResult">Result element type.</typeparam>
    /// <param name="sequence">The sequence to map over.</param>
    /// <param name="lambda">The lambda function to apply.</param>
    /// <returns>Sequence with lambda applied to each element.</returns>
    public static SeqExpr<TResult> Map<T, TResult>(this SeqExpr<T> sequence, LambdaExpr<T, TResult> lambda)
        where T : Z3Expr, IExprType<T>
        where TResult : Z3Expr, IExprType<TResult>
    {
        return sequence.Context.Map(sequence, lambda);
    }

    /// <summary>
    /// Maps lambda function over sequence elements with index tracking.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TResult">Result element type.</typeparam>
    /// <param name="sequence">The sequence to map over.</param>
    /// <param name="lambda">The lambda function taking index and element.</param>
    /// <param name="startIndex">The starting index value.</param>
    /// <returns>Sequence with lambda applied to each element with index.</returns>
    public static SeqExpr<TResult> Mapi<T, TResult>(
        this SeqExpr<T> sequence,
        LambdaExpr<IntExpr, T, TResult> lambda,
        IntExpr startIndex
    )
        where T : Z3Expr, IExprType<T>
        where TResult : Z3Expr, IExprType<TResult>
    {
        return sequence.Context.Mapi(sequence, lambda, startIndex);
    }

    /// <summary>
    /// Left fold over sequence with accumulator.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TAcc">Accumulator type.</typeparam>
    /// <param name="sequence">The sequence to fold over.</param>
    /// <param name="lambda">The lambda function taking accumulator and element.</param>
    /// <param name="accumulator">The initial accumulator value.</param>
    /// <returns>Final accumulator value after folding.</returns>
    public static TAcc Foldl<T, TAcc>(this SeqExpr<T> sequence, LambdaExpr<TAcc, T, TAcc> lambda, TAcc accumulator)
        where T : Z3Expr, IExprType<T>
        where TAcc : Z3Expr, IExprType<TAcc>
    {
        return sequence.Context.Foldl(sequence, lambda, accumulator);
    }

    /// <summary>
    /// Left fold over sequence with accumulator and index tracking.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TAcc">Accumulator type.</typeparam>
    /// <param name="sequence">The sequence to fold over.</param>
    /// <param name="lambda">The lambda function taking index, accumulator, and element.</param>
    /// <param name="startIndex">The starting index value.</param>
    /// <param name="accumulator">The initial accumulator value.</param>
    /// <returns>Final accumulator value after folding with index tracking.</returns>
    public static TAcc Foldli<T, TAcc>(
        this SeqExpr<T> sequence,
        LambdaExpr<IntExpr, TAcc, T, TAcc> lambda,
        IntExpr startIndex,
        TAcc accumulator
    )
        where T : Z3Expr, IExprType<T>
        where TAcc : Z3Expr, IExprType<TAcc>
    {
        return sequence.Context.Foldli(sequence, lambda, startIndex, accumulator);
    }
}
