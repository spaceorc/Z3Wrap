using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a Boolean expression in Z3 with support for logical operations, comparisons, and natural syntax.
/// Supports implicit conversion from bool values and provides comprehensive logical operators.
/// </summary>
public sealed class Z3BoolExpr : Z3Expr, IZ3ExprType<Z3BoolExpr>
{
    private Z3BoolExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3BoolExpr IZ3ExprType<Z3BoolExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3BoolExpr>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkBoolSort(context.Handle);

    /// <summary>
    /// Implicitly converts a Boolean value to a Z3BoolExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>A Z3BoolExpr representing the Boolean constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BoolExpr(bool value) => Z3Context.Current.Bool(value);

    /// <summary>
    /// Performs logical AND operation using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left AND right.</returns>
    public static Z3BoolExpr operator &(Z3BoolExpr left, Z3BoolExpr right) => left.And(right);

    /// <summary>
    /// Performs logical OR operation using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left OR right.</returns>
    public static Z3BoolExpr operator |(Z3BoolExpr left, Z3BoolExpr right) => left.Or(right);

    /// <summary>
    /// Performs logical XOR operation using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left XOR right.</returns>
    public static Z3BoolExpr operator ^(Z3BoolExpr left, Z3BoolExpr right) => left.Xor(right);

    /// <summary>
    /// Performs logical NOT operation using the ! operator.
    /// </summary>
    /// <param name="expr">The Boolean expression to negate.</param>
    /// <returns>A Boolean expression representing NOT expr.</returns>
    public static Z3BoolExpr operator !(Z3BoolExpr expr) => expr.Not();

    /// <summary>
    /// Compares a Z3BoolExpr with a bool value for equality using the == operator.
    /// </summary>
    /// <param name="left">The Z3BoolExpr operand.</param>
    /// <param name="right">The bool operand.</param>
    /// <returns>A Boolean expression representing the equality comparison.</returns>
    public static Z3BoolExpr operator ==(Z3BoolExpr left, bool right) =>
        left.Context.Eq(left, left.Context.Bool(right));

    /// <summary>
    /// Compares a Z3BoolExpr with a bool value for inequality using the != operator.
    /// </summary>
    /// <param name="left">The Z3BoolExpr operand.</param>
    /// <param name="right">The bool operand.</param>
    /// <returns>A Boolean expression representing the inequality comparison.</returns>
    public static Z3BoolExpr operator !=(Z3BoolExpr left, bool right) =>
        left.Context.Neq(left, left.Context.Bool(right));

    /// <summary>
    /// Compares a bool value with a Z3BoolExpr for equality using the == operator.
    /// </summary>
    /// <param name="left">The bool operand.</param>
    /// <param name="right">The Z3BoolExpr operand.</param>
    /// <returns>A Boolean expression representing the equality comparison.</returns>
    public static Z3BoolExpr operator ==(bool left, Z3BoolExpr right) =>
        right.Context.Eq(right.Context.Bool(left), right);

    /// <summary>
    /// Compares a bool value with a Z3BoolExpr for inequality using the != operator.
    /// </summary>
    /// <param name="left">The bool operand.</param>
    /// <param name="right">The Z3BoolExpr operand.</param>
    /// <returns>A Boolean expression representing the inequality comparison.</returns>
    public static Z3BoolExpr operator !=(bool left, Z3BoolExpr right) =>
        right.Context.Neq(right.Context.Bool(left), right);

    /// <summary>
    /// Creates a Boolean expression representing the logical AND of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this AND other.</returns>
    public Z3BoolExpr And(Z3BoolExpr other) => Context.And(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical OR of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this OR other.</returns>
    public Z3BoolExpr Or(Z3BoolExpr other) => Context.Or(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical negation of this expression.
    /// </summary>
    /// <returns>A Boolean expression representing NOT this.</returns>
    public Z3BoolExpr Not() => Context.Not(this);

    /// <summary>
    /// Creates a Boolean expression representing logical implication (this implies other).
    /// </summary>
    /// <param name="other">The consequent Boolean expression.</param>
    /// <returns>A Boolean expression representing this → other.</returns>
    public Z3BoolExpr Implies(Z3BoolExpr other) => Context.Implies(this, other);

    /// <summary>
    /// Creates a Boolean expression representing logical equivalence (if and only if).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this ↔ other.</returns>
    public Z3BoolExpr Iff(Z3BoolExpr other) => Context.Iff(this, other);

    /// <summary>
    /// Creates a Boolean expression representing the logical XOR of this expression and another.
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A Boolean expression representing this XOR other.</returns>
    public Z3BoolExpr Xor(Z3BoolExpr other) => Context.Xor(this, other);

    /// <summary>
    /// Creates a Boolean expression representing equality with a bool value.
    /// </summary>
    /// <param name="other">The bool value to compare with.</param>
    /// <returns>A Boolean expression representing this == other.</returns>
    public Z3BoolExpr Eq(bool other) => Context.Eq(this, other);

    /// <summary>
    /// Creates a Boolean expression representing inequality with a bool value.
    /// </summary>
    /// <param name="other">The bool value to compare with.</param>
    /// <returns>A Boolean expression representing this != other.</returns>
    public Z3BoolExpr Neq(bool other) => Context.Neq(this, other);

    /// <summary>
    /// Creates a conditional expression (if-then-else) using this Boolean expression as the condition.
    /// </summary>
    /// <typeparam name="T">The type of expressions for the branches (must inherit from Z3Expr).</typeparam>
    /// <param name="thenExpr">The expression to return when this condition is true.</param>
    /// <param name="elseExpr">The expression to return when this condition is false.</param>
    /// <returns>An expression representing: if (this) then thenExpr else elseExpr.</returns>
    public T If<T>(T thenExpr, T elseExpr)
        where T : Z3Expr, IZ3ExprType<T> => Context.Ite(this, thenExpr, elseExpr);
}
