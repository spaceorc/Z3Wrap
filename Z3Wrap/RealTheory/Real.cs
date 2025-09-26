using System.Globalization;
using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

/// <summary>
/// Represents an exact rational number with unlimited precision arithmetic.
/// Supports natural mathematical operations with automatic reduction to lowest terms.
/// </summary>
public readonly struct Real : IEquatable<Real>, IComparable<Real>, IFormattable
{
    private readonly BigInteger numerator;
    private readonly BigInteger denominator;

    /// <summary>
    /// Initializes a new rational number from integer numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (defaults to 1).</param>
    public Real(int numerator, int denominator = 1)
        : this(new BigInteger(numerator), new BigInteger(denominator)) { }

    /// <summary>
    /// Initializes a new rational number from long integer numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (defaults to 1).</param>
    public Real(long numerator, long denominator = 1)
        : this(new BigInteger(numerator), new BigInteger(denominator)) { }

    /// <summary>
    /// Initializes a new rational number from a BigInteger numerator (denominator = 1).
    /// </summary>
    /// <param name="numerator">The numerator value.</param>
    public Real(BigInteger numerator)
        : this(numerator, BigInteger.One) { }

    /// <summary>
    /// Initializes a new rational number from a decimal value with exact precision.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    public Real(decimal value)
    {
        var bits = decimal.GetBits(value);
        var sign = (bits[3] & 0x80000000) != 0 ? -1 : 1;
        var scale = (bits[3] >> 16) & 0x7F;

        var num =
            new BigInteger(sign)
            * (
                new BigInteger((uint)bits[2]) << 64
                | new BigInteger((uint)bits[1]) << 32
                | new BigInteger((uint)bits[0])
            );

        var den = BigInteger.Pow(10, scale);

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
        numerator = num / gcd;
        denominator = den / gcd;
    }

    /// <summary>
    /// Initializes a new rational number from BigInteger numerator and denominator.
    /// The fraction is automatically reduced to its lowest terms.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (must be non-zero).</param>
    /// <exception cref="ArgumentException">Thrown when denominator is zero.</exception>
    public Real(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominator must be non-zero", nameof(denominator));

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
        this.numerator = numerator / gcd;
        this.denominator = denominator / gcd;
    }

    /// <summary>
    /// Gets the numerator of the rational number.
    /// </summary>
    public BigInteger Numerator => numerator;

    /// <summary>
    /// Gets the denominator of the rational number.
    /// </summary>
    public BigInteger Denominator => denominator;

    /// <summary>
    /// Gets a value indicating whether this rational number represents an integer.
    /// </summary>
    public bool IsInteger => denominator == 1;

    /// <summary>
    /// Gets a value indicating whether this rational number is zero.
    /// </summary>
    public bool IsZero => numerator == 0;

    /// <summary>
    /// Gets a value indicating whether this rational number is positive.
    /// </summary>
    public bool IsPositive => numerator > 0;

    /// <summary>
    /// Gets a value indicating whether this rational number is negative.
    /// </summary>
    public bool IsNegative => numerator < 0;

    /// <summary>
    /// Parses a string representation of a rational number.
    /// Supports integer, decimal, and fraction formats (e.g., "3/4", "1.5", "42").
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>A rational number representing the parsed value.</returns>
    /// <exception cref="FormatException">Thrown when the string is not in a valid format.</exception>
    public static Real Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new FormatException("Input string must not be null or empty");

        value = value.Trim();

        var slashIndex = value.IndexOf('/');
        if (slashIndex >= 0)
        {
            var numStr = value[..slashIndex].Trim();
            var denStr = value[(slashIndex + 1)..].Trim();

            if (!BigInteger.TryParse(numStr, out var num))
                throw new FormatException($"Invalid numerator: {numStr}");

            if (!BigInteger.TryParse(denStr, out var den))
                throw new FormatException($"Invalid denominator: {denStr}");

            try
            {
                return new Real(num, den);
            }
            catch (ArgumentException ex)
            {
                throw new FormatException($"Invalid fraction: {value}", ex);
            }
        }

        if (value.Contains('.'))
        {
            if (
                !decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var decimalValue
                )
            )
                throw new FormatException($"Invalid decimal format: {value}");

            return new Real(decimalValue);
        }

        if (!BigInteger.TryParse(value, out var intValue))
            throw new FormatException($"Invalid integer format: {value}");

        return new Real(intValue, 1);
    }

    /// <summary>
    /// Attempts to parse a string representation of a rational number.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The parsed rational number, or default if parsing fails.</param>
    /// <returns>true if parsing succeeded; otherwise, false.</returns>
    public static bool TryParse(string value, out Real result)
    {
        try
        {
            result = Parse(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

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
    /// Adds two rational numbers using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A rational number representing the sum.</returns>
    public static Real operator +(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator + right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Subtracts one rational number from another using the - operator.
    /// </summary>
    /// <param name="left">The left operand (minuend).</param>
    /// <param name="right">The right operand (subtrahend).</param>
    /// <returns>A rational number representing the difference.</returns>
    public static Real operator -(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator - right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Multiplies two rational numbers using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A rational number representing the product.</returns>
    public static Real operator *(Real left, Real right)
    {
        var newNum = left.numerator * right.numerator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Divides one rational number by another using the / operator.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A rational number representing the quotient.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public static Real operator /(Real left, Real right)
    {
        if (right.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        var newNum = left.numerator * right.denominator;
        var newDen = left.denominator * right.numerator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Negates a rational number using the unary - operator.
    /// </summary>
    /// <param name="value">The rational number to negate.</param>
    /// <returns>A rational number representing the negated value.</returns>
    public static Real operator -(Real value) => new(-value.numerator, value.denominator);

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
    /// Implicitly converts an integer to a rational number.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A rational number representing the integer.</returns>
    public static implicit operator Real(int value) => new(value);

    /// <summary>
    /// Implicitly converts a long integer to a rational number.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A rational number representing the long integer.</returns>
    public static implicit operator Real(long value) => new(value);

    /// <summary>
    /// Implicitly converts a decimal to a rational number.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>A rational number representing the decimal with exact precision.</returns>
    public static implicit operator Real(decimal value) => new(value);

    /// <summary>
    /// Implicitly converts a BigInteger to a rational number.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A rational number representing the BigInteger.</returns>
    public static implicit operator Real(BigInteger value) => new(value, 1);

    /// <summary>
    /// Explicitly converts a rational number to a decimal.
    /// </summary>
    /// <param name="value">The rational number to convert.</param>
    /// <returns>The decimal approximation of the rational number.</returns>
    public static explicit operator decimal(Real value) => value.ToDecimal();

    /// <summary>
    /// Explicitly converts a rational number to a BigInteger.
    /// </summary>
    /// <param name="value">The rational number to convert.</param>
    /// <returns>The BigInteger value (must be an integer).</returns>
    public static explicit operator BigInteger(Real value) => value.ToBigInteger();

    /// <summary>
    /// Explicitly converts a rational number to a long integer.
    /// </summary>
    /// <param name="value">The rational number to convert.</param>
    /// <returns>The long integer value (must be an integer within range).</returns>
    public static explicit operator long(Real value) => value.ToLong();

    /// <summary>
    /// Explicitly converts a rational number to an integer.
    /// </summary>
    /// <param name="value">The rational number to convert.</param>
    /// <returns>The integer value (must be an integer within range).</returns>
    public static explicit operator int(Real value) => value.ToInt();

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

    /// <summary>
    /// Returns a string representation of this rational number using the default format.
    /// </summary>
    /// <returns>A string representation of the rational number.</returns>
    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this rational number using the specified format.
    /// </summary>
    /// <param name="format">A format string (F/FRACTION for fraction format, D/DECIMAL for decimal format, G/GENERAL for general format).</param>
    /// <returns>A formatted string representation of the rational number.</returns>
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this rational number using the specified format and format provider.
    /// </summary>
    /// <param name="format">A format string (F/FRACTION for fraction format, D/DECIMAL for decimal format, G/GENERAL for general format).</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A formatted string representation of the rational number.</returns>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "F";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "F" or "FRACTION" => denominator == 1
                ? numerator.ToString(formatProvider)
                : $"{numerator}/{denominator}",
            "D" or "DECIMAL" => ToDecimal().ToString(formatProvider),
            "G" or "GENERAL" => denominator == 1
                ? numerator.ToString(formatProvider)
                : $"{numerator}/{denominator}",
            _ => throw new FormatException($"Invalid format string: {format}"),
        };
    }

    /// <summary>
    /// Represents the rational number zero (0/1).
    /// </summary>
    public static readonly Real Zero = new(0);

    /// <summary>
    /// Represents the rational number one (1/1).
    /// </summary>
    public static readonly Real One = new(1);

    /// <summary>
    /// Represents the rational number minus one (-1/1).
    /// </summary>
    public static readonly Real MinusOne = new(-1);

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
