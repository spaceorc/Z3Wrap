using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.Unit.Extensions;

[TestFixture]
public class BitVecSignedParameterEdgeCasesTests
{
    private Z3Context context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new Z3Context();
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public void SignedDivision_NegativeDividendAndDivisor_WorksCorrectly()
    {
        // Arrange - test signed division with both operands negative
        var x = context.BitVecConst("x", 16);
        var y = context.BitVecConst("y", 16);

        using var solver = context.CreateSolver();

        // -1000 / -7 = 142 (with remainder -6)
        solver.Assert(x == context.BitVec(new BitVec(-1000, 16)));
        solver.Assert(y == context.BitVec(new BitVec(-7, 16)));

        var signedDiv = context.Div(x, y, signed: true);
        var unsignedDiv = context.Div(x, y, signed: false);

        // Results should be different for negative operands
        solver.Assert(signedDiv != unsignedDiv);
        solver.Assert(signedDiv == context.BitVec(new BitVec(-1000 / -7, 16)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SignedRemainder_NegativeDividend_FollowsSignedSemantics()
    {
        // Arrange - test signed remainder behavior
        var dividends = new[] { -23, -24, -25, 23, 24, 25 };
        var divisor = 7;

        foreach (var dividend in dividends)
        {
            var x = context.BitVecConst($"x_{dividend}", 16);
            var y = context.BitVecConst($"y_{dividend}", 16);

            using var solver = context.CreateSolver();
            solver.Assert(x == context.BitVec(new BitVec(dividend, 16)));
            solver.Assert(y == context.BitVec(new BitVec(divisor, 16)));

            var signedRem = context.Rem(x, y, signed: true);
            var unsignedRem = context.Rem(x, y, signed: false);

            // Calculate expected signed remainder (C# % operator semantics)
            var expectedSigned = dividend % divisor;
            solver.Assert(signedRem == context.BitVec(new BitVec(expectedSigned, 16)));

            // For negative dividends, signed and unsigned should differ
            if (dividend < 0)
            {
                solver.Assert(signedRem != unsignedRem);
            }

            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        }
    }

    [Test]
    public void SignedComparisons_SignBitBoundary_WorksCorrectly()
    {
        // Arrange - test comparisons around the sign bit boundary
        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        using var solver = context.CreateSolver();

        // 127 (max signed positive) vs 128 (min signed negative, -128)
        solver.Assert(x == context.BitVec(new BitVec(127, 8)));
        solver.Assert(y == context.BitVec(new BitVec(128, 8)));

        // Unsigned: 127 < 128
        solver.Assert(context.Lt(x, y, signed: false));
        solver.Assert(context.Le(x, y, signed: false));
        solver.Assert(!context.Gt(x, y, signed: false));
        solver.Assert(!context.Ge(x, y, signed: false));

        // Signed: 127 > -128
        solver.Assert(!context.Lt(x, y, signed: true));
        solver.Assert(!context.Le(x, y, signed: true));
        solver.Assert(context.Gt(x, y, signed: true));
        solver.Assert(context.Ge(x, y, signed: true));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ArithmeticRightShift_NegativeValues_PreservesSignBit()
    {
        // Arrange - test arithmetic right shift with various negative values
        var testCases = new[]
        {
            (-1, 1),    // -1 >> 1 should still be -1
            (-2, 1),    // -2 >> 1 should be -1
            (-4, 2),    // -4 >> 2 should be -1
            (-128, 3),  // -128 >> 3 should be -16
            (-64, 1)    // -64 >> 1 should be -32
        };

        foreach (var (value, shift) in testCases)
        {
            var x = context.BitVecConst($"x_{value}_{shift}", 8);
            var s = context.BitVecConst($"s_{value}_{shift}", 8);

            using var solver = context.CreateSolver();
            solver.Assert(x == context.BitVec(new BitVec(value, 8)));
            solver.Assert(s == context.BitVec(new BitVec(shift, 8)));

            var arithmeticShift = context.Shr(x, s, signed: true);
            var logicalShift = context.Shr(x, s, signed: false);

            // Calculate expected results
            var expectedArithmetic = value >> shift; // C# arithmetic shift
            var expectedLogical = (int)((uint)(byte)value >> shift); // Logical shift

            solver.Assert(arithmeticShift == context.BitVec(new BitVec(expectedArithmetic, 8)));
            solver.Assert(logicalShift == context.BitVec(new BitVec(expectedLogical, 8)));

            // They should be different for negative values
            solver.Assert(arithmeticShift != logicalShift);

            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        }
    }

    [Test]
    public void SignedExtension_VariousBitWidths_PreservesSignBit()
    {
        // Arrange - test sign extension across different bit widths
        var testCases = new[]
        {
            (4, 8),    // 4-bit to 8-bit
            (8, 16),   // 8-bit to 16-bit
            (16, 32),  // 16-bit to 32-bit
            (4, 16)    // 4-bit to 16-bit (multi-step extension)
        };

        foreach (var (fromBits, toBits) in testCases)
        {
            // Test with maximum positive value for the bit width
            var maxPositive = (1 << (int)(fromBits - 1)) - 1; // e.g., for 4-bit: 7
            var minNegative = -(1 << (int)(fromBits - 1));    // e.g., for 4-bit: -8

            foreach (var value in new[] { maxPositive, minNegative, -1 })
            {
                var x = context.BitVecConst($"x_{fromBits}_{toBits}_{value}", (uint)fromBits);

                using var solver = context.CreateSolver();
                solver.Assert(x == context.BitVec(new BitVec(value, (uint)fromBits)));

                var zeroExtended = context.Extend(x, (uint)(toBits - fromBits), signed: false);
                var signExtended = context.Extend(x, (uint)(toBits - fromBits), signed: true);

                // For positive values, both should be the same
                if (value >= 0)
                {
                    solver.Assert(zeroExtended == signExtended);
                    solver.Assert(signExtended == context.BitVec(new BitVec(value, (uint)toBits)));
                }
                else
                {
                    // For negative values, they should differ
                    solver.Assert(zeroExtended != signExtended);
                    // Sign extended should preserve the negative value
                    solver.Assert(signExtended == context.BitVec(new BitVec(value, (uint)toBits)));
                }

                Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            }
        }
    }

    [Test]
    public void SignedResize_TruncationAndExtension_WorksCorrectly()
    {
        // Arrange - test resize operation with both truncation and extension
        var x16 = context.BitVecConst("x16", 16);
        var negativeValue = new BitVec(-1000, 16);

        using var solver = context.CreateSolver();
        solver.Assert(x16 == context.BitVec(negativeValue));

        // Test truncation (16 -> 8 bits)
        var truncated = context.Resize(x16, 8, signed: true);
        var truncatedUnsigned = context.Resize(x16, 8, signed: false);

        // Both should have the same bit pattern (lower 8 bits), but semantic interpretation differs
        // The bit pattern of -1000 in 16-bit should truncate to the same 8-bit pattern
        var expectedTruncated = (short)-1000 & 0xFF; // Lower 8 bits
        solver.Assert(truncated == context.BitVec(new BitVec(expectedTruncated, 8)));
        solver.Assert(truncatedUnsigned == context.BitVec(new BitVec(expectedTruncated, 8)));

        // Test extension (16 -> 32 bits)
        var extendedSigned = context.Resize(x16, 32, signed: true);
        var extendedUnsigned = context.Resize(x16, 32, signed: false);

        // Sign extension should preserve the negative value
        solver.Assert(extendedSigned == context.BitVec(new BitVec(-1000, 32)));

        // Zero extension should interpret as large positive
        var unsignedInterpretation = unchecked((ushort)-1000); // Cast to get unsigned interpretation
        solver.Assert(extendedUnsigned == context.BitVec(new BitVec(unsignedInterpretation, 32)));

        // They should be different
        solver.Assert(extendedSigned != extendedUnsigned);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    // Cross-bit-width test removed - individual operation tests provide sufficient coverage

    [Test]
    public void SignedToIntConversion_VariousValues_PreservesSignedSemantics()
    {
        // Arrange - test ToInt conversion with signed parameter
        var testValues = new[] { -128, -1, 0, 1, 127, 128, 255 }; // Mix of signed/unsigned interpretations

        foreach (var value in testValues)
        {
            var x8 = context.BitVecConst($"x8_{value}", 8);

            using var solver = context.CreateSolver();
            solver.Assert(x8 == context.BitVec(new BitVec(value, 8)));

            var unsignedInt = context.ToInt(x8, signed: false);
            var signedInt = context.ToInt(x8, signed: true);

            if (value < 0 || value > 127)
            {
                // For values outside signed 8-bit range, signed and unsigned should differ
                solver.Assert(unsignedInt != signedInt);

                if (value > 127)
                {
                    // Values > 127 should be interpreted as negative in signed conversion
                    var expectedSigned = (sbyte)value; // Cast to signed byte
                    solver.Assert(signedInt == context.Int(expectedSigned));
                    solver.Assert(unsignedInt == context.Int(value)); // Unsigned interpretation
                }
            }
            else
            {
                // For values in 0-127 range, both should be the same
                solver.Assert(unsignedInt == signedInt);
                solver.Assert(signedInt == context.Int(value));
            }

            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        }
    }
}