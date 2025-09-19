namespace Spaceorc.Z3Wrap;

/// <summary>
/// Represents the result of a satisfiability check performed by a Z3 solver.
/// </summary>
public enum Z3Status
{
    /// <summary>
    /// The constraints are unsatisfiable (no solution exists).
    /// </summary>
    Unsatisfiable,

    /// <summary>
    /// The satisfiability is unknown (solver could not determine the result).
    /// Use GetReasonUnknown() to get more information about why the result is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// The constraints are satisfiable (at least one solution exists).
    /// A model can be retrieved using GetModel().
    /// </summary>
    Satisfiable,
}