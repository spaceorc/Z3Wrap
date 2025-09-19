using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    public static Z3RealExpr Abs(this Z3Context context, Z3RealExpr operand)
        => context.Ite(operand >= 0, operand, -operand);

    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(left < right, left, right);

    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
        => context.Ite(left > right, left, right);
}