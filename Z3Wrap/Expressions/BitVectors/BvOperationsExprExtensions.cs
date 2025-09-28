using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides arithmetic and bitwise operations for bit-vector expressions.
/// </summary>
public static class BvOperationsExprExtensions
{
    /// <summary>
    /// Creates addition operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the addition.</returns>
    public static BvExpr<TSize> Add<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Add(left, right);

    /// <summary>
    /// Creates subtraction operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the subtraction.</returns>
    public static BvExpr<TSize> Sub<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Sub(left, right);

    /// <summary>
    /// Creates multiplication operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the multiplication.</returns>
    public static BvExpr<TSize> Mul<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Mul(left, right);

    /// <summary>
    /// Creates division operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed division.</param>
    /// <returns>Bit-vector expression representing the division.</returns>
    public static BvExpr<TSize> Div<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Div(left, right, signed);

    /// <summary>
    /// Creates remainder operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">Whether to use signed remainder.</param>
    /// <returns>Bit-vector expression representing the remainder.</returns>
    public static BvExpr<TSize> Rem<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Rem(left, right, signed);

    /// <summary>
    /// Creates negation operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Bit-vector expression representing the negation.</returns>
    public static BvExpr<TSize> Neg<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.Neg(operand);

    /// <summary>
    /// Creates signed modulo operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the signed modulo.</returns>
    public static BvExpr<TSize> SignedMod<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedMod(left, right);

    /// <summary>
    /// Creates bitwise AND operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the bitwise AND.</returns>
    public static BvExpr<TSize> And<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.And(left, right);

    /// <summary>
    /// Creates bitwise OR operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the bitwise OR.</returns>
    public static BvExpr<TSize> Or<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Or(left, right);

    /// <summary>
    /// Creates bitwise XOR operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing the bitwise XOR.</returns>
    public static BvExpr<TSize> Xor<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Xor(left, right);

    /// <summary>
    /// Creates bitwise NOT operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="operand">The operand.</param>
    /// <returns>Bit-vector expression representing the bitwise NOT.</returns>
    public static BvExpr<TSize> Not<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.Not(operand);

    /// <summary>
    /// Creates left shift operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The expression to shift.</param>
    /// <param name="amount">The shift amount.</param>
    /// <returns>Bit-vector expression representing the left shift.</returns>
    public static BvExpr<TSize> Shl<TSize>(this BvExpr<TSize> left, BvExpr<TSize> amount)
        where TSize : ISize => left.Context.Shl(left, amount);

    /// <summary>
    /// Creates right shift operation for this bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size.</typeparam>
    /// <param name="left">The expression to shift.</param>
    /// <param name="amount">The shift amount.</param>
    /// <param name="signed">Whether to use signed right shift.</param>
    /// <returns>Bit-vector expression representing the right shift.</returns>
    public static BvExpr<TSize> Shr<TSize>(this BvExpr<TSize> left, BvExpr<TSize> amount, bool signed = false)
        where TSize : ISize => left.Context.Shr(left, amount, signed);
}
