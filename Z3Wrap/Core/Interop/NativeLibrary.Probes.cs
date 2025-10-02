using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsProbes(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_probe");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_const");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_lt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_gt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_le");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_ge");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_eq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_and");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_or");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_not");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_get_descr");
        LoadFunctionOrNull(handle, functionPointers, "Z3_probe_apply");
    }

    // Delegates
    private delegate IntPtr MkProbeDelegate(IntPtr ctx, IntPtr name);
    private delegate void ProbeIncRefDelegate(IntPtr ctx, IntPtr probe);
    private delegate void ProbeDecRefDelegate(IntPtr ctx, IntPtr probe);
    private delegate IntPtr ProbeConstDelegate(IntPtr ctx, double value);
    private delegate IntPtr ProbeLtDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeGtDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeLeDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeGeDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeEqDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeAndDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeOrDelegate(IntPtr ctx, IntPtr probe1, IntPtr probe2);
    private delegate IntPtr ProbeNotDelegate(IntPtr ctx, IntPtr probe);
    private delegate IntPtr ProbeGetDescrDelegate(IntPtr ctx, IntPtr name);
    private delegate double ProbeApplyDelegate(IntPtr ctx, IntPtr probe, IntPtr goal);

    // Methods
    /// <summary>
    /// Creates a probe by name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the probe.</param>
    /// <returns>Probe handle.</returns>
    /// <remarks>
    /// Creates a probe object given its name. Probes are used to inspect goals and
    /// guide tactic selection. Common probes include "is-qfbv", "arith-max-deg", "size".
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkProbe(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_probe");
        var func = Marshal.GetDelegateForFunctionPointer<MkProbeDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Increments the reference counter of probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">The probe handle.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Increment the reference count
    /// to prevent premature deallocation. Must be paired with ProbeDecRef.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ProbeIncRef(IntPtr ctx, IntPtr probe)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeIncRefDelegate>(funcPtr);
        func(ctx, probe);
    }

    /// <summary>
    /// Decrements the reference counter of probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">The probe handle.</param>
    /// <remarks>
    /// Must be paired with ProbeIncRef. When reference count reaches zero,
    /// the probe may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ProbeDecRef(IntPtr ctx, IntPtr probe)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeDecRefDelegate>(funcPtr);
        func(ctx, probe);
    }

    /// <summary>
    /// Creates a constant probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="value">Constant value.</param>
    /// <returns>Constant probe handle.</returns>
    /// <remarks>
    /// Returns a probe that always returns the given constant value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeConst(IntPtr ctx, double value)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_const");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeConstDelegate>(funcPtr);
        return func(ctx, value);
    }

    /// <summary>
    /// Creates less-than comparison probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that returns true if probe1 &lt; probe2.</returns>
    /// <remarks>
    /// Returns a probe that returns true (1.0) if probe1 is less than probe2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeLt(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_lt");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeLtDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates greater-than comparison probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that returns true if probe1 &gt; probe2.</returns>
    /// <remarks>
    /// Returns a probe that returns true (1.0) if probe1 is greater than probe2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeGt(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_gt");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeGtDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates less-than-or-equal comparison probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that returns true if probe1 &lt;= probe2.</returns>
    /// <remarks>
    /// Returns a probe that returns true (1.0) if probe1 is less than or equal to probe2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeLe(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_le");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeLeDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates greater-than-or-equal comparison probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that returns true if probe1 &gt;= probe2.</returns>
    /// <remarks>
    /// Returns a probe that returns true (1.0) if probe1 is greater than or equal to probe2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeGe(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_ge");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeGeDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates equality comparison probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that returns true if probe1 == probe2.</returns>
    /// <remarks>
    /// Returns a probe that returns true (1.0) if probe1 equals probe2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeEq(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_eq");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeEqDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates conjunction probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that is logical AND of probe1 and probe2.</returns>
    /// <remarks>
    /// Returns a probe that evaluates to true (1.0) if both probe1 and probe2
    /// evaluate to true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeAnd(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_and");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeAndDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates disjunction probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe1">First probe.</param>
    /// <param name="probe2">Second probe.</param>
    /// <returns>Probe that is logical OR of probe1 and probe2.</returns>
    /// <remarks>
    /// Returns a probe that evaluates to true (1.0) if either probe1 or probe2
    /// evaluates to true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeOr(IntPtr ctx, IntPtr probe1, IntPtr probe2)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_or");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeOrDelegate>(funcPtr);
        return func(ctx, probe1, probe2);
    }

    /// <summary>
    /// Creates negation probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">The probe to negate.</param>
    /// <returns>Probe that is logical NOT of probe.</returns>
    /// <remarks>
    /// Returns a probe that evaluates to true (1.0) if the input probe evaluates to false.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeNot(IntPtr ctx, IntPtr probe)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_not");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeNotDelegate>(funcPtr);
        return func(ctx, probe);
    }

    /// <summary>
    /// Retrieves description string for named probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the probe.</param>
    /// <returns>Description string for the probe.</returns>
    /// <remarks>
    /// Returns a string describing what the named probe measures or checks.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ProbeGetDescr(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_get_descr");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeGetDescrDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Applies probe to goal and returns result value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">The probe to apply.</param>
    /// <param name="goal">The goal to inspect.</param>
    /// <returns>Numeric result value from probe evaluation.</returns>
    /// <remarks>
    /// Execute the probe on the given goal and return the result. The result is
    /// interpreted as a Boolean for decision purposes (non-zero = true).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal double ProbeApply(IntPtr ctx, IntPtr probe, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_probe_apply");
        var func = Marshal.GetDelegateForFunctionPointer<ProbeApplyDelegate>(funcPtr);
        return func(ctx, probe, goal);
    }
}
