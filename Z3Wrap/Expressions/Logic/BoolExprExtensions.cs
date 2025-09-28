using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides logical operations for boolean expressions.
/// </summary>
public static class BoolExprExtensions
{
    /// <summary>
    /// Creates logical AND operation for this boolean expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>Boolean expression representing the logical AND.</returns>
    public static BoolExpr And(this BoolExpr left, BoolExpr other) => left.Context.And(left, other);

    /// <summary>
    /// Creates logical OR operation for this boolean expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>Boolean expression representing the logical OR.</returns>
    public static BoolExpr Or(this BoolExpr left, BoolExpr other) => left.Context.Or(left, other);

    /// <summary>
    /// Creates logical NOT operation for this boolean expression.
    /// </summary>
    /// <param name="operand">The operand.</param>
    /// <returns>Boolean expression representing the logical NOT.</returns>
    public static BoolExpr Not(this BoolExpr operand) => operand.Context.Not(operand);

    /// <summary>
    /// Creates logical implication for this boolean expression.
    /// </summary>
    /// <param name="left">The antecedent.</param>
    /// <param name="other">The consequent.</param>
    /// <returns>Boolean expression representing the implication.</returns>
    public static BoolExpr Implies(this BoolExpr left, BoolExpr other) => left.Context.Implies(left, other);

    /// <summary>
    /// Creates logical if-and-only-if for this boolean expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>Boolean expression representing the if-and-only-if.</returns>
    public static BoolExpr Iff(this BoolExpr left, BoolExpr other) => left.Context.Iff(left, other);

    /// <summary>
    /// Creates logical exclusive OR for this boolean expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>Boolean expression representing the exclusive OR.</returns>
    public static BoolExpr Xor(this BoolExpr left, BoolExpr other) => left.Context.Xor(left, other);

    /// <summary>
    /// Creates if-then-else operation for this boolean condition.
    /// </summary>
    /// <typeparam name="T">The expression type.</typeparam>
    /// <param name="condition">The condition.</param>
    /// <param name="thenExpr">The then expression.</param>
    /// <param name="elseExpr">The else expression.</param>
    /// <returns>Expression representing the conditional selection.</returns>
    public static T Ite<T>(this BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T> => condition.Context.Ite(condition, thenExpr, elseExpr);
}
