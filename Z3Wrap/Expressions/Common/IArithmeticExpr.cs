using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Marker interface for expressions supporting basic arithmetic operations (Add, Sub, Mul, Div, UnaryMinus).
/// Implemented by numeric types that support standard mathematical operations in Z3.
/// </summary>
public interface IArithmeticExpr<out T> : INumericExpr
    where T : Z3Expr
{
    internal static abstract T Zero(Z3Context context);
}
