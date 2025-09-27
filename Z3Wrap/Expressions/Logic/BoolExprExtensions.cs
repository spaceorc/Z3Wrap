using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

public static class BoolExprExtensions
{
    public static BoolExpr And(this BoolExpr left, BoolExpr other) => left.Context.And(left, other);

    public static BoolExpr Or(this BoolExpr left, BoolExpr other) => left.Context.Or(left, other);

    public static BoolExpr Not(this BoolExpr operand) => operand.Context.Not(operand);

    public static BoolExpr Implies(this BoolExpr left, BoolExpr other) => left.Context.Implies(left, other);

    public static BoolExpr Iff(this BoolExpr left, BoolExpr other) => left.Context.Iff(left, other);

    public static BoolExpr Xor(this BoolExpr left, BoolExpr other) => left.Context.Xor(left, other);

    public static T Ite<T>(this BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T> => condition.Context.Ite(condition, thenExpr, elseExpr);
}
