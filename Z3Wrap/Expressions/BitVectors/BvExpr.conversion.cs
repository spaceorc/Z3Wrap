using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public sealed partial class BvExpr<TSize>
{
    /// <summary>
    /// Converts this bitvector to an integer expression.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (true) or unsigned (false).</param>
    /// <returns>An integer expression representing this bitvector value.</returns>
    public IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);
}
