using System.Runtime.InteropServices;

namespace z3lib;

public class Z3IntExpr : Z3Expr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkAdd(left, right);
    }

    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkSub(left, right);
    }

    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkMul(left, right);
    }

    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkDiv(left, right);
    }

    public static Z3BoolExpr operator <(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkLt(left, right);
    }

    public static Z3BoolExpr operator <=(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkLe(left, right);
    }

    public static Z3BoolExpr operator >(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkGt(left, right);
    }

    public static Z3BoolExpr operator >=(Z3IntExpr left, Z3IntExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        return left.context.MkGe(left, right);
    }
}