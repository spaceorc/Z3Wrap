using Spaceorc.Z3Wrap.IntTheory;

namespace Spaceorc.Z3Wrap.RealTheory;

public sealed partial class Z3Real
{
    /// <summary>
    /// Converts this real expression to an integer expression (truncates towards zero).
    /// </summary>
    /// <returns>An integer expression representing the integer part of this real number.</returns>
    public Z3Int ToInt() => Context.ToInt(this);
}