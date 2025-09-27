namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Represents the three-valued Boolean logic used by Z3 for Boolean expressions.
/// Maps directly to Z3's internal Boolean value representation.
/// </summary>
public enum Z3BoolValue
{
    /// <summary>
    /// Represents false (Z3_L_FALSE).
    /// </summary>
    False = -1,

    /// <summary>
    /// Represents an undefined or unknown Boolean value (Z3_L_UNDEF).
    /// This occurs when Z3 cannot determine the truth value.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Represents true (Z3_L_TRUE).
    /// </summary>
    True = 1,
}
