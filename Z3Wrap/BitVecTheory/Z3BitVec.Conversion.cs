using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public sealed partial class Z3BitVec<TSize>
{
    /// <summary>
    /// Converts this bitvector to an integer expression.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (true) or unsigned (false).</param>
    /// <returns>An integer expression representing this bitvector value.</returns>
    public Z3IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);
}
