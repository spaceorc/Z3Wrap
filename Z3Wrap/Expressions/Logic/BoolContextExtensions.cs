using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

public static class BoolContextExtensions
{
    public static BoolExpr Bool(this Z3Context context, bool value)
    {
        var handle = value ? SafeNativeMethods.Z3MkTrue(context.Handle) : SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr True(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkTrue(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr False(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var boolSort = SafeNativeMethods.Z3MkBoolSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, boolSort);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr And(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAnd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Or(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkOr(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Xor(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Not(this Z3Context context, BoolExpr operand)
    {
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, operand.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Implies(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static BoolExpr Iff(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    public static T Ite<T>(this Z3Context context, BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkIte(
            context.Handle,
            condition.Handle,
            thenExpr.Handle,
            elseExpr.Handle
        );
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
