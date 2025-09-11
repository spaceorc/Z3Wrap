using System.Globalization;

namespace z3lib;

public partial class Z3Context
{
    public Z3IntExpr Int(int value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        return WrapIntExpr(handle);
    }

    public Z3IntExpr IntConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkIntSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapIntExpr(handle);
    }

    public Z3RealExpr Real(double value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString(CultureInfo.InvariantCulture));
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var handle = NativeMethods.Z3MkNumeral(Handle, numeralPtr, sortHandle);
        return WrapRealExpr(handle);
    }

    public Z3RealExpr RealConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkRealSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapRealExpr(handle);
    }

    public Z3BoolExpr True()
    {
        var handle = NativeMethods.Z3MkTrue(Handle);
        return WrapBoolExpr(handle);
    }

    public Z3BoolExpr False()
    {
        var handle = NativeMethods.Z3MkFalse(Handle);
        return WrapBoolExpr(handle);
    }

    public Z3BoolExpr BoolConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkBoolSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
        return WrapBoolExpr(handle);
    }

    public Z3IntExpr Add(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr Add(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr Sub(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr Sub(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr Mul(Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr Mul(Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(Handle, 2, args);
        return WrapRealExpr(resultHandle);
    }

    public Z3IntExpr Div(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr Div(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(Handle, left.Handle, right.Handle);
        return WrapRealExpr(resultHandle);
    }

    public Z3BoolExpr Lt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Lt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Le(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Le(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Gt(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Gt(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Ge(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Ge(Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Eq(Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr And(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAnd(Handle, 2, args);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Or(Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkOr(Handle, 2, args);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Not(Z3BoolExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkNot(Handle, expr.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Implies(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkImplies(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Iff(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkIff(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3BoolExpr Xor(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkXor(Handle, left.Handle, right.Handle);
        return WrapBoolExpr(resultHandle);
    }

    public Z3IntExpr Mod(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkMod(Handle, left.Handle, right.Handle);
        return WrapIntExpr(resultHandle);
    }
    
    public T Ite<T>(Z3BoolExpr condition, T thenExpr, T elseExpr) where T : Z3Expr
    {
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return (T)WrapExpr(resultHandle);
    }

    public Z3Expr Ite(Z3BoolExpr condition, Z3Expr thenExpr, Z3Expr elseExpr)
    {
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return WrapExpr(resultHandle);
    }

    public Z3IntExpr UnaryMinus(Z3IntExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        return WrapIntExpr(resultHandle);
    }

    public Z3RealExpr UnaryMinus(Z3RealExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        return WrapRealExpr(resultHandle);
    }

    public Z3Solver CreateSolver()
    {
        var solver = new Z3Solver(this, false);
        TrackSolver(solver);
        return solver;
    }

    public Z3Solver CreateSimpleSolver()
    {
        var solver = new Z3Solver(this, true);
        TrackSolver(solver);
        return solver;
    }
}