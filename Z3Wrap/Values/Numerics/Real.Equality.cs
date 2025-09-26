namespace Spaceorc.Z3Wrap.Values.Numerics;

public readonly partial struct Real
{
    /// <summary>
    /// Determines whether two rational numbers are equal using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the rational numbers are equal; otherwise, false.</returns>
    public static bool operator ==(Real left, Real right) =>
        left.numerator == right.numerator && left.denominator == right.denominator;

    /// <summary>
    /// Determines whether two rational numbers are not equal using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the rational numbers are not equal; otherwise, false.</returns>
    public static bool operator !=(Real left, Real right) => !(left == right);

    /// <summary>
    /// Determines whether this rational number is equal to another rational number.
    /// </summary>
    /// <param name="other">The rational number to compare with this rational number.</param>
    /// <returns>true if the rational numbers are equal; otherwise, false.</returns>
    public bool Equals(Real other) => this == other;

    /// <summary>
    /// Determines whether this rational number is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to compare with this rational number.</param>
    /// <returns>true if the object is a Real and is equal to this rational number; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Real other && Equals(other);

    /// <summary>
    /// Returns the hash code for this rational number.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(numerator, denominator);
}
