namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3BoolExpr : Z3Expr
{
    internal Z3BoolExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public new static Z3BoolExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3BoolExpr)Z3Expr.Create(context, handle);
    }

    // Implicit conversions using thread-local context
    public static implicit operator Z3BoolExpr(bool value) => Z3Context.Current.Bool(value);

    public static Z3BoolExpr operator &(Z3BoolExpr left, Z3BoolExpr right) => left.And(right);
    public static Z3BoolExpr operator |(Z3BoolExpr left, Z3BoolExpr right) => left.Or(right);
    public static Z3BoolExpr operator ^(Z3BoolExpr left, Z3BoolExpr right) => left.Xor(right);
    public static Z3BoolExpr operator !(Z3BoolExpr expr) => expr.Not();

    // Mixed-type equality operations
    public static Z3BoolExpr operator ==(Z3BoolExpr left, bool right) => left.Context.Eq(left, left.Context.Bool(right));
    public static Z3BoolExpr operator !=(Z3BoolExpr left, bool right) => left.Context.Neq(left, left.Context.Bool(right));
    public static Z3BoolExpr operator ==(bool left, Z3BoolExpr right) => right.Context.Eq(right.Context.Bool(left), right);
    public static Z3BoolExpr operator !=(bool left, Z3BoolExpr right) => right.Context.Neq(right.Context.Bool(left), right);

    public Z3BoolExpr And(Z3BoolExpr other) => Context.And(this, other);
    public Z3BoolExpr Or(Z3BoolExpr other) => Context.Or(this, other);
    public Z3BoolExpr Not() => Context.Not(this);

    public Z3BoolExpr Implies(Z3BoolExpr other) => Context.Implies(this, other);
    public Z3BoolExpr Iff(Z3BoolExpr other) => Context.Iff(this, other);
    public Z3BoolExpr Xor(Z3BoolExpr other) => Context.Xor(this, other);

    // Equality/inequality instance methods with bool overloads
    public Z3BoolExpr Eq(bool other) => Context.Eq(this, other);
    public Z3BoolExpr Neq(bool other) => Context.Neq(this, other);
    
    public T If<T>(T thenExpr, T elseExpr) where T : Z3Expr => Context.Ite(this, thenExpr, elseExpr);
}