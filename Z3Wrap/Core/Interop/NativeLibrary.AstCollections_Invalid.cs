// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 AST Containers API - P/Invoke bindings for Z3 AST collection types
//
// Source: z3_ast_containers.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_ast_containers.h
//
// This file provides bindings for Z3's AST Containers API (21 functions):
// - AST Vector operations (resizable array of AST nodes)
// - AST Map operations (hash map with AST keys and values)
// - Reference counting for both container types
// - Context translation support
// - Introspection and debugging utilities

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsAstCollections(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // AST Vector Operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ast_vector");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_get");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_set");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_resize");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_push");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_translate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_vector_to_string");

        // AST Map Operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ast_map");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_contains");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_find");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_insert");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_erase");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_reset");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_keys");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_map_to_string");
    }

    // Delegates

    // AST Vector Operations
    private delegate IntPtr MkAstVectorDelegate(IntPtr ctx);
    private delegate void AstVectorIncRefDelegate(IntPtr ctx, IntPtr astVector);
    private delegate void AstVectorDecRefDelegate(IntPtr ctx, IntPtr astVector);
    private delegate uint AstVectorSizeDelegate(IntPtr ctx, IntPtr astVector);
    private delegate IntPtr AstVectorGetDelegate(IntPtr ctx, IntPtr astVector, uint index);
    private delegate void AstVectorSetDelegate(IntPtr ctx, IntPtr astVector, uint index, IntPtr ast);
    private delegate void AstVectorResizeDelegate(IntPtr ctx, IntPtr astVector, uint newSize);
    private delegate void AstVectorPushDelegate(IntPtr ctx, IntPtr astVector, IntPtr ast);
    private delegate IntPtr AstVectorTranslateDelegate(IntPtr sourceCtx, IntPtr astVector, IntPtr targetCtx);
    private delegate IntPtr AstVectorToStringDelegate(IntPtr ctx, IntPtr astVector);

    // AST Map Operations
    private delegate IntPtr MkAstMapDelegate(IntPtr ctx);
    private delegate void AstMapIncRefDelegate(IntPtr ctx, IntPtr astMap);
    private delegate void AstMapDecRefDelegate(IntPtr ctx, IntPtr astMap);
    private delegate bool AstMapContainsDelegate(IntPtr ctx, IntPtr astMap, IntPtr key);
    private delegate IntPtr AstMapFindDelegate(IntPtr ctx, IntPtr astMap, IntPtr key);
    private delegate void AstMapInsertDelegate(IntPtr ctx, IntPtr astMap, IntPtr key, IntPtr value);
    private delegate void AstMapEraseDelegate(IntPtr ctx, IntPtr astMap, IntPtr key);
    private delegate void AstMapResetDelegate(IntPtr ctx, IntPtr astMap);
    private delegate uint AstMapSizeDelegate(IntPtr ctx, IntPtr astMap);
    private delegate IntPtr AstMapKeysDelegate(IntPtr ctx, IntPtr astMap);
    private delegate IntPtr AstMapToStringDelegate(IntPtr ctx, IntPtr astMap);

    // Methods

    // AST Vector Operations
    /// <summary>
    /// Creates empty AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>AST vector handle.</returns>
    /// <remarks>
    /// AST vectors are resizable arrays of AST nodes.
    /// Used for collecting multiple expressions, sorts, or function declarations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAstVector(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ast_vector");
        var func = Marshal.GetDelegateForFunctionPointer<MkAstVectorDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments reference count for AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <remarks>
    /// Prevents AST vector from being garbage collected by Z3.
    /// Must be balanced with AstVectorDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstVectorIncRef(IntPtr ctx, IntPtr astVector)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorIncRefDelegate>(funcPtr);
        func(ctx, astVector);
    }

    /// <summary>
    /// Decrements reference count for AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <remarks>
    /// When reference count reaches zero, AST vector is freed by Z3.
    /// Must be balanced with AstVectorIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstVectorDecRef(IntPtr ctx, IntPtr astVector)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorDecRefDelegate>(funcPtr);
        func(ctx, astVector);
    }

    /// <summary>
    /// Gets size of AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <returns>Number of elements in vector.</returns>
    /// <remarks>
    /// Returns current number of AST nodes stored in vector.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint AstVectorSize(IntPtr ctx, IntPtr astVector)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_size");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorSizeDelegate>(funcPtr);
        return func(ctx, astVector);
    }

    /// <summary>
    /// Gets element at index in AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <param name="index">Zero-based element index.</param>
    /// <returns>AST handle at specified index.</returns>
    /// <remarks>
    /// Retrieves AST node at given position. Index must be less than vector size.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstVectorGet(IntPtr ctx, IntPtr astVector, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_get");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorGetDelegate>(funcPtr);
        return func(ctx, astVector, index);
    }

    /// <summary>
    /// Sets element at index in AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <param name="index">Zero-based element index.</param>
    /// <param name="ast">AST handle to store at index.</param>
    /// <remarks>
    /// Replaces AST node at given position. Index must be less than vector size.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstVectorSet(IntPtr ctx, IntPtr astVector, uint index, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_set");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorSetDelegate>(funcPtr);
        func(ctx, astVector, index, ast);
    }

    /// <summary>
    /// Resizes AST vector to new size.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <param name="newSize">New vector size.</param>
    /// <remarks>
    /// Changes vector capacity. If growing, new elements are uninitialized.
    /// If shrinking, elements beyond new size are discarded.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstVectorResize(IntPtr ctx, IntPtr astVector, uint newSize)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_resize");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorResizeDelegate>(funcPtr);
        func(ctx, astVector, newSize);
    }

    /// <summary>
    /// Appends element to end of AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <param name="ast">AST handle to append.</param>
    /// <remarks>
    /// Adds AST node to end of vector, increasing size by one.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstVectorPush(IntPtr ctx, IntPtr astVector, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_push");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorPushDelegate>(funcPtr);
        func(ctx, astVector, ast);
    }

    /// <summary>
    /// Translates AST vector to another context.
    /// </summary>
    /// <param name="sourceCtx">Source context handle.</param>
    /// <param name="astVector">AST vector handle in source context.</param>
    /// <param name="targetCtx">Target context handle.</param>
    /// <returns>AST vector handle in target context.</returns>
    /// <remarks>
    /// Creates copy of AST vector with all elements translated to target context.
    /// Required when moving AST nodes between different Z3 contexts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstVectorTranslate(IntPtr sourceCtx, IntPtr astVector, IntPtr targetCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_translate");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorTranslateDelegate>(funcPtr);
        return func(sourceCtx, astVector, targetCtx);
    }

    /// <summary>
    /// Gets string representation of AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astVector">AST vector handle.</param>
    /// <returns>String representation of all elements.</returns>
    /// <remarks>
    /// Returns SMTLIB2 format string showing all AST nodes in vector.
    /// Useful for debugging and logging.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstVectorToString(IntPtr ctx, IntPtr astVector)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_vector_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<AstVectorToStringDelegate>(funcPtr);
        return func(ctx, astVector);
    }

    // AST Map Operations
    /// <summary>
    /// Creates empty AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>AST map handle.</returns>
    /// <remarks>
    /// AST maps store key-value pairs where both keys and values are AST nodes.
    /// Used for memoization, substitution tables, and caching.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAstMap(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ast_map");
        var func = Marshal.GetDelegateForFunctionPointer<MkAstMapDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments reference count for AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <remarks>
    /// Prevents AST map from being garbage collected by Z3.
    /// Must be balanced with AstMapDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstMapIncRef(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapIncRefDelegate>(funcPtr);
        func(ctx, astMap);
    }

    /// <summary>
    /// Decrements reference count for AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <remarks>
    /// When reference count reaches zero, AST map is freed by Z3.
    /// Must be balanced with AstMapIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstMapDecRef(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapDecRefDelegate>(funcPtr);
        func(ctx, astMap);
    }

    /// <summary>
    /// Checks if key exists in AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <param name="key">AST key to search for.</param>
    /// <returns>True if key exists in map, false otherwise.</returns>
    /// <remarks>
    /// Tests map membership without retrieving value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool AstMapContains(IntPtr ctx, IntPtr astMap, IntPtr key)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_contains");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapContainsDelegate>(funcPtr);
        return func(ctx, astMap, key);
    }

    /// <summary>
    /// Gets value for key in AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <param name="key">AST key to look up.</param>
    /// <returns>AST value associated with key, or IntPtr.Zero if key not found.</returns>
    /// <remarks>
    /// Retrieves mapped value for given key. Returns null pointer if key absent.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstMapFind(IntPtr ctx, IntPtr astMap, IntPtr key)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_find");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapFindDelegate>(funcPtr);
        return func(ctx, astMap, key);
    }

    /// <summary>
    /// Inserts or updates key-value pair in AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <param name="key">AST key.</param>
    /// <param name="value">AST value to associate with key.</param>
    /// <remarks>
    /// Adds new mapping or updates existing mapping for given key.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstMapInsert(IntPtr ctx, IntPtr astMap, IntPtr key, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_insert");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapInsertDelegate>(funcPtr);
        func(ctx, astMap, key, value);
    }

    /// <summary>
    /// Removes key-value pair from AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <param name="key">AST key to remove.</param>
    /// <remarks>
    /// Deletes mapping for given key. No-op if key not present.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstMapErase(IntPtr ctx, IntPtr astMap, IntPtr key)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_erase");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapEraseDelegate>(funcPtr);
        func(ctx, astMap, key);
    }

    /// <summary>
    /// Removes all entries from AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <remarks>
    /// Clears all key-value pairs, resulting in empty map.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void AstMapReset(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_reset");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapResetDelegate>(funcPtr);
        func(ctx, astMap);
    }

    /// <summary>
    /// Gets number of entries in AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <returns>Number of key-value pairs in map.</returns>
    /// <remarks>
    /// Returns current size of map.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint AstMapSize(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_size");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapSizeDelegate>(funcPtr);
        return func(ctx, astMap);
    }

    /// <summary>
    /// Gets all keys in AST map as AST vector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <returns>AST vector containing all keys.</returns>
    /// <remarks>
    /// Creates vector with all map keys. Order is unspecified.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstMapKeys(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_keys");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapKeysDelegate>(funcPtr);
        return func(ctx, astMap);
    }

    /// <summary>
    /// Gets string representation of AST map.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="astMap">AST map handle.</param>
    /// <returns>String representation of all key-value pairs.</returns>
    /// <remarks>
    /// Returns SMTLIB2 format string showing all mappings in map.
    /// Useful for debugging and logging.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstMapToString(IntPtr ctx, IntPtr astMap)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_map_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<AstMapToStringDelegate>(funcPtr);
        return func(ctx, astMap);
    }
}
