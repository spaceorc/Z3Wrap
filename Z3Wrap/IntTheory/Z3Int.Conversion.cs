using Spaceorc.Z3Wrap.BitVecTheory;
using Spaceorc.Z3Wrap.RealTheory;

namespace Spaceorc.Z3Wrap.IntTheory;

public sealed partial class Z3Int
{
    /// <summary>
    /// Converts this integer expression to a real (rational) number expression.
    /// </summary>
    /// <returns>A real expression representing this integer as a rational number.</returns>
    public Z3Real ToReal() => Context.ToReal(this);

    /// <summary>
    /// Converts this integer expression to a bitvector expression with the specified bit width.
    /// </summary>
    /// <typeparam name="TSize">The size type that determines the bit width of the resulting bitvector.</typeparam>
    /// <returns>A bitvector expression representing this integer value.</returns>
    public Z3BitVec<TSize> ToBitVec<TSize>()
        where TSize : ISize => Context.ToBitVec<TSize>(this);
}
