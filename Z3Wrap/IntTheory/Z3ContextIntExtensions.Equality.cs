using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.IntTheory;

public static partial class Z3ContextIntExtensions
{
    /// <summary>
    /// Creates an equality comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A Z3Bool representing left == right.</returns>
    public static Z3Bool Eq(this Z3Context context, Z3Int left, Z3Int right)
    {
        var resultHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates an inequality comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A Z3Bool representing left != right.</returns>
    public static Z3Bool Neq(this Z3Context context, Z3Int left, Z3Int right)
    {
        var eqHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }
}
