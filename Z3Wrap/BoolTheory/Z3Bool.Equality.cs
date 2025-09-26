namespace Spaceorc.Z3Wrap.BoolTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Bool
{
    /// <summary>
    /// Creates a Boolean expression representing equality with a bool value.
    /// </summary>
    /// <param name="other">The bool value to compare with.</param>
    /// <returns>A Boolean expression representing this == other.</returns>
    public Z3Bool Eq(bool other) => Context.Eq(this, other);

    /// <summary>
    /// Creates a Boolean expression representing inequality with a bool value.
    /// </summary>
    /// <param name="other">The bool value to compare with.</param>
    /// <returns>A Boolean expression representing this != other.</returns>
    public Z3Bool Neq(bool other) => Context.Neq(this, other);

    /// <summary>
    /// Creates a Boolean expression representing equality between two Boolean expressions.
    /// </summary>
    /// <param name="left">The left Boolean expression.</param>
    /// <param name="right">The right Boolean expression.</param>
    /// <returns>A Boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Z3Bool left, Z3Bool right) => left.Eq(right);

    /// <summary>
    /// Creates a Boolean expression representing inequality between two Boolean expressions.
    /// </summary>
    /// <param name="left">The left Boolean expression.</param>
    /// <param name="right">The right Boolean expression.</param>
    /// <returns>A Boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Z3Bool left, Z3Bool right) => left.Neq(right);
}
