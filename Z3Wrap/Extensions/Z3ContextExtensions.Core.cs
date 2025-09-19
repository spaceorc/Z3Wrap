using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates an equality expression between two Z3 expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3BoolExpr representing left == right.</returns>
    public static Z3BoolExpr Eq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var resultHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a not-equal expression between two Z3 expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3BoolExpr representing left != right.</returns>
    public static Z3BoolExpr Neq(this Z3Context context, Z3Expr left, Z3Expr right)
    {
        var eqHandle = NativeMethods.Z3MkEq(context.Handle, left.Handle, right.Handle);
        var resultHandle = NativeMethods.Z3MkNot(context.Handle, eqHandle);
        return Z3BoolExpr.Create(context, resultHandle);
    }

    /// <summary>
    /// Creates a type-safe equality expression between two expressions of the same type.
    /// </summary>
    /// <typeparam name="T">The type of Z3 expressions to compare.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3BoolExpr representing left == right.</returns>
    public static Z3BoolExpr Eq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr => context.Eq((Z3Expr)left, right);

    /// <summary>
    /// Creates a type-safe not-equal expression between two expressions of the same type.
    /// </summary>
    /// <typeparam name="T">The type of Z3 expressions to compare.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3BoolExpr representing left != right.</returns>
    public static Z3BoolExpr Neq<T>(this Z3Context context, T left, T right)
        where T : Z3Expr => context.Neq((Z3Expr)left, right);
}
