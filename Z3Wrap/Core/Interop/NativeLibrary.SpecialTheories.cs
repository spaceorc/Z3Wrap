// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Special Relations and Theories API - P/Invoke bindings for Z3 special relations, sorts, and theories
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Special Relations and Theories API (5 of 12 functions - 41.7% complete):
// - Transitive closure of binary relations (1 function)
// - Special sorts: finite domain and enumeration types (2 functions)
// - Fresh function/constant declarations with unique names (2 functions)
//
// Missing functions (7):
// - Special order relations: partial, linear, piecewise-linear, tree orders (4 functions)
// - List sort with constructors and accessors (1 function)
// - Recursive function declarations and definitions (2 functions)
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

        // Special Sorts
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_finite_domain_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_enumeration_sort");

        // Miscellaneous
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fresh_func_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fresh_const");
    }

    // Delegates

    // Relation Theory
    private delegate IntPtr MkTransitiveClosureDelegate(IntPtr ctx, IntPtr relation);

    // Special Sorts
    private delegate IntPtr MkFiniteDomainSortDelegate(IntPtr ctx, IntPtr name, ulong size);
    private delegate IntPtr MkEnumerationSortDelegate(
        IntPtr ctx,
        IntPtr name,
        uint numEnumNames,
        IntPtr[] enumNames,
        IntPtr[] enumConsts,
        IntPtr[] enumTesters
    );

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

    // Special Sorts
    /// <summary>
    /// Creates finite domain sort with specified size.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Sort name symbol.</param>
    /// <param name="size">Number of distinct elements in domain.</param>
    /// <returns>Sort handle representing finite domain.</returns>
    /// <remarks>
    /// Finite domain sorts have exactly 'size' distinct elements.
    /// More efficient than unbounded sorts for constraint solving.
    /// Useful for modeling bounded counters, state machines, etc.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFiniteDomainSort(IntPtr ctx, IntPtr name, ulong size)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_finite_domain_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkFiniteDomainSortDelegate>(funcPtr);
        return func(ctx, name, size);
    }

    /// <summary>
    /// Creates enumeration sort like C enum type.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Enumeration sort name symbol.</param>
    /// <param name="numEnumNames">Number of enumeration constants.</param>
    /// <param name="enumNames">Array of enumeration constant name symbols.</param>
    /// <param name="enumConsts">Output: array of enumeration constant expressions.</param>
    /// <param name="enumTesters">Output: array of tester predicates (is_EnumName functions).</param>
    /// <returns>Sort handle representing enumeration type.</returns>
    /// <remarks>
    /// Creates algebraic datatype with nullary constructors for each enumeration value.
    /// Similar to C/C++ enum or Haskell sum type with only constant constructors.
    /// Returns constants and tester predicates for each enumeration value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkEnumerationSort(
        IntPtr ctx,
        IntPtr name,
        uint numEnumNames,
        IntPtr[] enumNames,
        IntPtr[] enumConsts,
        IntPtr[] enumTesters
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_enumeration_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkEnumerationSortDelegate>(funcPtr);
        return func(ctx, name, numEnumNames, enumNames, enumConsts, enumTesters);
    }

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
