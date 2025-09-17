using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Core operations that don't belong to specific types
    // -------------------------------------------------
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

    public static Z3BoolExpr Eq<T>(this Z3Context context, T left, T right) where T : Z3Expr => context.Eq((Z3Expr)left, right);
    public static Z3BoolExpr Neq<T>(this Z3Context context, T left, T right) where T : Z3Expr => context.Neq((Z3Expr)left, right);
}