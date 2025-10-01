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
    private static void LoadFunctionsParameters(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_set_bool");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_set_uint");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_set_double");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_set_symbol");
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_to_string");
    }

    // Delegates
    private delegate IntPtr MkParamsDelegate(IntPtr ctx);
    private delegate void ParamsIncRefDelegate(IntPtr ctx, IntPtr paramsHandle);
    private delegate void ParamsDecRefDelegate(IntPtr ctx, IntPtr paramsHandle);
    private delegate void ParamsSetBoolDelegate(IntPtr ctx, IntPtr paramsHandle, IntPtr key, int value);
    private delegate void ParamsSetDoubleDelegate(IntPtr ctx, IntPtr paramsHandle, IntPtr key, double value);
    private delegate void ParamsSetSymbolDelegate(IntPtr ctx, IntPtr paramsHandle, IntPtr key, IntPtr value);
    private delegate IntPtr ParamsToStringDelegate(IntPtr ctx, IntPtr paramsHandle);
    private delegate void ParamsSetUIntDelegate(IntPtr ctx, IntPtr paramsHandle, IntPtr key, uint value);

    // Methods
    /// <summary>
    /// Creates an empty parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created parameter set.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkParams(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_params");
        var func = Marshal.GetDelegateForFunctionPointer<MkParamsDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments the reference counter of the given parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsIncRef(IntPtr ctx, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_params_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsIncRefDelegate>(funcPtr);
        func(ctx, paramsHandle);
    }

    /// <summary>
    /// Decrements the reference counter of the given parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsDecRef(IntPtr ctx, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_params_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsDecRefDelegate>(funcPtr);
        func(ctx, paramsHandle);
    }

    /// <summary>
    /// Sets a boolean parameter.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="key">The parameter key symbol.</param>
    /// <param name="value">The boolean value (0 for false, 1 for true).</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsSetBool(IntPtr ctx, IntPtr paramsHandle, IntPtr key, int value)
    {
        var funcPtr = GetFunctionPointer("Z3_params_set_bool");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsSetBoolDelegate>(funcPtr);
        func(ctx, paramsHandle, key, value);
    }

    /// <summary>
    /// Sets a double parameter.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="key">The parameter key symbol.</param>
    /// <param name="value">The double value.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsSetDouble(IntPtr ctx, IntPtr paramsHandle, IntPtr key, double value)
    {
        var funcPtr = GetFunctionPointer("Z3_params_set_double");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsSetDoubleDelegate>(funcPtr);
        func(ctx, paramsHandle, key, value);
    }

    /// <summary>
    /// Sets a symbol parameter.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="key">The parameter key symbol.</param>
    /// <param name="value">The parameter value symbol.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsSetSymbol(IntPtr ctx, IntPtr paramsHandle, IntPtr key, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_params_set_symbol");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsSetSymbolDelegate>(funcPtr);
        func(ctx, paramsHandle, key, value);
    }

    /// <summary>
    /// Converts a parameter set to a string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <returns>String representation of the parameter set.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParamsToString(IntPtr ctx, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_params_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsToStringDelegate>(funcPtr);
        return func(ctx, paramsHandle);
    }

    /// <summary>
    /// Sets an unsigned integer parameter.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="key">The parameter key symbol.</param>
    /// <param name="value">The unsigned integer value.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsSetUInt(IntPtr ctx, IntPtr paramsHandle, IntPtr key, uint value)
    {
        var funcPtr = GetFunctionPointer("Z3_params_set_uint");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsSetUIntDelegate>(funcPtr);
        func(ctx, paramsHandle, key, value);
    }
}
