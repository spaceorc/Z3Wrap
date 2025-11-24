using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides pseudo-boolean constraint methods for Z3Context.
/// </summary>
public static class PseudoBooleanContextExtensions
{
    /// <summary>
    /// Creates constraint that at most k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Maximum number of expressions that can be true.</param>
    /// <returns>Boolean expression representing the at-most constraint.</returns>
    /// <remarks>
    /// Encodes: (count of true values in expressions) &lt;= k
    /// Useful for resource allocation, scheduling, and combinatorial constraints.
    /// </remarks>
    public static BoolExpr AtMost(this Z3Context context, BoolExpr[] expressions, uint k)
    {
        if (expressions.Length == 0)
            throw new ArgumentException("At least one expression is required", nameof(expressions));

        var handles = expressions.Select(e => e.Handle).ToArray();
        var handle = context.Library.MkAtmost(context.Handle, (uint)expressions.Length, handles, k);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates constraint that at most k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Maximum number of expressions that can be true.</param>
    /// <returns>Boolean expression representing the at-most constraint.</returns>
    public static BoolExpr AtMost(this Z3Context context, BoolExpr[] expressions, int k)
    {
        if (k < 0)
            throw new ArgumentOutOfRangeException(nameof(k), "k must be non-negative");
        return context.AtMost(expressions, (uint)k);
    }

    /// <summary>
    /// Creates constraint that at least k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Minimum number of expressions that must be true.</param>
    /// <returns>Boolean expression representing the at-least constraint.</returns>
    /// <remarks>
    /// Encodes: (count of true values in expressions) &gt;= k
    /// Useful for requirement satisfaction, minimum resource allocation.
    /// </remarks>
    public static BoolExpr AtLeast(this Z3Context context, BoolExpr[] expressions, uint k)
    {
        if (expressions.Length == 0)
            throw new ArgumentException("At least one expression is required", nameof(expressions));

        var handles = expressions.Select(e => e.Handle).ToArray();
        var handle = context.Library.MkAtleast(context.Handle, (uint)expressions.Length, handles, k);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates constraint that at least k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Minimum number of expressions that must be true.</param>
    /// <returns>Boolean expression representing the at-least constraint.</returns>
    public static BoolExpr AtLeast(this Z3Context context, BoolExpr[] expressions, int k)
    {
        if (k < 0)
            throw new ArgumentOutOfRangeException(nameof(k), "k must be non-negative");
        return context.AtLeast(expressions, (uint)k);
    }

    /// <summary>
    /// Creates constraint that exactly k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Exact number of expressions that must be true.</param>
    /// <returns>Boolean expression representing the exactly-k constraint.</returns>
    /// <remarks>
    /// Encodes: (count of true values in expressions) = k
    /// Equivalent to AtLeast(k) AND AtMost(k).
    /// </remarks>
    public static BoolExpr Exactly(this Z3Context context, BoolExpr[] expressions, uint k)
    {
        return context.And(context.AtLeast(expressions, k), context.AtMost(expressions, k));
    }

    /// <summary>
    /// Creates constraint that exactly k of the boolean expressions are true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Exact number of expressions that must be true.</param>
    /// <returns>Boolean expression representing the exactly-k constraint.</returns>
    public static BoolExpr Exactly(this Z3Context context, BoolExpr[] expressions, int k)
    {
        if (k < 0)
            throw new ArgumentOutOfRangeException(nameof(k), "k must be non-negative");
        return context.Exactly(expressions, (uint)k);
    }

    /// <summary>
    /// Creates weighted pseudo-boolean constraint: coeffs[0]*exprs[0] + ... + coeffs[n-1]*exprs[n-1] &lt;= k.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="coefficients">Weight coefficients for each boolean expression.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Upper bound for the weighted sum.</param>
    /// <returns>Boolean expression representing the weighted less-than-or-equal constraint.</returns>
    /// <remarks>
    /// Each true boolean contributes its coefficient to the sum.
    /// Useful for budget constraints, resource allocation with costs.
    /// </remarks>
    public static BoolExpr PbLe(this Z3Context context, int[] coefficients, BoolExpr[] expressions, int k)
    {
        if (coefficients.Length != expressions.Length)
            throw new ArgumentException("Coefficients and expressions must have the same length", nameof(coefficients));
        if (expressions.Length == 0)
            throw new ArgumentException("At least one expression is required", nameof(expressions));

        var handles = expressions.Select(e => e.Handle).ToArray();
        var handle = context.Library.MkPble(context.Handle, (uint)expressions.Length, handles, coefficients, k);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates weighted pseudo-boolean constraint: coeffs[0]*exprs[0] + ... + coeffs[n-1]*exprs[n-1] &gt;= k.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="coefficients">Weight coefficients for each boolean expression.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Lower bound for the weighted sum.</param>
    /// <returns>Boolean expression representing the weighted greater-than-or-equal constraint.</returns>
    /// <remarks>
    /// Each true boolean contributes its coefficient to the sum.
    /// Useful for minimum requirement constraints with weighted values.
    /// </remarks>
    public static BoolExpr PbGe(this Z3Context context, int[] coefficients, BoolExpr[] expressions, int k)
    {
        if (coefficients.Length != expressions.Length)
            throw new ArgumentException("Coefficients and expressions must have the same length", nameof(coefficients));
        if (expressions.Length == 0)
            throw new ArgumentException("At least one expression is required", nameof(expressions));

        var handles = expressions.Select(e => e.Handle).ToArray();
        var handle = context.Library.MkPbge(context.Handle, (uint)expressions.Length, handles, coefficients, k);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates weighted pseudo-boolean constraint: coeffs[0]*exprs[0] + ... + coeffs[n-1]*exprs[n-1] = k.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="coefficients">Weight coefficients for each boolean expression.</param>
    /// <param name="expressions">The boolean expressions.</param>
    /// <param name="k">Exact value for the weighted sum.</param>
    /// <returns>Boolean expression representing the weighted equality constraint.</returns>
    /// <remarks>
    /// Each true boolean contributes its coefficient to the sum.
    /// Useful for exact budget constraints, precise resource allocation.
    /// </remarks>
    public static BoolExpr PbEq(this Z3Context context, int[] coefficients, BoolExpr[] expressions, int k)
    {
        if (coefficients.Length != expressions.Length)
            throw new ArgumentException("Coefficients and expressions must have the same length", nameof(coefficients));
        if (expressions.Length == 0)
            throw new ArgumentException("At least one expression is required", nameof(expressions));

        var handles = expressions.Select(e => e.Handle).ToArray();
        var handle = context.Library.MkPbeq(context.Handle, (uint)expressions.Length, handles, coefficients, k);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
