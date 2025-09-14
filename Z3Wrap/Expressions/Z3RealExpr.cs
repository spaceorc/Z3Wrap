using System.Numerics;

namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3RealExpr : Z3Expr
{
    internal Z3RealExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    public new static Z3RealExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3RealExpr)Z3Expr.Create(context, handle);
    }

    // Implicit conversions using thread-local context
    public static implicit operator Z3RealExpr(int value) => Z3Context.Current.Real(value);
    public static implicit operator Z3RealExpr(long value) => Z3Context.Current.Real(value);
    public static implicit operator Z3RealExpr(decimal value) => Z3Context.Current.Real(value);
    public static implicit operator Z3RealExpr(BigInteger value) => Z3Context.Current.Real(value);
    public static implicit operator Z3RealExpr(Real value) => Z3Context.Current.Real(value);

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

    // Mixed-type equality operations (Real)
    public static Z3BoolExpr operator ==(Z3RealExpr left, Real right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3RealExpr left, Real right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator ==(Real left, Z3RealExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Real left, Z3RealExpr right) => right.Context.Neq(left, right);

    public Z3RealExpr Add(Z3RealExpr other) => Context.Add(this, other);
    public Z3RealExpr Sub(Z3RealExpr other) => Context.Sub(this, other);
    public Z3RealExpr Mul(Z3RealExpr other) => Context.Mul(this, other);
    public Z3RealExpr Div(Z3RealExpr other) => Context.Div(this, other);
    public Z3BoolExpr Lt(Z3RealExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3RealExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3RealExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3RealExpr other) => Context.Ge(this, other);
    public Z3RealExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3RealExpr Abs() => Context.Abs(this);
    
    public Z3IntExpr ToInt() => Context.ToInt(this);
}