using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.BitVectors;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3BitVec<TSize>
{
    /// <summary>
    /// Checks equality between this bitvector and another bitvector.
    /// </summary>
    /// <param name="other">The other bitvector to compare with.</param>
    /// <returns>A boolean expression representing this == other.</returns>
    public Z3BoolExpr Eq(Z3BitVec<TSize> other) => Context.Eq(this, other);

    /// <summary>
    /// Checks inequality between this bitvector and another bitvector.
    /// </summary>
    /// <param name="other">The other bitvector to compare with.</param>
    /// <returns>A boolean expression representing this != other.</returns>
    public Z3BoolExpr Neq(Z3BitVec<TSize> other) => Context.Neq(this, other);

    /// <summary>
    /// Compares two bitvector expressions for equality using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Eq(right);

    /// <summary>
    /// Compares two bitvector expressions for inequality using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Neq(right);
}
