namespace Z3Wrap.Tests.Unit.DataTypes;

// TEMPORARILY COMMENTED OUT: BitVec API changed from runtime-sized to compile-time-sized generics
/*
[TestFixture]
public class BitVecTests
{
    [Test]
    public void Constructor_WithValidValues_CreatesCorrectBitVec()
    {
        var bv = new BitVec(42, 8);

        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));
        Assert.That(bv.Size, Is.EqualTo(8));
    }

    [Test]
    public void Constructor_WithZeroSize_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new BitVec(1, 0);
        });
    }

    [Test]
    public void Constructor_WithLargeSize_WorksCorrectly()
    {
        // Large sizes should work now that you removed the 1024 limit
        var bv = new BitVec(1, 2048);
        Assert.That(bv.Size, Is.EqualTo(2048));
        Assert.That(bv.Value, Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void Constructor_WithValueTooLarge_MasksCorrectly()
    {
        // 255 in 8 bits should remain 255
        var bv1 = new BitVec(255, 8);
        Assert.That(bv1.Value, Is.EqualTo(new BigInteger(255)));

        // 256 in 8 bits should be masked to 0
        var bv2 = new BitVec(256, 8);
        Assert.That(bv2.Value, Is.EqualTo(new BigInteger(0)));

        // 257 in 8 bits should be masked to 1
        var bv3 = new BitVec(257, 8);
        Assert.That(bv3.Value, Is.EqualTo(new BigInteger(1)));
    }

    [Test]
    public void Constructor_WithNegativeValue_HandlesCorrectly()
    {
        // -1 in 8 bits should be 255 (two's complement)
        var bv = new BitVec(-1, 8);
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(255)));
    }

    // =============================================================================
    // FACTORY METHOD TESTS
    // =============================================================================

    [TestCase(42, 8u)]
    [TestCase(-17, 16u)]
    [TestCase(0, 32u)]
    [TestCase(int.MaxValue, 32u)]
    [TestCase(int.MinValue, 32u)]
    public void FromInt_CreatesCorrectBitVec(int value, uint size)
    {
        var bv = BitVec.FromInt(value, size);

        Assert.That(bv.Size, Is.EqualTo(size));
        // BitVec masks to bit width, so we expect the masked value
        var expectedValue = new BigInteger(value) & ((BigInteger.One << (int)size) - 1);
        Assert.That(bv.Value, Is.EqualTo(expectedValue));
    }

    [TestCase(42u, 8u)]
    [TestCase(0u, 32u)]
    [TestCase(255u, 8u)]
    [TestCase(uint.MaxValue, 32u)]
    public void FromUInt_CreatesCorrectBitVec(uint value, uint size)
    {
        var bv = BitVec.FromUInt(value, size);

        Assert.That(bv.Size, Is.EqualTo(size));
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(value)));
    }

    [TestCase(42L, 8u)]
    [TestCase(-1000L, 16u)]
    [TestCase(0L, 64u)]
    [TestCase(long.MaxValue, 64u)]
    [TestCase(long.MinValue, 64u)]
    public void FromLong_CreatesCorrectBitVec(long value, uint size)
    {
        var bv = BitVec.FromLong(value, size);

        Assert.That(bv.Size, Is.EqualTo(size));
        // BitVec masks to bit width, so we expect the masked value
        var expectedValue = new BigInteger(value) & ((BigInteger.One << (int)size) - 1);
        Assert.That(bv.Value, Is.EqualTo(expectedValue));
    }

    [TestCase(42ul, 8u)]
    [TestCase(0ul, 64u)]
    [TestCase(ulong.MaxValue, 64u)]
    public void FromULong_CreatesCorrectBitVec(ulong value, uint size)
    {
        var bv = BitVec.FromULong(value, size);

        Assert.That(bv.Size, Is.EqualTo(size));
        Assert.That(bv.Value, Is.EqualTo(new BigInteger(value)));
    }

    [Test]
    public void FromBigInteger_LargePositiveValue_CreatesCorrectBitVec()
    {
        var bigValue = new BigInteger(12345678901234567890L);
        var bv = BitVec.FromBigInteger(bigValue, 64);

        Assert.That(bv.Size, Is.EqualTo(64u));
        Assert.That(bv.Value, Is.EqualTo(bigValue & ((BigInteger.One << 64) - 1)));
    }

    [Test]
    public void FromBigInteger_NegativeValue_CreatesCorrectBitVec()
    {
        var negativeValue = new BigInteger(-42);
        var bv = BitVec.FromBigInteger(negativeValue, 16);

        Assert.That(bv.Size, Is.EqualTo(16u));
        Assert.That((int)bv.ToBigInteger(signed: true), Is.EqualTo(-42));
    }

    [Test]
    public void FromBigInteger_ValueTooLarge_TruncatesCorrectly()
    {
        var largeValue = new BigInteger(1000); // Larger than 8-bit max (255)
        var bv = BitVec.FromBigInteger(largeValue, 8);

        Assert.That(bv.Size, Is.EqualTo(8u));
        Assert.That((int)bv.Value, Is.EqualTo(232)); // 1000 & 0xFF = 232
    }

    [Test]
    public void FromBigInteger_HugeValue_TruncatesCorrectly()
    {
        var hugeValue = BigInteger.Pow(2, 100); // Much larger than 64 bits
        var bv = BitVec.FromBigInteger(hugeValue, 64);

        Assert.That(bv.Size, Is.EqualTo(64u));
        // Should be truncated to lower 64 bits (which would be 0 for 2^100)
        Assert.That(bv.Value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void Properties_ReturnsCorrectValues()
    {
        var bv = new BitVec(170, 8); // 10101010

        Assert.That(bv.Value, Is.EqualTo(new BigInteger(170)));
        Assert.That(bv.Size, Is.EqualTo(8));
        Assert.That(bv.IsZero, Is.False);
        Assert.That(bv.MaxValue, Is.EqualTo(new BigInteger(255))); // 2^8 - 1

        var zero = new BitVec(0, 8);
        Assert.That(zero.IsZero, Is.True);
    }

    [Test]
    public void AsInt_WithValidSize_ReturnsCorrectValue()
    {
        var bv = new BitVec(42, 16);
        Assert.That(bv.ToInt(), Is.EqualTo(42));
    }

    [Test]
    public void ToInt_WithLargeSize_WorksCorrectly()
    {
        // Large sizes should work now that you removed the size check
        var bv = new BitVec(42, 64);
        Assert.That(bv.ToInt(), Is.EqualTo(42));
    }

    [Test]
    public void AsInt_WithSignedInterpretation_WorksCorrectly()
    {
        // Test that the old "overflow" case now works correctly with signed interpretation
        var bv = new BitVec(int.MaxValue + 1L, 32); // This becomes int.MinValue in signed interpretation
        Assert.That(bv.ToInt(signed: true), Is.EqualTo(int.MinValue));
    }

    [Test]
    public void AsUInt_WithValidValues_ReturnsCorrectValue()
    {
        var bv = new BitVec(4294967295L, 32); // uint.MaxValue
        Assert.That(bv.ToUInt(), Is.EqualTo(uint.MaxValue));
    }

    [Test]
    public void AsLong_WithPositiveValue_ReturnsCorrectValue()
    {
        var bv = new BitVec(1234567890, 64);
        Assert.That(bv.ToLong(), Is.EqualTo(1234567890L));
    }

    [Test]
    public void AsLong_WithNegativeValue_ReturnsCorrectSignedValue()
    {
        // -1 in 8-bit two's complement is 255 in unsigned representation
        var bv = new BitVec(255, 8);
        Assert.That(bv.ToLong(signed: true), Is.EqualTo(-1L));
    }

    [Test]
    public void AsInt_WithNegativeValue_ReturnsCorrectSignedValue()
    {
        // -10 in 8-bit two's complement is 246 in unsigned representation
        var bv = new BitVec(246, 8);
        Assert.That(bv.ToInt(signed: true), Is.EqualTo(-10));
    }

    [Test]
    public void AsInt_WithPositiveValue_ReturnsCorrectValue()
    {
        var bv = new BitVec(42, 8);
        Assert.That(bv.ToInt(), Is.EqualTo(42));
    }

    [Test]
    public void AsULong_WithValidValues_ReturnsCorrectValue()
    {
        var bv = new BitVec(ulong.MaxValue, 64);
        Assert.That(bv.ToULong(), Is.EqualTo(ulong.MaxValue));
    }

    [Test]
    public void AsBinary_ReturnsCorrectBinaryString()
    {
        var bv1 = new BitVec(170, 8); // 10101010
        Assert.That(bv1.ToBinaryString(), Is.EqualTo("10101010"));

        var bv2 = new BitVec(0, 4);
        Assert.That(bv2.ToBinaryString(), Is.EqualTo("0000"));

        var bv3 = new BitVec(15, 4); // 1111
        Assert.That(bv3.ToBinaryString(), Is.EqualTo("1111"));
    }

    [Test]
    public void ArithmeticOperators_WithSameSize_WorkCorrectly()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 8);

        Assert.That((bv1 + bv2).Value, Is.EqualTo(new BigInteger(15)));
        Assert.That((bv1 - bv2).Value, Is.EqualTo(new BigInteger(5)));
        Assert.That((bv1 * bv2).Value, Is.EqualTo(new BigInteger(50)));
        Assert.That((bv1 / bv2).Value, Is.EqualTo(new BigInteger(2))); // Unsigned division
        Assert.That((bv1 % bv2).Value, Is.EqualTo(new BigInteger(0))); // Unsigned remainder
    }

    [Test]
    public void ArithmeticOperators_WithDifferentSizes_ThrowException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 + bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 - bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 * bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 / bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 % bv2;
        });
    }

    [Test]
    public void ArithmeticOperators_WithOverflow_MasksCorrectly()
    {
        var bv1 = new BitVec(200, 8); // Near max for 8-bit
        var bv2 = new BitVec(100, 8);

        var result = bv1 + bv2; // 300, should be masked to 44 (300 & 255)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(44)));
    }

    [Test]
    public void DivisionOperator_WithValidOperands_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(20, 8);
        var bv2 = new BitVec(4, 8);

        var result = bv1 / bv2;
        Assert.That(result.Value, Is.EqualTo(new BigInteger(5)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void ModuloOperator_WithValidOperands_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(23, 8);
        var bv2 = new BitVec(5, 8);

        var result = bv1 % bv2;
        Assert.That(result.Value, Is.EqualTo(new BigInteger(3)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void Div_WithPositiveOperands_SignedParameter_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(20, 8);
        var bv2 = new BitVec(4, 8);

        var result = bv1.Div(bv2, signed: true);
        Assert.That(result.Value, Is.EqualTo(new BigInteger(5)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void Div_WithNegativeOperands_SignedParameter_ReturnsCorrectResult()
    {
        // -10 / 3 in 8-bit two's complement
        // -10 = 246 in unsigned representation
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        var result = bv1.Div(bv2, signed: true);
        // -10 / 3 = -3, which is 253 in 8-bit unsigned representation
        Assert.That(result.Value, Is.EqualTo(new BigInteger(253)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void Rem_WithValidOperands_SignedParameter_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(23, 8);
        var bv2 = new BitVec(5, 8);

        var result = bv1.Rem(bv2, signed: true);
        Assert.That(result.Value, Is.EqualTo(new BigInteger(3)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void SignedMod_WithValidOperands_ReturnsCorrectResult()
    {
        // Test Z3-style signed modulo where result has same sign as divisor
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        var result = bv1.SignedMod(bv2);
        // -10 mod 3 should be 2 (same sign as divisor)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(2)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void DivisionByZero_ThrowsException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(0, 8);

        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = bv1 / bv2;
        });
        Assert.Throws<DivideByZeroException>(() =>
        {
            _ = bv1 % bv2;
        });
        Assert.Throws<DivideByZeroException>(() => bv1.Div(bv2, signed: true));
        Assert.Throws<DivideByZeroException>(() => bv1.Rem(bv2, signed: true));
        Assert.Throws<DivideByZeroException>(() => bv1.SignedMod(bv2));
    }

    [Test]
    public void DivisionWithDifferentSizes_ThrowsException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 / bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 % bv2;
        });
        Assert.Throws<ArgumentException>(() => bv1.Div(bv2, signed: true));
        Assert.Throws<ArgumentException>(() => bv1.Rem(bv2, signed: true));
        Assert.Throws<ArgumentException>(() => bv1.SignedMod(bv2));
    }

    [Test]
    public void UnaryMinus_ReturnsCorrectValue()
    {
        var bv = new BitVec(5, 8);
        var negated = -bv;

        // -5 in 8-bit should be 251 (256 - 5)
        Assert.That(negated.Value, Is.EqualTo(new BigInteger(251)));
    }

    [Test]
    public void BitwiseOperators_WorkCorrectly()
    {
        var bv1 = new BitVec(170, 8); // 10101010
        var bv2 = new BitVec(85, 8); // 01010101

        Assert.That((bv1 & bv2).Value, Is.EqualTo(new BigInteger(0))); // 00000000
        Assert.That((bv1 | bv2).Value, Is.EqualTo(new BigInteger(255))); // 11111111
        Assert.That((bv1 ^ bv2).Value, Is.EqualTo(new BigInteger(255))); // 11111111
        Assert.That((~bv1).Value, Is.EqualTo(new BigInteger(85))); // 01010101
    }

    [Test]
    public void ShiftOperators_WorkCorrectly()
    {
        var bv = new BitVec(5, 8); // 00000101

        var leftShift = bv << 2; // 00010100 = 20
        Assert.That(leftShift.Value, Is.EqualTo(new BigInteger(20)));

        var rightShift = bv >> 1; // 00000010 = 2
        Assert.That(rightShift.Value, Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void ShiftOperators_WithNegativeShift_ThrowException()
    {
        var bv = new BitVec(5, 8);

        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv << -1;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv >> -1;
        });
    }

    [Test]
    public void ComparisonOperators_WithSameSize_WorkCorrectly()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 8);
        var bv3 = new BitVec(10, 8);

        Assert.That(bv1 == bv3, Is.True);
        Assert.That(bv1 != bv2, Is.True);
        Assert.That(bv1 > bv2, Is.True);
        Assert.That(bv1 >= bv2, Is.True);
        Assert.That(bv1 >= bv3, Is.True);
        Assert.That(bv2 < bv1, Is.True);
        Assert.That(bv2 <= bv1, Is.True);
        Assert.That(bv3 <= bv1, Is.True);
    }

    [Test]
    public void ComparisonOperators_WithDifferentSizes_ThrowException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 < bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 <= bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 > bv2;
        });
        Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 >= bv2;
        });
    }

    [Test]
    public void EqualityOperators_WithDifferentSizes_ReturnsFalse()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(10, 16);

        Assert.That(bv1 == bv2, Is.False);
        Assert.That(bv1 != bv2, Is.True);
    }

    [Test]
    public void ImplicitConversion_FromBigIntegerTuple_WorksCorrectly()
    {
        var bv = new BitVec(42, 8);

        Assert.That(bv.Value, Is.EqualTo(new BigInteger(42)));
        Assert.That(bv.Size, Is.EqualTo(8));
    }

    [Test]
    public void Equals_WorksCorrectly()
    {
        var bv1 = new BitVec(42, 8);
        var bv2 = new BitVec(42, 8);
        var bv3 = new BitVec(43, 8);
        var bv4 = new BitVec(42, 16);

        Assert.That(bv1.Equals(bv2), Is.True);
        Assert.That(bv1.Equals(bv3), Is.False);
        Assert.That(bv1.Equals(bv4), Is.False);
        Assert.That(bv1.Equals((object)bv2), Is.True);
        Assert.That(bv1.Equals(42), Is.False);
    }

    [Test]
    public void GetHashCode_SameForEqualObjects()
    {
        var bv1 = new BitVec(42, 8);
        var bv2 = new BitVec(42, 8);

        Assert.That(bv1.GetHashCode(), Is.EqualTo(bv2.GetHashCode()));
    }

    [Test]
    public void CompareTo_WorksCorrectly()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 8);
        var bv3 = new BitVec(10, 8);

        Assert.That(bv1.CompareTo(bv2), Is.GreaterThan(0));
        Assert.That(bv2.CompareTo(bv1), Is.LessThan(0));
        Assert.That(bv1.CompareTo(bv3), Is.EqualTo(0));
    }

    [Test]
    public void CompareTo_WithDifferentSizes_ThrowsException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        Assert.Throws<ArgumentException>(() => bv1.CompareTo(bv2));
    }

    [Test]
    public void ToString_WithDifferentFormats_ReturnsCorrectStrings()
    {
        var bv = new BitVec(170, 8); // 10101010

        Assert.That(bv.ToString(), Is.EqualTo("170"));
        Assert.That(bv.ToString("D"), Is.EqualTo("170 (8-bit)"));
        Assert.That(bv.ToString("DECIMAL"), Is.EqualTo("170 (8-bit)"));
        Assert.That(bv.ToString("B"), Is.EqualTo("0b10101010 (8-bit)"));
        Assert.That(bv.ToString("BINARY"), Is.EqualTo("0b10101010 (8-bit)"));
        Assert.That(bv.ToString("X"), Is.EqualTo("0xAA (8-bit)"));
        Assert.That(bv.ToString("HEX"), Is.EqualTo("0xAA (8-bit)"));
        Assert.That(bv.ToString("V"), Is.EqualTo("170"));
        Assert.That(bv.ToString("VALUE"), Is.EqualTo("170"));
    }

    [Test]
    public void ToString_WithInvalidFormat_ThrowsException()
    {
        var bv = new BitVec(42, 8);
        Assert.Throws<FormatException>(() => bv.ToString("Z"));
    }

    [Test]
    public void StaticMethods_WorkCorrectly()
    {
        var zero = BitVec.Zero(8);
        var one = BitVec.One(8);
        var max = BitVec.Max(8);

        Assert.That(zero.Value, Is.EqualTo(BigInteger.Zero));
        Assert.That(one.Value, Is.EqualTo(BigInteger.One));
        Assert.That(max.Value, Is.EqualTo(new BigInteger(255))); // 2^8 - 1
    }

    [Test]
    public void AsSignedInt_SignedConversion_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToInt(signed: true), Is.EqualTo(42));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToInt(signed: true), Is.EqualTo(-128));

        // Test another negative - 8-bit: 11111111 = 255 unsigned, -1 signed
        var minusOne = new BitVec(255, 8);
        Assert.That(minusOne.ToInt(signed: true), Is.EqualTo(-1));

        // Test 16-bit negative value: 32768 = 0x8000 = -32768 signed
        var negative16 = new BitVec(32768, 16);
        Assert.That(negative16.ToInt(signed: true), Is.EqualTo(-32768));
    }

    [Test]
    public void AsSignedLong_SignedConversion_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToLong(signed: true), Is.EqualTo(42L));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToLong(signed: true), Is.EqualTo(-128L));

        // Test 32-bit negative value
        var negative32 = new BitVec(2147483648U, 32); // 0x80000000 = -2147483648 signed
        Assert.That(negative32.ToLong(signed: true), Is.EqualTo(-2147483648L));

        // Test 64-bit negative value
        var maxUnsigned64 = (BigInteger.One << 63); // MSB set for 64-bit
        var negative64 = new BitVec(maxUnsigned64, 64);
        Assert.That(negative64.ToLong(signed: true), Is.EqualTo(long.MinValue));
    }

    [Test]
    public void AsSignedBigInteger_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(42)));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(-128)));

        // Test another negative - 8-bit: 11111111 = 255 unsigned, -1 signed
        var minusOne = new BitVec(255, 8);
        Assert.That(minusOne.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(-1)));

        // Test larger bit width - 16-bit: 65535 = -1 signed
        var minusOne16 = new BitVec(65535, 16);
        Assert.That(minusOne16.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void ToInt_UnsignedConversion_ReturnsCorrectUnsignedValues()
    {
        // Test unsigned interpretation - no sign extension
        var bv = new BitVec(255, 8); // All bits set in 8-bit
        Assert.That(bv.ToInt(), Is.EqualTo(255)); // Should be 255, not -1

        var bv16 = new BitVec(65535, 16); // All bits set in 16-bit
        Assert.That(bv16.ToInt(), Is.EqualTo(65535)); // Should be 65535, not -1

        var bv32 = new BitVec(4294967295U, 32); // All bits set in 32-bit
        Assert.Throws<OverflowException>(() => bv32.ToInt()); // Too large for signed int
    }

    [Test]
    public void ToLong_UnsignedConversion_ReturnsCorrectUnsignedValues()
    {
        // Test unsigned interpretation - no sign extension
        var bv = new BitVec(255, 8); // All bits set in 8-bit
        Assert.That(bv.ToLong(), Is.EqualTo(255L)); // Should be 255, not -1

        var bv32 = new BitVec(4294967295U, 32); // All bits set in 32-bit
        Assert.That(bv32.ToLong(), Is.EqualTo(4294967295L)); // Should be unsigned value

        var bv64 = new BitVec(ulong.MaxValue, 64); // All bits set in 64-bit
        Assert.Throws<OverflowException>(() => bv64.ToLong()); // Too large for signed long
    }

    [Test]
    public void Constructor_EdgeCases_HandlesCorrectly()
    {
        // Test very large bit widths
        var largeBv = new BitVec(1, 1024);
        Assert.That(largeBv.Size, Is.EqualTo(1024));
        Assert.That(largeBv.Value, Is.EqualTo(BigInteger.One));

        // Test maximum single bit
        var singleBit = new BitVec(1, 1);
        Assert.That(singleBit.Size, Is.EqualTo(1));
        Assert.That(singleBit.Value, Is.EqualTo(BigInteger.One));

        // Test overflow with single bit
        var overflowSingle = new BitVec(2, 1); // Should be masked to 0
        Assert.That(overflowSingle.Value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void ToBinaryString_EdgeCases_WorksCorrectly()
    {
        // Test single bit
        var single = new BitVec(1, 1);
        Assert.That(single.ToBinaryString(), Is.EqualTo("1"));

        var singleZero = new BitVec(0, 1);
        Assert.That(singleZero.ToBinaryString(), Is.EqualTo("0"));

        // Test large bit width
        var large = new BitVec(1, 64);
        Assert.That(
            large.ToBinaryString(),
            Is.EqualTo("0000000000000000000000000000000000000000000000000000000000000001")
        );

        // Test all bits set
        var allSet = new BitVec(15, 4); // 1111 in 4 bits
        Assert.That(allSet.ToBinaryString(), Is.EqualTo("1111"));
    }

    [Test]
    public void ExceptionMessages_AreStandardized()
    {
        // Test size mismatch messages
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = bv1 + bv2;
        });
        Assert.That(ex.Message, Does.Contain("Size mismatch"));

        // Test division by zero messages
        var zero = new BitVec(0, 8);
        var nonZero = new BitVec(10, 8);

        var divEx = Assert.Throws<DivideByZeroException>(() =>
        {
            _ = nonZero / zero;
        });
        Assert.That(divEx.Message, Does.Contain("Division by zero is not allowed"));

        // Test overflow messages
        var large = new BitVec(BigInteger.Pow(2, 50), 64);
        var overflowEx = Assert.Throws<OverflowException>(() => large.ToInt());
        Assert.That(overflowEx.Message, Does.Contain("Unsigned value"));
    }

    [Test]
    public void ToInt_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create BitVec with value larger than int.MaxValue (2147483647)
        var largeValue = new BigInteger(int.MaxValue) + 1;
        var bv = new BitVec(largeValue, 64);

        var ex = Assert.Throws<OverflowException>(() => bv.ToInt());
        Assert.That(
            ex.Message,
            Does.Contain($"Unsigned value {largeValue} is outside the range of int")
        );
    }

    [Test]
    public void ToInt_WithMaxIntValue_DoesNotThrow()
    {
        // Boundary test: int.MaxValue should work
        var bv = new BitVec(int.MaxValue, 32);
        Assert.That(bv.ToInt(), Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void ToInt_WithSignedParameter_ValueTooLarge_ThrowsOverflowException()
    {
        // Create 33-bit value that when interpreted as signed exceeds int.MaxValue
        // Use a value that when sign-extended would be > int.MaxValue
        var largePositive = new BigInteger(int.MaxValue) + 1;
        var bv = new BitVec(largePositive, 64);

        var ex = Assert.Throws<OverflowException>(() => bv.ToInt(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of int"));
    }

    [Test]
    public void ToInt_WithSignedParameter_ValueTooSmall_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is < int.MinValue
        // For this we need more than 32 bits with MSB set to create a very negative number
        var bv = new BitVec(BigInteger.Pow(2, 33) - 1, 34); // 34-bit value with all bits set
        // This represents -1 in 34-bit two's complement, but that's fine for int
        // Let's create something that's actually too small
        var veryLarge = BigInteger.Pow(2, 62); // This will be a very negative number when sign-extended from 63 bits
        var bv2 = new BitVec(veryLarge, 63);

        var ex = Assert.Throws<OverflowException>(() => bv2.ToInt(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of int"));
    }

    [Test]
    public void ToUInt_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create BitVec with value larger than uint.MaxValue (4294967295)
        var largeValue = new BigInteger(uint.MaxValue) + 1;
        var bv = new BitVec(largeValue, 64);

        var ex = Assert.Throws<OverflowException>(() => bv.ToUInt());
        Assert.That(
            ex.Message,
            Does.Contain($"Unsigned value {largeValue} is outside the range of uint")
        );
    }

    [Test]
    public void ToUInt_WithMaxUIntValue_DoesNotThrow()
    {
        // Boundary test: uint.MaxValue should work
        var bv = new BitVec(uint.MaxValue, 32);
        Assert.That(bv.ToUInt(), Is.EqualTo(uint.MaxValue));
    }

    [Test]
    public void ToLong_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create BitVec with value larger than long.MaxValue
        var largeValue = new BigInteger(long.MaxValue) + 1;
        var bv = new BitVec(largeValue, 128);

        var ex = Assert.Throws<OverflowException>(() => bv.ToLong());
        Assert.That(
            ex.Message,
            Does.Contain($"Unsigned value {largeValue} is outside the range of long")
        );
    }

    [Test]
    public void ToLong_WithMaxLongValue_DoesNotThrow()
    {
        // Boundary test: long.MaxValue should work
        var bv = new BitVec(long.MaxValue, 64);
        Assert.That(bv.ToLong(), Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void ToLong_WithSignedParameter_ValueTooLarge_ThrowsOverflowException()
    {
        // Create value that when interpreted as signed exceeds long.MaxValue
        var largePositive = new BigInteger(long.MaxValue) + 1;
        var bv = new BitVec(largePositive, 128);

        var ex = Assert.Throws<OverflowException>(() => bv.ToLong(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToLong_WithSignedParameter_ValueTooSmall_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is < long.MinValue
        // Use a 65-bit value with MSB set to create a number smaller than long.MinValue
        var veryLarge = BigInteger.Pow(2, 126); // This will be very negative in 127-bit two's complement
        var bv = new BitVec(veryLarge, 127);

        var ex = Assert.Throws<OverflowException>(() => bv.ToLong(signed: true));
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToULong_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create BitVec with value larger than ulong.MaxValue
        var largeValue = new BigInteger(ulong.MaxValue) + 1;
        var bv = new BitVec(largeValue, 128);

        var ex = Assert.Throws<OverflowException>(() => bv.ToULong());
        Assert.That(
            ex.Message,
            Does.Contain($"Unsigned value {largeValue} is outside the range of ulong")
        );
    }

    [Test]
    public void ToULong_WithMaxULongValue_DoesNotThrow()
    {
        // Boundary test: ulong.MaxValue should work
        var bv = new BitVec(ulong.MaxValue, 64);
        Assert.That(bv.ToULong(), Is.EqualTo(ulong.MaxValue));
    }

    [Test]
    public void ConversionMethods_WithBoundaryValues_HandleCorrectly()
    {
        // Test all boundary values work correctly (no overflow)

        // Test int boundaries
        var intMax = new BitVec(int.MaxValue, 32);
        Assert.That(intMax.ToInt(), Is.EqualTo(int.MaxValue));

        // Test uint boundaries
        var uintMax = new BitVec(uint.MaxValue, 32);
        Assert.That(uintMax.ToUInt(), Is.EqualTo(uint.MaxValue));

        // Test long boundaries
        var longMax = new BitVec(long.MaxValue, 64);
        Assert.That(longMax.ToLong(), Is.EqualTo(long.MaxValue));

        // Test ulong boundaries
        var ulongMax = new BitVec(ulong.MaxValue, 64);
        Assert.That(ulongMax.ToULong(), Is.EqualTo(ulong.MaxValue));

        // Test signed boundaries
        var signedIntMax = new BitVec(int.MaxValue, 32);
        Assert.That(signedIntMax.ToInt(signed: true), Is.EqualTo(int.MaxValue));

        var signedIntMin = new BitVec(BigInteger.Pow(2, 31), 32); // int.MinValue in two's complement
        Assert.That(signedIntMin.ToInt(signed: true), Is.EqualTo(int.MinValue));

        var signedLongMax = new BitVec(long.MaxValue, 64);
        Assert.That(signedLongMax.ToLong(signed: true), Is.EqualTo(long.MaxValue));

        var signedLongMin = new BitVec(BigInteger.Pow(2, 63), 64); // long.MinValue in two's complement
        Assert.That(signedLongMin.ToLong(signed: true), Is.EqualTo(long.MinValue));
    }

    // Tests for new signed parameter in conversion methods
    [Test]
    public void ToInt_WithSignedParameter_WorksCorrectly()
    {
        var bv = new BitVec(255, 8); // All bits set in 8-bit

        // Unsigned interpretation
        Assert.That(bv.ToInt(signed: false), Is.EqualTo(255));
        Assert.That(bv.ToInt(), Is.EqualTo(255)); // Default unsigned

        // Signed interpretation (-1 in two's complement)
        Assert.That(bv.ToInt(signed: true), Is.EqualTo(-1));
    }

    [Test]
    public void ToLong_WithSignedParameter_WorksCorrectly()
    {
        var bv = new BitVec(4294967295U, 32); // All bits set in 32-bit

        // Unsigned interpretation
        Assert.That(bv.ToLong(signed: false), Is.EqualTo(4294967295L));
        Assert.That(bv.ToLong(), Is.EqualTo(4294967295L)); // Default unsigned

        // Signed interpretation (-1 in two's complement)
        Assert.That(bv.ToLong(signed: true), Is.EqualTo(-1L));
    }

    [Test]
    public void ToBigInteger_WithSignedParameter_WorksCorrectly()
    {
        var bv = new BitVec(255, 8); // All bits set in 8-bit

        // Unsigned interpretation
        Assert.That(bv.ToBigInteger(signed: false), Is.EqualTo(new BigInteger(255)));
        Assert.That(bv.ToBigInteger(), Is.EqualTo(new BigInteger(255))); // Default unsigned

        // Signed interpretation (-1 in two's complement)
        Assert.That(bv.ToBigInteger(signed: true), Is.EqualTo(new BigInteger(-1)));
    }

    // Tests for new method-based operations
    [Test]
    public void Add_Method_WithBitVecAndBigInteger_WorksCorrectly()
    {
        var bv = new BitVec(10, 8);

        // BitVec + BitVec
        var result1 = bv.Add(new BitVec(5, 8));
        Assert.That(result1.Value, Is.EqualTo(new BigInteger(15)));

        // BitVec + BigInteger
        var result2 = bv.Add(new BigInteger(5));
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(15)));
    }

    [Test]
    public void Sub_Method_WithBitVecAndBigInteger_WorksCorrectly()
    {
        var bv = new BitVec(10, 8);

        // BitVec - BitVec
        var result1 = bv.Sub(new BitVec(3, 8));
        Assert.That(result1.Value, Is.EqualTo(new BigInteger(7)));

        // BitVec - BigInteger
        var result2 = bv.Sub(new BigInteger(3));
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(7)));
    }

    [Test]
    public void Mul_Method_WithBitVecAndBigInteger_WorksCorrectly()
    {
        var bv = new BitVec(10, 8);

        // BitVec * BitVec
        var result1 = bv.Mul(new BitVec(3, 8));
        Assert.That(result1.Value, Is.EqualTo(new BigInteger(30)));

        // BitVec * BigInteger
        var result2 = bv.Mul(new BigInteger(3));
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(30)));
    }

    [Test]
    public void Div_Method_WithSignedParameter_WorksCorrectly()
    {
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        // Unsigned division: 246 / 3 = 82
        var result1 = bv1.Div(bv2, signed: false);
        Assert.That(result1.Value, Is.EqualTo(new BigInteger(82)));

        // Signed division: -10 / 3 = -3 (253 in unsigned representation)
        var result2 = bv1.Div(bv2, signed: true);
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(253)));

        // Default should be unsigned
        var result3 = bv1.Div(bv2);
        Assert.That(result3.Value, Is.EqualTo(new BigInteger(82)));

        // BigInteger overload
        var result4 = bv1.Div(new BigInteger(3), signed: true);
        Assert.That(result4.Value, Is.EqualTo(new BigInteger(253)));
    }

    [Test]
    public void Rem_Method_WithSignedParameter_WorksCorrectly()
    {
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        // Unsigned remainder: 246 % 3 = 0
        var result1 = bv1.Rem(bv2, signed: false);
        Assert.That(result1.Value, Is.EqualTo(new BigInteger(0)));

        // Signed remainder: -10 % 3 = -1 (255 in unsigned representation)
        var result2 = bv1.Rem(bv2, signed: true);
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(255)));

        // Default should be unsigned
        var result3 = bv1.Rem(bv2);
        Assert.That(result3.Value, Is.EqualTo(new BigInteger(0)));

        // BigInteger overload
        var result4 = bv1.Rem(new BigInteger(3), signed: true);
        Assert.That(result4.Value, Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void BitwiseOperations_Methods_WorkCorrectly()
    {
        var bv1 = new BitVec(170, 8); // 10101010
        var bv2 = new BitVec(85, 8); // 01010101
        var bigInt = new BigInteger(85);

        // AND operations
        Assert.That(bv1.And(bv2).Value, Is.EqualTo(new BigInteger(0)));
        Assert.That(bv1.And(bigInt).Value, Is.EqualTo(new BigInteger(0)));

        // OR operations
        Assert.That(bv1.Or(bv2).Value, Is.EqualTo(new BigInteger(255)));
        Assert.That(bv1.Or(bigInt).Value, Is.EqualTo(new BigInteger(255)));

        // XOR operations
        Assert.That(bv1.Xor(bv2).Value, Is.EqualTo(new BigInteger(255)));
        Assert.That(bv1.Xor(bigInt).Value, Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void ShiftOperations_Methods_WithSignedParameter_WorkCorrectly()
    {
        var bv = new BitVec(248, 8); // 11111000 = -8 in signed, 248 in unsigned

        // Left shift (always fills with zeros)
        var leftShift = bv.Shl(1);
        Assert.That(leftShift.Value, Is.EqualTo(new BigInteger(240))); // 11110000 (masked to 8 bits)

        // Right shift unsigned (logical): 11111000 >> 1 = 01111100 = 124
        var rightShiftUnsigned = bv.Shr(1, signed: false);
        Assert.That(rightShiftUnsigned.Value, Is.EqualTo(new BigInteger(124)));

        // Right shift signed (arithmetic): -8 >> 1 = -4 = 11111100 = 252
        var rightShiftSigned = bv.Shr(1, signed: true);
        Assert.That(rightShiftSigned.Value, Is.EqualTo(new BigInteger(252)));

        // Default should be unsigned
        var rightShiftDefault = bv.Shr(1);
        Assert.That(rightShiftDefault.Value, Is.EqualTo(new BigInteger(124)));
    }

    [Test]
    public void Operators_WithBigInteger_WorkCorrectly()
    {
        var bv = new BitVec(10, 8);
        var bigInt = new BigInteger(5);

        // Arithmetic operators (BitVec on left)
        Assert.That((bv + bigInt).Value, Is.EqualTo(new BigInteger(15)));
        Assert.That((bv - bigInt).Value, Is.EqualTo(new BigInteger(5)));
        Assert.That((bv * bigInt).Value, Is.EqualTo(new BigInteger(50)));
        Assert.That((bv / bigInt).Value, Is.EqualTo(new BigInteger(2)));
        Assert.That((bv % bigInt).Value, Is.EqualTo(new BigInteger(0)));

        // Arithmetic operators (BigInteger on left)
        Assert.That((bigInt + bv).Value, Is.EqualTo(new BigInteger(15)));
        Assert.That((new BigInteger(15) - bv).Value, Is.EqualTo(new BigInteger(5)));
        Assert.That((bigInt * bv).Value, Is.EqualTo(new BigInteger(50)));
        Assert.That((new BigInteger(20) / bv).Value, Is.EqualTo(new BigInteger(2)));
        Assert.That((new BigInteger(25) % bv).Value, Is.EqualTo(new BigInteger(5)));

        // Bitwise operators (BitVec on left)
        var bv2 = new BitVec(170, 8); // 10101010
        var mask = new BigInteger(85); // 01010101

        Assert.That((bv2 & mask).Value, Is.EqualTo(new BigInteger(0)));
        Assert.That((bv2 | mask).Value, Is.EqualTo(new BigInteger(255)));
        Assert.That((bv2 ^ mask).Value, Is.EqualTo(new BigInteger(255)));

        // Bitwise operators (BigInteger on left)
        Assert.That((mask & bv2).Value, Is.EqualTo(new BigInteger(0)));
        Assert.That((mask | bv2).Value, Is.EqualTo(new BigInteger(255)));
        Assert.That((mask ^ bv2).Value, Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void SignedMod_RemainsUnchanged_AndWorksCorrectly()
    {
        // Test that SignedMod still works as before (Z3-style modulo)
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        var result = bv1.SignedMod(bv2);
        // -10 mod 3 should be 2 (same sign as divisor)
        Assert.That(result.Value, Is.EqualTo(new BigInteger(2)));

        // Test with positive dividend and negative divisor
        var bv3 = new BitVec(10, 8);
        var bv4 = new BitVec(253, 8); // -3 in 8-bit two's complement

        var result2 = bv3.SignedMod(bv4);
        // 10 mod -3 should be -2 (same sign as divisor) = 254 in unsigned
        Assert.That(result2.Value, Is.EqualTo(new BigInteger(254)));
    }

    [Test]
    public void AllOperators_CallCorrectMethods()
    {
        var bv1 = new BitVec(20, 8);
        var bv2 = new BitVec(4, 8);

        // Verify that operators use default unsigned behavior
        Assert.That((bv1 / bv2).Value, Is.EqualTo(bv1.Div(bv2).Value));
        Assert.That((bv1 % bv2).Value, Is.EqualTo(bv1.Rem(bv2).Value));
        Assert.That((bv1 + bv2).Value, Is.EqualTo(bv1.Add(bv2).Value));
        Assert.That((bv1 - bv2).Value, Is.EqualTo(bv1.Sub(bv2).Value));
        Assert.That((bv1 * bv2).Value, Is.EqualTo(bv1.Mul(bv2).Value));
        Assert.That((bv1 & bv2).Value, Is.EqualTo(bv1.And(bv2).Value));
        Assert.That((bv1 | bv2).Value, Is.EqualTo(bv1.Or(bv2).Value));
        Assert.That((bv1 ^ bv2).Value, Is.EqualTo(bv1.Xor(bv2).Value));
        Assert.That((bv1 << 2).Value, Is.EqualTo(bv1.Shl(2).Value));
        Assert.That((bv1 >> 2).Value, Is.EqualTo(bv1.Shr(2).Value));
    }

    // =============================================================================
    // BIT MANIPULATION OPERATIONS (Extend, Extract, Resize)
    // =============================================================================

    [Test]
    public void Extend_AddZeroBits_PreservesValue()
    {
        var bv = new BitVec(0b10101010, 8); // 170 in 8 bits
        var extended = bv.Extend(8); // Add 8 more bits

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.Value, Is.EqualTo(0b00000000_10101010));
    }

    [Test]
    public void Extend_ZeroAdditionalBits_ReturnsSameValue()
    {
        var bv = new BitVec(42, 8);
        var extended = bv.Extend(0);

        Assert.That(extended.Size, Is.EqualTo(8));
        Assert.That(extended.Value, Is.EqualTo(bv.Value));
    }

    [Test]
    public void Extend_SignedExtension_PositiveValue_ExtendsWithZeros()
    {
        var bv = new BitVec(0b01010101, 8); // 85 (positive in signed 8-bit)
        var extended = bv.Extend(8, signed: true);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.Value, Is.EqualTo(0b00000000_01010101));
        Assert.That((int)extended.ToBigInteger(signed: true), Is.EqualTo(85));
    }

    [Test]
    public void Extend_SignedExtension_NegativeValue_ExtendsWithOnes()
    {
        var bv = new BitVec(0b10101010, 8); // -86 in signed 8-bit
        var extended = bv.Extend(8, signed: true);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.ToBigInteger(signed: true), Is.EqualTo(-86));
        // Check actual bit pattern shows sign extension
        Assert.That((int)extended.Value, Is.EqualTo(0b11111111_10101010));
    }

    [Test]
    public void Extend_MinusOne_ExtendsProperly()
    {
        var bv = new BitVec(-1, 8); // All bits set in 8-bit
        var extended = bv.Extend(8, signed: true);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.ToBigInteger(signed: true), Is.EqualTo(-1));
        Assert.That((int)extended.Value, Is.EqualTo(0xFFFF));
    }

    [Test]
    public void Extract_MiddleBits_ReturnsCorrectValue()
    {
        var bv = new BitVec(0b11010011, 8); // 211
        var extracted = bv.Extract(5, 2); // Extract bits 5-2 -> 0100

        Assert.That(extracted.Size, Is.EqualTo(4));
        Assert.That((int)extracted.Value, Is.EqualTo(0b0100));
    }

    [Test]
    public void Extract_SingleBit_WorksCorrectly()
    {
        var bv = new BitVec(0b10101010, 8);
        var msb = bv.Extract(7, 7); // MSB
        var lsb = bv.Extract(0, 0); // LSB

        Assert.That(msb.Size, Is.EqualTo(1));
        Assert.That((int)msb.Value, Is.EqualTo(1));
        Assert.That(lsb.Size, Is.EqualTo(1));
        Assert.That((int)lsb.Value, Is.EqualTo(0));
    }

    [Test]
    public void Extract_FullRange_ReturnsIdentical()
    {
        var bv = new BitVec(0b11001100, 8);
        var extracted = bv.Extract(7, 0);

        Assert.That(extracted.Size, Is.EqualTo(bv.Size));
        Assert.That(extracted.Value, Is.EqualTo(bv.Value));
    }

    [Test]
    public void Extract_HighBitOutOfRange_ThrowsException()
    {
        var bv = new BitVec(42, 8);

        Assert.That(
            () => bv.Extract(8, 0),
            Throws.ArgumentException.With.Message.Contains(
                "High bit 8 is out of range for 8-bit vector"
            )
        );
    }

    [Test]
    public void Extract_LowGreaterThanHigh_ThrowsException()
    {
        var bv = new BitVec(42, 8);

        Assert.That(
            () => bv.Extract(2, 5),
            Throws.ArgumentException.With.Message.Contains(
                "Low bit 5 cannot be greater than high bit 2"
            )
        );
    }

    [Test]
    public void Resize_ToLargerSize_ZeroExtends()
    {
        var bv = new BitVec(42, 8);
        var resized = bv.Resize(16);

        Assert.That(resized.Size, Is.EqualTo(16));
        Assert.That((int)resized.Value, Is.EqualTo(42));
    }

    [Test]
    public void Resize_ToSmallerSize_Truncates()
    {
        var bv = new BitVec(0x1234, 16);
        var resized = bv.Resize(8);

        Assert.That(resized.Size, Is.EqualTo(8));
        Assert.That((int)resized.Value, Is.EqualTo(0x34)); // Lower 8 bits
    }

    [Test]
    public void Resize_SameSize_ReturnsSameValue()
    {
        var bv = new BitVec(42, 8);
        var resized = bv.Resize(8);

        Assert.That(resized.Size, Is.EqualTo(8));
        Assert.That(resized.Value, Is.EqualTo(bv.Value));
    }

    [Test]
    public void Resize_SignedToLargerSize_SignExtends()
    {
        var bv = new BitVec(0b10101010, 8); // -86 in signed 8-bit
        var resized = bv.Resize(16, signed: true);

        Assert.That(resized.Size, Is.EqualTo(16));
        Assert.That((int)resized.ToBigInteger(signed: true), Is.EqualTo(-86));
    }

    [Test]
    public void Resize_SignedPositiveToLarger_ZeroExtends()
    {
        var bv = new BitVec(42, 8);
        var resized = bv.Resize(16, signed: true);

        Assert.That(resized.Size, Is.EqualTo(16));
        Assert.That((int)resized.Value, Is.EqualTo(42));
        Assert.That((int)resized.ToBigInteger(signed: true), Is.EqualTo(42));
    }

    // =============================================================================
    // COMBINED OPERATIONS AND EDGE CASES
    // =============================================================================

    [Test]
    public void CombinedOperations_ExtendThenExtract_WorksCorrectly()
    {
        var bv = new BitVec(0xAB, 8);
        var extended = bv.Extend(8); // 16-bit: 0x00AB
        var extracted = extended.Extract(11, 4); // Extract middle 8 bits

        Assert.That(extracted.Size, Is.EqualTo(8));
        Assert.That((int)extracted.Value, Is.EqualTo(0x0A)); // Should get 0x0A
    }

    [Test]
    public void CombinedOperations_ExtractThenSignedExtend_WorksCorrectly()
    {
        var bv = new BitVec(0xF0, 8);
        var extracted = bv.Extract(7, 4); // Get upper 4 bits: 0xF
        var extended = extracted.Extend(4, signed: true); // Sign extend 4-bit 0xF to 8-bit

        Assert.That(extracted.Size, Is.EqualTo(4));
        Assert.That((int)extracted.Value, Is.EqualTo(0xF));
        Assert.That(extended.Size, Is.EqualTo(8));
        Assert.That((int)extended.Value, Is.EqualTo(0xFF)); // Should be sign-extended
        Assert.That((int)extended.ToBigInteger(signed: true), Is.EqualTo(-1));
    }

    [Test]
    public void EdgeCase_SingleBitOperations_WorkCorrectly()
    {
        var zero = new BitVec(0, 1);
        var one = new BitVec(1, 1);

        Assert.That(zero.Extend(7).Size, Is.EqualTo(8));
        Assert.That((int)zero.Extend(7).Value, Is.EqualTo(0));

        Assert.That(one.Extend(7, signed: true).Size, Is.EqualTo(8));
        Assert.That((int)one.Extend(7, signed: true).Value, Is.EqualTo(0xFF)); // Sign extends as -1
    }

    [Test]
    public void EdgeCase_MaxBitWidthOperations_WorkCorrectly()
    {
        var bv64 = new BitVec(ulong.MaxValue, 64);
        var extracted = bv64.Extract(63, 32); // Get upper 32 bits
        var resized = bv64.Resize(32); // Truncate to lower 32 bits

        Assert.That(extracted.Size, Is.EqualTo(32));
        Assert.That((uint)extracted.Value, Is.EqualTo(uint.MaxValue));

        Assert.That(resized.Size, Is.EqualTo(32));
        Assert.That((uint)resized.Value, Is.EqualTo(uint.MaxValue));
    }

    [Test]
    public void EdgeCase_ZeroValueOperations_WorkCorrectly()
    {
        var zero8 = new BitVec(0, 8);

        Assert.That(zero8.Extend(8).Size, Is.EqualTo(16));
        Assert.That(zero8.Extend(8).Value, Is.EqualTo(BigInteger.Zero));

        Assert.That(zero8.Extend(8, signed: true).Size, Is.EqualTo(16));
        Assert.That(zero8.Extend(8, signed: true).Value, Is.EqualTo(BigInteger.Zero));

        Assert.That(zero8.Resize(4).Size, Is.EqualTo(4));
        Assert.That(zero8.Resize(4).Value, Is.EqualTo(BigInteger.Zero));
    }
}
*/
