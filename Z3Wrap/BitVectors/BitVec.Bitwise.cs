namespace Spaceorc.Z3Wrap.BitVectors;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Performs bitwise AND operation on two bitvectors using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the result of the bitwise AND operation.</returns>
    public static BitVec<TSize> operator &(BitVec<TSize> left, BitVec<TSize> right) =>
        left.And(right);

    /// <summary>
    /// Performs bitwise OR operation on two bitvectors using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the result of the bitwise OR operation.</returns>
    public static BitVec<TSize> operator |(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Or(right);

    /// <summary>
    /// Performs bitwise XOR (exclusive OR) operation on two bitvectors using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the result of the bitwise XOR operation.</returns>
    public static BitVec<TSize> operator ^(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Xor(right);

    /// <summary>
    /// Performs bitwise NOT operation on a bitvector using the ~ operator.
    /// Inverts all bits within the bitvector's bit width.
    /// </summary>
    /// <param name="operand">The operand to invert.</param>
    /// <returns>A new bitvector containing the result of the bitwise NOT operation.</returns>
    public static BitVec<TSize> operator ~(BitVec<TSize> operand) => new(~operand.value);

    /// <summary>
    /// Performs left shift operation on a bitvector using the &lt;&lt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="shift">The number of bits to shift left.</param>
    /// <returns>A new bitvector containing the result of the left shift operation.</returns>
    public static BitVec<TSize> operator <<(BitVec<TSize> left, int shift) => left.Shl(shift);

    /// <summary>
    /// Performs right shift operation on a bitvector using the &gt;&gt; operator.
    /// Performs logical (unsigned) right shift.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="shift">The number of bits to shift right.</param>
    /// <returns>A new bitvector containing the result of the right shift operation.</returns>
    public static BitVec<TSize> operator >>(BitVec<TSize> left, int shift) => left.Shr(shift);

    /// <summary>
    /// Performs bitwise AND operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to AND with.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    public BitVec<TSize> And(BitVec<TSize> other) => new(value & other.value);

    /// <summary>
    /// Performs bitwise OR operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to OR with.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    public BitVec<TSize> Or(BitVec<TSize> other) => new(value | other.value);

    /// <summary>
    /// Performs bitwise XOR operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to XOR with.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    public BitVec<TSize> Xor(BitVec<TSize> other) => new(value ^ other.value);

    /// <summary>
    /// Performs left bit shift operation.
    /// </summary>
    /// <param name="shift">The number of positions to shift left (must be non-negative).</param>
    /// <returns>A new bitvector with bits shifted left, masked to the bit width.</returns>
    /// <exception cref="ArgumentException">Thrown when shift amount is negative.</exception>
    public BitVec<TSize> Shl(int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));
        return new BitVec<TSize>(value << shift);
    }

    /// <summary>
    /// Performs right bit shift operation.
    /// </summary>
    /// <param name="shift">The number of positions to shift right (must be non-negative).</param>
    /// <param name="signed">Whether to perform arithmetic shift (preserve sign bit) or logical shift (fill with zeros).</param>
    /// <returns>A new bitvector with bits shifted right.</returns>
    /// <exception cref="ArgumentException">Thrown when shift amount is negative.</exception>
    public BitVec<TSize> Shr(int shift, bool signed = false)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));

        if (!signed)
        {
            // Logical right shift - fill with zeros
            return new BitVec<TSize>(value >> shift);
        }

        // Arithmetic right shift - preserve sign bit
        var signedValue = ToBigInteger(signed: true);
        var result = signedValue >> shift;
        return new BitVec<TSize>(result);
    }
}
