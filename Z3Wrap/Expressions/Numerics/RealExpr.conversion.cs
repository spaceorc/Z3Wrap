namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class RealExpr
{
    /// <summary>
    /// Converts this real expression to an integer expression (truncates towards zero).
    /// </summary>
    /// <returns>An integer expression representing the integer part of this real number.</returns>
    public IntExpr ToInt() => Context.ToInt(this);
}
