using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static partial class BvExprContextExtensions
{
    /// <summary>
    /// Creates an equality comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the equality comparison.</returns>
    public static BoolExpr Eq<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a not-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the not-equal comparison.</returns>
    public static BoolExpr Neq<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var notHandle = SafeNativeMethods.Z3MkNot(context.Handle, handle);
        return Z3Expr.Create<BoolExpr>(context, notHandle);
    }
}
