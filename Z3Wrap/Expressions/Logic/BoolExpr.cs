using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Represents a boolean expression for logical constraints and operations.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class BoolExpr : Z3Expr, IExprType<BoolExpr>
{
    private BoolExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BoolExpr IExprType<BoolExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<BoolExpr>.Sort(Z3Context context) => context.Library.MkBoolSort(context.Handle);

    /// <summary>
    /// Implicit conversion from boolean value to boolean expression.
    /// </summary>
    /// <param name="value">The boolean value.</param>
    /// <returns>Boolean expression representing the value.</returns>
    public static implicit operator BoolExpr(bool value) => Z3Context.Current.Bool(value);

    /// <summary>
    /// Logical AND of two boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>AND expression.</returns>
    public static BoolExpr operator &(BoolExpr left, BoolExpr right) => left.Context.And(left, right);

    /// <summary>
    /// Logical OR of two boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>OR expression.</returns>
    public static BoolExpr operator |(BoolExpr left, BoolExpr right) => left.Context.Or(left, right);

    /// <summary>
    /// Logical XOR of two boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>XOR expression.</returns>
    public static BoolExpr operator ^(BoolExpr left, BoolExpr right) => left.Context.Xor(left, right);

    /// <summary>
    /// Logical NOT of a boolean expression.
    /// </summary>
    /// <param name="expr">The expression to negate.</param>
    /// <returns>NOT expression.</returns>
    public static BoolExpr operator !(BoolExpr expr) => expr.Context.Not(expr);

    /// <summary>
    /// Equality comparison of two boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Equality expression.</returns>
    public static BoolExpr operator ==(BoolExpr left, BoolExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Inequality expression.</returns>
    public static BoolExpr operator !=(BoolExpr left, BoolExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
