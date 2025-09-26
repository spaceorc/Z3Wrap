using Spaceorc.Z3Wrap.BoolTheory;

namespace Spaceorc.Z3Wrap.RealTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Real
{
    /// <summary>
    /// Checks equality between this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A boolean expression representing this == other.</returns>
    public Z3Bool Eq(Z3Real other) => Context.Eq(this, other);

    /// <summary>
    /// Checks inequality between this real expression and another real expression.
    /// </summary>
    /// <param name="other">The other real expression to compare with.</param>
    /// <returns>A boolean expression representing this != other.</returns>
    public Z3Bool Neq(Z3Real other) => Context.Neq(this, other);

    /// <summary>
    /// Compares two real expressions for equality using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Z3Real left, Z3Real right) => left.Eq(right);

    /// <summary>
    /// Compares two real expressions for inequality using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Z3Real left, Z3Real right) => left.Neq(right);
}
