// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Quantifiers API - P/Invoke bindings for Z3 quantifier and lambda expressions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's quantifier creation API (6 functions):
// - Universal quantifiers (forall) using constant-based syntax
// - Existential quantifiers (exists) using constant-based syntax
// - Lambda expressions (anonymous functions) using constant-based syntax
// - Lambda expressions (anonymous functions) using sorts/symbols syntax
// - Patterns for guiding quantifier instantiation
// - Bound variables for quantifier bodies
//
// Note: This file contains modern const-based quantifier/lambda constructors.
// Query functions for quantifiers are in NativeLibrary.Queries.cs.
// Predicate functions for quantifiers are in NativeLibrary.Predicates.cs.
// Old-style quantifier constructors (using sorts/symbols) are not currently bound.
//
// Missing Functions (6 functions):
// - Z3_mk_exists
// - Z3_mk_forall
// - Z3_mk_quantifier
// - Z3_mk_quantifier_const
// - Z3_mk_quantifier_const_ex
// - Z3_mk_quantifier_ex

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsQuantifiers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_forall_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_exists_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_lambda_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_lambda");
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
    private delegate IntPtr MkLambdaConstDelegate(IntPtr ctx, uint numBound, IntPtr[] bound, IntPtr body);
    private delegate IntPtr MkLambdaDelegate(
        IntPtr ctx,
        uint numDecls,
        IntPtr[] sorts,
        IntPtr[] declNames,
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
    /// Creates lambda expression using constant-based syntax.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numBound">The number of bound variables.</param>
    /// <param name="bound">Array of bound variable constants.</param>
    /// <param name="body">The expression body of the lambda.</param>
    /// <returns>Handle to the created lambda expression.</returns>
    /// <remarks>
    /// Creates λ bound_vars . body. Lambda expressions are anonymous functions
    /// that can be used in higher-order logic and array theory. The result has
    /// sort (Array domain_sorts range_sort) where range_sort is the sort of body.
    /// Modern const-based syntax similar to forall_const/exists_const.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLambdaConst(IntPtr ctx, uint numBound, IntPtr[] bound, IntPtr body)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lambda_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkLambdaConstDelegate>(funcPtr);
        return func(ctx, numBound, bound, body);
    }

    /// <summary>
    /// Creates lambda expression using sorts and symbol names.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numDecls">The number of bound variables.</param>
    /// <param name="sorts">Array of sorts for bound variables.</param>
    /// <param name="declNames">Array of symbol names for bound variables.</param>
    /// <param name="body">The expression body of the lambda.</param>
    /// <returns>Handle to the created lambda expression.</returns>
    /// <remarks>
    /// Creates λ (x1:sort1, ..., xn:sortn) . body. Lambda expressions are
    /// anonymous functions for higher-order logic. The result has sort
    /// (Array sorts range_sort) where range_sort is the sort of body.
    /// Old-style syntax using sorts and symbols instead of constants.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLambda(IntPtr ctx, uint numDecls, IntPtr[] sorts, IntPtr[] declNames, IntPtr body)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lambda");
        var func = Marshal.GetDelegateForFunctionPointer<MkLambdaDelegate>(funcPtr);
        return func(ctx, numDecls, sorts, declNames, body);
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
