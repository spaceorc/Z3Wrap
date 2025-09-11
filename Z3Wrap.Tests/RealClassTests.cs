using System.Numerics;

namespace Z3Wrap.Tests;

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
        Assert.That(oneThird >= oneThird, Is.True);
        Assert.That(oneHalf <= oneHalf, Is.True);
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
    public void Constructor_FromDecimal_CreatesExactRational()
    {
        var real = new Real(0.125m);
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(8));
    }

    [Test]
    public void ExplicitConversion_FromDouble_CreatesRationalApproximation()
    {
        Real real = (Real)0.5;
        Assert.That((int)real.Numerator, Is.EqualTo(1));
        Assert.That((int)real.Denominator, Is.EqualTo(2));
    }

    [Test]
    public void ExplicitConversion_ToDouble_ReturnsApproximateValue()
    {
        var oneThird = new Real(1, 3);
        double result = (double)oneThird;
        Assert.That(result, Is.EqualTo(1.0 / 3.0).Within(1e-15));
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
    public void Parse_DecimalString_CreatesExactRational()
    {
        var real = Real.Parse("0.25");
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
}