using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static partial class BvContextExtensions
{
    /// <summary>
    /// Creates a less-than comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the less-than comparison.</returns>
    public static BoolExpr Lt<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the less-than-or-equal comparison.</returns>
    public static BoolExpr Le<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a greater-than comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the greater-than comparison.</returns>
    public static BoolExpr Gt<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
