using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides basic arithmetic operation methods for Z3Context.
/// </summary>
public static class ArithmeticOperationsContextExtensions
{
    /// <summary>
    /// Creates addition expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to add.</param>
    /// <returns>Expression representing sum of operands.</returns>
    public static T Add<T>(this Z3Context context, params IEnumerable<T> operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var args = operands.Select(o => o.Handle).ToArray();
        if (args.Length == 0)
            return T.Zero(context);

        var resultHandle = context.Library.MkAdd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Creates subtraction expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to subtract.</param>
    /// <returns>Expression representing sequential subtraction of operands.</returns>
    public static T Sub<T>(this Z3Context context, params IEnumerable<T> operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var args = operands.Select(o => o.Handle).ToArray();
        if (args.Length == 0)
            throw new InvalidOperationException(
                "Sub requires at least one operand. Z3 does not support empty subtraction."
            );

        var resultHandle = context.Library.MkSub(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Creates multiplication expression for multiple arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to multiply.</param>
    /// <returns>Expression representing product of operands.</returns>
    public static T Mul<T>(this Z3Context context, params IEnumerable<T> operands)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var args = operands.Select(o => o.Handle).ToArray();
        if (args.Length == 0)
            return T.One(context);

        var resultHandle = context.Library.MkMul(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Creates division expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>Expression representing left / right.</returns>
    public static T Div<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }

    /// <summary>
    /// Creates unary minus expression for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>Expression representing -operand.</returns>
    public static T UnaryMinus<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.MkUnaryMinus(context.Handle, operand.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
