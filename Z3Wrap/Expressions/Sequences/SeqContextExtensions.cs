using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

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
    public static SeqExpr<T> Seq<T>(this Z3Context context, params ReadOnlySpan<T> elements)
        where T : Z3Expr, IExprType<T>
    {
        if (elements.Length == 0)
            return context.SeqEmpty<T>();

        var units = new SeqExpr<T>[elements.Length];
        for (var i = 0; i < elements.Length; i++)
            units[i] = context.SeqUnit(elements[i]);

        return context.SeqConcat<T>(units);
    }

    /// <summary>
    /// Concatenates multiple sequence expressions.
    /// </summary>
    /// <typeparam name="T">Sequence element type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sequences">The sequences to concatenate.</param>
    /// <returns>Concatenated sequence expression.</returns>
    public static SeqExpr<T> SeqConcat<T>(this Z3Context context, params ReadOnlySpan<SeqExpr<T>> sequences)
        where T : Z3Expr, IExprType<T>
    {
        if (sequences.Length == 0)
            throw new ArgumentException("SeqConcat requires at least one operand.", nameof(sequences));

        var args = new IntPtr[sequences.Length];
        for (var i = 0; i < sequences.Length; i++)
            args[i] = sequences[i].Handle;

        var handle = context.Library.MkSeqConcat(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<SeqExpr<T>>(context, handle);
    }
}
