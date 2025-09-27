using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class IntExprContextExtensions
{
    /// <summary>
    /// Creates a less-than comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt; right.</returns>
    public static BoolExpr Lt(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt;= right.</returns>
    public static BoolExpr Le(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt; right.</returns>
    public static BoolExpr Gt(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt;= right.</returns>
    public static BoolExpr Ge(this Z3Context context, IntExpr left, IntExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
