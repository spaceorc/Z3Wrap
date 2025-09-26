using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

public readonly partial struct Real
{
    /// <summary>
    /// Returns the absolute value of this rational number.
    /// </summary>
    /// <returns>A rational number representing the absolute value.</returns>
    public Real Abs() => numerator < 0 ? -this : this;

    /// <summary>
    /// Returns the reciprocal (multiplicative inverse) of this rational number.
    /// </summary>
    /// <returns>A rational number representing 1/this.</returns>
    /// <exception cref="DivideByZeroException">Thrown when attempting to get the reciprocal of zero.</exception>
    public Real Reciprocal() =>
        IsZero
            ? throw new DivideByZeroException("Division by zero is not allowed")
            : new Real(denominator, numerator);

    /// <summary>
    /// Raises this rational number to the specified integer power.
    /// </summary>
    /// <param name="exponent">The exponent (can be negative for reciprocal powers).</param>
    /// <returns>A rational number representing this^exponent.</returns>
    public Real Power(int exponent)
    {
        if (exponent == 0)
            return new Real(1);

        if (exponent < 0)
            return Reciprocal().Power(-exponent);

        var result = new Real(1);
        var baseValue = this;

        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
                result *= baseValue;

            baseValue *= baseValue;
            exponent >>= 1;
        }

        return result;
    }

    /// <summary>
    /// Rounds this rational number to the nearest integer using the specified rounding mode.
    /// </summary>
    /// <param name="mode">The rounding mode to use (defaults to ToEven).</param>
    /// <returns>The rounded integer value as a BigInteger.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid rounding mode is specified.</exception>
    public BigInteger Round(MidpointRounding mode = MidpointRounding.ToEven)
    {
        if (IsInteger)
            return numerator;

        var quotient = numerator / denominator;
        var remainder = numerator % denominator;
        var absRemainder = BigInteger.Abs(remainder);
        var halfDenominator = denominator / 2;

        if (absRemainder < halfDenominator)
            return quotient;

        if (absRemainder > halfDenominator)
            return numerator >= 0 ? quotient + 1 : quotient - 1;

        return mode switch
        {
            MidpointRounding.ToEven => (quotient % 2 == 0)
                ? quotient
                : (numerator >= 0 ? quotient + 1 : quotient - 1),
            MidpointRounding.AwayFromZero => numerator >= 0 ? quotient + 1 : quotient - 1,
            MidpointRounding.ToZero => quotient,
            MidpointRounding.ToNegativeInfinity => numerator >= 0 ? quotient : quotient - 1,
            MidpointRounding.ToPositiveInfinity => numerator >= 0 ? quotient + 1 : quotient,
            _ => throw new ArgumentException($"Invalid rounding mode: {mode}"),
        };
    }

    /// <summary>
    /// Returns the smaller of two rational numbers.
    /// </summary>
    /// <param name="left">The first rational number to compare.</param>
    /// <param name="right">The second rational number to compare.</param>
    /// <returns>The rational number with the smaller value.</returns>
    public static Real Min(Real left, Real right) => left <= right ? left : right;

    /// <summary>
    /// Returns the larger of two rational numbers.
    /// </summary>
    /// <param name="left">The first rational number to compare.</param>
    /// <param name="right">The second rational number to compare.</param>
    /// <returns>The rational number with the larger value.</returns>
    public static Real Max(Real left, Real right) => left >= right ? left : right;
}
