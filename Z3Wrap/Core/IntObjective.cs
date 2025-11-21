namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents an optimization objective for integer or bitvector expressions.
/// </summary>
public sealed class IntObjective
{
    /// <summary>
    /// Gets the Z3 objective identifier.
    /// </summary>
    internal uint ObjectiveId { get; }

    internal IntObjective(uint objectiveId)
    {
        ObjectiveId = objectiveId;
    }
}
