using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static partial class BvContextExtensions
{
    /// <summary>
    /// Performs left shift operation on compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the left shift.</returns>
    public static BvExpr<TSize> Shl<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs right shift operation on compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <param name="signed">If true, performs arithmetic right shift; otherwise logical right shift.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the right shift.</returns>
    public static BvExpr<TSize> Shr<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvAShr(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvLShr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
