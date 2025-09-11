namespace z3lib;

public sealed class Z3RealExpr : Z3Expr
{
    internal Z3RealExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3RealExpr operator +(Z3RealExpr left, Z3RealExpr right) => left.Context.MkAdd(left, right);
    public static Z3RealExpr operator -(Z3RealExpr left, Z3RealExpr right) => left.Context.MkSub(left, right);
    public static Z3RealExpr operator *(Z3RealExpr left, Z3RealExpr right) => left.Context.MkMul(left, right);
    public static Z3RealExpr operator /(Z3RealExpr left, Z3RealExpr right) => left.Context.MkDiv(left, right);
    public static Z3BoolExpr operator <(Z3RealExpr left, Z3RealExpr right) => left.Context.MkLt(left, right);
    public static Z3BoolExpr operator <=(Z3RealExpr left, Z3RealExpr right) => left.Context.MkLe(left, right);
    public static Z3BoolExpr operator >(Z3RealExpr left, Z3RealExpr right) => left.Context.MkGt(left, right);
    public static Z3BoolExpr operator >=(Z3RealExpr left, Z3RealExpr right) => left.Context.MkGe(left, right);
    
    public Z3RealExpr Add(Z3RealExpr other) => this + other;
    public Z3RealExpr Sub(Z3RealExpr other) => this - other;
    public Z3RealExpr Mul(Z3RealExpr other) => this * other;
    public Z3RealExpr Div(Z3RealExpr other) => this / other;
    public Z3BoolExpr Lt(Z3RealExpr other) => this < other;
    public Z3BoolExpr Le(Z3RealExpr other) => this <= other;
    public Z3BoolExpr Gt(Z3RealExpr other) => this > other;
    public Z3BoolExpr Ge(Z3RealExpr other) => this >= other;
}