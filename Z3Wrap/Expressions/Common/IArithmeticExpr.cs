using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public interface IArithmeticExpr<out T> : INumericExpr
    where T : Z3Expr
{
    internal static abstract T Zero(Z3Context context);
}
