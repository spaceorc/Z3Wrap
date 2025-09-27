using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class IntExpr
{
    /// <summary>
    /// Compares two integer expressions using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static BoolExpr operator <(IntExpr left, IntExpr right) => left.Lt(right);

    /// <summary>
    /// Compares two integer expressions using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static BoolExpr operator <=(IntExpr left, IntExpr right) => left.Le(right);

    /// <summary>
    /// Compares two integer expressions using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static BoolExpr operator >(IntExpr left, IntExpr right) => left.Gt(right);

    /// <summary>
    /// Compares two integer expressions using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static BoolExpr operator >=(IntExpr left, IntExpr right) => left.Ge(right);

    /// <summary>
    /// Creates a less-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public BoolExpr Lt(IntExpr other) => Context.Lt(this, other);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public BoolExpr Le(IntExpr other) => Context.Le(this, other);

    /// <summary>
    /// Creates a greater-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public BoolExpr Gt(IntExpr other) => Context.Gt(this, other);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public BoolExpr Ge(IntExpr other) => Context.Ge(this, other);
}
