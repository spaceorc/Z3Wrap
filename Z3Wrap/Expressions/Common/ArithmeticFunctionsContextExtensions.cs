using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides mathematical function methods for Z3Context.
/// </summary>
public static class ArithmeticFunctionsContextExtensions
{
    /// <summary>
    /// Creates absolute value expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The operand.</param>
    /// <returns>Expression representing |operand|.</returns>
    public static T Abs<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var handle = context.Library.MkAbs(context.Handle, operand.Handle);
        return Z3Expr.Create<T>(context, handle);
    }

    /// <summary>
    /// Creates minimum expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Expression representing min(left, right).</returns>
    public static T Min<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => context.Ite(context.Lt(left, right), left, right);

    /// <summary>
    /// Creates maximum expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Expression representing max(left, right).</returns>
    public static T Max<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => context.Ite(context.Gt(left, right), left, right);
}
