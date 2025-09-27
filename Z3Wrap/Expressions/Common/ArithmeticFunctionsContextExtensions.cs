using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides extension methods for Z3Context to create mathematical functions for arithmetic expressions.
/// Supports functions (Abs, Min, Max) for integer and real expressions.
/// </summary>
public static class ArithmeticFunctionsContextExtensions
{
    /// <summary>
    /// Computes the absolute value of an arithmetic expression (|operand|).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="operand">Arithmetic expression to compute absolute value of.</param>
    /// <returns>Expression representing |operand|.</returns>
    public static T Abs<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> =>
        context.Ite(context.Ge(operand, T.Zero(context)), operand, context.UnaryMinus(operand));

    /// <summary>
    /// Returns the minimum of two arithmetic expressions (min(left, right)).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">First arithmetic expression.</param>
    /// <param name="right">Second arithmetic expression.</param>
    /// <returns>Expression representing min(left, right).</returns>
    public static T Min<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> =>
        context.Ite(context.Lt(left, right), left, right);

    /// <summary>
    /// Returns the maximum of two arithmetic expressions (max(left, right)).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">First arithmetic expression.</param>
    /// <param name="right">Second arithmetic expression.</param>
    /// <returns>Expression representing max(left, right).</returns>
    public static T Max<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> =>
        context.Ite(context.Gt(left, right), left, right);
}
