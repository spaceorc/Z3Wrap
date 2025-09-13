using Z3Wrap.Expressions;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Ge(left, right), left, right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Ge(left, right), left, right);
}