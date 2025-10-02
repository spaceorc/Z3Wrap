using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsStatistics(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_get_key");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_get_uint_value");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_get_double_value");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_is_uint");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_is_double");
        LoadFunctionOrNull(handle, functionPointers, "Z3_stats_to_string");
    }

    // Delegates

    private delegate uint StatsSizeDelegate(IntPtr ctx, IntPtr stats);
    private delegate IntPtr StatsGetKeyDelegate(IntPtr ctx, IntPtr stats, uint idx);
    private delegate uint StatsGetUintValueDelegate(IntPtr ctx, IntPtr stats, uint idx);
    private delegate double StatsGetDoubleValueDelegate(IntPtr ctx, IntPtr stats, uint idx);
    private delegate bool StatsIsUintDelegate(IntPtr ctx, IntPtr stats, uint idx);
    private delegate bool StatsIsDoubleDelegate(IntPtr ctx, IntPtr stats, uint idx);
    private delegate IntPtr StatsToStringDelegate(IntPtr ctx, IntPtr stats);

    // Methods

    /// <summary>
    /// Gets number of statistical entries.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <returns>Number of entries in statistics object.</returns>
    /// <remarks>
    /// Statistics contain performance metrics from solver runs including
    /// decisions, conflicts, propagations, and other counters. Use this to
    /// determine array size for iterating through all statistics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint StatsSize(IntPtr ctx, IntPtr stats)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_size");
        var func = Marshal.GetDelegateForFunctionPointer<StatsSizeDelegate>(funcPtr);
        return func(ctx, stats);
    }

    /// <summary>
    /// Gets key name of statistic entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <param name="idx">Index of entry (0 to StatsSize-1).</param>
    /// <returns>String containing statistic key name.</returns>
    /// <remarks>
    /// Returns human-readable name of statistic (e.g., "decisions", "conflicts").
    /// Use with StatsGetUintValue or StatsGetDoubleValue to retrieve value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr StatsGetKey(IntPtr ctx, IntPtr stats, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_get_key");
        var func = Marshal.GetDelegateForFunctionPointer<StatsGetKeyDelegate>(funcPtr);
        return func(ctx, stats, idx);
    }

    /// <summary>
    /// Gets unsigned integer value of statistic entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <param name="idx">Index of entry (0 to StatsSize-1).</param>
    /// <returns>Unsigned integer value.</returns>
    /// <remarks>
    /// Only valid if StatsIsUint returns true for this index. Most counters
    /// (decisions, conflicts, propagations) are unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint StatsGetUintValue(IntPtr ctx, IntPtr stats, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_get_uint_value");
        var func = Marshal.GetDelegateForFunctionPointer<StatsGetUintValueDelegate>(funcPtr);
        return func(ctx, stats, idx);
    }

    /// <summary>
    /// Gets double-precision value of statistic entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <param name="idx">Index of entry (0 to StatsSize-1).</param>
    /// <returns>Double-precision floating-point value.</returns>
    /// <remarks>
    /// Only valid if StatsIsDouble returns true for this index. Timing and
    /// percentage statistics typically use double values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal double StatsGetDoubleValue(IntPtr ctx, IntPtr stats, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_get_double_value");
        var func = Marshal.GetDelegateForFunctionPointer<StatsGetDoubleValueDelegate>(funcPtr);
        return func(ctx, stats, idx);
    }

    /// <summary>
    /// Checks if statistic entry is unsigned integer type.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <param name="idx">Index of entry (0 to StatsSize-1).</param>
    /// <returns>True if entry is unsigned integer, false otherwise.</returns>
    /// <remarks>
    /// Use before calling StatsGetUintValue to ensure correct type.
    /// Statistics have either uint or double type.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool StatsIsUint(IntPtr ctx, IntPtr stats, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_is_uint");
        var func = Marshal.GetDelegateForFunctionPointer<StatsIsUintDelegate>(funcPtr);
        return func(ctx, stats, idx);
    }

    /// <summary>
    /// Checks if statistic entry is double type.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <param name="idx">Index of entry (0 to StatsSize-1).</param>
    /// <returns>True if entry is double, false otherwise.</returns>
    /// <remarks>
    /// Use before calling StatsGetDoubleValue to ensure correct type.
    /// Statistics have either uint or double type.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool StatsIsDouble(IntPtr ctx, IntPtr stats, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_is_double");
        var func = Marshal.GetDelegateForFunctionPointer<StatsIsDoubleDelegate>(funcPtr);
        return func(ctx, stats, idx);
    }

    /// <summary>
    /// Converts statistics to human-readable string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="stats">The statistics handle.</param>
    /// <returns>String representation of statistics.</returns>
    /// <remarks>
    /// Returns formatted string showing all statistic keys and values.
    /// Useful for debugging and performance analysis.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr StatsToString(IntPtr ctx, IntPtr stats)
    {
        var funcPtr = GetFunctionPointer("Z3_stats_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<StatsToStringDelegate>(funcPtr);
        return func(ctx, stats);
    }
}
