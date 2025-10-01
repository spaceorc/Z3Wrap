using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Provides real number expression creation and conversion methods for Z3Context.
/// </summary>
public static class RealContextExtensions
{
    /// <summary>
    /// Creates real expression from Real value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The real value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static RealExpr Real(this Z3Context context, Real value)
    {
        var realSort = context.Library.MkRealSort(context.Handle);
        var handle = context.Library.MkNumeral(context.Handle, value.ToString(), realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates real constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Real expression constant.</returns>
    public static RealExpr RealConst(this Z3Context context, string name)
    {
        var realSort = context.Library.MkRealSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Creates integer expression from real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The real expression to convert.</param>
    /// <returns>Integer expression representing the real (truncated).</returns>
    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = context.Library.MkReal2Int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
