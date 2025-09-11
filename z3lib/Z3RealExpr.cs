namespace z3lib;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3RealExpr : Z3Expr
{
    internal Z3RealExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3RealExpr <-> Z3RealExpr operations
    public static Z3RealExpr operator +(Z3RealExpr left, Z3RealExpr right) => left.Add(right);
    public static Z3RealExpr operator -(Z3RealExpr left, Z3RealExpr right) => left.Sub(right);
    public static Z3RealExpr operator *(Z3RealExpr left, Z3RealExpr right) => left.Mul(right);
    public static Z3RealExpr operator /(Z3RealExpr left, Z3RealExpr right) => left.Div(right);
    public static Z3RealExpr operator -(Z3RealExpr expr) => expr.UnaryMinus();
    public static Z3BoolExpr operator <(Z3RealExpr left, Z3RealExpr right) => left.Lt(right);
    public static Z3BoolExpr operator <=(Z3RealExpr left, Z3RealExpr right) => left.Le(right);
    public static Z3BoolExpr operator >(Z3RealExpr left, Z3RealExpr right) => left.Gt(right);
    public static Z3BoolExpr operator >=(Z3RealExpr left, Z3RealExpr right) => left.Ge(right);

    // Z3RealExpr <-> double operations
    public static Z3RealExpr operator +(Z3RealExpr left, double right) => left.Add(left.Context.MkReal(right));
    public static Z3RealExpr operator -(Z3RealExpr left, double right) => left.Sub(left.Context.MkReal(right));
    public static Z3RealExpr operator *(Z3RealExpr left, double right) => left.Mul(left.Context.MkReal(right));
    public static Z3RealExpr operator /(Z3RealExpr left, double right) => left.Div(left.Context.MkReal(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, double right) => left.Lt(left.Context.MkReal(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, double right) => left.Le(left.Context.MkReal(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, double right) => left.Gt(left.Context.MkReal(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, double right) => left.Ge(left.Context.MkReal(right));

    // double <-> Z3RealExpr operations
    public static Z3RealExpr operator +(double left, Z3RealExpr right) => right.Context.MkReal(left).Add(right);
    public static Z3RealExpr operator -(double left, Z3RealExpr right) => right.Context.MkReal(left).Sub(right);
    public static Z3RealExpr operator *(double left, Z3RealExpr right) => right.Context.MkReal(left).Mul(right);
    public static Z3RealExpr operator /(double left, Z3RealExpr right) => right.Context.MkReal(left).Div(right);
    public static Z3BoolExpr operator <(double left, Z3RealExpr right) => right.Context.MkReal(left).Lt(right);
    public static Z3BoolExpr operator <=(double left, Z3RealExpr right) => right.Context.MkReal(left).Le(right);
    public static Z3BoolExpr operator >(double left, Z3RealExpr right) => right.Context.MkReal(left).Gt(right);
    public static Z3BoolExpr operator >=(double left, Z3RealExpr right) => right.Context.MkReal(left).Ge(right);

    // Mixed-type equality operations (double)
    public static Z3BoolExpr operator ==(Z3RealExpr left, double right) => left.Eq(left.Context.MkReal(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, double right) => left.Neq(left.Context.MkReal(right));
    public static Z3BoolExpr operator ==(double left, Z3RealExpr right) => right.Context.MkReal(left).Eq(right);
    public static Z3BoolExpr operator !=(double left, Z3RealExpr right) => right.Context.MkReal(left).Neq(right);

    // Z3RealExpr <-> int operations (for convenience: myRealExpr < 10)
    public static Z3RealExpr operator +(Z3RealExpr left, int right) => left.Add(left.Context.MkReal(right));
    public static Z3RealExpr operator -(Z3RealExpr left, int right) => left.Sub(left.Context.MkReal(right));
    public static Z3RealExpr operator *(Z3RealExpr left, int right) => left.Mul(left.Context.MkReal(right));
    public static Z3RealExpr operator /(Z3RealExpr left, int right) => left.Div(left.Context.MkReal(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, int right) => left.Lt(left.Context.MkReal(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, int right) => left.Le(left.Context.MkReal(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, int right) => left.Gt(left.Context.MkReal(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, int right) => left.Ge(left.Context.MkReal(right));

    // int <-> Z3RealExpr operations
    public static Z3RealExpr operator +(int left, Z3RealExpr right) => right.Context.MkReal(left).Add(right);
    public static Z3RealExpr operator -(int left, Z3RealExpr right) => right.Context.MkReal(left).Sub(right);
    public static Z3RealExpr operator *(int left, Z3RealExpr right) => right.Context.MkReal(left).Mul(right);
    public static Z3RealExpr operator /(int left, Z3RealExpr right) => right.Context.MkReal(left).Div(right);
    public static Z3BoolExpr operator <(int left, Z3RealExpr right) => right.Context.MkReal(left).Lt(right);
    public static Z3BoolExpr operator <=(int left, Z3RealExpr right) => right.Context.MkReal(left).Le(right);
    public static Z3BoolExpr operator >(int left, Z3RealExpr right) => right.Context.MkReal(left).Gt(right);
    public static Z3BoolExpr operator >=(int left, Z3RealExpr right) => right.Context.MkReal(left).Ge(right);

    // Mixed-type equality operations (int)
    public static Z3BoolExpr operator ==(Z3RealExpr left, int right) => left.Eq(left.Context.MkReal(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, int right) => left.Neq(left.Context.MkReal(right));
    public static Z3BoolExpr operator ==(int left, Z3RealExpr right) => right.Context.MkReal(left).Eq(right);
    public static Z3BoolExpr operator !=(int left, Z3RealExpr right) => right.Context.MkReal(left).Neq(right);
    
    public Z3RealExpr Add(Z3RealExpr other) => Context.MkAdd(this, other);
    public Z3RealExpr Sub(Z3RealExpr other) => Context.MkSub(this, other);
    public Z3RealExpr Mul(Z3RealExpr other) => Context.MkMul(this, other);
    public Z3RealExpr Div(Z3RealExpr other) => Context.MkDiv(this, other);
    public Z3RealExpr UnaryMinus() => Context.MkUnaryMinus(this);
    public Z3BoolExpr Lt(Z3RealExpr other) => Context.MkLt(this, other);
    public Z3BoolExpr Le(Z3RealExpr other) => Context.MkLe(this, other);
    public Z3BoolExpr Gt(Z3RealExpr other) => Context.MkGt(this, other);
    public Z3BoolExpr Ge(Z3RealExpr other) => Context.MkGe(this, other);
    public Z3RealExpr Abs() => (this > 0).If(this, -this);
}