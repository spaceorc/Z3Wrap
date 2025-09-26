using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.BoolTheory;

public sealed partial class Z3Bool
{
    /// <summary>
    /// Creates a conditional expression (if-then-else) using this Boolean expression as the condition.
    /// </summary>
    /// <typeparam name="T">The type of expressions for the branches (must inherit from Z3Expr).</typeparam>
    /// <param name="thenExpr">The expression to return when this condition is true.</param>
    /// <param name="elseExpr">The expression to return when this condition is false.</param>
    /// <returns>An expression representing: if (this) then thenExpr else elseExpr.</returns>
    public T Ite<T>(T thenExpr, T elseExpr)
        where T : Z3Expr, IZ3ExprType<T> => Context.Ite(this, thenExpr, elseExpr);
}