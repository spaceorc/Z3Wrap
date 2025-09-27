using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Extension methods for integer expressions providing integer-specific operations.
/// </summary>
public static class IntExprExtensions
{
    /// <summary>
    /// Converts integer expression to real expression.
    /// </summary>
    /// <param name="expr">Integer expression to convert.</param>
    /// <returns>Real expression representing the integer as rational number.</returns>
    public static RealExpr ToReal(this IntExpr expr) => expr.Context.ToReal(expr);

    /// <summary>
    /// Converts integer expression to bitvector expression.
    /// </summary>
    /// <typeparam name="TSize">Size type determining bit width.</typeparam>
    /// <param name="expr">Integer expression to convert.</param>
    /// <returns>Bitvector expression representing the integer value.</returns>
    public static BvExpr<TSize> ToBitVec<TSize>(this IntExpr expr)
        where TSize : ISize => expr.Context.ToBitVec<TSize>(expr);

    /// <summary>
    /// Computes modulo of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left % right.</returns>
    public static IntExpr Mod(this IntExpr left, IntExpr right) => left.Context.Mod(left, right);
}
