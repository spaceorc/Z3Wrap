using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
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
    public static T Ite<T>(this Z3Context context, Z3BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr
    {
        var resultHandle = NativeMethods.Z3MkIte(
            context.Handle,
            condition.Handle,
            thenExpr.Handle,
            elseExpr.Handle
        );
        return (T)Z3Expr.Create(context, resultHandle);
    }
}
