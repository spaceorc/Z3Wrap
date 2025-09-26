namespace Spaceorc.Z3Wrap.IntTheory;

public sealed partial class Z3Int
{
    /// <summary>
    /// Computes the absolute value of this integer expression.
    /// </summary>
    /// <returns>An integer expression representing |this|.</returns>
    public Z3Int Abs() => Context.Abs(this);

    /// <summary>
    /// Computes the minimum of this integer expression and another integer expression.
    /// </summary>
    /// <param name="other">The other integer expression to compare with.</param>
    /// <returns>An integer expression representing min(this, other).</returns>
    public Z3Int Min(Z3Int other) => Context.Min(this, other);

    /// <summary>
    /// Computes the maximum of this integer expression and another integer expression.
    /// </summary>
    /// <param name="other">The other integer expression to compare with.</param>
    /// <returns>An integer expression representing max(this, other).</returns>
    public Z3Int Max(Z3Int other) => Context.Max(this, other);
}
