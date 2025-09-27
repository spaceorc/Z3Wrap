using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class RealExpr : Z3Expr, IArithmeticExpr<RealExpr>, IExprType<RealExpr>
{
    private RealExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static RealExpr IExprType<RealExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<RealExpr>.Sort(Z3Context context) => SafeNativeMethods.Z3MkRealSort(context.Handle);

    static RealExpr IArithmeticExpr<RealExpr>.Zero(Z3Context context) => context.Real(0);

    public static implicit operator RealExpr(int value) => Z3Context.Current.Real(value);

    public static implicit operator RealExpr(long value) => Z3Context.Current.Real(value);

    public static implicit operator RealExpr(decimal value) => Z3Context.Current.Real(value);

    public static implicit operator RealExpr(BigInteger value) => Z3Context.Current.Real(value);

    public static implicit operator RealExpr(Real value) => Z3Context.Current.Real(value);

    public IntExpr ToInt() => Context.ToInt(this);

    public static RealExpr operator +(RealExpr left, RealExpr right) => left.Add(right);

    public static RealExpr operator -(RealExpr left, RealExpr right) => left.Sub(right);

    public static RealExpr operator *(RealExpr left, RealExpr right) => left.Mul(right);

    public static RealExpr operator /(RealExpr left, RealExpr right) => left.Div(right);

    public static RealExpr operator -(RealExpr expr) => expr.UnaryMinus();

    public static BoolExpr operator <(RealExpr left, RealExpr right) => left.Lt(right);

    public static BoolExpr operator <=(RealExpr left, RealExpr right) => left.Le(right);

    public static BoolExpr operator >(RealExpr left, RealExpr right) => left.Gt(right);

    public static BoolExpr operator >=(RealExpr left, RealExpr right) => left.Ge(right);

    public static BoolExpr operator ==(RealExpr left, RealExpr right) => left.Eq(right);

    public static BoolExpr operator !=(RealExpr left, RealExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
