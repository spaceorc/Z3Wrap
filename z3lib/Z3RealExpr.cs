namespace z3lib;

public sealed class Z3RealExpr : Z3Expr
{
    internal Z3RealExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3RealExpr <-> Z3RealExpr operations
    public static Z3RealExpr operator +(Z3RealExpr left, Z3RealExpr right) => left.Context.MkAdd(left, right);
    public static Z3RealExpr operator -(Z3RealExpr left, Z3RealExpr right) => left.Context.MkSub(left, right);
    public static Z3RealExpr operator *(Z3RealExpr left, Z3RealExpr right) => left.Context.MkMul(left, right);
    public static Z3RealExpr operator /(Z3RealExpr left, Z3RealExpr right) => left.Context.MkDiv(left, right);
    public static Z3BoolExpr operator <(Z3RealExpr left, Z3RealExpr right) => left.Context.MkLt(left, right);
    public static Z3BoolExpr operator <=(Z3RealExpr left, Z3RealExpr right) => left.Context.MkLe(left, right);
    public static Z3BoolExpr operator >(Z3RealExpr left, Z3RealExpr right) => left.Context.MkGt(left, right);
    public static Z3BoolExpr operator >=(Z3RealExpr left, Z3RealExpr right) => left.Context.MkGe(left, right);

    // Z3RealExpr <-> double operations
    public static Z3RealExpr operator +(Z3RealExpr left, double right) => left.Context.MkAdd(left, left.Context.MkReal(right));
    public static Z3RealExpr operator -(Z3RealExpr left, double right) => left.Context.MkSub(left, left.Context.MkReal(right));
    public static Z3RealExpr operator *(Z3RealExpr left, double right) => left.Context.MkMul(left, left.Context.MkReal(right));
    public static Z3RealExpr operator /(Z3RealExpr left, double right) => left.Context.MkDiv(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, double right) => left.Context.MkLt(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, double right) => left.Context.MkLe(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, double right) => left.Context.MkGt(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, double right) => left.Context.MkGe(left, left.Context.MkReal(right));

    // double <-> Z3RealExpr operations
    public static Z3RealExpr operator +(double left, Z3RealExpr right) => right.Context.MkAdd(right.Context.MkReal(left), right);
    public static Z3RealExpr operator -(double left, Z3RealExpr right) => right.Context.MkSub(right.Context.MkReal(left), right);
    public static Z3RealExpr operator *(double left, Z3RealExpr right) => right.Context.MkMul(right.Context.MkReal(left), right);
    public static Z3RealExpr operator /(double left, Z3RealExpr right) => right.Context.MkDiv(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator <(double left, Z3RealExpr right) => right.Context.MkLt(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator <=(double left, Z3RealExpr right) => right.Context.MkLe(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator >(double left, Z3RealExpr right) => right.Context.MkGt(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator >=(double left, Z3RealExpr right) => right.Context.MkGe(right.Context.MkReal(left), right);

    // Mixed-type equality operations (double)
    public static Z3BoolExpr operator ==(Z3RealExpr left, double right) => left.Context.MkEq(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, double right) => !(left == right);
    public static Z3BoolExpr operator ==(double left, Z3RealExpr right) => right.Context.MkEq(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator !=(double left, Z3RealExpr right) => !(left == right);

    // Z3RealExpr <-> int operations (for convenience: myRealExpr < 10)
    public static Z3RealExpr operator +(Z3RealExpr left, int right) => left.Context.MkAdd(left, left.Context.MkReal(right));
    public static Z3RealExpr operator -(Z3RealExpr left, int right) => left.Context.MkSub(left, left.Context.MkReal(right));
    public static Z3RealExpr operator *(Z3RealExpr left, int right) => left.Context.MkMul(left, left.Context.MkReal(right));
    public static Z3RealExpr operator /(Z3RealExpr left, int right) => left.Context.MkDiv(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, int right) => left.Context.MkLt(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, int right) => left.Context.MkLe(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, int right) => left.Context.MkGt(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, int right) => left.Context.MkGe(left, left.Context.MkReal(right));

    // int <-> Z3RealExpr operations
    public static Z3RealExpr operator +(int left, Z3RealExpr right) => right.Context.MkAdd(right.Context.MkReal(left), right);
    public static Z3RealExpr operator -(int left, Z3RealExpr right) => right.Context.MkSub(right.Context.MkReal(left), right);
    public static Z3RealExpr operator *(int left, Z3RealExpr right) => right.Context.MkMul(right.Context.MkReal(left), right);
    public static Z3RealExpr operator /(int left, Z3RealExpr right) => right.Context.MkDiv(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator <(int left, Z3RealExpr right) => right.Context.MkLt(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator <=(int left, Z3RealExpr right) => right.Context.MkLe(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator >(int left, Z3RealExpr right) => right.Context.MkGt(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator >=(int left, Z3RealExpr right) => right.Context.MkGe(right.Context.MkReal(left), right);

    // Mixed-type equality operations (int)
    public static Z3BoolExpr operator ==(Z3RealExpr left, int right) => left.Context.MkEq(left, left.Context.MkReal(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, int right) => !(left == right);
    public static Z3BoolExpr operator ==(int left, Z3RealExpr right) => right.Context.MkEq(right.Context.MkReal(left), right);
    public static Z3BoolExpr operator !=(int left, Z3RealExpr right) => !(left == right);
    
    public Z3RealExpr Add(Z3RealExpr other) => this + other;
    public Z3RealExpr Sub(Z3RealExpr other) => this - other;
    public Z3RealExpr Mul(Z3RealExpr other) => this * other;
    public Z3RealExpr Div(Z3RealExpr other) => this / other;
    public Z3BoolExpr Lt(Z3RealExpr other) => this < other;
    public Z3BoolExpr Le(Z3RealExpr other) => this <= other;
    public Z3BoolExpr Gt(Z3RealExpr other) => this > other;
    public Z3BoolExpr Ge(Z3RealExpr other) => this >= other;
}