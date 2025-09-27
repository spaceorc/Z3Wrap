using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Extensions;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class IntExpr
{
    /// <summary>
    /// Checks equality between two integer expressions using the == operator.
    /// </summary>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(IntExpr left, IntExpr right) => left.Eq(right);

    /// <summary>
    /// Checks inequality between two integer expressions using the != operator.
    /// </summary>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(IntExpr left, IntExpr right) => left.Neq(right);

    /// <summary>
    /// Creates an equality comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this == other.</returns>
    public BoolExpr Eq(IntExpr other) => Context.Eq(this, other);

    /// <summary>
    /// Creates an inequality comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this != other.</returns>
    public BoolExpr Neq(IntExpr other) => Context.Neq(this, other);
}
