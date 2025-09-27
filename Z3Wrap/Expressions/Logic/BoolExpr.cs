using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a Boolean expression in Z3 with support for logical operations, comparisons, and natural syntax.
/// Supports implicit conversion from bool values and provides comprehensive logical operators.
/// </summary>
public sealed class BoolExpr : Z3Expr, IExprType<BoolExpr>
{
    private BoolExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BoolExpr IExprType<BoolExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<BoolExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkBoolSort(context.Handle);

    /// <summary>
    /// Implicitly converts a Boolean value to a BoolExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>A BoolExpr representing the Boolean constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BoolExpr(bool value) => Z3Context.Current.Bool(value);

    /// <summary>
    /// Logical AND operator (left ∧ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left AND right.</returns>
    public static BoolExpr operator &(BoolExpr left, BoolExpr right) =>
        left.Context.And(left, right);

    /// <summary>
    /// Logical OR operator (left ∨ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left OR right.</returns>
    public static BoolExpr operator |(BoolExpr left, BoolExpr right) =>
        left.Context.Or(left, right);

    /// <summary>
    /// Logical XOR operator (left ⊕ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left XOR right.</returns>
    public static BoolExpr operator ^(BoolExpr left, BoolExpr right) =>
        left.Context.Xor(left, right);

    /// <summary>
    /// Logical NOT operator (¬expr).
    /// </summary>
    /// <param name="expr">The Boolean expression to negate.</param>
    /// <returns>A BoolExpr representing NOT expr.</returns>
    public static BoolExpr operator !(BoolExpr expr) => expr.Context.Not(expr);

    /// <summary>
    /// Equality operator for boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(BoolExpr left, BoolExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality operator for boolean expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(BoolExpr left, BoolExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
