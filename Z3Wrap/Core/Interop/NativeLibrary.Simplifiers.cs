using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSimplifiers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_simplifier");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_param_descrs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_descr");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_using_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_and_then");
    }

    // Delegates
    private delegate IntPtr MkSimplifierDelegate(IntPtr ctx, IntPtr name);
    private delegate void SimplifierIncRefDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate void SimplifierDecRefDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetHelpDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetParamDescrsDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetDescrDelegate(IntPtr ctx, IntPtr name);
    private delegate IntPtr SimplifierUsingParamsDelegate(IntPtr ctx, IntPtr simplifier, IntPtr paramsHandle);
    private delegate IntPtr SimplifierAndThenDelegate(IntPtr ctx, IntPtr simplifier1, IntPtr simplifier2);

    // Methods
    /// <summary>
    /// Creates a simplifier by name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the simplifier.</param>
    /// <returns>Simplifier handle.</returns>
    /// <remarks>
    /// Creates a simplifier object given its name. Simplifiers are used to preprocess
    /// formulas before solving. Common simplifiers include "simplify", "propagate-values", "elim-uncnstr".
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSimplifier(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_simplifier");
        var func = Marshal.GetDelegateForFunctionPointer<MkSimplifierDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Increments the reference counter of simplifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier">The simplifier handle.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Increment the reference count
    /// to prevent premature deallocation. Must be paired with SimplifierDecRef.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SimplifierIncRef(IntPtr ctx, IntPtr simplifier)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierIncRefDelegate>(funcPtr);
        func(ctx, simplifier);
    }

    /// <summary>
    /// Decrements the reference counter of simplifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier">The simplifier handle.</param>
    /// <remarks>
    /// Must be paired with SimplifierIncRef. When reference count reaches zero,
    /// the simplifier may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SimplifierDecRef(IntPtr ctx, IntPtr simplifier)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierDecRefDelegate>(funcPtr);
        func(ctx, simplifier);
    }

    /// <summary>
    /// Retrieves help string for simplifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier">The simplifier handle.</param>
    /// <returns>Help string describing the simplifier.</returns>
    /// <remarks>
    /// Returns a string describing the simplifier and its parameters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifierGetHelp(IntPtr ctx, IntPtr simplifier)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_get_help");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierGetHelpDelegate>(funcPtr);
        return func(ctx, simplifier);
    }

    /// <summary>
    /// Retrieves parameter descriptors for simplifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier">The simplifier handle.</param>
    /// <returns>Parameter descriptors object handle.</returns>
    /// <remarks>
    /// Returns a parameter descriptor set that describes all available parameters
    /// for the given simplifier.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifierGetParamDescrs(IntPtr ctx, IntPtr simplifier)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_get_param_descrs");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierGetParamDescrsDelegate>(funcPtr);
        return func(ctx, simplifier);
    }

    /// <summary>
    /// Retrieves description string for named simplifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the simplifier.</param>
    /// <returns>Description string for the simplifier.</returns>
    /// <remarks>
    /// Returns a string describing what the named simplifier does.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifierGetDescr(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_get_descr");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierGetDescrDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Creates simplifier configured with parameters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier">The simplifier to configure.</param>
    /// <param name="paramsHandle">Parameter set handle.</param>
    /// <returns>Parameterized simplifier handle.</returns>
    /// <remarks>
    /// Returns a simplifier that is a copy of the given simplifier, but uses the given
    /// parameter set.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifierUsingParams(IntPtr ctx, IntPtr simplifier, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_using_params");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierUsingParamsDelegate>(funcPtr);
        return func(ctx, simplifier, paramsHandle);
    }

    /// <summary>
    /// Creates sequential composition of two simplifiers.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="simplifier1">First simplifier to apply.</param>
    /// <param name="simplifier2">Second simplifier to apply.</param>
    /// <returns>Composite simplifier that applies simplifier1 then simplifier2.</returns>
    /// <remarks>
    /// Returns a simplifier that applies simplifier1 to a formula, then applies
    /// simplifier2 to the result.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifierAndThen(IntPtr ctx, IntPtr simplifier1, IntPtr simplifier2)
    {
        var funcPtr = GetFunctionPointer("Z3_simplifier_and_then");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifierAndThenDelegate>(funcPtr);
        return func(ctx, simplifier1, simplifier2);
    }
}
