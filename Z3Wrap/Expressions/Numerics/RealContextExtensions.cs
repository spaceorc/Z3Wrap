using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Extension methods for Z3Context to create real expressions.
/// </summary>
public static class RealContextExtensions
{
    /// <summary>
    /// Creates real expression from Real value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">Real value.</param>
    /// <returns>Real expression.</returns>
    public static RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates real constant with specified name.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="name">Constant name.</param>
    /// <returns>Real constant expression.</returns>
    public static RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Converts real expression to integer expression.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="expr">Real expression to convert.</param>
    /// <returns>Integer expression (truncated towards zero).</returns>
    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
