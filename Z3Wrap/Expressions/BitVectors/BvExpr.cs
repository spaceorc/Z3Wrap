using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class BvExpr<TSize> : Z3Expr, INumericExpr, IExprType<BvExpr<TSize>>
    where TSize : ISize
{
    public static uint Size => TSize.Size;

    private BvExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BvExpr<TSize> IExprType<BvExpr<TSize>>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<BvExpr<TSize>>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);

    public override string ToString() => $"Z3BitVec[{Size}]({base.ToString()})";

    public static implicit operator BvExpr<TSize>(BigInteger value) => Z3Context.Current.BitVec<TSize>(value);

    public static implicit operator BvExpr<TSize>(int value) => Z3Context.Current.BitVec<TSize>(value);

    public static implicit operator BvExpr<TSize>(uint value) => Z3Context.Current.BitVec<TSize>(value);

    public static implicit operator BvExpr<TSize>(long value) => Z3Context.Current.BitVec<TSize>(value);

    public static implicit operator BvExpr<TSize>(ulong value) => Z3Context.Current.BitVec<TSize>(value);

    public static implicit operator BvExpr<TSize>(Bv<TSize> value) => Z3Context.Current.BitVec(value);

    public IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);

    public BvExpr<TOutputSize> Extract<TOutputSize>(uint startBit)
        where TOutputSize : ISize => Context.Extract<TSize, TOutputSize>(this, startBit);

    public BvExpr<TOutputSize> Resize<TOutputSize>(bool signed = false)
        where TOutputSize : ISize => Context.Resize<TSize, TOutputSize>(this, signed);

    public BvExpr<TOutputSize> Repeat<TOutputSize>()
        where TOutputSize : ISize => Context.Repeat<TSize, TOutputSize>(this);

    public static BoolExpr operator ==(BvExpr<TSize> left, BvExpr<TSize> right) => left.Eq(right);

    public static BoolExpr operator !=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Neq(right);

    public static BoolExpr operator <(BvExpr<TSize> left, BvExpr<TSize> right) => left.Lt(right);

    public static BoolExpr operator <=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Le(right);

    public static BoolExpr operator >(BvExpr<TSize> left, BvExpr<TSize> right) => left.Gt(right);

    public static BoolExpr operator >=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Ge(right);

    public static BvExpr<TSize> operator +(BvExpr<TSize> left, BvExpr<TSize> right) => left.Add(right);

    public static BvExpr<TSize> operator -(BvExpr<TSize> left, BvExpr<TSize> right) => left.Sub(right);

    public static BvExpr<TSize> operator *(BvExpr<TSize> left, BvExpr<TSize> right) => left.Mul(right);

    public static BvExpr<TSize> operator /(BvExpr<TSize> left, BvExpr<TSize> right) => left.Div(right);

    public static BvExpr<TSize> operator %(BvExpr<TSize> left, BvExpr<TSize> right) => left.Rem(right);

    public static BvExpr<TSize> operator -(BvExpr<TSize> operand) => operand.Neg();

    public static BvExpr<TSize> operator &(BvExpr<TSize> left, BvExpr<TSize> right) => left.And(right);

    public static BvExpr<TSize> operator |(BvExpr<TSize> left, BvExpr<TSize> right) => left.Or(right);

    public static BvExpr<TSize> operator ^(BvExpr<TSize> left, BvExpr<TSize> right) => left.Xor(right);

    public static BvExpr<TSize> operator ~(BvExpr<TSize> operand) => operand.Not();

    public static BvExpr<TSize> operator <<(BvExpr<TSize> left, BvExpr<TSize> right) => left.Shl(right);

    public static BvExpr<TSize> operator >>(BvExpr<TSize> left, BvExpr<TSize> right) => left.Shr(right);
}
