using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Tests.DataTypesTests;

[TestFixture]
public class RealClassTests
{
    [Test]
    public void Constructor_IntegerValues_CreatesCorrectRational()
    {
        var real = new Real(3, 4);
        Assert.That((int)real.Numerator, Is.EqualTo(3));
        Assert.That((int)real.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Constructor_NegativeDenominator_MovesSignToNumerator()
    {
        var real = new Real(3, -4);
        Assert.That((int)real.Numerator, Is.EqualTo(-3));
        Assert.That((int)real.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Constructor_ZeroDenominator_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Real(1, 0));
    }

    [Test]
    public void Constructor_SimplifiesFraction_ReducesToLowestTerms()
    {
        var real = new Real(6, 9);
        Assert.That((int)real.Numerator, Is.EqualTo(2));
        Assert.That((int)real.Denominator, Is.EqualTo(3));
    }

    [Test]
    public void Constructor_IntegerValue_CreatesIntegerRational()
    {
        var real = new Real(42);
        Assert.That((int)real.Numerator, Is.EqualTo(42));
        Assert.That((int)real.Denominator, Is.EqualTo(1));
        Assert.That(real.IsInteger, Is.True);
    }

    [Test]
    public void Properties_IsZero_DetectsZeroCorrectly()
    {
        Assert.That(new Real(0, 1).IsZero, Is.True);
        Assert.That(new Real(0, 5).IsZero, Is.True);
        Assert.That(new Real(1, 5).IsZero, Is.False);
    }

    [Test]
    public void Properties_IsPositiveNegative_DetectsSignCorrectly()
    {
        Assert.That(new Real(3, 4).IsPositive, Is.True);
        Assert.That(new Real(3, 4).IsNegative, Is.False);
        Assert.That(new Real(-3, 4).IsPositive, Is.False);
        Assert.That(new Real(-3, 4).IsNegative, Is.True);
        Assert.That(new Real(0, 1).IsPositive, Is.False);
        Assert.That(new Real(0, 1).IsNegative, Is.False);
    }

    [Test]
    public void Addition_ExactArithmetic_PreservesRationalPrecision()
    {
        var oneThird = new Real(1, 3);
        var oneSixth = new Real(1, 6);
        var result = oneThird + oneSixth;

        Assert.That((int)result.Numerator, Is.EqualTo(1));
        Assert.That((int)result.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void Subtraction_ExactArithmetic_PreservesRationalPrecision()
    {
        var twoThirds = new Real(2, 3);
        var oneSixth = new Real(1, 6);
        var result = twoThirds - oneSixth;

        Assert.That((int)result.Numerator, Is.EqualTo(1));
        Assert.That((int)result.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void Multiplication_ExactArithmetic_PreservesRationalPrecision()
    {
        var twoThirds = new Real(2, 3);
        var threeFourths = new Real(3, 4);
        var result = twoThirds * threeFourths;

        Assert.That((int)result.Numerator, Is.EqualTo(1));
        Assert.That((int)result.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void Division_ExactArithmetic_PreservesRationalPrecision()
    {
        var oneHalf = new Real(1, 2);
        var oneThird = new Real(1, 3);
        var result = oneHalf / oneThird;

        Assert.That((int)result.Numerator, Is.EqualTo(3));
        Assert.That((int)result.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void Division_ByZero_ThrowsDivideByZeroException()
    {
        var nonZero = new Real(1, 2);
        var zero = new Real(0);
        Assert.Throws<DivideByZeroException>(() => { var result = nonZero / zero; });
    }

    [Test]
    public void UnaryMinus_NegatesValue_CorrectlyFlipsSign()
    {
        var positive = new Real(3, 4);
        var negative = -positive;

        Assert.That((int)negative.Numerator, Is.EqualTo(-3));
        Assert.That((int)negative.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Comparison_EqualValues_ReturnsTrue()
    {
        var a = new Real(1, 2);
        var b = new Real(2, 4);
        Assert.That(a == b, Is.True);
        Assert.That(a != b, Is.False);
    }

    [Test]
    public void Comparison_OrderedValues_ReturnsCorrectComparison()
    {
        var oneThird = new Real(1, 3);
        var oneHalf = new Real(1, 2);

        Assert.That(oneThird < oneHalf, Is.True);
        Assert.That(oneThird <= oneHalf, Is.True);
        Assert.That(oneHalf > oneThird, Is.True);
        Assert.That(oneHalf >= oneThird, Is.True);
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.That(oneThird >= oneThird, Is.True);
        Assert.That(oneHalf <= oneHalf, Is.True);
#pragma warning restore CS1718
    }

    [Test]
    public void ImplicitConversion_FromInt_CreatesIntegerRational()
    {
        Real real = 42;
        Assert.That((int)real.Numerator, Is.EqualTo(42));
        Assert.That((int)real.Denominator, Is.EqualTo(1));
        Assert.That(real.IsInteger, Is.True);
    }

    [Test]
    public void ImplicitConversion_FromLong_CreatesIntegerRational()
    {
        Real real = 42L;
        Assert.That((int)real.Numerator, Is.EqualTo(42));
        Assert.That((int)real.Denominator, Is.EqualTo(1));
        Assert.That(real.IsInteger, Is.True);
    }

    [Test]
    public void ImplicitConversion_FromDecimal_CreatesExactRational()
    {
        Real real = 0.25m; // Implicit conversion from decimal
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void ImplicitConversion_FromBigInteger_CreatesIntegerRational()
    {
        var bigInt = new BigInteger(12345678901234567890L);
        Real real = bigInt; // Implicit conversion from BigInteger
        Assert.That(real.Numerator, Is.EqualTo(bigInt));
        Assert.That(real.Denominator, Is.EqualTo(BigInteger.One));
        Assert.That(real.IsInteger, Is.True);
    }

    [Test]
    public void Constructor_FromDecimal_CreatesExactRational()
    {
        var real = new Real(0.125m);
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(8));
    }

    [Test]
    public void ExplicitConversion_FromDecimal_CreatesRationalRepresentation()
    {
        var real = (Real)0.5m;
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void ExplicitConversion_FromDecimal_PreservesExactValues()
    {
        // Values that can be exactly represented
        var quarter = (Real)0.25m;
        Assert.That((int)quarter.Numerator, Is.EqualTo(1));
        Assert.That((int)quarter.Denominator, Is.EqualTo(4));

        var eighth = (Real)0.125m;
        Assert.That((int)eighth.Numerator, Is.EqualTo(1));
        Assert.That((int)eighth.Denominator, Is.EqualTo(8));
    }

    [Test]
    public void ExplicitConversion_ToDecimal_ReturnsDecimalValue()
    {
        var oneQuarter = new Real(1, 4);
        var result = (decimal)oneQuarter;
        Assert.That(result, Is.EqualTo(0.25m));

        var twoThirds = new Real(2, 3);
        var result2 = (decimal)twoThirds;
        Assert.That(result2, Is.EqualTo(2.0m / 3.0m));
    }

    [Test]
    public void ToDecimal_NormalValues_ReturnsCorrectDecimal()
    {
        var oneHalf = new Real(1, 2);
        Assert.That(oneHalf.ToDecimal(), Is.EqualTo(0.5m));

        var threeQuarters = new Real(3, 4);
        Assert.That(threeQuarters.ToDecimal(), Is.EqualTo(0.75m));

        var integer = new Real(42, 1);
        Assert.That(integer.ToDecimal(), Is.EqualTo(42m));

        var negative = new Real(-7, 2);
        Assert.That(negative.ToDecimal(), Is.EqualTo(-3.5m));
    }

    [Test]
    public void ExplicitConversion_ToInt_ReturnsIntegerValue()
    {
        var fortyTwo = new Real(42);
        var result = (int)fortyTwo;
        Assert.That(result, Is.EqualTo(42));
        
        var negative = new Real(-123);
        var result2 = (int)negative;
        Assert.That(result2, Is.EqualTo(-123));
    }

    [Test]
    public void ExplicitConversion_ToInt_NonIntegerThrows()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => { var result = (int)fraction; });
    }

    [Test]
    public void ExplicitConversion_ToInt_OverflowThrows()
    {
        var tooLarge = new Real(long.MaxValue);
        Assert.Throws<OverflowException>(() => { var result = (int)tooLarge; });
    }

    [Test]
    public void ExplicitConversion_ToLong_ReturnsLongValue()
    {
        var fortyTwo = new Real(42L);
        var result = (long)fortyTwo;
        Assert.That(result, Is.EqualTo(42L));
        
        var maxValue = new Real(long.MaxValue);
        var result2 = (long)maxValue;
        Assert.That(result2, Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void ExplicitConversion_ToLong_NonIntegerThrows()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => { var result = (long)fraction; });
    }

    [Test]
    public void ExplicitConversion_ToLong_OverflowThrows()
    {
        var tooLarge = new Real(new BigInteger(long.MaxValue) + 1, 1);
        Assert.Throws<OverflowException>(() => { var result = (long)tooLarge; });
    }

    [Test]
    public void ExplicitConversion_ToBigInteger_ReturnsCorrectValue()
    {
        var fortyTwo = new Real(42);
        var result = (BigInteger)fortyTwo;
        Assert.That(result, Is.EqualTo(new BigInteger(42)));
        
        var large = new Real(long.MaxValue);
        var result2 = (BigInteger)large;
        Assert.That(result2, Is.EqualTo(new BigInteger(long.MaxValue)));
        
        var veryLarge = new Real(new BigInteger(long.MaxValue) + 1000, 1);
        var result3 = (BigInteger)veryLarge;
        Assert.That(result3, Is.EqualTo(new BigInteger(long.MaxValue) + 1000));
    }

    [Test]
    public void ExplicitConversion_ToBigInteger_NonIntegerThrows()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => { var result = (BigInteger)fraction; });
    }

    [Test]
    public void Parse_IntegerString_CreatesIntegerRational()
    {
        var real = Real.Parse("42");
        Assert.That((int)real.Numerator, Is.EqualTo(42));
        Assert.That((int)real.Denominator, Is.EqualTo(1));
        Assert.That(real.IsInteger, Is.True);
    }

    [Test]
    public void Parse_FractionString_CreatesFractionalRational()
    {
        var real = Real.Parse("22/7");
        Assert.That((int)real.Numerator, Is.EqualTo(22));
        Assert.That((int)real.Denominator, Is.EqualTo(7));
    }

    [Test]
    public void Parse_NegativeFractionString_CreatesNegativeRational()
    {
        var real = Real.Parse("-3/4");
        Assert.That((int)real.Numerator, Is.EqualTo(-3));
        Assert.That((int)real.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Parse_FractionString_CreatesExactRational()
    {
        var real = Real.Parse("1/4");
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Parse_InvalidString_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Real.Parse("invalid"));
        Assert.Throws<FormatException>(() => Real.Parse("1/0"));
        Assert.Throws<FormatException>(() => Real.Parse(""));
    }

    [Test]
    public void Parse_InvalidNumerator_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Real.Parse("invalid/5"));
        Assert.Throws<FormatException>(() => Real.Parse("abc/10"));
    }

    [Test]
    public void Parse_InvalidDenominator_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Real.Parse("5/invalid"));
        Assert.Throws<FormatException>(() => Real.Parse("10/abc"));
    }

    [Test]
    public void Parse_InvalidDecimalFormat_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => Real.Parse("1.2m.3"));
        Assert.Throws<FormatException>(() => Real.Parse("abc.def"));
    }

    [Test]
    public void TryParse_ValidString_ReturnsTrue()
    {
        var success = Real.TryParse("3/4", out var result);
        Assert.That(success, Is.True);
        Assert.That((int)result.Numerator, Is.EqualTo(3));
        Assert.That((int)result.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        var success = Real.TryParse("invalid", out var result);
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(Real)));
    }

    [Test]
    public void ToString_DefaultFormat_ShowsFraction()
    {
        var real = new Real(3, 4);
        Assert.That(real.ToString(), Is.EqualTo("3/4"));
    }

    [Test]
    public void ToString_IntegerValue_ShowsInteger()
    {
        var real = new Real(42);
        Assert.That(real.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ToString_FractionFormat_ShowsFraction()
    {
        var real = new Real(3, 4);
        Assert.That(real.ToString("F"), Is.EqualTo("3/4"));
        Assert.That(real.ToString("FRACTION"), Is.EqualTo("3/4"));
    }

    [Test]
    public void ToString_DecimalFormat_ShowsDecimalApproximation()
    {
        var real = new Real(1, 4);
        Assert.That(real.ToString("D"), Is.EqualTo("0.25"));
        Assert.That(real.ToString("DECIMAL"), Is.EqualTo("0.25"));
    }

    [Test]
    public void Abs_PositiveValue_ReturnsSameValue()
    {
        var positive = new Real(3, 4);
        var result = positive.Abs();
        Assert.That(result, Is.EqualTo(positive));
    }

    [Test]
    public void Abs_NegativeValue_ReturnsPositiveValue()
    {
        var negative = new Real(-3, 4);
        var result = negative.Abs();
        Assert.That((int)result.Numerator, Is.EqualTo(3));
        Assert.That((int)result.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void Reciprocal_NonZeroValue_ReturnsCorrectReciprocal()
    {
        var real = new Real(3, 4);
        var reciprocal = real.Reciprocal();
        Assert.That((int)reciprocal.Numerator, Is.EqualTo(4));
        Assert.That((int)reciprocal.Denominator, Is.EqualTo(3));
    }

    [Test]
    public void Reciprocal_ZeroValue_ThrowsDivideByZeroException()
    {
        var zero = new Real(0);
        Assert.Throws<DivideByZeroException>(() => zero.Reciprocal());
    }

    [Test]
    public void Power_IntegerExponent_ReturnsCorrectPower()
    {
        var base_ = new Real(2, 3);
        var result = base_.Power(2);

        Assert.That((int)result.Numerator, Is.EqualTo(4));
        Assert.That((int)result.Denominator, Is.EqualTo(9));
    }

    [Test]
    public void Power_ZeroExponent_ReturnsOne()
    {
        var base_ = new Real(2, 3);
        var result = base_.Power(0);
        Assert.That(result, Is.EqualTo(Real.One));
    }

    [Test]
    public void Power_NegativeExponent_ReturnsReciprocalPower()
    {
        var base_ = new Real(2, 3);
        var result = base_.Power(-2);

        Assert.That((int)result.Numerator, Is.EqualTo(9));
        Assert.That((int)result.Denominator, Is.EqualTo(4));
    }

    [Test]
    public void StaticConstants_HaveCorrectValues()
    {
        Assert.That(Real.Zero.IsZero, Is.True);
        Assert.That((int)Real.One.Numerator, Is.EqualTo(1));
        Assert.That((int)Real.One.Denominator, Is.EqualTo(1));
        Assert.That((int)Real.MinusOne.Numerator, Is.EqualTo(-1));
        Assert.That((int)Real.MinusOne.Denominator, Is.EqualTo(1));
    }

    [Test]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new Real(1, 2);
        var b = new Real(2, 4);
        
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals((object)b), Is.True);
    }

    [Test]
    public void GetHashCode_EqualValues_ReturnsSameHashCode()
    {
        var a = new Real(1, 2);
        var b = new Real(2, 4);
        
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void CompareTo_OrderedValues_ReturnsCorrectComparison()
    {
        var smaller = new Real(1, 3);
        var larger = new Real(1, 2);
        
        Assert.That(smaller.CompareTo(larger), Is.LessThan(0));
        Assert.That(larger.CompareTo(smaller), Is.GreaterThan(0));
        Assert.That(smaller.CompareTo(smaller), Is.EqualTo(0));
    }

    [Test]
    public void LargeNumbers_BigIntegerSupport_HandlesLargeValues()
    {
        var large1 = new Real(long.MaxValue, 2);
        var large2 = new Real(long.MaxValue, 3);
        var result = large1 + large2;
        
        Assert.That(result.Numerator, Is.Not.EqualTo(BigInteger.Zero));
        Assert.That(result.Denominator, Is.GreaterThan(BigInteger.Zero));
    }

    [Test]
    public void Min_ReturnsSmaller()
    {
        var oneThird = new Real(1, 3);
        var oneHalf = new Real(1, 2);
        
        Assert.That(Real.Min(oneThird, oneHalf), Is.EqualTo(oneThird));
        Assert.That(Real.Min(oneHalf, oneThird), Is.EqualTo(oneThird));
        Assert.That(Real.Min(oneThird, oneThird), Is.EqualTo(oneThird));
    }

    [Test]
    public void Max_ReturnsLarger()
    {
        var oneThird = new Real(1, 3);
        var oneHalf = new Real(1, 2);
        
        Assert.That(Real.Max(oneThird, oneHalf), Is.EqualTo(oneHalf));
        Assert.That(Real.Max(oneHalf, oneThird), Is.EqualTo(oneHalf));
        Assert.That(Real.Max(oneThird, oneThird), Is.EqualTo(oneThird));
    }

    [Test]
    public void ToInt_IntegerValue_ReturnsCorrectInt()
    {
        var real = new Real(42);
        Assert.That(real.ToInt(), Is.EqualTo(42));
        
        var negative = new Real(-123);
        Assert.That(negative.ToInt(), Is.EqualTo(-123));
    }

    [Test]
    public void ToInt_NonIntegerValue_ThrowsInvalidOperationException()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => fraction.ToInt());
    }

    [Test]
    public void ToLong_IntegerValue_ReturnsCorrectLong()
    {
        var real = new Real(42L);
        Assert.That(real.ToLong(), Is.EqualTo(42L));
        
        var large = new Real(long.MaxValue);
        Assert.That(large.ToLong(), Is.EqualTo(long.MaxValue));
    }

    [Test]
    public void ToLong_NonIntegerValue_ThrowsInvalidOperationException()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => fraction.ToLong());
    }

    [Test]
    public void ToBigInteger_IntegerValue_ReturnsCorrectBigInteger()
    {
        var real = new Real(42);
        Assert.That(real.ToBigInteger(), Is.EqualTo(new BigInteger(42)));
        
        var large = new Real(long.MaxValue);
        Assert.That(large.ToBigInteger(), Is.EqualTo(new BigInteger(long.MaxValue)));
        
        var veryLarge = new Real(new BigInteger(long.MaxValue) + 1000, 1);
        Assert.That(veryLarge.ToBigInteger(), Is.EqualTo(new BigInteger(long.MaxValue) + 1000));
    }

    [Test]
    public void ToBigInteger_NonIntegerValue_ThrowsInvalidOperationException()
    {
        var fraction = new Real(1, 3);
        Assert.Throws<InvalidOperationException>(() => fraction.ToBigInteger());
    }

    [Test]
    public void ToInt_OverflowValue_ThrowsOverflowException()
    {
        var tooLarge = new Real(long.MaxValue);
        Assert.Throws<OverflowException>(() => tooLarge.ToInt());
    }

    [Test]
    public void ToLong_OverflowValue_ThrowsOverflowException()
    {
        // Create a value larger than long.MaxValue using BigInteger
        var tooLarge = new Real(new BigInteger(long.MaxValue) + 1, 1);
        Assert.Throws<OverflowException>(() => tooLarge.ToLong());
    }

    [Test]
    public void Round_IntegerValue_ReturnsSameValue()
    {
        var integer = new Real(42);
        Assert.That(integer.Round(), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Round_DefaultToEven_RoundsCorrectly()
    {
        // Test rounding down
        var oneQuarter = new Real(1, 4); // 0.25m
        Assert.That(oneQuarter.Round(), Is.EqualTo(BigInteger.Zero));

        // Test rounding up  
        var threeQuarters = new Real(3, 4); // 0.75m
        Assert.That(threeQuarters.Round(), Is.EqualTo(BigInteger.One));

        // Test midpoint - should round to even
        var oneHalf = new Real(1, 2); // 0.5m - rounds to 0 (even)
        Assert.That(oneHalf.Round(), Is.EqualTo(BigInteger.Zero));

        var threeHalves = new Real(3, 2); // 1.5m - rounds to 2 (even)
        Assert.That(threeHalves.Round(), Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void Round_AwayFromZero_RoundsCorrectly()
    {
        var oneHalf = new Real(1, 2); // 0.5m
        Assert.That(oneHalf.Round(MidpointRounding.AwayFromZero), Is.EqualTo(BigInteger.One));

        var negativeHalf = new Real(-1, 2); // -0.5m
        Assert.That(negativeHalf.Round(MidpointRounding.AwayFromZero), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void Round_ToZero_RoundsCorrectly()
    {
        var oneHalf = new Real(1, 2); // 0.5m
        Assert.That(oneHalf.Round(MidpointRounding.ToZero), Is.EqualTo(BigInteger.Zero));

        var negativeHalf = new Real(-1, 2); // -0.5m
        Assert.That(negativeHalf.Round(MidpointRounding.ToZero), Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void ToDecimal_OverflowValues_ThrowsOverflowException()
    {
        // Very large integer
        var veryLarge = new Real(BigInteger.Pow(10, 50), 1);
        Assert.Throws<OverflowException>(() => veryLarge.ToDecimal());

        // Very small integer
        var verySmall = new Real(-BigInteger.Pow(10, 50), 1);
        Assert.Throws<OverflowException>(() => verySmall.ToDecimal());

        // Very large fraction
        var largeFraction = new Real(BigInteger.Pow(10, 100), 3);
        Assert.Throws<OverflowException>(() => largeFraction.ToDecimal());

        // Extremely large fraction
        var extremelyLargeFraction = new Real(BigInteger.Pow(10, 1000), 3);
        Assert.Throws<OverflowException>(() => extremelyLargeFraction.ToDecimal());
    }

    [Test]
    public void Constructor_EdgeCases_HandlesCorrectly()
    {
        // Test very large numerator and denominator
        var largeNum = BigInteger.Pow(10, 100);
        var largeDen = BigInteger.Pow(10, 50);
        var largeReal = new Real(largeNum, largeDen);

        // Should be simplified to 10^50 / 1
        Assert.That(largeReal.Numerator, Is.EqualTo(BigInteger.Pow(10, 50)));
        Assert.That(largeReal.Denominator, Is.EqualTo(BigInteger.One));

        // Test with common factor
        var real = new Real(999999, 333333); // Should reduce to 3/1
        Assert.That(real.Numerator, Is.EqualTo(new BigInteger(3)));
        Assert.That(real.Denominator, Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void Parse_EdgeCases_WorksCorrectly()
    {
        // Test very large fractions
        var largeFraction = Real.Parse("123456789012345678901234567890/987654321098765432109876543210");
        Assert.That(largeFraction.Numerator, Is.Not.EqualTo(BigInteger.Zero));
        Assert.That(largeFraction.Denominator, Is.Not.EqualTo(BigInteger.Zero));

        // Test whitespace handling
        var whitespace = Real.Parse("  1 / 2  ");
        Assert.That(whitespace.Numerator, Is.EqualTo(BigInteger.One));
        Assert.That(whitespace.Denominator, Is.EqualTo(new BigInteger(2)));

        // Test decimal edge cases
        var smallDecimal = Real.Parse("0.000000000000001");
        Assert.That(smallDecimal.Numerator, Is.EqualTo(BigInteger.One));
        Assert.That(smallDecimal.Denominator, Is.EqualTo(BigInteger.Pow(10, 15)));
    }

    [Test]
    public void Arithmetic_EdgeCases_WorksCorrectly()
    {
        // Test arithmetic with very large numbers
        var large1 = new Real(BigInteger.Pow(10, 50), 1);
        var large2 = new Real(BigInteger.Pow(10, 50), 1);

        var sum = large1 + large2;
        Assert.That(sum.Numerator, Is.EqualTo(BigInteger.Pow(10, 50) * 2));

        // Test multiplication resulting in simplification
        var a = new Real(2, 3);
        var b = new Real(3, 4);
        var product = a * b; // Should be 6/12 = 1/2

        Assert.That(product.Numerator, Is.EqualTo(BigInteger.One));
        Assert.That(product.Denominator, Is.EqualTo(new BigInteger(2)));

        // Test division by very small number
        var tiny = new Real(1, BigInteger.Pow(10, 20));
        var one = new Real(1);
        var huge = one / tiny;

        Assert.That(huge.Numerator, Is.EqualTo(BigInteger.Pow(10, 20)));
        Assert.That(huge.Denominator, Is.EqualTo(BigInteger.One));
    }

    [Test]
    public void ExceptionMessages_AreStandardized()
    {
        // Test division by zero messages
        var one = new Real(1);
        var zero = new Real(0);

        var divEx = Assert.Throws<DivideByZeroException>(() => { var result = one / zero; });
        Assert.That(divEx.Message, Does.Contain("Division by zero is not allowed"));

        var reciprocalEx = Assert.Throws<DivideByZeroException>(() => zero.Reciprocal());
        Assert.That(reciprocalEx.Message, Does.Contain("Division by zero is not allowed"));

        // Test format exception messages
        var formatEx = Assert.Throws<FormatException>(() => Real.Parse(""));
        Assert.That(formatEx.Message, Does.Contain("Input string must not be null or empty"));

        // Test conversion exception messages
        var fraction = new Real(1, 3);
        var conversionEx = Assert.Throws<InvalidOperationException>(() => fraction.ToInt());
        Assert.That(conversionEx.Message, Does.Contain("Cannot convert non-integer value"));
    }

    [Test]
    public void ToString_EdgeCases_WorksCorrectly()
    {
        // Test very large numbers
        var large = new Real(BigInteger.Pow(10, 50), 1);
        var largeStr = large.ToString();
        Assert.That(largeStr, Does.Not.Contain("/"));
        Assert.That(largeStr.Length, Is.GreaterThan(40));

        // Test very small fractions
        var small = new Real(1, BigInteger.Pow(10, 20));
        var smallStr = small.ToString();
        Assert.That(smallStr, Does.Contain("/"));
        Assert.That(smallStr, Does.StartWith("1/"));

        // Test decimal format with large numbers throws for very large values
        Assert.Throws<OverflowException>(() => large.ToString("D"));
    }

    [Test]
    public void Round_EdgeCases_WorksCorrectly()
    {
        // Test rounding very large fractions
        var largeFraction = new Real(BigInteger.Pow(10, 50) + 1, 2); // (10^50 + 1) / 2
        var rounded = largeFraction.Round();
        Assert.That(rounded, Is.EqualTo(BigInteger.Pow(10, 50) / 2)); // Should round down

        // Test all rounding modes with midpoint
        var midpoint = new Real(5, 2); // 2.5

        Assert.That(midpoint.Round(MidpointRounding.ToEven), Is.EqualTo(new BigInteger(2)));
        Assert.That(midpoint.Round(MidpointRounding.AwayFromZero), Is.EqualTo(new BigInteger(3)));
        Assert.That(midpoint.Round(MidpointRounding.ToZero), Is.EqualTo(new BigInteger(2)));
        Assert.That(midpoint.Round(MidpointRounding.ToPositiveInfinity), Is.EqualTo(new BigInteger(3)));
        Assert.That(midpoint.Round(MidpointRounding.ToNegativeInfinity), Is.EqualTo(new BigInteger(2)));

        // Test negative midpoint
        var negativeMidpoint = new Real(-5, 2); // -2.5

        Assert.That(negativeMidpoint.Round(MidpointRounding.ToEven), Is.EqualTo(new BigInteger(-2)));
        Assert.That(negativeMidpoint.Round(MidpointRounding.AwayFromZero), Is.EqualTo(new BigInteger(-3)));
        Assert.That(negativeMidpoint.Round(MidpointRounding.ToZero), Is.EqualTo(new BigInteger(-2)));
        Assert.That(negativeMidpoint.Round(MidpointRounding.ToPositiveInfinity), Is.EqualTo(new BigInteger(-2)));
        Assert.That(negativeMidpoint.Round(MidpointRounding.ToNegativeInfinity), Is.EqualTo(new BigInteger(-3)));
    }
}