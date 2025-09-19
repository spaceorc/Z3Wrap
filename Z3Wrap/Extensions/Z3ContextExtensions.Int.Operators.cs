using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    // Integer arithmetic operations
    public static Z3IntExpr Add(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException("Add requires at least one operand. Z3 does not support empty addition.");

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Sub(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException("Sub requires at least one operand. Z3 does not support empty subtraction.");

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = NativeMethods.Z3MkSub(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Mul(this Z3Context context, params Z3IntExpr[] operands)
    {
        if (operands.Length == 0)
            throw new InvalidOperationException("Mul requires at least one operand. Z3 does not support empty multiplication.");

        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = NativeMethods.Z3MkMul(context.Handle, (uint)args.Length, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkMod(context.Handle, left.Handle, right.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr UnaryMinus(this Z3Context context, Z3IntExpr operand)
    {
        var args = new[] { operand.Handle };
        var resultHandle = NativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3IntExpr.Create(context, resultHandle);
    }

    // Integer comparison operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    // Mixed integer operations with BigInteger
    public static Z3IntExpr Add(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Add(left, context.Int(right));

    public static Z3IntExpr Add(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Add(context.Int(left), right);

    public static Z3IntExpr Sub(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Sub(left, context.Int(right));

    public static Z3IntExpr Sub(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Sub(context.Int(left), right);

    public static Z3IntExpr Mul(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Mul(left, context.Int(right));

    public static Z3IntExpr Mul(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Mul(context.Int(left), right);

    public static Z3IntExpr Div(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Div(left, context.Int(right));

    public static Z3IntExpr Div(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Div(context.Int(left), right);

    public static Z3IntExpr Mod(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Mod(left, context.Int(right));

    public static Z3IntExpr Mod(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Mod(context.Int(left), right);

    // Integer comparison operations with BigInteger
    public static Z3BoolExpr Lt(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Lt(left, context.Int(right));

    public static Z3BoolExpr Lt(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Lt(context.Int(left), right);

    public static Z3BoolExpr Le(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Le(left, context.Int(right));

    public static Z3BoolExpr Le(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Le(context.Int(left), right);

    public static Z3BoolExpr Gt(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Gt(left, context.Int(right));

    public static Z3BoolExpr Gt(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Gt(context.Int(left), right);

    public static Z3BoolExpr Ge(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Ge(left, context.Int(right));

    public static Z3BoolExpr Ge(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Ge(context.Int(left), right);
}