using System.Globalization;
using System.Numerics;

namespace Z3Wrap;

public readonly struct Real : IEquatable<Real>, IComparable<Real>, IFormattable
{
    private readonly BigInteger numerator;
    private readonly BigInteger denominator;

    public Real(int numerator, int denominator = 1) : this(new BigInteger(numerator), new BigInteger(denominator))
    {
    }

    public Real(long numerator, long denominator = 1) : this(new BigInteger(numerator), new BigInteger(denominator))
    {
    }

    public Real(decimal value)
    {
        var real = FromDecimal(value);
        numerator = real.numerator;
        denominator = real.denominator;
    }

    public Real(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominator cannot be zero", nameof(denominator));

        // Handle negative denominators by moving sign to numerator
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        // Simplify the fraction by dividing by GCD
        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
        this.numerator = numerator / gcd;
        this.denominator = denominator / gcd;
    }

    public BigInteger Numerator => numerator;

    public BigInteger Denominator => denominator == 0 ? 1 : denominator;

    public bool IsInteger => Denominator == 1;

    public bool IsZero => numerator == 0;

    public bool IsPositive => numerator > 0;

    public bool IsNegative => numerator < 0;

    public static Real FromInt(int value) => new(value);

    public static Real FromLong(long value) => new(value);

    public static Real Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new FormatException("Input string cannot be null or empty");

        value = value.Trim();

        // Handle fraction format "num/den"
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

        // Handle decimal format "3.14" by converting to fraction
        if (value.Contains('.'))
        {
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var decimalValue))
                throw new FormatException($"Invalid decimal format: {value}");

            return FromDecimal(decimalValue);
        }

        // Handle integer format "123"
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

    public static Real FromDecimal(decimal value)
    {
        // Convert decimal to fraction using its internal representation
        var bits = decimal.GetBits(value);
        var sign = (bits[3] & 0x80000000) != 0 ? -1 : 1;
        var scale = (bits[3] >> 16) & 0x7F;
        
        var numerator = new BigInteger(sign) * 
                       (new BigInteger((uint)bits[2]) << 64 | 
                        new BigInteger((uint)bits[1]) << 32 | 
                        new BigInteger((uint)bits[0]));
        
        var denominator = BigInteger.Pow(10, scale);
        
        return new Real(numerator, denominator);
    }

    public decimal ToDecimal()
    {
        // For simple cases, do direct division
        if (Denominator == 1)
        {
            // Handle potential overflow by checking bounds
            try
            {
                return (decimal)numerator;
            }
            catch (OverflowException)
            {
                // If numerator is too large for decimal, return max/min value
                return numerator > 0 ? decimal.MaxValue : decimal.MinValue;
            }
        }

        // For fractions, compute decimal approximation
        try
        {
            return (decimal)numerator / (decimal)Denominator;
        }
        catch (OverflowException)
        {
            // Handle cases where BigInteger values are too large for decimal
            // Use double for approximation in extreme cases
            var doubleResult = (double)numerator / (double)Denominator;
            if (double.IsInfinity(doubleResult))
                return doubleResult > 0 ? decimal.MaxValue : decimal.MinValue;
            
            try
            {
                return (decimal)doubleResult;
            }
            catch
            {
                return doubleResult > 0 ? decimal.MaxValue : decimal.MinValue;
            }
        }
    }

    public double ToDouble()
    {
        return (double)numerator / (double)Denominator;
    }

    public static Real operator +(Real left, Real right)
    {
        var newNum = left.numerator * right.Denominator + right.numerator * left.Denominator;
        var newDen = left.Denominator * right.Denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator -(Real left, Real right)
    {
        var newNum = left.numerator * right.Denominator - right.numerator * left.Denominator;
        var newDen = left.Denominator * right.Denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator *(Real left, Real right)
    {
        var newNum = left.numerator * right.numerator;
        var newDen = left.Denominator * right.Denominator;
        return new Real(newNum, newDen);
    }

    public static Real operator /(Real left, Real right)
    {
        if (right.IsZero)
            throw new DivideByZeroException("Cannot divide by zero");

        var newNum = left.numerator * right.Denominator;
        var newDen = left.Denominator * right.numerator;
        return new Real(newNum, newDen);
    }

    public static Real operator -(Real value)
    {
        return new Real(-value.numerator, value.Denominator);
    }

    public static bool operator ==(Real left, Real right)
    {
        return left.numerator == right.numerator && left.Denominator == right.Denominator;
    }

    public static bool operator !=(Real left, Real right)
    {
        return !(left == right);
    }

    public static bool operator <(Real left, Real right)
    {
        return left.numerator * right.Denominator < right.numerator * left.Denominator;
    }

    public static bool operator <=(Real left, Real right)
    {
        return left.numerator * right.Denominator <= right.numerator * left.Denominator;
    }

    public static bool operator >(Real left, Real right)
    {
        return left.numerator * right.Denominator > right.numerator * left.Denominator;
    }

    public static bool operator >=(Real left, Real right)
    {
        return left.numerator * right.Denominator >= right.numerator * left.Denominator;
    }

    public static implicit operator Real(int value) => new(value);
    public static implicit operator Real(long value) => new(value);
    public static implicit operator Real(decimal value) => new(value);
    public static implicit operator Real(BigInteger value) => new(value, 1);

    public static explicit operator Real(double value)
    {
        return FromDecimal((decimal)value);
    }


    public static explicit operator double(Real value)
    {
        return value.ToDouble();
    }

    public static explicit operator decimal(Real value)
    {
        return value.ToDecimal();
    }

    public bool Equals(Real other)
    {
        return this == other;
    }

    public override bool Equals(object? obj)
    {
        return obj is Real other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(numerator, Denominator);
    }

    public int CompareTo(Real other)
    {
        if (this < other) return -1;
        if (this > other) return 1;
        return 0;
    }

    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format)
    {
        return ToString(format, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "F"; // Default to fraction format
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "F" or "FRACTION" => Denominator == 1 ? numerator.ToString(formatProvider) : $"{numerator}/{Denominator}",
            "D" or "DECIMAL" => ToDecimal().ToString(formatProvider),
            "G" or "GENERAL" => Denominator == 1 ? numerator.ToString(formatProvider) : $"{numerator}/{Denominator}",
            _ => throw new FormatException($"Invalid format string: {format}")
        };
    }

    public Real Abs()
    {
        return numerator < 0 ? -this : this;
    }

    public Real Reciprocal()
    {
        if (IsZero)
            throw new DivideByZeroException("Cannot compute reciprocal of zero");

        return new Real(Denominator, numerator);
    }

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

    public static readonly Real Zero = new(0);
    public static readonly Real One = new(1);
    public static readonly Real MinusOne = new(-1);
}