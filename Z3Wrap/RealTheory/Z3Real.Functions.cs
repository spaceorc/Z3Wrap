namespace Spaceorc.Z3Wrap.RealTheory;

public sealed partial class Z3Real
{
    /// <summary>
    /// Computes the absolute value of this real expression.
    /// </summary>
    /// <returns>A real expression representing |this|.</returns>
    public Z3Real Abs() => Context.Abs(this);
}