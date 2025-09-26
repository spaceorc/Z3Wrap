using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.IntTheory;

public static partial class Z3ContextIntExtensions
{
    /// <summary>
    /// Creates a less-than comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt; right.</returns>
    public static Z3Bool Lt(this Z3Context context, Z3Int left, Z3Int right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &lt;= right.</returns>
    public static Z3Bool Le(this Z3Context context, Z3Int left, Z3Int right)
    {
        var resultHandle = SafeNativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt; right.</returns>
    public static Z3Bool Gt(this Z3Context context, Z3Int left, Z3Int right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison between two integer expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Z3Bool representing left &gt;= right.</returns>
    public static Z3Bool Ge(this Z3Context context, Z3Int left, Z3Int right)
    {
        var resultHandle = SafeNativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }
}
