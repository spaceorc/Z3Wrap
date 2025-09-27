using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Extension methods for arithmetic expression functions enabling natural mathematical syntax.
/// </summary>
public static class ArithmeticFunctionExprExtensions
{
    /// <summary>
    /// Computes absolute value of arithmetic expression.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="operand">Expression to compute absolute value of.</param>
    /// <returns>Expression representing |operand|.</returns>
    public static T Abs<T>(this T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => operand.Context.Abs(operand);

    /// <summary>
    /// Computes minimum of two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing min(left, right).</returns>
    public static T Min<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Min(left, right);

    /// <summary>
    /// Computes maximum of two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing max(left, right).</returns>
    public static T Max<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Max(left, right);
}