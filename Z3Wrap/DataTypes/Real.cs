using System.Globalization;
using System.Numerics;

namespace Spaceorc.Z3Wrap.DataTypes;

public readonly struct Real : IEquatable<Real>, IComparable<Real>, IFormattable
{
    private readonly BigInteger numerator;
    private readonly BigInteger denominator;

    // Constructors
    public Real(int numerator, int denominator = 1) : this(new BigInteger(numerator), new BigInteger(denominator)) { }
    public Real(long numerator, long denominator = 1) : this(new BigInteger(numerator), new BigInteger(denominator)) { }
    public Real(BigInteger numerator) : this(numerator, BigInteger.One) { }

    public Real(decimal value)
    {
        var bits = decimal.GetBits(value);
        var sign = (bits[3] & 0x80000000) != 0 ? -1 : 1;
        var scale = (bits[3] >> 16) & 0x7F;
        
        var num = new BigInteger(sign) * 
                  (new BigInteger((uint)bits[2]) << 64 | 
                   new BigInteger((uint)bits[1]) << 32 | 
                   new BigInteger((uint)bits[0]));
        
        var den = BigInteger.Pow(10, scale);

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
        numerator = num / gcd;
        denominator = den / gcd;
    }

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

    // Properties
    public BigInteger Numerator => numerator;
    public BigInteger Denominator => denominator;
    public bool IsInteger => denominator == 1;
    public bool IsZero => numerator == 0;
    public bool IsPositive => numerator > 0;
    public bool IsNegative => numerator < 0;

    // Static factory methods
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
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var decimalValue))
                throw new FormatException($"Invalid decimal format: {value}");

            return new Real(decimalValue);
        }

        if (!BigInteger.TryParse(value, out var intValue))
            throw new FormatException($"Invalid integer format: {value}");

        return new Real(intValue, 1);
    }

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

    // Conversion methods
    public int ToInt()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to int");
        
        if (numerator > int.MaxValue || numerator < int.MinValue)
            throw new OverflowException($"Value {this} is outside the range of int");
        
        return (int)numerator;
    }

    public long ToLong()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to long");
        
        if (numerator > long.MaxValue || numerator < long.MinValue)
            throw new OverflowException($"Value {this} is outside the range of long");
        
        return (long)numerator;
    }

    public BigInteger ToBigInteger()
    {
        if (!IsInteger)
            throw new InvalidOperationException("Cannot convert non-integer value to BigInteger");
        
        return numerator;
    }

    public decimal ToDecimal()
    {
        if (denominator == 1)
            return (decimal)numerator;

        return (decimal)numerator / (decimal)denominator;
    }

    // Mathematical operations
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
            MidpointRounding.ToEven => (quotient % 2 == 0) ? quotient : (numerator >= 0 ? quotient + 1 : quotient - 1),
            MidpointRounding.AwayFromZero => numerator >= 0 ? quotient + 1 : quotient - 1,
            MidpointRounding.ToZero => quotient,
            MidpointRounding.ToNegativeInfinity => numerator >= 0 ? quotient : quotient - 1,
            MidpointRounding.ToPositiveInfinity => numerator >= 0 ? quotient + 1 : quotient,
            _ => throw new ArgumentException($"Invalid rounding mode: {mode}")
        };
    }

    public Real Abs() => numerator < 0 ? -this : this;
    
    public Real Reciprocal() =>
        IsZero
            ? throw new DivideByZeroException("Division by zero is not allowed")
            : new Real(denominator, numerator);

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

    // Arithmetic operators
    public static Real operator +(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator + right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator -(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator - right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator *(Real left, Real right)
    {
        var newNum = left.numerator * right.numerator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator /(Real left, Real right)
    {
        if (right.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        var newNum = left.numerator * right.denominator;
        var newDen = left.denominator * right.numerator;
        return new Real(newNum, newDen);
    }

    public static Real operator -(Real value) => new(-value.numerator, value.denominator);

    // Comparison operators
    public static bool operator ==(Real left, Real right) => left.numerator == right.numerator && left.denominator == right.denominator;
    public static bool operator !=(Real left, Real right) => !(left == right);
    public static bool operator <(Real left, Real right) => left.numerator * right.denominator < right.numerator * left.denominator;
    public static bool operator <=(Real left, Real right) => left.numerator * right.denominator <= right.numerator * left.denominator;
    public static bool operator >(Real left, Real right) => left.numerator * right.denominator > right.numerator * left.denominator;
    public static bool operator >=(Real left, Real right) => left.numerator * right.denominator >= right.numerator * left.denominator;

    // Conversion operators
    public static implicit operator Real(int value) => new(value);
    public static implicit operator Real(long value) => new(value);
    public static implicit operator Real(decimal value) => new(value);
    public static implicit operator Real(BigInteger value) => new(value, 1);

    public static explicit operator decimal(Real value) => value.ToDecimal();
    public static explicit operator BigInteger(Real value) => value.ToBigInteger();
    public static explicit operator long(Real value) => value.ToLong();
    public static explicit operator int(Real value) => value.ToInt();

    // Interface implementations
    public bool Equals(Real other) => this == other;
    public override bool Equals(object? obj) => obj is Real other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(numerator, denominator);

    public int CompareTo(Real other)
    {
        if (this < other) return -1;
        if (this > other) return 1;
        return 0;
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "F";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "F" or "FRACTION" => denominator == 1 ? numerator.ToString(formatProvider) : $"{numerator}/{denominator}",
            "D" or "DECIMAL" => ToDecimal().ToString(formatProvider),
            "G" or "GENERAL" => denominator == 1 ? numerator.ToString(formatProvider) : $"{numerator}/{denominator}",
            _ => throw new FormatException($"Invalid format string: {format}")
        };
    }

    // Static constants and utility methods
    public static readonly Real Zero = new(0);
    public static readonly Real One = new(1);
    public static readonly Real MinusOne = new(-1);

    public static Real Min(Real left, Real right) => left <= right ? left : right;
    public static Real Max(Real left, Real right) => left >= right ? left : right;
}