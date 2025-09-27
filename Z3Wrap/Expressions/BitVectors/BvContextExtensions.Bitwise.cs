using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static partial class BvContextExtensions
{
    /// <summary>
    /// Performs bitwise AND on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise AND.</returns>
    public static BvExpr<TSize> And<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise OR on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise OR.</returns>
    public static BvExpr<TSize> Or<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise XOR on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise XOR.</returns>
    public static BvExpr<TSize> Xor<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise NOT on a compile-time size-validated bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to negate bitwise.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise NOT.</returns>
    public static BvExpr<TSize> Not<TSize>(this Z3Context context, BvExpr<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
