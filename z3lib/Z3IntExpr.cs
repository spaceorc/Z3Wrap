namespace z3lib;

public class Z3IntExpr : Z3Expr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right) => left.Context.MkAdd(left, right);
    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right) => left.Context.MkSub(left, right);
    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right) => left.Context.MkMul(left, right);
    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right) => left.Context.MkDiv(left, right);
    public static Z3BoolExpr operator <(Z3IntExpr left, Z3IntExpr right) => left.Context.MkLt(left, right);
    public static Z3BoolExpr operator <=(Z3IntExpr left, Z3IntExpr right) => left.Context.MkLe(left, right);
    public static Z3BoolExpr operator >(Z3IntExpr left, Z3IntExpr right) => left.Context.MkGt(left, right);
    public static Z3BoolExpr operator >=(Z3IntExpr left, Z3IntExpr right) => left.Context.MkGe(left, right);
    
    public Z3IntExpr Add(Z3IntExpr other) => this + other;
    public Z3IntExpr Sub(Z3IntExpr other) => this - other;
    public Z3IntExpr Mul(Z3IntExpr other) => this * other;
    public Z3IntExpr Div(Z3IntExpr other) => this / other;
    public Z3BoolExpr Lt(Z3IntExpr other) => this < other;
    public Z3BoolExpr Le(Z3IntExpr other) => this <= other;
    public Z3BoolExpr Gt(Z3IntExpr other) => this > other;
    public Z3BoolExpr Ge(Z3IntExpr other) => this >= other;
}