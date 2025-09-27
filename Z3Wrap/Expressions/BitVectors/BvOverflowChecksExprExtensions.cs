using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvOverflowChecksExprExtensions
{
    public static BoolExpr AddNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.AddNoOverflow(left, right, signed);

    public static BoolExpr SignedSubNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedSubNoOverflow(left, right);

    public static BoolExpr SubNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = true)
        where TSize : ISize => left.Context.SubNoUnderflow(left, right, signed);

    public static BoolExpr MulNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.MulNoOverflow(left, right, signed);

    public static BoolExpr SignedMulNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedMulNoUnderflow(left, right);

    public static BoolExpr SignedAddNoUnderflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedAddNoUnderflow(left, right);

    public static BoolExpr SignedDivNoOverflow<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize => left.Context.SignedDivNoOverflow(left, right);

    public static BoolExpr SignedNegNoOverflow<TSize>(this BvExpr<TSize> operand)
        where TSize : ISize => operand.Context.SignedNegNoOverflow(operand);
}
