// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Tactics, Simplifiers, and Probes API - P/Invoke bindings for Z3 tactic-based solving
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Tactics, Simplifiers, and Probes API (55 functions):
//
// Tactics (24 functions):
// - Tactic creation and reference counting
// - Tactic combinators (sequential, parallel, conditional composition)
// - Control flow tactics (repeat, timeout, conditional execution)
// - Trivial tactics (skip, fail) and failure conditions
// - Introspection (help, descriptions, parameters)
// - Application to goals
// - Enumeration (get_num_tactics, get_tactic_name)
//
// Probes (16 functions):
// - Probe creation (Z3_mk_probe, Z3_probe_const)
// - Reference counting (Z3_probe_inc_ref, Z3_probe_dec_ref)
// - Comparison probes (Z3_probe_lt, Z3_probe_gt, Z3_probe_le, Z3_probe_ge, Z3_probe_eq)
// - Logical probes (Z3_probe_and, Z3_probe_or, Z3_probe_not)
// - Probe utilities (Z3_probe_get_descr, Z3_probe_apply)
// - Enumeration (get_num_probes, get_probe_name)
//
// Simplifiers (10 functions):
// - Simplifier creation and management (Z3_mk_simplifier)
// - Reference counting (inc_ref/dec_ref)
// - Parameter configuration (using_params, get_param_descrs)
// - Simplifier composition (and_then for sequential application)
// - Documentation queries (get_help, get_descr)
// - Enumeration (get_num_simplifiers, get_simplifier_name)
// - Solver integration (solver_add_simplifier)
//
// Apply Results (5 functions):
// - Reference counting for tactic application results (Z3_apply_result_inc_ref, Z3_apply_result_dec_ref)
// - Result queries (get_num_subgoals, get_subgoal, to_string)
//
// Probes are functions/predicates used to inspect goals and collect information
// that may be used to decide which solver and/or preprocessing step will be used.
// They always return double values (0.0 = false, non-zero = true for Boolean probes).
//
// Simplifiers are basic building blocks for creating custom pre-processing
// simplifiers for specific problem domains. They can be combined and configured
// with parameters before being applied to formulas.
//
// Missing Functions (0 functions):

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsTactics(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Tactics
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_tactic");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_and_then");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_or_else");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_par_or");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_par_and_then");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_try_for");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_when");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_cond");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_repeat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_skip");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_fail");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_fail_if");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_fail_if_not_decided");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_using_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_get_param_descrs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_get_descr");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_apply");
        LoadFunctionOrNull(handle, functionPointers, "Z3_tactic_apply_ex");

        // Probes
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

        // Simplifiers
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_simplifier");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_param_descrs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_get_descr");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_using_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_simplifier_and_then");

        // Apply Result
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_get_num_subgoals");
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_get_subgoal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_apply_result_to_string");

        // Enumeration
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_num_tactics");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_tactic_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_num_probes");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_probe_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_num_simplifiers");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_simplifier_name");

        // Solver integration
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_add_simplifier");
    }

    // Delegates - Tactics
    private delegate IntPtr MkTacticDelegate(IntPtr ctx, IntPtr name);
    private delegate void TacticIncRefDelegate(IntPtr ctx, IntPtr tactic);
    private delegate void TacticDecRefDelegate(IntPtr ctx, IntPtr tactic);
    private delegate IntPtr TacticAndThenDelegate(IntPtr ctx, IntPtr tactic1, IntPtr tactic2);
    private delegate IntPtr TacticOrElseDelegate(IntPtr ctx, IntPtr tactic1, IntPtr tactic2);
    private delegate IntPtr TacticParOrDelegate(IntPtr ctx, uint numTactics, IntPtr[] tactics);
    private delegate IntPtr TacticParAndThenDelegate(IntPtr ctx, IntPtr tactic1, IntPtr tactic2);
    private delegate IntPtr TacticTryForDelegate(IntPtr ctx, IntPtr tactic, uint milliseconds);
    private delegate IntPtr TacticWhenDelegate(IntPtr ctx, IntPtr probe, IntPtr tactic);
    private delegate IntPtr TacticCondDelegate(IntPtr ctx, IntPtr probe, IntPtr tacticThen, IntPtr tacticElse);
    private delegate IntPtr TacticRepeatDelegate(IntPtr ctx, IntPtr tactic, uint max);
    private delegate IntPtr TacticSkipDelegate(IntPtr ctx);
    private delegate IntPtr TacticFailDelegate(IntPtr ctx);
    private delegate IntPtr TacticFailIfDelegate(IntPtr ctx, IntPtr probe);
    private delegate IntPtr TacticFailIfNotDecidedDelegate(IntPtr ctx);
    private delegate IntPtr TacticUsingParamsDelegate(IntPtr ctx, IntPtr tactic, IntPtr paramsHandle);
    private delegate IntPtr TacticGetHelpDelegate(IntPtr ctx, IntPtr tactic);
    private delegate IntPtr TacticGetParamDescrsDelegate(IntPtr ctx, IntPtr tactic);
    private delegate IntPtr TacticGetDescrDelegate(IntPtr ctx, IntPtr name);
    private delegate IntPtr TacticApplyDelegate(IntPtr ctx, IntPtr tactic, IntPtr goal);
    private delegate IntPtr TacticApplyExDelegate(IntPtr ctx, IntPtr tactic, IntPtr goal, IntPtr paramsHandle);

    // Delegates - Probes
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

    // Delegates - Simplifiers
    private delegate IntPtr MkSimplifierDelegate(IntPtr ctx, IntPtr name);
    private delegate void SimplifierIncRefDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate void SimplifierDecRefDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetHelpDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetParamDescrsDelegate(IntPtr ctx, IntPtr simplifier);
    private delegate IntPtr SimplifierGetDescrDelegate(IntPtr ctx, IntPtr name);
    private delegate IntPtr SimplifierUsingParamsDelegate(IntPtr ctx, IntPtr simplifier, IntPtr paramsHandle);
    private delegate IntPtr SimplifierAndThenDelegate(IntPtr ctx, IntPtr simplifier1, IntPtr simplifier2);

    // Delegates - Apply Result
    private delegate void ApplyResultIncRefDelegate(IntPtr ctx, IntPtr applyResult);
    private delegate void ApplyResultDecRefDelegate(IntPtr ctx, IntPtr applyResult);
    private delegate uint ApplyResultGetNumSubgoalsDelegate(IntPtr ctx, IntPtr applyResult);
    private delegate IntPtr ApplyResultGetSubgoalDelegate(IntPtr ctx, IntPtr applyResult, uint index);
    private delegate IntPtr ApplyResultToStringDelegate(IntPtr ctx, IntPtr applyResult);

    // Delegates - Enumeration
    private delegate uint GetNumTacticsDelegate(IntPtr ctx);
    private delegate IntPtr GetTacticNameDelegate(IntPtr ctx, uint index);
    private delegate uint GetNumProbesDelegate(IntPtr ctx);
    private delegate IntPtr GetProbeNameDelegate(IntPtr ctx, uint index);
    private delegate uint GetNumSimplifiersDelegate(IntPtr ctx);
    private delegate IntPtr GetSimplifierNameDelegate(IntPtr ctx, uint index);

    // Delegates - Solver Integration
    private delegate void SolverAddSimplifierDelegate(IntPtr ctx, IntPtr solver, IntPtr simplifier);

    // Methods
    /// <summary>
    /// Creates a tactic by name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the tactic (e.g., "simplify", "solve-eqs").</param>
    /// <returns>Tactic handle.</returns>
    /// <remarks>
    /// Creates a tactic object given its name. Use Z3_get_tactic_names to get the list
    /// of available tactics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTactic(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_tactic");
        var func = Marshal.GetDelegateForFunctionPointer<MkTacticDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Increments the reference counter of tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic handle.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Increment the reference count
    /// to prevent premature deallocation. Must be paired with TacticDecRef.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void TacticIncRef(IntPtr ctx, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<TacticIncRefDelegate>(funcPtr);
        func(ctx, tactic);
    }

    /// <summary>
    /// Decrements the reference counter of tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic handle.</param>
    /// <remarks>
    /// Must be paired with TacticIncRef. When reference count reaches zero,
    /// the tactic may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void TacticDecRef(IntPtr ctx, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<TacticDecRefDelegate>(funcPtr);
        func(ctx, tactic);
    }

    /// <summary>
    /// Creates sequential composition of two tactics.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic1">First tactic to apply.</param>
    /// <param name="tactic2">Second tactic to apply.</param>
    /// <returns>Composite tactic that applies tactic1 then tactic2.</returns>
    /// <remarks>
    /// Returns a tactic that applies tactic1 to a given goal, and then applies tactic2
    /// to every subgoal produced by tactic1.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticAndThen(IntPtr ctx, IntPtr tactic1, IntPtr tactic2)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_and_then");
        var func = Marshal.GetDelegateForFunctionPointer<TacticAndThenDelegate>(funcPtr);
        return func(ctx, tactic1, tactic2);
    }

    /// <summary>
    /// Creates alternative composition of two tactics.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic1">First tactic to try.</param>
    /// <param name="tactic2">Alternative tactic if first fails.</param>
    /// <returns>Composite tactic that tries tactic1, falling back to tactic2.</returns>
    /// <remarks>
    /// Returns a tactic that first applies tactic1 to a given goal. If it fails,
    /// then returns the result of applying tactic2 to the original goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticOrElse(IntPtr ctx, IntPtr tactic1, IntPtr tactic2)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_or_else");
        var func = Marshal.GetDelegateForFunctionPointer<TacticOrElseDelegate>(funcPtr);
        return func(ctx, tactic1, tactic2);
    }

    /// <summary>
    /// Creates parallel disjunction of tactics.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numTactics">Number of tactics.</param>
    /// <param name="tactics">Array of tactic handles.</param>
    /// <returns>Composite tactic that tries all tactics in parallel.</returns>
    /// <remarks>
    /// Returns a tactic that applies all given tactics to a goal in parallel.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticParOr(IntPtr ctx, uint numTactics, IntPtr[] tactics)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_par_or");
        var func = Marshal.GetDelegateForFunctionPointer<TacticParOrDelegate>(funcPtr);
        return func(ctx, numTactics, tactics);
    }

    /// <summary>
    /// Creates parallel sequential composition of two tactics.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic1">First tactic.</param>
    /// <param name="tactic2">Second tactic.</param>
    /// <returns>Composite tactic for parallel and-then.</returns>
    /// <remarks>
    /// Returns a tactic that applies tactic1 to a goal and then tactic2 to every
    /// subgoal produced by tactic1. Subgoals are processed in parallel.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticParAndThen(IntPtr ctx, IntPtr tactic1, IntPtr tactic2)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_par_and_then");
        var func = Marshal.GetDelegateForFunctionPointer<TacticParAndThenDelegate>(funcPtr);
        return func(ctx, tactic1, tactic2);
    }

    /// <summary>
    /// Creates tactic with timeout.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic to wrap with timeout.</param>
    /// <param name="milliseconds">Timeout in milliseconds.</param>
    /// <returns>Tactic that fails if execution exceeds timeout.</returns>
    /// <remarks>
    /// Returns a tactic that applies the given tactic to a goal, but fails if
    /// the tactic takes more than the given timeout.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticTryFor(IntPtr ctx, IntPtr tactic, uint milliseconds)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_try_for");
        var func = Marshal.GetDelegateForFunctionPointer<TacticTryForDelegate>(funcPtr);
        return func(ctx, tactic, milliseconds);
    }

    /// <summary>
    /// Creates conditional tactic based on probe.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">Probe to test goal condition.</param>
    /// <param name="tactic">Tactic to apply if probe succeeds.</param>
    /// <returns>Conditional tactic.</returns>
    /// <remarks>
    /// Returns a tactic that applies the given tactic to a goal if the probe
    /// returns true. Otherwise, it returns the input goal unmodified.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticWhen(IntPtr ctx, IntPtr probe, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_when");
        var func = Marshal.GetDelegateForFunctionPointer<TacticWhenDelegate>(funcPtr);
        return func(ctx, probe, tactic);
    }

    /// <summary>
    /// Creates if-then-else conditional tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">Probe to test goal condition.</param>
    /// <param name="tacticThen">Tactic if probe succeeds.</param>
    /// <param name="tacticElse">Tactic if probe fails.</param>
    /// <returns>Conditional tactic with else branch.</returns>
    /// <remarks>
    /// Returns a tactic that applies tacticThen if the probe returns true,
    /// otherwise applies tacticElse.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticCond(IntPtr ctx, IntPtr probe, IntPtr tacticThen, IntPtr tacticElse)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_cond");
        var func = Marshal.GetDelegateForFunctionPointer<TacticCondDelegate>(funcPtr);
        return func(ctx, probe, tacticThen, tacticElse);
    }

    /// <summary>
    /// Creates tactic that repeats until fixpoint or max iterations.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic to repeat.</param>
    /// <param name="max">Maximum number of iterations.</param>
    /// <returns>Repeating tactic.</returns>
    /// <remarks>
    /// Returns a tactic that keeps applying the given tactic until no subgoal is
    /// modified by it, or the maximum number of iterations is reached.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticRepeat(IntPtr ctx, IntPtr tactic, uint max)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_repeat");
        var func = Marshal.GetDelegateForFunctionPointer<TacticRepeatDelegate>(funcPtr);
        return func(ctx, tactic, max);
    }

    /// <summary>
    /// Creates no-op tactic that always succeeds.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Skip tactic handle.</returns>
    /// <remarks>
    /// Returns a tactic that just returns the given goal without modifications.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticSkip(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_skip");
        var func = Marshal.GetDelegateForFunctionPointer<TacticSkipDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates tactic that always fails.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Fail tactic handle.</returns>
    /// <remarks>
    /// Returns a tactic that always fails. Useful for testing and debugging.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticFail(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_fail");
        var func = Marshal.GetDelegateForFunctionPointer<TacticFailDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates tactic that fails when probe succeeds.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="probe">Probe to test goal condition.</param>
    /// <returns>Conditional fail tactic.</returns>
    /// <remarks>
    /// Returns a tactic that fails if the probe returns true. Otherwise, it returns
    /// the input goal unmodified.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticFailIf(IntPtr ctx, IntPtr probe)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_fail_if");
        var func = Marshal.GetDelegateForFunctionPointer<TacticFailIfDelegate>(funcPtr);
        return func(ctx, probe);
    }

    /// <summary>
    /// Creates tactic that fails if goal is not decided.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Fail-if-not-decided tactic handle.</returns>
    /// <remarks>
    /// Returns a tactic that fails if the goal is not definitely SAT or definitely UNSAT.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticFailIfNotDecided(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_fail_if_not_decided");
        var func = Marshal.GetDelegateForFunctionPointer<TacticFailIfNotDecidedDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates tactic configured with parameters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic to configure.</param>
    /// <param name="paramsHandle">Parameter set handle.</param>
    /// <returns>Parameterized tactic handle.</returns>
    /// <remarks>
    /// Returns a tactic that is a copy of the given tactic, but uses the given
    /// parameter set.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticUsingParams(IntPtr ctx, IntPtr tactic, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_using_params");
        var func = Marshal.GetDelegateForFunctionPointer<TacticUsingParamsDelegate>(funcPtr);
        return func(ctx, tactic, paramsHandle);
    }

    /// <summary>
    /// Retrieves help string for tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic handle.</param>
    /// <returns>Help string describing the tactic.</returns>
    /// <remarks>
    /// Returns a string describing the tactic and its parameters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticGetHelp(IntPtr ctx, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_get_help");
        var func = Marshal.GetDelegateForFunctionPointer<TacticGetHelpDelegate>(funcPtr);
        return func(ctx, tactic);
    }

    /// <summary>
    /// Retrieves parameter descriptors for tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic handle.</param>
    /// <returns>Parameter descriptors object handle.</returns>
    /// <remarks>
    /// Returns a parameter descriptor set that describes all available parameters
    /// for the given tactic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticGetParamDescrs(IntPtr ctx, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_get_param_descrs");
        var func = Marshal.GetDelegateForFunctionPointer<TacticGetParamDescrsDelegate>(funcPtr);
        return func(ctx, tactic);
    }

    /// <summary>
    /// Retrieves description string for named tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Name of the tactic.</param>
    /// <returns>Description string for the tactic.</returns>
    /// <remarks>
    /// Returns a string describing what the named tactic does.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticGetDescr(IntPtr ctx, IntPtr name)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_get_descr");
        var func = Marshal.GetDelegateForFunctionPointer<TacticGetDescrDelegate>(funcPtr);
        return func(ctx, name);
    }

    /// <summary>
    /// Applies tactic to goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic to apply.</param>
    /// <param name="goal">The goal to transform.</param>
    /// <returns>Apply result handle containing resulting goals.</returns>
    /// <remarks>
    /// Applies the tactic to the given goal and returns an apply result object
    /// containing the resulting subgoals.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticApply(IntPtr ctx, IntPtr tactic, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_apply");
        var func = Marshal.GetDelegateForFunctionPointer<TacticApplyDelegate>(funcPtr);
        return func(ctx, tactic, goal);
    }

    /// <summary>
    /// Applies tactic to goal with parameters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic to apply.</param>
    /// <param name="goal">The goal to transform.</param>
    /// <param name="paramsHandle">Parameter set handle.</param>
    /// <returns>Apply result handle containing resulting goals.</returns>
    /// <remarks>
    /// Applies the tactic to the given goal using the given parameters, and returns
    /// an apply result object containing the resulting subgoals.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr TacticApplyEx(IntPtr ctx, IntPtr tactic, IntPtr goal, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_tactic_apply_ex");
        var func = Marshal.GetDelegateForFunctionPointer<TacticApplyExDelegate>(funcPtr);
        return func(ctx, tactic, goal, paramsHandle);
    }

    // Probe Methods
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

    // Simplifier Methods
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

    // Apply Result Methods
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

    /// <summary>
    /// Gets number of subgoals in apply result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="applyResult">Apply result handle.</param>
    /// <returns>Number of subgoals produced by tactic application.</returns>
    /// <remarks>
    /// Returns the number of subgoals resulting from applying a tactic to a goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ApplyResultGetNumSubgoals(IntPtr ctx, IntPtr applyResult)
    {
        var funcPtr = GetFunctionPointer("Z3_apply_result_get_num_subgoals");
        var func = Marshal.GetDelegateForFunctionPointer<ApplyResultGetNumSubgoalsDelegate>(funcPtr);
        return func(ctx, applyResult);
    }

    /// <summary>
    /// Gets subgoal from apply result by index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="applyResult">Apply result handle.</param>
    /// <param name="index">Index of the subgoal (0-based).</param>
    /// <returns>Goal handle for the specified subgoal.</returns>
    /// <remarks>
    /// Returns the i-th subgoal in the apply result. Index must be less than
    /// the number returned by ApplyResultGetNumSubgoals.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ApplyResultGetSubgoal(IntPtr ctx, IntPtr applyResult, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_apply_result_get_subgoal");
        var func = Marshal.GetDelegateForFunctionPointer<ApplyResultGetSubgoalDelegate>(funcPtr);
        return func(ctx, applyResult, index);
    }

    /// <summary>
    /// Converts apply result to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="applyResult">Apply result handle.</param>
    /// <returns>String representation of the apply result.</returns>
    /// <remarks>
    /// Returns a string describing the apply result and its subgoals.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ApplyResultToString(IntPtr ctx, IntPtr applyResult)
    {
        var funcPtr = GetFunctionPointer("Z3_apply_result_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<ApplyResultToStringDelegate>(funcPtr);
        return func(ctx, applyResult);
    }

    // Enumeration Methods
    /// <summary>
    /// Gets number of built-in tactics.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Number of available tactics.</returns>
    /// <remarks>
    /// Returns the number of built-in tactics available in Z3.
    /// Use with GetTacticName to enumerate all tactics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetNumTactics(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_num_tactics");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumTacticsDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Gets name of tactic by index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="index">Index of the tactic (0-based).</param>
    /// <returns>String name of the tactic.</returns>
    /// <remarks>
    /// Returns the name of the i-th tactic. Index must be less than
    /// the number returned by GetNumTactics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetTacticName(IntPtr ctx, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_get_tactic_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetTacticNameDelegate>(funcPtr);
        return func(ctx, index);
    }

    /// <summary>
    /// Gets number of built-in probes.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Number of available probes.</returns>
    /// <remarks>
    /// Returns the number of built-in probes available in Z3.
    /// Use with GetProbeName to enumerate all probes.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetNumProbes(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_num_probes");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumProbesDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Gets name of probe by index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="index">Index of the probe (0-based).</param>
    /// <returns>String name of the probe.</returns>
    /// <remarks>
    /// Returns the name of the i-th probe. Index must be less than
    /// the number returned by GetNumProbes.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetProbeName(IntPtr ctx, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_get_probe_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetProbeNameDelegate>(funcPtr);
        return func(ctx, index);
    }

    /// <summary>
    /// Gets number of built-in simplifiers.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Number of available simplifiers.</returns>
    /// <remarks>
    /// Returns the number of built-in simplifiers available in Z3.
    /// Use with GetSimplifierName to enumerate all simplifiers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetNumSimplifiers(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_num_simplifiers");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumSimplifiersDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Gets name of simplifier by index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="index">Index of the simplifier (0-based).</param>
    /// <returns>String name of the simplifier.</returns>
    /// <remarks>
    /// Returns the name of the i-th simplifier. Index must be less than
    /// the number returned by GetNumSimplifiers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetSimplifierName(IntPtr ctx, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_get_simplifier_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetSimplifierNameDelegate>(funcPtr);
        return func(ctx, index);
    }

    /// <summary>
    /// Adds simplifier to solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="simplifier">The simplifier to add.</param>
    /// <remarks>
    /// Adds a simplifier to be applied to formulas before the solver processes them.
    /// Simplifiers are applied in the order they are added.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverAddSimplifier(IntPtr ctx, IntPtr solver, IntPtr simplifier)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_add_simplifier");
        var func = Marshal.GetDelegateForFunctionPointer<SolverAddSimplifierDelegate>(funcPtr);
        func(ctx, solver, simplifier);
    }
}
