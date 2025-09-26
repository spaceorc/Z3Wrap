using Spaceorc.Z3Wrap.Values;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public sealed partial class Z3BitVec<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Performs bitwise AND with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to AND with.</param>
    /// <returns>A bitvector expression representing this &amp; other.</returns>
    public Z3BitVec<TSize> And(Z3BitVec<TSize> other) => Context.And(this, other);

    /// <summary>
    /// Performs bitwise OR with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to OR with.</param>
    /// <returns>A bitvector expression representing this | other.</returns>
    public Z3BitVec<TSize> Or(Z3BitVec<TSize> other) => Context.Or(this, other);

    /// <summary>
    /// Performs bitwise XOR with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to XOR with.</param>
    /// <returns>A bitvector expression representing this ^ other.</returns>
    public Z3BitVec<TSize> Xor(Z3BitVec<TSize> other) => Context.Xor(this, other);

    /// <summary>
    /// Performs bitwise NOT of this bitvector.
    /// </summary>
    /// <returns>A bitvector expression representing ~this.</returns>
    public Z3BitVec<TSize> Not() => Context.Not(this);

    /// <summary>
    /// Performs bitwise AND between two bitvector expressions using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left &amp; right.</returns>
    public static Z3BitVec<TSize> operator &(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.And(right);

    /// <summary>
    /// Performs bitwise OR between two bitvector expressions using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left | right.</returns>
    public static Z3BitVec<TSize> operator |(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Or(right);

    /// <summary>
    /// Performs bitwise XOR between two bitvector expressions using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left ^ right.</returns>
    public static Z3BitVec<TSize> operator ^(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Xor(right);

    /// <summary>
    /// Performs bitwise NOT of a bitvector expression using the ~ operator.
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>A bitvector expression representing ~operand.</returns>
    public static Z3BitVec<TSize> operator ~(Z3BitVec<TSize> operand) => operand.Not();
}
