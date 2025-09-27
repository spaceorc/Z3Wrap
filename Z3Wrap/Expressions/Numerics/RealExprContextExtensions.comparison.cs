using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class RealExprContextExtensions
{
    /// <summary>
    /// Creates a boolean expression that is true if the left real expression is less than the right real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt; right.</returns>
    public static BoolExpr Lt(this Z3Context context, RealExpr left, RealExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a boolean expression that is true if the left real expression is less than or equal to the right real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt;= right.</returns>
    public static BoolExpr Le(this Z3Context context, RealExpr left, RealExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a boolean expression that is true if the left real expression is greater than the right real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt; right.</returns>
    public static BoolExpr Gt(this Z3Context context, RealExpr left, RealExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a boolean expression that is true if the left real expression is greater than or equal to the right real expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt;= right.</returns>
    public static BoolExpr Ge(this Z3Context context, RealExpr left, RealExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
