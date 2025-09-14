using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Boolean expression creation
    public static Z3BoolExpr Bool(this Z3Context context, bool value)
    {
        var handle = value ? NativeMethods.Z3MkTrue(context.Handle) : NativeMethods.Z3MkFalse(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr True(this Z3Context context)
    {
        var handle = NativeMethods.Z3MkTrue(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr False(this Z3Context context)
    {
        var handle = NativeMethods.Z3MkFalse(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var boolSort = NativeMethods.Z3MkBoolSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, boolSort);
        return Z3BoolExpr.Create(context, handle);
    }

    // Boolean operations
    public static Z3BoolExpr And(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAnd(context.Handle, 2, args);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Or(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkOr(context.Handle, 2, args);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Xor(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Not(this Z3Context context, Z3BoolExpr operand)
    {
        var resultHandle = NativeMethods.Z3MkNot(context.Handle, operand.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Implies(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Iff(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right)
    {
        var resultHandle = NativeMethods.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    // If-then-else operation
    public static T Ite<T>(this Z3Context context, Z3BoolExpr condition, T thenExpr, T elseExpr) where T : Z3Expr
    {
        var resultHandle = NativeMethods.Z3MkIte(context.Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return (T)Z3Expr.Create(context, resultHandle);
    }
}