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

    /// <summary>
    /// Computes the sum of arithmetic expressions, like LINQ's Sum().
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="operands">The operands to sum.</param>
    /// <returns>Expression representing the sum, or zero if empty.</returns>
    public static T Sum<T>(this IEnumerable<T> operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        return Z3Context.Current.Add(operands);
    }

    /// <summary>
    /// Computes the product of arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="operands">The operands to multiply.</param>
    /// <returns>Expression representing the product, or one if empty.</returns>
    public static T Product<T>(this IEnumerable<T> operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        return Z3Context.Current.Mul(operands);
    }

    /// <summary>
    /// Computes the sum of projected arithmetic expressions, like LINQ's Sum(selector).
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>Expression representing the sum, or zero if empty.</returns>
    public static T Sum<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        return source.Select(selector).Sum();
    }

    /// <summary>
    /// Computes the product of projected arithmetic expressions.
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>Expression representing the product, or one if empty.</returns>
    public static T Product<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        return source.Select(selector).Product();
    }
}
