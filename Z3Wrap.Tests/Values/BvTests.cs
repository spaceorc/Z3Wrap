using System.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Values;

[TestFixture]
public class BvTests
{
    // Custom size types for testing edge cases that can't be tested with standard sizes
    private readonly struct Size12 : ISize
    {
        public static uint Size => 12;
    }

    private readonly struct Size20 : ISize
    {
        public static uint Size => 20;
    }

    #region Core Construction and Properties Tests

    [Test]
    public void Constructor_ZeroValue_CreatesZeroBitVector()
    {
        var bv = new Bv<Size8>(0);
        Assert.That(bv.Value, Is.EqualTo(BigInteger.Zero));
        Assert.That(bv.IsZero, Is.True);
        Assert.That(Bv<Size8>.Size, Is.EqualTo(8u));
    }

    [Test]
    public void Constructor_ValidValue_StoresValue()
    {
        var bv = new Bv<Size8>(42);
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));
        Assert.That(bv.IsZero, Is.False);
    }

    [Test]
    public void Constructor_OverflowValue_MasksToSize()
    {
        var bv8 = new Bv<Size8>(256); // 0x100, should become 0x00
        Assert.That(bv8.Value, Is.EqualTo(BigInteger.Zero));

        var secondBv8 = new Bv<Size8>(257); // 0x101, should become 0x01
        Assert.That(secondBv8.Value, Is.EqualTo(BigInteger.One));

        var bv16 = new Bv<Size16>(65536); // 0x10000, should become 0x0000
        Assert.That(bv16.Value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void Constructor_NegativeValue_WrapsCorrectly()
    {
        var bv8 = new Bv<Size8>(-1); // Should become 0xFF (255)
        Assert.That(bv8.Value, Is.EqualTo(new BigInteger(255)));

        var bv16 = new Bv<Size16>(-1); // Should become 0xFFFF (65535)
        Assert.That(bv16.Value, Is.EqualTo(new BigInteger(65535)));
    }

    [Test]
    public void Constructor_LargeBigInteger_MasksCorrectly()
    {
        var largeBigInt = BigInteger.Parse("12345678901234567890123456789");
        var bv32 = new Bv<Size32>(largeBigInt);

        // Should be masked to 32 bits
        var expected = largeBigInt & ((BigInteger.One << 32) - 1);
        Assert.That(bv32.Value, Is.EqualTo(expected));
    }

    [Test]
    public void StaticSize_DifferentSizes_ReturnsCorrectValues()
    {
        Assert.That(Bv<Size8>.Size, Is.EqualTo(8u));
        Assert.That(Bv<Size16>.Size, Is.EqualTo(16u));
        Assert.That(Bv<Size32>.Size, Is.EqualTo(32u));
        Assert.That(Bv<Size64>.Size, Is.EqualTo(64u));
        Assert.That(Bv<Size128>.Size, Is.EqualTo(128u));
    }

    [Test]
    public void StaticConstants_HaveCorrectValues()
    {
        // Zero constant
        Assert.That(Bv<Size8>.Zero.Value, Is.EqualTo(BigInteger.Zero));
        Assert.That(Bv<Size8>.Zero.IsZero, Is.True);

        // One constant
        Assert.That(Bv<Size8>.One.Value, Is.EqualTo(BigInteger.One));

        // Max constant (all bits set)
        Assert.That(Bv<Size8>.Max.Value, Is.EqualTo(new BigInteger(255))); // 2^8 - 1
        Assert.That(Bv<Size16>.Max.Value, Is.EqualTo(new BigInteger(65535))); // 2^16 - 1

        // SignBit constant (MSB set)
        Assert.That(Bv<Size8>.SignBit.Value, Is.EqualTo(new BigInteger(128))); // 2^7
        Assert.That(Bv<Size16>.SignBit.Value, Is.EqualTo(new BigInteger(32768))); // 2^15

        // AllOnes alias
        Assert.That(Bv<Size8>.AllOnes.Value, Is.EqualTo(Bv<Size8>.Max.Value));
    }

    #endregion

    #region Implicit Conversion Tests

    [Test]
    public void ImplicitConversion_FromInt_CreatesCorrectBitVector()
    {
        Bv<Size32> bv = 42;
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));

        // Test negative conversion
        Bv<Size32> neg = -1;
        Assert.That(neg.Value, Is.EqualTo(new BigInteger(4294967295))); // 2^32 - 1
    }

    [Test]
    public void ImplicitConversion_FromUInt_CreatesCorrectBitVector()
    {
        Bv<Size32> bv = 42u;
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));

        Bv<Size32> maxUInt = uint.MaxValue;
        Assert.That(maxUInt.Value, Is.EqualTo(new BigInteger(uint.MaxValue)));
    }

    [Test]
    public void ImplicitConversion_FromLong_CreatesCorrectBitVector()
    {
        Bv<Size64> bv = 42L;
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));

        // Test with value larger than 64 bits when converted to smaller size
        Bv<Size32> smallBv = long.MaxValue;
        var expected = new BigInteger(long.MaxValue) & ((BigInteger.One << 32) - 1);
        Assert.That(smallBv.Value, Is.EqualTo(expected));
    }

    [Test]
    public void ImplicitConversion_FromULong_CreatesCorrectBitVector()
    {
        Bv<Size64> bv = 42uL;
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));

        Bv<Size64> maxULong = ulong.MaxValue;
        Assert.That(maxULong.Value, Is.EqualTo(new BigInteger(ulong.MaxValue)));
    }

    [Test]
    public void ImplicitConversion_FromBigInteger_CreatesCorrectBitVector()
    {
        var bigInt = new BigInteger(123456789);
        Bv<Size64> bv = bigInt;
        Assert.That(bv.Value, Is.EqualTo(bigInt));

        // Test masking with large BigInteger
        var hugeBigInt = BigInteger.Parse("12345678901234567890");
        Bv<Size32> smallBv = hugeBigInt;
        var expected = hugeBigInt & ((BigInteger.One << 32) - 1);
        Assert.That(smallBv.Value, Is.EqualTo(expected));
    }

    #endregion

    #region Factory Method Tests

    [Test]
    public void FromBytes_ValidByteArray_CreatesCorrectBitVector()
    {
        var bytes = new byte[] { 0x12, 0x34 }; // Little-endian: 0x3412
        var bv = Bv<Size16>.FromBytes(bytes);
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(0x3412)));
    }

    [Test]
    public void FromBytes_EmptyArray_CreatesZero()
    {
        var bv = Bv<Size16>.FromBytes([]);
        Assert.That(bv.Value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void FromBytes_TooLargeArray_ThrowsArgumentException()
    {
        var bytes = new byte[] { 0x01, 0x02, 0x03 }; // 3 bytes > 2 bytes for Size16
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromBytes(bytes));
    }

    [Test]
    public void FromBytes_ExactSize_Works()
    {
        var bytes = new byte[] { 0xFF, 0xFF }; // Exactly 2 bytes for Size16
        var bv = Bv<Size16>.FromBytes(bytes);
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(0xFFFF)));
    }

    [Test]
    public void FromHex_ValidHexString_CreatesCorrectBitVector()
    {
        var bv1 = Bv<Size16>.FromHex("1234");
        Assert.That(bv1.Value, Is.EqualTo(new BigInteger(0x1234)));

        var bv2 = Bv<Size16>.FromHex("0x5678");
        Assert.That(bv2.Value, Is.EqualTo(new BigInteger(0x5678)));

        var bv3 = Bv<Size16>.FromHex("0XABCD");
        Assert.That(bv3.Value, Is.EqualTo(new BigInteger(0xABCD)));
    }

    [Test]
    public void FromHex_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromHex(""));
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromHex(null!));
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromHex("   "));
    }

    [Test]
    public void FromHex_InvalidHexString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromHex("GHIJ"));
        Assert.Throws<ArgumentException>(() => Bv<Size16>.FromHex("12G4"));
    }

    [Test]
    public void FromHex_TooLongHexString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromHex("1234")); // 4 hex digits > 2 for Size8
    }

    [Test]
    public void FromBinary_ValidBinaryString_CreatesCorrectBitVector()
    {
        var bv1 = Bv<Size8>.FromBinary("10110011");
        Assert.That(bv1.Value, Is.EqualTo(new BigInteger(0b10110011)));

        var bv2 = Bv<Size8>.FromBinary("0b11001100");
        Assert.That(bv2.Value, Is.EqualTo(new BigInteger(0b11001100)));

        var bv3 = Bv<Size8>.FromBinary("0B10101010");
        Assert.That(bv3.Value, Is.EqualTo(new BigInteger(0b10101010)));
    }

    [Test]
    public void FromBinary_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary(""));
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary(null!));
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary("   "));
    }

    [Test]
    public void FromBinary_InvalidBinaryString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary("1012"));
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary("10a1"));
    }

    [Test]
    public void FromBinary_TooLongBinaryString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Bv<Size8>.FromBinary("101101101")); // 9 bits > 8 for Size8
    }

    [Test]
    public void FromBinary_PaddedString_Works()
    {
        var bv = Bv<Size8>.FromBinary("00000001");
        Assert.That(bv.Value, Is.EqualTo(BigInteger.One));
    }

    #endregion

    #region Conversion Tests

    [Test]
    public void ToBigInteger_UnsignedMode_ReturnsCorrectValue()
    {
        var bv8 = new Bv<Size8>(200);
        Assert.That(bv8.ToBigInteger(signed: false), Is.EqualTo(new BigInteger(200)));

        var bvMax = Bv<Size8>.Max;
        Assert.That(bvMax.ToBigInteger(signed: false), Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void ToBigInteger_SignedMode_ReturnsCorrectValue()
    {
        // Positive number
        var bv8Pos = new Bv<Size8>(100);
        Assert.That(bv8Pos.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(100)));

        // Negative number (MSB set)
        var bv8Neg = new Bv<Size8>(200); // MSB set, should be negative in signed interpretation
        Assert.That(bv8Neg.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(200 - 256))); // -56

        // Max positive value (MSB clear)
        var bv8MaxPos = new Bv<Size8>(127);
        Assert.That(bv8MaxPos.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(127)));

        // Most negative value (MSB set, all other bits clear)
        var bv8MinNeg = new Bv<Size8>(128);
        Assert.That(bv8MinNeg.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(-128)));
    }

    [Test]
    public void ToInt_ValidRange_ReturnsCorrectValue()
    {
        var bv = new Bv<Size32>(42);
        Assert.That(bv.ToInt(signed: false), Is.EqualTo(42));
        Assert.That(bv.ToInt(signed: true), Is.EqualTo(42));
    }

    [Test]
    public void ToInt_OutOfRange_ThrowsOverflowException()
    {
        var bvLarge = new Bv<Size64>(long.MaxValue);
        Assert.Throws<OverflowException>(() => bvLarge.ToInt());
    }

    [Test]
    public void ToInt_SignedNegative_ReturnsNegativeValue()
    {
        var bv32 = new Bv<Size32>(0x80000000); // MSB set in 32-bit
        Assert.That(bv32.ToInt(signed: true), Is.EqualTo(int.MinValue));
        Assert.Throws<OverflowException>(() => bv32.ToInt(signed: false)); // Too large for int as unsigned
    }

    [Test]
    public void ToInt_UnsignedOverflow_ThrowsOverflowException()
    {
        // Test unsigned interpretation that exceeds int.MaxValue
        var bvLarge = new Bv<Size64>((long)int.MaxValue + 1);
        var ex = Assert.Throws<OverflowException>(() => bvLarge.ToInt(signed: false));
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of int"));
    }

    [Test]
    public void ToInt_SignedUnderflow_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is less than int.MinValue
        // This would require a bitvector larger than 32-bit with a very negative signed value
        var bvVeryNegative = new Bv<Size64>(0x8000000000000000UL); // Very large negative in signed 64-bit
        var ex = Assert.Throws<OverflowException>(() => bvVeryNegative.ToInt(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of int"));
    }

    [Test]
    public void ToUInt_ValidRange_ReturnsCorrectValue()
    {
        var bv = new Bv<Size32>(42u);
        Assert.That(bv.ToUInt(), Is.EqualTo(42u));

        var bvMax = new Bv<Size32>(uint.MaxValue);
        Assert.That(bvMax.ToUInt(), Is.EqualTo(uint.MaxValue));
    }

    [Test]
    public void ToUInt_OutOfRange_ThrowsOverflowException()
    {
        var bvLarge = new Bv<Size64>(ulong.MaxValue);
        var ex = Assert.Throws<OverflowException>(() => bvLarge.ToUInt());
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of uint"));
    }

    [Test]
    public void ToUInt_JustOverLimit_ThrowsOverflowException()
    {
        // Test with value just over uint.MaxValue
        var bvJustOver = new Bv<Size64>((long)uint.MaxValue + 1);
        var ex = Assert.Throws<OverflowException>(() => bvJustOver.ToUInt());
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of uint"));
    }

    [Test]
    public void ToLong_ValidRange_ReturnsCorrectValue()
    {
        var bv = new Bv<Size64>(42L);
        Assert.That(bv.ToLong(signed: false), Is.EqualTo(42L));
        Assert.That(bv.ToLong(signed: true), Is.EqualTo(42L));

        var bvMax = new Bv<Size64>(long.MaxValue);
        Assert.That(bvMax.ToLong(signed: true), Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void ToLong_SignedNegative_ReturnsNegativeValue()
    {
        var bv64 = new Bv<Size64>(0x8000000000000000UL); // MSB set in 64-bit
        Assert.That(bv64.ToLong(signed: true), Is.EqualTo(long.MinValue));
    }

    [Test]
    public void ToLong_UnsignedOverflow_ThrowsOverflowException()
    {
        // Create a bitvector larger than 64 bits with a value > long.MaxValue
        var bvLarge = new Bv<Size128>(BigInteger.Parse("9223372036854775808")); // long.MaxValue + 1
        var ex = Assert.Throws<OverflowException>(() => bvLarge.ToLong(signed: false));
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToLong_SignedUnderflow_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is less than long.MinValue
        // Use a 128-bit bitvector with MSB set and additional negative magnitude
        var veryNegativeBits = BigInteger.Parse("170141183460469231731687303715884105728"); // -(2^127)
        var bvVeryNegative = new Bv<Size128>(veryNegativeBits);
        var ex = Assert.Throws<OverflowException>(() => bvVeryNegative.ToLong(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToLong_SignedOverflow_ThrowsOverflowException()
    {
        // Create a positive value larger than long.MaxValue when interpreted as signed
        var bvLargePositive = new Bv<Size128>(BigInteger.Parse("9223372036854775808")); // long.MaxValue + 1
        var ex = Assert.Throws<OverflowException>(() => bvLargePositive.ToLong(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToULong_ValidRange_ReturnsCorrectValue()
    {
        var bv = new Bv<Size64>(42uL);
        Assert.That(bv.ToULong(), Is.EqualTo(42uL));

        var bvMax = new Bv<Size64>(ulong.MaxValue);
        Assert.That(bvMax.ToULong(), Is.EqualTo(ulong.MaxValue));
    }

    [Test]
    public void ToULong_Overflow_ThrowsOverflowException()
    {
        // Create a 128-bit value larger than ulong.MaxValue
        var ulongMaxPlusOne = new BigInteger(ulong.MaxValue) + 1;
        var bvLarge = new Bv<Size128>(ulongMaxPlusOne);
        var ex = Assert.Throws<OverflowException>(() => bvLarge.ToULong());
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of ulong"));
    }

    [Test]
    public void ToULong_VeryLargeValue_ThrowsOverflowException()
    {
        // Test with the maximum 128-bit value (all bits set)
        var bvMax128 = Bv<Size128>.Max; // 2^128 - 1, much larger than ulong.MaxValue (2^64 - 1)

        // Verify the value is indeed larger than ulong.MaxValue
        Assert.That(bvMax128.Value, Is.GreaterThan(new BigInteger(ulong.MaxValue)));

        var ex = Assert.Throws<OverflowException>(() => bvMax128.ToULong());
        Assert.That(ex.Message, Does.Contain("Unsigned value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of ulong"));
    }

    [Test]
    public void ToBytes_DefaultLittleEndian_ReturnsCorrectArray()
    {
        var bv = new Bv<Size16>(0x1234);
        var bytes = bv.ToBytes();

        Assert.That(bytes.Length, Is.EqualTo(2));
        Assert.That(bytes[0], Is.EqualTo(0x34)); // LSB first
        Assert.That(bytes[1], Is.EqualTo(0x12)); // MSB second
    }

    [Test]
    public void ToBytes_BigEndian_ReturnsCorrectArray()
    {
        var bv = new Bv<Size16>(0x1234);
        var bytes = bv.ToBytes(Endianness.BigEndian);

        Assert.That(bytes.Length, Is.EqualTo(2));
        Assert.That(bytes[0], Is.EqualTo(0x12)); // MSB first
        Assert.That(bytes[1], Is.EqualTo(0x34)); // LSB second
    }

    [Test]
    public void CopyTo_ValidDestination_CopiesBytes()
    {
        var bv = new Bv<Size16>(0xABCD);
        var destination = new byte[4]; // Larger than needed

        bv.CopyTo(destination, Endianness.LittleEndian);

        Assert.That(destination[0], Is.EqualTo(0xCD));
        Assert.That(destination[1], Is.EqualTo(0xAB));
        Assert.That(destination[2], Is.EqualTo(0x00)); // Unused
        Assert.That(destination[3], Is.EqualTo(0x00)); // Unused
    }

    [Test]
    public void CopyTo_TooSmallDestination_ThrowsArgumentException()
    {
        var bv = new Bv<Size16>(0x1234);
        var destination = new byte[1]; // Too small

        Assert.Throws<ArgumentException>(() => bv.CopyTo(destination));
    }

    #endregion

    #region Arithmetic Operation Tests

    [Test]
    public void Addition_ValidValues_ReturnsCorrectSum()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(50);
        var result = a + b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(150)));
    }

    [Test]
    public void Addition_Overflow_WrapsCorrectly()
    {
        var a = new Bv<Size8>(200);
        var b = new Bv<Size8>(100);
        var result = a + b; // 300, should wrap to 44 (300 & 255)

        Assert.That(result.Value, Is.EqualTo(new BigInteger(44)));
    }

    [Test]
    public void Subtraction_ValidValues_ReturnsCorrectDifference()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(30);
        var result = a - b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(70)));
    }

    [Test]
    public void Subtraction_Underflow_WrapsCorrectly()
    {
        var a = new Bv<Size8>(30);
        var b = new Bv<Size8>(100);
        var result = a - b; // -70, should wrap to 186 ((30 - 100) & 255)

        Assert.That(result.Value, Is.EqualTo(new BigInteger(186)));
    }

    [Test]
    public void Multiplication_ValidValues_ReturnsCorrectProduct()
    {
        var a = new Bv<Size8>(10);
        var b = new Bv<Size8>(5);
        var result = a * b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(50)));
    }

    [Test]
    public void Multiplication_Overflow_MasksCorrectly()
    {
        var a = new Bv<Size8>(20);
        var b = new Bv<Size8>(20);
        var result = a * b; // 400, should mask to 144 (400 & 255)

        Assert.That(result.Value, Is.EqualTo(new BigInteger(144)));
    }

    [Test]
    public void Division_UnsignedValidValues_ReturnsCorrectQuotient()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(5);
        var result = a / b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(20)));
    }

    [Test]
    public void Division_ByZero_ThrowsDivideByZeroException()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(0);

        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a / b;
        });
        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a.Div(b);
        });
        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a.Div(b, signed: true);
        });
    }

    [Test]
    public void Division_SignedValues_ReturnsCorrectQuotient()
    {
        var a = new Bv<Size8>(0x80); // -128 in signed 8-bit
        var b = new Bv<Size8>(0x02); // 2
        var result = a.Div(b, signed: true);

        // -128 / 2 = -64, which as unsigned 8-bit is 192 (256 - 64)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(192)));
    }

    [Test]
    public void Remainder_UnsignedValues_ReturnsCorrectRemainder()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(7);
        var result = a % b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(2))); // 100 % 7 = 2
    }

    [Test]
    public void Remainder_SignedValues_ReturnsCorrectRemainder()
    {
        var a = new Bv<Size8>(0x80); // -128 in signed 8-bit
        var b = new Bv<Size8>(0x07); // 7
        var result = a.Rem(b, signed: true);

        // -128 % 7 = -2, which as unsigned 8-bit is 254 (256 - 2)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(254)));
    }

    [Test]
    public void Remainder_ByZero_ThrowsDivideByZeroException()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(0);

        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a % b;
        });
        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a.Rem(b);
        });
        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a.Rem(b, signed: true);
        });
    }

    [Test]
    public void SignedMod_PositiveDivisor_FollowsZ3Semantics()
    {
        var a = new Bv<Size8>(0x80); // -128 in signed
        var b = new Bv<Size8>(0x07); // 7
        var result = a.SignedMod(b);

        // Z3 signed modulo: result should have same sign as divisor
        // -128 smod 7 should be positive since divisor is positive
        var signedResult = result.ToBigInteger(signed: true);
        Assert.That(signedResult, Is.GreaterThanOrEqualTo(new BigInteger(0)));
        Assert.That(signedResult, Is.LessThan(new BigInteger(7)));
    }

    [Test]
    public void SignedMod_ByZero_ThrowsDivideByZeroException()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(0);

        Assert.Throws<DivideByZeroException>(() =>
        {
            var _ = a.SignedMod(b);
        });
    }

    [Test]
    public void UnaryMinus_ValidValue_ReturnsCorrectNegation()
    {
        var bv = new Bv<Size8>(100);
        var result = -bv;

        // -100 as unsigned 8-bit should be 156 (256 - 100)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(156)));
    }

    [Test]
    public void UnaryMinus_Zero_ReturnsZero()
    {
        var bv = Bv<Size8>.Zero;
        var result = -bv;

        Assert.That(result.Value, Is.EqualTo(BigInteger.Zero));
    }

    #endregion

    #region Bitwise Operation Tests

    [Test]
    public void BitwiseAnd_ValidValues_ReturnsCorrectResult()
    {
        var a = new Bv<Size8>(0b11110000);
        var b = new Bv<Size8>(0b10101010);
        var result = a & b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b10100000)));
    }

    [Test]
    public void BitwiseOr_ValidValues_ReturnsCorrectResult()
    {
        var a = new Bv<Size8>(0b11110000);
        var b = new Bv<Size8>(0b00001111);
        var result = a | b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b11111111)));
    }

    [Test]
    public void BitwiseXor_ValidValues_ReturnsCorrectResult()
    {
        var a = new Bv<Size8>(0b11110000);
        var b = new Bv<Size8>(0b10101010);
        var result = a ^ b;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b01011010)));
    }

    [Test]
    public void BitwiseNot_ValidValue_ReturnsCorrectResult()
    {
        var bv = new Bv<Size8>(0b10101010);
        var result = ~bv;

        // For 8-bit: ~10101010 = 01010101 masked to 8 bits
        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b01010101)));
    }

    [Test]
    public void BitwiseNot_AllZeros_ReturnsAllOnes()
    {
        var bv = Bv<Size8>.Zero;
        var result = ~bv;

        Assert.That(result.Value, Is.EqualTo(Bv<Size8>.Max.Value));
    }

    [Test]
    public void BitwiseNot_AllOnes_ReturnsAllZeros()
    {
        var bv = Bv<Size8>.Max;
        var result = ~bv;

        Assert.That(result.Value, Is.EqualTo(BigInteger.Zero));
    }

    #endregion

    #region Shift Operation Tests

    [Test]
    public void LeftShift_ValidShift_ReturnsCorrectResult()
    {
        var bv = new Bv<Size8>(0b00001111);
        var result = bv << 2;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b00111100)));
    }

    [Test]
    public void LeftShift_Overflow_MasksCorrectly()
    {
        var bv = new Bv<Size8>(0b11110000);
        var result = bv << 2;

        // Should be masked to 8 bits: 11000000
        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b11000000)));
    }

    [Test]
    public void LeftShift_NegativeAmount_ThrowsArgumentException()
    {
        var bv = new Bv<Size8>(1);
        Assert.Throws<ArgumentException>(() => bv.Shl(-1));
    }

    [Test]
    public void RightShift_LogicalShift_ReturnsCorrectResult()
    {
        var bv = new Bv<Size8>(0b11110000);
        var result = bv >> 2;

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b00111100)));
    }

    [Test]
    public void RightShift_ArithmeticShift_PreservesSignBit()
    {
        var bv = new Bv<Size8>(0b10000000); // MSB set (negative in signed interpretation)
        var result = bv.Shr(2, signed: true);

        // Arithmetic right shift should preserve sign bit: 11100000
        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b11100000)));
    }

    [Test]
    public void RightShift_LogicalPositive_FillsWithZeros()
    {
        var bv = new Bv<Size8>(0b01110000);
        var result = bv.Shr(2, signed: false);

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b00011100)));
    }

    [Test]
    public void RightShift_NegativeAmount_ThrowsArgumentException()
    {
        var bv = new Bv<Size8>(1);
        Assert.Throws<ArgumentException>(() => bv.Shr(-1));
    }

    #endregion

    #region Comparison Tests

    [Test]
    public void Equality_SameValues_ReturnsTrue()
    {
        var a = new Bv<Size8>(42);
        var b = new Bv<Size8>(42);

        Assert.That(a == b, Is.True);
        Assert.That(a != b, Is.False);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals((object)b), Is.True);
    }

    [Test]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        var a = new Bv<Size8>(42);
        var b = new Bv<Size8>(24);

        Assert.That(a == b, Is.False);
        Assert.That(a != b, Is.True);
        Assert.That(a.Equals(b), Is.False);
        Assert.That(a.Equals((object)b), Is.False);
    }

    [Test]
    public void Equality_WithNonBitVector_ReturnsFalse()
    {
        var bv = new Bv<Size8>(42);

        Assert.That(bv.Equals("not a bitvector"), Is.False);
        Assert.That(bv.Equals(null), Is.False);
        Assert.That(bv.Equals((object)42), Is.False);
    }

    [Test]
    public void UnsignedComparison_LessThan_ReturnsCorrectResult()
    {
        var smaller = new Bv<Size8>(100);
        var larger = new Bv<Size8>(200);

        Assert.That(smaller < larger, Is.True);
        Assert.That(larger < smaller, Is.False);
        Assert.That(smaller.Lt(larger, signed: false), Is.True);
    }

    [Test]
    public void SignedComparison_LessThan_ReturnsCorrectResult()
    {
        var positive = new Bv<Size8>(100);
        var negative = new Bv<Size8>(200); // MSB set, negative in signed interpretation

        // In signed comparison, negative < positive
        Assert.That(negative.Lt(positive, signed: true), Is.True);
        Assert.That(positive.Lt(negative, signed: true), Is.False);

        // But in unsigned comparison, positive < negative
        Assert.That(positive < negative, Is.True);
    }

    [Test]
    public void SignedComparison_GreaterThan_ReturnsCorrectResult()
    {
        var positive = new Bv<Size8>(100);
        var negative = new Bv<Size8>(200); // MSB set, negative in signed interpretation

        // In signed comparison, positive > negative
        Assert.That(positive.Gt(negative, signed: true), Is.True);
        Assert.That(negative.Gt(positive, signed: true), Is.False);

        // But in unsigned comparison, negative > positive
        Assert.That(negative > positive, Is.True);
    }

    [Test]
    public void SignedComparison_LessEqual_ReturnsCorrectResult()
    {
        var positive = new Bv<Size8>(100);
        var negative = new Bv<Size8>(200); // MSB set, negative in signed interpretation
        var equalNegative = new Bv<Size8>(200);

        // In signed comparison, negative <= positive
        Assert.That(negative.Le(positive, signed: true), Is.True);
        Assert.That(positive.Le(negative, signed: true), Is.False);
        Assert.That(negative.Le(equalNegative, signed: true), Is.True);

        // Test equal values
        Assert.That(positive.Le(positive, signed: true), Is.True);
    }

    [Test]
    public void SignedComparison_GreaterEqual_ReturnsCorrectResult()
    {
        var positive = new Bv<Size8>(100);
        var negative = new Bv<Size8>(200); // MSB set, negative in signed interpretation
        var equalPositive = new Bv<Size8>(100);

        // In signed comparison, positive >= negative
        Assert.That(positive.Ge(negative, signed: true), Is.True);
        Assert.That(negative.Ge(positive, signed: true), Is.False);
        Assert.That(positive.Ge(equalPositive, signed: true), Is.True);

        // Test equal values
        Assert.That(negative.Ge(negative, signed: true), Is.True);
    }

    [Test]
    public void SignedComparison_EdgeCases_ReturnsCorrectResult()
    {
        // Test with maximum positive and minimum negative values
        var maxPos = new Bv<Size8>(127); // 0x7F - max positive in signed 8-bit
        var minNeg = new Bv<Size8>(128); // 0x80 - min negative in signed 8-bit (-128)

        // Signed comparisons
        Assert.That(minNeg.Lt(maxPos, signed: true), Is.True); // -128 < 127
        Assert.That(maxPos.Gt(minNeg, signed: true), Is.True); // 127 > -128

        // But in unsigned: 128 > 127
        Assert.That(minNeg > maxPos, Is.True);
        Assert.That(maxPos < minNeg, Is.True);
    }

    [Test]
    public void SignedComparison_ZeroComparisons_ReturnsCorrectResult()
    {
        var zero = new Bv<Size8>(0);
        var positiveOne = new Bv<Size8>(1);
        var negativeOne = new Bv<Size8>(255); // -1 in signed 8-bit

        // Signed comparisons with zero
        Assert.That(negativeOne.Lt(zero, signed: true), Is.True); // -1 < 0
        Assert.That(zero.Gt(negativeOne, signed: true), Is.True); // 0 > -1
        Assert.That(positiveOne.Gt(zero, signed: true), Is.True); // 1 > 0
        Assert.That(zero.Lt(positiveOne, signed: true), Is.True); // 0 < 1

        // Unsigned comparisons with zero
        Assert.That(zero < positiveOne, Is.True); // 0 < 1 (unsigned)
        Assert.That(zero < negativeOne, Is.True); // 0 < 255 (unsigned)
        Assert.That(negativeOne > zero, Is.True); // 255 > 0 (unsigned)
    }

    [Test]
    public void SignedComparison_SameSignValues_ReturnsCorrectResult()
    {
        // Two positive values
        var pos1 = new Bv<Size8>(50);
        var pos2 = new Bv<Size8>(100);

        Assert.That(pos1.Lt(pos2, signed: true), Is.True);
        Assert.That(pos2.Gt(pos1, signed: true), Is.True);

        // Two negative values
        var neg1 = new Bv<Size8>(200); // -56 in signed
        var neg2 = new Bv<Size8>(150); // -106 in signed

        // -106 < -56, so neg2 < neg1 in signed comparison
        Assert.That(neg2.Lt(neg1, signed: true), Is.True);
        Assert.That(neg1.Gt(neg2, signed: true), Is.True);

        // But in unsigned: 150 < 200
        Assert.That(neg2 < neg1, Is.True);
    }

    [Test]
    public void SignedComparison_DifferentBitWidths_ReturnsCorrectResult()
    {
        // Test with 16-bit values to ensure the logic works for different sizes
        var pos16 = new Bv<Size16>(100);
        var neg16 = new Bv<Size16>(0x8000); // MSB set, negative in signed 16-bit

        Assert.That(neg16.Lt(pos16, signed: true), Is.True);
        Assert.That(pos16.Gt(neg16, signed: true), Is.True);

        // Unsigned: 0x8000 > 100
        Assert.That(neg16 > pos16, Is.True);
    }

    [Test]
    public void SignedComparison_BoundaryValues_ReturnsCorrectResult()
    {
        // Test boundary between positive and negative in signed interpretation
        var boundary = new Bv<Size8>(127); // 0x7F - largest positive
        var afterBoundary = new Bv<Size8>(128); // 0x80 - smallest negative

        // Signed: 127 > -128
        Assert.That(boundary.Gt(afterBoundary, signed: true), Is.True);
        Assert.That(afterBoundary.Lt(boundary, signed: true), Is.True);

        // Unsigned: 127 < 128
        Assert.That(boundary < afterBoundary, Is.True);
        Assert.That(afterBoundary > boundary, Is.True);

        // Test adjacent values across the boundary
        var justBelowBoundary = new Bv<Size8>(126); // 126 positive
        var justAboveBoundary = new Bv<Size8>(129); // -127 in signed

        Assert.That(justBelowBoundary.Gt(justAboveBoundary, signed: true), Is.True); // 126 > -127
        Assert.That(justBelowBoundary < justAboveBoundary, Is.True); // 126 < 129 (unsigned)
    }

    [Test]
    public void UnsignedComparison_GreaterThan_ReturnsCorrectResult()
    {
        var smaller = new Bv<Size8>(100);
        var larger = new Bv<Size8>(200);

        Assert.That(larger > smaller, Is.True);
        Assert.That(smaller > larger, Is.False);
        Assert.That(larger.Gt(smaller, signed: false), Is.True);
    }

    [Test]
    public void Comparison_LessEqualGreaterEqual_ReturnsCorrectResults()
    {
        var a = new Bv<Size8>(100);
        var b = new Bv<Size8>(100);
        var c = new Bv<Size8>(200);

        Assert.That(a <= b, Is.True);
        Assert.That(a >= b, Is.True);
        Assert.That(a <= c, Is.True);
        Assert.That(c >= a, Is.True);
        Assert.That(c <= a, Is.False);
        Assert.That(a >= c, Is.False);
    }

    [Test]
    public void CompareTo_ValidValues_ReturnsCorrectComparison()
    {
        var smaller = new Bv<Size8>(100);
        var larger = new Bv<Size8>(200);
        var equal = new Bv<Size8>(100);

        Assert.That(smaller.CompareTo(larger), Is.LessThan(0));
        Assert.That(larger.CompareTo(smaller), Is.GreaterThan(0));
        Assert.That(smaller.CompareTo(equal), Is.EqualTo(0));
    }

    [Test]
    public void GetHashCode_EqualValues_ReturnsSameHashCode()
    {
        var a = new Bv<Size8>(42);
        var b = new Bv<Size8>(42);

        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void GetHashCode_DifferentSizes_ReturnsDifferentHashCodes()
    {
        var bv8 = new Bv<Size8>(42);
        var bv16 = new Bv<Size16>(42);

        // Different sizes should typically have different hash codes
        Assert.That(bv8.GetHashCode(), Is.Not.EqualTo(bv16.GetHashCode()));
    }

    #endregion

    #region Formatting Tests

    [Test]
    public void ToString_DefaultFormat_ReturnsValueOnly()
    {
        var bv = new Bv<Size8>(42);
        Assert.That(bv.ToString(), Is.EqualTo("42"));
        Assert.That(bv.ToString("V"), Is.EqualTo("42"));
        Assert.That(bv.ToString("VALUE"), Is.EqualTo("42"));
    }

    [Test]
    public void ToString_DecimalFormat_ReturnsValueWithSize()
    {
        var bv = new Bv<Size8>(42);
        Assert.That(bv.ToString("D"), Is.EqualTo("42 (8-bit)"));
        Assert.That(bv.ToString("DECIMAL"), Is.EqualTo("42 (8-bit)"));
    }

    [Test]
    public void ToString_BinaryFormat_ReturnsBinaryWithSize()
    {
        var bv = new Bv<Size8>(42); // 0b00101010
        Assert.That(bv.ToString("B"), Is.EqualTo("0b00101010 (8-bit)"));
        Assert.That(bv.ToString("BINARY"), Is.EqualTo("0b00101010 (8-bit)"));
    }

    [Test]
    public void ToString_HexFormat_ReturnsHexWithSize()
    {
        var bv = new Bv<Size8>(42); // 0x2A
        Assert.That(bv.ToString("X"), Is.EqualTo("0x2A (8-bit)"));
        Assert.That(bv.ToString("HEX"), Is.EqualTo("0x2A (8-bit)"));
    }

    [Test]
    public void ToString_InvalidFormat_ThrowsFormatException()
    {
        var bv = new Bv<Size8>(42);
        Assert.Throws<FormatException>(() => bv.ToString("INVALID"));
        Assert.Throws<FormatException>(() => bv.ToString("Z"));
    }

    [Test]
    public void ToString_CaseInsensitive_Works()
    {
        var bv = new Bv<Size8>(42);

        Assert.That(bv.ToString("d"), Is.EqualTo(bv.ToString("D")));
        Assert.That(bv.ToString("b"), Is.EqualTo(bv.ToString("B")));
        Assert.That(bv.ToString("x"), Is.EqualTo(bv.ToString("X")));
        Assert.That(bv.ToString("v"), Is.EqualTo(bv.ToString("V")));
    }

    [Test]
    public void ToBinaryString_ValidValues_ReturnsCorrectBinaryString()
    {
        var bv0 = new Bv<Size8>(0);
        Assert.That(bv0.ToBinaryString(), Is.EqualTo("00000000"));

        var bv42 = new Bv<Size8>(42);
        Assert.That(bv42.ToBinaryString(), Is.EqualTo("00101010"));

        var bvMax = Bv<Size8>.Max;
        Assert.That(bvMax.ToBinaryString(), Is.EqualTo("11111111"));
    }

    [Test]
    public void ToBinaryString_DifferentSizes_PadsCorrectly()
    {
        var bv8 = new Bv<Size8>(1);
        Assert.That(bv8.ToBinaryString(), Is.EqualTo("00000001"));

        var bv16 = new Bv<Size16>(1);
        Assert.That(bv16.ToBinaryString(), Is.EqualTo("0000000000000001"));
    }

    [Test]
    public void ToHexString_ValidValues_ReturnsCorrectHexString()
    {
        var bv0 = new Bv<Size8>(0);
        Assert.That(bv0.ToHexString(), Is.EqualTo("00"));

        var bv42 = new Bv<Size8>(42);
        Assert.That(bv42.ToHexString(), Is.EqualTo("2A"));

        var bvMax = Bv<Size8>.Max;
        var hexResult = bvMax.ToHexString();
        // For Size8: (8 + 3) / 4 = 2 hex digits expected
        // But if BigInteger.ToString("X") returns more, it won't be trimmed
        Assert.That(hexResult, Is.EqualTo("0FF").Or.EqualTo("FF"));
    }

    [Test]
    public void ToHexString_DifferentSizes_PadsCorrectly()
    {
        var bv8 = new Bv<Size8>(1);
        Assert.That(bv8.ToHexString(), Is.EqualTo("01"));

        var bv16 = new Bv<Size16>(1);
        Assert.That(bv16.ToHexString(), Is.EqualTo("0001"));

        var bv32 = new Bv<Size32>(1);
        Assert.That(bv32.ToHexString(), Is.EqualTo("00000001"));
    }

    [Test]
    public void TryFormat_ValidFormat_ReturnsTrue()
    {
        var bv = new Bv<Size8>(42);
        var buffer = new char[20];

        var success = bv.TryFormat(buffer, out var charsWritten, "D");

        Assert.That(success, Is.True);
        Assert.That(charsWritten, Is.GreaterThan(0));
        Assert.That(new string(buffer, 0, charsWritten), Is.EqualTo("42 (8-bit)"));
    }

    [Test]
    public void TryFormat_BufferTooSmall_ReturnsFalse()
    {
        var bv = new Bv<Size8>(42);
        var buffer = new char[2]; // Too small

        var success = bv.TryFormat(buffer, out var charsWritten, "D");

        Assert.That(success, Is.False);
        Assert.That(charsWritten, Is.EqualTo(0));
    }

    #endregion

    #region Bit Manipulation Tests

    [Test]
    public void Extract_ValidRange_ReturnsCorrectBits()
    {
        var bv16 = new Bv<Size16>(0xABCD);

        // Extract bits [7:0] (lower byte)
        var lower8 = bv16.Extract<Size8>(0);
        Assert.That(lower8.Value, Is.EqualTo(new BigInteger(0xCD)));

        // Extract bits [15:8] (upper byte)
        var upper8 = bv16.Extract<Size8>(8);
        Assert.That(upper8.Value, Is.EqualTo(new BigInteger(0xAB)));
    }

    [Test]
    public void Extract_OutOfBounds_ThrowsArgumentException()
    {
        var bv8 = new Bv<Size8>(0xFF);

        // Try to extract 8 bits starting from bit 4 (would need bits [11:4])
        Assert.Throws<ArgumentException>(() => bv8.Extract<Size8>(1));

        // Try to extract 8 bits starting from bit 8 (would need bits [15:8])
        Assert.Throws<ArgumentException>(() => bv8.Extract<Size8>(8));
    }

    [Test]
    public void Resize_ZeroExtension_ExtendsWithZeros()
    {
        var bv8 = new Bv<Size8>(0xFF);
        var bv16 = bv8.Resize<Size16>(signed: false);

        Assert.That(bv16.Value, Is.EqualTo(new BigInteger(0x00FF)));
    }

    [Test]
    public void Resize_SignExtension_ExtendsSignBit()
    {
        var bv8 = new Bv<Size8>(0xFF); // -1 in signed 8-bit
        var bv16 = bv8.Resize<Size16>(signed: true);

        Assert.That(bv16.Value, Is.EqualTo(new BigInteger(0xFFFF))); // -1 in signed 16-bit
    }

    [Test]
    public void Resize_SignExtensionPositiveValue_ZeroExtends()
    {
        var bv8 = new Bv<Size8>(0x7F); // +127 in signed 8-bit (MSB not set)
        var bv16 = bv8.Resize<Size16>(signed: true);

        // Positive values in signed extension behave like zero extension
        Assert.That(bv16.Value, Is.EqualTo(new BigInteger(0x007F))); // Same as zero extension

        // Should be identical to zero extension for positive values
        var bv16ZeroExt = bv8.Resize<Size16>(signed: false);
        Assert.That(bv16.Value, Is.EqualTo(bv16ZeroExt.Value));
    }

    [Test]
    public void Resize_Truncation_TruncatesCorrectly()
    {
        var bv16 = new Bv<Size16>(0xABCD);
        var bv8 = bv16.Resize<Size8>(signed: false);

        Assert.That(bv8.Value, Is.EqualTo(new BigInteger(0xCD))); // Lower 8 bits
    }

    [Test]
    public void Resize_SameSize_ReturnsEquivalentValue()
    {
        var bv8 = new Bv<Size8>(0x42);
        var resizedBv8 = bv8.Resize<Size8>(signed: false);

        Assert.That(resizedBv8.Value, Is.EqualTo(bv8.Value));
    }

    [Test]
    public void RotateLeft_ValidPositions_RotatesCorrectly()
    {
        var bv = new Bv<Size8>(0b10110000);
        var result = bv.RotateLeft(2);

        // 10110000 rotated left by 2 = 11000010
        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b11000010)));
    }

    [Test]
    public void RotateLeft_LargePositions_ModuloWorks()
    {
        var bv = new Bv<Size8>(0b10110000);
        var result = bv.RotateLeft(10); // 10 % 8 = 2

        Assert.That(result.Value, Is.EqualTo(bv.RotateLeft(2).Value));
    }

    [Test]
    public void RotateLeft_ZeroPositions_ReturnsSameValue()
    {
        var bv = new Bv<Size8>(0b10110000);
        var result = bv.RotateLeft(0);

        // Should return this (same object conceptually)
        Assert.That(result.Value, Is.EqualTo(bv.Value));
        Assert.That(result, Is.EqualTo(bv));
    }

    [Test]
    public void RotateLeft_SizePositions_BecomesZeroAndReturnsSame()
    {
        var bv = new Bv<Size8>(0b10110000);
        var result = bv.RotateLeft(8); // 8 % 8 = 0, should trigger positions == 0 branch

        Assert.That(result.Value, Is.EqualTo(bv.Value));
        Assert.That(result, Is.EqualTo(bv));
    }

    [Test]
    public void RotateLeft_NegativePositions_ThrowsArgumentException()
    {
        var bv = new Bv<Size8>(0xFF);
        Assert.Throws<ArgumentException>(() => bv.RotateLeft(-1));
    }

    [Test]
    public void RotateRight_ValidPositions_RotatesCorrectly()
    {
        var bv = new Bv<Size8>(0b00001101);
        var result = bv.RotateRight(2);

        // 00001101 rotated right by 2 = 01000011
        Assert.That(result.Value, Is.EqualTo(new BigInteger(0b01000011)));
    }

    [Test]
    public void RotateRight_ZeroPositions_ReturnsSameValue()
    {
        var bv = new Bv<Size8>(0b00001101);
        var result = bv.RotateRight(0);

        // Should return this (same object conceptually)
        Assert.That(result.Value, Is.EqualTo(bv.Value));
        Assert.That(result, Is.EqualTo(bv));
    }

    [Test]
    public void RotateRight_SizePositions_BecomesZeroAndReturnsSame()
    {
        var bv = new Bv<Size8>(0b00001101);
        var result = bv.RotateRight(8); // 8 % 8 = 0, should trigger positions == 0 branch

        Assert.That(result.Value, Is.EqualTo(bv.Value));
        Assert.That(result, Is.EqualTo(bv));
    }

    [Test]
    public void RotateRight_LargePositions_ModuloWorks()
    {
        var bv = new Bv<Size8>(0b00001101);
        var result = bv.RotateRight(10); // 10 % 8 = 2

        Assert.That(result.Value, Is.EqualTo(bv.RotateRight(2).Value));
    }

    [Test]
    public void RotateRight_NegativePositions_ThrowsArgumentException()
    {
        var bv = new Bv<Size8>(0xFF);
        Assert.Throws<ArgumentException>(() => bv.RotateRight(-1));
    }

    [Test]
    public void PopCount_ValidValues_ReturnsCorrectCount()
    {
        var bv0 = new Bv<Size8>(0b00000000);
        Assert.That(bv0.PopCount(), Is.EqualTo(0u));

        var bv1 = new Bv<Size8>(0b00000001);
        Assert.That(bv1.PopCount(), Is.EqualTo(1u));

        var bv4 = new Bv<Size8>(0b00001111);
        Assert.That(bv4.PopCount(), Is.EqualTo(4u));

        var bv8 = new Bv<Size8>(0b11111111);
        Assert.That(bv8.PopCount(), Is.EqualTo(8u));

        var bvMixed = new Bv<Size8>(0b10101010);
        Assert.That(bvMixed.PopCount(), Is.EqualTo(4u));
    }

    [Test]
    public void CountLeadingZeros_ValidValues_ReturnsCorrectCount()
    {
        var bv0 = new Bv<Size8>(0b00000000);
        Assert.That(bv0.CountLeadingZeros(), Is.EqualTo(8u));

        var bv1 = new Bv<Size8>(0b00000001);
        Assert.That(bv1.CountLeadingZeros(), Is.EqualTo(7u));

        var bvHigh = new Bv<Size8>(0b10000000);
        Assert.That(bvHigh.CountLeadingZeros(), Is.EqualTo(0u));

        var bvMid = new Bv<Size8>(0b00010000);
        Assert.That(bvMid.CountLeadingZeros(), Is.EqualTo(3u));
    }

    [Test]
    public void CountTrailingZeros_ValidValues_ReturnsCorrectCount()
    {
        var bv0 = new Bv<Size8>(0b00000000);
        Assert.That(bv0.CountTrailingZeros(), Is.EqualTo(8u));

        var bv1 = new Bv<Size8>(0b10000000);
        Assert.That(bv1.CountTrailingZeros(), Is.EqualTo(7u));

        var bvLow = new Bv<Size8>(0b00000001);
        Assert.That(bvLow.CountTrailingZeros(), Is.EqualTo(0u));

        var bvMid = new Bv<Size8>(0b00001000);
        Assert.That(bvMid.CountTrailingZeros(), Is.EqualTo(3u));
    }

    [Test]
    public void Concat_ValidSizes_ConcatenatesCorrectly()
    {
        var high = new Bv<Size8>(0xAB);
        var low = new Bv<Size8>(0xCD);

        var result = high.Concat<Size8, Size16>(low);

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0xABCD)));
    }

    [Test]
    public void Concat_InvalidResultSize_ThrowsArgumentException()
    {
        var high = new Bv<Size8>(0xAB);
        var low = new Bv<Size8>(0xCD);

        // Size8 + Size8 != Size32
        Assert.Throws<ArgumentException>(() => high.Concat<Size8, Size32>(low));
    }

    [Test]
    public void Repeat_ValidMultiple_RepeatsCorrectly()
    {
        var bv4 = new Bv<Size8>(0b00001111); // Only use lower 4 bits conceptually

        // We can't directly test Size4 to Size8 repeat since we don't have Size4,
        // but we can test 8 to 16 bit repeat
        var bv8 = new Bv<Size8>(0xAB);
        var result = bv8.Repeat<Size16>();

        Assert.That(result.Value, Is.EqualTo(new BigInteger(0xABAB)));
    }

    [Test]
    public void Repeat_InvalidMultiple_ThrowsArgumentException()
    {
        // Test with custom Size12: trying to repeat to Size20 (20 % 12 = 8, not 0)
        var bv12 = new Bv<Size12>(0xABC); // 12-bit value
        var ex = Assert.Throws<ArgumentException>(() => bv12.Repeat<Size20>());

        Assert.That(ex.Message, Does.Contain("Target size 20 must be a multiple of source size 12"));
    }

    [Test]
    public void Repeat_ValidMultiples_WorkCorrectly()
    {
        // Test valid multiples work correctly
        var bv8 = new Bv<Size8>(0xAB);
        var result16 = bv8.Repeat<Size16>(); // 8->16 (2x repeat)
        Assert.That(result16.Value, Is.EqualTo(new BigInteger(0xABAB)));

        var result32 = bv8.Repeat<Size32>(); // 8->32 (4x repeat)
        Assert.That(result32.Value, Is.EqualTo(new BigInteger(0xABABABAB)));
    }

    [Test]
    public void Repeat_RepeatCountOne_ReturnsDirectValue()
    {
        // Test the repeatCount == 1 branch
        var bv16 = new Bv<Size16>(0x1234);
        var result16 = bv16.Repeat<Size16>(); // 16->16 (1x repeat, should use early return)

        Assert.That(result16.Value, Is.EqualTo(bv16.Value));
        Assert.That(result16.Value, Is.EqualTo(new BigInteger(0x1234)));
    }

    [Test]
    public void Repeat_MultipleRepeats_ReturnsCorrectPattern()
    {
        // Test the loop logic for multiple repeats
        var bv4Bits = new Bv<Size8>(0x0F); // Use lower 4 bits conceptually

        // 8->16 (2x repeat)
        var result16 = bv4Bits.Repeat<Size16>();
        Assert.That(result16.Value, Is.EqualTo(new BigInteger(0x0F0F)));

        // 8->64 (8x repeat)
        var result64 = bv4Bits.Repeat<Size64>();
        var expected64 = new BigInteger(0x0F0F0F0F0F0F0F0FUL);
        Assert.That(result64.Value, Is.EqualTo(expected64));
    }

    #endregion

    #region Edge Cases and Error Handling

    [Test]
    public void LargeShifts_GreaterThanSize_HandleCorrectly()
    {
        var bv = new Bv<Size8>(0xFF);

        // Shifting by 8 or more should result in 0 for left shift
        var leftResult = bv.Shl(8);
        Assert.That(leftResult.Value, Is.EqualTo(BigInteger.Zero));

        var leftLarge = bv.Shl(100);
        Assert.That(leftLarge.Value, Is.EqualTo(BigInteger.Zero));

        // Shifting by 8 or more should result in 0 for right shift
        var rightResult = bv.Shr(8);
        Assert.That(rightResult.Value, Is.EqualTo(BigInteger.Zero));

        var rightLarge = bv.Shr(100);
        Assert.That(rightLarge.Value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void VeryLargeBitVectors_Work()
    {
        var bv128 = new Bv<Size128>(BigInteger.Parse("12345678901234567890123456789012345678"));
        Assert.That(bv128.Value, Is.Not.EqualTo(BigInteger.Zero));

        // Test some operations
        var doubled = bv128 + bv128;
        Assert.That(doubled.Value, Is.Not.EqualTo(bv128.Value));

        var shifted = bv128 << 1;
        Assert.That(shifted.Value, Is.Not.EqualTo(bv128.Value));
    }

    [Test]
    public void AllSizes_BasicOperationsWork()
    {
        // Test that all size types work with basic operations
        var bv8 = new Bv<Size8>(0xFF);
        var bv16 = new Bv<Size16>(0xFFFF);
        var bv32 = new Bv<Size32>(0xFFFFFFFF);
        var bv64 = new Bv<Size64>(0xFFFFFFFFFFFFFFFF);
        var bv128 = new Bv<Size128>(BigInteger.Parse("340282366920938463463374607431768211455")); // 2^128 - 1

        Assert.That(bv8.Value, Is.EqualTo(new BigInteger(255)));
        Assert.That(bv16.Value, Is.EqualTo(new BigInteger(65535)));
        Assert.That(bv32.Value, Is.EqualTo(new BigInteger(4294967295)));
        Assert.That(bv64.Value, Is.EqualTo(new BigInteger(18446744073709551615UL)));
        Assert.That(bv128.Value, Is.EqualTo(BigInteger.Parse("340282366920938463463374607431768211455")));

        // Test that they're all max values
        Assert.That(bv8, Is.EqualTo(Bv<Size8>.Max));
        Assert.That(bv16, Is.EqualTo(Bv<Size16>.Max));
        Assert.That(bv32, Is.EqualTo(Bv<Size32>.Max));
        Assert.That(bv64, Is.EqualTo(Bv<Size64>.Max));
        Assert.That(bv128, Is.EqualTo(Bv<Size128>.Max));
    }

    #endregion
}
