using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class IntExpr
{
    /// <summary>
    /// Converts this integer expression to a real (rational) number expression.
    /// </summary>
    /// <returns>A real expression representing this integer as a rational number.</returns>
    public RealExpr ToReal() => Context.ToReal(this);

    /// <summary>
    /// Converts this integer expression to a bitvector expression with the specified bit width.
    /// </summary>
    /// <typeparam name="TSize">The size type that determines the bit width of the resulting bitvector.</typeparam>
    /// <returns>A bitvector expression representing this integer value.</returns>
    public BvExpr<TSize> ToBitVec<TSize>()
        where TSize : ISize => Context.ToBitVec<TSize>(this);
}
