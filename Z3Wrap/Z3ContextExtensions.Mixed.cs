using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Operations that work across different expression types

    // Equality operations (work with any Z3Expr)
    public static Z3BoolExpr Eq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Neq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var eqHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = NativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    // Mixed-type equality operations with primitive values
    public static Z3BoolExpr Eq(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Eq(left, context.Int(right));

    public static Z3BoolExpr Eq(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Eq(context.Int(left), right);

    public static Z3BoolExpr Neq(this Z3Context context, Z3IntExpr left, BigInteger right)
        => context.Neq(left, context.Int(right));

    public static Z3BoolExpr Neq(this Z3Context context, BigInteger left, Z3IntExpr right)
        => context.Neq(context.Int(left), right);

    public static Z3BoolExpr Eq(this Z3Context context, Z3RealExpr left, Real right)
        => context.Eq(left, context.Real(right));

    public static Z3BoolExpr Eq(this Z3Context context, Real left, Z3RealExpr right)
        => context.Eq(context.Real(left), right);

    public static Z3BoolExpr Neq(this Z3Context context, Z3RealExpr left, Real right)
        => context.Neq(left, context.Real(right));

    public static Z3BoolExpr Neq(this Z3Context context, Real left, Z3RealExpr right)
        => context.Neq(context.Real(left), right);
}