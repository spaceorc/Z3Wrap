using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Represents a rounding mode expression for floating-point operations.
/// </summary>
public sealed class RoundingModeExpr : Z3Expr
{
    internal RoundingModeExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }
}
