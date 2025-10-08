using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides equality operations for expressions.
/// </summary>
public static class CommonExprExtensions
{
    /// <summary>
    /// Creates equality comparison for this expression.
    /// </summary>
    /// <typeparam name="T">The expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the equality comparison.</returns>
    public static BoolExpr Eq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Eq(left, right);

    /// <summary>
    /// Creates inequality comparison for this expression.
    /// </summary>
    /// <typeparam name="T">The expression type.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the inequality comparison.</returns>
    public static BoolExpr Neq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Neq(left, right);

    /// <summary>
    /// Simplifies this expression using Z3's simplification rules.
    /// </summary>
    /// <typeparam name="T">The expression type.</typeparam>
    /// <param name="expr">The expression to simplify.</param>
    /// <returns>Simplified expression.</returns>
    public static T Simplify<T>(this T expr)
        where T : Z3Expr, IExprType<T> => expr.Context.Simplify(expr);
}
