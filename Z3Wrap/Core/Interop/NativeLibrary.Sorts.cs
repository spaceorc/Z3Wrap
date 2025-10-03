// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Sorts API - P/Invoke bindings for Z3 sort creation functions
//
// Source: z3_api.h (Sorts section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Sorts API (14 functions):
// - Basic sort creation (bool, int, real)
// - Constructor building (mk_constructor, query, field count, delete)
// - Datatype creation (single and mutually recursive)
// - Tuple sorts (special single-constructor datatypes)
// - List sorts (recursive cons-cell structures)
//
// Missing Functions (7 functions):
// - Z3_mk_array_sort
// - Z3_mk_array_sort_n
// - Z3_mk_bv_sort
// - Z3_mk_enumeration_sort
// - Z3_mk_finite_domain_sort
// - Z3_mk_type_variable
// - Z3_mk_uninterpreted_sort

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSorts(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_bool_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_int_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_real_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_constructor");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_constructor_list");
        LoadFunctionInternal(handle, functionPointers, "Z3_query_constructor");
        LoadFunctionInternal(handle, functionPointers, "Z3_constructor_num_fields");
        LoadFunctionInternal(handle, functionPointers, "Z3_del_constructor");
        LoadFunctionInternal(handle, functionPointers, "Z3_del_constructor_list");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_datatype");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_datatypes");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_datatype_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_tuple_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_list_sort");
    }

    // Delegates
    private delegate IntPtr MkBoolSortDelegate(IntPtr ctx);
    private delegate IntPtr MkIntSortDelegate(IntPtr ctx);
    private delegate IntPtr MkRealSortDelegate(IntPtr ctx);

    private delegate IntPtr MkConstructorDelegate(
        IntPtr ctx,
        IntPtr name,
        IntPtr recognizer,
        uint numFields,
        IntPtr[] fieldNames,
        IntPtr[] sorts,
        uint[] sortRefs
    );

    private delegate IntPtr MkConstructorListDelegate(IntPtr ctx, uint numConstructors, IntPtr[] constructors);

    private delegate void QueryConstructorDelegate(
        IntPtr ctx,
        IntPtr constructor,
        uint numFields,
        ref IntPtr constructorFunc,
        ref IntPtr testerFunc,
        IntPtr[] accessors
    );

    private delegate uint ConstructorNumFieldsDelegate(IntPtr ctx, IntPtr constructor);
    private delegate void DelConstructorDelegate(IntPtr ctx, IntPtr constructor);
    private delegate void DelConstructorListDelegate(IntPtr ctx, IntPtr constructorList);
    private delegate IntPtr MkDatatypeDelegate(IntPtr ctx, IntPtr name, uint numConstructors, IntPtr[] constructors);

    private delegate void MkDatatypesDelegate(
        IntPtr ctx,
        uint numSorts,
        IntPtr[] sortNames,
        IntPtr[] sorts,
        IntPtr[] constructorLists
    );

    private delegate IntPtr MkDatatypeSortDelegate(IntPtr ctx, IntPtr symbol);

    private delegate IntPtr MkTupleSortDelegate(
        IntPtr ctx,
        IntPtr mkTupleDecl,
        uint numFields,
        IntPtr[] fieldNames,
        IntPtr[] fieldSorts,
        ref IntPtr projDecls,
        IntPtr[] projFuncs
    );

    private delegate IntPtr MkListSortDelegate(
        IntPtr ctx,
        IntPtr name,
        IntPtr elemSort,
        ref IntPtr nilDecl,
        ref IntPtr isNilDecl,
        ref IntPtr consDecl,
        ref IntPtr isConsDecl,
        ref IntPtr headDecl,
        ref IntPtr tailDecl
    );

    // Methods
    /// <summary>
    /// Creates a Boolean sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Boolean sort.</returns>
    /// <remarks>
    /// Boolean sorts are used for creating Boolean expressions and constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBoolSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bool_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkBoolSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates an integer sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created integer sort.</returns>
    /// <remarks>
    /// Integer sorts are used for creating integer expressions and arithmetic constraints.
    /// Z3 integers have unlimited precision (BigInteger semantics).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIntSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkIntSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a real number sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created real sort.</returns>
    /// <remarks>
    /// Real sorts are used for creating real number expressions and arithmetic constraints.
    /// Z3 reals support exact rational arithmetic with unlimited precision.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRealSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkRealSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates datatype constructor with fields.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Constructor name symbol.</param>
    /// <param name="recognizer">Recognizer function name symbol.</param>
    /// <param name="numFields">Number of fields.</param>
    /// <param name="fieldNames">Array of field name symbols.</param>
    /// <param name="sorts">Array of field sorts (IntPtr.Zero for recursive references).</param>
    /// <param name="sortRefs">Array of sort reference indices (0 for non-recursive).</param>
    /// <returns>Constructor handle for use in datatype creation.</returns>
    /// <remarks>
    /// Constructors define how to build values of an algebraic datatype.
    /// Use IntPtr.Zero in sorts array and non-zero sortRefs for recursive fields.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConstructor(
        IntPtr ctx,
        IntPtr name,
        IntPtr recognizer,
        uint numFields,
        IntPtr[] fieldNames,
        IntPtr[] sorts,
        uint[] sortRefs
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_constructor");
        var func = Marshal.GetDelegateForFunctionPointer<MkConstructorDelegate>(funcPtr);
        return func(ctx, name, recognizer, numFields, fieldNames, sorts, sortRefs);
    }

    /// <summary>
    /// Creates constructor list for mutually recursive datatypes.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numConstructors">Number of constructors.</param>
    /// <param name="constructors">Array of constructor handles.</param>
    /// <returns>Constructor list handle.</returns>
    /// <remarks>
    /// Constructor lists are used when creating mutually recursive datatypes.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConstructorList(IntPtr ctx, uint numConstructors, IntPtr[] constructors)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_constructor_list");
        var func = Marshal.GetDelegateForFunctionPointer<MkConstructorListDelegate>(funcPtr);
        return func(ctx, numConstructors, constructors);
    }

    /// <summary>
    /// Queries constructor properties after datatype creation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="constructor">Constructor handle.</param>
    /// <param name="numFields">Number of fields in constructor.</param>
    /// <param name="constructorFunc">Output: constructor function declaration.</param>
    /// <param name="testerFunc">Output: recognizer/tester function declaration.</param>
    /// <param name="accessors">Output: array of accessor function declarations.</param>
    /// <remarks>
    /// After creating datatype, query constructor to get function declarations
    /// for building values, testing constructor membership, and accessing fields.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void QueryConstructor(
        IntPtr ctx,
        IntPtr constructor,
        uint numFields,
        ref IntPtr constructorFunc,
        ref IntPtr testerFunc,
        IntPtr[] accessors
    )
    {
        var funcPtr = GetFunctionPointer("Z3_query_constructor");
        var func = Marshal.GetDelegateForFunctionPointer<QueryConstructorDelegate>(funcPtr);
        func(ctx, constructor, numFields, ref constructorFunc, ref testerFunc, accessors);
    }

    /// <summary>
    /// Gets number of fields in constructor.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="constructor">Constructor handle.</param>
    /// <returns>Number of fields in constructor.</returns>
    /// <remarks>
    /// Query how many fields exist in a datatype constructor.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ConstructorNumFields(IntPtr ctx, IntPtr constructor)
    {
        var funcPtr = GetFunctionPointer("Z3_constructor_num_fields");
        var func = Marshal.GetDelegateForFunctionPointer<ConstructorNumFieldsDelegate>(funcPtr);
        return func(ctx, constructor);
    }

    /// <summary>
    /// Deletes constructor handle.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="constructor">Constructor handle to delete.</param>
    /// <remarks>
    /// Call after creating datatype to clean up constructor resources.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DelConstructor(IntPtr ctx, IntPtr constructor)
    {
        var funcPtr = GetFunctionPointer("Z3_del_constructor");
        var func = Marshal.GetDelegateForFunctionPointer<DelConstructorDelegate>(funcPtr);
        func(ctx, constructor);
    }

    /// <summary>
    /// Deletes constructor list handle.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="constructorList">Constructor list handle to delete.</param>
    /// <remarks>
    /// Call after creating mutually recursive datatypes to clean up resources.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DelConstructorList(IntPtr ctx, IntPtr constructorList)
    {
        var funcPtr = GetFunctionPointer("Z3_del_constructor_list");
        var func = Marshal.GetDelegateForFunctionPointer<DelConstructorListDelegate>(funcPtr);
        func(ctx, constructorList);
    }

    /// <summary>
    /// Creates single datatype sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Datatype name symbol.</param>
    /// <param name="numConstructors">Number of constructors.</param>
    /// <param name="constructors">Array of constructor handles.</param>
    /// <returns>Sort handle representing the datatype.</returns>
    /// <remarks>
    /// Creates algebraic datatype with specified constructors.
    /// Constructors must be created first using MkConstructor.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDatatype(IntPtr ctx, IntPtr name, uint numConstructors, IntPtr[] constructors)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_datatype");
        var func = Marshal.GetDelegateForFunctionPointer<MkDatatypeDelegate>(funcPtr);
        return func(ctx, name, numConstructors, constructors);
    }

    /// <summary>
    /// Creates mutually recursive datatypes.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numSorts">Number of datatypes to create.</param>
    /// <param name="sortNames">Array of datatype name symbols.</param>
    /// <param name="sorts">Output: array to receive created sort handles.</param>
    /// <param name="constructorLists">Array of constructor list handles.</param>
    /// <remarks>
    /// Creates multiple datatypes that can reference each other recursively.
    /// Constructor lists must be created first using MkConstructorList.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void MkDatatypes(IntPtr ctx, uint numSorts, IntPtr[] sortNames, IntPtr[] sorts, IntPtr[] constructorLists)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_datatypes");
        var func = Marshal.GetDelegateForFunctionPointer<MkDatatypesDelegate>(funcPtr);
        func(ctx, numSorts, sortNames, sorts, constructorLists);
    }

    /// <summary>
    /// Creates datatype sort from symbol.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">Datatype name symbol.</param>
    /// <returns>Sort handle representing the datatype.</returns>
    /// <remarks>
    /// Alternative datatype creation method using symbol.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDatatypeSort(IntPtr ctx, IntPtr symbol)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_datatype_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkDatatypeSortDelegate>(funcPtr);
        return func(ctx, symbol);
    }

    /// <summary>
    /// Creates tuple datatype sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="mkTupleDecl">Tuple name symbol.</param>
    /// <param name="numFields">Number of tuple fields.</param>
    /// <param name="fieldNames">Array of field name symbols.</param>
    /// <param name="fieldSorts">Array of field sorts.</param>
    /// <param name="projDecls">Output: tuple constructor declaration.</param>
    /// <param name="projFuncs">Output: array of field accessor declarations.</param>
    /// <returns>Sort handle representing the tuple datatype.</returns>
    /// <remarks>
    /// Tuples are special datatypes with single constructor and named fields.
    /// Returns constructor and accessor function declarations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTupleSort(
        IntPtr ctx,
        IntPtr mkTupleDecl,
        uint numFields,
        IntPtr[] fieldNames,
        IntPtr[] fieldSorts,
        ref IntPtr projDecls,
        IntPtr[] projFuncs
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_tuple_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkTupleSortDelegate>(funcPtr);
        return func(ctx, mkTupleDecl, numFields, fieldNames, fieldSorts, ref projDecls, projFuncs);
    }

    /// <summary>
    /// Creates list datatype sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">List type name symbol.</param>
    /// <param name="elemSort">Element sort for list.</param>
    /// <param name="nilDecl">Output: nil constructor declaration.</param>
    /// <param name="isNilDecl">Output: is_nil recognizer declaration.</param>
    /// <param name="consDecl">Output: cons constructor declaration.</param>
    /// <param name="isConsDecl">Output: is_cons recognizer declaration.</param>
    /// <param name="headDecl">Output: head accessor declaration.</param>
    /// <param name="tailDecl">Output: tail accessor declaration.</param>
    /// <returns>Sort handle representing list datatype.</returns>
    /// <remarks>
    /// Creates standard recursive list datatype with nil and cons constructors.
    /// Returns all necessary function declarations for list operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkListSort(
        IntPtr ctx,
        IntPtr name,
        IntPtr elemSort,
        ref IntPtr nilDecl,
        ref IntPtr isNilDecl,
        ref IntPtr consDecl,
        ref IntPtr isConsDecl,
        ref IntPtr headDecl,
        ref IntPtr tailDecl
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_list_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkListSortDelegate>(funcPtr);
        return func(
            ctx,
            name,
            elemSort,
            ref nilDecl,
            ref isNilDecl,
            ref consDecl,
            ref isConsDecl,
            ref headDecl,
            ref tailDecl
        );
    }

}
