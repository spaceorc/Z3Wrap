using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides real number expression creation and conversion methods for Z3Context.
/// </summary>
public static class RealContextExtensions
{
    /// <summary>
    /// Creates real expression from Real value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The real value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static RealExpr Real(this Z3Context context, Real value)
    {
        var realSort = context.Library.MkRealSort(context.Handle);
        var handle = context.Library.MkNumeral(context.Handle, value.ToString(), realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates real constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Real expression constant.</returns>
    public static RealExpr RealConst(this Z3Context context, string name)
    {
        var realSort = context.Library.MkRealSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates integer expression from real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The real expression to convert.</param>
    /// <returns>Integer expression representing the real (truncated).</returns>
    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = context.Library.MkReal2int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates boolean expression checking if real expression represents an integer.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The real expression to check.</param>
    /// <returns>Boolean expression that is true when expr is an integer value.</returns>
    public static Logic.BoolExpr IsInt(this Z3Context context, RealExpr expr)
    {
        var handle = context.Library.MkIsInt(context.Handle, expr.Handle);
        return Z3Expr.Create<Logic.BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates power expression for arithmetic expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="base">The base operand (int or real).</param>
    /// <param name="exponent">The exponent operand (int or real).</param>
    /// <returns>Real expression representing base^exponent.</returns>
    /// <remarks>
    /// Z3's power operation always returns Real sort, even for integer inputs.
    /// Z3 support for exponentiation is limited: works best with constant exponents,
    /// may return unknown for variable exponents or complex nonlinear real arithmetic.
    /// Nonlinear arithmetic is incomplete in Z3.
    /// </remarks>
    public static RealExpr Power(this Z3Context context, ArithmeticExpr @base, ArithmeticExpr exponent)
    {
        var handle = context.Library.MkPower(context.Handle, @base.Handle, exponent.Handle);
        return Z3Expr.Create<RealExpr>(context, handle);
    }
}
