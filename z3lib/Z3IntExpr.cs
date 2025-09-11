namespace z3lib;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3IntExpr : Z3Expr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3IntExpr <-> Z3IntExpr operations
    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right) => left.Add(right);
    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right) => left.Sub(right);
    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right) => left.Mul(right);
    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right) => left.Div(right);
    public static Z3IntExpr operator %(Z3IntExpr left, Z3IntExpr right) => left.Mod(right);
    public static Z3IntExpr operator -(Z3IntExpr expr) => expr.UnaryMinus();
    public static Z3BoolExpr operator <(Z3IntExpr left, Z3IntExpr right) => left.Lt(right);
    public static Z3BoolExpr operator <=(Z3IntExpr left, Z3IntExpr right) => left.Le(right);
    public static Z3BoolExpr operator >(Z3IntExpr left, Z3IntExpr right) => left.Gt(right);
    public static Z3BoolExpr operator >=(Z3IntExpr left, Z3IntExpr right) => left.Ge(right);

    // Z3IntExpr <-> int operations  
    public static Z3IntExpr operator +(Z3IntExpr left, int right) => left.Add(left.Context.Int(right));
    public static Z3IntExpr operator -(Z3IntExpr left, int right) => left.Sub(left.Context.Int(right));
    public static Z3IntExpr operator *(Z3IntExpr left, int right) => left.Mul(left.Context.Int(right));
    public static Z3IntExpr operator /(Z3IntExpr left, int right) => left.Div(left.Context.Int(right));
    public static Z3IntExpr operator %(Z3IntExpr left, int right) => left.Mod(left.Context.Int(right));
    public static Z3BoolExpr operator <(Z3IntExpr left, int right) => left.Lt(left.Context.Int(right));
    public static Z3BoolExpr operator <=(Z3IntExpr left, int right) => left.Le(left.Context.Int(right));
    public static Z3BoolExpr operator >(Z3IntExpr left, int right) => left.Gt(left.Context.Int(right));
    public static Z3BoolExpr operator >=(Z3IntExpr left, int right) => left.Ge(left.Context.Int(right));

    // int <-> Z3IntExpr operations
    public static Z3IntExpr operator +(int left, Z3IntExpr right) => right.Context.Int(left).Add(right);
    public static Z3IntExpr operator -(int left, Z3IntExpr right) => right.Context.Int(left).Sub(right);
    public static Z3IntExpr operator *(int left, Z3IntExpr right) => right.Context.Int(left).Mul(right);
    public static Z3IntExpr operator /(int left, Z3IntExpr right) => right.Context.Int(left).Div(right);
    public static Z3IntExpr operator %(int left, Z3IntExpr right) => right.Context.Int(left).Mod(right);
    public static Z3BoolExpr operator <(int left, Z3IntExpr right) => right.Context.Int(left).Lt(right);
    public static Z3BoolExpr operator <=(int left, Z3IntExpr right) => right.Context.Int(left).Le(right);
    public static Z3BoolExpr operator >(int left, Z3IntExpr right) => right.Context.Int(left).Gt(right);
    public static Z3BoolExpr operator >=(int left, Z3IntExpr right) => right.Context.Int(left).Ge(right);

    // Mixed-type equality operations
    public static Z3BoolExpr operator ==(Z3IntExpr left, int right) => left.Eq(left.Context.Int(right));
    public static Z3BoolExpr operator !=(Z3IntExpr left, int right) => left.Neq(left.Context.Int(right));
    public static Z3BoolExpr operator ==(int left, Z3IntExpr right) => right.Context.Int(left).Eq(right);
    public static Z3BoolExpr operator !=(int left, Z3IntExpr right) => right.Context.Int(left).Neq(right);
    
    public Z3IntExpr Add(Z3IntExpr other) => Context.Add(this, other);
    public Z3IntExpr Sub(Z3IntExpr other) => Context.Sub(this, other);
    public Z3IntExpr Mul(Z3IntExpr other) => Context.Mul(this, other);
    public Z3IntExpr Div(Z3IntExpr other) => Context.Div(this, other);
    public Z3IntExpr Mod(Z3IntExpr other) => Context.Mod(this, other);
    public Z3IntExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3BoolExpr Lt(Z3IntExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3IntExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3IntExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3IntExpr other) => Context.Ge(this, other);
    public Z3IntExpr Abs() => (this > 0).If(this, -this);
}