using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class IntExpr : Z3Expr, IArithmeticExpr<IntExpr>, IExprType<IntExpr>
{
    private IntExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static IntExpr IExprType<IntExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<IntExpr>.Sort(Z3Context context) => SafeNativeMethods.Z3MkIntSort(context.Handle);

    static IntExpr IArithmeticExpr<IntExpr>.Zero(Z3Context context) => context.Int(0);

    public static implicit operator IntExpr(int value) => Z3Context.Current.Int(value);

    public static implicit operator IntExpr(long value) => Z3Context.Current.Int(value);

    public static implicit operator IntExpr(BigInteger value) => Z3Context.Current.Int(value);

    public RealExpr ToReal() => Context.ToReal(this);

    public BvExpr<TSize> ToBitVec<TSize>()
        where TSize : ISize => Context.ToBitVec<TSize>(this);

    public static IntExpr operator +(IntExpr left, IntExpr right) => left.Add(right);

    public static IntExpr operator -(IntExpr left, IntExpr right) => left.Sub(right);

    public static IntExpr operator *(IntExpr left, IntExpr right) => left.Mul(right);

    public static IntExpr operator /(IntExpr left, IntExpr right) => left.Div(right);

    public static IntExpr operator %(IntExpr left, IntExpr right) => left.Mod(right);

    public static IntExpr operator -(IntExpr expr) => expr.UnaryMinus();

    public static BoolExpr operator <(IntExpr left, IntExpr right) => left.Lt(right);

    public static BoolExpr operator <=(IntExpr left, IntExpr right) => left.Le(right);

    public static BoolExpr operator >(IntExpr left, IntExpr right) => left.Gt(right);

    public static BoolExpr operator >=(IntExpr left, IntExpr right) => left.Ge(right);

    public static BoolExpr operator ==(IntExpr left, IntExpr right) => left.Eq(right);

    public static BoolExpr operator !=(IntExpr left, IntExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
