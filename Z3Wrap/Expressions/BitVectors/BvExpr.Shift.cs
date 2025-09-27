namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public sealed partial class BvExpr<TSize>
{
    /// <summary>
    /// Left-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <returns>A bitvector expression representing this &lt;&lt; amount.</returns>
    public BvExpr<TSize> Shl(BvExpr<TSize> amount) => Context.Shl(this, amount);

    /// <summary>
    /// Right-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <param name="signed">Whether to perform arithmetic (true) or logical (false) shift.</param>
    /// <returns>A bitvector expression representing this &gt;&gt; amount.</returns>
    public BvExpr<TSize> Shr(BvExpr<TSize> amount, bool signed = false) =>
        Context.Shr(this, amount, signed);

    /// <summary>
    /// Left-shifts a bitvector expression using the &lt;&lt; operator.
    /// </summary>
    /// <param name="left">The operand to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &lt;&lt; right.</returns>
    public static BvExpr<TSize> operator <<(BvExpr<TSize> left, BvExpr<TSize> right) =>
        left.Shl(right);

    /// <summary>
    /// Right-shifts a bitvector expression using the &gt;&gt; operator (logical shift).
    /// </summary>
    /// <param name="left">The operand to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &gt;&gt; right.</returns>
    public static BvExpr<TSize> operator >>(BvExpr<TSize> left, BvExpr<TSize> right) =>
        left.Shr(right);
}
