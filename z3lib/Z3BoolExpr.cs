namespace z3lib;

public class Z3BoolExpr : Z3Expr
{
    internal Z3BoolExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3BoolExpr operator &(Z3BoolExpr left, Z3BoolExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        

        return left.context.MkAnd(left, right);
    }

    public static Z3BoolExpr operator |(Z3BoolExpr left, Z3BoolExpr right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));
        

        return left.context.MkOr(left, right);
    }

    public static Z3BoolExpr operator !(Z3BoolExpr expr)
    {
        if (expr is null) throw new ArgumentNullException(nameof(expr));

        return expr.context.MkNot(expr);
    }

    public Z3BoolExpr And(Z3BoolExpr other)
    {
        return this & other;
    }

    public Z3BoolExpr Or(Z3BoolExpr other)
    {
        return this | other;
    }

    public Z3BoolExpr Not()
    {
        return !this;
    }
}