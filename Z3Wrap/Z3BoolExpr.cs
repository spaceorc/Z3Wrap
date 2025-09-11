namespace z3lib;

public sealed class Z3BoolExpr : Z3Expr
{
    internal Z3BoolExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public static Z3BoolExpr operator &(Z3BoolExpr left, Z3BoolExpr right) => left.And(right);
    public static Z3BoolExpr operator |(Z3BoolExpr left, Z3BoolExpr right) => left.Or(right);
    public static Z3BoolExpr operator ^(Z3BoolExpr left, Z3BoolExpr right) => left.Xor(right);
    public static Z3BoolExpr operator !(Z3BoolExpr expr) => expr.Not();

    public Z3BoolExpr And(Z3BoolExpr other) => Context.And(this, other);
    public Z3BoolExpr Or(Z3BoolExpr other) => Context.Or(this, other);
    public Z3BoolExpr Not() => Context.Not(this);
    
    public Z3BoolExpr Implies(Z3BoolExpr other) => Context.Implies(this, other);
    public Z3BoolExpr Iff(Z3BoolExpr other) => Context.Iff(this, other);
    public Z3BoolExpr Xor(Z3BoolExpr other) => Context.Xor(this, other);
    
    public T If<T>(T thenExpr, T elseExpr) where T : Z3Expr => Context.Ite(this, thenExpr, elseExpr);
}