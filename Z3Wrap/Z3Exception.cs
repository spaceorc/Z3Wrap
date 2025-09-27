using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap;

/// <summary>
/// Exception thrown when a Z3 native operation fails.
/// </summary>
public class Z3Exception : Exception
{
    /// <summary>
    /// Gets the Z3 error code that caused this exception.
    /// </summary>
    public Z3ErrorCode ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the Z3Exception class.
    /// </summary>
    /// <param name="errorCode">The Z3 error code.</param>
    /// <param name="message">The error message.</param>
    public Z3Exception(Z3ErrorCode errorCode, string message)
        : base($"Z3 Error ({errorCode}): {message}")
    {
        ErrorCode = errorCode;
    }
}
