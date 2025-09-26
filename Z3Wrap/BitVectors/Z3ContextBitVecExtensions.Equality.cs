using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVectors;

public static partial class Z3ContextBitVecExtensions
{
    /// <summary>
    /// Creates an equality comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the equality comparison.</returns>
    public static Z3BoolExpr Eq<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a not-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the not-equal comparison.</returns>
    public static Z3BoolExpr Neq<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var notHandle = SafeNativeMethods.Z3MkNot(context.Handle, handle);
        return Z3Expr.Create<Z3BoolExpr>(context, notHandle);
    }
}
