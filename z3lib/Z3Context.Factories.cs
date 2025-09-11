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

    public Z3BoolExpr MkBoolConst(string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var sortHandle = NativeMethods.Z3MkBoolSort(Handle);
        var symbolHandle = NativeMethods.Z3MkStringSymbol(Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(Handle, symbolHandle, sortHandle);
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

    // Extended boolean operations
    public Z3BoolExpr MkImplies(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkImplies(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkIff(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkIff(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    public Z3BoolExpr MkXor(Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkXor(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3BoolExpr(this, resultHandle);
    }

    // Extended arithmetic operations
    public Z3IntExpr MkMod(Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkMod(Handle, left.Handle, right.Handle);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3IntExpr MkAbs(Z3IntExpr expr)
    {
        var zero = MkInt(0);
        var negExpr = MkUnaryMinus(expr);
        
        var condition = MkGe(expr, zero);
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, expr.Handle, negExpr.Handle);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkAbs(Z3RealExpr expr)
    {
        var zero = MkReal(0.0);
        var negExpr = MkUnaryMinus(expr);
        
        var condition = MkGe(expr, zero);
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, expr.Handle, negExpr.Handle);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }
    
    // If-Then-Else operation
    public Z3Expr MkIte(Z3BoolExpr condition, Z3Expr thenExpr, Z3Expr elseExpr)
    {
        var resultHandle = NativeMethods.Z3MkIte(Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        TrackExpression(resultHandle);
        
        // Return the appropriate type based on the then/else expressions
        // In Z3, if-then-else preserves the type if both branches have the same type
        return thenExpr switch
        {
            Z3IntExpr when elseExpr is Z3IntExpr => new Z3IntExpr(this, resultHandle),
            Z3RealExpr when elseExpr is Z3RealExpr => new Z3RealExpr(this, resultHandle),  
            Z3BoolExpr when elseExpr is Z3BoolExpr => new Z3BoolExpr(this, resultHandle),
            _ => throw new ArgumentException($"Both then and else expressions must have the same type. Got {thenExpr.GetType().Name} and {elseExpr.GetType().Name}")
        };
    }

    public Z3IntExpr MkUnaryMinus(Z3IntExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        TrackExpression(resultHandle);
        return new Z3IntExpr(this, resultHandle);
    }

    public Z3RealExpr MkUnaryMinus(Z3RealExpr expr)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(Handle, expr.Handle);
        TrackExpression(resultHandle);
        return new Z3RealExpr(this, resultHandle);
    }

    // Solver factory methods
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