using System.Numerics;

namespace Spaceorc.Z3Wrap.BitVectors;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Converts the bitvector to a BigInteger value.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The BigInteger representation of the bitvector value.</returns>
    public BigInteger ToBigInteger(bool signed = false)
    {
        if (!signed)
            return value;

        // For signed interpretation: check if MSB (sign bit) is set
        var signBit = BigInteger.One << ((int)Size - 1);
        if ((value & signBit) == 0)
            return value; // Positive number, same as unsigned

        // MSB is set: convert from unsigned to signed using two's complement
        return value - (BigInteger.One << (int)Size);
    }

    /// <summary>
    /// Converts the bitvector to a 32-bit signed integer.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of int.</exception>
    public int ToInt(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > int.MaxValue || bigIntValue < int.MinValue)
            throw new OverflowException(
                $"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of int"
            );
        return (int)bigIntValue;
    }

    /// <summary>
    /// Converts the bitvector to a 32-bit unsigned integer.
    /// </summary>
    /// <returns>The unsigned integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of uint.</exception>
    public uint ToUInt()
    {
        if (value > uint.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of uint");
        return (uint)value;
    }

    /// <summary>
    /// Converts the bitvector to a 64-bit signed integer.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The long integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of long.</exception>
    public long ToLong(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > long.MaxValue || bigIntValue < long.MinValue)
            throw new OverflowException(
                $"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of long"
            );
        return (long)bigIntValue;
    }

    /// <summary>
    /// Converts the bitvector to a 64-bit unsigned integer.
    /// </summary>
    /// <returns>The unsigned long integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of ulong.</exception>
    public ulong ToULong()
    {
        if (value > ulong.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of ulong");
        return (ulong)value;
    }

    /// <summary>
    /// Converts the bitvector to a byte array in little-endian format.
    /// </summary>
    /// <returns>A byte array representing the bitvector value in little-endian format.</returns>
    public byte[] ToBytes() => ToBytes(Endianness.LittleEndian);

    /// <summary>
    /// Converts the bitvector to a byte array in the specified endianness.
    /// </summary>
    /// <param name="endianness">The byte order for the output array.</param>
    /// <returns>A byte array representing the bitvector value.</returns>
    public byte[] ToBytes(Endianness endianness)
    {
        var byteCount = (int)((Size + 7) / 8); // Round up to nearest byte
        var bytes = new byte[byteCount];
        CopyTo(bytes, endianness);
        return bytes;
    }

    /// <summary>
    /// Copies the bitvector bytes to the specified destination span.
    /// </summary>
    /// <param name="destination">The destination span to write the bytes to.</param>
    /// <param name="endianness">The byte order for the output (default: little-endian).</param>
    /// <exception cref="ArgumentException">Thrown when the destination span is too small.</exception>
    public void CopyTo(Span<byte> destination, Endianness endianness = Endianness.LittleEndian)
    {
        var byteCount = (int)((Size + 7) / 8); // Round up to nearest byte

        if (destination.Length < byteCount)
            throw new ArgumentException(
                $"Destination span length {destination.Length} is too small for {byteCount} bytes",
                nameof(destination)
            );

        var val = value;

        if (endianness == Endianness.LittleEndian)
        {
            // Little-endian: least significant byte first
            for (int i = 0; i < byteCount; i++)
            {
                destination[i] = (byte)(val & 0xFF);
                val >>= 8;
            }
        }
        else
        {
            // Big-endian: most significant byte first
            for (int i = byteCount - 1; i >= 0; i--)
            {
                destination[i] = (byte)(val & 0xFF);
                val >>= 8;
            }
        }
    }
}

/// <summary>
/// Specifies the byte order for multi-byte values.
/// </summary>
public enum Endianness
{
    /// <summary>
    /// Little-endian byte order (least significant byte first).
    /// </summary>
    LittleEndian,

    /// <summary>
    /// Big-endian byte order (most significant byte first).
    /// </summary>
    BigEndian
}
