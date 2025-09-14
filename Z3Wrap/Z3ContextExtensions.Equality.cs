using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3BoolExpr Eq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    public static Z3BoolExpr Neq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        return context.Not(context.Eq(left, right));
    }

    // Overloads that use implicit conversions via SetUp scope
    public static Z3BoolExpr Eq(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Eq((Z3Expr)left, right);
    public static Z3BoolExpr Eq(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right) => context.Eq((Z3Expr)left, right);
    public static Z3BoolExpr Eq(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Eq((Z3Expr)left, right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Neq((Z3Expr)left, right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3BoolExpr left, Z3BoolExpr right) => context.Neq((Z3Expr)left, right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Neq((Z3Expr)left, right);
}