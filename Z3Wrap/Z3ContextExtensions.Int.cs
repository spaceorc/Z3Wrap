using System.Numerics;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Integer expression creation
    public static Z3IntExpr Int(this Z3Context context, BigInteger value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var intSort = NativeMethods.Z3MkIntSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, valueStr, intSort);
        return Z3IntExpr.Create(context, handle);
    }

    public static Z3IntExpr IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var intSort = NativeMethods.Z3MkIntSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, intSort);
        return Z3IntExpr.Create(context, handle);
    }

}