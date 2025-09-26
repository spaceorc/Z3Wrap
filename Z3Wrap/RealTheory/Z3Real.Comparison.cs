using Spaceorc.Z3Wrap.BoolTheory;

namespace Spaceorc.Z3Wrap.RealTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Real
{
    /// <summary>
    /// Compares two real expressions using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3Bool operator <(Z3Real left, Z3Real right) => left.Lt(right);

    /// <summary>
    /// Compares two real expressions using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3Bool operator <=(Z3Real left, Z3Real right) => left.Le(right);

    /// <summary>
    /// Compares two real expressions using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3Bool operator >(Z3Real left, Z3Real right) => left.Gt(right);

    /// <summary>
    /// Compares two real expressions using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3Bool operator >=(Z3Real left, Z3Real right) => left.Ge(right);

    /// <summary>
    /// Creates a less-than comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3Bool Lt(Z3Real other) => Context.Lt(this, other);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3Bool Le(Z3Real other) => Context.Le(this, other);

    /// <summary>
    /// Creates a greater-than comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3Bool Gt(Z3Real other) => Context.Gt(this, other);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3Bool Ge(Z3Real other) => Context.Ge(this, other);
}
