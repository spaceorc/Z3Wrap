namespace Spaceorc.Z3Wrap.RealTheory;

public sealed partial class Z3Real
{
    /// <summary>
    /// Computes the absolute value of this real expression.
    /// </summary>
    /// <returns>A real expression representing |this|.</returns>
    public Z3Real Abs() => Context.Abs(this);

    /// <summary>
    /// Computes the minimum of this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A real expression representing the minimum of this and other.</returns>
    public Z3Real Min(Z3Real other) => Context.Min(this, other);

    /// <summary>
    /// Computes the maximum of this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A real expression representing the maximum of this and other.</returns>
    public Z3Real Max(Z3Real other) => Context.Max(this, other);
}
