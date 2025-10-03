// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Context Management API - P/Invoke bindings for Z3 context and configuration operations
//
// Source: z3_api.h from Z3 C API (Context Management section)
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's context management API (8 functions):
// - Configuration object creation and management (MkConfig, DelConfig, SetParamValue)
// - Context lifecycle management (MkContextRc, DelContext, UpdateParamValue)
// - Reference counting for memory management (IncRef, DecRef)
//
// Note: Global parameter functions (Z3_global_param_*) are in NativeLibrary.Parameters.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsContext(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_config");
        LoadFunctionInternal(handle, functionPointers, "Z3_del_config");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_context_rc");
        LoadFunctionInternal(handle, functionPointers, "Z3_del_context");
        LoadFunctionInternal(handle, functionPointers, "Z3_inc_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_dec_ref");

        LoadFunctionOrNull(handle, functionPointers, "Z3_set_param_value");
        LoadFunctionOrNull(handle, functionPointers, "Z3_update_param_value");
    }

    // Delegates
    private delegate IntPtr MkConfigDelegate();
    private delegate void DelConfigDelegate(IntPtr cfg);
    private delegate void SetParamValueDelegate(IntPtr cfg, IntPtr paramId, IntPtr paramValue);
    private delegate IntPtr MkContextRcDelegate(IntPtr cfg);
    private delegate void DelContextDelegate(IntPtr ctx);
    private delegate void UpdateParamValueDelegate(IntPtr ctx, IntPtr paramId, IntPtr paramValue);
    private delegate void IncRefDelegate(IntPtr ctx, IntPtr ast);
    private delegate void DecRefDelegate(IntPtr ctx, IntPtr ast);

    // Methods

    /// <summary>
    /// Creates a Z3 configuration object that can be used to configure various solver settings.
    /// </summary>
    /// <returns>Handle to the created Z3 configuration object.</returns>
    /// <remarks>
    /// The configuration object must be deleted using DelConfig when no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConfig()
    {
        var funcPtr = GetFunctionPointer("Z3_mk_config");
        var func = Marshal.GetDelegateForFunctionPointer<MkConfigDelegate>(funcPtr);
        return func();
    }

    /// <summary>
    /// Deletes a Z3 configuration object and releases its memory.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle to delete.</param>
    /// <remarks>
    /// Should be called for every configuration object created with MkConfig.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DelConfig(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_del_config");
        var func = Marshal.GetDelegateForFunctionPointer<DelConfigDelegate>(funcPtr);
        func(cfg);
    }

    /// <summary>
    /// Sets a configuration parameter value on a Z3 configuration object.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle.</param>
    /// <param name="paramId">The ANSI string pointer for parameter name.</param>
    /// <param name="paramValue">The ANSI string pointer for parameter value.</param>
    /// <remarks>
    /// Must be called before creating a context with the configuration.
    /// Some parameters can only be set at context creation time.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SetParamValue(IntPtr cfg, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_set_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<SetParamValueDelegate>(funcPtr);
        func(cfg, paramId, paramValue);
    }

    /// <summary>
    /// Creates a Z3 context with reference counting enabled for automatic memory management.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle to use for context creation.</param>
    /// <returns>Handle to the created Z3 context with reference counting.</returns>
    /// <remarks>
    /// Reference counting automatically manages memory for AST nodes created within this context.
    /// The context must be deleted using DelContext when no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkContextRc(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context_rc");
        var func = Marshal.GetDelegateForFunctionPointer<MkContextRcDelegate>(funcPtr);
        return func(cfg);
    }

    /// <summary>
    /// Deletes a Z3 context and releases all associated memory.
    /// </summary>
    /// <param name="ctx">The Z3 context handle to delete.</param>
    /// <remarks>
    /// All objects created within this context become invalid after deletion.
    /// Should be called for every context created with MkContextRc.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DelContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_del_context");
        var func = Marshal.GetDelegateForFunctionPointer<DelContextDelegate>(funcPtr);
        func(ctx);
    }

    /// <summary>
    /// Updates a parameter value in the Z3 context configuration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramId">The parameter identifier to update.</param>
    /// <param name="paramValue">The new parameter value to set.</param>
    /// <remarks>
    /// Used to dynamically modify Z3 solver behavior and optimization settings.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_update_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<UpdateParamValueDelegate>(funcPtr);
        func(ctx, paramId, paramValue);
    }

    /// <summary>
    /// Increments the reference counter of a Z3 AST node to prevent premature deallocation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The Z3 AST node handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Must be paired with DecRef
    /// when the AST node is no longer needed to prevent memory leaks.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void IncRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<IncRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    /// <summary>
    /// Decrements the reference counter of a Z3 AST node, potentially allowing its deallocation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The Z3 AST node handle to decrement reference count for.</param>
    /// <remarks>
    /// Should be called for every IncRef to properly manage memory and prevent leaks.
    /// When reference count reaches zero, the AST node may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void DecRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<DecRefDelegate>(funcPtr);
        func(ctx, ast);
    }
}
