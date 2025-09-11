using System.Globalization;

namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3IntExpr Int(this Z3Context context, int value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sortHandle = NativeMethods.Z3MkIntSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sortHandle);
        return context.WrapIntExpr(handle);
    }

    public static Z3IntExpr IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkIntSort(context.Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbolHandle, sortHandle);
        return context.WrapIntExpr(handle);
    }

    public static Z3RealExpr Real(this Z3Context context, double value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString(CultureInfo.InvariantCulture));
        var sortHandle = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sortHandle);
        return context.WrapRealExpr(handle);
    }

    public static Z3RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkRealSort(context.Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbolHandle, sortHandle);
        return context.WrapRealExpr(handle);
    }

    public static Z3BoolExpr True(this Z3Context context)
    {
        var handle = NativeMethods.Z3MkTrue(context.Handle);
        return context.WrapBoolExpr(handle);
    }

    public static Z3BoolExpr False(this Z3Context context)
    {
        var handle = NativeMethods.Z3MkFalse(context.Handle);
        return context.WrapBoolExpr(handle);
    }

    public static Z3BoolExpr Bool(this Z3Context context, bool value)
    {
        return value ? context.True() : context.False();
    }

    public static Z3BoolExpr BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkBoolSort(context.Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbolHandle, sortHandle);
        return context.WrapBoolExpr(handle);
    }
}