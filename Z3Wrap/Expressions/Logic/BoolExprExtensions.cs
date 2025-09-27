using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Extension methods for Boolean expressions.
/// </summary>
public static class BoolExprExtensions
{
    /// <summary>
    /// Creates logical AND expression.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="other">Right operand.</param>
    /// <returns>Boolean expression representing left ∧ other.</returns>
    public static BoolExpr And(this BoolExpr left, BoolExpr other) => left.Context.And(left, other);

    /// <summary>
    /// Creates logical OR expression.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="other">Right operand.</param>
    /// <returns>Boolean expression representing left ∨ other.</returns>
    public static BoolExpr Or(this BoolExpr left, BoolExpr other) => left.Context.Or(left, other);

    /// <summary>
    /// Creates logical NOT expression.
    /// </summary>
    /// <param name="operand">Expression to negate.</param>
    /// <returns>Boolean expression representing ¬operand.</returns>
    public static BoolExpr Not(this BoolExpr operand) => operand.Context.Not(operand);

    /// <summary>
    /// Creates logical implication expression.
    /// </summary>
    /// <param name="left">Antecedent.</param>
    /// <param name="other">Consequent.</param>
    /// <returns>Boolean expression representing left → other.</returns>
    public static BoolExpr Implies(this BoolExpr left, BoolExpr other) =>
        left.Context.Implies(left, other);

    /// <summary>
    /// Creates logical equivalence expression.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="other">Right operand.</param>
    /// <returns>Boolean expression representing left ↔ other.</returns>
    public static BoolExpr Iff(this BoolExpr left, BoolExpr other) => left.Context.Iff(left, other);

    /// <summary>
    /// Creates logical XOR expression.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="other">Right operand.</param>
    /// <returns>Boolean expression representing left ⊕ other.</returns>
    public static BoolExpr Xor(this BoolExpr left, BoolExpr other) => left.Context.Xor(left, other);

    /// <summary>
    /// Creates conditional expression using Boolean as condition.
    /// </summary>
    /// <typeparam name="T">Expression type for branches.</typeparam>
    /// <param name="condition">Boolean condition.</param>
    /// <param name="thenExpr">Expression when condition is true.</param>
    /// <param name="elseExpr">Expression when condition is false.</param>
    /// <returns>Expression representing if condition then thenExpr else elseExpr.</returns>
    public static T Ite<T>(this BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T> => condition.Context.Ite(condition, thenExpr, elseExpr);
}
