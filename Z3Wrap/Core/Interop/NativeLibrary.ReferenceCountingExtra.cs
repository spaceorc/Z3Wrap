using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for additional reference counting functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for additional reference counting functions.
    /// </summary>
    private static void LoadFunctionsReferenceCountingExtra(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Apply Result
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_dec_ref");

        // Statistics
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_dec_ref");

        // Func Entry
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_entry_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_entry_dec_ref");

        // Func Interp
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_dec_ref");

        // Pattern
        LoadFunctionOrNull(handle, functionPointers, "Z3_pattern_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_pattern_dec_ref");

        // Fixedpoint
        LoadFunctionOrNull(handle, functionPointers, "Z3_fixedpoint_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fixedpoint_dec_ref");
    }

    // Delegates

    // Apply Result
    private delegate void ApplyResultIncRefDelegate(IntPtr ctx, IntPtr applyResult);
    private delegate void ApplyResultDecRefDelegate(IntPtr ctx, IntPtr applyResult);

    // Statistics
    private delegate void StatsIncRefDelegate(IntPtr ctx, IntPtr stats);
    private delegate void StatsDecRefDelegate(IntPtr ctx, IntPtr stats);

    // Func Entry
    private delegate void FuncEntryIncRefDelegate(IntPtr ctx, IntPtr funcEntry);
    private delegate void FuncEntryDecRefDelegate(IntPtr ctx, IntPtr funcEntry);

    // Func Interp
    private delegate void FuncInterpIncRefDelegate(IntPtr ctx, IntPtr funcInterp);
    private delegate void FuncInterpDecRefDelegate(IntPtr ctx, IntPtr funcInterp);

    // Pattern
    private delegate void PatternIncRefDelegate(IntPtr ctx, IntPtr pattern);
    private delegate void PatternDecRefDelegate(IntPtr ctx, IntPtr pattern);

    // Fixedpoint
    private delegate void FixedpointIncRefDelegate(IntPtr ctx, IntPtr fixedpoint);
    private delegate void FixedpointDecRefDelegate(IntPtr ctx, IntPtr fixedpoint);

    // Methods

    // Apply Result
    /// <summary>
    /// Increments reference count for apply result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="applyResult">Apply result handle.</param>
    /// <remarks>
    /// Prevents apply result from being garbage collected by Z3.
    /// Apply results are returned by tactic application.
    /// Must be balanced with ApplyResultDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ApplyResultIncRef(IntPtr ctx, IntPtr applyResult)
    {
        var funcPtr = GetFunctionPointer("Z3_apply_result_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ApplyResultIncRefDelegate>(funcPtr);
        func(ctx, applyResult);
    }

    /// <summary>
    /// Decrements reference count for apply result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="applyResult">Apply result handle.</param>
    /// <remarks>
    /// When reference count reaches zero, apply result is freed by Z3.
    /// Must be balanced with ApplyResultIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ApplyResultDecRef(IntPtr ctx, IntPtr applyResult)
    {
        var funcPtr = GetFunctionPointer("Z3_apply_result_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ApplyResultDecRefDelegate>(funcPtr);
        func(ctx, applyResult);
    }

    // Statistics
    /// <summary>
    /// Increments reference count for statistics object.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">Statistics object handle.</param>
    /// <remarks>
    /// Prevents statistics object from being garbage collected by Z3.
    /// Statistics objects contain solver performance metrics.
    /// Must be balanced with StatsDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void StatsIncRef(IntPtr ctx, IntPtr stats)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<StatsIncRefDelegate>(funcPtr);
        func(ctx, stats);
    }

    /// <summary>
    /// Decrements reference count for statistics object.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">Statistics object handle.</param>
    /// <remarks>
    /// When reference count reaches zero, statistics object is freed by Z3.
    /// Must be balanced with StatsIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void StatsDecRef(IntPtr ctx, IntPtr stats)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<StatsDecRefDelegate>(funcPtr);
        func(ctx, stats);
    }

    // Func Entry
    /// <summary>
    /// Increments reference count for function entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcEntry">Function entry handle.</param>
    /// <remarks>
    /// Prevents function entry from being garbage collected by Z3.
    /// Function entries represent single mappings in function interpretations.
    /// Must be balanced with FuncEntryDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncEntryIncRef(IntPtr ctx, IntPtr funcEntry)
    {
        var funcPtr = GetFunctionPointer("Z3_func_entry_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FuncEntryIncRefDelegate>(funcPtr);
        func(ctx, funcEntry);
    }

    /// <summary>
    /// Decrements reference count for function entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcEntry">Function entry handle.</param>
    /// <remarks>
    /// When reference count reaches zero, function entry is freed by Z3.
    /// Must be balanced with FuncEntryIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncEntryDecRef(IntPtr ctx, IntPtr funcEntry)
    {
        var funcPtr = GetFunctionPointer("Z3_func_entry_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FuncEntryDecRefDelegate>(funcPtr);
        func(ctx, funcEntry);
    }

    // Func Interp
    /// <summary>
    /// Increments reference count for function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">Function interpretation handle.</param>
    /// <remarks>
    /// Prevents function interpretation from being garbage collected by Z3.
    /// Function interpretations define how uninterpreted functions behave in models.
    /// Must be balanced with FuncInterpDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncInterpIncRef(IntPtr ctx, IntPtr funcInterp)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpIncRefDelegate>(funcPtr);
        func(ctx, funcInterp);
    }

    /// <summary>
    /// Decrements reference count for function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">Function interpretation handle.</param>
    /// <remarks>
    /// When reference count reaches zero, function interpretation is freed by Z3.
    /// Must be balanced with FuncInterpIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncInterpDecRef(IntPtr ctx, IntPtr funcInterp)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpDecRefDelegate>(funcPtr);
        func(ctx, funcInterp);
    }

    // Pattern
    /// <summary>
    /// Increments reference count for pattern.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">Pattern handle.</param>
    /// <remarks>
    /// Prevents pattern from being garbage collected by Z3.
    /// Patterns guide quantifier instantiation in SMT solving.
    /// Must be balanced with PatternDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void PatternIncRef(IntPtr ctx, IntPtr pattern)
    {
        var funcPtr = GetFunctionPointer("Z3_pattern_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<PatternIncRefDelegate>(funcPtr);
        func(ctx, pattern);
    }

    /// <summary>
    /// Decrements reference count for pattern.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">Pattern handle.</param>
    /// <remarks>
    /// When reference count reaches zero, pattern is freed by Z3.
    /// Must be balanced with PatternIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void PatternDecRef(IntPtr ctx, IntPtr pattern)
    {
        var funcPtr = GetFunctionPointer("Z3_pattern_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<PatternDecRefDelegate>(funcPtr);
        func(ctx, pattern);
    }

    // Fixedpoint
    /// <summary>
    /// Increments reference count for fixedpoint solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="fixedpoint">Fixedpoint solver handle.</param>
    /// <remarks>
    /// Prevents fixedpoint solver from being garbage collected by Z3.
    /// Fixedpoint solvers are used for Datalog queries and Horn clause solving.
    /// Must be balanced with FixedpointDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FixedpointIncRef(IntPtr ctx, IntPtr fixedpoint)
    {
        var funcPtr = GetFunctionPointer("Z3_fixedpoint_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FixedpointIncRefDelegate>(funcPtr);
        func(ctx, fixedpoint);
    }

    /// <summary>
    /// Decrements reference count for fixedpoint solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="fixedpoint">Fixedpoint solver handle.</param>
    /// <remarks>
    /// When reference count reaches zero, fixedpoint solver is freed by Z3.
    /// Must be balanced with FixedpointIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FixedpointDecRef(IntPtr ctx, IntPtr fixedpoint)
    {
        var funcPtr = GetFunctionPointer("Z3_fixedpoint_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<FixedpointDecRefDelegate>(funcPtr);
        func(ctx, fixedpoint);
    }
}
