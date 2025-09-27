using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Extension methods for arithmetic expression comparisons enabling natural relational syntax.
/// </summary>
public static class ArithmeticComparisonExprExtensions
{
    /// <summary>
    /// Creates less-than comparison between arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
    public static BoolExpr Lt<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Lt(left, right);

    /// <summary>
    /// Creates less-than-or-equal comparison between arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left ≤ right.</returns>
    public static BoolExpr Le<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Le(left, right);

    /// <summary>
    /// Creates greater-than comparison between arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
    public static BoolExpr Gt<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Gt(left, right);

    /// <summary>
    /// Creates greater-than-or-equal comparison between arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left ≥ right.</returns>
    public static BoolExpr Ge<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Ge(left, right);
}
