using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BoolTheory;

/// <summary>
/// Provides extension methods for Z3Context to work with boolean equality operations.
/// </summary>
public static partial class Z3ContextBoolExtensions
{
    /// <summary>
    /// Creates a Boolean expression representing equality between two Boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left Boolean expression.</param>
    /// <param name="right">The right Boolean expression.</param>
    /// <returns>A Boolean expression representing left == right.</returns>
    public static Z3Bool Eq(this Z3Context context, Z3Bool left, Z3Bool right)
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Creates a Boolean expression representing inequality between two Boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left Boolean expression.</param>
    /// <param name="right">The right Boolean expression.</param>
    /// <returns>A Boolean expression representing left != right.</returns>
    public static Z3Bool Neq(this Z3Context context, Z3Bool left, Z3Bool right)
    {
        var eqHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var handle = SafeNativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }
}