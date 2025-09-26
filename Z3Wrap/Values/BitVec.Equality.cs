namespace Spaceorc.Z3Wrap.Values;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Determines whether two bitvectors are equal using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the bitvectors have the same value; otherwise, false.</returns>
    public static bool operator ==(BitVec<TSize> left, BitVec<TSize> right) => left.Equals(right);

    /// <summary>
    /// Determines whether two bitvectors are not equal using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the bitvectors have different values; otherwise, false.</returns>
    public static bool operator !=(BitVec<TSize> left, BitVec<TSize> right) => !left.Equals(right);

    /// <summary>
    /// Determines whether this bitvector is equal to another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <returns>true if the bitvectors have the same value; otherwise, false.</returns>
    public bool Equals(BitVec<TSize> other) => value == other.value;

    /// <summary>
    /// Determines whether this bitvector is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to compare with this bitvector.</param>
    /// <returns>true if the object is a BitVec&lt;TSize&gt; and is equal to this bitvector; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is BitVec<TSize> other && Equals(other);

    /// <summary>
    /// Returns the hash code for this bitvector.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(value, Size);
}
