using System.Globalization;
using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

public readonly partial struct Real
{
    /// <summary>
    /// Represents the rational number zero (0/1).
    /// </summary>
    public static readonly Real Zero = new(0);

    /// <summary>
    /// Represents the rational number one (1/1).
    /// </summary>
    public static readonly Real One = new(1);

    /// <summary>
    /// Represents the rational number minus one (-1/1).
    /// </summary>
    public static readonly Real MinusOne = new(-1);

    /// <summary>
    /// Parses a string representation of a rational number.
    /// Supports integer, decimal, and fraction formats (e.g., "3/4", "1.5", "42").
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>A rational number representing the parsed value.</returns>
    /// <exception cref="FormatException">Thrown when the string is not in a valid format.</exception>
    public static Real Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new FormatException("Input string must not be null or empty");

        value = value.Trim();

        var slashIndex = value.IndexOf('/');
        if (slashIndex >= 0)
        {
            var numStr = value[..slashIndex].Trim();
            var denStr = value[(slashIndex + 1)..].Trim();

            if (!BigInteger.TryParse(numStr, out var num))
                throw new FormatException($"Invalid numerator: {numStr}");

            if (!BigInteger.TryParse(denStr, out var den))
                throw new FormatException($"Invalid denominator: {denStr}");

            try
            {
                return new Real(num, den);
            }
            catch (ArgumentException ex)
            {
                throw new FormatException($"Invalid fraction: {value}", ex);
            }
        }

        if (value.Contains('.'))
        {
            if (
                !decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var decimalValue
                )
            )
                throw new FormatException($"Invalid decimal format: {value}");

            return new Real(decimalValue);
        }

        if (!BigInteger.TryParse(value, out var intValue))
            throw new FormatException($"Invalid integer format: {value}");

        return new Real(intValue, 1);
    }

    /// <summary>
    /// Attempts to parse a string representation of a rational number.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The parsed rational number, or default if parsing fails.</param>
    /// <returns>true if parsing succeeded; otherwise, false.</returns>
    public static bool TryParse(string value, out Real result)
    {
        try
        {
            result = Parse(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}
