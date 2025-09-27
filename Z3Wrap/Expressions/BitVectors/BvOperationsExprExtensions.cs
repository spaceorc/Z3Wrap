using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvOperationsExprExtensions
{
    public static BvExpr<TSize> Add<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Add(left, right);

    public static BvExpr<TSize> Sub<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Sub(left, right);

    public static BvExpr<TSize> Mul<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Mul(left, right);

    public static BvExpr<TSize> Div<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Div(left, right, signed);

    public static BvExpr<TSize> Rem<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Rem(left, right, signed);

    public static BvExpr<TSize> Neg<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.Neg(operand);

    public static BvExpr<TSize> SignedMod<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedMod(left, right);

    public static BvExpr<TSize> And<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.And(left, right);

    public static BvExpr<TSize> Or<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Or(left, right);

    public static BvExpr<TSize> Xor<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.Xor(left, right);

    public static BvExpr<TSize> Not<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.Not(operand);

    public static BvExpr<TSize> Shl<TSize>(this BvExpr<TSize> left, BvExpr<TSize> amount)
        where TSize : ISize => left.Context.Shl(left, amount);

    public static BvExpr<TSize> Shr<TSize>(this BvExpr<TSize> left, BvExpr<TSize> amount, bool signed = false)
        where TSize : ISize => left.Context.Shr(left, amount, signed);
}
