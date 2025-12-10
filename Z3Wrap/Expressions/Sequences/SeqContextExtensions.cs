using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Sequences;

/// <summary>
/// Provides sequence expression creation methods for Z3Context.
/// </summary>
public static class SeqContextExtensions
{
    /// <summary>
    /// Creates sequence constant with specified name and element type.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Sequence expression constant.</returns>
    public static SeqExpr<T> SeqConst<T>(this Z3Context context, string name)
        where T : Z3Expr, IExprType<T>
    {
        var seqSort = context.GetSortForType<SeqExpr<T>>();
        var handle = context.Library.MkConst(context.Handle, name, seqSort);
        return Z3Expr.Create<SeqExpr<T>>(context, handle);
    }

    /// <summary>
    /// Creates empty sequence of specified element type.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Empty sequence expression.</returns>
    public static SeqExpr<T> SeqEmpty<T>(this Z3Context context)
        where T : Z3Expr, IExprType<T>
    {
        var seqSort = context.GetSortForType<SeqExpr<T>>();
        var handle = context.Library.MkSeqEmpty(context.Handle, seqSort);
        return Z3Expr.Create<SeqExpr<T>>(context, handle);
    }

    /// <summary>
    /// Creates unit sequence containing single element.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="element">The element to wrap in a sequence.</param>
    /// <returns>Sequence expression containing single element.</returns>
    public static SeqExpr<T> SeqUnit<T>(this Z3Context context, T element)
        where T : Z3Expr, IExprType<T>
    {
        var handle = context.Library.MkSeqUnit(context.Handle, element.Handle);
        return Z3Expr.Create<SeqExpr<T>>(context, handle);
    }

    /// <summary>
    /// Creates sequence from array of elements.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="elements">The elements to create sequence from.</param>
    /// <returns>Sequence expression containing the elements.</returns>
    public static SeqExpr<T> Seq<T>(this Z3Context context, params IEnumerable<T> elements)
        where T : Z3Expr, IExprType<T>
    {
        var units = elements.Select(context.SeqUnit).ToArray();
        if (units.Length == 0)
            return context.SeqEmpty<T>();

        return context.SeqConcat<T>(units);
    }

    /// <summary>
    /// Concatenates multiple sequence expressions.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequences">The sequences to concatenate.</param>
    /// <returns>Concatenated sequence expression.</returns>
    public static SeqExpr<T> SeqConcat<T>(this Z3Context context, params IEnumerable<SeqExpr<T>> sequences)
        where T : Z3Expr, IExprType<T>
    {
        var args = sequences.Select(s => s.Handle).ToArray();
        if (args.Length == 0)
            throw new ArgumentException("SeqConcat requires at least one operand.", nameof(sequences));

        var handle = context.Library.MkSeqConcat(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<SeqExpr<T>>(context, handle);
    }

    /// <summary>
    /// Maps lambda function over sequence elements.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TResult">Result element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequence">The sequence to map over.</param>
    /// <param name="lambda">The lambda function to apply.</param>
    /// <returns>Sequence with lambda applied to each element.</returns>
    public static SeqExpr<TResult> Map<T, TResult>(
        this Z3Context context,
        SeqExpr<T> sequence,
        LambdaExpr<T, TResult> lambda
    )
        where T : Z3Expr, IExprType<T>
        where TResult : Z3Expr, IExprType<TResult>
    {
        var handle = context.Library.MkSeqMap(context.Handle, lambda.Handle, sequence.Handle);
        return Z3Expr.Create<SeqExpr<TResult>>(context, handle);
    }

    /// <summary>
    /// Maps lambda function over sequence elements with index tracking.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TResult">Result element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequence">The sequence to map over.</param>
    /// <param name="lambda">The lambda function taking index and element.</param>
    /// <param name="startIndex">The starting index value.</param>
    /// <returns>Sequence with lambda applied to each element with index.</returns>
    public static SeqExpr<TResult> Mapi<T, TResult>(
        this Z3Context context,
        SeqExpr<T> sequence,
        LambdaExpr<IntExpr, T, TResult> lambda,
        IntExpr startIndex
    )
        where T : Z3Expr, IExprType<T>
        where TResult : Z3Expr, IExprType<TResult>
    {
        var handle = context.Library.MkSeqMapi(context.Handle, lambda.Handle, startIndex.Handle, sequence.Handle);
        return Z3Expr.Create<SeqExpr<TResult>>(context, handle);
    }

    /// <summary>
    /// Left fold over sequence with accumulator.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TAcc">Accumulator type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequence">The sequence to fold over.</param>
    /// <param name="lambda">The lambda function taking accumulator and element.</param>
    /// <param name="accumulator">The initial accumulator value.</param>
    /// <returns>Final accumulator value after folding.</returns>
    public static TAcc Foldl<T, TAcc>(
        this Z3Context context,
        SeqExpr<T> sequence,
        LambdaExpr<TAcc, T, TAcc> lambda,
        TAcc accumulator
    )
        where T : Z3Expr, IExprType<T>
        where TAcc : Z3Expr, IExprType<TAcc>
    {
        var handle = context.Library.MkSeqFoldl(context.Handle, lambda.Handle, accumulator.Handle, sequence.Handle);
        return Z3Expr.Create<TAcc>(context, handle);
    }

    /// <summary>
    /// Left fold over sequence with accumulator and index tracking.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <typeparam name="TAcc">Accumulator type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequence">The sequence to fold over.</param>
    /// <param name="lambda">The lambda function taking index, accumulator, and element.</param>
    /// <param name="startIndex">The starting index value.</param>
    /// <param name="accumulator">The initial accumulator value.</param>
    /// <returns>Final accumulator value after folding with index tracking.</returns>
    public static TAcc Foldli<T, TAcc>(
        this Z3Context context,
        SeqExpr<T> sequence,
        LambdaExpr<IntExpr, TAcc, T, TAcc> lambda,
        IntExpr startIndex,
        TAcc accumulator
    )
        where T : Z3Expr, IExprType<T>
        where TAcc : Z3Expr, IExprType<TAcc>
    {
        var handle = context.Library.MkSeqFoldli(
            context.Handle,
            lambda.Handle,
            startIndex.Handle,
            accumulator.Handle,
            sequence.Handle
        );
        return Z3Expr.Create<TAcc>(context, handle);
    }
}
