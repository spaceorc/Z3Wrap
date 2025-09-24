using System.Numerics;
using Spaceorc.Z3Wrap.BitVectors;

namespace Z3Wrap.Tests.Unit.BitVectors;

/// <summary>
/// Test file showcasing the desired beautiful syntax for strongly-typed BitVec operations.
/// This demonstrates the API we want to achieve - some syntax may not compile yet.
/// </summary>
[TestFixture]
public class BitVecSyntaxTests
{
    [Test]
    public void BasicCreation_ShouldHaveCleanSyntax()
    {
        // ✨ Beautiful creation syntax - no size parameters!
        var bv8 = new BitVec<Size8>(255);
        var bv16 = new BitVec<Size16>(0x1234);
        var bv32 = new BitVec<Size32>(0x12345678);
        var bv64 = new BitVec<Size64>(0x123456789ABCDEF0);

        // ✨ Static factory properties
        var zero8 = BitVec<Size8>.Zero;
        var one32 = BitVec<Size32>.One;
        var max64 = BitVec<Size64>.Max;

        Assert.That(bv8.Value, Is.EqualTo(new BigInteger(255)));
        Assert.That(zero8.IsZero, Is.True);
        Assert.That(one32.Value, Is.EqualTo(new BigInteger(1)));
    }

    [Test]
    public void ArithmeticOperations_ShouldBeTypeSafe()
    {
        var a = new BitVec<Size32>(100);
        var b = new BitVec<Size32>(50);

        // ✨ Natural arithmetic operators - all return same type
        BitVec<Size32> sum = a + b;
        BitVec<Size32> diff = a - b;
        BitVec<Size32> product = a * b;
        BitVec<Size32> quotient = a / b;
        BitVec<Size32> remainder = a % b;
        BitVec<Size32> negated = -a;

        // ✨ Method versions for when you need options
        var signedDiv = a.Div(b, signed: true);
        var signedRem = a.Rem(b, signed: true);
        var signedMod = a.SignedMod(b);

        Assert.That(sum.Value, Is.EqualTo(new BigInteger(150)));
        Assert.That(diff.Value, Is.EqualTo(new BigInteger(50)));
        Assert.That(product.Value, Is.EqualTo(new BigInteger(5000)));
    }

    [Test]
    public void BitwiseOperations_ShouldBeNatural()
    {
        var a = new BitVec<Size16>(0b1111000011110000);
        var b = new BitVec<Size16>(0b1010101010101010);

        // ✨ Natural bitwise operators
        BitVec<Size16> and = a & b;
        BitVec<Size16> or = a | b;
        BitVec<Size16> xor = a ^ b;
        BitVec<Size16> not = ~a;

        // ✨ Method versions
        var andMethod = a.And(b);
        var orMethod = a.Or(b);
        var xorMethod = a.Xor(b);

        Assert.That(and.Value, Is.EqualTo(new BigInteger(0b1010000010100000)));
        Assert.That(or.Value, Is.EqualTo(new BigInteger(0b1111101011111010)));
    }

    [Test]
    public void ShiftOperations_ShouldBeClean()
    {
        var bv = new BitVec<Size32>(0x12345678);

        // ✨ Natural shift operators
        BitVec<Size32> leftShifted = bv << 4;
        BitVec<Size32> rightShifted = bv >> 4;

        // ✨ Method versions for signed shifts
        var logicalRight = bv.Shr(4, signed: false);
        var arithmeticRight = bv.Shr(4, signed: true);
        var leftShift = bv.Shl(4);

        Assert.That(leftShifted.Value, Is.EqualTo(new BigInteger(0x23456780)));
        Assert.That(rightShifted.Value, Is.EqualTo(new BigInteger(0x01234567)));
    }

