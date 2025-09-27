using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class IntExprContextExtensions
{
    /// <summary>
    /// Creates an equality comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A Z3Bool representing left == right.</returns>
    public static BoolExpr Eq(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates an inequality comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left integer expression.</param>
    /// <param name="right">The right integer expression.</param>
    /// <returns>A Z3Bool representing left != right.</returns>
    public static BoolExpr Neq(this Z3Context context, IntExpr left, IntExpr right)
    {
        var eqHandle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
