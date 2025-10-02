using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
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
        LoadFunctionOrNull(handle, functionPointers, "Z3_params_validate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_get_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_get_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_get_documentation");
        LoadFunctionOrNull(handle, functionPointers, "Z3_param_descrs_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_global_param_set");
        LoadFunctionOrNull(handle, functionPointers, "Z3_global_param_reset_all");
        LoadFunctionOrNull(handle, functionPointers, "Z3_global_param_get");
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
    private delegate void ParamsValidateDelegate(IntPtr ctx, IntPtr paramsHandle, IntPtr paramDescrs);
    private delegate void ParamDescrsIncRefDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate void ParamDescrsDecRefDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate int ParamDescrsGetKindDelegate(IntPtr ctx, IntPtr paramDescrs, IntPtr name);
    private delegate uint ParamDescrsSizeDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate IntPtr ParamDescrsGetNameDelegate(IntPtr ctx, IntPtr paramDescrs, uint index);
    private delegate IntPtr ParamDescrsGetDocumentationDelegate(IntPtr ctx, IntPtr paramDescrs, IntPtr name);
    private delegate IntPtr ParamDescrsToStringDelegate(IntPtr ctx, IntPtr paramDescrs);
    private delegate void GlobalParamSetDelegate(IntPtr paramId, IntPtr paramValue);
    private delegate void GlobalParamResetAllDelegate();
    private delegate bool GlobalParamGetDelegate(IntPtr paramId, IntPtr paramValue);

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

    /// <summary>
    /// Validates parameters against parameter descriptors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="paramDescrs">The parameter descriptors handle.</param>
    /// <remarks>
    /// Checks that all parameters in the parameter set are valid according to the
    /// parameter descriptors. Raises an error if validation fails.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParamsValidate(IntPtr ctx, IntPtr paramsHandle, IntPtr paramDescrs)
    {
        var funcPtr = GetFunctionPointer("Z3_params_validate");
        var func = Marshal.GetDelegateForFunctionPointer<ParamsValidateDelegate>(funcPtr);
        func(ctx, paramsHandle, paramDescrs);
    }

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
}
