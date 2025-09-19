using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BitVecExprTests;

[TestFixture]
public class Z3BitVecExprFluentBigIntegerTests
{
    [Test]
    public void FluentAPI_BigIntegerArithmetic_AllOperations_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 32);
        var bigValue1 = new BigInteger(1000000);
        var bigValue2 = new BigInteger(123456);

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(bigValue1, 32)));

        // Test all arithmetic operations with BigInteger via fluent API
        var addResult = x + bigValue2;
        var subResult = x - bigValue2;
        var mulResult = x * bigValue2;
        var divResult = x / bigValue2;
        var remResult = x % bigValue2;

        // Verify results
        solver.Assert(addResult == context.BitVec(new BitVec(bigValue1 + bigValue2, 32)));
        solver.Assert(subResult == context.BitVec(new BitVec(bigValue1 - bigValue2, 32)));
        solver.Assert(mulResult == context.BitVec(new BitVec(bigValue1 * bigValue2, 32)));
        solver.Assert(divResult == context.BitVec(new BitVec(bigValue1 / bigValue2, 32)));
        solver.Assert(remResult == context.BitVec(new BitVec(bigValue1 % bigValue2, 32)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerBitwise_AllOperations_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 32);
        var mask = new BigInteger(0xFF00FF00);
        var pattern = new BigInteger(0x12345678);

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(pattern, 32)));

        // Test bitwise operations with BigInteger via fluent API
        var andResult = x & mask;
        var orResult = x | mask;
        var xorResult = x ^ mask;

        // Verify results
        var expectedAnd = pattern & mask;
        var expectedOr = pattern | mask;
        var expectedXor = pattern ^ mask;

        solver.Assert(andResult == context.BitVec(new BitVec(expectedAnd, 32)));
        solver.Assert(orResult == context.BitVec(new BitVec(expectedOr, 32)));
        solver.Assert(xorResult == context.BitVec(new BitVec(expectedXor, 32)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerShift_BothDirections_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 32);
        var value = new BigInteger(0x12345678);
        var shiftAmount = new BigInteger(4);

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(value, 32)));

        // Test shift operations with BigInteger amounts
        var leftShift = x << shiftAmount;
        var rightShiftLogical = x >> shiftAmount;

        // Note: For right shift with signed parameter, we need to use the method form
        var y = context.BitVecConst("y", 32);
        var negativeValue = new BitVec(-1000, 32);
        solver.Assert(y == context.BitVec(negativeValue));
        var rightShiftArithmetic = y.Shr(context.BitVec(shiftAmount, 32), signed: true);

        // Verify results
        var expectedLeft = value << (int)shiftAmount;
        var expectedRightLogical = (uint)((ulong)(uint)value >> (int)shiftAmount);

        solver.Assert(leftShift == context.BitVec(new BitVec(expectedLeft, 32)));
        solver.Assert(rightShiftLogical == context.BitVec(new BitVec(expectedRightLogical, 32)));

        // For arithmetic shift, the result should preserve the sign bit
        var expectedRightArithmetic = -1000 >> (int)shiftAmount;
        solver.Assert(
            rightShiftArithmetic == context.BitVec(new BitVec(expectedRightArithmetic, 32))
        );

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerComparison_SignedVsUnsigned_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange - test with a value that has different signed/unsigned interpretation
        var x = context.BitVecConst("x", 16);
        var negativeAsSigned = new BigInteger(-1000);
        var sameAsUnsigned = new BigInteger(65536 - 1000); // 2^16 - 1000 = unsigned representation
        var positiveValue = new BigInteger(500);

        using var solver = context.CreateSolver();

        // Set x to the unsigned representation of -1000
        solver.Assert(x == context.BitVec(new BitVec(sameAsUnsigned, 16)));

        // Test unsigned comparisons using fluent API
        solver.Assert(x > positiveValue); // Unsigned: large positive > small positive
        solver.Assert(x >= positiveValue);
        solver.Assert(!(x < positiveValue));
        solver.Assert(!(x <= positiveValue));

        // Test signed comparisons using fluent API with signed parameter
        solver.Assert(x.Lt(positiveValue, signed: true)); // Signed: -1000 < 500
        solver.Assert(x.Le(positiveValue, signed: true));
        solver.Assert(!x.Gt(positiveValue, signed: true));
        solver.Assert(!x.Ge(positiveValue, signed: true));

        // Test with BigInteger comparisons - x contains the unsigned representation (65536-1000=64536)
        // When compared as unsigned: 64536 > 1000 (which is the unsigned representation of -1000)
        // But since we're comparing with the signed value -1000, this doesn't make sense
        // Let's fix this logic
        solver.Assert(x.Gt(positiveValue, signed: false)); // 64536 > 500 unsigned

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerComparison_EdgeCases_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 8);
        var zero = BigInteger.Zero;
        var maxUnsigned = new BigInteger(255);
        var maxSigned = new BigInteger(127);
        var minSigned = new BigInteger(-128);

        using var solver = context.CreateSolver();

        // Test edge case: zero
        solver.Push();
        solver.Assert(x == context.BitVec(new BitVec(0, 8)));
        solver.Assert(x == zero);
        solver.Assert(x.Ge(zero));
        solver.Assert(x.Le(zero));
        solver.Assert(!x.Gt(zero));
        solver.Assert(!x.Lt(zero));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        solver.Pop();

        // Test edge case: maximum unsigned value (255)
        solver.Push();
        solver.Assert(x == context.BitVec(new BitVec(255, 8)));
        solver.Assert(x == maxUnsigned);
        solver.Assert(x.Gt(maxSigned, signed: false)); // 255 > 127 unsigned
        solver.Assert(x.Lt(zero, signed: true)); // 255 is -1 in signed interpretation
        solver.Assert(x.Gt(minSigned, signed: true)); // -1 > -128 in signed
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        solver.Pop();

        // Test edge case: maximum signed positive (127)
        solver.Push();
        solver.Assert(x == context.BitVec(new BitVec(127, 8)));
        solver.Assert(x == maxSigned);
        solver.Assert(x.Gt(zero, signed: true));
        solver.Assert(x.Lt(maxUnsigned, signed: false));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        solver.Pop();

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerOverflowChecks_VariousScenarios_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 8);
        var largeValue = new BigInteger(200);
        var smallValue = new BigInteger(100);
        var negativeValue = new BigInteger(-50);

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(largeValue, 8)));

        // Test overflow detection with BigInteger operations
        var wouldOverflow = x.AddNoOverflow(smallValue, signed: false); // 200 + 100 = 300 > 255
        var wouldNotOverflow = x.AddNoOverflow(new BigInteger(50), signed: false); // 200 + 50 = 250 <= 255

        solver.Assert(!wouldOverflow); // Should detect overflow
        solver.Assert(wouldNotOverflow); // Should not detect overflow

        // Test signed overflow detection
        var y = context.BitVecConst("y", 8);
        solver.Assert(y == context.BitVec(new BitVec(120, 8)));
        var signedOverflow = y.AddNoOverflow(new BigInteger(20), signed: true); // 120 + 20 = 140, but max signed is 127
        solver.Assert(!signedOverflow); // Should detect signed overflow

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerChainedOperations_ComplexExpressions_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange
        var x = context.BitVecConst("x", 32);
        var y = context.BitVecConst("y", 32);
        var bigConst1 = new BigInteger(1000000);
        var bigConst2 = new BigInteger(123456);
        var bigConst3 = new BigInteger(7);

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(bigConst1, 32)));
        solver.Assert(y == context.BitVec(new BitVec(bigConst2, 32)));

        // Test complex chained expressions with BigInteger
        var complexExpr = (x + bigConst2) * bigConst3 - (y & new BigInteger(0xFFFF));

        // Calculate expected result
        var expectedResult = (bigConst1 + bigConst2) * bigConst3 - (bigConst2 & 0xFFFF);
        solver.Assert(complexExpr == context.BitVec(new BitVec(expectedResult, 32)));

        // Test chained comparisons
        solver.Assert(x.Gt(bigConst2));
        solver.Assert(y.Lt(bigConst1));
        solver.Assert((x - y).Gt(new BigInteger(800000)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void FluentAPI_BigIntegerLargeValues_Performance_WorkCorrectly()
    {
        using var context = new Z3Context();
        // Arrange - test with very large BigInteger values
        var x = context.BitVecConst("x", 64);
        var veryLarge = BigInteger.Pow(2, 50); // 2^50
        var anotherLarge = BigInteger.Parse("1000000000000000"); // 10^15, smaller than 2^50

        using var solver = context.CreateSolver();
        solver.Assert(x == context.BitVec(new BitVec(veryLarge, 64)));

        // Test operations with very large values
        var addResult = x + anotherLarge;
        var comparison = x.Gt(anotherLarge);
        var bitwiseResult = x & (BigInteger.Pow(2, 60) - 1); // Large bitmask

        // These should complete without performance issues
        solver.Assert(addResult == context.BitVec(new BitVec(veryLarge + anotherLarge, 64)));
        solver.Assert(comparison); // 2^50 should be greater than 10^15

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
