using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides bit-vector comparison methods for Z3Context.
/// </summary>
public static class BvComparisonContextExtensions
{
    /// <summary>
    /// Creates less-than comparison for bit-vector expressions.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed comparison, false for unsigned.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
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
    /// Creates less-than-or-equal comparison for bit-vector expressions.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed comparison, false for unsigned.</param>
    /// <returns>Boolean expression representing left &lt;= right.</returns>
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
    /// Creates greater-than comparison for bit-vector expressions.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed comparison, false for unsigned.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
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
    /// Creates greater-than-or-equal comparison for bit-vector expressions.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed comparison, false for unsigned.</param>
    /// <returns>Boolean expression representing left &gt;= right.</returns>
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
