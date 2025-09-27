using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class RealExpr
{
    /// <summary>
    /// Addition operator for real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left + right.</returns>
    public static RealExpr operator +(RealExpr left, RealExpr right) => left.Add(right);

    /// <summary>
    /// Subtraction operator for real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left - right.</returns>
    public static RealExpr operator -(RealExpr left, RealExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplication operator for real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left * right.</returns>
    public static RealExpr operator *(RealExpr left, RealExpr right) => left.Mul(right);

    /// <summary>
    /// Division operator for real expressions (exact rational division).
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left / right.</returns>
    public static RealExpr operator /(RealExpr left, RealExpr right) => left.Div(right);

    /// <summary>
    /// Unary minus operator for real expressions.
    /// </summary>
    /// <param name="expr">Expression to negate.</param>
    /// <returns>Expression representing -expr.</returns>
    public static RealExpr operator -(RealExpr expr) => expr.UnaryMinus();
}
