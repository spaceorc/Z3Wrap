using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides extension methods for Z3Context to create integer expressions and constants.
/// Supports unlimited precision integer arithmetic and type conversions.
/// </summary>
public static class IntContextExtensions
{
    /// <summary>
    /// Creates an integer expression from a BigInteger value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">BigInteger value to convert.</param>
    /// <returns>IntExpr representing the BigInteger value.</returns>
    public static IntExpr Int(this Z3Context context, BigInteger value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates an integer expression from an int value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">Int value to convert.</param>
    /// <returns>IntExpr representing the int value.</returns>
    public static IntExpr Int(this Z3Context context, int value) =>
        context.Int(new BigInteger(value));

    /// <summary>
    /// Creates an integer expression from a long value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">Long value to convert.</param>
    /// <returns>IntExpr representing the long value.</returns>
    public static IntExpr Int(this Z3Context context, long value) =>
        context.Int(new BigInteger(value));

    /// <summary>
    /// Creates an integer constant (variable) with the specified name.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="name">Name of the integer constant.</param>
    /// <returns>IntExpr representing the integer constant.</returns>
    public static IntExpr IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Converts an integer expression to a real expression.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="expr">Integer expression to convert.</param>
    /// <returns>RealExpr representing the same value.</returns>
    public static RealExpr ToReal(this Z3Context context, IntExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Converts an integer expression to a bitvector with the specified size.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="expr">Integer expression to convert.</param>
    /// <typeparam name="TSize">Size type determining the bit width.</typeparam>
    /// <returns>BvExpr representing the integer value with specified bit width.</returns>
    public static BvExpr<TSize> ToBitVec<TSize>(this Z3Context context, IntExpr expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkInt2Bv(context.Handle, TSize.Size, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