    [Test]
    public void Comparisons_ShouldWorkNaturally()
    {
        var a = new BitVec<Size32>(100);
        var b = new BitVec<Size32>(50);
        var c = new BitVec<Size32>(100);

        // ✨ Natural comparison operators - return bool
        bool equal = a == c;
        bool notEqual = a != b;
        bool less = b < a;
        bool lessEqual = b <= a;
        bool greater = a > b;
        bool greaterEqual = a >= b;

        // ✨ Method versions for signed comparisons
        bool signedLess = b.Lt(a, signed: true);
        bool signedGreater = a.Gt(b, signed: true);

        // ✨ Comparisons with integer literals - should work via implicit conversion
        bool equalInt = a == 100; // BitVec == int
        bool equalLong = a == 100L; // BitVec == long
        bool equalUInt = a == 100u; // BitVec == uint
        bool equalBig = a == new BigInteger(100); // BitVec == BigInteger

        bool intEqual = 100 == a; // int == BitVec
        bool longEqual = 100L == a; // long == BitVec
        bool uintEqual = 100u == a; // uint == BitVec
        bool bigEqual = new BigInteger(100) == a; // BigInteger == BitVec

        bool notEqualInt = b != 100; // BitVec != int
        bool lessInt = b < 100; // BitVec < int
        bool greaterInt = a > 50; // BitVec > int

        bool intLess = 50 < a; // int < BitVec
        bool intGreater = 150 > a; // int > BitVec

        Assert.That(equal, Is.True);
        Assert.That(notEqual, Is.True);
        Assert.That(less, Is.True);
        Assert.That(greater, Is.True);
        Assert.That(equalInt, Is.True);
        Assert.That(equalLong, Is.True);
        Assert.That(equalUInt, Is.True);
        Assert.That(equalBig, Is.True);
        Assert.That(intEqual, Is.True);
        Assert.That(longEqual, Is.True);
        Assert.That(uintEqual, Is.True);
        Assert.That(bigEqual, Is.True);
        Assert.That(notEqualInt, Is.True);
        Assert.That(lessInt, Is.True);
        Assert.That(greaterInt, Is.True);
        Assert.That(intLess, Is.True);
        Assert.That(intGreater, Is.True);
    }

    [Test]
    public void BigIntegerInterop_ShouldBeSeamless()
    {
        var bv32 = new BitVec<Size32>(100);
        BigInteger big = 200;

        // ✨ Seamless BigInteger interop through implicit conversions
        BitVec<Size32> sum = bv32 + big; // Should work automatically
        BitVec<Size32> product = big * bv32; // Should work automatically
        bool equal = bv32 == big; // Should work automatically
        bool less = bv32 < big; // Should work automatically

        // ✨ Implicit conversion FROM BigInteger, explicit TO BigInteger
        BigInteger extracted = bv32.ToBigInteger(); // BitVec -> BigInteger (explicit)
        BitVec<Size32> converted = big; // BigInteger -> BitVec (implicit)

        Assert.That(sum.Value, Is.EqualTo(new BigInteger(300)));
        Assert.That(extracted, Is.EqualTo(new BigInteger(100)));
    }

    [Test]
    public void IntegerInterop_ShouldBeNatural()
    {
        var bv32 = new BitVec<Size32>(100);

        // ✨ Natural integer operations (through BigInteger conversion)
        var sumInt = bv32 + 50; // Should work via BigInteger
        var sumLong = bv32 + 50L; // Should work via BigInteger
        var sumUInt = bv32 + 50u; // Should work via BigInteger

        Assert.That(sumInt.Value, Is.EqualTo(new BigInteger(150)));
        Assert.That(sumLong.Value, Is.EqualTo(new BigInteger(150)));
        Assert.That(sumUInt.Value, Is.EqualTo(new BigInteger(150)));
    }

