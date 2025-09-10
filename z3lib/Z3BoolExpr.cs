namespace z3lib;

public class Z3BoolExpr : Z3Expr
{
    internal Z3BoolExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3BoolExpr operator &(Z3BoolExpr left, Z3BoolExpr right) => left.Context.MkAnd(left, right);
    public static Z3BoolExpr operator |(Z3BoolExpr left, Z3BoolExpr right) => left.Context.MkOr(left, right);
    public static Z3BoolExpr operator !(Z3BoolExpr expr) => expr.Context.MkNot(expr);

    public Z3BoolExpr And(Z3BoolExpr other) => this & other;
    public Z3BoolExpr Or(Z3BoolExpr other) => this | other;
    public Z3BoolExpr Not() => !this;
}