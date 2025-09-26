namespace Spaceorc.Z3Wrap.Values.BitVectors;

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
}
