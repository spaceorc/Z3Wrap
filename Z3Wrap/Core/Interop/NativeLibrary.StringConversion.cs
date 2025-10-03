// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 StringConversion API - P/Invoke bindings for Z3 string_conversion functions
//
// Source: z3_api.h (StringConversion section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's StringConversion API (4 functions):
// - String conversion for AST objects (Z3_func_decl_to_string, Z3_pattern_to_string, Z3_sort_to_string)
// - Print mode control (Z3_set_ast_print_mode)
//
// Missing Functions (3 functions):
// - Z3_ast_to_string
// - Z3_benchmark_to_smtlib_string
// - Z3_model_to_string

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsStringConversion(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_func_decl_to_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_pattern_to_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_sort_to_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_set_ast_print_mode");
    }

    // Delegates
    private delegate IntPtr FuncDeclToStringDelegate(IntPtr ctx, IntPtr funcDecl);
    private delegate IntPtr PatternToStringDelegate(IntPtr ctx, IntPtr pattern);
    private delegate IntPtr SortToStringDelegate(IntPtr ctx, IntPtr sort);
    private delegate void SetAstPrintModeDelegate(IntPtr ctx, int mode);

    // Methods

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
}
