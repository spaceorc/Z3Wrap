// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Modifiers API - P/Invoke bindings for Z3 AST modification functions
//
// Source: z3_api.h (Modifiers section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Modifiers API (5 functions):
// - Expression substitution (Z3_substitute)
// - Bound variable substitution (Z3_substitute_vars)
// - Function substitution (Z3_substitute_funs)
// - Context translation (Z3_translate)
// - Term argument updates (Z3_update_term)
//
// Missing Functions (0 functions):

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsModifiers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_substitute");
        LoadFunctionInternal(handle, functionPointers, "Z3_substitute_vars");
        LoadFunctionInternal(handle, functionPointers, "Z3_substitute_funs");
        LoadFunctionInternal(handle, functionPointers, "Z3_translate");
        LoadFunctionInternal(handle, functionPointers, "Z3_update_term");
    }

    // Delegates
    private delegate IntPtr SubstituteDelegate(IntPtr ctx, IntPtr ast, uint numExprs, IntPtr[] from, IntPtr[] to);
    private delegate IntPtr SubstituteVarsDelegate(IntPtr ctx, IntPtr ast, uint numVars, IntPtr[] to);
    private delegate IntPtr SubstituteFunsDelegate(IntPtr ctx, IntPtr ast, uint numFuns, IntPtr[] from, IntPtr[] to);
    private delegate IntPtr TranslateDelegate(IntPtr sourceCtx, IntPtr ast, IntPtr targetCtx);
    private delegate IntPtr UpdateTermDelegate(IntPtr ctx, IntPtr ast, uint numArgs, IntPtr[] args);

    // Methods

    /// <summary>
    /// Substitutes subexpressions in AST.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST expression to perform substitution on.</param>
    /// <param name="numExprs">Number of expressions to substitute.</param>
    /// <param name="from">Array of source expressions to replace.</param>
    /// <param name="to">Array of target expressions to substitute in.</param>
    /// <returns>New AST with substitutions applied.</returns>
    /// <remarks>
    /// Replaces all occurrences of expressions in 'from' array with corresponding
    /// expressions in 'to' array. Arrays must be same length. Substitution is
    /// simultaneous (not sequential).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Substitute(IntPtr ctx, IntPtr ast, uint numExprs, IntPtr[] from, IntPtr[] to)
    {
        var funcPtr = GetFunctionPointer("Z3_substitute");
        var func = Marshal.GetDelegateForFunctionPointer<SubstituteDelegate>(funcPtr);
        return func(ctx, ast, numExprs, from, to);
    }

    /// <summary>
    /// Substitutes bound variables in AST.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST expression containing bound variables.</param>
    /// <param name="numVars">Number of variables to substitute.</param>
    /// <param name="to">Array of expressions to substitute for variables.</param>
    /// <returns>New AST with variable substitutions applied.</returns>
    /// <remarks>
    /// Replaces de Bruijn indices (bound variables) in quantified formulas.
    /// Variable i is replaced by to[i]. Used for instantiating quantified formulas.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SubstituteVars(IntPtr ctx, IntPtr ast, uint numVars, IntPtr[] to)
    {
        var funcPtr = GetFunctionPointer("Z3_substitute_vars");
        var func = Marshal.GetDelegateForFunctionPointer<SubstituteVarsDelegate>(funcPtr);
        return func(ctx, ast, numVars, to);
    }

    /// <summary>
    /// Substitutes function declarations in AST.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST expression to perform substitution on.</param>
    /// <param name="numFuns">Number of function declarations to substitute.</param>
    /// <param name="from">Array of source function declarations to replace.</param>
    /// <param name="to">Array of target expressions to substitute in.</param>
    /// <returns>New AST with function substitutions applied.</returns>
    /// <remarks>
    /// Replaces all applications of functions in 'from' array with corresponding
    /// expressions in 'to' array. Used for function interpretation and macro expansion.
    /// Substitution is simultaneous (not sequential).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SubstituteFuns(IntPtr ctx, IntPtr ast, uint numFuns, IntPtr[] from, IntPtr[] to)
    {
        var funcPtr = GetFunctionPointer("Z3_substitute_funs");
        var func = Marshal.GetDelegateForFunctionPointer<SubstituteFunsDelegate>(funcPtr);
        return func(ctx, ast, numFuns, from, to);
    }

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
