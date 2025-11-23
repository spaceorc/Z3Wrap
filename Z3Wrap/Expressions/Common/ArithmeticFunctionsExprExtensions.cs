using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides mathematical function operations for arithmetic expressions.
/// </summary>
public static class ArithmeticFunctionExprExtensions
{
    /// <summary>
    /// Creates absolute value function for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Expression representing the absolute value.</returns>
    public static T Abs<T>(this T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => operand.Context.Abs(operand);

    /// <summary>
    /// Creates minimum expression for this arithmetic expression and another.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Expression representing min(left, right).</returns>
    public static T Min<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Min(left, right);

    /// <summary>
    /// Creates maximum expression for this arithmetic expression and another.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Expression representing max(left, right).</returns>
    public static T Max<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Max(left, right);
}
