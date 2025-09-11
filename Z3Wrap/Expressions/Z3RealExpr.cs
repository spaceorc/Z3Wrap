namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3RealExpr : Z3Expr
{
    internal Z3RealExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3RealExpr <-> Z3RealExpr operations
    public static Z3RealExpr operator +(Z3RealExpr left, Z3RealExpr right) => left.Context.Add(left, right);
    public static Z3RealExpr operator -(Z3RealExpr left, Z3RealExpr right) => left.Context.Sub(left, right);
    public static Z3RealExpr operator *(Z3RealExpr left, Z3RealExpr right) => left.Context.Mul(left, right);
    public static Z3RealExpr operator /(Z3RealExpr left, Z3RealExpr right) => left.Context.Div(left, right);
    public static Z3BoolExpr operator <(Z3RealExpr left, Z3RealExpr right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3RealExpr left, Z3RealExpr right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3RealExpr left, Z3RealExpr right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3RealExpr left, Z3RealExpr right) => left.Context.Ge(left, right);
    public static Z3RealExpr operator -(Z3RealExpr expr) => expr.Context.UnaryMinus(expr);

    // Z3RealExpr <-> double operations
    public static Z3RealExpr operator +(Z3RealExpr left, double right) => left.Context.Add(left, right);
    public static Z3RealExpr operator -(Z3RealExpr left, double right) => left.Context.Sub(left, right);
    public static Z3RealExpr operator *(Z3RealExpr left, double right) => left.Context.Mul(left, right);
    public static Z3RealExpr operator /(Z3RealExpr left, double right) => left.Context.Div(left, right);
    public static Z3BoolExpr operator <(Z3RealExpr left, double right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3RealExpr left, double right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3RealExpr left, double right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3RealExpr left, double right) => left.Context.Ge(left, right);

    // double <-> Z3RealExpr operations
    public static Z3RealExpr operator +(double left, Z3RealExpr right) => right.Context.Add(left, right);
    public static Z3RealExpr operator -(double left, Z3RealExpr right) => right.Context.Sub(left, right);
    public static Z3RealExpr operator *(double left, Z3RealExpr right) => right.Context.Mul(left, right);
    public static Z3RealExpr operator /(double left, Z3RealExpr right) => right.Context.Div(left, right);
    public static Z3BoolExpr operator <(double left, Z3RealExpr right) => right.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(double left, Z3RealExpr right) => right.Context.Le(left, right);
    public static Z3BoolExpr operator >(double left, Z3RealExpr right) => right.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(double left, Z3RealExpr right) => right.Context.Ge(left, right);

    // Mixed-type equality operations (double)
    public static Z3BoolExpr operator ==(Z3RealExpr left, double right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3RealExpr left, double right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator ==(double left, Z3RealExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(double left, Z3RealExpr right) => right.Context.Neq(left, right);

    public Z3RealExpr Add(Z3RealExpr other) => Context.Add(this, other);
    public Z3RealExpr Sub(Z3RealExpr other) => Context.Sub(this, other);
    public Z3RealExpr Mul(Z3RealExpr other) => Context.Mul(this, other);
    public Z3RealExpr Div(Z3RealExpr other) => Context.Div(this, other);
    public Z3BoolExpr Lt(Z3RealExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3RealExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3RealExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3RealExpr other) => Context.Ge(this, other);

    public Z3RealExpr Add(double other) => Context.Add(this, other);
    public Z3RealExpr Sub(double other) => Context.Sub(this, other);
    public Z3RealExpr Mul(double other) => Context.Mul(this, other);
    public Z3RealExpr Div(double other) => Context.Div(this, other);
    public Z3BoolExpr Lt(double other) => Context.Lt(this, other);
    public Z3BoolExpr Le(double other) => Context.Le(this, other);
    public Z3BoolExpr Gt(double other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(double other) => Context.Ge(this, other);

    public Z3RealExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3RealExpr Abs() => Context.Abs(this);
}