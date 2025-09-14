using Z3Wrap.Expressions;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    public static Z3IntExpr Abs(this Z3Context context, Z3IntExpr operand)
        => context.Ite(operand >= 0, operand, -operand);

    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
        => context.Ite(left < right, left, right);

    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
        => context.Ite(left > right, left, right);
}