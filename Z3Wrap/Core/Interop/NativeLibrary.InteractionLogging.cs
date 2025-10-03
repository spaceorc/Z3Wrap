// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 InteractionLogging API - P/Invoke bindings for Z3 interaction_logging functions
//
// Source: z3_api.h (InteractionLogging section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's InteractionLogging API (0 functions - placeholder):
// - TODO: Add functions from c_headers/z3_api_interaction_logging.txt
//
// Missing Functions (0 functions):
// None - all functions implemented ✓

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsInteractionLogging(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_open_log");
        LoadFunctionInternal(handle, functionPointers, "Z3_close_log");
        LoadFunctionInternal(handle, functionPointers, "Z3_append_log");
        LoadFunctionInternal(handle, functionPointers, "Z3_toggle_warning_messages");
    }

    // Delegates
    private delegate bool OpenLogDelegate(IntPtr filename);
    private delegate void CloseLogDelegate();
    private delegate void AppendLogDelegate(IntPtr str);
    private delegate void ToggleWarningMessagesDelegate(bool enabled);

    // Methods

    /// <summary>
    /// Opens log file for Z3 operations.
    /// </summary>
    /// <param name="filename">Path to log file.</param>
    /// <returns>True if successful, false otherwise.</returns>
    /// <remarks>
    /// Creates log file recording Z3 operations. Used for debugging and performance
    /// analysis. Must call CloseLog when done.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool OpenLog(IntPtr filename)
    {
        var funcPtr = GetFunctionPointer("Z3_open_log");
        var func = Marshal.GetDelegateForFunctionPointer<OpenLogDelegate>(funcPtr);
        return func(filename);
    }

    /// <summary>
    /// Closes currently open log file.
    /// </summary>
    /// <remarks>
    /// Flushes and closes log file opened by OpenLog. Should be called before
    /// application exit to ensure log is properly written.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void CloseLog()
    {
        var funcPtr = GetFunctionPointer("Z3_close_log");
        var func = Marshal.GetDelegateForFunctionPointer<CloseLogDelegate>(funcPtr);
        func();
    }

    /// <summary>
    /// Appends message to log file.
    /// </summary>
    /// <param name="str">String to append to log.</param>
    /// <remarks>
    /// Writes custom message to currently open log file. Used for annotating logs
    /// with application-specific information.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AppendLog(IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_append_log");
        var func = Marshal.GetDelegateForFunctionPointer<AppendLogDelegate>(funcPtr);
        func(str);
    }

    /// <summary>
    /// Toggles warning message output.
    /// </summary>
    /// <param name="enabled">True to enable warnings, false to disable.</param>
    /// <remarks>
    /// Controls whether Z3 prints warning messages to stderr. Disable for cleaner
    /// output or when warnings are expected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ToggleWarningMessages(bool enabled)
    {
        var funcPtr = GetFunctionPointer("Z3_toggle_warning_messages");
        var func = Marshal.GetDelegateForFunctionPointer<ToggleWarningMessagesDelegate>(funcPtr);
        func(enabled);
    }
}
