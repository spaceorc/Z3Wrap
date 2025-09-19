using System.Globalization;
using System.Numerics;
using System.Text;

namespace Spaceorc.Z3Wrap.DataTypes;

/// <summary>
/// Represents a fixed-width bitvector with unlimited precision arithmetic operations.
/// Supports both signed and unsigned interpretations with natural operator syntax.
/// </summary>
public readonly struct BitVec : IEquatable<BitVec>, IComparable<BitVec>, IFormattable
{
    private readonly BigInteger value;

    private static uint ValidateSize(uint leftSize, uint rightSize)
    {
        if (leftSize != rightSize)
            throw new ArgumentException($"Size mismatch: {leftSize} != {rightSize}");
        return leftSize;
    }

    /// <summary>
    /// Initializes a new bitvector with the specified value and bit width.
    /// </summary>
    /// <param name="value">The value to store, automatically masked to fit the bit width.</param>
    /// <param name="size">The bit width of the bitvector (must be greater than zero).</param>
    /// <exception cref="ArgumentException">Thrown when size is zero.</exception>
    public BitVec(BigInteger value, uint size)
    {
        if (size == 0)
            throw new ArgumentException("Size must be greater than zero", nameof(size));

        // Mask to ensure value fits in the specified bit width
        var maxValue = (BigInteger.One << (int)size) - 1;
        this.value = value & maxValue;
        Size = size;
    }

    /// <summary>
    /// Initializes a new bitvector from a 32-bit signed integer.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    public BitVec(int value, uint size) : this(new BigInteger(value), size) { }

    /// <summary>
    /// Initializes a new bitvector from a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The unsigned integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    public BitVec(uint value, uint size) : this(new BigInteger(value), size) { }

    /// <summary>
    /// Initializes a new bitvector from a 64-bit signed integer.
    /// </summary>
    /// <param name="value">The long integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    public BitVec(long value, uint size) : this(new BigInteger(value), size) { }

    /// <summary>
    /// Initializes a new bitvector from a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The unsigned long integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    public BitVec(ulong value, uint size) : this(new BigInteger(value), size) { }

    /// <summary>
    /// Gets the underlying value of the bitvector as a BigInteger.
    /// </summary>
    public BigInteger Value => value;

    /// <summary>
    /// Gets the bit width of the bitvector.
    /// </summary>
    public uint Size { get; }

    /// <summary>
    /// Gets a value indicating whether this bitvector represents zero.
    /// </summary>
    public bool IsZero => value == 0;

    /// <summary>
    /// Gets the maximum value that can be represented by this bitvector size.
    /// </summary>
    public BigInteger MaxValue => (BigInteger.One << (int)Size) - 1;

    /// <summary>
    /// Creates a bitvector representing zero with the specified bit width.
    /// </summary>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector with value zero.</returns>
    public static BitVec Zero(uint size) => new(0, size);

    /// <summary>
    /// Creates a bitvector representing one with the specified bit width.
    /// </summary>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector with value one.</returns>
    public static BitVec One(uint size) => new(1, size);

    /// <summary>
    /// Creates a bitvector with the maximum possible value for the specified bit width.
    /// </summary>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector with all bits set to 1.</returns>
    public static BitVec Max(uint size) => new((BigInteger.One << (int)size) - 1, size);

    /// <summary>
    /// Creates a bitvector from a 32-bit signed integer.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector representing the specified value.</returns>
    public static BitVec FromInt(int value, uint size) => new(value, size);

    /// <summary>
    /// Creates a bitvector from a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The unsigned integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector representing the specified value.</returns>
    public static BitVec FromUInt(uint value, uint size) => new(value, size);

    /// <summary>
    /// Creates a bitvector from a 64-bit signed integer.
    /// </summary>
    /// <param name="value">The long integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector representing the specified value.</returns>
    public static BitVec FromLong(long value, uint size) => new(value, size);

    /// <summary>
    /// Creates a bitvector from a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The unsigned long integer value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector representing the specified value.</returns>
    public static BitVec FromULong(ulong value, uint size) => new(value, size);

    /// <summary>
    /// Creates a bitvector from a BigInteger value.
    /// </summary>
    /// <param name="value">The BigInteger value.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A bitvector representing the specified value.</returns>
    public static BitVec FromBigInteger(BigInteger value, uint size) => new(value, size);

    /// <summary>
    /// Converts the bitvector to a 32-bit signed integer.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of int.</exception>
    public int ToInt(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > int.MaxValue || bigIntValue < int.MinValue)
            throw new OverflowException($"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of int");
        return (int)bigIntValue;
    }

    /// <summary>
    /// Converts the bitvector to a 32-bit unsigned integer.
    /// </summary>
    /// <returns>The unsigned integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of uint.</exception>
    public uint ToUInt()
    {
        if (value > uint.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of uint");
        return (uint)value;
    }

    /// <summary>
    /// Converts the bitvector to a 64-bit signed integer.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The long integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of long.</exception>
    public long ToLong(bool signed = false)
    {
        var bigIntValue = ToBigInteger(signed);
        if (bigIntValue > long.MaxValue || bigIntValue < long.MinValue)
            throw new OverflowException($"{(signed ? "Signed" : "Unsigned")} value {bigIntValue} is outside the range of long");
        return (long)bigIntValue;
    }

    /// <summary>
    /// Converts the bitvector to a 64-bit unsigned integer.
    /// </summary>
    /// <returns>The unsigned long integer value.</returns>
    /// <exception cref="OverflowException">Thrown when the value is outside the range of ulong.</exception>
    public ulong ToULong()
    {
        if (value > ulong.MaxValue)
            throw new OverflowException($"Unsigned value {value} is outside the range of ulong");
        return (ulong)value;
    }

    /// <summary>
    /// Converts the bitvector to a BigInteger value.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (two's complement).</param>
    /// <returns>The BigInteger representation of the bitvector value.</returns>
    public BigInteger ToBigInteger(bool signed = false)
    {
        if (!signed)
            return value;

        // For signed interpretation: check if MSB (sign bit) is set
        var signBit = BigInteger.One << ((int)Size - 1);
        if ((value & signBit) == 0)
            return value; // Positive number, same as unsigned

        // MSB is set: convert from unsigned to signed using two's complement
        return value - (BigInteger.One << (int)Size);
    }

    /// <summary>
    /// Converts the bitvector to its binary string representation.
    /// </summary>
    /// <returns>A string of '0' and '1' characters representing the bitvector value, padded to the full bit width.</returns>
    public string ToBinaryString()
    {
        if (value == 0)
            return new string('0', (int)Size);

        var sb = new StringBuilder((int)Size);
        var val = value;
        while (val > 0)
        {
            sb.Insert(0, (val & 1).ToString());
            val >>= 1;
        }

        return sb.ToString().PadLeft((int)Size, '0');
    }

    /// <summary>
    /// Adds another bitvector to this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A new bitvector containing the sum, masked to the bit width.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec Add(BitVec other) => new(value + other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Adds a BigInteger value to this bitvector.
    /// </summary>
    /// <param name="other">The BigInteger value to add.</param>
    /// <returns>A new bitvector containing the sum, masked to the bit width.</returns>
    public BitVec Add(BigInteger other) => Add(new BitVec(other, Size));

    /// <summary>
    /// Subtracts another bitvector from this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A new bitvector containing the difference, masked to the bit width.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec Sub(BitVec other) => new(value - other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Subtracts a BigInteger value from this bitvector.
    /// </summary>
    /// <param name="other">The BigInteger value to subtract.</param>
    /// <returns>A new bitvector containing the difference, masked to the bit width.</returns>
    public BitVec Sub(BigInteger other) => Sub(new BitVec(other, Size));

    /// <summary>
    /// Multiplies this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A new bitvector containing the product, masked to the bit width.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec Mul(BitVec other) => new(value * other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Multiplies this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to multiply by.</param>
    /// <returns>A new bitvector containing the product, masked to the bit width.</returns>
    public BitVec Mul(BigInteger other) => Mul(new BitVec(other, Size));

    /// <summary>
    /// Divides this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed division (two's complement).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec Div(BitVec other, bool signed = false)
    {
        var validSize = ValidateSize(Size, other.Size);
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (!signed)
            return new BitVec(value / other.value, validSize);
        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return new BitVec(leftSigned / rightSigned, validSize);
    }

    /// <summary>
    /// Divides this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to divide by.</param>
    /// <param name="signed">Whether to perform signed division (two's complement).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec Div(BigInteger other, bool signed = false) => Div(new BitVec(other, Size), signed);

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed remainder operation (two's complement).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec Rem(BitVec other, bool signed = false)
    {
        var validSize = ValidateSize(Size, other.Size);
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

    /// <summary>
    /// Computes the remainder of dividing this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to divide by.</param>
    /// <param name="signed">Whether to perform signed remainder operation (two's complement).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec Rem(BigInteger other, bool signed = false) => Rem(new BitVec(other, Size), signed);

    /// <summary>
    /// Computes the Z3-style signed modulo operation where the result has the same sign as the divisor.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <returns>A new bitvector containing the signed modulo result.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec SignedMod(BitVec other)
    {
        var validSize = ValidateSize(Size, other.Size);
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

    /// <summary>
    /// Performs bitwise AND operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to AND with.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec And(BitVec other) => new(value & other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Performs bitwise AND operation with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to AND with.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    public BitVec And(BigInteger other) => And(new BitVec(other, Size));

    /// <summary>
    /// Performs bitwise OR operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to OR with.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec Or(BitVec other) => new(value | other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Performs bitwise OR operation with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to OR with.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    public BitVec Or(BigInteger other) => Or(new BitVec(other, Size));

    /// <summary>
    /// Performs bitwise XOR operation with another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to XOR with.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public BitVec Xor(BitVec other) => new(value ^ other.value, ValidateSize(Size, other.Size));

    /// <summary>
    /// Performs bitwise XOR operation with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to XOR with.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    public BitVec Xor(BigInteger other) => Xor(new BitVec(other, Size));

    /// <summary>
    /// Performs left bit shift operation.
    /// </summary>
    /// <param name="shift">The number of positions to shift left (must be non-negative).</param>
    /// <returns>A new bitvector with bits shifted left, masked to the bit width.</returns>
    /// <exception cref="ArgumentException">Thrown when shift amount is negative.</exception>
    public BitVec Shl(int shift)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));
        return new BitVec(value << shift, Size);
    }

    /// <summary>
    /// Performs right bit shift operation.
    /// </summary>
    /// <param name="shift">The number of positions to shift right (must be non-negative).</param>
    /// <param name="signed">Whether to perform arithmetic shift (preserve sign bit) or logical shift (fill with zeros).</param>
    /// <returns>A new bitvector with bits shifted right.</returns>
    /// <exception cref="ArgumentException">Thrown when shift amount is negative.</exception>
    public BitVec Shr(int shift, bool signed = false)
    {
        if (shift < 0)
            throw new ArgumentException("Shift amount must be non-negative", nameof(shift));

        if (!signed)
        {
            // Logical right shift - fill with zeros
            return new BitVec(value >> shift, Size);
        }

        // Arithmetic right shift - preserve sign bit
        var signedValue = ToBigInteger(signed: true);
        var result = signedValue >> shift;
        return new BitVec(result, Size);
    }

    /// <summary>
    /// Extends the bitvector to a larger bit width.
    /// </summary>
    /// <param name="additionalBits">The number of additional bits to add.</param>
    /// <param name="signed">Whether to perform sign extension (replicate sign bit) or zero extension.</param>
    /// <returns>A new bitvector with extended bit width.</returns>
    public BitVec Extend(uint additionalBits, bool signed = false)
    {
        if (signed)
        {
            var signedValue = ToBigInteger(signed: true);
            return new BitVec(signedValue, Size + additionalBits);
        }
        return new BitVec(value, Size + additionalBits);
    }


    /// <summary>
    /// Extracts a range of bits from the bitvector.
    /// </summary>
    /// <param name="high">The highest bit position to extract (inclusive).</param>
    /// <param name="low">The lowest bit position to extract (inclusive).</param>
    /// <returns>A new bitvector containing the extracted bits.</returns>
    /// <exception cref="ArgumentException">Thrown when bit positions are invalid or out of range.</exception>
    public BitVec Extract(uint high, uint low)
    {
        if (high >= Size)
            throw new ArgumentException($"High bit {high} is out of range for {Size}-bit vector");
        if (low > high)
            throw new ArgumentException($"Low bit {low} cannot be greater than high bit {high}");

        var newSize = high - low + 1;
        var extractedValue = (value >> (int)low) & ((BigInteger.One << (int)newSize) - 1);
        return new BitVec(extractedValue, newSize);
    }

    /// <summary>
    /// Resizes the bitvector to a different bit width.
    /// </summary>
    /// <param name="newSize">The new bit width.</param>
    /// <param name="signed">Whether to perform signed extension/truncation when changing size.</param>
    /// <returns>A new bitvector with the specified bit width.</returns>
    public BitVec Resize(uint newSize, bool signed = false)
    {
        if (newSize == Size)
            return this;
        if (newSize > Size)
            return Extend(newSize - Size, signed);
        return new BitVec(value, newSize);
    }

    /// <summary>
    /// Adds two bitvectors using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the sum.</returns>
    public static BitVec operator +(BitVec left, BitVec right) => left.Add(right);

    /// <summary>
    /// Adds a BigInteger to a bitvector using the + operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A new bitvector containing the sum.</returns>
    public static BitVec operator +(BitVec left, BigInteger right) => left.Add(right);

    /// <summary>
    /// Adds a bitvector to a BigInteger using the + operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A new bitvector containing the sum.</returns>
    public static BitVec operator +(BigInteger left, BitVec right) => right.Add(left);

    /// <summary>
    /// Subtracts one bitvector from another using the - operator.
    /// </summary>
    /// <param name="left">The left operand (minuend).</param>
    /// <param name="right">The right operand (subtrahend).</param>
    /// <returns>A new bitvector containing the difference.</returns>
    public static BitVec operator -(BitVec left, BitVec right) => left.Sub(right);

    /// <summary>
    /// Subtracts a BigInteger from a bitvector using the - operator.
    /// </summary>
    /// <param name="left">The bitvector operand (minuend).</param>
    /// <param name="right">The BigInteger operand (subtrahend).</param>
    /// <returns>A new bitvector containing the difference.</returns>
    public static BitVec operator -(BitVec left, BigInteger right) => left.Sub(right);

    /// <summary>
    /// Subtracts a bitvector from a BigInteger using the - operator.
    /// </summary>
    /// <param name="left">The BigInteger operand (minuend).</param>
    /// <param name="right">The bitvector operand (subtrahend).</param>
    /// <returns>A new bitvector containing the difference.</returns>
    public static BitVec operator -(BigInteger left, BitVec right) => new BitVec(left, right.Size).Sub(right);

    /// <summary>
    /// Multiplies two bitvectors using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the product.</returns>
    public static BitVec operator *(BitVec left, BitVec right) => left.Mul(right);

    /// <summary>
    /// Multiplies a bitvector by a BigInteger using the * operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A new bitvector containing the product.</returns>
    public static BitVec operator *(BitVec left, BigInteger right) => left.Mul(right);

    /// <summary>
    /// Multiplies a BigInteger by a bitvector using the * operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A new bitvector containing the product.</returns>
    public static BitVec operator *(BigInteger left, BitVec right) => right.Mul(left);

    /// <summary>
    /// Divides one bitvector by another using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    public static BitVec operator /(BitVec left, BitVec right) => left.Div(right);

    /// <summary>
    /// Divides a bitvector by a BigInteger using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The bitvector operand (dividend).</param>
    /// <param name="right">The BigInteger operand (divisor).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    public static BitVec operator /(BitVec left, BigInteger right) => left.Div(right);

    /// <summary>
    /// Divides a BigInteger by a bitvector using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The BigInteger operand (dividend).</param>
    /// <param name="right">The bitvector operand (divisor).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    public static BitVec operator /(BigInteger left, BitVec right) => new BitVec(left, right.Size).Div(right);

    /// <summary>
    /// Computes the remainder of dividing one bitvector by another using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    public static BitVec operator %(BitVec left, BitVec right) => left.Rem(right);

    /// <summary>
    /// Computes the remainder of dividing a bitvector by a BigInteger using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The bitvector operand (dividend).</param>
    /// <param name="right">The BigInteger operand (divisor).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    public static BitVec operator %(BitVec left, BigInteger right) => left.Rem(right);

    /// <summary>
    /// Computes the remainder of dividing a BigInteger by a bitvector using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The BigInteger operand (dividend).</param>
    /// <param name="right">The bitvector operand (divisor).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    public static BitVec operator %(BigInteger left, BitVec right) => new BitVec(left, right.Size).Rem(right);

    /// <summary>
    /// Negates a bitvector using the unary - operator (two's complement negation).
    /// </summary>
    /// <param name="operand">The bitvector to negate.</param>
    /// <returns>A new bitvector containing the negated value.</returns>
    public static BitVec operator -(BitVec operand) => new(-operand.value, operand.Size);

    /// <summary>
    /// Performs bitwise AND on two bitvectors using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    public static BitVec operator &(BitVec left, BitVec right) => left.And(right);

    /// <summary>
    /// Performs bitwise AND on a bitvector and BigInteger using the &amp; operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    public static BitVec operator &(BitVec left, BigInteger right) => left.And(right);

    /// <summary>
    /// Performs bitwise AND on a BigInteger and bitvector using the &amp; operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A new bitvector containing the bitwise AND result.</returns>
    public static BitVec operator &(BigInteger left, BitVec right) => right.And(left);

    /// <summary>
    /// Performs bitwise OR on two bitvectors using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    public static BitVec operator |(BitVec left, BitVec right) => left.Or(right);

    /// <summary>
    /// Performs bitwise OR on a bitvector and BigInteger using the | operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    public static BitVec operator |(BitVec left, BigInteger right) => left.Or(right);

    /// <summary>
    /// Performs bitwise OR on a BigInteger and bitvector using the | operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A new bitvector containing the bitwise OR result.</returns>
    public static BitVec operator |(BigInteger left, BitVec right) => right.Or(left);

    /// <summary>
    /// Performs bitwise XOR on two bitvectors using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    public static BitVec operator ^(BitVec left, BitVec right) => left.Xor(right);

    /// <summary>
    /// Performs bitwise XOR on a bitvector and BigInteger using the ^ operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    public static BitVec operator ^(BitVec left, BigInteger right) => left.Xor(right);

    /// <summary>
    /// Performs bitwise XOR on a BigInteger and bitvector using the ^ operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A new bitvector containing the bitwise XOR result.</returns>
    public static BitVec operator ^(BigInteger left, BitVec right) => right.Xor(left);

    /// <summary>
    /// Performs bitwise NOT (complement) on a bitvector using the ~ operator.
    /// </summary>
    /// <param name="operand">The bitvector to complement.</param>
    /// <returns>A new bitvector with all bits flipped.</returns>
    public static BitVec operator ~(BitVec operand) => new(~operand.value, operand.Size);

    /// <summary>
    /// Performs left bit shift using the &lt;&lt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="shift">The number of positions to shift left.</param>
    /// <returns>A new bitvector with bits shifted left.</returns>
    public static BitVec operator <<(BitVec left, int shift) => left.Shl(shift);

    /// <summary>
    /// Performs logical right bit shift using the &gt;&gt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="shift">The number of positions to shift right.</param>
    /// <returns>A new bitvector with bits shifted right (logical shift).</returns>
    public static BitVec operator >>(BitVec left, int shift) => left.Shr(shift);

    /// <summary>
    /// Determines whether two bitvectors are equal using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the bitvectors have the same size and value; otherwise, false.</returns>
    public static bool operator ==(BitVec left, BitVec right) => left.Size == right.Size && left.value == right.value;

    /// <summary>
    /// Determines whether two bitvectors are not equal using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the bitvectors have different sizes or values; otherwise, false.</returns>
    public static bool operator !=(BitVec left, BitVec right) => !(left == right);

    /// <summary>
    /// Determines whether the left bitvector is less than the right bitvector using unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the left bitvector is less than the right bitvector; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public static bool operator <(BitVec left, BitVec right)
    {
        ValidateSize(left.Size, right.Size);
        return left.value < right.value;
    }

    /// <summary>
    /// Determines whether the left bitvector is less than or equal to the right bitvector using unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the left bitvector is less than or equal to the right bitvector; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public static bool operator <=(BitVec left, BitVec right)
    {
        ValidateSize(left.Size, right.Size);
        return left.value <= right.value;
    }

    /// <summary>
    /// Determines whether the left bitvector is greater than the right bitvector using unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the left bitvector is greater than the right bitvector; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public static bool operator >(BitVec left, BitVec right)
    {
        ValidateSize(left.Size, right.Size);
        return left.value > right.value;
    }

    /// <summary>
    /// Determines whether the left bitvector is greater than or equal to the right bitvector using unsigned comparison.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>true if the left bitvector is greater than or equal to the right bitvector; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public static bool operator >=(BitVec left, BitVec right)
    {
        ValidateSize(left.Size, right.Size);
        return left.value >= right.value;
    }

    /// <summary>
    /// Determines whether this bitvector is equal to another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to compare with this bitvector.</param>
    /// <returns>true if the bitvectors are equal; otherwise, false.</returns>
    public bool Equals(BitVec other) => this == other;

    /// <summary>
    /// Determines whether this bitvector is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to compare with this bitvector.</param>
    /// <returns>true if the object is a BitVec and is equal to this bitvector; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is BitVec other && Equals(other);

    /// <summary>
    /// Returns the hash code for this bitvector.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(value, Size);

    /// <summary>
    /// Compares this bitvector to another bitvector using unsigned comparison.
    /// </summary>
    /// <param name="other">The bitvector to compare with this bitvector.</param>
    /// <returns>A value less than 0 if this bitvector is less than other; 0 if they are equal; greater than 0 if this bitvector is greater than other.</returns>
    /// <exception cref="ArgumentException">Thrown when bitvectors have different sizes.</exception>
    public int CompareTo(BitVec other)
    {
        ValidateSize(Size, other.Size);
        return this < other ? -1 : this > other ? 1 : 0;
    }

    /// <summary>
    /// Returns a string representation of this bitvector using the default format.
    /// </summary>
    /// <returns>A string representation of the bitvector value.</returns>
    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this bitvector using the specified format.
    /// </summary>
    /// <param name="format">A format string (V/VALUE for value only, D/DECIMAL for decimal with size, B/BINARY for binary with size, X/HEX for hexadecimal with size).</param>
    /// <returns>A formatted string representation of the bitvector.</returns>
    public string ToString(string? format) => ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns a string representation of this bitvector using the specified format and format provider.
    /// </summary>
    /// <param name="format">A format string (V/VALUE for value only, D/DECIMAL for decimal with size, B/BINARY for binary with size, X/HEX for hexadecimal with size).</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A formatted string representation of the bitvector.</returns>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "V";
        formatProvider ??= CultureInfo.InvariantCulture;

        return format.ToUpperInvariant() switch
        {
            "D" or "DECIMAL" => $"{value} ({Size}-bit)",
            "B" or "BINARY" => $"0b{ToBinaryString()} ({Size}-bit)",
            "X" or "HEX" => $"0x{value.ToString("X").TrimStart('0').PadLeft(1, '0')} ({Size}-bit)",
            "V" or "VALUE" => value.ToString(formatProvider),
            _ => throw new FormatException($"Invalid format string: {format}"),
        };
    }
}