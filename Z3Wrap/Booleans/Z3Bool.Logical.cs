namespace Spaceorc.Z3Wrap.Booleans;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Bool
{
    /// <summary>
    /// Creates a Boolean expression representing the logical AND of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this AND other.</returns>
    public Z3Bool And(Z3Bool other) => Context.And(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical OR of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this OR other.</returns>
    public Z3Bool Or(Z3Bool other) => Context.Or(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical negation of this expression.
    /// </summary>
    /// <returns>A Boolean expression representing NOT this.</returns>
    public Z3Bool Not() => Context.Not(this);

    /// <summary>
    /// Creates a Boolean expression representing logical implication (this implies other).
    /// </summary>
    /// <param name="other">The consequent Boolean expression.</param>
    /// <returns>A Boolean expression representing this → other.</returns>
    public Z3Bool Implies(Z3Bool other) => Context.Implies(this, other);

    /// <summary>
    /// Creates a Boolean expression representing logical equivalence (if and only if).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this ↔ other.</returns>
    public Z3Bool Iff(Z3Bool other) => Context.Iff(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical XOR of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this XOR other.</returns>
    public Z3Bool Xor(Z3Bool other) => Context.Xor(this, other);

    /// <summary>
    /// Performs logical AND operation using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left AND right.</returns>
    public static Z3Bool operator &(Z3Bool left, Z3Bool right) => left.And(right);

    /// <summary>
    /// Performs logical OR operation using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left OR right.</returns>
    public static Z3Bool operator |(Z3Bool left, Z3Bool right) => left.Or(right);

    /// <summary>
    /// Performs logical XOR operation using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left XOR right.</returns>
    public static Z3Bool operator ^(Z3Bool left, Z3Bool right) => left.Xor(right);

    /// <summary>
    /// Performs logical NOT operation using the ! operator.
    /// </summary>
    /// <param name="expr">The Boolean expression to negate.</param>
    /// <returns>A Boolean expression representing NOT expr.</returns>
    public static Z3Bool operator !(Z3Bool expr) => expr.Not();
}
#pragma warning restore CS0660, CS0661