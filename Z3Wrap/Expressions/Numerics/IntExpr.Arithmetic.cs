using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class IntExpr
{
    /// <summary>
    /// Addition operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left + right.</returns>
    public static IntExpr operator +(IntExpr left, IntExpr right) => left.Add(right);

    /// <summary>
    /// Subtraction operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left - right.</returns>
    public static IntExpr operator -(IntExpr left, IntExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplication operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left * right.</returns>
    public static IntExpr operator *(IntExpr left, IntExpr right) => left.Mul(right);

    /// <summary>
    /// Division operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left / right.</returns>
    public static IntExpr operator /(IntExpr left, IntExpr right) => left.Div(right);

    /// <summary>
    /// Modulo operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left % right.</returns>
    public static IntExpr operator %(IntExpr left, IntExpr right) => left.Mod(right);

    /// <summary>
    /// Unary minus operator for integer expressions.
    /// </summary>
    /// <param name="expr">Expression to negate.</param>
    /// <returns>Expression representing -expr.</returns>
    public static IntExpr operator -(IntExpr expr) => expr.UnaryMinus();

    /// <summary>
    /// Computes modulo of two integer expressions.
    /// </summary>
    /// <param name="other">Right operand.</param>
    /// <returns>Expression representing this % other.</returns>
    public IntExpr Mod(IntExpr other) => Context.Mod(this, other);
}
