// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Datatypes API - P/Invoke bindings for Z3 algebraic datatype definitions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Datatypes API (20 functions):
// - Constructor building (mk_constructor, query, field count, delete)
// - Datatype creation (single and mutually recursive)
// - Datatype queries (constructor count, accessors, recognizers)
// - Tuple sorts (special single-constructor datatypes)
// - List sorts (recursive cons-cell structures)
// - Relation sorts (multi-column relational types)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsDatatypes(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Constructor Building
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_constructor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_constructor_list");
        LoadFunctionOrNull(handle, functionPointers, "Z3_query_constructor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_constructor_num_fields");
        LoadFunctionOrNull(handle, functionPointers, "Z3_del_constructor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_del_constructor_list");

        // Datatype Creation
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_datatype");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_datatypes");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_datatype_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_tuple_sort");

        // Datatype Queries
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_num_constructors");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_constructor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_recognizer");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_constructor_accessor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_mk_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_num_fields");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_field_decl");

        // Related Functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_list_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_relation_arity");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_relation_column");
    }

    // Delegates

    // Constructor Building
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

    // Datatype Creation
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

    // Datatype Queries
    private delegate uint GetDatatypeSortNumConstructorsDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetDatatypeSortConstructorDelegate(IntPtr ctx, IntPtr sort, uint idx);
    private delegate IntPtr GetDatatypeSortRecognizerDelegate(IntPtr ctx, IntPtr sort, uint idx);
    private delegate IntPtr GetDatatypeSortConstructorAccessorDelegate(IntPtr ctx, IntPtr sort, uint idxC, uint idxA);
    private delegate IntPtr GetTupleSortMkDeclDelegate(IntPtr ctx, IntPtr sort);
    private delegate uint GetTupleSortNumFieldsDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetTupleSortFieldDeclDelegate(IntPtr ctx, IntPtr sort, uint i);

    // Related Functions
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

    private delegate uint GetRelationArityDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetRelationColumnDelegate(IntPtr ctx, IntPtr sort, uint col);

    // Methods

    // Constructor Building
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

    // Datatype Creation
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

    // Datatype Queries
    /// <summary>
    /// Gets number of constructors in datatype sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Datatype sort handle.</param>
    /// <returns>Number of constructors.</returns>
    /// <remarks>
    /// Query how many constructors exist in an algebraic datatype.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetDatatypeSortNumConstructors(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_datatype_sort_num_constructors");
        var func = Marshal.GetDelegateForFunctionPointer<GetDatatypeSortNumConstructorsDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets constructor function declaration at index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Datatype sort handle.</param>
    /// <param name="idx">Constructor index (zero-based).</param>
    /// <returns>Function declaration for constructor at index.</returns>
    /// <remarks>
    /// Retrieve constructor function to build datatype values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDatatypeSortConstructor(IntPtr ctx, IntPtr sort, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_datatype_sort_constructor");
        var func = Marshal.GetDelegateForFunctionPointer<GetDatatypeSortConstructorDelegate>(funcPtr);
        return func(ctx, sort, idx);
    }

    /// <summary>
    /// Gets recognizer function declaration for constructor.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Datatype sort handle.</param>
    /// <param name="idx">Constructor index (zero-based).</param>
    /// <returns>Function declaration for recognizer (is_Constructor predicate).</returns>
    /// <remarks>
    /// Recognizers test if value was constructed with specific constructor.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDatatypeSortRecognizer(IntPtr ctx, IntPtr sort, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_datatype_sort_recognizer");
        var func = Marshal.GetDelegateForFunctionPointer<GetDatatypeSortRecognizerDelegate>(funcPtr);
        return func(ctx, sort, idx);
    }

    /// <summary>
    /// Gets field accessor function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Datatype sort handle.</param>
    /// <param name="idxC">Constructor index (zero-based).</param>
    /// <param name="idxA">Accessor/field index within constructor (zero-based).</param>
    /// <returns>Function declaration for field accessor.</returns>
    /// <remarks>
    /// Accessors extract field values from datatype values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDatatypeSortConstructorAccessor(IntPtr ctx, IntPtr sort, uint idxC, uint idxA)
    {
        var funcPtr = GetFunctionPointer("Z3_get_datatype_sort_constructor_accessor");
        var func = Marshal.GetDelegateForFunctionPointer<GetDatatypeSortConstructorAccessorDelegate>(funcPtr);
        return func(ctx, sort, idxC, idxA);
    }

    /// <summary>
    /// Gets tuple constructor function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Tuple sort handle.</param>
    /// <returns>Function declaration for tuple constructor.</returns>
    /// <remarks>
    /// Returns function to create tuple values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetTupleSortMkDecl(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_tuple_sort_mk_decl");
        var func = Marshal.GetDelegateForFunctionPointer<GetTupleSortMkDeclDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets number of fields in tuple sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Tuple sort handle.</param>
    /// <returns>Number of fields in tuple.</returns>
    /// <remarks>
    /// Query tuple field count.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetTupleSortNumFields(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_tuple_sort_num_fields");
        var func = Marshal.GetDelegateForFunctionPointer<GetTupleSortNumFieldsDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets tuple field accessor function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Tuple sort handle.</param>
    /// <param name="i">Field index (zero-based).</param>
    /// <returns>Function declaration for field accessor.</returns>
    /// <remarks>
    /// Returns function to extract tuple field values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetTupleSortFieldDecl(IntPtr ctx, IntPtr sort, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_tuple_sort_field_decl");
        var func = Marshal.GetDelegateForFunctionPointer<GetTupleSortFieldDeclDelegate>(funcPtr);
        return func(ctx, sort, i);
    }

    // Related Functions
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

    /// <summary>
    /// Gets arity of relation sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Relation sort handle.</param>
    /// <returns>Number of columns in relation.</returns>
    /// <remarks>
    /// Query relation column count.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetRelationArity(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_relation_arity");
        var func = Marshal.GetDelegateForFunctionPointer<GetRelationArityDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets sort of relation column.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">Relation sort handle.</param>
    /// <param name="col">Column index (zero-based).</param>
    /// <returns>Sort handle for column at index.</returns>
    /// <remarks>
    /// Query type of relation column.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetRelationColumn(IntPtr ctx, IntPtr sort, uint col)
    {
        var funcPtr = GetFunctionPointer("Z3_get_relation_column");
        var func = Marshal.GetDelegateForFunctionPointer<GetRelationColumnDelegate>(funcPtr);
        return func(ctx, sort, col);
    }
}
