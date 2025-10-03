// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Miscellaneous API - P/Invoke bindings for Z3 miscellaneous functions
//
// Source: z3_api.h (Miscellaneous section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Miscellaneous API (0 functions - placeholder):
// - TODO: Add functions from c_headers/z3_api_miscellaneous.txt

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsMiscellaneous(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_enable_trace");
        LoadFunctionInternal(handle, functionPointers, "Z3_disable_trace");
        LoadFunctionInternal(handle, functionPointers, "Z3_finalize_memory");
        LoadFunctionInternal(handle, functionPointers, "Z3_reset_memory");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_version");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_full_version");
    }

    // Delegates
    private delegate void EnableTraceDelegate(IntPtr tag);
    private delegate void DisableTraceDelegate(IntPtr tag);
    private delegate void FinalizeMemoryDelegate();
    private delegate void ResetMemoryDelegate();
    private delegate void GetVersionDelegate(
        out uint major,
        out uint minor,
        out uint buildNumber,
        out uint revisionNumber
    );
    private delegate IntPtr GetFullVersionDelegate();

    // Methods

    /// <summary>
    /// Enables tracing for specific tag.
    /// </summary>
    /// <param name="tag">Trace tag to enable.</param>
    /// <remarks>
    /// Activates detailed tracing for Z3 subsystem identified by tag. Used for
    /// deep debugging of specific Z3 components. Output goes to stderr.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void EnableTrace(IntPtr tag)
    {
        var funcPtr = GetFunctionPointer("Z3_enable_trace");
        var func = Marshal.GetDelegateForFunctionPointer<EnableTraceDelegate>(funcPtr);
        func(tag);
    }

    /// <summary>
    /// Disables tracing for specific tag.
    /// </summary>
    /// <param name="tag">Trace tag to disable.</param>
    /// <remarks>
    /// Deactivates tracing previously enabled by EnableTrace. Used to reduce
    /// trace output volume when debugging specific issues.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DisableTrace(IntPtr tag)
    {
        var funcPtr = GetFunctionPointer("Z3_disable_trace");
        var func = Marshal.GetDelegateForFunctionPointer<DisableTraceDelegate>(funcPtr);
        func(tag);
    }

    /// <summary>
    /// Finalizes Z3 memory manager.
    /// </summary>
    /// <remarks>
    /// Performs final memory cleanup. Should be called before process exit to
    /// release all Z3 resources. No Z3 functions can be called after this.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FinalizeMemory()
    {
        var funcPtr = GetFunctionPointer("Z3_finalize_memory");
        var func = Marshal.GetDelegateForFunctionPointer<FinalizeMemoryDelegate>(funcPtr);
        func();
    }

    /// <summary>
    /// Resets Z3 internal memory manager.
    /// </summary>
    /// <remarks>
    /// Resets memory allocation statistics. Used for memory profiling and leak
    /// detection. Should only be called when no Z3 objects exist.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ResetMemory()
    {
        var funcPtr = GetFunctionPointer("Z3_reset_memory");
        var func = Marshal.GetDelegateForFunctionPointer<ResetMemoryDelegate>(funcPtr);
        func();
    }

    /// <summary>
    /// Gets Z3 version information.
    /// </summary>
    /// <param name="major">Output parameter for major version.</param>
    /// <param name="minor">Output parameter for minor version.</param>
    /// <param name="buildNumber">Output parameter for build number.</param>
    /// <param name="revisionNumber">Output parameter for revision number.</param>
    /// <remarks>
    /// Returns Z3 version as four components (e.g., 4.8.17.0). Use for compatibility
    /// checking and feature detection.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GetVersion(out uint major, out uint minor, out uint buildNumber, out uint revisionNumber)
    {
        var funcPtr = GetFunctionPointer("Z3_get_version");
        var func = Marshal.GetDelegateForFunctionPointer<GetVersionDelegate>(funcPtr);
        func(out major, out minor, out buildNumber, out revisionNumber);
    }

    /// <summary>
    /// Gets full Z3 version string.
    /// </summary>
    /// <returns>String containing complete version information.</returns>
    /// <remarks>
    /// Returns detailed version string including build information and commit hash.
    /// More comprehensive than GetVersion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetFullVersion()
    {
        var funcPtr = GetFunctionPointer("Z3_get_full_version");
        var func = Marshal.GetDelegateForFunctionPointer<GetFullVersionDelegate>(funcPtr);
        return func();
    }
}
