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
    private static void LoadFunctionsQuantifiers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_forall_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_exists_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_pattern");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bound");
    }

    // Delegates
    private delegate IntPtr MkForallConstDelegate(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    );
    private delegate IntPtr MkExistsConstDelegate(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    );
    private delegate IntPtr MkPatternDelegate(IntPtr ctx, uint numPatterns, IntPtr[] terms);
    private delegate IntPtr MkBoundDelegate(IntPtr ctx, uint index, IntPtr ty);

    // Methods
    /// <summary>
    /// Creates a universal quantifier (for-all) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="weight">Quantifier weight for instantiation heuristics (0 for default).</param>
    /// <param name="numBound">The number of bound variables.</param>
    /// <param name="bound">Array of bound variable constants.</param>
    /// <param name="numPatterns">The number of patterns for instantiation.</param>
    /// <param name="patterns">Array of pattern expressions (can be null).</param>
    /// <param name="body">The Boolean formula body of the quantifier.</param>
    /// <returns>Handle to the created universal quantifier expression.</returns>
    /// <remarks>
    /// Creates ∀ bound_vars : body. Patterns help guide quantifier instantiation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_forall_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkForallConstDelegate>(funcPtr);
        return func(ctx, weight, numBound, bound, numPatterns, patterns, body);
    }

    /// <summary>
    /// Creates an existential quantifier (there-exists) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="weight">Quantifier weight for instantiation heuristics (0 for default).</param>
    /// <param name="numBound">The number of bound variables.</param>
    /// <param name="bound">Array of bound variable constants.</param>
    /// <param name="numPatterns">The number of patterns for instantiation.</param>
    /// <param name="patterns">Array of pattern expressions (can be null).</param>
    /// <param name="body">The Boolean formula body of the quantifier.</param>
    /// <returns>Handle to the created existential quantifier expression.</returns>
    /// <remarks>
    /// Creates ∃ bound_vars : body. Patterns help guide quantifier instantiation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_exists_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkExistsConstDelegate>(funcPtr);
        return func(ctx, weight, numBound, bound, numPatterns, patterns, body);
    }

    /// <summary>
    /// Creates a pattern for quantifier instantiation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numPatterns">The number of terms in the pattern.</param>
    /// <param name="terms">Array of expressions forming the pattern.</param>
    /// <returns>Handle to the created pattern.</returns>
    /// <remarks>
    /// Patterns guide Z3's quantifier instantiation by specifying which terms
    /// should trigger instantiation of the quantified formula.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pattern");
        var func = Marshal.GetDelegateForFunctionPointer<MkPatternDelegate>(funcPtr);
        return func(ctx, numPatterns, terms);
    }

    /// <summary>
    /// Creates bound variable for use in quantifiers.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="index">De Bruijn index of the bound variable.</param>
    /// <param name="ty">Sort of the bound variable.</param>
    /// <returns>AST node representing bound variable.</returns>
    /// <remarks>
    /// Bound variables are indexed by de Bruijn indices used in quantifiers.
    /// Index 0 refers to the innermost bound variable.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBound(IntPtr ctx, uint index, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bound");
        var func = Marshal.GetDelegateForFunctionPointer<MkBoundDelegate>(funcPtr);
        return func(ctx, index, ty);
    }
}
