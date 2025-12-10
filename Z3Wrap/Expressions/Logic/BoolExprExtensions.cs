using System.Diagnostics.CodeAnalysis;
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
    /// <param name="others">Additional operands to combine with AND.</param>
    /// <returns>Boolean expression representing the logical AND.</returns>
    public static BoolExpr And(this BoolExpr left, params IEnumerable<BoolExpr> others) =>
        left.Context.And([left, .. others]);

    /// <summary>
    /// Creates logical OR operation for this boolean expression.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="others">Additional operands to combine with OR.</param>
    /// <returns>Boolean expression representing the logical OR.</returns>
    public static BoolExpr Or(this BoolExpr left, params IEnumerable<BoolExpr> others) =>
        left.Context.Or([left, .. others]);

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

    /// <summary>
    /// Determines whether all elements satisfy a condition, like LINQ's All(predicate).
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>Boolean expression representing the conjunction of predicates.</returns>
    public static BoolExpr All<TSource>(this IEnumerable<TSource> source, Func<TSource, BoolExpr> predicate)
    {
        return Z3Context.Current.And(source.Select(predicate));
    }

    /// <summary>
    /// Determines whether any element satisfies a condition, like LINQ's Any(predicate).
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>Boolean expression representing the disjunction of predicates.</returns>
    public static BoolExpr Any<TSource>(this IEnumerable<TSource> source, Func<TSource, BoolExpr> predicate)
    {
        return Z3Context.Current.Or(source.Select(predicate));
    }
}
