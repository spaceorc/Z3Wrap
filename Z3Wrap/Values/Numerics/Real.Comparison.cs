namespace Spaceorc.Z3Wrap.Values.Numerics;

public readonly partial struct Real
{
    /// <summary>
    /// Determines whether the left rational number is less than the right using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is less than right; otherwise, false.</returns>
    public static bool operator <(Real left, Real right) =>
        left.numerator * right.denominator < right.numerator * left.denominator;

    /// <summary>
    /// Determines whether the left rational number is less than or equal to the right using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is less than or equal to right; otherwise, false.</returns>
    public static bool operator <=(Real left, Real right) =>
        left.numerator * right.denominator <= right.numerator * left.denominator;

    /// <summary>
    /// Determines whether the left rational number is greater than the right using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is greater than right; otherwise, false.</returns>
    public static bool operator >(Real left, Real right) =>
        left.numerator * right.denominator > right.numerator * left.denominator;

    /// <summary>
    /// Determines whether the left rational number is greater than or equal to the right using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if left is greater than or equal to right; otherwise, false.</returns>
    public static bool operator >=(Real left, Real right) =>
        left.numerator * right.denominator >= right.numerator * left.denominator;

    /// <summary>
    /// Compares this rational number to another rational number.
    /// </summary>
    /// <param name="other">The rational number to compare with this rational number.</param>
    /// <returns>A value less than 0 if this rational number is less than other; 0 if they are equal; greater than 0 if this rational number is greater than other.</returns>
    public int CompareTo(Real other)
    {
        if (this < other)
            return -1;
        if (this > other)
            return 1;
        return 0;
    }
}
