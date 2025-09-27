using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides extension methods for Z3Context to create real number expressions and constants.
/// Supports exact rational arithmetic with unlimited precision and type conversions.
/// </summary>
public static class RealContextExtensions
{
    /// <summary>
    /// Creates a real expression from a Real value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">Real value to convert.</param>
    /// <returns>RealExpr representing the Real value.</returns>
    public static RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates a real constant (variable) with the specified name.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="name">Name of the real constant.</param>
    /// <returns>RealExpr representing the real constant.</returns>
    public static RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Converts a real expression to integer by truncating towards zero.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="expr">Real expression to convert.</param>
    /// <returns>IntExpr representing the truncated integer value.</returns>
    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
