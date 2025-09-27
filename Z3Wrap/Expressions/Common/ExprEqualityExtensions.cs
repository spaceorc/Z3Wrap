using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Extensions;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Extension methods for expression equality operations enabling natural comparison syntax.
/// </summary>
public static class ExprEqualityExtensions
{
    /// <summary>
    /// Creates equality comparison between expressions.
    /// </summary>
    /// <typeparam name="T">Expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr Eq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Eq(left, right);

    /// <summary>
    /// Creates inequality comparison between expressions.
    /// </summary>
    /// <typeparam name="T">Expression type.</typeparam>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr Neq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Neq(left, right);
}
