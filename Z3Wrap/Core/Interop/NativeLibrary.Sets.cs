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
    private static void LoadFunctionsSets(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_empty_set");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_full_set");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_add");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_del");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_union");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_intersect");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_difference");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_complement");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_member");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_subset");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_set_has_size");
    }

    // Delegates
    private delegate IntPtr MkSetSortDelegate(IntPtr ctx, IntPtr ty);
    private delegate IntPtr MkEmptySetDelegate(IntPtr ctx, IntPtr domain);
    private delegate IntPtr MkFullSetDelegate(IntPtr ctx, IntPtr domain);
    private delegate IntPtr MkSetAddDelegate(IntPtr ctx, IntPtr set, IntPtr elem);
    private delegate IntPtr MkSetDelDelegate(IntPtr ctx, IntPtr set, IntPtr elem);
    private delegate IntPtr MkSetUnionDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkSetIntersectDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkSetDifferenceDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkSetComplementDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkSetMemberDelegate(IntPtr ctx, IntPtr elem, IntPtr set);
    private delegate IntPtr MkSetSubsetDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkSetHasSizeDelegate(IntPtr ctx, IntPtr set, IntPtr k);

    // Methods
    /// <summary>
    /// Creates set sort with specified element type.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ty">The element sort.</param>
    /// <returns>Sort handle representing set of elements of type ty.</returns>
    /// <remarks>
    /// Sets are represented as arrays from element type to Boolean.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetSort(IntPtr ctx, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetSortDelegate>(funcPtr);
        return func(ctx, ty);
    }

    /// <summary>
    /// Creates empty set constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The element sort for the set.</param>
    /// <returns>AST node representing empty set.</returns>
    /// <remarks>
    /// Creates set containing no elements of the specified domain sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkEmptySet(IntPtr ctx, IntPtr domain)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_empty_set");
        var func = Marshal.GetDelegateForFunctionPointer<MkEmptySetDelegate>(funcPtr);
        return func(ctx, domain);
    }

    /// <summary>
    /// Creates full set constant (universe).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The element sort for the set.</param>
    /// <returns>AST node representing full set containing all elements.</returns>
    /// <remarks>
    /// Creates set containing all possible elements of the specified domain sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFullSet(IntPtr ctx, IntPtr domain)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_full_set");
        var func = Marshal.GetDelegateForFunctionPointer<MkFullSetDelegate>(funcPtr);
        return func(ctx, domain);
    }

    /// <summary>
    /// Creates set with element added.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="set">The set expression.</param>
    /// <param name="elem">The element to add.</param>
    /// <returns>AST node representing set with elem added.</returns>
    /// <remarks>
    /// Creates new set containing all elements from set plus elem.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetAdd(IntPtr ctx, IntPtr set, IntPtr elem)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_add");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetAddDelegate>(funcPtr);
        return func(ctx, set, elem);
    }

    /// <summary>
    /// Creates set with element removed.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="set">The set expression.</param>
    /// <param name="elem">The element to remove.</param>
    /// <returns>AST node representing set with elem removed.</returns>
    /// <remarks>
    /// Creates new set containing all elements from set except elem.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetDel(IntPtr ctx, IntPtr set, IntPtr elem)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_del");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetDelDelegate>(funcPtr);
        return func(ctx, set, elem);
    }

    /// <summary>
    /// Creates union of sets.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of set expressions.</param>
    /// <param name="args">Array of set expressions.</param>
    /// <returns>AST node representing union of all sets.</returns>
    /// <remarks>
    /// Creates set containing elements present in any of the argument sets.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetUnion(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_union");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetUnionDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates intersection of sets.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of set expressions.</param>
    /// <param name="args">Array of set expressions.</param>
    /// <returns>AST node representing intersection of all sets.</returns>
    /// <remarks>
    /// Creates set containing only elements present in all argument sets.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetIntersect(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_intersect");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetIntersectDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates set difference.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First set expression.</param>
    /// <param name="t2">Second set expression.</param>
    /// <returns>AST node representing elements in t1 but not in t2.</returns>
    /// <remarks>
    /// Creates set containing elements from t1 that are not in t2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetDifference(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_difference");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetDifferenceDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates set complement.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">Set expression.</param>
    /// <returns>AST node representing complement of t1.</returns>
    /// <remarks>
    /// Creates set containing all elements not in t1.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetComplement(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_complement");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetComplementDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates set membership test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="elem">Element expression.</param>
    /// <param name="set">Set expression.</param>
    /// <returns>AST node representing Boolean predicate testing if elem is in set.</returns>
    /// <remarks>
    /// Returns true if elem is a member of set, false otherwise.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetMember(IntPtr ctx, IntPtr elem, IntPtr set)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_member");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetMemberDelegate>(funcPtr);
        return func(ctx, elem, set);
    }

    /// <summary>
    /// Creates set subset test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First set expression.</param>
    /// <param name="t2">Second set expression.</param>
    /// <returns>AST node representing Boolean predicate testing if t1 is subset of t2.</returns>
    /// <remarks>
    /// Returns true if all elements of t1 are also in t2, false otherwise.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetSubset(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_subset");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetSubsetDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates set cardinality constraint.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="set">Set expression.</param>
    /// <param name="k">Size expression (integer).</param>
    /// <returns>AST node representing Boolean predicate testing if set has k elements.</returns>
    /// <remarks>
    /// Returns true if the cardinality of set equals k, false otherwise.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSetHasSize(IntPtr ctx, IntPtr set, IntPtr k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_set_has_size");
        var func = Marshal.GetDelegateForFunctionPointer<MkSetHasSizeDelegate>(funcPtr);
        return func(ctx, set, k);
    }
}
