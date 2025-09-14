using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Real expression creation
    public static Z3RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3RealExpr.Create(context, handle);
    }

    public static Z3RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3RealExpr.Create(context, handle);
    }

    // Real arithmetic operations
    public static Z3RealExpr Add(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkAdd(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Sub(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkSub(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Mul(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var args = new[] { left.Handle, right.Handle };
        var resultHandle = NativeMethods.Z3MkMul(context.Handle, 2, args);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Div(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkDiv(context.Handle, left.Handle, right.Handle);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr UnaryMinus(this Z3Context context, Z3RealExpr operand)
    {
        var resultHandle = NativeMethods.Z3MkUnaryMinus(context.Handle, operand.Handle);
        return Z3RealExpr.Create(context, resultHandle);
    }

    public static Z3RealExpr Abs(this Z3Context context, Z3RealExpr operand)
    {
        // abs(x) = ite(x >= 0, x, -x)
        var zero = context.Real(new Real(0));
        var condition = context.Ge(operand, zero);
        var negated = context.UnaryMinus(operand);
        return context.Ite(condition, operand, negated);
    }

    // Real comparison operations
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var resultHandle = NativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    // Mixed real operations with Real values
    public static Z3RealExpr Add(this Z3Context context, Z3RealExpr left, Real right)
        => context.Add(left, context.Real(right));

    public static Z3RealExpr Add(this Z3Context context, Real left, Z3RealExpr right)
        => context.Add(context.Real(left), right);

    public static Z3RealExpr Sub(this Z3Context context, Z3RealExpr left, Real right)
        => context.Sub(left, context.Real(right));

    public static Z3RealExpr Sub(this Z3Context context, Real left, Z3RealExpr right)
        => context.Sub(context.Real(left), right);

    public static Z3RealExpr Mul(this Z3Context context, Z3RealExpr left, Real right)
        => context.Mul(left, context.Real(right));

    public static Z3RealExpr Mul(this Z3Context context, Real left, Z3RealExpr right)
        => context.Mul(context.Real(left), right);

    public static Z3RealExpr Div(this Z3Context context, Z3RealExpr left, Real right)
        => context.Div(left, context.Real(right));

    public static Z3RealExpr Div(this Z3Context context, Real left, Z3RealExpr right)
        => context.Div(context.Real(left), right);

    // Real comparison operations with Real values
    public static Z3BoolExpr Lt(this Z3Context context, Z3RealExpr left, Real right)
        => context.Lt(left, context.Real(right));

    public static Z3BoolExpr Lt(this Z3Context context, Real left, Z3RealExpr right)
        => context.Lt(context.Real(left), right);

    public static Z3BoolExpr Le(this Z3Context context, Z3RealExpr left, Real right)
        => context.Le(left, context.Real(right));

    public static Z3BoolExpr Le(this Z3Context context, Real left, Z3RealExpr right)
        => context.Le(context.Real(left), right);

    public static Z3BoolExpr Gt(this Z3Context context, Z3RealExpr left, Real right)
        => context.Gt(left, context.Real(right));

    public static Z3BoolExpr Gt(this Z3Context context, Real left, Z3RealExpr right)
        => context.Gt(context.Real(left), right);

    public static Z3BoolExpr Ge(this Z3Context context, Z3RealExpr left, Real right)
        => context.Ge(left, context.Real(right));

    public static Z3BoolExpr Ge(this Z3Context context, Real left, Z3RealExpr right)
        => context.Ge(context.Real(left), right);

    // Min/Max operations for reals
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(context.Le(left, right), left, right);

    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(context.Ge(left, right), left, right);

    // Real to Integer conversion (truncates)
    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var handle = NativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3IntExpr.Create(context, handle);
    }
}