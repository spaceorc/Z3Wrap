using System.Globalization;

namespace z3lib;

public partial class Z3Context
{
    // Integer factory methods
    public Z3IntExpr MkInt(int value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        TrackExpression(handle);
        return new Z3IntExpr(this, handle);
    }

    public Z3IntExpr MkIntConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        TrackExpression(handle);
        return new Z3IntExpr(this, handle);
    }

    // Real factory methods
    public Z3RealExpr MkReal(double value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString(CultureInfo.InvariantCulture));
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        TrackExpression(handle);
        return new Z3RealExpr(this, handle);
    }

    public Z3RealExpr MkRealConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        TrackExpression(handle);
        return new Z3RealExpr(this, handle);
    }

    // Boolean factory methods
    public Z3BoolExpr MkTrue()
    {
        var handle = NativeMethods.Z3MkTrue(Handle);
        TrackExpression(handle);
        return new Z3BoolExpr(this, handle);
    }

    public Z3BoolExpr MkFalse()
    {
        var handle = NativeMethods.Z3MkFalse(Handle);
        TrackExpression(handle);
        return new Z3BoolExpr(this, handle);
    }

    // Arithmetic operators
    public Z3IntExpr MkAdd(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkAdd(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }

    public Z3IntExpr MkSub(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkSub(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }

    public Z3IntExpr MkMul(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkMul(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }

    public Z3IntExpr MkDiv(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkDiv(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }

    // Comparison operators
    public Z3BoolExpr MkLt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkLt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkLe(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkLe(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkGt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkGt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkGe(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkGe(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkEq(Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    // Boolean operators
    public Z3BoolExpr MkAnd(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAnd(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkOr(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkOr(Handle, 2, args);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkNot(Z3BoolExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkNot(Handle, expr.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }
}