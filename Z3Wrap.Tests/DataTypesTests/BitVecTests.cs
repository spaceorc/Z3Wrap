using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.DataTypesTests;

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
        Assert.Throws<ArgumentException>(() => new BitVec(1, 0));
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
        Assert.That(bv.ToSignedInt(), Is.EqualTo(int.MinValue));
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
        Assert.That(bv.ToSignedLong(), Is.EqualTo(-1L));
    }

    [Test]
    public void AsInt_WithNegativeValue_ReturnsCorrectSignedValue()
    {
        // -10 in 8-bit two's complement is 246 in unsigned representation
        var bv = new BitVec(246, 8);
        Assert.That(bv.ToSignedInt(), Is.EqualTo(-10));
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

        Assert.Throws<ArgumentException>(() => { _ = bv1 + bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 - bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 * bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 / bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 % bv2; });
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
    public void SignedDiv_WithPositiveOperands_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(20, 8);
        var bv2 = new BitVec(4, 8);

        var result = bv1.SignedDiv(bv2);
        Assert.That(result.Value, Is.EqualTo(new BigInteger(5)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void SignedDiv_WithNegativeOperands_ReturnsCorrectResult()
    {
        // -10 / 3 in 8-bit two's complement
        // -10 = 246 in unsigned representation
        var bv1 = new BitVec(246, 8); // -10 in 8-bit two's complement
        var bv2 = new BitVec(3, 8);

        var result = bv1.SignedDiv(bv2);
        // -10 / 3 = -3, which is 253 in 8-bit unsigned representation
        Assert.That(result.Value, Is.EqualTo(new BigInteger(253)));
        Assert.That(result.Size, Is.EqualTo(8));
    }

    [Test]
    public void SignedRem_WithValidOperands_ReturnsCorrectResult()
    {
        var bv1 = new BitVec(23, 8);
        var bv2 = new BitVec(5, 8);

        var result = bv1.SignedRem(bv2);
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

        Assert.Throws<DivideByZeroException>(() => { _ = bv1 / bv2; });
        Assert.Throws<DivideByZeroException>(() => { _ = bv1 % bv2; });
        Assert.Throws<DivideByZeroException>(() => bv1.SignedDiv(bv2));
        Assert.Throws<DivideByZeroException>(() => bv1.SignedRem(bv2));
        Assert.Throws<DivideByZeroException>(() => bv1.SignedMod(bv2));
    }

    [Test]
    public void DivisionWithDifferentSizes_ThrowsException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(5, 16);

        Assert.Throws<ArgumentException>(() => { _ = bv1 / bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 % bv2; });
        Assert.Throws<ArgumentException>(() => bv1.SignedDiv(bv2));
        Assert.Throws<ArgumentException>(() => bv1.SignedRem(bv2));
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
        var bv2 = new BitVec(85, 8);  // 01010101

        Assert.That((bv1 & bv2).Value, Is.EqualTo(new BigInteger(0)));     // 00000000
        Assert.That((bv1 | bv2).Value, Is.EqualTo(new BigInteger(255)));   // 11111111
        Assert.That((bv1 ^ bv2).Value, Is.EqualTo(new BigInteger(255)));   // 11111111
        Assert.That((~bv1).Value, Is.EqualTo(new BigInteger(85)));         // 01010101
    }

    [Test]
    public void ShiftOperators_WorkCorrectly()
    {
        var bv = new BitVec(5, 8); // 00000101

        var leftShift = bv << 2;   // 00010100 = 20
        Assert.That(leftShift.Value, Is.EqualTo(new BigInteger(20)));

        var rightShift = bv >> 1;  // 00000010 = 2
        Assert.That(rightShift.Value, Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void ShiftOperators_WithNegativeShift_ThrowException()
    {
        var bv = new BitVec(5, 8);

        Assert.Throws<ArgumentException>(() => { _ = bv << -1; });
        Assert.Throws<ArgumentException>(() => { _ = bv >> -1; });
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

        Assert.Throws<ArgumentException>(() => { _ = bv1 < bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 <= bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 > bv2; });
        Assert.Throws<ArgumentException>(() => { _ = bv1 >= bv2; });
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

        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(20, 8);

        Assert.That(BitVec.Min(bv1, bv2), Is.EqualTo(bv1));
        Assert.That(BitVec.Max(bv1, bv2), Is.EqualTo(bv2));
    }

    [Test]
    public void StaticMethods_WithDifferentSizes_ThrowException()
    {
        var bv1 = new BitVec(10, 8);
        var bv2 = new BitVec(20, 16);

        Assert.Throws<ArgumentException>(() => BitVec.Min(bv1, bv2));
        Assert.Throws<ArgumentException>(() => BitVec.Max(bv1, bv2));
    }

    [Test]
    public void AsSignedInt_SignedConversion_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToSignedInt(), Is.EqualTo(42));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToSignedInt(), Is.EqualTo(-128));

        // Test another negative - 8-bit: 11111111 = 255 unsigned, -1 signed
        var minusOne = new BitVec(255, 8);
        Assert.That(minusOne.ToSignedInt(), Is.EqualTo(-1));

        // Test 16-bit negative value: 32768 = 0x8000 = -32768 signed
        var negative16 = new BitVec(32768, 16);
        Assert.That(negative16.ToSignedInt(), Is.EqualTo(-32768));
    }

    [Test]
    public void AsSignedLong_SignedConversion_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToSignedLong(), Is.EqualTo(42L));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToSignedLong(), Is.EqualTo(-128L));

        // Test 32-bit negative value
        var negative32 = new BitVec(2147483648U, 32); // 0x80000000 = -2147483648 signed
        Assert.That(negative32.ToSignedLong(), Is.EqualTo(-2147483648L));

        // Test 64-bit negative value
        var maxUnsigned64 = (BigInteger.One << 63); // MSB set for 64-bit
        var negative64 = new BitVec(maxUnsigned64, 64);
        Assert.That(negative64.ToSignedLong(), Is.EqualTo(long.MinValue));
    }

    [Test]
    public void AsSignedBigInteger_ReturnsCorrectSignedValues()
    {
        // Test positive value
        var positive = new BitVec(42, 8);
        Assert.That(positive.ToSignedBigInteger(), Is.EqualTo(new BigInteger(42)));

        // Test negative value (MSB set) - 8-bit: 10000000 = 128 unsigned, -128 signed
        var negative = new BitVec(128, 8);
        Assert.That(negative.ToSignedBigInteger(), Is.EqualTo(new BigInteger(-128)));

        // Test another negative - 8-bit: 11111111 = 255 unsigned, -1 signed
        var minusOne = new BitVec(255, 8);
        Assert.That(minusOne.ToSignedBigInteger(), Is.EqualTo(new BigInteger(-1)));

        // Test larger bit width - 16-bit: 65535 = -1 signed
        var minusOne16 = new BitVec(65535, 16);
        Assert.That(minusOne16.ToSignedBigInteger(), Is.EqualTo(new BigInteger(-1)));
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
        Assert.That(large.ToBinaryString(), Is.EqualTo("0000000000000000000000000000000000000000000000000000000000000001"));

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

        var ex = Assert.Throws<ArgumentException>(() => { _ = bv1 + bv2; });
        Assert.That(ex.Message, Does.Contain("Size mismatch"));

        // Test division by zero messages
        var zero = new BitVec(0, 8);
        var nonZero = new BitVec(10, 8);

        var divEx = Assert.Throws<DivideByZeroException>(() => { _ = nonZero / zero; });
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
        Assert.That(ex.Message, Does.Contain($"Unsigned value {largeValue} is outside the range of int"));
    }

    [Test]
    public void ToInt_WithMaxIntValue_DoesNotThrow()
    {
        // Boundary test: int.MaxValue should work
        var bv = new BitVec(int.MaxValue, 32);
        Assert.That(bv.ToInt(), Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void ToSignedInt_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create 33-bit value that when interpreted as signed exceeds int.MaxValue
        // Use a value that when sign-extended would be > int.MaxValue
        var largePositive = new BigInteger(int.MaxValue) + 1;
        var bv = new BitVec(largePositive, 64);

        var ex = Assert.Throws<OverflowException>(() => bv.ToSignedInt());
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of int"));
    }

    [Test]
    public void ToSignedInt_WithValueTooSmall_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is < int.MinValue
        // For this we need more than 32 bits with MSB set to create a very negative number
        var bv = new BitVec(BigInteger.Pow(2, 33) - 1, 34); // 34-bit value with all bits set
        // This represents -1 in 34-bit two's complement, but that's fine for int
        // Let's create something that's actually too small
        var veryLarge = BigInteger.Pow(2, 62); // This will be a very negative number when sign-extended from 63 bits
        var bv2 = new BitVec(veryLarge, 63);

        var ex = Assert.Throws<OverflowException>(() => bv2.ToSignedInt());
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
        Assert.That(ex.Message, Does.Contain($"Unsigned value {largeValue} is outside the range of uint"));
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
        Assert.That(ex.Message, Does.Contain($"Unsigned value {largeValue} is outside the range of long"));
    }

    [Test]
    public void ToLong_WithMaxLongValue_DoesNotThrow()
    {
        // Boundary test: long.MaxValue should work
        var bv = new BitVec(long.MaxValue, 64);
        Assert.That(bv.ToLong(), Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void ToSignedLong_WithValueTooLarge_ThrowsOverflowException()
    {
        // Create value that when interpreted as signed exceeds long.MaxValue
        var largePositive = new BigInteger(long.MaxValue) + 1;
        var bv = new BitVec(largePositive, 128);

        var ex = Assert.Throws<OverflowException>(() => bv.ToSignedLong());
        Assert.That(ex.Message, Does.Contain("Signed value"));
        Assert.That(ex.Message, Does.Contain("is outside the range of long"));
    }

    [Test]
    public void ToSignedLong_WithValueTooSmall_ThrowsOverflowException()
    {
        // Create a value that when interpreted as signed is < long.MinValue
        // Use a 65-bit value with MSB set to create a number smaller than long.MinValue
        var veryLarge = BigInteger.Pow(2, 126); // This will be very negative in 127-bit two's complement
        var bv = new BitVec(veryLarge, 127);

        var ex = Assert.Throws<OverflowException>(() => bv.ToSignedLong());
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
        Assert.That(ex.Message, Does.Contain($"Unsigned value {largeValue} is outside the range of ulong"));
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
        Assert.That(signedIntMax.ToSignedInt(), Is.EqualTo(int.MaxValue));

        var signedIntMin = new BitVec(BigInteger.Pow(2, 31), 32); // int.MinValue in two's complement
        Assert.That(signedIntMin.ToSignedInt(), Is.EqualTo(int.MinValue));

        var signedLongMax = new BitVec(long.MaxValue, 64);
        Assert.That(signedLongMax.ToSignedLong(), Is.EqualTo(long.MaxValue));

        var signedLongMin = new BitVec(BigInteger.Pow(2, 63), 64); // long.MinValue in two's complement
        Assert.That(signedLongMin.ToSignedLong(), Is.EqualTo(long.MinValue));
    }
}