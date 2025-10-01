using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides integer expression creation and conversion methods for Z3Context.
/// </summary>
public static class IntContextExtensions
{
    /// <summary>
    /// Creates integer expression from BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The integer value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static IntExpr Int(this Z3Context context, BigInteger value)
    {
        var intSort = context.Library.MkIntSort(context.Handle);
        var handle = context.Library.MkNumeral(context.Handle, value.ToString(), intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates integer expression from int value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The integer value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static IntExpr Int(this Z3Context context, int value) => context.Int(new BigInteger(value));

    /// <summary>
    /// Creates integer expression from long value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The integer value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static IntExpr Int(this Z3Context context, long value) => context.Int(new BigInteger(value));

    /// <summary>
    /// Creates integer constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Integer expression constant.</returns>
    public static IntExpr IntConst(this Z3Context context, string name)
    {
        var intSort = context.Library.MkIntSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates real expression from integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <returns>Real expression representing the integer.</returns>
    public static RealExpr ToReal(this Z3Context context, IntExpr expr)
    {
        var handle = context.Library.MkInt2Real(context.Handle, expr.Handle);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector expression from integer expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <returns>Bit-vector expression representing the integer.</returns>
    public static BvExpr<TSize> ToBv<TSize>(this Z3Context context, IntExpr expr)
        where TSize : ISize
    {
        var handle = context.Library.MkInt2Bv(context.Handle, TSize.Size, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates modulo expression for integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>Integer expression representing left mod right.</returns>
    public static IntExpr Mod(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = context.Library.MkMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<IntExpr>(context, resultHandle);
    }
}
