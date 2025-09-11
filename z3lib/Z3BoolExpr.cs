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

    public Z3BoolExpr And(Z3BoolExpr other) => Context.MkAnd(this, other);
    public Z3BoolExpr Or(Z3BoolExpr other) => Context.MkOr(this, other);
    public Z3BoolExpr Not() => Context.MkNot(this);
    
    public Z3BoolExpr Implies(Z3BoolExpr other) => Context.MkImplies(this, other);
    public Z3BoolExpr Iff(Z3BoolExpr other) => Context.MkIff(this, other);
    public Z3BoolExpr Xor(Z3BoolExpr other) => Context.MkXor(this, other);
    
    public T If<T>(T thenExpr, T elseExpr) where T : Z3Expr => (T)Context.MkIte(this, thenExpr, elseExpr);
}