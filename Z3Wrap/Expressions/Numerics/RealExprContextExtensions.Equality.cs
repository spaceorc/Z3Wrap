using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class RealExprContextExtensions
{
    /// <summary>
    /// Creates an equality comparison expression between two real expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left real expression.</param>
    /// <param name="right">The right real expression.</param>
    /// <returns>A Z3Bool representing left == right.</returns>
    public static BoolExpr Eq(this Z3Context context, RealExpr left, RealExpr right)
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a not-equal comparison expression between two real expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left real expression.</param>
    /// <param name="right">The right real expression.</param>
    /// <returns>A Z3Bool representing left != right.</returns>
    public static BoolExpr Neq(this Z3Context context, RealExpr left, RealExpr right)
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var notHandle = SafeNativeMethods.Z3MkNot(context.Handle, handle);
        return Z3Expr.Create<BoolExpr>(context, notHandle);
    }
}
