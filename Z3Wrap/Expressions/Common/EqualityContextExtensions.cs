using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides equality comparison methods for Z3Context.
/// </summary>
public static class EqualityContextExtensions
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
        var resultHandle = context.Library.Z3MkEq(context.Handle, left.Handle, right.Handle);
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
        var eqHandle = context.Library.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = context.Library.Z3MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
