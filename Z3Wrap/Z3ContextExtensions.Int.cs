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

    // Integer arithmetic operations
    public static Z3IntExpr Add(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, 2, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Sub(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(context.Handle, 2, args);
        return Z3IntExpr.Create(context, resultHandle);
    }

    public static Z3IntExpr Mul(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(context.Handle, 2, args);
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

    public static Z3IntExpr Abs(this Z3Context context, Z3IntExpr operand)
    {
        // abs(x) = ite(x >= 0, x, -x)
        var zero = context.Int(0);
        var condition = context.Ge(operand, zero);
        var negated = context.UnaryMinus(operand);
        return context.Ite(condition, operand, negated);
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

    // Min/Max operations for integers
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
        => context.Ite(context.Le(left, right), left, right);

    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
        => context.Ite(context.Ge(left, right), left, right);

    // Integer to Real conversion
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var handle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3RealExpr.Create(context, handle);
    }
}