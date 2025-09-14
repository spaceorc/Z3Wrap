using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Real expression creation
    public static Z3RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3RealExpr.Create(context, handle);
    }

    public static Z3RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3RealExpr.Create(context, handle);
    }
}