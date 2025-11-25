using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Extension methods for floating-point predicate operations.
/// </summary>
public static class FpPredicateExprExtensions
{
    /// <summary>
    /// Check if floating-point value is NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsNaN.</returns>
    public static BoolExpr IsNaN<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsNan(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is infinite.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsInfinite.</returns>
    public static BoolExpr IsInfinite<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsInfinite(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is zero (positive or negative).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsZero.</returns>
    public static BoolExpr IsZero<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsZero(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is normal (not zero, subnormal, infinite, or NaN).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsNormal.</returns>
    public static BoolExpr IsNormal<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsNormal(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is subnormal (denormalized).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsSubnormal.</returns>
    public static BoolExpr IsSubnormal<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsSubnormal(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is negative (including -0 and -Infinity).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsNegative.</returns>
    public static BoolExpr IsNegative<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsNegative(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Check if floating-point value is positive (including +0 and +Infinity).
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to check.</param>
    /// <returns>Boolean expression representing IsPositive.</returns>
    public static BoolExpr IsPositive<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaIsPositive(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<BoolExpr>(expr.Context, handle);
    }
}
