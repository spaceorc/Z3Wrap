using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides comparison operations for bit-vector expressions.
/// </summary>
public static class BvComparisonExprExtension
{
    /// <summary>
    /// Creates less-than comparison for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed comparison.</param>
    /// <returns>Boolean expression representing the less-than comparison.</returns>
    public static BoolExpr Lt<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Lt(left, right, signed);

    /// <summary>
    /// Creates less-than-or-equal comparison for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed comparison.</param>
    /// <returns>Boolean expression representing the less-than-or-equal comparison.</returns>
    public static BoolExpr Le<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Le(left, right, signed);

    /// <summary>
    /// Creates greater-than comparison for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed comparison.</param>
    /// <returns>Boolean expression representing the greater-than comparison.</returns>
    public static BoolExpr Gt<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Gt(left, right, signed);

    /// <summary>
    /// Creates greater-than-or-equal comparison for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed comparison.</param>
    /// <returns>Boolean expression representing the greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Ge(left, right, signed);
}
