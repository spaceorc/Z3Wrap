using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Returns the absolute value of a real expression.
    /// Uses conditional logic to return the operand if non-negative, otherwise returns its negation.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The real expression to get the absolute value of.</param>
    /// <returns>A Z3RealExpr representing the absolute value of the operand.</returns>
    public static Z3RealExpr Abs(this Z3Context context, Z3RealExpr operand)
        => context.Ite(operand >= 0, operand, -operand);

    /// <summary>
    /// Returns the minimum of two real expressions.
    /// Uses conditional logic to return the smaller of the two values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first real expression to compare.</param>
    /// <param name="right">The second real expression to compare.</param>
    /// <returns>A Z3RealExpr representing the minimum of left and right.</returns>
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(left < right, left, right);

    /// <summary>
    /// Returns the maximum of two real expressions.
    /// Uses conditional logic to return the larger of the two values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first real expression to compare.</param>
    /// <param name="right">The second real expression to compare.</param>
    /// <returns>A Z3RealExpr representing the maximum of left and right.</returns>
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(left > right, left, right);
}