using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Defines factory methods for Z3 expression types.
/// </summary>
/// <typeparam name="T">The expression type.</typeparam>
public interface IExprType<out T>
    where T : Z3Expr
{
    /// <summary>
    /// Creates an expression instance from a Z3 handle.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="handle">The Z3 expression handle.</param>
    /// <returns>A typed expression instance.</returns>
    internal static abstract T Create(Z3Context context, IntPtr handle);

    /// <summary>
    /// Gets the Z3 sort handle for this expression type.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>The Z3 sort handle.</returns>
    internal static abstract IntPtr Sort(Z3Context context);
}
