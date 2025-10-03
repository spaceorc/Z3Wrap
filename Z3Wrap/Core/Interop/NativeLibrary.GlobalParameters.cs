// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Global Parameters API - P/Invoke bindings for Z3 global parameter management
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Global Parameters API (3 functions):
// - Global parameter get/set (Z3_global_param_set, Z3_global_param_get)
// - Global parameter reset (Z3_global_param_reset_all)
//
// Note: Global parameters affect all Z3 contexts created after they are set
//
// Missing Functions (0 functions):
// None - all functions implemented ✓

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsGlobalParameters(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_global_param_set");
        LoadFunctionInternal(handle, functionPointers, "Z3_global_param_get");
        LoadFunctionInternal(handle, functionPointers, "Z3_global_param_reset_all");
    }

    // Delegates
    private delegate void GlobalParamSetDelegate(IntPtr paramId, IntPtr paramValue);
    private delegate void GlobalParamResetAllDelegate();
    private delegate bool GlobalParamGetDelegate(IntPtr paramId, IntPtr paramValue);

    // Methods
    /// <summary>
    /// Sets a global parameter.
    /// </summary>
    /// <param name="paramId">Parameter ID string.</param>
    /// <param name="paramValue">Parameter value string.</param>
    /// <remarks>
    /// Sets a global (context-independent) parameter. Global parameters affect all Z3
    /// contexts created after the parameter is set.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GlobalParamSet(IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_global_param_set");
        var func = Marshal.GetDelegateForFunctionPointer<GlobalParamSetDelegate>(funcPtr);
        func(paramId, paramValue);
    }


    /// <summary>
    /// Retrieves the value of a global parameter.
    /// </summary>
    /// <param name="paramId">Parameter ID string.</param>
    /// <param name="paramValue">Output buffer for parameter value string.</param>
    /// <returns>True if parameter exists, false otherwise.</returns>
    /// <remarks>
    /// Retrieves the current value of a global parameter. Returns false if the
    /// parameter has not been set.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GlobalParamGet(IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_global_param_get");
        var func = Marshal.GetDelegateForFunctionPointer<GlobalParamGetDelegate>(funcPtr);
        return func(paramId, paramValue);
    }

    /// <summary>
    /// Resets all global parameters to default values.
    /// </summary>
    /// <remarks>
    /// Resets all global parameters to their default values. Affects subsequently
    /// created contexts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GlobalParamResetAll()
    {
        var funcPtr = GetFunctionPointer("Z3_global_param_reset_all");
        var func = Marshal.GetDelegateForFunctionPointer<GlobalParamResetAllDelegate>(funcPtr);
        func();
    }
}
