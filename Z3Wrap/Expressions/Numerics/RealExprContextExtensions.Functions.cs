using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class RealExprContextExtensions
{
    /// <summary>
    /// Returns the absolute value of a real expression.
    /// Uses conditional logic to return the operand if non-negative, otherwise returns its negation.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The real expression to get the absolute value of.</param>
    /// <returns>A Z3Real representing the absolute value of the operand.</returns>
    public static RealExpr Abs(this Z3Context context, RealExpr operand) =>
        context.Ite(operand >= 0, operand, -operand);

    /// <summary>
    /// Returns the minimum of two real expressions.
    /// Uses conditional logic to return the smaller of the two values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first real expression to compare.</param>
    /// <param name="right">The second real expression to compare.</param>
    /// <returns>A Z3Real representing the minimum of left and right.</returns>
    public static RealExpr Min(this Z3Context context, RealExpr left, RealExpr right) =>
        context.Ite(left < right, left, right);

    /// <summary>
    /// Returns the maximum of two real expressions.
    /// Uses conditional logic to return the larger of the two values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first real expression to compare.</param>
    /// <param name="right">The second real expression to compare.</param>
    /// <returns>A Z3Real representing the maximum of left and right.</returns>
    public static RealExpr Max(this Z3Context context, RealExpr left, RealExpr right) =>
        context.Ite(left > right, left, right);
}
