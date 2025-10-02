using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSubstitution(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_substitute");
        LoadFunctionOrNull(handle, functionPointers, "Z3_substitute_vars");
        LoadFunctionOrNull(handle, functionPointers, "Z3_substitute_funs");
    }

    // Delegates

    private delegate IntPtr SubstituteDelegate(IntPtr ctx, IntPtr ast, uint numExprs, IntPtr[] from, IntPtr[] to);
    private delegate IntPtr SubstituteVarsDelegate(IntPtr ctx, IntPtr ast, uint numVars, IntPtr[] to);
    private delegate IntPtr SubstituteFunsDelegate(IntPtr ctx, IntPtr ast, uint numFuns, IntPtr[] from, IntPtr[] to);

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
}
