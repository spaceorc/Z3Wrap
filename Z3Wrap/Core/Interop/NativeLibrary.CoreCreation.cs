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
    private static void LoadFunctionsCoreCreation(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_distinct");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_abs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_power");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_is_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_divides");
    }

    // Delegates
    private delegate IntPtr MkDistinctDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkAbsDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr MkPowerDelegate(IntPtr ctx, IntPtr arg1, IntPtr arg2);
    private delegate IntPtr MkIsIntDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkDividesDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Methods
    /// <summary>
    /// Creates distinct constraint over expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">Number of expressions.</param>
    /// <param name="args">Array of expression handles.</param>
    /// <returns>AST node representing distinct constraint ensuring all arguments are pairwise different.</returns>
    /// <remarks>
    /// The distinct constraint asserts that all provided expressions must have different values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDistinct(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_distinct");
        var func = Marshal.GetDelegateForFunctionPointer<MkDistinctDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates absolute value expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The argument expression.</param>
    /// <returns>AST node representing absolute value of argument.</returns>
    /// <remarks>
    /// Computes |arg| for integer and real arguments.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAbs(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_abs");
        var func = Marshal.GetDelegateForFunctionPointer<MkAbsDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates power expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg1">The base expression.</param>
    /// <param name="arg2">The exponent expression.</param>
    /// <returns>AST node representing arg1 raised to the power of arg2.</returns>
    /// <remarks>
    /// Computes arg1 ^ arg2 for integer and real arguments.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPower(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_power");
        var func = Marshal.GetDelegateForFunctionPointer<MkPowerDelegate>(funcPtr);
        return func(ctx, arg1, arg2);
    }

    /// <summary>
    /// Creates integer test predicate.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The expression to test.</param>
    /// <returns>AST node representing Boolean predicate testing if t1 is an integer.</returns>
    /// <remarks>
    /// Returns true if real-valued expression t1 has an integer value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIsInt(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_is_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkIsIntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates divisibility test predicate.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The divisor expression.</param>
    /// <param name="t2">The dividend expression.</param>
    /// <returns>AST node representing Boolean predicate testing if t1 divides t2.</returns>
    /// <remarks>
    /// Returns true if t2 is divisible by t1 (i.e., t2 mod t1 = 0).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDivides(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_divides");
        var func = Marshal.GetDelegateForFunctionPointer<MkDividesDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }
}