    [Test]
    public void Conversions_ShouldBeTypeSafe()
    {
        var bv32 = new BitVec<Size32>(0x12345678);

        // ✨ Clean conversion methods - no size parameters!
        int asInt = bv32.ToInt();
        uint asUInt = bv32.ToUInt();
        long asLong = bv32.ToLong();
        ulong asULong = bv32.ToULong();
        BigInteger asBigInt = bv32.ToBigInteger();

        // ✨ Signed conversions
        int signedInt = bv32.ToInt(signed: true);
        long signedLong = bv32.ToLong(signed: true);

        Assert.That(asUInt, Is.EqualTo(0x12345678u));
        Assert.That(asLong, Is.EqualTo(0x12345678L));
    }

    [Test]
    public void Formatting_ShouldBePretty()
    {
        var bv32 = new BitVec<Size32>(0x12345678);

        // ✨ Beautiful formatting with type-safe sizes
        string valueOnly = bv32.ToString(); // "305419896"
        string withSize = bv32.ToString("D"); // "305419896 (32-bit)"
        string hex = bv32.ToString("X"); // "0x12345678 (32-bit)"
        string binary = bv32.ToString("B"); // "0b00010010001101000101011001111000 (32-bit)"

        Assert.That(hex, Does.Contain("12345678"));
        Assert.That(withSize, Does.Contain("32-bit"));
    }

    [Test]
    public void CompileTimeSafety_ShouldPreventSizeMismatches()
    {
        var bv8 = new BitVec<Size8>(255);
        var bv16 = new BitVec<Size16>(0x1234);
        var bv32 = new BitVec<Size32>(0x12345678);

        // ✨ These should work - same sizes
        var sum8 = bv8 + new BitVec<Size8>(1);
        var sum16 = bv16 + new BitVec<Size16>(1);
        var sum32 = bv32 + new BitVec<Size32>(1);

        // ❌ These should NOT compile - different sizes
        // var invalid1 = bv8 + bv16;     // Size8 + Size16 - compile error!
        // var invalid2 = bv16 + bv32;    // Size16 + Size32 - compile error!
        // var invalid3 = bv8 == bv32;    // Size8 == Size32 - compile error!

        Assert.That(sum8.Value, Is.EqualTo(new BigInteger(0))); // Wraps around for 8-bit
        Assert.That(sum16.Value, Is.EqualTo(new BigInteger(0x1235)));
        Assert.That(sum32.Value, Is.EqualTo(new BigInteger(0x12345679)));
    }

    [Test]
    public void TypeSafeBitManipulation_DesiredSyntax()
    {
        var bv32 = new BitVec<Size32>(0x12345678);

        // 🚧 DESIRED: Type-safe bit manipulation (not implemented yet)
        // var bv64 = bv32.Extend<Size64>();                    // Zero extend to 64-bit
        // var bv64Signed = bv32.Extend<Size64>(signed: true);  // Sign extend to 64-bit
        // var bv16 = bv32.Resize<Size16>();                    // Truncate to 16-bit
        // var bv8 = bv32.Extract<High15, Low8, Size8>();       // Extract bits [15:8] as Size8

        // For now, we'll test the existing methods
        Assert.That(bv32.Value, Is.EqualTo(new BigInteger(0x12345678u)));
    }

