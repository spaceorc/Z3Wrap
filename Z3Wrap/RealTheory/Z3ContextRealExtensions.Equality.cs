using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.RealTheory;

public static partial class Z3ContextRealExtensions
{
    /// <summary>
    /// Creates an equality comparison expression between two real expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left real expression.</param>
    /// <param name="right">The right real expression.</param>
    /// <returns>A Z3Bool representing left == right.</returns>
    public static Z3Bool Eq(this Z3Context context, Z3Real left, Z3Real right)
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Creates a not-equal comparison expression between two real expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left real expression.</param>
    /// <param name="right">The right real expression.</param>
    /// <returns>A Z3Bool representing left != right.</returns>
    public static Z3Bool Neq(this Z3Context context, Z3Real left, Z3Real right)
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var notHandle = SafeNativeMethods.Z3MkNot(context.Handle, handle);
        return Z3Expr.Create<Z3Bool>(context, notHandle);
    }
}