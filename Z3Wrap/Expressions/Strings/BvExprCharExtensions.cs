using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides character conversion methods for bitvector expressions.
/// </summary>
public static class BvExprCharExtensions
{
    /// <summary>
    /// Converts 18-bit bitvector to character expression.
    /// </summary>
    /// <param name="bv">The 18-bit bitvector expression.</param>
    /// <returns>Character expression.</returns>
    /// <remarks>Z3 requires exactly 18-bit bitvectors for character conversion.</remarks>
    public static CharExpr ToChar(this BvExpr<Size18> bv)
    {
        var handle = bv.Context.Library.MkCharFromBv(bv.Context.Handle, bv.Handle);
        return Z3Expr.Create<CharExpr>(bv.Context, handle);
    }

    /// <summary>
    /// Converts bitvector to character expression with custom size type.
    /// </summary>
    /// <typeparam name="TSize">Bitvector size type (must be 18 bits).</typeparam>
    /// <param name="bv">The bitvector expression.</param>
    /// <returns>Character expression.</returns>
    /// <exception cref="ArgumentException">Thrown if TSize is not 18 bits.</exception>
    /// <remarks>Z3 requires exactly 18-bit bitvectors for character conversion.</remarks>
    public static CharExpr ToChar<TSize>(this BvExpr<TSize> bv)
        where TSize : ISize
    {
        if (TSize.Size != 18)
        {
            throw new ArgumentException(
                $"Bitvector to character conversion requires exactly 18-bit bitvector, but got {TSize.Size} bits.",
                nameof(TSize)
            );
        }

        var handle = bv.Context.Library.MkCharFromBv(bv.Context.Handle, bv.Handle);
        return Z3Expr.Create<CharExpr>(bv.Context, handle);
    }
}
