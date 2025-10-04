// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Datatypes API - P/Invoke bindings for Z3 datatype query functions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Datatypes API (9 functions):
// - Datatype queries (constructor count, accessors, recognizers)
// - Tuple sort queries (constructor, field count, field accessors)
// - Relation queries (arity, column sorts)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsDatatypes(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Constructor Building

        // Datatype Creation

        // Datatype Queries
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_num_constructors");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_constructor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_recognizer");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_datatype_sort_constructor_accessor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_mk_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_num_fields");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tuple_sort_field_decl");

        // Related Functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_relation_arity");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_relation_column");
    }

    // Delegates
    private delegate uint GetDatatypeSortNumConstructorsDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetDatatypeSortConstructorDelegate(IntPtr ctx, IntPtr sort, uint idx);
    private delegate IntPtr GetDatatypeSortRecognizerDelegate(IntPtr ctx, IntPtr sort, uint idx);
    private delegate IntPtr GetDatatypeSortConstructorAccessorDelegate(IntPtr ctx, IntPtr sort, uint idxC, uint idxA);
    private delegate IntPtr GetTupleSortMkDeclDelegate(IntPtr ctx, IntPtr sort);
    private delegate uint GetTupleSortNumFieldsDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetTupleSortFieldDeclDelegate(IntPtr ctx, IntPtr sort, uint i);
    private delegate uint GetRelationArityDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr GetRelationColumnDelegate(IntPtr ctx, IntPtr sort, uint col);

    // Methods

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
