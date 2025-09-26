namespace Spaceorc.Z3Wrap.BitVecTheory;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Determines whether one bitvector is less than another using the &lt; operator.
    /// Performs unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is less than right; otherwise, false.</returns>
    public static bool operator <(BitVec<TSize> left, BitVec<TSize> right) => left.Lt(right);

    /// <summary>
    /// Determines whether one bitvector is less than or equal to another using the &lt;= operator.
    /// Performs unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is less than or equal to right; otherwise, false.</returns>
    public static bool operator <=(BitVec<TSize> left, BitVec<TSize> right) => left.Le(right);

    /// <summary>
    /// Determines whether one bitvector is greater than another using the &gt; operator.
    /// Performs unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is greater than right; otherwise, false.</returns>
    public static bool operator >(BitVec<TSize> left, BitVec<TSize> right) => left.Gt(right);

    /// <summary>
    /// Determines whether one bitvector is greater than or equal to another using the &gt;= operator.
    /// Performs unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is greater than or equal to right; otherwise, false.</returns>
    public static bool operator >=(BitVec<TSize> left, BitVec<TSize> right) => left.Ge(right);

    /// <summary>
    /// Determines whether this bitvector is less than another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>true if this bitvector is less than the other; otherwise, false.</returns>
    public bool Lt(BitVec<TSize> other, bool signed = false)
    {
        if (!signed)
            return value < other.value;

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return leftSigned < rightSigned;
    }

    /// <summary>
    /// Determines whether this bitvector is greater than another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>true if this bitvector is greater than the other; otherwise, false.</returns>
    public bool Gt(BitVec<TSize> other, bool signed = false)
    {
        if (!signed)
            return value > other.value;

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return leftSigned > rightSigned;
    }

    /// <summary>
    /// Determines whether this bitvector is less than or equal to another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>true if this bitvector is less than or equal to the other; otherwise, false.</returns>
    public bool Le(BitVec<TSize> other, bool signed = false) => Lt(other, signed) || Equals(other);

    /// <summary>
    /// Determines whether this bitvector is greater than or equal to another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>true if this bitvector is greater than or equal to the other; otherwise, false.</returns>
    public bool Ge(BitVec<TSize> other, bool signed = false) => Gt(other, signed) || Equals(other);

    /// <summary>
    /// Compares this bitvector to another bitvector using unsigned comparison.
    /// </summary>
    /// <param name="other">The bitvector to compare with this bitvector.</param>
    /// <returns>A value less than 0 if this bitvector is less than other; 0 if they are equal; greater than 0 if this bitvector is greater than other.</returns>
    public int CompareTo(BitVec<TSize> other) => value.CompareTo(other.value);
}
