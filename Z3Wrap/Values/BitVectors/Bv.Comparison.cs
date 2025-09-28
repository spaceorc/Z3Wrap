namespace Spaceorc.Z3Wrap.Values.BitVectors;

public readonly partial struct Bv<TSize>
{
    /// <summary>
    /// Unsigned less-than comparison of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>true if left is less than right; otherwise, false.</returns>
    public static bool operator <(Bv<TSize> left, Bv<TSize> right) => left.Lt(right);

    /// <summary>
    /// Unsigned less-than-or-equal comparison of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>true if left is less than or equal to right; otherwise, false.</returns>
    public static bool operator <=(Bv<TSize> left, Bv<TSize> right) => left.Le(right);

    /// <summary>
    /// Unsigned greater-than comparison of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>true if left is greater than right; otherwise, false.</returns>
    public static bool operator >(Bv<TSize> left, Bv<TSize> right) => left.Gt(right);

    /// <summary>
    /// Unsigned greater-than-or-equal comparison of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>true if left is greater than or equal to right; otherwise, false.</returns>
    public static bool operator >=(Bv<TSize> left, Bv<TSize> right) => left.Ge(right);

    /// <summary>
    /// Determines whether this bitvector is less than another.
    /// </summary>
    /// <param name="other">Bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed comparison.</param>
    /// <returns>true if this bitvector is less than the other; otherwise, false.</returns>
    public bool Lt(Bv<TSize> other, bool signed = false)
    {
        if (!signed)
            return value < other.value;

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return leftSigned < rightSigned;
    }

    /// <summary>
    /// Determines whether this bitvector is greater than another.
    /// </summary>
    /// <param name="other">Bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed comparison.</param>
    /// <returns>true if this bitvector is greater than the other; otherwise, false.</returns>
    public bool Gt(Bv<TSize> other, bool signed = false)
    {
        if (!signed)
            return value > other.value;

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return leftSigned > rightSigned;
    }

    /// <summary>
    /// Determines whether this bitvector is less than or equal to another.
    /// </summary>
    /// <param name="other">Bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed comparison.</param>
    /// <returns>true if this bitvector is less than or equal to the other; otherwise, false.</returns>
    public bool Le(Bv<TSize> other, bool signed = false) => Lt(other, signed) || Equals(other);

    /// <summary>
    /// Determines whether this bitvector is greater than or equal to another.
    /// </summary>
    /// <param name="other">Bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed comparison.</param>
    /// <returns>true if this bitvector is greater than or equal to the other; otherwise, false.</returns>
    public bool Ge(Bv<TSize> other, bool signed = false) => Gt(other, signed) || Equals(other);

    /// <summary>
    /// Compares this bitvector to another using unsigned comparison.
    /// </summary>
    /// <param name="other">Bitvector to compare with.</param>
    /// <returns>Negative if less than, 0 if equal, positive if greater than.</returns>
    public int CompareTo(Bv<TSize> other) => value.CompareTo(other.value);
}
