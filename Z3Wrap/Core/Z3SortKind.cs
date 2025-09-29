namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents the different types of sorts (data types) supported by Z3.
/// Maps directly to Z3's internal sort kind enumeration.
/// </summary>
public enum Z3SortKind
{
    /// <summary>
    /// Uninterpreted sort - a free type with no predefined interpretation (Z3_UNINTERPRETED_SORT).
    /// </summary>
    Uninterpreted = 0,

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
    Bv = 4,

    /// <summary>
    /// Array sort for indexed collections (Z3_ARRAY_SORT).
    /// </summary>
    Array = 5,

    /// <summary>
    /// Datatype sort for algebraic data types like lists, trees, records, or enumerations (Z3_DATATYPE_SORT).
    /// </summary>
    Datatype = 6,

    /// <summary>
    /// Relation sort for relational data structures (Z3_RELATION_SORT).
    /// </summary>
    Relation = 7,

    /// <summary>
    /// Finite domain sort with a fixed, limited number of possible values (Z3_FINITE_DOMAIN_SORT).
    /// </summary>
    FiniteDomain = 8,

    /// <summary>
    /// Floating-point sort for IEEE 754 floating-point numbers (Z3_FLOATING_POINT_SORT).
    /// </summary>
    FloatingPoint = 9,

    /// <summary>
    /// Rounding mode sort for floating-point rounding modes (Z3_ROUNDING_MODE_SORT).
    /// </summary>
    RoundingMode = 10,

    /// <summary>
    /// Sequence sort for ordered collections of elements (Z3_SEQ_SORT).
    /// </summary>
    Seq = 11,

    /// <summary>
    /// Regular expression sort (Z3_RE_SORT).
    /// </summary>
    Re = 12,

    /// <summary>
    /// Character sort (Z3_CHAR_SORT).
    /// </summary>
    Char = 13,

    /// <summary>
    /// Type variable for generic or polymorphic type definitions (Z3_TYPE_VAR).
    /// </summary>
    TypeVar = 14,

    /// <summary>
    /// Unknown sort - used when the specific type cannot be determined (Z3_UNKNOWN_SORT).
    /// </summary>
    Unknown = 1000,
}
