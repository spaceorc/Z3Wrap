using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Extends a bitvector expression by adding additional bits.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to extend.</param>
    /// <param name="additionalBits">The number of additional bits to add.</param>
    /// <param name="signed">If true, performs sign extension; otherwise zero extension.</param>
    /// <returns>A Z3 bitvector expression with the extended width.</returns>
    public static Z3BitVecExpr Extend(
        this Z3Context context,
        Z3BitVecExpr expr,
        uint additionalBits,
        bool signed = false
    )
    {
        var handle = signed
            ? NativeMethods.Z3MkSignExt(context.Handle, additionalBits, expr.Handle)
            : NativeMethods.Z3MkZeroExt(context.Handle, additionalBits, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Resizes a bitvector expression to a specific bit width by extending or truncating.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to resize.</param>
    /// <param name="newSize">The target bit width.</param>
    /// <param name="signed">If true, performs sign extension when enlarging; otherwise zero extension.</param>
    /// <returns>A Z3 bitvector expression with the specified width.</returns>
    public static Z3BitVecExpr Resize(
        this Z3Context context,
        Z3BitVecExpr expr,
        uint newSize,
        bool signed = false
    )
    {
        if (newSize == expr.Size)
            return expr;

        if (newSize > expr.Size)
        {
            var additionalBits = newSize - expr.Size;
            return context.Extend(expr, additionalBits, signed);
        }

        // Truncate by extracting lower bits
        return context.Extract(expr, newSize - 1, 0);
    }

    /// <summary>
    /// Extracts a range of bits from a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to extract from.</param>
    /// <param name="high">The high bit index (inclusive).</param>
    /// <param name="low">The low bit index (inclusive).</param>
    /// <returns>A Z3 bitvector expression containing the extracted bits.</returns>
    public static Z3BitVecExpr Extract(
        this Z3Context context,
        Z3BitVecExpr expr,
        uint high,
        uint low
    )
    {
        var handle = NativeMethods.Z3MkExtract(context.Handle, high, low, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a bitvector expression by repeating the input expression a specified number of times.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to repeat.</param>
    /// <param name="count">The number of times to repeat the expression.</param>
    /// <returns>A Z3 bitvector expression containing the repeated pattern.</returns>
    public static Z3BitVecExpr Repeat(this Z3Context context, Z3BitVecExpr expr, uint count)
    {
        var handle = NativeMethods.Z3MkRepeat(context.Handle, count, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }
}
