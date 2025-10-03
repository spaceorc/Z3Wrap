// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Parameter Descriptions API - P/Invoke bindings for Z3 parameter descriptor queries
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Parameter Descriptions API (7 functions):
// - Reference counting (Z3_param_descrs_inc_ref, Z3_param_descrs_dec_ref)
// - Descriptor queries (kind, size, name, documentation)
// - String conversion (Z3_param_descrs_to_string)
//
// Parameter descriptors describe available parameters for solvers, tactics, etc.
//
// Missing Functions (0 functions):
// None - all functions implemented ✓

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsParameterDescriptions(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_inc_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_dec_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_get_kind");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_size");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_get_name");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_get_documentation");
        LoadFunctionInternal(handle, functionPointers, "Z3_param_descrs_to_string");
    }

    // Delegates
    private delegate void ParamDescrsIncRefDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate void ParamDescrsDecRefDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate int ParamDescrsGetKindDelegate(IntPtr ctx, IntPtr paramDescrs, IntPtr name);
    private delegate uint ParamDescrsSizeDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate IntPtr ParamDescrsGetNameDelegate(IntPtr ctx, IntPtr paramDescrs, uint index);
    private delegate IntPtr ParamDescrsGetDocumentationDelegate(IntPtr ctx, IntPtr paramDescrs, IntPtr name);
    private delegate IntPtr ParamDescrsToStringDelegate(IntPtr ctx, IntPtr paramDescrs);

    // Methods
    /// <summary>
    /// Increments the reference counter of parameter descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Must be paired with ParamDescrsDecRef.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamDescrsIncRef(IntPtr ctx, IntPtr paramDescrs)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsIncRefDelegate>(funcPtr);
        func(ctx, paramDescrs);
    }

    /// <summary>
    /// Decrements the reference counter of parameter descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <remarks>
    /// Must be paired with ParamDescrsIncRef. When reference count reaches zero,
    /// the descriptors may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamDescrsDecRef(IntPtr ctx, IntPtr paramDescrs)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsDecRefDelegate>(funcPtr);
        func(ctx, paramDescrs);
    }

    /// <summary>
    /// Retrieves parameter kind from descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <param name="name">Parameter name symbol.</param>
    /// <returns>Parameter kind (0=uint, 1=bool, 2=double, 3=symbol, 4=invalid).</returns>
    /// <remarks>
    /// Returns the type/kind of the parameter with the given name.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int ParamDescrsGetKind(IntPtr ctx, IntPtr paramDescrs, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_get_kind");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsGetKindDelegate>(funcPtr);
        return func(ctx, paramDescrs, name);
    }

    /// <summary>
    /// Retrieves number of parameters in descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <returns>Number of parameters.</returns>
    /// <remarks>
    /// Returns the number of parameters described in the parameter descriptors.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ParamDescrsSize(IntPtr ctx, IntPtr paramDescrs)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_size");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsSizeDelegate>(funcPtr);
        return func(ctx, paramDescrs);
    }

    /// <summary>
    /// Retrieves parameter name by index from descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <param name="index">Parameter index (0-based).</param>
    /// <returns>Parameter name symbol.</returns>
    /// <remarks>
    /// Returns the name symbol of the parameter at the given index.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParamDescrsGetName(IntPtr ctx, IntPtr paramDescrs, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_get_name");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsGetNameDelegate>(funcPtr);
        return func(ctx, paramDescrs, index);
    }

    /// <summary>
    /// Retrieves documentation string for parameter.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <param name="name">Parameter name symbol.</param>
    /// <returns>Documentation string for the parameter.</returns>
    /// <remarks>
    /// Returns a string describing what the parameter does and its valid values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParamDescrsGetDocumentation(IntPtr ctx, IntPtr paramDescrs, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_get_documentation");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsGetDocumentationDelegate>(funcPtr);
        return func(ctx, paramDescrs, name);
    }

    /// <summary>
    /// Converts parameter descriptors to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <returns>String representation of parameter descriptors.</returns>
    /// <remarks>
    /// Returns a string describing all parameters in the descriptor set.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParamDescrsToString(IntPtr ctx, IntPtr paramDescrs)
    {
        var funcPtr = GetFunctionPointer("Z3_param_descrs_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<ParamDescrsToStringDelegate>(funcPtr);
        return func(ctx, paramDescrs);
    }
}
