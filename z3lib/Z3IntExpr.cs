namespace z3lib;

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
    public static Z3IntExpr operator +(Z3IntExpr left, int right) => left.Add(left.Context.MkInt(right));
    public static Z3IntExpr operator -(Z3IntExpr left, int right) => left.Sub(left.Context.MkInt(right));
    public static Z3IntExpr operator *(Z3IntExpr left, int right) => left.Mul(left.Context.MkInt(right));
    public static Z3IntExpr operator /(Z3IntExpr left, int right) => left.Div(left.Context.MkInt(right));
    public static Z3IntExpr operator %(Z3IntExpr left, int right) => left.Mod(left.Context.MkInt(right));
    public static Z3BoolExpr operator <(Z3IntExpr left, int right) => left.Lt(left.Context.MkInt(right));
    public static Z3BoolExpr operator <=(Z3IntExpr left, int right) => left.Le(left.Context.MkInt(right));
    public static Z3BoolExpr operator >(Z3IntExpr left, int right) => left.Gt(left.Context.MkInt(right));
    public static Z3BoolExpr operator >=(Z3IntExpr left, int right) => left.Ge(left.Context.MkInt(right));

    // int <-> Z3IntExpr operations
    public static Z3IntExpr operator +(int left, Z3IntExpr right) => right.Context.MkInt(left).Add(right);
    public static Z3IntExpr operator -(int left, Z3IntExpr right) => right.Context.MkInt(left).Sub(right);
    public static Z3IntExpr operator *(int left, Z3IntExpr right) => right.Context.MkInt(left).Mul(right);
    public static Z3IntExpr operator /(int left, Z3IntExpr right) => right.Context.MkInt(left).Div(right);
    public static Z3IntExpr operator %(int left, Z3IntExpr right) => right.Context.MkInt(left).Mod(right);
    public static Z3BoolExpr operator <(int left, Z3IntExpr right) => right.Context.MkInt(left).Lt(right);
    public static Z3BoolExpr operator <=(int left, Z3IntExpr right) => right.Context.MkInt(left).Le(right);
    public static Z3BoolExpr operator >(int left, Z3IntExpr right) => right.Context.MkInt(left).Gt(right);
    public static Z3BoolExpr operator >=(int left, Z3IntExpr right) => right.Context.MkInt(left).Ge(right);

    // Mixed-type equality operations
    public static Z3BoolExpr operator ==(Z3IntExpr left, int right) => left.Eq(left.Context.MkInt(right));
    public static Z3BoolExpr operator !=(Z3IntExpr left, int right) => left.Neq(left.Context.MkInt(right));
    public static Z3BoolExpr operator ==(int left, Z3IntExpr right) => right.Context.MkInt(left).Eq(right);
    public static Z3BoolExpr operator !=(int left, Z3IntExpr right) => right.Context.MkInt(left).Neq(right);
    
    public Z3IntExpr Add(Z3IntExpr other) => Context.MkAdd(this, other);
    public Z3IntExpr Sub(Z3IntExpr other) => Context.MkSub(this, other);
    public Z3IntExpr Mul(Z3IntExpr other) => Context.MkMul(this, other);
    public Z3IntExpr Div(Z3IntExpr other) => Context.MkDiv(this, other);
    public Z3IntExpr Mod(Z3IntExpr other) => Context.MkMod(this, other);
    public Z3IntExpr UnaryMinus() => Context.MkUnaryMinus(this);
    public Z3BoolExpr Lt(Z3IntExpr other) => Context.MkLt(this, other);
    public Z3BoolExpr Le(Z3IntExpr other) => Context.MkLe(this, other);
    public Z3BoolExpr Gt(Z3IntExpr other) => Context.MkGt(this, other);
    public Z3BoolExpr Ge(Z3IntExpr other) => Context.MkGe(this, other);
    
    public Z3IntExpr Abs() => Context.MkAbs(this);
}