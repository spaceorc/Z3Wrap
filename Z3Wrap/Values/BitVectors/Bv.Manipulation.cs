using System.Numerics;

namespace Spaceorc.Z3Wrap.Values.BitVectors;

public readonly partial struct Bv<TSize>
{
    /// <summary>
    /// Resizes this bitvector to a different bit width.
    /// </summary>
    /// <typeparam name="TNewSize">Target size type implementing ISize.</typeparam>
    /// <param name="signed">Whether to perform signed extension.</param>
    /// <returns>Resized bitvector with the target size.</returns>
    public Bv<TNewSize> Resize<TNewSize>(bool signed)
        where TNewSize : ISize
    {
        var newSize = TNewSize.Size;
        var currentSize = Size;

        if (newSize == currentSize)
        {
            // Same size - just change type
            return new Bv<TNewSize>(value);
        }

        if (newSize < currentSize)
        {
            // Truncation - constructor will handle the masking
            return new Bv<TNewSize>(value);
        }

        // Extension
        if (!signed)
        {
            // Zero extension - value is already correctly masked
            return new Bv<TNewSize>(value);
        }

        // Sign extension
        var signBit = BigInteger.One << ((int)currentSize - 1);
        var isNegative = (value & signBit) != 0;

        if (!isNegative)
        {
            // Positive number - zero extend
            return new Bv<TNewSize>(value);
        }

        // Negative number - sign extend by setting all higher bits
        var extensionMask = ((BigInteger.One << (int)newSize) - 1) ^ ((BigInteger.One << (int)currentSize) - 1);
        var signExtended = value | extensionMask;

        return new Bv<TNewSize>(signExtended);
    }

    /// <summary>
    /// Extracts a contiguous sequence of bits from this bitvector with compile-time type safety.
    /// </summary>
    /// <typeparam name="TResultSize">The size type of the result, determines how many bits to extract.</typeparam>
    /// <param name="startBit">The bit position to start extraction from (0 = LSB).</param>
    /// <returns>A new bitvector containing the extracted bits.</returns>
    /// <exception cref="ArgumentException">Thrown when the extraction range exceeds the bitvector bounds.</exception>
    /// <remarks>
    /// Extracts TResultSize.Value consecutive bits starting from startBit.
    /// For example, Extract&lt;Size8&gt;(8) extracts bits [15:8] as an 8-bit value.
    /// The extraction range must be entirely within the source bitvector bounds.
    /// </remarks>
    public Bv<TResultSize> Extract<TResultSize>(uint startBit)
        where TResultSize : ISize
    {
        var extractSize = TResultSize.Size;
        var currentSize = Size;

        // Validate extraction bounds
        if (startBit + extractSize > currentSize)
        {
            throw new ArgumentException(
                $"Cannot extract {extractSize} bits starting from bit {startBit} from {currentSize}-bit vector. "
                    + $"Extraction range [{startBit + extractSize - 1}:{startBit}] exceeds available bits [0:{currentSize - 1}]."
            );
        }

        // Extract: shift right to align the desired bits to LSB, then mask to result size
        var shifted = value >> (int)startBit;

        // The constructor will handle masking to TResultSize, so we don't need to mask here
        return new Bv<TResultSize>(shifted);
    }

    /// <summary>
    /// Rotates the bits of this bitvector to the left by the specified number of positions.
    /// </summary>
    /// <param name="positions">The number of positions to rotate left (0 to Size-1).</param>
    /// <returns>A new bitvector with bits rotated left.</returns>
    /// <exception cref="ArgumentException">Thrown when positions is negative or >= Size.</exception>
    public Bv<TSize> RotateLeft(int positions)
    {
        if (positions < 0)
            throw new ArgumentException("Positions must be non-negative", nameof(positions));

        var size = (int)Size;
        positions %= size; // Handle positions >= size

        if (positions == 0)
            return this;

        // Rotate: (value << positions) | (value >> (size - positions))
        var leftShifted = value << positions;
        var rightShifted = value >> (size - positions);

        return new Bv<TSize>(leftShifted | rightShifted);
    }

    /// <summary>
    /// Rotates the bits of this bitvector to the right by the specified number of positions.
    /// </summary>
    /// <param name="positions">The number of positions to rotate right (0 to Size-1).</param>
    /// <returns>A new bitvector with bits rotated right.</returns>
    /// <exception cref="ArgumentException">Thrown when positions is negative or >= Size.</exception>
    public Bv<TSize> RotateRight(int positions)
    {
        if (positions < 0)
            throw new ArgumentException("Positions must be non-negative", nameof(positions));

        var size = (int)Size;
        positions %= size; // Handle positions >= size

        if (positions == 0)
            return this;

        // Rotate: (value >> positions) | (value << (size - positions))
        var rightShifted = value >> positions;
        var leftShifted = value << (size - positions);

        return new Bv<TSize>(rightShifted | leftShifted);
    }

    /// <summary>
    /// Counts the number of set bits (1s) in this bitvector.
    /// </summary>
    /// <returns>The number of set bits.</returns>
    public uint PopCount()
    {
        return (uint)BigInteger.PopCount(value);
    }

    /// <summary>
    /// Counts the number of leading zero bits (from most significant bit).
    /// </summary>
    /// <returns>The number of leading zero bits (0 to Size).</returns>
    public uint CountLeadingZeros()
    {
        if (value == 0)
            return Size;

        var bitLength = (uint)value.GetBitLength();
        return Size - bitLength;
    }

    /// <summary>
    /// Counts the number of trailing zero bits (from least significant bit).
    /// </summary>
    /// <returns>The number of trailing zero bits (0 to Size).</returns>
    public uint CountTrailingZeros()
    {
        if (value == 0)
            return Size;

        uint count = 0;
        var val = value;

        while ((val & 1) == 0)
        {
            count++;
            val >>= 1;
        }

        return count;
    }

    /// <summary>
    /// Concatenates two bitvectors to form a larger bitvector with compile-time type safety.
    /// </summary>
    /// <typeparam name="TLowSize">The size type of the low (right) bitvector.</typeparam>
    /// <typeparam name="TResultSize">The size type of the result bitvector.</typeparam>
    /// <param name="low">The bitvector that will occupy the lower bits.</param>
    /// <returns>A new bitvector with this bitvector in the high bits and low in the low bits.</returns>
    /// <remarks>
    /// The result size must equal the sum of the input sizes: TResultSize.Value == TSize.Value + TLowSize.Value.
    /// This bitvector becomes the high-order bits, and the low parameter becomes the low-order bits.
    /// </remarks>
    public Bv<TResultSize> Concat<TLowSize, TResultSize>(Bv<TLowSize> low)
        where TLowSize : ISize
        where TResultSize : ISize
    {
        var highSize = Size;
        var lowSize = TLowSize.Size;
        var resultSize = TResultSize.Size;

        if (resultSize != highSize + lowSize)
        {
            throw new ArgumentException(
                $"Result size {resultSize} must equal sum of input sizes ({highSize} + {lowSize} = {highSize + lowSize})"
            );
        }

        // Shift this bitvector to the high bits and OR with low bitvector
        var highValue = value << (int)lowSize;
        var result = highValue | low.Value;

        return new Bv<TResultSize>(result);
    }

    /// <summary>
    /// Repeats this bitvector multiple times to create a larger bitvector with compile-time type safety.
    /// </summary>
    /// <typeparam name="TResultSize">The size type of the result bitvector.</typeparam>
    /// <returns>A new bitvector containing this bitvector repeated to fill the target size.</returns>
    /// <exception cref="ArgumentException">Thrown when the result size is not a multiple of this bitvector's size.</exception>
    /// <remarks>
    /// The result size must be an exact multiple of this bitvector's size.
    /// For example, an 8-bit bitvector can be repeated to create 16, 24, 32-bit results, but not 12 or 20-bit.
    /// The pattern is repeated from LSB to MSB: repeat(0x3, 4→8) = 0x33, repeat(0x3, 4→12) = 0x333.
    /// </remarks>
    public Bv<TResultSize> Repeat<TResultSize>()
        where TResultSize : ISize
    {
        var inputSize = Size;
        var resultSize = TResultSize.Size;

        if (resultSize % inputSize != 0)
            throw new ArgumentException($"Target size {resultSize} must be a multiple of source size {inputSize}");

        var repeatCount = resultSize / inputSize;
        if (repeatCount == 1)
            return new Bv<TResultSize>(value);

        BigInteger result = 0;
        for (uint i = 0; i < repeatCount; i++)
        {
            result |= value << (int)(i * inputSize);
        }

        return new Bv<TResultSize>(result);
    }
}
