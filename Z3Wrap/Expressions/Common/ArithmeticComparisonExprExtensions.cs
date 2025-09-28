using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides arithmetic comparison operations for arithmetic expressions.
/// </summary>
public static class ArithmeticComparisonExprExtensions
{
    /// <summary>
    /// Creates less-than comparison for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than comparison.</returns>
    public static BoolExpr Lt<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Lt(left, right);

    /// <summary>
    /// Creates less-than-or-equal comparison for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the less-than-or-equal comparison.</returns>
    public static BoolExpr Le<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Le(left, right);

    /// <summary>
    /// Creates greater-than comparison for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than comparison.</returns>
    public static BoolExpr Gt<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Gt(left, right);

    /// <summary>
    /// Creates greater-than-or-equal comparison for this arithmetic expression.
    /// </summary>
    /// <typeparam name="T">The arithmetic expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Ge(left, right);
}
