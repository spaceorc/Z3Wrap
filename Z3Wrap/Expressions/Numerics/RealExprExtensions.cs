namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Extension methods for real expressions.
/// </summary>
public static class RealExprExtensions
{
    /// <summary>
    /// Converts real expression to integer expression.
    /// </summary>
    /// <param name="expr">Real expression to convert.</param>
    /// <returns>Integer expression (truncated towards zero).</returns>
    public static IntExpr ToInt(this RealExpr expr) => expr.Context.ToInt(expr);
}
