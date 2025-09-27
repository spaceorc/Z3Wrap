using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static class IntExprExtensions
{
    public static IntExpr Mod(this IntExpr left, IntExpr right) => left.Context.Mod(left, right);
}
