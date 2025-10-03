// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Utilities API - P/Invoke bindings for Z3 utility and miscellaneous functions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Utilities API (17 out of 20 functions, 85% complete):
// - Version Information: Z3_get_version, Z3_get_full_version
// - Logging and Tracing: Z3_open_log, Z3_append_log, Z3_close_log, Z3_enable_trace, Z3_disable_trace
// - Memory Management: Z3_reset_memory, Z3_finalize_memory
// - Translation and Conversion: Z3_translate, Z3_update_term
// - String Conversions: Z3_sort_to_string, Z3_func_decl_to_string, Z3_pattern_to_string
// - Miscellaneous: Z3_set_error, Z3_set_ast_print_mode, Z3_toggle_warning_messages
//
// Missing functions (3):
// - Z3_ast_to_string (generic AST to string)
// - Z3_model_to_string (model to string, may be in Model.cs)
// - Z3_benchmark_to_smtlib_string (benchmark formatting, specialized use case)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsUtilities(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Version Information
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_version");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_full_version");

        // Logging and Tracing
        LoadFunctionOrNull(handle, functionPointers, "Z3_open_log");
        LoadFunctionOrNull(handle, functionPointers, "Z3_append_log");
        LoadFunctionOrNull(handle, functionPointers, "Z3_close_log");
        LoadFunctionOrNull(handle, functionPointers, "Z3_enable_trace");
        LoadFunctionOrNull(handle, functionPointers, "Z3_disable_trace");

        // Memory Management
        LoadFunctionOrNull(handle, functionPointers, "Z3_reset_memory");
        LoadFunctionOrNull(handle, functionPointers, "Z3_finalize_memory");

        // Translation and Conversion
        LoadFunctionOrNull(handle, functionPointers, "Z3_translate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_update_term");

        // String Conversions
        LoadFunctionOrNull(handle, functionPointers, "Z3_sort_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_decl_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_pattern_to_string");

        // Miscellaneous
        LoadFunctionOrNull(handle, functionPointers, "Z3_set_error");
        LoadFunctionOrNull(handle, functionPointers, "Z3_set_ast_print_mode");
        LoadFunctionOrNull(handle, functionPointers, "Z3_toggle_warning_messages");
    }

    // Delegates

    // Version Information
    private delegate void GetVersionDelegate(
        out uint major,
        out uint minor,
        out uint buildNumber,
        out uint revisionNumber
    );
    private delegate IntPtr GetFullVersionDelegate();

    // Logging and Tracing
    private delegate bool OpenLogDelegate(IntPtr filename);
    private delegate void AppendLogDelegate(IntPtr str);
    private delegate void CloseLogDelegate();
    private delegate void EnableTraceDelegate(IntPtr tag);
    private delegate void DisableTraceDelegate(IntPtr tag);

    // Memory Management
    private delegate void ResetMemoryDelegate();
    private delegate void FinalizeMemoryDelegate();

    // Translation and Conversion
    private delegate IntPtr TranslateDelegate(IntPtr sourceCtx, IntPtr ast, IntPtr targetCtx);
    private delegate IntPtr UpdateTermDelegate(IntPtr ctx, IntPtr ast, uint numArgs, IntPtr[] args);

    // String Conversions
    private delegate IntPtr SortToStringDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr FuncDeclToStringDelegate(IntPtr ctx, IntPtr funcDecl);
    private delegate IntPtr PatternToStringDelegate(IntPtr ctx, IntPtr pattern);

    // Miscellaneous
    private delegate void SetErrorDelegate(IntPtr ctx, int errorCode);
    private delegate void SetAstPrintModeDelegate(IntPtr ctx, int mode);
    private delegate void ToggleWarningMessagesDelegate(bool enabled);

    // Methods

    // Version Information

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

    // Logging and Tracing

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

    // Memory Management

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

    // Translation and Conversion

    /// <summary>
    /// Translates AST from one context to another.
    /// </summary>
    /// <param name="sourceCtx">Source context handle.</param>
    /// <param name="ast">AST to translate.</param>
    /// <param name="targetCtx">Target context handle.</param>
    /// <returns>Translated AST in target context.</returns>
    /// <remarks>
    /// Copies AST from source context to target context. Handles all dependencies
    /// (sorts, function declarations). Used for sharing ASTs across contexts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Translate(IntPtr sourceCtx, IntPtr ast, IntPtr targetCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_translate");
        var func = Marshal.GetDelegateForFunctionPointer<TranslateDelegate>(funcPtr);
        return func(sourceCtx, ast, targetCtx);
    }

    /// <summary>
    /// Updates term with new arguments.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The term to update.</param>
    /// <param name="numArgs">Number of new arguments.</param>
    /// <param name="args">Array of new argument ASTs.</param>
    /// <returns>New AST with updated arguments.</returns>
    /// <remarks>
    /// Creates new term by replacing arguments of existing term. Preserves function
    /// declaration but substitutes arguments. Used for AST manipulation and rewriting.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr UpdateTerm(IntPtr ctx, IntPtr ast, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_update_term");
        var func = Marshal.GetDelegateForFunctionPointer<UpdateTermDelegate>(funcPtr);
        return func(ctx, ast, numArgs, args);
    }

    // String Conversions

    /// <summary>
    /// Converts sort to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The sort to convert.</param>
    /// <returns>String representation of sort.</returns>
    /// <remarks>
    /// Returns human-readable string representation of sort (e.g., "Int", "Array Int Int").
    /// Used for debugging and display purposes.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SortToString(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_sort_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<SortToStringDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Converts function declaration to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcDecl">The function declaration to convert.</param>
    /// <returns>String representation of function declaration.</returns>
    /// <remarks>
    /// Returns human-readable string showing function signature (name, domain, range).
    /// Used for debugging and display purposes.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncDeclToString(IntPtr ctx, IntPtr funcDecl)
    {
        var funcPtr = GetFunctionPointer("Z3_func_decl_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<FuncDeclToStringDelegate>(funcPtr);
        return func(ctx, funcDecl);
    }

    /// <summary>
    /// Converts quantifier pattern to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">The pattern to convert.</param>
    /// <returns>String representation of pattern.</returns>
    /// <remarks>
    /// Returns human-readable string showing quantifier instantiation pattern.
    /// Used for debugging quantifier issues.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html">Z3 C API Documentation</seealso>
    internal IntPtr PatternToString(IntPtr ctx, IntPtr pattern)
    {
        var funcPtr = GetFunctionPointer("Z3_pattern_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<PatternToStringDelegate>(funcPtr);
        return func(ctx, pattern);
    }

    // Miscellaneous

    /// <summary>
    /// Sets error code for context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="errorCode">Error code to set.</param>
    /// <remarks>
    /// Manually sets error state. Used for error handling in user callbacks and
    /// custom theory implementations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SetError(IntPtr ctx, int errorCode)
    {
        var funcPtr = GetFunctionPointer("Z3_set_error");
        var func = Marshal.GetDelegateForFunctionPointer<SetErrorDelegate>(funcPtr);
        func(ctx, errorCode);
    }

    /// <summary>
    /// Sets AST printing mode for context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="mode">Print mode (0 = default, 1 = low-level, 2 = SMTLIB2).</param>
    /// <remarks>
    /// Controls format used by AST ToString methods. Mode 0 uses default notation,
    /// mode 1 shows internal structure, mode 2 uses SMTLIB2 syntax.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SetAstPrintMode(IntPtr ctx, int mode)
    {
        var funcPtr = GetFunctionPointer("Z3_set_ast_print_mode");
        var func = Marshal.GetDelegateForFunctionPointer<SetAstPrintModeDelegate>(funcPtr);
        func(ctx, mode);
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
