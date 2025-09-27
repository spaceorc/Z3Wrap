using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class IntExprContextExtensions
{
    /// <summary>
    /// Computes the absolute value of an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The integer expression to compute the absolute value of.</param>
    /// <returns>An integer expression representing |operand|.</returns>
    public static IntExpr Abs(this Z3Context context, IntExpr operand) =>
        context.Ite(operand >= 0, operand, -operand);

    /// <summary>
    /// Returns the minimum of two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first integer expression.</param>
    /// <param name="right">The second integer expression.</param>
    /// <returns>An integer expression representing min(left, right).</returns>
    public static IntExpr Min(this Z3Context context, IntExpr left, IntExpr right) =>
        context.Ite(left < right, left, right);

    /// <summary>
    /// Returns the maximum of two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The first integer expression.</param>
    /// <param name="right">The second integer expression.</param>
    /// <returns>An integer expression representing max(left, right).</returns>
    public static IntExpr Max(this Z3Context context, IntExpr left, IntExpr right) =>
        context.Ite(left > right, left, right);
}
