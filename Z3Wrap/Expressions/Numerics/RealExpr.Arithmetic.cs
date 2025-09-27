namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class RealExpr
{
    /// <summary>
    /// Adds two real expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left + right.</returns>
    public static RealExpr operator +(RealExpr left, RealExpr right) => left.Add(right);

    /// <summary>
    /// Subtracts two real expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left - right.</returns>
    public static RealExpr operator -(RealExpr left, RealExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplies two real expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left * right.</returns>
    public static RealExpr operator *(RealExpr left, RealExpr right) => left.Mul(right);

    /// <summary>
    /// Divides two real expressions using the / operator (exact rational division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A real expression representing left / right.</returns>
    public static RealExpr operator /(RealExpr left, RealExpr right) => left.Div(right);

    /// <summary>
    /// Negates a real expression using the unary - operator.
    /// </summary>
    /// <param name="expr">The real expression to negate.</param>
    /// <returns>A real expression representing -expr.</returns>
    public static RealExpr operator -(RealExpr expr) => expr.UnaryMinus();

    /// <summary>
    /// Adds this real expression to another real expression.
    /// </summary>
    /// <param name="other">The real expression to add.</param>
    /// <returns>A real expression representing this + other.</returns>
    public RealExpr Add(RealExpr other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another real expression from this real expression.
    /// </summary>
    /// <param name="other">The real expression to subtract.</param>
    /// <returns>A real expression representing this - other.</returns>
    public RealExpr Sub(RealExpr other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this real expression by another real expression.
    /// </summary>
    /// <param name="other">The real expression to multiply by.</param>
    /// <returns>A real expression representing this * other.</returns>
    public RealExpr Mul(RealExpr other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this real expression by another real expression (exact rational division).
    /// </summary>
    /// <param name="other">The real expression to divide by.</param>
    /// <returns>A real expression representing this / other.</returns>
    public RealExpr Div(RealExpr other) => Context.Div(this, other);

    /// <summary>
    /// Negates this real expression (computes the unary minus).
    /// </summary>
    /// <returns>A real expression representing -this.</returns>
    public RealExpr UnaryMinus() => Context.UnaryMinus(this);
}
