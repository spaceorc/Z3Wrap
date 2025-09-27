using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static class RealContextExtensions
{
    public static RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    public static RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = SafeNativeMethods.Z3MkRealSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
