namespace Spaceorc.Z3Wrap.BitVecTheory;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Adds two bitvectors using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new bitvector containing the sum.</returns>
    public static BitVec<TSize> operator +(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Add(right);

    /// <summary>
    /// Subtracts one bitvector from another using the - operator.
    /// </summary>
    /// <param name="left">The left operand (minuend).</param>
    /// <param name="right">The right operand (subtrahend).</param>
    /// <returns>A new bitvector containing the difference.</returns>
    public static BitVec<TSize> operator -(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Sub(right);

    /// <summary>
    /// Multiplies two bitvectors using the * operator.
    /// </summary>
    /// <param name="left">The left operand (multiplicand).</param>
    /// <param name="right">The right operand (multiplier).</param>
    /// <returns>A new bitvector containing the product.</returns>
    public static BitVec<TSize> operator *(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Mul(right);

    /// <summary>
    /// Divides one bitvector by another using the / operator.
    /// Performs unsigned division.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    public static BitVec<TSize> operator /(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Div(right);

    /// <summary>
    /// Computes the remainder of dividing one bitvector by another using the % operator.
    /// Performs unsigned remainder operation.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    public static BitVec<TSize> operator %(BitVec<TSize> left, BitVec<TSize> right) =>
        left.Rem(right);

    /// <summary>
    /// Negates a bitvector using the unary - operator.
    /// Performs two's complement negation.
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>A new bitvector containing the negated value.</returns>
    public static BitVec<TSize> operator -(BitVec<TSize> operand) => new(-operand.value);

    /// <summary>
    /// Adds another bitvector to this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A new bitvector containing the sum, masked to the bit width.</returns>
    public BitVec<TSize> Add(BitVec<TSize> other) => new(value + other.value);

    /// <summary>
    /// Subtracts another bitvector from this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A new bitvector containing the difference, masked to the bit width.</returns>
    public BitVec<TSize> Sub(BitVec<TSize> other) => new(value - other.value);

    /// <summary>
    /// Multiplies this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A new bitvector containing the product, masked to the bit width.</returns>
    public BitVec<TSize> Mul(BitVec<TSize> other) => new(value * other.value);

    /// <summary>
    /// Divides this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) division.</param>
    /// <returns>A new bitvector containing the quotient.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec<TSize> Div(BitVec<TSize> other, bool signed = false)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (!signed)
            return new BitVec<TSize>(value / other.value);
        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        return new BitVec<TSize>(leftSigned / rightSigned);
    }

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another bitvector.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) remainder.</param>
    /// <returns>A new bitvector containing the remainder.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec<TSize> Rem(BitVec<TSize> other, bool signed = false)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");
        if (signed)
        {
            var leftSigned = ToBigInteger(signed: true);
            var rightSigned = other.ToBigInteger(signed: true);
            return new BitVec<TSize>(leftSigned % rightSigned);
        }
        return new BitVec<TSize>(value % other.value);
    }

    /// <summary>
    /// Computes the Z3-style signed modulo operation where the result has the same sign as the divisor.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <returns>A new bitvector containing the signed modulo result.</returns>
    /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
    public BitVec<TSize> SignedMod(BitVec<TSize> other)
    {
        if (other.IsZero)
            throw new DivideByZeroException("Division by zero is not allowed");

        var leftSigned = ToBigInteger(signed: true);
        var rightSigned = other.ToBigInteger(signed: true);
        var result = leftSigned % rightSigned;

        // Z3-style signed modulo: result has same sign as divisor
        if (result != 0 && (result < 0) != (rightSigned < 0))
            result += rightSigned;

        return new BitVec<TSize>(result);
    }
}
