using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Defines arithmetic operations for numeric expression types.
/// </summary>
/// <typeparam name="T">The arithmetic expression type.</typeparam>
public interface IArithmeticExpr<out T> : INumericExpr
    where T : Z3Expr
{
    /// <summary>
    /// Creates a zero constant of this arithmetic type.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A zero constant expression.</returns>
    internal static abstract T Zero(Z3Context context);
}
