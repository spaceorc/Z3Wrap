using System.Globalization;

namespace z3lib;

public partial class Z3Context
{
    public Z3IntExpr MkInt(int value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        return WrapIntExpr(handle);
    }

    public Z3IntExpr MkIntConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapIntExpr(handle);
    }

    public Z3RealExpr MkReal(double value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString(CultureInfo.InvariantCulture));
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        return WrapRealExpr(handle);
    }

    public Z3RealExpr MkRealConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapRealExpr(handle);
    }

    public Z3BoolExpr MkTrue()
    {
        var handle = NativeMethods.Z3MkTrue(Handle);
        return WrapBoolExpr(handle);
    }

    public Z3BoolExpr MkFalse()
    {
        var handle = NativeMethods.Z3MkFalse(Handle);
        return WrapBoolExpr(handle);
    }

    public Z3BoolExpr MkBoolConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkBoolSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapBoolExpr(handle);
    }

    public Z3IntExpr MkAdd(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr MkAdd(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr MkSub(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr MkSub(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr MkMul(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr MkMul(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr MkDiv(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr MkDiv(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        return WrapRealExpr(resultHandle);
    }

    public Z3BoolExpr MkLt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkLt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkLe(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkLe(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkGt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkGt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkGe(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkGe(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkEq(Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkAnd(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAnd(Handle, 2, args);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkOr(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkOr(Handle, 2, args);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkNot(Z3BoolExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkNot(Handle, expr.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkImplies(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkImplies(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkIff(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkIff(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr MkXor(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkXor(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3IntExpr MkMod(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkMod(Handle, left.Handle, right.Handle);
        return WrapIntExpr(resultHandle);
    }
    
    public T MkIte<T>(Z3BoolExpr condition, T thenExpr, T elseExpr) where T : Z3Expr
    {
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return (T)WrapExpr(resultHandle);
    }

    public Z3Expr MkIte(Z3BoolExpr condition, Z3Expr thenExpr, Z3Expr elseExpr)
    {
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return WrapExpr(resultHandle);
    }

    public Z3IntExpr MkUnaryMinus(Z3IntExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr MkUnaryMinus(Z3RealExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        return WrapRealExpr(resultHandle);
    }

    public Z3Solver MkSolver()
    {
        var solver = new Z3Solver(this, false);
        TrackSolver(solver);
        return solver;
    }

    public Z3Solver MkSimpleSolver()
    {
        var solver = new Z3Solver(this, true);
        TrackSolver(solver);
        return solver;
    }
}