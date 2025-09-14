using System.Globalization;
using System.Numerics;
using System.Text;

namespace Z3Wrap.DataTypes;

public readonly struct BitVec : IEquatable<BitVec>, IComparable<BitVec>, IFormattable
{
    private readonly BigInteger value;
    private readonly uint size;

    public BitVec(BigInteger value, uint size)
    {
        if (size == 0)
            throw new ArgumentException("Size must be greater than zero", nameof(size));

        // Mask to ensure value fits in the specified bit width
        var maxValue = (BigInteger.One << (int)size) - 1;
        this.value = value & maxValue;
        this.size = size;
    }

    public BitVec(int value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(uint value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(long value, uint size) : this(new BigInteger(value), size) { }
    public BitVec(ulong value, uint size) : this(new BigInteger(value), size) { }

    public BigInteger Value => value;
    public uint Size => size;
    public bool IsZero => value == 0;
    public BigInteger MaxValue => (BigInteger.One << (int)size) - 1;

    public static BitVec Zero(uint size) => new(0, size);
    public static BitVec One(uint size) => new(1, size);
    public static BitVec Max(uint size) => new((BigInteger.One << (int)size) - 1, size);

    public int ToInt()
    {
        if (value > int.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of int");

        return (int)value;
    }

    public int ToSignedInt()
    {
        // For signed interpretation, convert using two's complement
        var signedValue = ToSignedBigInteger();

        if (signedValue > int.MaxValue || signedValue < int.MinValue)
            throw new OverflowException($"Signed value {signedValue} is outside the range of int");

        return (int)signedValue;
    }

    public uint ToUInt()
    {
        if (value > uint.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of uint");

        return (uint)value;
    }

    public long ToLong()
    {
        if (value > long.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of long");

        return (long)value;
    }

    public long ToSignedLong()
    {
        // For signed interpretation, convert using two's complement
        var signedValue = ToSignedBigInteger();

        if (signedValue > long.MaxValue || signedValue < long.MinValue)
            throw new OverflowException($"Signed value {signedValue} is outside the range of long");

        return (long)signedValue;
    }

    public ulong ToULong()
    {
        if (value > ulong.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of ulong");

        return (ulong)value;
    }

    public string ToBinaryString()
    {
        if (value == 0)
            return new string('0', (int)size);

        var sb = new StringBuilder((int)size);
        var val = value;
        while (val > 0)
        {
            sb.Insert(0, (val & 1).ToString());
            val >>= 1;
        }

        return sb.ToString().PadLeft((int)size, '0');
    }

    public BigInteger ToSignedBigInteger()
    {
        // Check if MSB is set (negative in two's complement)
        var msb = BigInteger.One << ((int)size - 1);
        if ((value & msb) != 0)
        {
            // Convert from unsigned to signed (two's complement)
            return value - (BigInteger.One << (int)size);
        }
        return value;
    }

    public BitVec SignedDiv(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"Size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        // Convert to signed interpretation
        var leftSigned = ToSignedBigInteger();
        var rightSigned = other.ToSignedBigInteger();

        var result = leftSigned / rightSigned;
        return new BitVec(result, size);
    }

    public BitVec SignedRem(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"Size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        // Convert to signed interpretation
        var leftSigned = ToSignedBigInteger();
        var rightSigned = other.ToSignedBigInteger();

        var result = leftSigned % rightSigned;
        return new BitVec(result, size);
    }

    public BitVec SignedMod(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"Size mismatch: {size} != {other.size}");

        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

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

    public static BitVec operator +(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value + right.value, left.size);
    }

    public static BitVec operator -(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value - right.value, left.size);
    }

    public static BitVec operator *(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value * right.value, left.size);
    }

    public static BitVec operator /(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        if (right.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        // Unsigned division - straightforward for positive values
        return new BitVec(left.value / right.value, left.size);
    }

    public static BitVec operator %(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        if (right.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        // Unsigned remainder - straightforward for positive values
        return new BitVec(left.value % right.value, left.size);
    }

    public static BitVec operator -(BitVec operand)
    {
        // Two's complement negation
        return new BitVec(-operand.value, operand.size);
    }

    public static BitVec operator &(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value & right.value, left.size);
    }

    public static BitVec operator |(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value | right.value, left.size);
    }

    public static BitVec operator ^(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return new BitVec(left.value ^ right.value, left.size);
    }

    public static BitVec operator ~(BitVec operand)
    {
        // Bitwise NOT with proper masking to bit width
        var inverted = ~operand.value;
        return new BitVec(inverted, operand.size);
    }

    public static BitVec operator <<(BitVec left, int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));

        return new BitVec(left.value << shift, left.size);
    }

    public static BitVec operator >>(BitVec left, int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));

        // Logical right shift (unsigned)
        return new BitVec(left.value >> shift, left.size);
    }

    public static bool operator ==(BitVec left, BitVec right)
    {
        return left.size == right.size && left.value == right.value;
    }

    public static bool operator !=(BitVec left, BitVec right) => !(left == right);

    public static bool operator <(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left.value < right.value;
    }

    public static bool operator <=(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left.value <= right.value;
    }

    public static bool operator >(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left.value > right.value;
    }

    public static bool operator >=(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left.value >= right.value;
    }

    public bool Equals(BitVec other) => this == other;

    public override bool Equals(object? obj) => obj is BitVec other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(value, size);

    public int CompareTo(BitVec other)
    {
        if (size != other.size)
            throw new ArgumentException($"Size mismatch: {size} != {other.size}");

        if (this < other) return -1;
        if (this > other) return 1;
        return 0;
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "V";
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

    public static BitVec Min(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left <= right ? left : right;
    }

    public static BitVec Max(BitVec left, BitVec right)
    {
        if (left.size != right.size)
            throw new ArgumentException($"Size mismatch: {left.size} != {right.size}");

        return left >= right ? left : right;
    }

    public BitVec Extend(uint additionalBits)
    {
        return new BitVec(value, size + additionalBits);
    }

    public BitVec SignedExtend(uint additionalBits)
    {
        var signedValue = ToSignedBigInteger();
        return new BitVec(signedValue, size + additionalBits);
    }

    public BitVec Extract(uint high, uint low)
    {
        if (high >= size)
            throw new ArgumentException($"High bit {high} is out of range for {size}-bit vector");
        if (low > high)
            throw new ArgumentException($"Low bit {low} cannot be greater than high bit {high}");

        var newSize = high - low + 1;
        var extractedValue = (value >> (int)low) & ((BigInteger.One << (int)newSize) - 1);
        return new BitVec(extractedValue, newSize);
    }

    public BitVec Resize(uint newSize)
    {
        if (newSize == size)
            return this;

        if (newSize > size)
            return Extend(newSize - size);

        return new BitVec(value, newSize);
    }

    public BitVec SignedResize(uint newSize)
    {
        if (newSize == size)
            return this;

        if (newSize > size)
            return SignedExtend(newSize - size);

        return new BitVec(value, newSize);
    }

    public static BitVec FromInt(int value, uint size) => new(value, size);
    public static BitVec FromUInt(uint value, uint size) => new(value, size);
    public static BitVec FromLong(long value, uint size) => new(value, size);
    public static BitVec FromULong(ulong value, uint size) => new(value, size);
    public static BitVec FromBigInteger(BigInteger value, uint size) => new(value, size);
}