using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BoolTheory;

public static partial class Z3ContextBoolExtensions
{
    /// <summary>
    /// Creates an if-then-else expression that selects between two expressions based on a condition.
    /// </summary>
    /// <typeparam name="T">The type of expressions to choose between.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="condition">The boolean condition to evaluate.</param>
    /// <param name="thenExpr">The expression to return if condition is true.</param>
    /// <param name="elseExpr">The expression to return if condition is false.</param>
    /// <returns>A new expression of type T representing if condition then thenExpr else elseExpr.</returns>
    public static T Ite<T>(this Z3Context context, Z3Bool condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IZ3ExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkIte(
            context.Handle,
            condition.Handle,
            thenExpr.Handle,
            elseExpr.Handle
        );
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
