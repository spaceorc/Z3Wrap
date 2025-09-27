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
    #region Core Implementation

    private BoolExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BoolExpr IExprType<BoolExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<BoolExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkBoolSort(context.Handle);

    #endregion

    #region Implicit Conversions

    /// <summary>
    /// Implicitly converts a Boolean value to a BoolExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>A BoolExpr representing the Boolean constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BoolExpr(bool value) => Z3Context.Current.Bool(value);

    #endregion

    #region Logical Operations

    /// <summary>
    /// Creates a logical AND expression (this ∧ other).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A BoolExpr representing this AND other.</returns>
    public BoolExpr And(BoolExpr other) => Context.And(this, other);

    /// <summary>
    /// Creates a logical OR expression (this ∨ other).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A BoolExpr representing this OR other.</returns>
    public BoolExpr Or(BoolExpr other) => Context.Or(this, other);

    /// <summary>
    /// Creates a logical NOT expression (¬this).
    /// </summary>
    /// <returns>A BoolExpr representing NOT this.</returns>
    public BoolExpr Not() => Context.Not(this);

    /// <summary>
    /// Creates a logical implication expression (this → other).
    /// </summary>
    /// <param name="other">The consequent Boolean expression.</param>
    /// <returns>A BoolExpr representing this IMPLIES other.</returns>
    public BoolExpr Implies(BoolExpr other) => Context.Implies(this, other);

    /// <summary>
    /// Creates a logical equivalence expression (this ↔ other).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A BoolExpr representing this IFF other.</returns>
    public BoolExpr Iff(BoolExpr other) => Context.Iff(this, other);

    /// <summary>
    /// Creates a logical XOR expression (this ⊕ other).
    /// </summary>
    /// <param name="other">The other Boolean expression.</param>
    /// <returns>A BoolExpr representing this XOR other.</returns>
    public BoolExpr Xor(BoolExpr other) => Context.Xor(this, other);

    #endregion

    #region Logical Operators

    /// <summary>
    /// Logical AND operator (left ∧ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left AND right.</returns>
    public static BoolExpr operator &(BoolExpr left, BoolExpr right) => left.And(right);

    /// <summary>
    /// Logical OR operator (left ∨ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left OR right.</returns>
    public static BoolExpr operator |(BoolExpr left, BoolExpr right) => left.Or(right);

    /// <summary>
    /// Logical XOR operator (left ⊕ right).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A BoolExpr representing left XOR right.</returns>
    public static BoolExpr operator ^(BoolExpr left, BoolExpr right) => left.Xor(right);

    /// <summary>
    /// Logical NOT operator (¬expr).
    /// </summary>
    /// <param name="expr">The Boolean expression to negate.</param>
    /// <returns>A BoolExpr representing NOT expr.</returns>
    public static BoolExpr operator !(BoolExpr expr) => expr.Not();

    #endregion

    #region Equality Operators

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

    #endregion

    #region Functions

    /// <summary>
    /// Creates a conditional expression (if-then-else) using this Boolean expression as the condition.
    /// </summary>
    /// <typeparam name="T">The type of expressions for the branches (must inherit from Z3Expr).</typeparam>
    /// <param name="thenExpr">The expression to return when this condition is true.</param>
    /// <param name="elseExpr">The expression to return when this condition is false.</param>
    /// <returns>An expression representing: if (this) then thenExpr else elseExpr.</returns>
    public T Ite<T>(T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T> => Context.Ite(this, thenExpr, elseExpr);

    #endregion
}
#pragma warning restore CS0660, CS0661
