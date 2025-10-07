using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a typed optimization objective with compile-time type safety.
/// </summary>
/// <typeparam name="TExpr">The type of expression being optimized.</typeparam>
public sealed class OptimizeObjective<TExpr>
    where TExpr : Z3Expr, IOptimizableExpr
{
    internal OptimizeObjective(uint objectiveId)
    {
        ObjectiveId = objectiveId;
    }

    /// <summary>
    /// Gets the internal objective ID used by Z3.
    /// </summary>
    internal uint ObjectiveId { get; }
}