    [Test]
    public void TypeSafeResize_ShouldWorkWithCompileTimeSizes()
    {
        // ✨ Type-safe resizing - compile-time size validation
        var bv32 = new BitVec<Size32>(0x12345678);
        var bv64 = new BitVec<Size64>(0x8000000012345678); // High bit set for sign extension test

        // Unsigned resize (truncate) - keeps lower bits
        BitVec<Size16> resized16 = bv32.Resize<Size16>(signed: false);
        BitVec<Size8> resized8 = bv32.Resize<Size8>(signed: false);

        // Signed resize (truncate with sign consideration)
        BitVec<Size16> signedResize16 = bv64.Resize<Size16>(signed: true);
        BitVec<Size8> signedResize8 = bv64.Resize<Size8>(signed: true);

        // Resize to larger size (extend)
        BitVec<Size64> extended64 = bv32.Resize<Size64>(signed: false); // Zero extend
        BitVec<Size64> signedExtended64 = bv32.Resize<Size64>(signed: true); // Sign extend

        // Resize to same size (identity operation)
        BitVec<Size32> identity32 = bv32.Resize<Size32>(signed: false);

        // Expected results:
        // 0x12345678 -> 0x5678 (16-bit truncate)
        Assert.That(resized16.Value, Is.EqualTo(new BigInteger(0x5678)));

        // 0x12345678 -> 0x78 (8-bit truncate)
        Assert.That(resized8.Value, Is.EqualTo(new BigInteger(0x78)));

        // 0x8000000012345678 -> 0x5678 (16-bit signed truncate)
        Assert.That(signedResize16.Value, Is.EqualTo(new BigInteger(0x5678)));

        // 0x8000000012345678 -> 0x78 (8-bit signed truncate)
        Assert.That(signedResize8.Value, Is.EqualTo(new BigInteger(0x78)));

        // 0x12345678 -> 0x0000000012345678 (64-bit zero extend)
        Assert.That(extended64.Value, Is.EqualTo(new BigInteger(0x12345678)));

        // 0x12345678 -> 0x0000000012345678 (64-bit sign extend - positive number)
        Assert.That(signedExtended64.Value, Is.EqualTo(new BigInteger(0x12345678)));

        // Identity resize
        Assert.That(identity32.Value, Is.EqualTo(new BigInteger(0x12345678)));

        // Test negative number sign extension
        var negativeBv32 = new BitVec<Size32>(0x80000000); // -2147483648 in 32-bit signed
        BitVec<Size64> negativeExtended = negativeBv32.Resize<Size64>(signed: true);

        // Should sign-extend to 0xFFFFFFFF80000000 (all high bits set)
        Assert.That(negativeExtended.Value, Is.EqualTo(new BigInteger(0xFFFFFFFF80000000UL)));
    }

    [Test]
    public void TypeSafeBitExtraction_ShouldWorkWithStartBit()
    {
        var bv32 = new BitVec<Size32>(0x12345678); // 0001 0010 0011 0100 0101 0110 0111 1000

        // ✨ Type-safe bit extraction - specify start bit, size from type
        BitVec<Size8> byte0 = bv32.Extract<Size8>(0); // Extract 8 bits starting from bit 0 → bits [7:0]
        BitVec<Size8> byte1 = bv32.Extract<Size8>(8); // Extract 8 bits starting from bit 8 → bits [15:8]
        BitVec<Size8> byte2 = bv32.Extract<Size8>(16); // Extract 8 bits starting from bit 16 → bits [23:16]
        BitVec<Size8> byte3 = bv32.Extract<Size8>(24); // Extract 8 bits starting from bit 24 → bits [31:24]

        BitVec<Size16> word0 = bv32.Extract<Size16>(0); // Extract 16 bits starting from bit 0 → bits [15:0]
        BitVec<Size16> word1 = bv32.Extract<Size16>(16); // Extract 16 bits starting from bit 16 → bits [31:16]

        BitVec<Size32> full = bv32.Extract<Size32>(0); // Extract all 32 bits → identity

        // Expected results for 0x12345678:
        // bits [7:0]   = 0x78 = 120
        Assert.That(byte0.Value, Is.EqualTo(new BigInteger(0x78)));

        // bits [15:8]  = 0x56 = 86
        Assert.That(byte1.Value, Is.EqualTo(new BigInteger(0x56)));

        // bits [23:16] = 0x34 = 52
        Assert.That(byte2.Value, Is.EqualTo(new BigInteger(0x34)));

        // bits [31:24] = 0x12 = 18
        Assert.That(byte3.Value, Is.EqualTo(new BigInteger(0x12)));

        // bits [15:0]  = 0x5678 = 22136
        Assert.That(word0.Value, Is.EqualTo(new BigInteger(0x5678)));

        // bits [31:16] = 0x1234 = 4660
        Assert.That(word1.Value, Is.EqualTo(new BigInteger(0x1234)));

        // bits [31:0]  = 0x12345678 = 305419896
        Assert.That(full.Value, Is.EqualTo(new BigInteger(0x12345678)));

        // Test partial extractions using standard sizes
        BitVec<Size8> halfbyte0 = bv32.Extract<Size8>(0); // bits [7:0] = 0x78
        BitVec<Size8> halfbyte1 = bv32.Extract<Size8>(8); // bits [15:8] = 0x56
        BitVec<Size16> sixteenbits = bv32.Extract<Size16>(8); // bits [23:8] = 0x3456

        Assert.That(halfbyte0.Value, Is.EqualTo(new BigInteger(0x78)));
        Assert.That(halfbyte1.Value, Is.EqualTo(new BigInteger(0x56)));
        Assert.That(sixteenbits.Value, Is.EqualTo(new BigInteger(0x3456)));
    }

