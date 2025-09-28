using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides overflow and underflow check operations for bit-vector expressions.
/// </summary>
public static class BvOverflowChecksExprExtensions
{
    /// <summary>
    /// Creates addition overflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed overflow check.</param>
    /// <returns>Boolean expression representing the no-overflow condition.</returns>
    public static BoolExpr AddNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.AddNoOverflow(left, right, signed);

    /// <summary>
    /// Creates signed subtraction overflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the no-overflow condition.</returns>
    public static BoolExpr SignedSubNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedSubNoOverflow(left, right);

    /// <summary>
    /// Creates subtraction underflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed underflow check.</param>
    /// <returns>Boolean expression representing the no-underflow condition.</returns>
    public static BoolExpr SubNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = true)
        where TSize : ISize => left.Context.SubNoUnderflow(left, right, signed);

    /// <summary>
    /// Creates multiplication overflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed overflow check.</param>
    /// <returns>Boolean expression representing the no-overflow condition.</returns>
    public static BoolExpr MulNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.MulNoOverflow(left, right, signed);

    /// <summary>
    /// Creates signed multiplication underflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the no-underflow condition.</returns>
    public static BoolExpr SignedMulNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedMulNoUnderflow(left, right);

    /// <summary>
    /// Creates signed addition underflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the no-underflow condition.</returns>
    public static BoolExpr SignedAddNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedAddNoUnderflow(left, right);

    /// <summary>
    /// Creates signed division overflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing the no-overflow condition.</returns>
    public static BoolExpr SignedDivNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedDivNoOverflow(left, right);

    /// <summary>
    /// Creates signed negation overflow check for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Boolean expression representing the no-overflow condition.</returns>
    public static BoolExpr SignedNegNoOverflow<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.SignedNegNoOverflow(operand);
}
