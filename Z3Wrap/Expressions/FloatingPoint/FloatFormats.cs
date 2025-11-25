namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Represents IEEE 754 half-precision (16-bit) floating-point format.
/// </summary>
public struct Float16 : IFloatFormat
{
    /// <inheritdoc />
    public static uint ExponentBits => 5;

    /// <inheritdoc />
    public static uint SignificandBits => 11;
}

/// <summary>
/// Represents IEEE 754 single-precision (32-bit) floating-point format.
/// </summary>
public struct Float32 : IFloatFormat
{
    /// <inheritdoc />
    public static uint ExponentBits => 8;

    /// <inheritdoc />
    public static uint SignificandBits => 24;
}

/// <summary>
/// Represents IEEE 754 double-precision (64-bit) floating-point format.
/// </summary>
public struct Float64 : IFloatFormat
{
    /// <inheritdoc />
    public static uint ExponentBits => 11;

    /// <inheritdoc />
    public static uint SignificandBits => 53;
}
