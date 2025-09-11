namespace Z3Wrap;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3IntExpr : Z3Expr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    // Z3IntExpr <-> Z3IntExpr operations
    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right) => left.Context.Add(left, right);
    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right) => left.Context.Sub(left, right);
    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right) => left.Context.Mul(left, right);
    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right) => left.Context.Div(left, right);
    public static Z3IntExpr operator %(Z3IntExpr left, Z3IntExpr right) => left.Context.Mod(left, right);
    public static Z3BoolExpr operator <(Z3IntExpr left, Z3IntExpr right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3IntExpr left, Z3IntExpr right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3IntExpr left, Z3IntExpr right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3IntExpr left, Z3IntExpr right) => left.Context.Ge(left, right);
    public static Z3IntExpr operator -(Z3IntExpr expr) => expr.Context.UnaryMinus(expr);

    // Z3IntExpr <-> int operations  
    public static Z3IntExpr operator +(Z3IntExpr left, int right) => left.Context.Add(left, right);
    public static Z3IntExpr operator -(Z3IntExpr left, int right) => left.Context.Sub(left, right);
    public static Z3IntExpr operator *(Z3IntExpr left, int right) => left.Context.Mul(left, right);
    public static Z3IntExpr operator /(Z3IntExpr left, int right) => left.Context.Div(left, right);
    public static Z3IntExpr operator %(Z3IntExpr left, int right) => left.Context.Mod(left, right);
    public static Z3BoolExpr operator <(Z3IntExpr left, int right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3IntExpr left, int right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3IntExpr left, int right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3IntExpr left, int right) => left.Context.Ge(left, right);

    // int <-> Z3IntExpr operations
    public static Z3IntExpr operator +(int left, Z3IntExpr right) => right.Context.Add(left, right);
    public static Z3IntExpr operator -(int left, Z3IntExpr right) => right.Context.Sub(left, right);
    public static Z3IntExpr operator *(int left, Z3IntExpr right) => right.Context.Mul(left, right);
    public static Z3IntExpr operator /(int left, Z3IntExpr right) => right.Context.Div(left, right);
    public static Z3IntExpr operator %(int left, Z3IntExpr right) => right.Context.Mod(left, right);
    public static Z3BoolExpr operator <(int left, Z3IntExpr right) => right.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(int left, Z3IntExpr right) => right.Context.Le(left, right);
    public static Z3BoolExpr operator >(int left, Z3IntExpr right) => right.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(int left, Z3IntExpr right) => right.Context.Ge(left, right);

    // Mixed-type equality operations
    public static Z3BoolExpr operator ==(Z3IntExpr left, int right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3IntExpr left, int right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator ==(int left, Z3IntExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(int left, Z3IntExpr right) => right.Context.Neq(left, right);
    
    public Z3IntExpr Add(Z3IntExpr other) => Context.Add(this, other);
    public Z3IntExpr Sub(Z3IntExpr other) => Context.Sub(this, other);
    public Z3IntExpr Mul(Z3IntExpr other) => Context.Mul(this, other);
    public Z3IntExpr Div(Z3IntExpr other) => Context.Div(this, other);
    public Z3IntExpr Mod(Z3IntExpr other) => Context.Mod(this, other);
    public Z3BoolExpr Lt(Z3IntExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3IntExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3IntExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3IntExpr other) => Context.Ge(this, other);

    public Z3IntExpr Add(int other) => Context.Add(this, other);
    public Z3IntExpr Sub(int other) => Context.Sub(this, other);
    public Z3IntExpr Mul(int other) => Context.Mul(this, other);
    public Z3IntExpr Div(int other) => Context.Div(this, other);
    public Z3IntExpr Mod(int other) => Context.Mod(this, other);
    public Z3BoolExpr Lt(int other) => Context.Lt(this, other);
    public Z3BoolExpr Le(int other) => Context.Le(this, other);
    public Z3BoolExpr Gt(int other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(int other) => Context.Ge(this, other);

    public Z3IntExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3IntExpr Abs() => Context.Abs(this);
}