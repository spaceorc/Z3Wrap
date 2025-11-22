using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Strings;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides core bit-vector creation and conversion methods for Z3Context.
/// </summary>
public static class BvCoreContextExtensions
{
    /// <summary>
    /// Creates bit-vector constant with specified name and size.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Bit-vector expression constant.</returns>
    public static BvExpr<TSize> BvConst<TSize>(this Z3Context context, string name)
        where TSize : ISize
    {
        var sort = context.Library.MkBvSort(context.Handle, TSize.Size);
        var handle = context.Library.MkConst(context.Handle, name, sort);

        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector expression from value.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The bit-vector value.</param>
    /// <returns>Bit-vector expression representing the value.</returns>
    public static BvExpr<TSize> Bv<TSize>(this Z3Context context, Bv<TSize> value)
        where TSize : ISize
    {
        var sort = context.Library.MkBvSort(context.Handle, TSize.Size);
        var handle = context.Library.MkNumeral(context.Handle, value.ToString(), sort);

        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates integer expression from bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bit-vector expression.</param>
    /// <param name="signed">True for signed conversion, false for unsigned.</param>
    /// <returns>Integer expression converted from bit-vector.</returns>
    public static IntExpr ToInt<TSize>(this Z3Context context, BvExpr<TSize> expr, bool signed = false)
        where TSize : ISize
    {
        var handle = context.Library.MkBv2int(context.Handle, expr.Handle, signed);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector expression resized to target size.
    /// </summary>
    /// <typeparam name="TInputSize">Input bit-vector size type.</typeparam>
    /// <typeparam name="TOutputSize">Output bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bit-vector expression to resize.</param>
    /// <param name="signed">True for sign extension, false for zero extension.</param>
    /// <returns>Bit-vector expression resized to target size.</returns>
    public static BvExpr<TOutputSize> Resize<TInputSize, TOutputSize>(
        this Z3Context context,
        BvExpr<TInputSize> expr,
        bool signed = false
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size == TInputSize.Size)
            return Z3Expr.Create<BvExpr<TOutputSize>>(context, expr.Handle);

        if (TOutputSize.Size > TInputSize.Size)
        {
            var additionalBits = TOutputSize.Size - TInputSize.Size;
            var handle = signed
                ? context.Library.MkSignExt(context.Handle, additionalBits, expr.Handle)
                : context.Library.MkZeroExt(context.Handle, additionalBits, expr.Handle);
            return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
        }

        // Truncate by extracting lower bits
        return context.Extract<TInputSize, TOutputSize>(expr, 0);
    }

    /// <summary>
    /// Creates bit-vector expression by extracting bits from another bit-vector.
    /// </summary>
    /// <typeparam name="TInputSize">Input bit-vector size type.</typeparam>
    /// <typeparam name="TOutputSize">Output bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bit-vector expression to extract from.</param>
    /// <param name="startBit">The starting bit position.</param>
    /// <returns>Bit-vector expression with extracted bits.</returns>
    public static BvExpr<TOutputSize> Extract<TInputSize, TOutputSize>(
        this Z3Context context,
        BvExpr<TInputSize> expr,
        uint startBit
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (startBit + TOutputSize.Size > TInputSize.Size)
            throw new ArgumentException(
                $"Extraction would exceed input bounds: startBit({startBit}) + outputSize({TOutputSize.Size}) > inputSize({TInputSize.Size})"
            );

        var high = startBit + TOutputSize.Size - 1;
        var handle = context.Library.MkExtract(context.Handle, high, startBit, expr.Handle);
        return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector expression by repeating another bit-vector.
    /// </summary>
    /// <typeparam name="TInputSize">Input bit-vector size type.</typeparam>
    /// <typeparam name="TOutputSize">Output bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bit-vector expression to repeat.</param>
    /// <returns>Bit-vector expression with repeated pattern.</returns>
    public static BvExpr<TOutputSize> Repeat<TInputSize, TOutputSize>(this Z3Context context, BvExpr<TInputSize> expr)
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size % TInputSize.Size != 0)
            throw new ArgumentException(
                $"Target size {TOutputSize.Size} must be a multiple of source size {TInputSize.Size}"
            );

        var count = TOutputSize.Size / TInputSize.Size;
        var handle = context.Library.MkRepeat(context.Handle, count, expr.Handle);
        return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
    }
}
