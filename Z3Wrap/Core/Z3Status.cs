namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents the result status of Z3 solver operations.
/// </summary>
public enum Z3Status
{
    /// <summary>
    /// The constraints are unsatisfiable.
    /// </summary>
    Unsatisfiable,

    /// <summary>
    /// The solver could not determine satisfiability within resource limits.
    /// </summary>
    Unknown,

    /// <summary>
    /// The constraints are satisfiable.
    /// </summary>
    Satisfiable,
}
