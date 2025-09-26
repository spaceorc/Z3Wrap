using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

public readonly partial struct Real
{
    /// <summary>
    /// Adds two rational numbers using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A rational number representing the sum.</returns>
    public static Real operator +(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator + right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Subtracts one rational number from another using the - operator.
    /// </summary>
    /// <param name="left">The left operand (minuend).</param>
    /// <param name="right">The right operand (subtrahend).</param>
    /// <returns>A rational number representing the difference.</returns>
    public static Real operator -(Real left, Real right)
    {
        var newNum = left.numerator * right.denominator - right.numerator * left.denominator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Multiplies two rational numbers using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A rational number representing the product.</returns>
    public static Real operator *(Real left, Real right)
    {
        var newNum = left.numerator * right.numerator;
        var newDen = left.denominator * right.denominator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Divides one rational number by another using the / operator.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A rational number representing the quotient.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public static Real operator /(Real left, Real right)
    {
        if (right.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        var newNum = left.numerator * right.denominator;
        var newDen = left.denominator * right.numerator;
        return new Real(newNum, newDen);
    }

    /// <summary>
    /// Negates a rational number using the unary - operator.
    /// </summary>
    /// <param name="value">The rational number to negate.</param>
    /// <returns>A rational number representing the negated value.</returns>
    public static Real operator -(Real value) => new(-value.numerator, value.denominator);
}