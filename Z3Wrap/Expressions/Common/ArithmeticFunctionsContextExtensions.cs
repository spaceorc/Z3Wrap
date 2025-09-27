using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public static class ArithmeticFunctionsContextExtensions
{
    public static T Abs<T>(this Z3Context context, T operand)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> =>
        context.Ite(context.Ge(operand, T.Zero(context)), operand, context.UnaryMinus(operand));

    public static T Min<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => context.Ite(context.Lt(left, right), left, right);

    public static T Max<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T> => context.Ite(context.Gt(left, right), left, right);
}
