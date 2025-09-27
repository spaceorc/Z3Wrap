using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public static class EqualityExprExtensions
{
    public static BoolExpr Eq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Eq(left, right);

    public static BoolExpr Neq<T>(this T left, T right)
        where T : Z3Expr, IExprType<T> => left.Context.Neq(left, right);
}
