using Spaceorc.Z3Wrap.Booleans;

namespace Spaceorc.Z3Wrap.BitVectors;

public sealed partial class Z3BitVec<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Creates a less-than comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3Bool Lt(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Lt(this, other, signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3Bool Le(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Le(this, other, signed);

    /// <summary>
    /// Creates a greater-than comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3Bool Gt(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Gt(this, other, signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3Bool Ge(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Ge(this, other, signed);

    /// <summary>
    /// Compares two bitvector expressions using the &lt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3Bool operator <(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Lt(right);

    /// <summary>
    /// Compares two bitvector expressions using the &lt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3Bool operator <=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Le(right);

    /// <summary>
    /// Compares two bitvector expressions using the &gt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3Bool operator >(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Gt(right);

    /// <summary>
    /// Compares two bitvector expressions using the >= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3Bool operator >=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Ge(right);
}
