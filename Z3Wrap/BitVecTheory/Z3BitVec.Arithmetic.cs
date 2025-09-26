namespace Spaceorc.Z3Wrap.BitVecTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3BitVec<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Adds this bitvector to another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A bitvector expression representing this + other.</returns>
    public Z3BitVec<TSize> Add(Z3BitVec<TSize> other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another bitvector expression of the same size from this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A bitvector expression representing this - other.</returns>
    public Z3BitVec<TSize> Sub(Z3BitVec<TSize> other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A bitvector expression representing this * other.</returns>
    public Z3BitVec<TSize> Mul(Z3BitVec<TSize> other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) division.</param>
    /// <returns>A bitvector expression representing this / other.</returns>
    public Z3BitVec<TSize> Div(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Div(this, other, signed);

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) remainder.</param>
    /// <returns>A bitvector expression representing this % other.</returns>
    public Z3BitVec<TSize> Rem(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Rem(this, other, signed);

    /// <summary>
    /// Negates this bitvector (computes two's complement).
    /// </summary>
    /// <returns>A bitvector expression representing -this.</returns>
    public Z3BitVec<TSize> Neg() => Context.Neg(this);

    /// <summary>
    /// Computes the signed modulo of this bitvector with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compute modulo with.</param>
    /// <returns>A bitvector expression representing signed this mod other.</returns>
    public Z3BitVec<TSize> SignedMod(Z3BitVec<TSize> other) => Context.SignedMod(this, other);

    /// <summary>
    /// Adds two bitvector expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left + right.</returns>
    public static Z3BitVec<TSize> operator +(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Add(right);

    /// <summary>
    /// Subtracts two bitvector expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left - right.</returns>
    public static Z3BitVec<TSize> operator -(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Sub(right);

    /// <summary>
    /// Multiplies two bitvector expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left * right.</returns>
    public static Z3BitVec<TSize> operator *(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Mul(right);

    /// <summary>
    /// Divides two bitvector expressions using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left / right.</returns>
    public static Z3BitVec<TSize> operator /(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Div(right);

    /// <summary>
    /// Computes the remainder of two bitvector expressions using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left % right.</returns>
    public static Z3BitVec<TSize> operator %(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Rem(right);

    /// <summary>
    /// Negates a bitvector expression using the unary - operator (two's complement).
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>A bitvector expression representing -operand.</returns>
    public static Z3BitVec<TSize> operator -(Z3BitVec<TSize> operand) => operand.Neg();
}
