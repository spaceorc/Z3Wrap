using System.Globalization;

namespace Spaceorc.Z3Wrap.Values.BitVectors;

public readonly partial struct Bv<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Returns a string representation of this bitvector using the default format.
    /// </summary>
    /// <returns>A string representation of the bitvector value.</returns>
    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this bitvector using the specified format.
    /// </summary>
    /// <param name="format">A format string (V/VALUE for value only, D/DECIMAL for decimal with size, B/BINARY for binary with size, X/HEX for hexadecimal with size).</param>
    /// <returns>A formatted string representation of the bitvector.</returns>
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this bitvector using the specified format and format provider.
    /// </summary>
    /// <param name="format">A format string (V/VALUE for value only, D/DECIMAL for decimal with size, B/BINARY for binary with size, X/HEX for hexadecimal with size).</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A formatted string representation of the bitvector.</returns>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "V";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "D" or "DECIMAL" => $"{value} ({Size}-bit)",
            "B" or "BINARY" => $"0b{ToBinaryString()} ({Size}-bit)",
            "X" or "HEX" => $"0x{value.ToString("X").TrimStart('0').PadLeft(1, '0')} ({Size}-bit)",
            "V" or "VALUE" => value.ToString(formatProvider),
            _ => throw new FormatException($"Invalid format string: {format}"),
        };
    }

    /// <summary>
    /// Converts the bitvector to its binary string representation.
    /// </summary>
    /// <returns>A string of '0' and '1' characters representing the bitvector value, padded to the full bit width.</returns>
    public string ToBinaryString()
    {
        if (value == 0)
            return new string('0', (int)Size);

        Span<char> buffer = stackalloc char[(int)Size];
        var val = value;
        int pos = (int)Size - 1;

        // Fill from right to left (LSB to MSB)
        while (val > 0 && pos >= 0)
        {
            buffer[pos--] = (char)('0' + (val & 1));
            val >>= 1;
        }

        // Fill remaining positions with zeros
        while (pos >= 0)
            buffer[pos--] = '0';

        return new string(buffer);
    }

    /// <summary>
    /// Converts the bitvector to its hexadecimal string representation.
    /// </summary>
    /// <returns>A string of hexadecimal characters representing the bitvector value, padded to the full hex width.</returns>
    public string ToHexString()
    {
        var hexDigits = (Size + 3) / 4; // Round up to nearest hex digit
        return value.ToString("X").PadLeft((int)hexDigits, '0');
    }

    /// <summary>
    /// Tries to format this bitvector into the provided span of characters.
    /// </summary>
    /// <param name="destination">The span to write the formatted string to.</param>
    /// <param name="charsWritten">The number of characters written to the span.</param>
    /// <param name="format">A format string (V/VALUE for value only, D/DECIMAL for decimal with size, B/BINARY for binary with size, X/HEX for hexadecimal with size).</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>true if the formatting was successful; otherwise, false.</returns>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        var formatString = format.IsEmpty ? "V" : format.ToString();
        var formattedString = ToString(formatString, provider ?? CultureInfo.InvariantCulture);

        if (formattedString.Length <= destination.Length)
        {
            formattedString.AsSpan().CopyTo(destination);
            charsWritten = formattedString.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }
}
