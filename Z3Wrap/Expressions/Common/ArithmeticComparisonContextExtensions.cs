using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Provides extension methods for Z3Context to create comparison operations for arithmetic expressions.
/// Supports relational comparisons (&lt;, ≤, &gt;, ≥) for integer and real expressions.
/// </summary>
public static class ArithmeticComparisonContextExtensions
{
    /// <summary>
    /// Creates a less-than comparison between two arithmetic expressions (left &lt; right).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>BoolExpr representing left &lt; right.</returns>
    public static BoolExpr Lt<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison between two arithmetic expressions (left ≤ right).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>BoolExpr representing left ≤ right.</returns>
    public static BoolExpr Le<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than comparison (left &gt; right).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>BoolExpr representing left &gt; right.</returns>
    public static BoolExpr Gt<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison (left ≥ right).
    /// </summary>
    /// <typeparam name="T">Type of arithmetic expression.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>BoolExpr representing left ≥ right.</returns>
    public static BoolExpr Ge<T>(this Z3Context context, T left, T right)
        where T : Z3Expr, IArithmeticExpr<T>, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }
}
