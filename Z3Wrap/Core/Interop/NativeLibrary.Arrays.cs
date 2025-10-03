// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Arrays API - P/Invoke bindings for Z3 array theory
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's array theory API (11 functions):
// - Array sort creation (single and multi-dimensional)
// - Array operations (select, store, const arrays, map, n-dimensional variants)
// - Array property queries (domain, range, default value, n-th domain)
// - Array function conversion
//
// Note: Z3_mk_set_has_size is in NativeLibrary.Sets.cs (per c_headers categorization)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsArrays(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_select");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_store");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_const_array");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_map");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_domain");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_range");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_array_default");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_as_array");

        // Multi-dimensional array functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_select_n");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_store_n");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_domain_n");
    }

    // Delegates
    private delegate IntPtr MkSelectDelegate(IntPtr ctx, IntPtr array, IntPtr index);
    private delegate IntPtr MkStoreDelegate(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value);
    private delegate IntPtr MkConstArrayDelegate(IntPtr ctx, IntPtr domain, IntPtr value);
    private delegate IntPtr MkMapDelegate(IntPtr ctx, IntPtr f, uint n, IntPtr[] args);
    private delegate IntPtr GetArraySortDomainDelegate(IntPtr ctx, IntPtr arraySort);
    private delegate IntPtr GetArraySortRangeDelegate(IntPtr ctx, IntPtr arraySort);
    private delegate IntPtr MkArrayDefaultDelegate(IntPtr ctx, IntPtr array);
    private delegate IntPtr MkAsArrayDelegate(IntPtr ctx, IntPtr f);

    // Delegates - Multi-dimensional arrays
    private delegate IntPtr MkSelectNDelegate(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices);
    private delegate IntPtr MkStoreNDelegate(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices, IntPtr value);
    private delegate IntPtr GetArraySortDomainNDelegate(IntPtr ctx, IntPtr arraySort, uint n);

    // Methods
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
    /// Creates array map expression applying function to arrays element-wise.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Function declaration to apply.</param>
    /// <param name="n">Number of array arguments.</param>
    /// <param name="args">Array of array expressions to map over.</param>
    /// <returns>AST node representing mapped array.</returns>
    /// <remarks>
    /// Creates array where each element is the result of applying f to corresponding elements of input arrays.
    /// For single array: map(f, a)[i] = f(a[i]). For multiple arrays: map(f, a1, a2)[i] = f(a1[i], a2[i]).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkMap(IntPtr ctx, IntPtr f, uint n, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_map");
        var func = Marshal.GetDelegateForFunctionPointer<MkMapDelegate>(funcPtr);
        return func(ctx, f, n, args);
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

    // Methods - Multi-dimensional arrays

    /// <summary>
    /// Creates n-dimensional array select expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The array expression to read from.</param>
    /// <param name="n">Number of indices.</param>
    /// <param name="indices">Array of index expressions.</param>
    /// <returns>Handle to select expression reading from n-dimensional array.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSelectN(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_select_n");
        var func = Marshal.GetDelegateForFunctionPointer<MkSelectNDelegate>(funcPtr);
        return func(ctx, array, n, indices);
    }

    /// <summary>
    /// Creates n-dimensional array store expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The original array expression.</param>
    /// <param name="n">Number of indices.</param>
    /// <param name="indices">Array of index expressions.</param>
    /// <param name="value">The value to store at specified indices.</param>
    /// <returns>Handle to array expression with updated value at n-dimensional position.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStoreN(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_store_n");
        var func = Marshal.GetDelegateForFunctionPointer<MkStoreNDelegate>(funcPtr);
        return func(ctx, array, n, indices, value);
    }

    /// <summary>
    /// Retrieves n-th domain sort of multi-dimensional array sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arraySort">The array sort to query.</param>
    /// <param name="n">Index of domain sort to retrieve.</param>
    /// <returns>Handle to n-th domain sort of the array.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetArraySortDomainN(IntPtr ctx, IntPtr arraySort, uint n)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_domain_n");
        var func = Marshal.GetDelegateForFunctionPointer<GetArraySortDomainNDelegate>(funcPtr);
        return func(ctx, arraySort, n);
    }
}
