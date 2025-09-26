using System.Globalization;
using System.Numerics;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Creates a bitvector from a byte array, interpreting bytes in little-endian order.
    /// </summary>
    /// <param name="bytes">The byte array to convert (little-endian).</param>
    /// <returns>A new bitvector containing the value from the byte array.</returns>
    /// <exception cref="ArgumentException">Thrown when the byte array is too large for the bitvector size.</exception>
    public static BitVec<TSize> FromBytes(ReadOnlySpan<byte> bytes)
    {
        var maxBytes = (Size + 7) / 8; // Round up to nearest byte
        if (bytes.Length > maxBytes)
            throw new ArgumentException(
                $"Byte array length {bytes.Length} exceeds maximum {maxBytes} bytes for {Size}-bit bitvector"
            );

        var value = BigInteger.Zero;
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            value = (value << 8) | bytes[i];
        }

        return new BitVec<TSize>(value);
    }

    /// <summary>
    /// Creates a bitvector from a hexadecimal string.
    /// </summary>
    /// <param name="hex">The hexadecimal string (with or without 0x prefix).</param>
    /// <returns>A new bitvector containing the parsed hexadecimal value.</returns>
    /// <exception cref="ArgumentException">Thrown when the hex string is invalid or too large.</exception>
    public static BitVec<TSize> FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Hex string cannot be null or empty", nameof(hex));

        // Remove 0x prefix if present
        var cleanHex = hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hex[2..] : hex;

        // Check max hex digits for this size
        var maxHexDigits = (Size + 3) / 4; // Round up to nearest hex digit
        if (cleanHex.Length > maxHexDigits)
            throw new ArgumentException(
                $"Hex string length {cleanHex.Length} exceeds maximum {maxHexDigits} digits for {Size}-bit bitvector"
            );

        try
        {
            var value = BigInteger.Parse(cleanHex, NumberStyles.HexNumber);
            return new BitVec<TSize>(value);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException($"Invalid hexadecimal string: {hex}", nameof(hex), ex);
        }
    }

    /// <summary>
    /// Creates a bitvector from a binary string.
    /// </summary>
    /// <param name="binary">The binary string (with or without 0b prefix, only '0' and '1' characters).</param>
    /// <returns>A new bitvector containing the parsed binary value.</returns>
    /// <exception cref="ArgumentException">Thrown when the binary string is invalid or too large.</exception>
    public static BitVec<TSize> FromBinary(string binary)
    {
        if (string.IsNullOrWhiteSpace(binary))
            throw new ArgumentException("Binary string cannot be null or empty", nameof(binary));

        // Remove 0b prefix if present
        var cleanBinary = binary.StartsWith("0b", StringComparison.OrdinalIgnoreCase)
            ? binary[2..]
            : binary;

        // Check max binary digits for this size
        if (cleanBinary.Length > Size)
            throw new ArgumentException(
                $"Binary string length {cleanBinary.Length} exceeds maximum {Size} bits for {Size}-bit bitvector"
            );

        // Validate binary string contains only 0s and 1s
        foreach (char c in cleanBinary)
        {
            if (c != '0' && c != '1')
                throw new ArgumentException(
                    $"Invalid binary character '{c}' in string: {binary}",
                    nameof(binary)
                );
        }

        var value = BigInteger.Zero;
        foreach (char bit in cleanBinary)
        {
            value = (value << 1) | (bit == '1' ? 1 : 0);
        }

        return new BitVec<TSize>(value);
    }
}
