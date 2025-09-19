namespace Spaceorc.Z3Wrap.Interop;

/// <summary>
/// Represents the different types of sorts (data types) supported by Z3.
/// Maps directly to Z3's internal sort kind enumeration.
/// </summary>
public enum Z3SortKind
{
    /// <summary>
    /// Boolean sort for true/false values (Z3_BOOL_SORT).
    /// </summary>
    Bool = 1,

    /// <summary>
    /// Integer sort for unlimited precision integers (Z3_INT_SORT).
    /// </summary>
    Int = 2,

    /// <summary>
    /// Real sort for exact rational numbers (Z3_REAL_SORT).
    /// </summary>
    Real = 3,

    /// <summary>
    /// Bitvector sort for fixed-width binary sequences (Z3_BV_SORT).
    /// </summary>
    BV = 4,

    /// <summary>
    /// Array sort for indexed collections (Z3_ARRAY_SORT).
    /// </summary>
    Array = 5,
}