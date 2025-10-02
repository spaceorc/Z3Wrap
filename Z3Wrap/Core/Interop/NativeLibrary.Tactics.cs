using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsTactics(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
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
    }

    // Delegates
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
}
