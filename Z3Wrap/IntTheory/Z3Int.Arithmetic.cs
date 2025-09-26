namespace Spaceorc.Z3Wrap.IntTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Int
{
    /// <summary>
    /// Adds two integer expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left + right.</returns>
    public static Z3Int operator +(Z3Int left, Z3Int right) => left.Add(right);

    /// <summary>
    /// Subtracts two integer expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left - right.</returns>
    public static Z3Int operator -(Z3Int left, Z3Int right) => left.Sub(right);

    /// <summary>
    /// Multiplies two integer expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left * right.</returns>
    public static Z3Int operator *(Z3Int left, Z3Int right) => left.Mul(right);

    /// <summary>
    /// Divides two integer expressions using the / operator (integer division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>An integer expression representing left / right.</returns>
    public static Z3Int operator /(Z3Int left, Z3Int right) => left.Div(right);

    /// <summary>
    /// Computes the modulo of two integer expressions using the % operator.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>An integer expression representing left % right.</returns>
    public static Z3Int operator %(Z3Int left, Z3Int right) => left.Mod(right);

    /// <summary>
    /// Negates an integer expression using the unary - operator.
    /// </summary>
    /// <param name="expr">The integer expression to negate.</param>
    /// <returns>An integer expression representing -expr.</returns>
    public static Z3Int operator -(Z3Int expr) => expr.UnaryMinus();

    /// <summary>
    /// Adds this integer expression to another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to add.</param>
    /// <returns>An integer expression representing this + other.</returns>
    public Z3Int Add(Z3Int other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another integer expression from this integer expression.
    /// </summary>
    /// <param name="other">The integer expression to subtract.</param>
    /// <returns>An integer expression representing this - other.</returns>
    public Z3Int Sub(Z3Int other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this integer expression by another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to multiply by.</param>
    /// <returns>An integer expression representing this * other.</returns>
    public Z3Int Mul(Z3Int other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this integer expression by another integer expression (integer division).
    /// </summary>
    /// <param name="other">The integer expression to divide by.</param>
    /// <returns>An integer expression representing this / other.</returns>
    public Z3Int Div(Z3Int other) => Context.Div(this, other);

    /// <summary>
    /// Computes the modulo of this integer expression with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compute modulo with.</param>
    /// <returns>An integer expression representing this % other.</returns>
    public Z3Int Mod(Z3Int other) => Context.Mod(this, other);

    /// <summary>
    /// Negates this integer expression (computes the unary minus).
    /// </summary>
    /// <returns>An integer expression representing -this.</returns>
    public Z3Int UnaryMinus() => Context.UnaryMinus(this);
}
