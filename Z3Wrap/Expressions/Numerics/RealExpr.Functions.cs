namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class RealExpr
{
    /// <summary>
    /// Computes the absolute value of this real expression.
    /// </summary>
    /// <returns>A real expression representing |this|.</returns>
    public RealExpr Abs() => Context.Abs(this);

    /// <summary>
    /// Computes the minimum of this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A real expression representing the minimum of this and other.</returns>
    public RealExpr Min(RealExpr other) => Context.Min(this, other);

    /// <summary>
    /// Computes the maximum of this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A real expression representing the maximum of this and other.</returns>
    public RealExpr Max(RealExpr other) => Context.Max(this, other);
}
