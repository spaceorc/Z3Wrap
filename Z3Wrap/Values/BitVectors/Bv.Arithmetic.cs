namespace Spaceorc.Z3Wrap.Values.BitVectors;

public readonly partial struct Bv<TSize>
{
    /// <summary>
    /// Addition of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Sum of the bitvectors.</returns>
    public static Bv<TSize> operator +(Bv<TSize> left, Bv<TSize> right) => left.Add(right);

    /// <summary>
    /// Subtraction of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Difference of the bitvectors.</returns>
    public static Bv<TSize> operator -(Bv<TSize> left, Bv<TSize> right) => left.Sub(right);

    /// <summary>
    /// Multiplication of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Product of the bitvectors.</returns>
    public static Bv<TSize> operator *(Bv<TSize> left, Bv<TSize> right) => left.Mul(right);

    /// <summary>
    /// Unsigned division of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Quotient of the division.</returns>
    public static Bv<TSize> operator /(Bv<TSize> left, Bv<TSize> right) => left.Div(right);

    /// <summary>
    /// Unsigned remainder of two bitvectors.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Remainder of the division.</returns>
    public static Bv<TSize> operator %(Bv<TSize> left, Bv<TSize> right) => left.Rem(right);

    /// <summary>
    /// Negation of a bitvector using two's complement.
    /// </summary>
    /// <param name="operand">Operand to negate.</param>
    /// <returns>Negated bitvector value.</returns>
    public static Bv<TSize> operator -(Bv<TSize> operand) => new(-operand.value);

    /// <summary>
    /// Adds another bitvector to this bitvector.
    /// </summary>
    /// <param name="other">Bitvector to add.</param>
    /// <returns>Sum masked to the bit width.</returns>
    public Bv<TSize> Add(Bv<TSize> other) => new(value + other.value);

    /// <summary>
    /// Subtracts another bitvector from this bitvector.
    /// </summary>
    /// <param name="other">Bitvector to subtract.</param>
    /// <returns>Difference masked to the bit width.</returns>
    public Bv<TSize> Sub(Bv<TSize> other) => new(value - other.value);

    /// <summary>
    /// Multiplies this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">Bitvector to multiply by.</param>
    /// <returns>Product masked to the bit width.</returns>
    public Bv<TSize> Mul(Bv<TSize> other) => new(value * other.value);

    /// <summary>
    /// Divides this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">Bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed division.</param>
    /// <returns>Quotient of the division.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public Bv<TSize> Div(Bv<TSize> other, bool signed = false)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (!signed)
            return new Bv<TSize>(value / other.value);
        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return new Bv<TSize>(leftSigned / rightSigned);
    }

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another.
    /// </summary>
    /// <param name="other">Bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed remainder.</param>
    /// <returns>Remainder of the division.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public Bv<TSize> Rem(Bv<TSize> other, bool signed = false)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (signed)
        {
            var leftSigned = ToBigInteger(signed: true);
            var rightSigned = other.ToBigInteger(signed: true);
            return new Bv<TSize>(leftSigned % rightSigned);
        }
        return new Bv<TSize>(value % other.value);
    }

    /// <summary>
    /// Computes Z3-style signed modulo where result has same sign as divisor.
    /// </summary>
    /// <param name="other">Bitvector to divide by.</param>
    /// <returns>Signed modulo result.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public Bv<TSize> SignedMod(Bv<TSize> other)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        var result = leftSigned % rightSigned;

        // Z3-style signed modulo: result has same sign as divisor
        if (result != 0 && (result < 0) != (rightSigned < 0))
            result += rightSigned;

        return new Bv<TSize>(result);
    }
}
