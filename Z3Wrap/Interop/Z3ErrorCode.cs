namespace Spaceorc.Z3Wrap.Interop;

/// <summary>
/// Z3 error codes that can be returned by Z3 operations.
/// Maps directly to Z3's internal Z3_error_code enumeration from the C API.
/// </summary>
public enum Z3ErrorCode
{
    /// <summary>
    /// No error occurred (Z3_OK).
    /// All operations completed successfully.
    /// </summary>
    Ok = 0,

    /// <summary>
    /// User tried to build an invalid (type incorrect) AST (Z3_SORT_ERROR).
    /// This typically occurs when there are sort mismatches between expressions,
    /// such as comparing integers with bitvectors of different sizes.
    /// </summary>
    SortError = 1,

    /// <summary>
    /// Index out of bounds (Z3_IOB).
    /// An attempt was made to access an array or collection element
    /// using an index that exceeds the valid range.
    /// </summary>
    IndexOutOfBounds = 2,

    /// <summary>
    /// Invalid argument was provided to a Z3 function (Z3_INVALID_ARG).
    /// This occurs when function parameters don't meet the expected requirements,
    /// such as passing constants instead of bound variables to quantifiers.
    /// </summary>
    InvalidArgument = 3,

    /// <summary>
    /// An error occurred when parsing a string or file (Z3_PARSER_ERROR).
    /// The input contains syntax errors or invalid Z3 expressions.
    /// </summary>
    ParserError = 4,

    /// <summary>
    /// Parser output is not available (Z3_NO_PARSER).
    /// The parser failed to produce a result or no parser is configured.
    /// </summary>
    NoParser = 5,

    /// <summary>
    /// Invalid pattern was used to build a quantifier (Z3_INVALID_PATTERN).
    /// Quantifier patterns must follow specific rules for trigger selection.
    /// </summary>
    InvalidPattern = 6,

    /// <summary>
    /// A memory allocation failure was encountered (Z3_MEMOUT_FAIL).
    /// Z3 ran out of available memory during operation.
    /// </summary>
    MemoryFailure = 7,

    /// <summary>
    /// A file could not be accessed (Z3_FILE_ACCESS_ERROR).
    /// File system permissions or file not found errors.
    /// </summary>
    FileAccessError = 8,

    /// <summary>
    /// An error internal to Z3 occurred (Z3_INTERNAL_FATAL).
    /// This indicates a bug in the Z3 theorem prover itself.
    /// </summary>
    InternalFatal = 9,

    /// <summary>
    /// API call is invalid in the current state (Z3_INVALID_USAGE).
    /// The operation cannot be performed with the current solver state
    /// or context configuration.
    /// </summary>
    InvalidUsage = 10,

    /// <summary>
    /// Trying to decrement the reference counter of an AST that was deleted (Z3_DEC_REF_ERROR).
    /// This indicates improper memory management of Z3 expressions.
    /// </summary>
    DecRefError = 11,

    /// <summary>
    /// Internal Z3 exception occurred (Z3_EXCEPTION).
    /// Additional details can be retrieved using Z3_get_error_msg().
    /// </summary>
    Exception = 12,
}
