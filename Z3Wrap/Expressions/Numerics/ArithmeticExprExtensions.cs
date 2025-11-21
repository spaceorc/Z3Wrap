namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Extension methods for working with optimization results on arithmetic expressions.
/// </summary>
public static class ArithmeticExprExtensions
{
    /// <summary>
    /// Converts optimization result to RealExpr (returns self if already Real, converts Int to Real if needed).
    /// </summary>
    /// <param name="expr">The arithmetic expression from optimization.</param>
    /// <returns>A real number expression.</returns>
    public static RealExpr AsRealExpr(this ArithmeticExpr expr)
    {
        return expr switch
        {
            RealExpr real => real,
            IntExpr integer => integer.ToReal(),
            _ => throw new InvalidOperationException(
                $"Expected IntExpr or RealExpr from optimization, but got {expr.GetType().Name}"
            ),
        };
    }

    /// <summary>
    /// Gets optimization result as IntExpr (throws if result is Real with non-integer value).
    /// </summary>
    /// <param name="expr">The arithmetic expression from optimization.</param>
    /// <returns>An integer expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression is RealExpr (cannot convert without loss of precision).</exception>
    public static IntExpr AsIntExpr(this ArithmeticExpr expr)
    {
        if (expr is IntExpr integer)
            return integer;

        if (expr is RealExpr)
            throw new InvalidOperationException(
                "Optimal value is RealExpr, cannot convert to IntExpr without loss of precision. "
                    + "Use AsRealExpr() to work with the actual value, or check IsIntExpr() first."
            );

        throw new InvalidOperationException(
            $"Expected IntExpr or RealExpr from optimization, but got {expr.GetType().Name}"
        );
    }

    /// <summary>
    /// Checks if the optimization result is an integer.
    /// </summary>
    /// <param name="expr">The arithmetic expression from optimization.</param>
    /// <returns>True if the expression is IntExpr; false if it's RealExpr.</returns>
    public static bool IsIntExpr(this ArithmeticExpr expr) => expr is IntExpr;

    /// <summary>
    /// Checks if the optimization result is a real number.
    /// </summary>
    /// <param name="expr">The arithmetic expression from optimization.</param>
    /// <returns>True if the expression is RealExpr; false if it's IntExpr.</returns>
    public static bool IsRealExpr(this ArithmeticExpr expr) => expr is RealExpr;
}
