#pragma warning disable NUnit2021 // Comparison of value types is allowed here for BitVec tests

using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.DataTypes;

[TestFixture]
public class BitVecResizingTests
{
    // =============================================================================
    // EXTEND OPERATIONS (zero extension - add additional bits)
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
    public void Extend_SingleBit_PreservesData()
    {
        var bv = new BitVec(1, 1); // Single bit set
        var extended = bv.Extend(7);

        Assert.That(extended.Size, Is.EqualTo(8));
        Assert.That((int)extended.Value, Is.EqualTo(1));
    }

    [Test]
    public void Extend_AllBitsSet_PreservesPattern()
    {
        var bv = new BitVec(0xFF, 8); // All 8 bits set
        var extended = bv.Extend(8);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.Value, Is.EqualTo(0x00FF));
    }

    [Test]
    public void Extend_LargeBitVector_WorksCorrectly()
    {
        var bv = new BitVec(0xDEADBEEF, 32);
        var extended = bv.Extend(32);

        Assert.That(extended.Size, Is.EqualTo(64));
        Assert.That((uint)extended.Value, Is.EqualTo(0xDEADBEEF));
    }

    // =============================================================================
    // SIGNED EXTEND OPERATIONS (sign extension - add additional bits with sign)
    // =============================================================================

    [Test]
    public void SignedExtend_PositiveValue_ExtendsWithZeros()
    {
        var bv = new BitVec(0b01010101, 8); // 85 (positive in signed 8-bit)
        var extended = bv.SignedExtend(8);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.Value, Is.EqualTo(0b00000000_01010101));
        Assert.That((int)extended.ToSignedBigInteger(), Is.EqualTo(85));
    }

    [Test]
    public void SignedExtend_NegativeValue_ExtendsWithOnes()
    {
        var bv = new BitVec(0b10101010, 8); // -86 in signed 8-bit
        var extended = bv.SignedExtend(8);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.ToSignedBigInteger(), Is.EqualTo(-86));
        // Check actual bit pattern shows sign extension
        Assert.That((int)extended.Value, Is.EqualTo(0b11111111_10101010));
    }

    [Test]
    public void SignedExtend_ZeroAdditionalBits_ReturnsSameValue()
    {
        var bv = new BitVec(200, 8); // -56 in signed 8-bit
        var extended = bv.SignedExtend(0);

        Assert.That(extended.Size, Is.EqualTo(8));
        Assert.That(extended.Value, Is.EqualTo(bv.Value));
    }

    [Test]
    public void SignedExtend_MaxNegativeValue_WorksCorrectly()
    {
        var bv = new BitVec(0b10000000, 8); // -128 in 8-bit signed
        var extended = bv.SignedExtend(8);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.ToSignedBigInteger(), Is.EqualTo(-128));
    }

    [Test]
    public void SignedExtend_MinusOne_ExtendsProperly()
    {
        var bv = new BitVec(-1, 8); // All bits set in 8-bit
        var extended = bv.SignedExtend(8);

        Assert.That(extended.Size, Is.EqualTo(16));
        Assert.That((int)extended.ToSignedBigInteger(), Is.EqualTo(-1));
        Assert.That((int)extended.Value, Is.EqualTo(0xFFFF));
    }

    [Test]
    public void SignedExtend_SmallToLarge_MaintainsValue()
    {
        var bv = new BitVec(-1000, 16);
        var extended = bv.SignedExtend(48); // Extend to 64 bits

        Assert.That(extended.Size, Is.EqualTo(64));
        Assert.That((long)extended.ToSignedBigInteger(), Is.EqualTo(-1000));
    }

    // =============================================================================
    // EXTRACT OPERATIONS (bit slice extraction)
    // =============================================================================

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
    public void Extract_UpperByte_WorksCorrectly()
    {
        var bv = new BitVec(0xABCD, 16);
        var upper = bv.Extract(15, 8);

        Assert.That(upper.Size, Is.EqualTo(8));
        Assert.That((int)upper.Value, Is.EqualTo(0xAB));
    }

    [Test]
    public void Extract_LowerByte_WorksCorrectly()
    {
        var bv = new BitVec(0xABCD, 16);
        var lower = bv.Extract(7, 0);

        Assert.That(lower.Size, Is.EqualTo(8));
        Assert.That((int)lower.Value, Is.EqualTo(0xCD));
    }

    [Test]
    public void Extract_LargeValue_WorksCorrectly()
    {
        var bv = new BitVec(0x123456789ABCDEF0UL, 64);
        var middle = bv.Extract(31, 16); // Extract 16 bits from middle

        Assert.That(middle.Size, Is.EqualTo(16));
        // 0x123456789ABCDEF0 >> 16 & 0xFFFF = 0xDEF0 >> 16 & 0xFFFF = 0x9ABC
        Assert.That((int)middle.Value, Is.EqualTo(0x9ABC));
    }

    [Test]
    public void Extract_HighBitOutOfRange_ThrowsException()
    {
        var bv = new BitVec(42, 8);

        Assert.That(() => bv.Extract(8, 0),
            Throws.ArgumentException.With.Message.Contains("High bit 8 is out of range for 8-bit vector"));
    }

    [Test]
    public void Extract_LowGreaterThanHigh_ThrowsException()
    {
        var bv = new BitVec(42, 8);

        Assert.That(() => bv.Extract(2, 5),
            Throws.ArgumentException.With.Message.Contains("Low bit 5 cannot be greater than high bit 2"));
    }

    // =============================================================================
    // RESIZE OPERATIONS (zero-extend resize to specific size)
    // =============================================================================

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
    public void Resize_TruncateWithDataLoss_WorksCorrectly()
    {
        var bv = new BitVec(0xFFFF, 16); // All 16 bits set
        var resized = bv.Resize(4);

        Assert.That(resized.Size, Is.EqualTo(4));
        Assert.That((int)resized.Value, Is.EqualTo(0xF)); // Only lower 4 bits
    }

    [Test]
    public void Resize_ExpandFromSingleBit_WorksCorrectly()
    {
        var bv = new BitVec(1, 1);
        var resized = bv.Resize(32);

        Assert.That(resized.Size, Is.EqualTo(32));
        Assert.That((int)resized.Value, Is.EqualTo(1));
    }

    // =============================================================================
    // SIGNED RESIZE OPERATIONS (sign-aware resize to specific size)
    // =============================================================================

    [Test]
    public void SignedResize_ToLargerSize_SignExtends()
    {
        var bv = new BitVec(0b10101010, 8); // -86 in signed 8-bit
        var resized = bv.SignedResize(16);

        Assert.That(resized.Size, Is.EqualTo(16));
        Assert.That((int)resized.ToSignedBigInteger(), Is.EqualTo(-86));
    }

    [Test]
    public void SignedResize_PositiveToLarger_ZeroExtends()
    {
        var bv = new BitVec(42, 8);
        var resized = bv.SignedResize(16);

        Assert.That(resized.Size, Is.EqualTo(16));
        Assert.That((int)resized.Value, Is.EqualTo(42));
        Assert.That((int)resized.ToSignedBigInteger(), Is.EqualTo(42));
    }

    [Test]
    public void SignedResize_ToSmallerSize_Truncates()
    {
        var bv = new BitVec(0x1234, 16);
        var resized = bv.SignedResize(8);

        Assert.That(resized.Size, Is.EqualTo(8));
        Assert.That((int)resized.Value, Is.EqualTo(0x34));
    }

    [Test]
    public void SignedResize_SameSize_ReturnsSameValue()
    {
        var bv = new BitVec(42, 8);
        var resized = bv.SignedResize(8);

        Assert.That(resized.Size, Is.EqualTo(8));
        Assert.That(resized.Value, Is.EqualTo(bv.Value));
    }

    [Test]
    public void SignedResize_NegativeValueTruncation_WorksCorrectly()
    {
        var bv = new BitVec(-1000, 16); // Negative value
        var resized = bv.SignedResize(8);

        Assert.That(resized.Size, Is.EqualTo(8));
        // Should truncate to lower 8 bits
        var expected = (-1000) & 0xFF;
        Assert.That((int)resized.Value, Is.EqualTo(expected));
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
        Assert.That((int)bv.ToSignedBigInteger(), Is.EqualTo(-42));
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

    // =============================================================================
    // COMBINED OPERATIONS TESTS
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
    public void CombinedOperations_ResizeThenSignedResize_WorksCorrectly()
    {
        var bv = new BitVec(0x80, 8); // -128 in signed 8-bit
        var enlarged = bv.Resize(16); // Zero extend to 16-bit: 0x0080
        var signResized = new BitVec(0x8000, 16).SignedResize(8); // Should truncate

        Assert.That(enlarged.Size, Is.EqualTo(16));
        Assert.That((int)enlarged.Value, Is.EqualTo(0x0080));
        Assert.That(signResized.Size, Is.EqualTo(8));
        Assert.That((int)signResized.Value, Is.EqualTo(0x00));
    }

    [Test]
    public void CombinedOperations_ExtractThenSignedExtend_WorksCorrectly()
    {
        var bv = new BitVec(0xF0, 8);
        var extracted = bv.Extract(7, 4); // Get upper 4 bits: 0xF
        var extended = extracted.SignedExtend(4); // Sign extend 4-bit 0xF to 8-bit

        Assert.That(extracted.Size, Is.EqualTo(4));
        Assert.That((int)extracted.Value, Is.EqualTo(0xF));
        Assert.That(extended.Size, Is.EqualTo(8));
        Assert.That((int)extended.Value, Is.EqualTo(0xFF)); // Should be sign-extended
        Assert.That((int)extended.ToSignedBigInteger(), Is.EqualTo(-1));
    }

    // =============================================================================
    // BOUNDARY AND EDGE CASE TESTS
    // =============================================================================

    [Test]
    public void EdgeCase_SingleBitOperations_WorkCorrectly()
    {
        var zero = new BitVec(0, 1);
        var one = new BitVec(1, 1);

        Assert.That(zero.Extend(7).Size, Is.EqualTo(8));
        Assert.That((int)zero.Extend(7).Value, Is.EqualTo(0));

        Assert.That(one.SignedExtend(7).Size, Is.EqualTo(8));
        Assert.That((int)one.SignedExtend(7).Value, Is.EqualTo(0xFF)); // Sign extends as -1
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

        Assert.That(zero8.SignedExtend(8).Size, Is.EqualTo(16));
        Assert.That(zero8.SignedExtend(8).Value, Is.EqualTo(BigInteger.Zero));

        Assert.That(zero8.Resize(4).Size, Is.EqualTo(4));
        Assert.That(zero8.Resize(4).Value, Is.EqualTo(BigInteger.Zero));
    }
}