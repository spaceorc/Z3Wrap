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
    public static Z3RealExpr operator +(Z3RealExpr left, double right) => left.Add(left.Context.Real(right));
    public static Z3RealExpr operator -(Z3RealExpr left, double right) => left.Sub(left.Context.Real(right));
    public static Z3RealExpr operator *(Z3RealExpr left, double right) => left.Mul(left.Context.Real(right));
    public static Z3RealExpr operator /(Z3RealExpr left, double right) => left.Div(left.Context.Real(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, double right) => left.Lt(left.Context.Real(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, double right) => left.Le(left.Context.Real(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, double right) => left.Gt(left.Context.Real(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, double right) => left.Ge(left.Context.Real(right));

    // double <-> Z3RealExpr operations
    public static Z3RealExpr operator +(double left, Z3RealExpr right) => right.Context.Real(left).Add(right);
    public static Z3RealExpr operator -(double left, Z3RealExpr right) => right.Context.Real(left).Sub(right);
    public static Z3RealExpr operator *(double left, Z3RealExpr right) => right.Context.Real(left).Mul(right);
    public static Z3RealExpr operator /(double left, Z3RealExpr right) => right.Context.Real(left).Div(right);
    public static Z3BoolExpr operator <(double left, Z3RealExpr right) => right.Context.Real(left).Lt(right);
    public static Z3BoolExpr operator <=(double left, Z3RealExpr right) => right.Context.Real(left).Le(right);
    public static Z3BoolExpr operator >(double left, Z3RealExpr right) => right.Context.Real(left).Gt(right);
    public static Z3BoolExpr operator >=(double left, Z3RealExpr right) => right.Context.Real(left).Ge(right);

    // Mixed-type equality operations (double)
    public static Z3BoolExpr operator ==(Z3RealExpr left, double right) => left.Eq(left.Context.Real(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, double right) => left.Neq(left.Context.Real(right));
    public static Z3BoolExpr operator ==(double left, Z3RealExpr right) => right.Context.Real(left).Eq(right);
    public static Z3BoolExpr operator !=(double left, Z3RealExpr right) => right.Context.Real(left).Neq(right);

    // Z3RealExpr <-> int operations (for convenience: myRealExpr < 10)
    public static Z3RealExpr operator +(Z3RealExpr left, int right) => left.Add(left.Context.Real(right));
    public static Z3RealExpr operator -(Z3RealExpr left, int right) => left.Sub(left.Context.Real(right));
    public static Z3RealExpr operator *(Z3RealExpr left, int right) => left.Mul(left.Context.Real(right));
    public static Z3RealExpr operator /(Z3RealExpr left, int right) => left.Div(left.Context.Real(right));
    public static Z3BoolExpr operator <(Z3RealExpr left, int right) => left.Lt(left.Context.Real(right));
    public static Z3BoolExpr operator <=(Z3RealExpr left, int right) => left.Le(left.Context.Real(right));
    public static Z3BoolExpr operator >(Z3RealExpr left, int right) => left.Gt(left.Context.Real(right));
    public static Z3BoolExpr operator >=(Z3RealExpr left, int right) => left.Ge(left.Context.Real(right));

    // int <-> Z3RealExpr operations
    public static Z3RealExpr operator +(int left, Z3RealExpr right) => right.Context.Real(left).Add(right);
    public static Z3RealExpr operator -(int left, Z3RealExpr right) => right.Context.Real(left).Sub(right);
    public static Z3RealExpr operator *(int left, Z3RealExpr right) => right.Context.Real(left).Mul(right);
    public static Z3RealExpr operator /(int left, Z3RealExpr right) => right.Context.Real(left).Div(right);
    public static Z3BoolExpr operator <(int left, Z3RealExpr right) => right.Context.Real(left).Lt(right);
    public static Z3BoolExpr operator <=(int left, Z3RealExpr right) => right.Context.Real(left).Le(right);
    public static Z3BoolExpr operator >(int left, Z3RealExpr right) => right.Context.Real(left).Gt(right);
    public static Z3BoolExpr operator >=(int left, Z3RealExpr right) => right.Context.Real(left).Ge(right);

    // Mixed-type equality operations (int)
    public static Z3BoolExpr operator ==(Z3RealExpr left, int right) => left.Eq(left.Context.Real(right));
    public static Z3BoolExpr operator !=(Z3RealExpr left, int right) => left.Neq(left.Context.Real(right));
    public static Z3BoolExpr operator ==(int left, Z3RealExpr right) => right.Context.Real(left).Eq(right);
    public static Z3BoolExpr operator !=(int left, Z3RealExpr right) => right.Context.Real(left).Neq(right);
    
    public Z3RealExpr Add(Z3RealExpr other) => Context.Add(this, other);
    public Z3RealExpr Sub(Z3RealExpr other) => Context.Sub(this, other);
    public Z3RealExpr Mul(Z3RealExpr other) => Context.Mul(this, other);
    public Z3RealExpr Div(Z3RealExpr other) => Context.Div(this, other);
    public Z3RealExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3BoolExpr Lt(Z3RealExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3RealExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3RealExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3RealExpr other) => Context.Ge(this, other);
    public Z3RealExpr Abs() => (this > 0).If(this, -this);
}