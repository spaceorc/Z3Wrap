using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Boolean operations
    public static Z3BoolExpr And(this Z3Context context, params Z3BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = NativeMethods.Z3MkAnd(context.Handle, (uint)args.Length, args);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Or(this Z3Context context, params Z3BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = NativeMethods.Z3MkOr(context.Handle, (uint)args.Length, args);
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
}