using System.Numerics;

namespace Spaceorc.Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3IntExpr : Z3NumericExpr
{
    internal Z3IntExpr(Z3Context context, IntPtr handle) : base(context, handle)
    {
    }

    internal new static Z3IntExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3IntExpr)Z3Expr.Create(context, handle);
    }

    public static implicit operator Z3IntExpr(int value) => Z3Context.Current.Int(value);
    public static implicit operator Z3IntExpr(long value) => Z3Context.Current.Int(value);
    public static implicit operator Z3IntExpr(BigInteger value) => Z3Context.Current.Int(value);

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

    public static Z3BoolExpr operator ==(Z3IntExpr left, BigInteger right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3IntExpr left, BigInteger right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator ==(BigInteger left, Z3IntExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(BigInteger left, Z3IntExpr right) => right.Context.Neq(left, right);
    
    public Z3IntExpr Add(Z3IntExpr other) => Context.Add(this, other);
    public Z3IntExpr Sub(Z3IntExpr other) => Context.Sub(this, other);
    public Z3IntExpr Mul(Z3IntExpr other) => Context.Mul(this, other);
    public Z3IntExpr Div(Z3IntExpr other) => Context.Div(this, other);
    public Z3IntExpr Mod(Z3IntExpr other) => Context.Mod(this, other);
    public Z3BoolExpr Lt(Z3IntExpr other) => Context.Lt(this, other);
    public Z3BoolExpr Le(Z3IntExpr other) => Context.Le(this, other);
    public Z3BoolExpr Gt(Z3IntExpr other) => Context.Gt(this, other);
    public Z3BoolExpr Ge(Z3IntExpr other) => Context.Ge(this, other);
    public Z3IntExpr UnaryMinus() => Context.UnaryMinus(this);
    public Z3IntExpr Abs() => Context.Abs(this);
    
    public Z3RealExpr ToReal() => Context.ToReal(this);
    public Z3BitVecExpr ToBitVec(uint size) => Context.ToBitVec(this, size);
}