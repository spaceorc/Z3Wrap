using System.Globalization;

namespace Spaceorc.Z3Wrap.Values.Numerics;

public readonly partial struct Real
{
    /// <summary>
    /// Returns a string representation of this rational number using the default format.
    /// </summary>
    /// <returns>A string representation of the rational number.</returns>
    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this rational number using the specified format.
    /// </summary>
    /// <param name="format">A format string (F/FRACTION for fraction format, D/DECIMAL for decimal format, G/GENERAL for general format).</param>
    /// <returns>A formatted string representation of the rational number.</returns>
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this rational number using the specified format and format provider.
    /// </summary>
    /// <param name="format">A format string (F/FRACTION for fraction format, D/DECIMAL for decimal format, G/GENERAL for general format).</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A formatted string representation of the rational number.</returns>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "G";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "F" or "FRACTION" => $"{numerator}/{denominator}",
            "D" or "DECIMAL" => ToDecimal().ToString(formatProvider),
            "G" or "GENERAL" => denominator == 1 ? numerator.ToString(formatProvider) : $"{numerator}/{denominator}",
            _ => throw new FormatException($"Invalid format string: {format}"),
        };
    }
}
