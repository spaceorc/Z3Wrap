using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public static partial class Z3ContextBitVecExtensions
{
    /// <summary>
    /// Resizes a compile-time size-validated bitvector expression to a new compile-time validated size by extending or truncating.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The type-safe bitvector expression to resize.</param>
    /// <param name="signed">If true, performs sign extension when enlarging; otherwise zero extension.</param>
    /// <typeparam name="TInputSize">The input size specification implementing ISize for compile-time validation.</typeparam>
    /// <typeparam name="TOutputSize">The output size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression with the target size.</returns>
    public static Z3BitVec<TOutputSize> Resize<TInputSize, TOutputSize>(
        this Z3Context context,
        Z3BitVec<TInputSize> expr,
        bool signed = false
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size == TInputSize.Size)
            return Z3Expr.Create<Z3BitVec<TOutputSize>>(context, expr.Handle);

        if (TOutputSize.Size > TInputSize.Size)
        {
            var additionalBits = TOutputSize.Size - TInputSize.Size;
            var handle = signed
                ? SafeNativeMethods.Z3MkSignExt(context.Handle, additionalBits, expr.Handle)
                : SafeNativeMethods.Z3MkZeroExt(context.Handle, additionalBits, expr.Handle);
            return Z3Expr.Create<Z3BitVec<TOutputSize>>(context, handle);
        }

        // Truncate by extracting lower bits
        return context.Extract<TInputSize, TOutputSize>(expr, 0);
    }

    /// <summary>
    /// Extracts a compile-time size-validated range of bits from a bitvector expression starting at the specified bit.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The type-safe bitvector expression to extract from.</param>
    /// <param name="startBit">The starting bit index (inclusive).</param>
    /// <typeparam name="TInputSize">The input size specification implementing ISize for compile-time validation.</typeparam>
    /// <typeparam name="TOutputSize">The output size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression containing the extracted bits.</returns>
    public static Z3BitVec<TOutputSize> Extract<TInputSize, TOutputSize>(
        this Z3Context context,
        Z3BitVec<TInputSize> expr,
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
        var handle = SafeNativeMethods.Z3MkExtract(context.Handle, high, startBit, expr.Handle);
        return Z3Expr.Create<Z3BitVec<TOutputSize>>(context, handle);
    }

    /// <summary>
    /// Creates a compile-time size-validated bitvector expression by repeating the input expression a calculated number of times.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The type-safe bitvector expression to repeat.</param>
    /// <typeparam name="TInputSize">The input size specification implementing ISize for compile-time validation.</typeparam>
    /// <typeparam name="TOutputSize">The output size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression containing the repeated pattern.</returns>
    public static Z3BitVec<TOutputSize> Repeat<TInputSize, TOutputSize>(
        this Z3Context context,
        Z3BitVec<TInputSize> expr
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size % TInputSize.Size != 0)
            throw new ArgumentException(
                $"Target size {TOutputSize.Size} must be a multiple of source size {TInputSize.Size}"
            );

        var count = TOutputSize.Size / TInputSize.Size;
        var handle = SafeNativeMethods.Z3MkRepeat(context.Handle, count, expr.Handle);
        return Z3Expr.Create<Z3BitVec<TOutputSize>>(context, handle);
    }
}
