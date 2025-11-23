namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides real-specific operations for real expressions.
/// </summary>
public static class RealExprExtensions
{
    /// <summary>
    /// Creates power expression for this real expression.
    /// </summary>
    /// <param name="base">The base operand.</param>
    /// <param name="exponent">The exponent operand (int or real).</param>
    /// <returns>Real expression representing base^exponent.</returns>
    /// <remarks>
    /// Z3 support for exponentiation is limited: works best with constant exponents,
    /// may return unknown for variable exponents or complex nonlinear real arithmetic.
    /// Nonlinear real arithmetic is incomplete in Z3.
    /// </remarks>
    public static RealExpr Power(this RealExpr @base, ArithmeticExpr exponent) => @base.Context.Power(@base, exponent);

    /// <summary>
    /// Creates boolean expression checking if this real expression represents an integer.
    /// </summary>
    /// <param name="expr">The real expression to check.</param>
    /// <returns>Boolean expression that is true when expr is an integer value.</returns>
    public static Logic.BoolExpr IsInt(this RealExpr expr) => expr.Context.IsInt(expr);
}
