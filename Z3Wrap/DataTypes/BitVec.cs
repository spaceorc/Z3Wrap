using System.Globalization;
using System.Numerics;
using System.Text;

namespace Spaceorc.Z3Wrap.DataTypes;

public readonly struct BitVec : IEquatable<BitVec>, IComparable<BitVec>, IFormattable
{
    private readonly BigInteger value;
    private readonly uint size;

    private static uint ValidateSize(uint leftSize, uint rightSize)
    {
        if (leftSize != rightSize)
            throw new ArgumentException($"Size mismatch: {leftSize} != {rightSize}");
        return leftSize;
    }

    // Constructors
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

    // Properties
    public BigInteger Value => value;
    public uint Size => size;
    public bool IsZero => value == 0;
    public BigInteger MaxValue => (BigInteger.One << (int)size) - 1;

    // Static factory methods
    public static BitVec Zero(uint size) => new(0, size);
    public static BitVec One(uint size) => new(1, size);
    public static BitVec Max(uint size) => new((BigInteger.One << (int)size) - 1, size);

    public static BitVec FromInt(int value, uint size) => new(value, size);
    public static BitVec FromUInt(uint value, uint size) => new(value, size);
    public static BitVec FromLong(long value, uint size) => new(value, size);
    public static BitVec FromULong(ulong value, uint size) => new(value, size);
    public static BitVec FromBigInteger(BigInteger value, uint size) => new(value, size);

    // Conversion methods
    public int ToInt(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > int.MaxValue || bigIntValue < int.MinValue)
            throw new OverflowException($"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of int");
        return (int)bigIntValue;
    }

    public uint ToUInt()
    {
        if (value > uint.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of uint");
        return (uint)value;
    }

    public long ToLong(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > long.MaxValue || bigIntValue < long.MinValue)
            throw new OverflowException($"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of long");
        return (long)bigIntValue;
    }

    public ulong ToULong()
    {
        if (value > ulong.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of ulong");
        return (ulong)value;
    }

    public BigInteger ToBigInteger(bool signed = false)
    {
        if (!signed)
            return value;

        // For signed interpretation: check if MSB (sign bit) is set
        var signBit = BigInteger.One << ((int)size - 1);
        if ((value & signBit) == 0)
            return value; // Positive number, same as unsigned

        // MSB is set: convert from unsigned to signed using two's complement
        return value - (BigInteger.One << (int)size);
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

    // Arithmetic method implementations
    public BitVec Add(BitVec other) => new(value + other.value, ValidateSize(size, other.size));
    public BitVec Add(BigInteger other) => Add(new BitVec(other, size));

    public BitVec Sub(BitVec other) => new(value - other.value, ValidateSize(size, other.size));
    public BitVec Sub(BigInteger other) => Sub(new BitVec(other, size));

    public BitVec Mul(BitVec other) => new(value * other.value, ValidateSize(size, other.size));
    public BitVec Mul(BigInteger other) => Mul(new BitVec(other, size));

    public BitVec Div(BitVec other, bool signed = false)
    {
        var validSize = ValidateSize(size, other.size);
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (!signed)
            return new BitVec(value / other.value, validSize);
        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return new BitVec(leftSigned / rightSigned, validSize);
    }

    public BitVec Div(BigInteger other, bool signed = false) => Div(new BitVec(other, size), signed);
    
    public BitVec Rem(BitVec other, bool signed = false)
    {
        var validSize = ValidateSize(size, other.size);
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (signed)
        {
            var leftSigned = ToBigInteger(signed: true);
            var rightSigned = other.ToBigInteger(signed: true);
            return new BitVec(leftSigned % rightSigned, validSize);
        }
        return new BitVec(value % other.value, validSize);
    }

    public BitVec Rem(BigInteger other, bool signed = false) => Rem(new BitVec(other, size), signed);
    
    public BitVec SignedMod(BitVec other)
    {
        var validSize = ValidateSize(size, other.size);
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        // Convert to signed interpretation
        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        // Z3-style signed modulo: result has same sign as divisor
        var remainder = leftSigned % rightSigned;
        if (remainder != 0 && ((leftSigned < 0) != (rightSigned < 0)))
            remainder += rightSigned;
        return new BitVec(remainder, validSize);
    }

    // Bitwise method implementations
    public BitVec And(BitVec other) => new(value & other.value, ValidateSize(size, other.size));
    public BitVec And(BigInteger other) => And(new BitVec(other, size));

    public BitVec Or(BitVec other) => new(value | other.value, ValidateSize(size, other.size));
    public BitVec Or(BigInteger other) => Or(new BitVec(other, size));

    public BitVec Xor(BitVec other) => new(value ^ other.value, ValidateSize(size, other.size));
    public BitVec Xor(BigInteger other) => Xor(new BitVec(other, size));

    // Shift method implementations
    public BitVec Shl(int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));
        return new BitVec(value << shift, size);
    }

    public BitVec Shr(int shift, bool signed = false)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));

        if (!signed)
        {
            // Logical right shift - fill with zeros
            return new BitVec(value >> shift, size);
        }

        // Arithmetic right shift - preserve sign bit
        var signedValue = ToBigInteger(signed: true);
        var result = signedValue >> shift;
        return new BitVec(result, size);
    }

    // Bit manipulation methods
    public BitVec Extend(uint additionalBits, bool signed = false)
    {
        if (signed)
        {
            var signedValue = ToBigInteger(signed: true);
            return new BitVec(signedValue, size + additionalBits);
        }
        return new BitVec(value, size + additionalBits);
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

    public BitVec Resize(uint newSize, bool signed = false)
    {
        if (newSize == size)
            return this;
        if (newSize > size)
            return Extend(newSize - size, signed);
        return new BitVec(value, newSize);
    }


    // Utility methods
    public static BitVec Min(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left <= right ? left : right;
    }

    public static BitVec Max(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left >= right ? left : right;
    }

    // Arithmetic operators
    public static BitVec operator +(BitVec left, BitVec right) => left.Add(right);
    public static BitVec operator +(BitVec left, BigInteger right) => left.Add(right);
    public static BitVec operator +(BigInteger left, BitVec right) => right.Add(left);

    public static BitVec operator -(BitVec left, BitVec right) => left.Sub(right);
    public static BitVec operator -(BitVec left, BigInteger right) => left.Sub(right);
    public static BitVec operator -(BigInteger left, BitVec right) => new BitVec(left, right.Size).Sub(right);

    public static BitVec operator *(BitVec left, BitVec right) => left.Mul(right);
    public static BitVec operator *(BitVec left, BigInteger right) => left.Mul(right);
    public static BitVec operator *(BigInteger left, BitVec right) => right.Mul(left);

    public static BitVec operator /(BitVec left, BitVec right) => left.Div(right);
    public static BitVec operator /(BitVec left, BigInteger right) => left.Div(right);
    public static BitVec operator /(BigInteger left, BitVec right) => new BitVec(left, right.Size).Div(right);

    public static BitVec operator %(BitVec left, BitVec right) => left.Rem(right);
    public static BitVec operator %(BitVec left, BigInteger right) => left.Rem(right);
    public static BitVec operator %(BigInteger left, BitVec right) => new BitVec(left, right.Size).Rem(right);

    public static BitVec operator -(BitVec operand) => new(-operand.value, operand.size);

    // Bitwise operators
    public static BitVec operator &(BitVec left, BitVec right) => left.And(right);
    public static BitVec operator &(BitVec left, BigInteger right) => left.And(right);
    public static BitVec operator &(BigInteger left, BitVec right) => right.And(left);

    public static BitVec operator |(BitVec left, BitVec right) => left.Or(right);
    public static BitVec operator |(BitVec left, BigInteger right) => left.Or(right);
    public static BitVec operator |(BigInteger left, BitVec right) => right.Or(left);

    public static BitVec operator ^(BitVec left, BitVec right) => left.Xor(right);
    public static BitVec operator ^(BitVec left, BigInteger right) => left.Xor(right);
    public static BitVec operator ^(BigInteger left, BitVec right) => right.Xor(left);

    public static BitVec operator ~(BitVec operand) => new(~operand.value, operand.size);

    // Shift operators
    public static BitVec operator <<(BitVec left, int shift) => left.Shl(shift);
    public static BitVec operator >>(BitVec left, int shift) => left.Shr(shift);

    // Comparison operators
    public static bool operator ==(BitVec left, BitVec right) => left.size == right.size && left.value == right.value;
    public static bool operator !=(BitVec left, BitVec right) => !(left == right);

    public static bool operator <(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left.value < right.value;
    }

    public static bool operator <=(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left.value <= right.value;
    }

    public static bool operator >(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left.value > right.value;
    }

    public static bool operator >=(BitVec left, BitVec right)
    {
        ValidateSize(left.size, right.size);
        return left.value >= right.value;
    }

    // Interface implementations
    public bool Equals(BitVec other) => this == other;
    public override bool Equals(object? obj) => obj is BitVec other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(value, size);

    public int CompareTo(BitVec other)
    {
        ValidateSize(size, other.size); 
        return this < other ? -1 : this > other ? 1 : 0;
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
            _ => throw new FormatException($"Invalid format string: {format}"),
        };
    }
}