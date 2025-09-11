namespace z3lib;

public sealed class Z3IntExpr : Z3Expr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3IntExpr <-> Z3IntExpr operations
    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right) => left.Context.MkAdd(left, right);
    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right) => left.Context.MkSub(left, right);
    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right) => left.Context.MkMul(left, right);
    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right) => left.Context.MkDiv(left, right);
    public static Z3BoolExpr operator <(Z3IntExpr left, Z3IntExpr right) => left.Context.MkLt(left, right);
    public static Z3BoolExpr operator <=(Z3IntExpr left, Z3IntExpr right) => left.Context.MkLe(left, right);
    public static Z3BoolExpr operator >(Z3IntExpr left, Z3IntExpr right) => left.Context.MkGt(left, right);
    public static Z3BoolExpr operator >=(Z3IntExpr left, Z3IntExpr right) => left.Context.MkGe(left, right);

    // Z3IntExpr <-> int operations  
    public static Z3IntExpr operator +(Z3IntExpr left, int right) => left.Context.MkAdd(left, left.Context.MkInt(right));
    public static Z3IntExpr operator -(Z3IntExpr left, int right) => left.Context.MkSub(left, left.Context.MkInt(right));
    public static Z3IntExpr operator *(Z3IntExpr left, int right) => left.Context.MkMul(left, left.Context.MkInt(right));
    public static Z3IntExpr operator /(Z3IntExpr left, int right) => left.Context.MkDiv(left, left.Context.MkInt(right));
    public static Z3BoolExpr operator <(Z3IntExpr left, int right) => left.Context.MkLt(left, left.Context.MkInt(right));
    public static Z3BoolExpr operator <=(Z3IntExpr left, int right) => left.Context.MkLe(left, left.Context.MkInt(right));
    public static Z3BoolExpr operator >(Z3IntExpr left, int right) => left.Context.MkGt(left, left.Context.MkInt(right));
    public static Z3BoolExpr operator >=(Z3IntExpr left, int right) => left.Context.MkGe(left, left.Context.MkInt(right));

    // int <-> Z3IntExpr operations
    public static Z3IntExpr operator +(int left, Z3IntExpr right) => right.Context.MkAdd(right.Context.MkInt(left), right);
    public static Z3IntExpr operator -(int left, Z3IntExpr right) => right.Context.MkSub(right.Context.MkInt(left), right);
    public static Z3IntExpr operator *(int left, Z3IntExpr right) => right.Context.MkMul(right.Context.MkInt(left), right);
    public static Z3IntExpr operator /(int left, Z3IntExpr right) => right.Context.MkDiv(right.Context.MkInt(left), right);
    public static Z3BoolExpr operator <(int left, Z3IntExpr right) => right.Context.MkLt(right.Context.MkInt(left), right);
    public static Z3BoolExpr operator <=(int left, Z3IntExpr right) => right.Context.MkLe(right.Context.MkInt(left), right);
    public static Z3BoolExpr operator >(int left, Z3IntExpr right) => right.Context.MkGt(right.Context.MkInt(left), right);
    public static Z3BoolExpr operator >=(int left, Z3IntExpr right) => right.Context.MkGe(right.Context.MkInt(left), right);

    // Mixed-type equality operations
    public static Z3BoolExpr operator ==(Z3IntExpr left, int right) => left.Context.MkEq(left, left.Context.MkInt(right));
    public static Z3BoolExpr operator !=(Z3IntExpr left, int right) => !(left == right);
    public static Z3BoolExpr operator ==(int left, Z3IntExpr right) => right.Context.MkEq(right.Context.MkInt(left), right);
    public static Z3BoolExpr operator !=(int left, Z3IntExpr right) => !(left == right);
    
    public Z3IntExpr Add(Z3IntExpr other) => this + other;
    public Z3IntExpr Sub(Z3IntExpr other) => this - other;
    public Z3IntExpr Mul(Z3IntExpr other) => this * other;
    public Z3IntExpr Div(Z3IntExpr other) => this / other;
    public Z3BoolExpr Lt(Z3IntExpr other) => this < other;
    public Z3BoolExpr Le(Z3IntExpr other) => this <= other;
    public Z3BoolExpr Gt(Z3IntExpr other) => this > other;
    public Z3BoolExpr Ge(Z3IntExpr other) => this >= other;
}