using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Extension methods for arithmetic expressions enabling natural mathematical syntax.
/// </summary>
public static class ArithmeticOperationsExprExtensions
{
    /// <summary>
    /// Adds two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left + right.</returns>
    public static T Add<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Add(left, right);

    /// <summary>
    /// Subtracts two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left - right.</returns>
    public static T Sub<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Sub(left, right);

    /// <summary>
    /// Multiplies two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left * right.</returns>
    public static T Mul<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Mul(left, right);

    /// <summary>
    /// Divides two arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left / right.</returns>
    public static T Div<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => left.Context.Div(left, right);

    /// <summary>
    /// Negates an arithmetic expression.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="operand">Expression to negate.</param>
    /// <returns>Expression representing -operand.</returns>
    public static T UnaryMinus<T>(this T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
        => operand.Context.UnaryMinus(operand);
}