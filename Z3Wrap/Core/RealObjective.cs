namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents an optimization objective for real number expressions.
/// </summary>
public sealed class RealObjective
{
    /// <summary>
    /// Gets the Z3 objective identifier.
    /// </summary>
    internal uint ObjectiveId { get; }

    internal RealObjective(uint objectiveId)
    {
        ObjectiveId = objectiveId;
    }
}
