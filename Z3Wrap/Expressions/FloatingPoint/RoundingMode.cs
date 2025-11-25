namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// IEEE 754 rounding modes for floating-point operations.
/// </summary>
public enum RoundingMode
{
    /// <summary>
    /// Round to nearest, ties to even (default IEEE 754 mode).
    /// </summary>
    NearestTiesToEven,

    /// <summary>
    /// Round to nearest, ties away from zero.
    /// </summary>
    NearestTiesToAway,

    /// <summary>
    /// Round toward positive infinity.
    /// </summary>
    TowardPositive,

    /// <summary>
    /// Round toward negative infinity.
    /// </summary>
    TowardNegative,

    /// <summary>
    /// Round toward zero (truncate).
    /// </summary>
    TowardZero,
}