    [Test]
    public void BitExtraction_ShouldThrowOnOutOfBounds()
    {
        var bv32 = new BitVec<Size32>(0x12345678);

        // These should throw - trying to extract beyond available bits
        Assert.Throws<ArgumentException>(() => bv32.Extract<Size8>(32)); // Start at bit 32, need bits [39:32]
        Assert.Throws<ArgumentException>(() => bv32.Extract<Size16>(24)); // Start at bit 24, need bits [39:24]
        Assert.Throws<ArgumentException>(() => bv32.Extract<Size32>(1)); // Start at bit 1, need bits [32:1]

        // These should work - within bounds
        var valid1 = bv32.Extract<Size8>(24); // bits [31:24] - exactly at boundary
        var valid2 = bv32.Extract<Size8>(0);  // bits [7:0] - full byte

        Assert.That(valid1.Value, Is.EqualTo(new BigInteger(0x12)));
        Assert.That(valid2.Value, Is.EqualTo(new BigInteger(0x78))); // bits [7:0] of 0x12345678
    }

    [Test]
    public void StaticProperties_ShouldBeConvenient()
    {
        // ✨ Convenient static properties - no size parameters!
        var zero8 = BitVec<Size8>.Zero;
        var zero32 = BitVec<Size32>.Zero;
        var zero64 = BitVec<Size64>.Zero;

        var one8 = BitVec<Size8>.One;
        var one32 = BitVec<Size32>.One;
        var one64 = BitVec<Size64>.One;

        var max8 = BitVec<Size8>.Max; // 0xFF
        var max32 = BitVec<Size32>.Max; // 0xFFFFFFFF
        var max64 = BitVec<Size64>.Max; // 0xFFFFFFFFFFFFFFFF

        Assert.That(zero8.IsZero, Is.True);
        Assert.That(one8.Value, Is.EqualTo(new BigInteger(1)));
        Assert.That(max8.Value, Is.EqualTo(new BigInteger(255)));
        Assert.That(max32.Value, Is.EqualTo(new BigInteger(0xFFFFFFFFu)));
    }

    [Test]
    public void Z3Integration_DesiredSyntax()
    {
        // 🚧 DESIRED: Beautiful Z3 integration (not implemented yet)
        // using var context = new Z3Context();
        // using var scope = context.SetUp();

        // // ✨ Type-safe Z3 BitVec creation
        // var x = context.BitVecConst<Size32>("x");
        // var y = context.BitVec<Size32>(42);

        // // ✨ All operations maintain exact types
        // Z3BitVec<Size32> sum = x + y;
        // Z3BitVec<Size32> product = x * y;
        // Z3BoolExpr condition = x > y;

        // // ❌ Size mismatches caught at compile time
        // // var z = context.BitVecConst<Size64>("z");
        // // var invalid = x + z;  // Won't compile!

        // Placeholder test
        Assert.That(true, Is.True);
    }
}
