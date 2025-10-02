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
    private static void LoadFunctionsConstraints(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_atleast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_atmost");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_pbeq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_pbge");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_pble");
    }

    // Delegates
    private delegate IntPtr MkAtLeastDelegate(IntPtr ctx, uint numArgs, IntPtr[] args, uint k);
    private delegate IntPtr MkAtMostDelegate(IntPtr ctx, uint numArgs, IntPtr[] args, uint k);
    private delegate IntPtr MkPbEqDelegate(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);
    private delegate IntPtr MkPbGeDelegate(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);
    private delegate IntPtr MkPbLeDelegate(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);

    // Methods
    /// <summary>
    /// Creates at-least-K constraint over Boolean variables.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of Boolean variables.</param>
    /// <param name="args">Array of Boolean expressions.</param>
    /// <param name="k">Minimum number of variables that must be true.</param>
    /// <returns>AST node representing constraint that at least k of args are true.</returns>
    /// <remarks>
    /// Cardinality constraint: at least k variables must be true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAtLeast(IntPtr ctx, uint numArgs, IntPtr[] args, uint k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_atleast");
        var func = Marshal.GetDelegateForFunctionPointer<MkAtLeastDelegate>(funcPtr);
        return func(ctx, numArgs, args, k);
    }

    /// <summary>
    /// Creates at-most-K constraint over Boolean variables.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of Boolean variables.</param>
    /// <param name="args">Array of Boolean expressions.</param>
    /// <param name="k">Maximum number of variables that can be true.</param>
    /// <returns>AST node representing constraint that at most k of args are true.</returns>
    /// <remarks>
    /// Cardinality constraint: at most k variables can be true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAtMost(IntPtr ctx, uint numArgs, IntPtr[] args, uint k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_atmost");
        var func = Marshal.GetDelegateForFunctionPointer<MkAtMostDelegate>(funcPtr);
        return func(ctx, numArgs, args, k);
    }

    /// <summary>
    /// Creates pseudo-Boolean equality constraint.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of Boolean variables.</param>
    /// <param name="args">Array of Boolean expressions.</param>
    /// <param name="coeffs">Array of integer coefficients.</param>
    /// <param name="k">Target sum value.</param>
    /// <returns>AST node representing constraint that weighted sum equals k.</returns>
    /// <remarks>
    /// Pseudo-Boolean constraint: coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] = k.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPbEq(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pbeq");
        var func = Marshal.GetDelegateForFunctionPointer<MkPbEqDelegate>(funcPtr);
        return func(ctx, numArgs, args, coeffs, k);
    }

    /// <summary>
    /// Creates pseudo-Boolean greater-than-or-equal constraint.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of Boolean variables.</param>
    /// <param name="args">Array of Boolean expressions.</param>
    /// <param name="coeffs">Array of integer coefficients.</param>
    /// <param name="k">Minimum sum value.</param>
    /// <returns>AST node representing constraint that weighted sum is at least k.</returns>
    /// <remarks>
    /// Pseudo-Boolean constraint: coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] &gt;= k.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPbGe(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pbge");
        var func = Marshal.GetDelegateForFunctionPointer<MkPbGeDelegate>(funcPtr);
        return func(ctx, numArgs, args, coeffs, k);
    }

    /// <summary>
    /// Creates pseudo-Boolean less-than-or-equal constraint.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of Boolean variables.</param>
    /// <param name="args">Array of Boolean expressions.</param>
    /// <param name="coeffs">Array of integer coefficients.</param>
    /// <param name="k">Maximum sum value.</param>
    /// <returns>AST node representing constraint that weighted sum is at most k.</returns>
    /// <remarks>
    /// Pseudo-Boolean constraint: coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] &lt;= k.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPbLe(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pble");
        var func = Marshal.GetDelegateForFunctionPointer<MkPbLeDelegate>(funcPtr);
        return func(ctx, numArgs, args, coeffs, k);
    }
}
