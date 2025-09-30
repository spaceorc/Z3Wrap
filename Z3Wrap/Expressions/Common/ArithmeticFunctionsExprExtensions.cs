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
}
