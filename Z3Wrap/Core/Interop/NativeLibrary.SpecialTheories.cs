// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Special Relations and Theories API - P/Invoke bindings for Z3 special relations, sorts, and theories
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Special Relations and Theories API (9 functions):
// - Transitive closure of binary relations (1 function)
// - Special order relations: partial, linear, piecewise-linear, tree orders (4 functions)
// - Special sorts: finite domain and enumeration types (2 functions)
// - Fresh function/constant declarations with unique names (2 functions)
//
// Note: List sort (Z3_mk_list_sort) is in NativeLibrary.Sorts.cs
// Note: Recursive function declarations (Z3_mk_rec_func_decl, Z3_add_rec_def) not yet implemented
//
// See COMPARISON_SpecialTheories.md for complete API comparison and recommendations.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSpecialTheories(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Relation Theory
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_transitive_closure");

        // Order Relations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_partial_order");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_linear_order");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_tree_order");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_piecewise_linear_order");

        // Special Sorts

        // Miscellaneous
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fresh_func_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fresh_const");
    }

    // Delegates

    // Relation Theory
    private delegate IntPtr MkTransitiveClosureDelegate(IntPtr ctx, IntPtr relation);

    // Order Relations
    private delegate IntPtr MkPartialOrderDelegate(IntPtr ctx, IntPtr sort, uint id);
    private delegate IntPtr MkLinearOrderDelegate(IntPtr ctx, IntPtr sort, uint id);
    private delegate IntPtr MkTreeOrderDelegate(IntPtr ctx, IntPtr sort, uint id);
    private delegate IntPtr MkPiecewiseLinearOrderDelegate(IntPtr ctx, IntPtr sort, uint id);

    // Miscellaneous
    private delegate IntPtr MkFreshFuncDeclDelegate(
        IntPtr ctx,
        IntPtr prefix,
        uint numDomains,
        IntPtr[] domains,
        IntPtr range
    );
    private delegate IntPtr MkFreshConstDelegate(IntPtr ctx, IntPtr prefix, IntPtr sort);

    // Methods

    // Relation Theory
    /// <summary>
    /// Creates transitive closure of binary relation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="relation">Binary relation function declaration.</param>
    /// <returns>Function declaration representing transitive closure.</returns>
    /// <remarks>
    /// For binary relation R(x,y), transitive closure R+(x,y) means there exists
    /// a path from x to y: R(x,z1), R(z1,z2), ..., R(zn,y).
    /// Used in reachability analysis and graph algorithms.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTransitiveClosure(IntPtr ctx, IntPtr relation)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_transitive_closure");
        var func = Marshal.GetDelegateForFunctionPointer<MkTransitiveClosureDelegate>(funcPtr);
        return func(ctx, relation);
    }

    // Order Relations
    /// <summary>
    /// Creates partial ordering relation over signature.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Sort for the ordering relation.</param>
    /// <param name="id">Index identifier for the relation.</param>
    /// <returns>Function declaration representing partial order relation.</returns>
    /// <remarks>
    /// Creates binary relation representing partial order (reflexive, antisymmetric, transitive).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPartialOrder(IntPtr ctx, IntPtr sort, uint id)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_partial_order");
        var func = Marshal.GetDelegateForFunctionPointer<MkPartialOrderDelegate>(funcPtr);
        return func(ctx, sort, id);
    }

    /// <summary>
    /// Creates linear ordering relation over signature.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Sort for the ordering relation.</param>
    /// <param name="id">Index identifier for the relation.</param>
    /// <returns>Function declaration representing linear order relation.</returns>
    /// <remarks>
    /// Creates binary relation representing total/linear order (reflexive, antisymmetric, transitive, total).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLinearOrder(IntPtr ctx, IntPtr sort, uint id)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_linear_order");
        var func = Marshal.GetDelegateForFunctionPointer<MkLinearOrderDelegate>(funcPtr);
        return func(ctx, sort, id);
    }

    /// <summary>
    /// Creates tree ordering relation over signature.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Sort for the ordering relation.</param>
    /// <param name="id">Index identifier for the relation.</param>
    /// <returns>Function declaration representing tree order relation.</returns>
    /// <remarks>
    /// Creates binary relation for tree ordering (hierarchical structure with single parent).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTreeOrder(IntPtr ctx, IntPtr sort, uint id)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_tree_order");
        var func = Marshal.GetDelegateForFunctionPointer<MkTreeOrderDelegate>(funcPtr);
        return func(ctx, sort, id);
    }

    /// <summary>
    /// Creates piecewise linear ordering relation over signature.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Sort for the ordering relation.</param>
    /// <param name="id">Index identifier for the relation.</param>
    /// <returns>Function declaration representing piecewise linear order relation.</returns>
    /// <remarks>
    /// Creates binary relation for piecewise linear ordering.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPiecewiseLinearOrder(IntPtr ctx, IntPtr sort, uint id)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_piecewise_linear_order");
        var func = Marshal.GetDelegateForFunctionPointer<MkPiecewiseLinearOrderDelegate>(funcPtr);
        return func(ctx, sort, id);
    }

    // Special Sorts
    // Miscellaneous
    /// <summary>
    /// Creates fresh function declaration with unique name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="prefix">Name prefix for function (actual name will have unique suffix).</param>
    /// <param name="numDomains">Number of function arguments.</param>
    /// <param name="domains">Array of argument sorts.</param>
    /// <param name="range">Function return sort.</param>
    /// <returns>Function declaration with guaranteed unique name.</returns>
    /// <remarks>
    /// Creates uninterpreted function with unique name based on prefix.
    /// Useful when generating auxiliary functions that must not conflict with existing names.
    /// Z3 ensures name uniqueness by appending numeric suffix.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFreshFuncDecl(IntPtr ctx, IntPtr prefix, uint numDomains, IntPtr[] domains, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fresh_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<MkFreshFuncDeclDelegate>(funcPtr);
        return func(ctx, prefix, numDomains, domains, range);
    }

    /// <summary>
    /// Creates fresh constant with unique name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="prefix">Name prefix for constant (actual name will have unique suffix).</param>
    /// <param name="sort">Constant sort.</param>
    /// <returns>Expression representing constant with guaranteed unique name.</returns>
    /// <remarks>
    /// Creates constant (nullary uninterpreted function) with unique name based on prefix.
    /// Useful when generating auxiliary variables that must not conflict with existing names.
    /// Z3 ensures name uniqueness by appending numeric suffix.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFreshConst(IntPtr ctx, IntPtr prefix, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fresh_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkFreshConstDelegate>(funcPtr);
        return func(ctx, prefix, sort);
    }
}
