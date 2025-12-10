using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides basic arithmetic operations for arithmetic expressions.
/// </summary>
public static class ArithmeticOperationsExprExtensions
{
    /// <summary>
    /// Creates addition operation for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional operands to add.</param>
    /// <returns>Expression representing the addition.</returns>
    public static T Add<T>(this T left, params IEnumerable<T> others)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Add(others.Prepend(left));

    /// <summary>
    /// Creates subtraction operation for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional operands to subtract.</param>
    /// <returns>Expression representing the subtraction.</returns>
    public static T Sub<T>(this T left, params IEnumerable<T> others)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Sub(others.Prepend(left));

    /// <summary>
    /// Creates multiplication operation for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional operands to multiply.</param>
    /// <returns>Expression representing the multiplication.</returns>
    public static T Mul<T>(this T left, params IEnumerable<T> others)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Mul(others.Prepend(left));

    /// <summary>
    /// Creates division operation for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Expression representing the division.</returns>
    public static T Div<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Div(left, right);

    /// <summary>
    /// Creates unary minus operation for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Expression representing the unary minus.</returns>
    public static T UnaryMinus<T>(this T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => operand.Context.UnaryMinus(operand);
}
