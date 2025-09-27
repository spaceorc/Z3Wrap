using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class BoolExpr : Z3Expr, IExprType<BoolExpr>
{
    private BoolExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BoolExpr IExprType<BoolExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<BoolExpr>.Sort(Z3Context context) => SafeNativeMethods.Z3MkBoolSort(context.Handle);

    public static implicit operator BoolExpr(bool value) => Z3Context.Current.Bool(value);

    public static BoolExpr operator &(BoolExpr left, BoolExpr right) => left.Context.And(left, right);

    public static BoolExpr operator |(BoolExpr left, BoolExpr right) => left.Context.Or(left, right);

    public static BoolExpr operator ^(BoolExpr left, BoolExpr right) => left.Context.Xor(left, right);

    public static BoolExpr operator !(BoolExpr expr) => expr.Context.Not(expr);

    public static BoolExpr operator ==(BoolExpr left, BoolExpr right) => left.Eq(right);

    public static BoolExpr operator !=(BoolExpr left, BoolExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
