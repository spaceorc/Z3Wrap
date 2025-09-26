using System.Numerics;

namespace Spaceorc.Z3Wrap.Values;

public readonly partial struct Real
{
    /// <summary>
    /// Converts this rational number to a 32-bit signed integer.
    /// </summary>
    /// <returns>The integer value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the rational number is not an integer.</exception>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of int.</exception>
    public int ToInt()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to int");

        if (numerator > int.MaxValue || numerator < int.MinValue)
            throw new OverflowException($"Value {this} is outside the range of int");

        return (int)numerator;
    }

    /// <summary>
    /// Converts this rational number to a 64-bit signed integer.
    /// </summary>
    /// <returns>The long integer value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the rational number is not an integer.</exception>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of long.</exception>
    public long ToLong()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to long");

        if (numerator > long.MaxValue || numerator < long.MinValue)
            throw new OverflowException($"Value {this} is outside the range of long");

        return (long)numerator;
    }

    /// <summary>
    /// Converts this rational number to a BigInteger.
    /// </summary>
    /// <returns>The BigInteger value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the rational number is not an integer.</exception>
    public BigInteger ToBigInteger()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to BigInteger");

        return numerator;
    }

    /// <summary>
    /// Converts this rational number to a decimal value.
    /// </summary>
    /// <returns>The decimal approximation of the rational number.</returns>
    public decimal ToDecimal()
    {
        if (denominator == 1)
            return (decimal)numerator;

        return (decimal)numerator / (decimal)denominator;
    }
}
