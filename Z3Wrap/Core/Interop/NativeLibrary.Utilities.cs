// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Utilities API - P/Invoke bindings for Z3 translation and conversion functions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's translation and AST manipulation functions (2 functions):
// - Translation: Z3_translate (copy AST between contexts)
// - AST Manipulation: Z3_update_term (replace term arguments)
//
// Note: Other utility functions have been distributed to specialized files:
// - Version info: NativeLibrary.Miscellaneous.cs
// - Logging: NativeLibrary.InteractionLogging.cs
// - String conversion: NativeLibrary.StringConversion.cs
// - Error handling: NativeLibrary.ErrorHandling.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsUtilities(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Translation and Conversion
        LoadFunctionOrNull(handle, functionPointers, "Z3_translate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_update_term");
    }

    // Delegates

    // Translation and Conversion
    private delegate IntPtr TranslateDelegate(IntPtr sourceCtx, IntPtr ast, IntPtr targetCtx);
    private delegate IntPtr UpdateTermDelegate(IntPtr ctx, IntPtr ast, uint numArgs, IntPtr[] args);

    // Methods

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
}
