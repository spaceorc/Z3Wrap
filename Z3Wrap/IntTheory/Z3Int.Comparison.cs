using Spaceorc.Z3Wrap.BoolTheory;

namespace Spaceorc.Z3Wrap.IntTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Int
{
    /// <summary>
    /// Compares two integer expressions using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3Bool operator <(Z3Int left, Z3Int right) => left.Lt(right);

    /// <summary>
    /// Compares two integer expressions using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3Bool operator <=(Z3Int left, Z3Int right) => left.Le(right);

    /// <summary>
    /// Compares two integer expressions using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3Bool operator >(Z3Int left, Z3Int right) => left.Gt(right);

    /// <summary>
    /// Compares two integer expressions using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3Bool operator >=(Z3Int left, Z3Int right) => left.Ge(right);

    /// <summary>
    /// Creates a less-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3Bool Lt(Z3Int other) => Context.Lt(this, other);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3Bool Le(Z3Int other) => Context.Le(this, other);

    /// <summary>
    /// Creates a greater-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3Bool Gt(Z3Int other) => Context.Gt(this, other);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3Bool Ge(Z3Int other) => Context.Ge(this, other);
}
