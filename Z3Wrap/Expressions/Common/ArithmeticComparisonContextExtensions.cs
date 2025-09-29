using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides arithmetic comparison methods for Z3Context.
/// </summary>
public static class ArithmeticComparisonContextExtensions
{
    /// <summary>
    /// Creates less-than comparison for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
    public static BoolExpr Lt<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates less-than-or-equal comparison for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &lt;= right.</returns>
    public static BoolExpr Le<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates greater-than comparison for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
    public static BoolExpr Gt<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates greater-than-or-equal comparison for arithmetic expressions.
    /// </summary>
    /// <typeparam name="T">Arithmetic expression type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &gt;= right.</returns>
    public static BoolExpr Ge<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = context.Library.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
