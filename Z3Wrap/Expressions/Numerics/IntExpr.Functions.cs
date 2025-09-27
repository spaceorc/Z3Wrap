using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class IntExpr
{
    /// <summary>
    /// Computes the absolute value of this integer expression.
    /// </summary>
    /// <returns>An integer expression representing |this|.</returns>
    public IntExpr Abs() => Context.Abs(this);

    /// <summary>
    /// Computes the minimum of this integer expression and another integer expression.
    /// </summary>
    /// <param name="other">The other integer expression to compare with.</param>
    /// <returns>An integer expression representing min(this, other).</returns>
    public IntExpr Min(IntExpr other) => Context.Min(this, other);

    /// <summary>
    /// Computes the maximum of this integer expression and another integer expression.
    /// </summary>
    /// <param name="other">The other integer expression to compare with.</param>
    /// <returns>An integer expression representing max(this, other).</returns>
    public IntExpr Max(IntExpr other) => Context.Max(this, other);
}
