using System.Numerics;
using Z3Wrap.Expressions;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Ge(left, right), left, right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Ge(left, right), left, right);

    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Min(left, context.Int(right));
    public static Z3IntExpr Min(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Min(context.Int(left), right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, BigInteger right) => context.Max(left, context.Int(right));
    public static Z3IntExpr Max(this Z3Context context, BigInteger left, Z3IntExpr right) => context.Max(context.Int(left), right);

    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Real right) => context.Min(left, context.Real(right));
    public static Z3RealExpr Min(this Z3Context context, Real left, Z3RealExpr right) => context.Min(context.Real(left), right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Real right) => context.Max(left, context.Real(right));
    public static Z3RealExpr Max(this Z3Context context, Real left, Z3RealExpr right) => context.Max(context.Real(left), right);
}