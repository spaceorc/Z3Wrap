// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Arrays API - P/Invoke bindings for Z3 array theory
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's array theory API (9 of 14 functions - 64.3% complete):
// - Array sort creation (single-dimensional)
// - Array operations (select, store, const arrays)
// - Array property queries (domain, range, default value)
// - Array extensionality and function conversion
//
// Missing: Multi-dimensional array support (mk_array_sort_n, mk_select_n, mk_store_n,
//          get_array_sort_domain_n) and array mapping (mk_map)
//
// Missing Functions (4 functions):
// - Z3_mk_map
// - Z3_mk_select_n
// - Z3_mk_set_has_size
// - Z3_mk_store_n

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsArrays(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_array_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_select");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_store");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_const_array");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_domain");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_range");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_array_default");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_array_ext");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_as_array");
    }

    // Delegates
    private delegate IntPtr MkArraySortDelegate(IntPtr ctx, IntPtr domain, IntPtr range);
    private delegate IntPtr MkSelectDelegate(IntPtr ctx, IntPtr array, IntPtr index);
    private delegate IntPtr MkStoreDelegate(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value);
    private delegate IntPtr MkConstArrayDelegate(IntPtr ctx, IntPtr domain, IntPtr value);
    private delegate IntPtr GetArraySortDomainDelegate(IntPtr ctx, IntPtr arraySort);
    private delegate IntPtr GetArraySortRangeDelegate(IntPtr ctx, IntPtr arraySort);
    private delegate IntPtr MkArrayDefaultDelegate(IntPtr ctx, IntPtr array);
    private delegate IntPtr MkArrayExtDelegate(IntPtr ctx, IntPtr arg1, IntPtr arg2);
    private delegate IntPtr MkAsArrayDelegate(IntPtr ctx, IntPtr f);

    // Methods
    /// <summary>
    /// Creates a Z3 array sort with specified domain and range sorts.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The sort for array indices (keys).</param>
    /// <param name="range">The sort for array values.</param>
    /// <returns>Handle to the created array sort.</returns>
    /// <remarks>
    /// Array sorts define mappings from domain elements to range elements.
    /// Used for creating array expressions and constants.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkArraySort(IntPtr ctx, IntPtr domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkArraySortDelegate>(funcPtr);
        return func(ctx, domain, range);
    }

    /// <summary>
    /// Creates a Z3 array select expression that reads a value at a given index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The array expression to read from.</param>
    /// <param name="index">The index expression specifying the position to read.</param>
    /// <returns>Handle to the created select expression (array[index]).</returns>
    /// <remarks>
    /// The index must match the array's domain sort, and the result has the array's range sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_select");
        var func = Marshal.GetDelegateForFunctionPointer<MkSelectDelegate>(funcPtr);
        return func(ctx, array, index);
    }

    /// <summary>
    /// Creates a Z3 array store expression that writes a value at a given index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The original array expression.</param>
    /// <param name="index">The index expression specifying where to write.</param>
    /// <param name="value">The value expression to store at the index.</param>
    /// <returns>Handle to the created array expression with updated value (array[index := value]).</returns>
    /// <remarks>
    /// Creates a new array identical to the original except at the specified index.
    /// The index must match the domain sort and value must match the range sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_store");
        var func = Marshal.GetDelegateForFunctionPointer<MkStoreDelegate>(funcPtr);
        return func(ctx, array, index, value);
    }

    /// <summary>
    /// Creates a Z3 constant array expression where all elements have the same value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The sort for array indices (keys).</param>
    /// <param name="value">The constant value for all array elements.</param>
    /// <returns>Handle to the created constant array expression.</returns>
    /// <remarks>
    /// Creates an array where every possible index maps to the same value.
    /// Useful for initializing arrays with default values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConstArray(IntPtr ctx, IntPtr domain, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const_array");
        var func = Marshal.GetDelegateForFunctionPointer<MkConstArrayDelegate>(funcPtr);
        return func(ctx, domain, value);
    }

    /// <summary>
    /// Retrieves the domain sort (index type) of an array sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arraySort">The array sort to query.</param>
    /// <returns>Handle to the domain sort of the array.</returns>
    /// <remarks>
    /// Returns the sort used for array indices. Used for type checking and sort queries.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetArraySortDomain(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_domain");
        var func = Marshal.GetDelegateForFunctionPointer<GetArraySortDomainDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    /// <summary>
    /// Retrieves the range sort (value type) of an array sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arraySort">The array sort to query.</param>
    /// <returns>Handle to the range sort of the array.</returns>
    /// <remarks>
    /// Returns the sort used for array values. Used for type checking and sort queries.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetArraySortRange(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_range");
        var func = Marshal.GetDelegateForFunctionPointer<GetArraySortRangeDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    /// <summary>
    /// Creates array default value accessor.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The array expression.</param>
    /// <returns>AST node representing default value of the array.</returns>
    /// <remarks>
    /// Accesses the else value of a constant array created with MkConstArray.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkArrayDefault(IntPtr ctx, IntPtr array)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_default");
        var func = Marshal.GetDelegateForFunctionPointer<MkArrayDefaultDelegate>(funcPtr);
        return func(ctx, array);
    }

    /// <summary>
    /// Creates array extensionality constraint.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg1">First array expression.</param>
    /// <param name="arg2">Second array expression.</param>
    /// <returns>AST node representing an index where arrays differ (if they differ).</returns>
    /// <remarks>
    /// Array extensionality: two arrays are equal iff they agree on all indices.
    /// This function returns an index witnessing inequality if arrays are not equal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkArrayExt(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_ext");
        var func = Marshal.GetDelegateForFunctionPointer<MkArrayExtDelegate>(funcPtr);
        return func(ctx, arg1, arg2);
    }

    /// <summary>
    /// Creates array from function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Function declaration handle.</param>
    /// <returns>AST node representing array corresponding to function.</returns>
    /// <remarks>
    /// Creates an array expression that behaves like the given function.
    /// For any index i, select(as-array(f), i) = f(i).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAsArray(IntPtr ctx, IntPtr f)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_as_array");
        var func = Marshal.GetDelegateForFunctionPointer<MkAsArrayDelegate>(funcPtr);
        return func(ctx, f);
    }
}
