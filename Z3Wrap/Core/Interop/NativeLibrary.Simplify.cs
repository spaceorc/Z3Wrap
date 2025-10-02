using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSimplify(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplify");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplify_ex");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplify_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplify_get_param_descrs");
    }

    // Delegates

    private delegate IntPtr SimplifyDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr SimplifyExDelegate(IntPtr ctx, IntPtr ast, IntPtr paramsHandle);
    private delegate IntPtr SimplifyGetHelpDelegate(IntPtr ctx);
    private delegate IntPtr SimplifyGetParamDescrsDelegate(IntPtr ctx);

    // Methods

    /// <summary>
    /// Simplifies expression using default parameter settings.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST expression to simplify.</param>
    /// <returns>Simplified AST expression.</returns>
    /// <remarks>
    /// Applies rewriting rules and simplifications to reduce expression complexity.
    /// Uses default simplification parameters. For custom parameters, use SimplifyEx.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Simplify(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_simplify");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifyDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Simplifies expression using custom parameter settings.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST expression to simplify.</param>
    /// <param name="paramsHandle">The parameters handle controlling simplification.</param>
    /// <returns>Simplified AST expression.</returns>
    /// <remarks>
    /// Extended version of Simplify allowing fine-grained control over simplification
    /// strategies through parameter object. Useful for performance tuning or specific
    /// simplification requirements.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifyEx(IntPtr ctx, IntPtr ast, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_simplify_ex");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifyExDelegate>(funcPtr);
        return func(ctx, ast, paramsHandle);
    }

    /// <summary>
    /// Gets help string describing simplification parameters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>String containing parameter documentation.</returns>
    /// <remarks>
    /// Returns human-readable description of available simplification parameters
    /// and their effects. Useful for understanding simplification options.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifyGetHelp(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_simplify_get_help");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifyGetHelpDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Gets parameter descriptors for simplification.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Parameter descriptors handle.</returns>
    /// <remarks>
    /// Returns formal description of all simplification parameters including types,
    /// valid values, and documentation. Used for programmatic parameter discovery
    /// and validation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SimplifyGetParamDescrs(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_simplify_get_param_descrs");
        var func = Marshal.GetDelegateForFunctionPointer<SimplifyGetParamDescrsDelegate>(funcPtr);
        return func(ctx);
    }
}
