namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Defines IEEE 754 floating-point format with exponent and significand bit counts.
/// </summary>
public interface IFloatFormat
{
    /// <summary>
    /// Gets the number of exponent bits.
    /// </summary>
    static abstract uint ExponentBits { get; }

    /// <summary>
    /// Gets the number of significand bits (including implicit leading bit).
    /// </summary>
    static abstract uint SignificandBits { get; }
}
