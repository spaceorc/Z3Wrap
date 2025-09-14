using System.Globalization;
using System.Numerics;

namespace Z3Wrap.DataTypes;

public readonly struct BitVec : IEquatable<BitVec>, IComparable<BitVec>, IFormattable
{
    private readonly BigInteger value;
    private readonly uint size;

    // Constructors
    public BitVec(BigInteger value, uint size)
    {
        if (size == 0)
            throw new ArgumentException("BitVec size must be greater than zero", nameof(size));

        // Mask to ensure value fits in the specified bit width
        var maxValue = (BigInteger.One << (int)size) - 1;
        this.value = value & maxValue;
        this.size = size;
    }

    public BitVec(int value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(uint value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(long value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(ulong value, uint size) : this(new BigInteger(value), size) { }

    // Properties
    public BigInteger Value => value;
    public uint Size => size;
    public bool IsZero => value == 0;
    public BigInteger MaxValue => (BigInteger.One << (int)size) - 1;

    // Conversion methods
    public int ToInt()
    {
        // For signed interpretation, convert using two's complement
        var signedValue = ToSignedBigInteger();

        if (signedValue > int.MaxValue || signedValue < int.MinValue)
            throw new OverflowException($"BitVec signed value {signedValue} is outside the range of int");

        return (int)signedValue;
    }

    public uint ToUInt()
    {
        if (value > uint.MaxValue)
            throw new OverflowException($"BitVec value {value} is outside the range of uint");

        return (uint)value;
    }

    public long ToLong()
    {
        // For signed interpretation, convert using two's complement
        var signedValue = ToSignedBigInteger();

        if (signedValue > long.MaxValue || signedValue < long.MinValue)
            throw new OverflowException($"BitVec signed value {signedValue} is outside the range of long");

        return (long)signedValue;
    }

    public ulong ToULong()
    {
        if (value > ulong.MaxValue)
            throw new OverflowException($"BitVec value {value} is outside the range of ulong");

        return (ulong)value;
    }

    public string ToBinaryString()
    {
        if (value == 0)
            return new string('0', (int)size);

        var binaryStr = "";
        var val = value;
        while (val > 0)
        {
            binaryStr = (val & 1) + binaryStr;
            val >>= 1;
        }

        return binaryStr.PadLeft((int)size, '0');
    }

    // Arithmetic operators (unsigned semantics)
    public static BitVec operator +(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value + right.value, left.size);
    }

    public static BitVec operator -(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value - right.value, left.size);
    }

    public static BitVec operator *(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value * right.value, left.size);
    }


    public static BitVec operator /(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        if (right.IsZero)
            throw new DivideByZeroException("Cannot divide by zero");

        // Unsigned division - straightforward for positive values
        return new BitVec(left.value / right.value, left.size);
    }

    public static BitVec operator %(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        if (right.IsZero)
            throw new DivideByZeroException("Cannot compute remainder with zero divisor");

        // Unsigned remainder - straightforward for positive values
        return new BitVec(left.value % right.value, left.size);
    }

    public static BitVec operator -(BitVec operand)
    {
        // Two's complement negation
        return new BitVec(-operand.value, operand.size);
    }

    // Division operations (signed semantics)
    public BitVec SignedDiv(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"BitVec size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Cannot divide by zero");

        // Convert to signed interpretation
        var leftSigned = ToSignedBigInteger();
        var rightSigned = other.ToSignedBigInteger();

        var result = leftSigned / rightSigned;
        return new BitVec(result, size);
    }

    public BitVec SignedRem(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"BitVec size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Cannot compute remainder with zero divisor");

        // Convert to signed interpretation
        var leftSigned = ToSignedBigInteger();
        var rightSigned = other.ToSignedBigInteger();

        var result = leftSigned % rightSigned;
        return new BitVec(result, size);
    }

    public BitVec SignedMod(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"BitVec size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Cannot compute modulo with zero divisor");

        // Convert to signed interpretation
        var leftSigned = ToSignedBigInteger();
        var rightSigned = other.ToSignedBigInteger();

        // Z3-style signed modulo: result has same sign as divisor
        var remainder = leftSigned % rightSigned;
        if (remainder != 0 && ((leftSigned < 0) != (rightSigned < 0)))
        {
            remainder += rightSigned;
        }

        return new BitVec(remainder, size);
    }

    public BigInteger ToSignedBigInteger()
    {
        // Check if MSB is set (negative in two's complement)
        var msb = BigInteger.One << ((int)size - 1);
        if ((value & msb) != 0)
        {
            // Convert from unsigned to signed (two's complement)
            var maxUnsigned = (BigInteger.One << (int)size) - 1;
            return value - maxUnsigned - 1;
        }
        return value;
    }

    // Bitwise operators
    public static BitVec operator &(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value & right.value, left.size);
    }

    public static BitVec operator |(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value | right.value, left.size);
    }

    public static BitVec operator ^(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value ^ right.value, left.size);
    }

    public static BitVec operator ~(BitVec operand)
    {
        // Bitwise NOT with proper masking to bit width
        var inverted = ~operand.value;
        return new BitVec(inverted, operand.size);
    }

    // Shift operators
    public static BitVec operator <<(BitVec left, int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount cannot be negative", nameof(shift));

        return new BitVec(left.value << shift, left.size);
    }

    public static BitVec operator >>(BitVec left, int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount cannot be negative", nameof(shift));

        // Logical right shift (unsigned)
        return new BitVec(left.value >> shift, left.size);
    }

    // Comparison operators (unsigned semantics)
    public static bool operator ==(BitVec left, BitVec right)
    {
        return left.size == right.size && left.value == right.value;
    }

    public static bool operator !=(BitVec left, BitVec right) => !(left == right);

    public static bool operator <(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left.value < right.value;
    }

    public static bool operator <=(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left.value <= right.value;
    }

    public static bool operator >(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left.value > right.value;
    }

    public static bool operator >=(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left.value >= right.value;
    }

    // Interface implementations
    public bool Equals(BitVec other) => this == other;
    public override bool Equals(object? obj) => obj is BitVec other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(value, size);

    public int CompareTo(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"BitVec size mismatch: {size} != {other.size}");

        if (this < other) return -1;
        if (this > other) return 1;
        return 0;
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "D";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "D" or "DECIMAL" => $"{value} ({size}-bit)",
            "B" or "BINARY" => $"0b{ToBinaryString()} ({size}-bit)",
            "X" or "HEX" => $"0x{value.ToString("X").TrimStart('0').PadLeft(1, '0')} ({size}-bit)",
            "V" or "VALUE" => value.ToString(formatProvider),
            _ => throw new FormatException($"Invalid format string: {format}")
        };
    }

    // Static constants
    public static BitVec Zero(uint size) => new(0, size);
    public static BitVec One(uint size) => new(1, size);
    public static BitVec Max(uint size) => new((BigInteger.One << (int)size) - 1, size);

    // Utility methods
    public static BitVec Min(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left <= right ? left : right;
    }

    public static BitVec Max(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"BitVec size mismatch: {left.size} != {right.size}");

        return left >= right ? left : right;
    }
}