using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public static class ArithmeticFunctionExprExtensions
{
    public static T Abs<T>(this T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => operand.Context.Abs(operand);

    public static T Min<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Min(left, right);

    public static T Max<T>(this T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => left.Context.Max(left, right);
}
