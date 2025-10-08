using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides equality comparison methods for Z3Context.
/// </summary>
public static class CommonContextExtensions
{
    /// <summary>
    /// Creates equality expression for any expressions.
    /// </summary>
    /// <typeparam name="T">Expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr Eq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = context.Library.MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates inequality expression for any expressions.
    /// </summary>
    /// <typeparam name="T">Expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr Neq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IExprType<T>
    {
        var eqHandle = context.Library.MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = context.Library.MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Simplifies an expression using Z3's simplification rules.
    /// </summary>
    /// <typeparam name="T">Expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The expression to simplify.</param>
    /// <returns>Simplified expression.</returns>
    public static T Simplify<T>(this Z3Context context, T expr)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = context.Library.Simplify(context.Handle, expr.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
