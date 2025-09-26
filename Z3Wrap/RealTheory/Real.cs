using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

/// <summary>
/// Represents an exact rational number with unlimited precision arithmetic.
/// Supports natural mathematical operations with automatic reduction to lowest terms.
/// </summary>
public readonly partial struct Real : IEquatable<Real>, IComparable<Real>, IFormattable
{
    private readonly BigInteger numerator;
    private readonly BigInteger denominator;

    /// <summary>
    /// Initializes a new rational number from integer numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (defaults to 1).</param>
    public Real(int numerator, int denominator = 1)
        : this(new BigInteger(numerator), new BigInteger(denominator)) { }

    /// <summary>
    /// Initializes a new rational number from long integer numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (defaults to 1).</param>
    public Real(long numerator, long denominator = 1)
        : this(new BigInteger(numerator), new BigInteger(denominator)) { }

    /// <summary>
    /// Initializes a new rational number from a BigInteger numerator (denominator = 1).
    /// </summary>
    /// <param name="numerator">The numerator value.</param>
    public Real(BigInteger numerator)
        : this(numerator, BigInteger.One) { }

    /// <summary>
    /// Initializes a new rational number from a decimal value with exact precision.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    public Real(decimal value)
    {
        var bits = decimal.GetBits(value);
        var sign = (bits[3] & 0x80000000) != 0 ? -1 : 1;
        var scale = (bits[3] >> 16) & 0x7F;

        var num =
            new BigInteger(sign)
            * (
                new BigInteger((uint)bits[2]) << 64
                | new BigInteger((uint)bits[1]) << 32
                | new BigInteger((uint)bits[0])
            );

        var den = BigInteger.Pow(10, scale);

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
        numerator = num / gcd;
        denominator = den / gcd;
    }

    /// <summary>
    /// Initializes a new rational number from BigInteger numerator and denominator.
    /// The fraction is automatically reduced to its lowest terms.
    /// </summary>
    /// <param name="numerator">The numerator.</param>
    /// <param name="denominator">The denominator (must be non-zero).</param>
    /// <exception cref="ArgumentException">Thrown when denominator is zero.</exception>
    public Real(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominator must be non-zero", nameof(denominator));

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
        this.numerator = numerator / gcd;
        this.denominator = denominator / gcd;
    }

    /// <summary>
    /// Gets the numerator of the rational number.
    /// </summary>
    public BigInteger Numerator => numerator;

    /// <summary>
    /// Gets the denominator of the rational number.
    /// </summary>
    public BigInteger Denominator => denominator;

    /// <summary>
    /// Gets a value indicating whether this rational number represents an integer.
    /// </summary>
    public bool IsInteger => denominator == 1;

    /// <summary>
    /// Gets a value indicating whether this rational number is zero.
    /// </summary>
    public bool IsZero => numerator == 0;

    /// <summary>
    /// Gets a value indicating whether this rational number is positive.
    /// </summary>
    public bool IsPositive => numerator > 0;

    /// <summary>
    /// Gets a value indicating whether this rational number is negative.
    /// </summary>
    public bool IsNegative => numerator < 0;
}