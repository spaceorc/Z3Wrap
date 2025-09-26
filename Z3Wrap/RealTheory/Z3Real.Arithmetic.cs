namespace Spaceorc.Z3Wrap.RealTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Real
{
    /// <summary>
    /// Adds two real expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left + right.</returns>
    public static Z3Real operator +(Z3Real left, Z3Real right) => left.Add(right);

    /// <summary>
    /// Subtracts two real expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left - right.</returns>
    public static Z3Real operator -(Z3Real left, Z3Real right) => left.Sub(right);

    /// <summary>
    /// Multiplies two real expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left * right.</returns>
    public static Z3Real operator *(Z3Real left, Z3Real right) => left.Mul(right);

    /// <summary>
    /// Divides two real expressions using the / operator (exact rational division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A real expression representing left / right.</returns>
    public static Z3Real operator /(Z3Real left, Z3Real right) => left.Div(right);

    /// <summary>
    /// Negates a real expression using the unary - operator.
    /// </summary>
    /// <param name="expr">The real expression to negate.</param>
    /// <returns>A real expression representing -expr.</returns>
    public static Z3Real operator -(Z3Real expr) => expr.UnaryMinus();

    /// <summary>
    /// Adds this real expression to another real expression.
    /// </summary>
    /// <param name="other">The real expression to add.</param>
    /// <returns>A real expression representing this + other.</returns>
    public Z3Real Add(Z3Real other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another real expression from this real expression.
    /// </summary>
    /// <param name="other">The real expression to subtract.</param>
    /// <returns>A real expression representing this - other.</returns>
    public Z3Real Sub(Z3Real other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this real expression by another real expression.
    /// </summary>
    /// <param name="other">The real expression to multiply by.</param>
    /// <returns>A real expression representing this * other.</returns>
    public Z3Real Mul(Z3Real other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this real expression by another real expression (exact rational division).
    /// </summary>
    /// <param name="other">The real expression to divide by.</param>
    /// <returns>A real expression representing this / other.</returns>
    public Z3Real Div(Z3Real other) => Context.Div(this, other);

    /// <summary>
    /// Negates this real expression (computes the unary minus).
    /// </summary>
    /// <returns>A real expression representing -this.</returns>
    public Z3Real UnaryMinus() => Context.UnaryMinus(this);
}
