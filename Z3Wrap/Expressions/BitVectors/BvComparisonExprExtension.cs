using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvComparisonExprExtension
{
    public static BoolExpr Lt<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Lt(left, right, signed);

    public static BoolExpr Le<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Le(left, right, signed);

    public static BoolExpr Gt<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Gt(left, right, signed);

    public static BoolExpr Ge<TSize>(this BvExpr<TSize> left, BvExpr<TSize> right, bool signed = false)
        where TSize : ISize => left.Context.Ge(left, right, signed);
}
