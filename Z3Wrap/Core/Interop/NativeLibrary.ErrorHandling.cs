using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for specific API functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for this group of Z3 API functions.
    /// </summary>
    private static void LoadFunctionsErrorHandling(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_get_error_code");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_error_msg");
        LoadFunctionInternal(handle, functionPointers, "Z3_set_error_handler");
    }

    // Delegates
    private delegate int GetErrorCodeDelegate(IntPtr ctx);
    private delegate IntPtr GetErrorMsgDelegate(IntPtr ctx, int errorCode);
    private delegate void SetErrorHandlerDelegate(IntPtr ctx, ErrorHandlerDelegate? handler);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ErrorHandlerDelegate(IntPtr ctx, int errorCode);

    // Methods

    /// <summary>
    /// Retrieves the error code from the last Z3 operation on the context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>The error code from the last operation.</returns>
    /// <remarks>
    /// Returns Z3_OK if no error occurred. Use GetErrorMsg to get
    /// a human-readable description of the error.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal Z3ErrorCode GetErrorCode(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_error_code");
        var func = Marshal.GetDelegateForFunctionPointer<GetErrorCodeDelegate>(funcPtr);
        return (Z3ErrorCode)func(ctx);
    }

    /// <summary>
    /// Retrieves a human-readable error message for the given error code.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="errorCode">The error code to get a message for.</param>
    /// <returns>String description of the error, or "Unknown error" if unavailable.</returns>
    /// <remarks>
    /// Provides detailed information about Z3 errors for debugging and user feedback.
    /// The returned string is managed by Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetErrorMsg(IntPtr ctx, Z3ErrorCode errorCode)
    {
        var funcPtr = GetFunctionPointer("Z3_get_error_msg");
        var func = Marshal.GetDelegateForFunctionPointer<GetErrorMsgDelegate>(funcPtr);
        return func(ctx, (int)errorCode);
    }

    /// <summary>
    /// Sets a custom error handler for the Z3 context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="handler">The error handler delegate, or null to remove current handler.</param>
    /// <remarks>
    /// The error handler is called when Z3 encounters internal errors.
    /// Provides custom error handling instead of default Z3 behavior.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SetErrorHandler(IntPtr ctx, ErrorHandlerDelegate? handler)
    {
        var funcPtr = GetFunctionPointer("Z3_set_error_handler");
        var func = Marshal.GetDelegateForFunctionPointer<SetErrorHandlerDelegate>(funcPtr);
        func(ctx, handler);
    }
}
