// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Solver API - P/Invoke bindings for Z3 solver operations
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's solver API (56 functions):
// - Solver creation (Z3_mk_solver, Z3_mk_simple_solver, Z3_mk_solver_from_tactic, Z3_mk_solver_for_logic)
// - Reference counting (inc_ref, dec_ref)
// - Assertions and satisfiability checking (assert, check, check_assumptions, solve_for)
// - Backtracking stack operations (push, pop, reset)
// - Model extraction and conversion
// - Parameter configuration
// - Assumption-based checking with unsat core extraction
// - Proof generation and extraction
// - SMT-LIB2 format I/O (file and string parsing)
// - Solver statistics and diagnostics
// - Context translation (moving solvers between contexts)
// - Advanced solving modes (cubes, consequences)
// - Solver control and interruption
// - Congruence closure operations (root, next, explain)
// - Implied equalities computation
// - Solver trail and units inspection (trail, units, non-units, levels)
// - User propagator API (init, register, callbacks for fixed, eq, diseq, created, decide, final, binding)
// - Clause learning callbacks
//
// Missing Functions (0 functions):

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSolvers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_solver");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_simple_solver");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_inc_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_dec_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_assert");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_check");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_get_model");

        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_solver_from_tactic");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_solver_for_logic");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_push");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_pop");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_reset");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_reason_unknown");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_set_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_assert_and_track");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_check_assumptions");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_assertions");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_unsat_core");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_proof");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_statistics");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_from_file");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_from_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_param_descrs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_num_scopes");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_translate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_to_dimacs_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_cube");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_consequences");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_interrupt");

        LoadFunctionOrNull(handle, functionPointers, "Z3_get_implied_equalities");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_congruence_explain");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_congruence_next");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_congruence_root");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_levels");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_non_units");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_trail");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_units");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_import_model_converter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_next_split");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_consequence");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_created");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_decide");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_declare");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_diseq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_eq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_final");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_fixed");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_init");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_on_binding");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_register");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_propagate_register_cb");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_register_on_clause");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_set_initial_value");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_solve_for");
    }

    // Delegates
    private delegate IntPtr MkSolverDelegate(IntPtr ctx);
    private delegate IntPtr MkSimpleSolverDelegate(IntPtr ctx);
    private delegate IntPtr MkSolverFromTacticDelegate(IntPtr ctx, IntPtr tactic);
    private delegate IntPtr MkSolverForLogicDelegate(IntPtr ctx, IntPtr logic);
    private delegate void SolverIncRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverDecRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverAssertDelegate(IntPtr ctx, IntPtr solver, IntPtr formula);
    private delegate int SolverCheckDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverPushDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverPopDelegate(IntPtr ctx, IntPtr solver, uint numScopes);
    private delegate void SolverResetDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetModelDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetReasonUnknownDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverSetParamsDelegate(IntPtr ctx, IntPtr solver, IntPtr paramsHandle);
    private delegate void SolverAssertAndTrackDelegate(IntPtr ctx, IntPtr solver, IntPtr assertion, IntPtr tracker);
    private delegate int SolverCheckAssumptionsDelegate(
        IntPtr ctx,
        IntPtr solver,
        uint numAssumptions,
        IntPtr[] assumptions
    );
    private delegate IntPtr SolverGetAssertionsDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetUnsatCoreDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetProofDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetStatisticsDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverFromFileDelegate(IntPtr ctx, IntPtr solver, IntPtr fileName);
    private delegate void SolverFromStringDelegate(IntPtr ctx, IntPtr solver, IntPtr str);
    private delegate IntPtr SolverToStringDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetHelpDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetParamDescrsDelegate(IntPtr ctx, IntPtr solver);
    private delegate uint SolverGetNumScopesDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverTranslateDelegate(IntPtr sourceCtx, IntPtr solver, IntPtr targetCtx);
    private delegate IntPtr SolverToDimacsStringDelegate(IntPtr ctx, IntPtr solver, bool includeName);
    private delegate IntPtr SolverCubeDelegate(IntPtr ctx, IntPtr solver, IntPtr vars, uint numVars);
    private delegate int SolverGetConsequencesDelegate(
        IntPtr ctx,
        IntPtr solver,
        IntPtr assumptions,
        IntPtr variables,
        IntPtr consequences
    );
    private delegate void SolverInterruptDelegate(IntPtr ctx, IntPtr solver);

    // Callback delegates for user propagators
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CreatedEhDelegate(IntPtr ctx, IntPtr cb, IntPtr t);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DecideEhDelegate(IntPtr ctx, IntPtr cb, IntPtr t, uint idx, int phase);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EqEhDelegate(IntPtr ctx, IntPtr cb, IntPtr s, IntPtr t);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FinalEhDelegate(IntPtr ctx, IntPtr cb);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FixedEhDelegate(IntPtr ctx, IntPtr cb, IntPtr t, IntPtr value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void PushEhDelegate(IntPtr ctx, IntPtr cb);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void PopEhDelegate(IntPtr ctx, IntPtr cb, uint numScopes);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate IntPtr FreshEhDelegate(IntPtr ctx, IntPtr newContext);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate bool OnBindingEhDelegate(IntPtr ctx, IntPtr cb, IntPtr expr, IntPtr value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void OnClauseEhDelegate(
        IntPtr ctx,
        IntPtr expr,
        uint numLits,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] litIds,
        uint numBounds,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] IntPtr[] bounds
    );

    // New function delegates
    private delegate int GetImpliedEqualitiesDelegate(
        IntPtr ctx,
        IntPtr solver,
        uint numTerms,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] terms,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] classIds
    );

    private delegate IntPtr SolverCongruenceExplainDelegate(IntPtr ctx, IntPtr solver, IntPtr a, IntPtr b);
    private delegate IntPtr SolverCongruenceNextDelegate(IntPtr ctx, IntPtr solver, IntPtr a);
    private delegate IntPtr SolverCongruenceRootDelegate(IntPtr ctx, IntPtr solver, IntPtr a);

    private delegate void SolverGetLevelsDelegate(
        IntPtr ctx,
        IntPtr solver,
        IntPtr literals,
        uint sz,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] levels
    );

    private delegate IntPtr SolverGetNonUnitsDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetTrailDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr SolverGetUnitsDelegate(IntPtr ctx, IntPtr solver);
    private delegate void SolverImportModelConverterDelegate(IntPtr ctx, IntPtr src, IntPtr dst);
    private delegate bool SolverNextSplitDelegate(IntPtr ctx, IntPtr cb, IntPtr t, uint idx, int phase);

    private delegate bool SolverPropagateConsequenceDelegate(
        IntPtr ctx,
        IntPtr cb,
        uint numFixed,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] fixedIds,
        uint numEqs,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] IntPtr[] eqLhs,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] IntPtr[] eqRhs,
        IntPtr conseq
    );

    private delegate void SolverPropagateCreatedDelegate(IntPtr ctx, IntPtr solver, CreatedEhDelegate createdEh);
    private delegate void SolverPropagateDecideDelegate(IntPtr ctx, IntPtr solver, DecideEhDelegate decideEh);

    private delegate IntPtr SolverPropagateDeclareDelegate(
        IntPtr ctx,
        IntPtr name,
        uint n,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] domain,
        IntPtr range
    );

    private delegate void SolverPropagateDiseqDelegate(IntPtr ctx, IntPtr solver, EqEhDelegate diseqEh);
    private delegate void SolverPropagateEqDelegate(IntPtr ctx, IntPtr solver, EqEhDelegate eqEh);
    private delegate void SolverPropagateFinalDelegate(IntPtr ctx, IntPtr solver, FinalEhDelegate finalEh);
    private delegate void SolverPropagateFixedDelegate(IntPtr ctx, IntPtr solver, FixedEhDelegate fixedEh);

    private delegate void SolverPropagateInitDelegate(
        IntPtr ctx,
        IntPtr solver,
        IntPtr userContext,
        PushEhDelegate pushEh,
        PopEhDelegate popEh,
        FreshEhDelegate freshEh
    );

    private delegate void SolverPropagateOnBindingDelegate(IntPtr ctx, IntPtr solver, OnBindingEhDelegate bindingEh);
    private delegate void SolverPropagateRegisterDelegate(IntPtr ctx, IntPtr solver, IntPtr e);
    private delegate void SolverPropagateRegisterCbDelegate(IntPtr ctx, IntPtr cb, IntPtr e);

    private delegate void SolverRegisterOnClauseDelegate(
        IntPtr ctx,
        IntPtr solver,
        IntPtr userContext,
        OnClauseEhDelegate onClauseEh
    );

    private delegate void SolverSetInitialValueDelegate(IntPtr ctx, IntPtr solver, IntPtr constant, IntPtr value);

    private delegate int SolverSolveForDelegate(
        IntPtr ctx,
        IntPtr solver,
        uint numVars,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] vars
    );

    // Methods
    /// <summary>
    /// Creates a Z3 solver instance for satisfiability checking.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Z3 solver.</returns>
    /// <remarks>
    /// The solver must be disposed using reference counting. Use this for general
    /// satisfiability checking with full Z3 capabilities.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver");
        var func = Marshal.GetDelegateForFunctionPointer<MkSolverDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 simple solver instance with reduced functionality for basic satisfiability checking.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Z3 simple solver.</returns>
    /// <remarks>
    /// Simple solvers have fewer features but may be more efficient for basic use cases.
    /// Prefer MkSolver for full functionality.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSimpleSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_simple_solver");
        var func = Marshal.GetDelegateForFunctionPointer<MkSimpleSolverDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates solver from tactic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="tactic">The tactic handle.</param>
    /// <returns>Solver handle created from tactic.</returns>
    /// <remarks>
    /// Creates a solver that applies the given tactic to each check-sat call.
    /// This allows using tactics within the standard solver interface.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSolverFromTactic(IntPtr ctx, IntPtr tactic)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver_from_tactic");
        var func = Marshal.GetDelegateForFunctionPointer<MkSolverFromTacticDelegate>(funcPtr);
        return func(ctx, tactic);
    }

    /// <summary>
    /// Creates solver customized for specific logic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="logic">The logic symbol (e.g., QF_LIA, QF_BV, QF_UFLIA).</param>
    /// <returns>Solver handle optimized for the specified logic.</returns>
    /// <remarks>
    /// Creates a solver that uses specialized tactics optimized for the given logic.
    /// This can provide significant performance improvements over the general solver.
    /// Common logics: QF_LIA (linear integer), QF_BV (bit-vectors), QF_UFLIA (uninterpreted functions).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSolverForLogic(IntPtr ctx, IntPtr logic)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver_for_logic");
        var func = Marshal.GetDelegateForFunctionPointer<MkSolverForLogicDelegate>(funcPtr);
        return func(ctx, logic);
    }

    /// <summary>
    /// Increments the reference counter of the given solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. When you receive a solver object,
    /// increment its reference count to prevent premature deallocation. Must be paired
    /// with SolverDecRef when the solver is no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<SolverIncRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Decrements the reference counter of the given solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle to decrement reference count for.</param>
    /// <remarks>
    /// Must be paired with SolverIncRef to properly manage memory. When reference
    /// count reaches zero, the solver may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<SolverDecRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Asserts a Boolean constraint to the solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="formula">The Boolean formula to assert as a constraint.</param>
    /// <remarks>
    /// The formula must be a Boolean expression. Asserted formulas are added to the
    /// solver's constraint set for satisfiability checking.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverAssert(IntPtr ctx, IntPtr solver, IntPtr formula)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_assert");
        var func = Marshal.GetDelegateForFunctionPointer<SolverAssertDelegate>(funcPtr);
        func(ctx, solver, formula);
    }

    /// <summary>
    /// Checks the satisfiability of the asserted constraints in the solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Satisfiability result: 1 (satisfiable), -1 (unsatisfiable), 0 (unknown).</returns>
    /// <remarks>
    /// Returns Z3_L_TRUE (1) if satisfiable, Z3_L_FALSE (-1) if unsatisfiable,
    /// or Z3_L_UNDEF (0) if the result is unknown (e.g., due to timeout).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_check");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCheckDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Pushes a new scope level onto the solver's assertion stack.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <remarks>
    /// Creates a backtracking point. Assertions added after push can be removed with pop.
    /// Used for incremental solving and backtracking search.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPush(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_push");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPushDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Pops scope levels from the solver's assertion stack, removing recent assertions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="numScopes">The number of scope levels to pop.</param>
    /// <remarks>
    /// Removes assertions added after the corresponding push operations.
    /// Must have at least numScopes push operations to pop from.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_pop");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPopDelegate>(funcPtr);
        func(ctx, solver, numScopes);
    }

    /// <summary>
    /// Resets the solver by removing all asserted constraints and clearing the assertion stack.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <remarks>
    /// Removes all asserted formulas and resets the solver to its initial state.
    /// More efficient than creating a new solver for reuse scenarios.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverReset(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_reset");
        var func = Marshal.GetDelegateForFunctionPointer<SolverResetDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Retrieves a satisfying model from the solver after a successful satisfiability check.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Handle to the model, or null if no model is available.</returns>
    /// <remarks>
    /// Only valid after SolverCheck returns satisfiable (1). The model contains
    /// variable assignments that satisfy all asserted constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_model");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetModelDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves the reason why the solver returned an unknown result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Handle to a string describing the reason for unknown result.</returns>
    /// <remarks>
    /// Only valid after SolverCheck returns unknown (0). Provides information
    /// about why the solver could not determine satisfiability (e.g., timeout, incomplete theory).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_reason_unknown");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetReasonUnknownDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Sets parameters on a solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverSetParams(IntPtr ctx, IntPtr solver, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_set_params");
        var func = Marshal.GetDelegateForFunctionPointer<SolverSetParamsDelegate>(funcPtr);
        func(ctx, solver, paramsHandle);
    }

    /// <summary>
    /// Asserts constraint into solver and tracks it with Boolean literal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="assertion">The constraint to assert.</param>
    /// <param name="tracker">The Boolean constant used for tracking in unsat core.</param>
    /// <remarks>
    /// Asserts a constraint into the solver, and tracks it (in the unsat core) using a Boolean constant.
    /// This API is an alternative to Z3_solver_check_assumptions for extracting unsat cores.
    /// Both APIs can be used in the same solver. The unsat core will contain a combination of
    /// the Boolean variables provided using Z3_solver_assert_and_track and the Boolean literals
    /// provided using Z3_solver_check_assumptions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverAssertAndTrack(IntPtr ctx, IntPtr solver, IntPtr assertion, IntPtr tracker)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_assert_and_track");
        var func = Marshal.GetDelegateForFunctionPointer<SolverAssertAndTrackDelegate>(funcPtr);
        func(ctx, solver, assertion, tracker);
    }

    /// <summary>
    /// Checks satisfiability of assertions in solver with additional assumptions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="numAssumptions">Number of assumption literals.</param>
    /// <param name="assumptions">Array of Boolean literals to assume temporarily.</param>
    /// <returns>Satisfiability result: 1 (satisfiable), -1 (unsatisfiable), 0 (unknown).</returns>
    /// <remarks>
    /// Check whether the assertions in the given solver and optional assumptions are consistent or not.
    /// The function Z3_solver_get_unsat_core retrieves the subset of the assumptions used in the
    /// unsatisfiability proof produced by Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int SolverCheckAssumptions(IntPtr ctx, IntPtr solver, uint numAssumptions, IntPtr[] assumptions)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_check_assumptions");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCheckAssumptionsDelegate>(funcPtr);
        return func(ctx, solver, numAssumptions, assumptions);
    }

    /// <summary>
    /// Retrieves the set of asserted formulas from the solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST vector containing all asserted formulas.</returns>
    /// <remarks>
    /// Returns a Z3_ast_vector containing all assertions that have been asserted to the solver.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetAssertions(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_assertions");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetAssertionsDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves unsatisfiable core from last check.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST vector containing subset of assertions causing unsatisfiability.</returns>
    /// <remarks>
    /// Retrieve the subset of the assumptions used in the unsatisfiability proof produced by
    /// the most recent call to Z3_solver_check_assumptions. Only valid if the previous call
    /// returned Z3_L_FALSE (unsatisfiable).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetUnsatCore(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_unsat_core");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetUnsatCoreDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves proof object from solver after unsatisfiable result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST node representing the proof, or null if not available.</returns>
    /// <remarks>
    /// Retrieve a proof for the last Z3_solver_check or Z3_solver_check_assumptions.
    /// The error handler is invoked if proof generation is not enabled, or if the last call
    /// was not Z3_L_FALSE.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetProof(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_proof");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetProofDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves statistics from solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Statistics object handle.</returns>
    /// <remarks>
    /// Returns statistics for the given solver. Contains information about the solver's
    /// performance, such as number of decisions, conflicts, restarts, etc.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetStatistics(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_statistics");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetStatisticsDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Parses SMT-LIB2 file and loads assertions into solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="fileName">Path to SMT-LIB2 file.</param>
    /// <remarks>
    /// Load solver assertions from an SMT-LIB2 formatted file.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverFromFile(IntPtr ctx, IntPtr solver, IntPtr fileName)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_from_file");
        var func = Marshal.GetDelegateForFunctionPointer<SolverFromFileDelegate>(funcPtr);
        func(ctx, solver, fileName);
    }

    /// <summary>
    /// Parses SMT-LIB2 string and loads assertions into solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="str">SMT-LIB2 formatted string.</param>
    /// <remarks>
    /// Load solver assertions from an SMT-LIB2 formatted string.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverFromString(IntPtr ctx, IntPtr solver, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_from_string");
        var func = Marshal.GetDelegateForFunctionPointer<SolverFromStringDelegate>(funcPtr);
        func(ctx, solver, str);
    }

    /// <summary>
    /// Converts solver state to SMT-LIB2 string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>String representation of solver in SMT-LIB2 format.</returns>
    /// <remarks>
    /// Returns a string containing the solver state in SMT-LIB2 format.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverToString(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<SolverToStringDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves help string describing solver parameters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>String containing help documentation.</returns>
    /// <remarks>
    /// Returns a string describing the available parameters for the solver.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetHelp(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_help");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetHelpDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves parameter descriptors for solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Parameter descriptors object handle.</returns>
    /// <remarks>
    /// Returns a parameter descriptor set that describes all available parameters
    /// for the given solver.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetParamDescrs(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_param_descrs");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetParamDescrsDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves number of backtracking points.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Number of active scopes (push levels).</returns>
    /// <remarks>
    /// Returns the number of times push has been called without a matching pop.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint SolverGetNumScopes(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_num_scopes");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetNumScopesDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Translates solver from one context to another.
    /// </summary>
    /// <param name="sourceCtx">The source Z3 context handle.</param>
    /// <param name="solver">The solver handle in source context.</param>
    /// <param name="targetCtx">The target Z3 context handle.</param>
    /// <returns>Solver handle in target context.</returns>
    /// <remarks>
    /// Copy a solver from one context to another context. All assertions and state
    /// are transferred to the new context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverTranslate(IntPtr sourceCtx, IntPtr solver, IntPtr targetCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_translate");
        var func = Marshal.GetDelegateForFunctionPointer<SolverTranslateDelegate>(funcPtr);
        return func(sourceCtx, solver, targetCtx);
    }

    /// <summary>
    /// Converts solver state to DIMACS format string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="includeName">Whether to include variable names in output.</param>
    /// <returns>String in DIMACS format.</returns>
    /// <remarks>
    /// Returns a string representation of the solver in DIMACS format, commonly used
    /// for SAT solvers. Only works for pure Boolean formulas.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverToDimacsString(IntPtr ctx, IntPtr solver, bool includeName)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_to_dimacs_string");
        var func = Marshal.GetDelegateForFunctionPointer<SolverToDimacsStringDelegate>(funcPtr);
        return func(ctx, solver, includeName);
    }

    /// <summary>
    /// Retrieves cube (partial model) from solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="vars">Array of variables to include in cube.</param>
    /// <param name="numVars">Number of variables.</param>
    /// <returns>AST vector representing the cube.</returns>
    /// <remarks>
    /// Return a cube for the given solver. The cube is a subset of the variables
    /// that can be used to guide search.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverCube(IntPtr ctx, IntPtr solver, IntPtr vars, uint numVars)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_cube");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCubeDelegate>(funcPtr);
        return func(ctx, solver, vars, numVars);
    }

    /// <summary>
    /// Computes consequences of current solver state.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="assumptions">AST vector of assumption literals.</param>
    /// <param name="variables">AST vector of variables of interest.</param>
    /// <param name="consequences">Output AST vector of consequences.</param>
    /// <returns>Result code: 1 (success), -1 (failure), 0 (unknown).</returns>
    /// <remarks>
    /// Compute consequences (implied literals) of the current solver state with respect
    /// to the given assumptions and variables. A consequence is a literal that holds in
    /// all models that satisfy the assumptions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int SolverGetConsequences(
        IntPtr ctx,
        IntPtr solver,
        IntPtr assumptions,
        IntPtr variables,
        IntPtr consequences
    )
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_consequences");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetConsequencesDelegate>(funcPtr);
        return func(ctx, solver, assumptions, variables, consequences);
    }

    /// <summary>
    /// Interrupts ongoing solver computation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <remarks>
    /// Interrupt the execution of the solver. This is useful for stopping long-running
    /// checks. The solver can be used again after interruption.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverInterrupt(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_interrupt");
        var func = Marshal.GetDelegateForFunctionPointer<SolverInterruptDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Computes equivalence classes of terms under current solver state.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="numTerms">Number of terms to analyze.</param>
    /// <param name="terms">Array of term handles.</param>
    /// <param name="classIds">Output array of class identifiers.</param>
    /// <returns>Result code: 1 (satisfiable), -1 (unsatisfiable), 0 (unknown).</returns>
    /// <remarks>
    /// Assigns each term to an equivalence class. Terms in the same class are implied equal
    /// under current solver constraints. Returns Z3_L_TRUE if satisfiable, Z3_L_FALSE if unsatisfiable.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetImpliedEqualities(IntPtr ctx, IntPtr solver, uint numTerms, IntPtr[] terms, uint[] classIds)
    {
        var funcPtr = GetFunctionPointer("Z3_get_implied_equalities");
        var func = Marshal.GetDelegateForFunctionPointer<GetImpliedEqualitiesDelegate>(funcPtr);
        return func(ctx, solver, numTerms, terms, classIds);
    }

    /// <summary>
    /// Retrieves explanation for why two terms are congruent.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="a">First term handle.</param>
    /// <param name="b">Second term handle.</param>
    /// <returns>AST handle representing congruence explanation.</returns>
    /// <remarks>
    /// Provides proof or explanation showing why terms are in same congruence class.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverCongruenceExplain(IntPtr ctx, IntPtr solver, IntPtr a, IntPtr b)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_congruence_explain");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCongruenceExplainDelegate>(funcPtr);
        return func(ctx, solver, a, b);
    }

    /// <summary>
    /// Retrieves next term in congruence class.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="a">Term handle.</param>
    /// <returns>Next term in circular congruence class list.</returns>
    /// <remarks>
    /// Returns next term in circular linked list of congruent terms.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverCongruenceNext(IntPtr ctx, IntPtr solver, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_congruence_next");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCongruenceNextDelegate>(funcPtr);
        return func(ctx, solver, a);
    }

    /// <summary>
    /// Retrieves canonical representative of congruence class.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="a">Term handle.</param>
    /// <returns>Root term of congruence class.</returns>
    /// <remarks>
    /// Returns canonical representative term for congruence class.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverCongruenceRoot(IntPtr ctx, IntPtr solver, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_congruence_root");
        var func = Marshal.GetDelegateForFunctionPointer<SolverCongruenceRootDelegate>(funcPtr);
        return func(ctx, solver, a);
    }

    /// <summary>
    /// Retrieves decision levels for literals.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="literals">AST vector of literal handles.</param>
    /// <param name="sz">Number of literals.</param>
    /// <param name="levels">Output array of decision levels.</param>
    /// <remarks>
    /// Returns decision level at which each literal was assigned in solver trail.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverGetLevels(IntPtr ctx, IntPtr solver, IntPtr literals, uint sz, uint[] levels)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_levels");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetLevelsDelegate>(funcPtr);
        func(ctx, solver, literals, sz, levels);
    }

    /// <summary>
    /// Retrieves non-unit literals from solver trail.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST vector of non-unit literals.</returns>
    /// <remarks>
    /// Returns literals that are not unit clauses in current solver state.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetNonUnits(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_non_units");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetNonUnitsDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves complete solver decision trail.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST vector of trail literals.</returns>
    /// <remarks>
    /// Returns sequence of literals assigned during solver execution.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetTrail(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_trail");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetTrailDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves unit literals from solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>AST vector of unit literals.</returns>
    /// <remarks>
    /// Returns literals that form unit clauses in current solver state.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverGetUnits(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_units");
        var func = Marshal.GetDelegateForFunctionPointer<SolverGetUnitsDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Imports model converter from source to destination solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="src">Source solver handle.</param>
    /// <param name="dst">Destination solver handle.</param>
    /// <remarks>
    /// Transfers model conversion information between solvers for incremental solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverImportModelConverter(IntPtr ctx, IntPtr src, IntPtr dst)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_import_model_converter");
        var func = Marshal.GetDelegateForFunctionPointer<SolverImportModelConverterDelegate>(funcPtr);
        func(ctx, src, dst);
    }

    /// <summary>
    /// Suggests next split decision for solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="cb">Solver callback handle.</param>
    /// <param name="t">Term to split on.</param>
    /// <param name="idx">Variable index.</param>
    /// <param name="phase">Suggested phase value.</param>
    /// <returns>True if split suggestion accepted.</returns>
    /// <remarks>
    /// Used in user propagators to guide solver search decisions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool SolverNextSplit(IntPtr ctx, IntPtr cb, IntPtr t, uint idx, int phase)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_next_split");
        var func = Marshal.GetDelegateForFunctionPointer<SolverNextSplitDelegate>(funcPtr);
        return func(ctx, cb, t, idx, phase);
    }

    /// <summary>
    /// Propagates consequence in user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="cb">Solver callback handle.</param>
    /// <param name="numFixed">Number of fixed terms.</param>
    /// <param name="fixedIds">Array of fixed term handles.</param>
    /// <param name="numEqs">Number of equality premises.</param>
    /// <param name="eqLhs">Array of left-hand sides of equalities.</param>
    /// <param name="eqRhs">Array of right-hand sides of equalities.</param>
    /// <param name="conseq">Consequence to propagate.</param>
    /// <returns>True if propagation succeeded.</returns>
    /// <remarks>
    /// Propagates logical consequence based on fixed values and equalities in user propagator.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool SolverPropagateConsequence(
        IntPtr ctx,
        IntPtr cb,
        uint numFixed,
        IntPtr[] fixedIds,
        uint numEqs,
        IntPtr[] eqLhs,
        IntPtr[] eqRhs,
        IntPtr conseq
    )
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_consequence");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateConsequenceDelegate>(funcPtr);
        return func(ctx, cb, numFixed, fixedIds, numEqs, eqLhs, eqRhs, conseq);
    }

    /// <summary>
    /// Registers created callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="createdEh">Callback invoked when terms created.</param>
    /// <remarks>
    /// Registers callback invoked when new terms are created during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateCreated(IntPtr ctx, IntPtr solver, CreatedEhDelegate createdEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_created");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateCreatedDelegate>(funcPtr);
        func(ctx, solver, createdEh);
    }

    /// <summary>
    /// Registers decide callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="decideEh">Callback invoked for decision points.</param>
    /// <remarks>
    /// Registers callback invoked when solver makes split decisions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateDecide(IntPtr ctx, IntPtr solver, DecideEhDelegate decideEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_decide");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateDecideDelegate>(funcPtr);
        func(ctx, solver, decideEh);
    }

    /// <summary>
    /// Declares function for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Function name symbol.</param>
    /// <param name="n">Number of domain sorts.</param>
    /// <param name="domain">Array of domain sort handles.</param>
    /// <param name="range">Range sort handle.</param>
    /// <returns>Function declaration handle.</returns>
    /// <remarks>
    /// Creates function declaration tracked by user propagator.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SolverPropagateDeclare(IntPtr ctx, IntPtr name, uint n, IntPtr[] domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_declare");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateDeclareDelegate>(funcPtr);
        return func(ctx, name, n, domain, range);
    }

    /// <summary>
    /// Registers disequality callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="diseqEh">Callback invoked for disequalities.</param>
    /// <remarks>
    /// Registers callback invoked when terms become disequal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateDiseq(IntPtr ctx, IntPtr solver, EqEhDelegate diseqEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_diseq");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateDiseqDelegate>(funcPtr);
        func(ctx, solver, diseqEh);
    }

    /// <summary>
    /// Registers equality callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="eqEh">Callback invoked for equalities.</param>
    /// <remarks>
    /// Registers callback invoked when terms become equal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateEq(IntPtr ctx, IntPtr solver, EqEhDelegate eqEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_eq");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateEqDelegate>(funcPtr);
        func(ctx, solver, eqEh);
    }

    /// <summary>
    /// Registers final check callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="finalEh">Callback invoked for final check.</param>
    /// <remarks>
    /// Registers callback invoked during final satisfiability check.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateFinal(IntPtr ctx, IntPtr solver, FinalEhDelegate finalEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_final");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateFinalDelegate>(funcPtr);
        func(ctx, solver, finalEh);
    }

    /// <summary>
    /// Registers fixed callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="fixedEh">Callback invoked when terms fixed.</param>
    /// <remarks>
    /// Registers callback invoked when registered terms become fixed to values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateFixed(IntPtr ctx, IntPtr solver, FixedEhDelegate fixedEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_fixed");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateFixedDelegate>(funcPtr);
        func(ctx, solver, fixedEh);
    }

    /// <summary>
    /// Initializes user propagator with callbacks.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="userContext">User-provided context pointer.</param>
    /// <param name="pushEh">Push callback.</param>
    /// <param name="popEh">Pop callback.</param>
    /// <param name="freshEh">Fresh context callback.</param>
    /// <remarks>
    /// Initializes user propagator with essential callbacks for backtracking and context management.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateInit(
        IntPtr ctx,
        IntPtr solver,
        IntPtr userContext,
        PushEhDelegate pushEh,
        PopEhDelegate popEh,
        FreshEhDelegate freshEh
    )
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_init");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateInitDelegate>(funcPtr);
        func(ctx, solver, userContext, pushEh, popEh, freshEh);
    }

    /// <summary>
    /// Registers binding callback for user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="bindingEh">Callback invoked for variable bindings.</param>
    /// <remarks>
    /// Registers callback invoked when quantifier variables are bound.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateOnBinding(IntPtr ctx, IntPtr solver, OnBindingEhDelegate bindingEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_on_binding");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateOnBindingDelegate>(funcPtr);
        func(ctx, solver, bindingEh);
    }

    /// <summary>
    /// Registers expression for propagation in user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="e">Expression handle to register.</param>
    /// <remarks>
    /// Registers expression to track in user propagator callbacks.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateRegister(IntPtr ctx, IntPtr solver, IntPtr e)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_register");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateRegisterDelegate>(funcPtr);
        func(ctx, solver, e);
    }

    /// <summary>
    /// Registers expression via callback in user propagator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="cb">Solver callback handle.</param>
    /// <param name="e">Expression handle to register.</param>
    /// <remarks>
    /// Registers expression dynamically within propagator callback context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverPropagateRegisterCb(IntPtr ctx, IntPtr cb, IntPtr e)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_propagate_register_cb");
        var func = Marshal.GetDelegateForFunctionPointer<SolverPropagateRegisterCbDelegate>(funcPtr);
        func(ctx, cb, e);
    }

    /// <summary>
    /// Registers clause callback for solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="userContext">User-provided context pointer.</param>
    /// <param name="onClauseEh">Callback invoked when clauses learned.</param>
    /// <remarks>
    /// Registers callback invoked when solver learns new clauses.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverRegisterOnClause(IntPtr ctx, IntPtr solver, IntPtr userContext, OnClauseEhDelegate onClauseEh)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_register_on_clause");
        var func = Marshal.GetDelegateForFunctionPointer<SolverRegisterOnClauseDelegate>(funcPtr);
        func(ctx, solver, userContext, onClauseEh);
    }

    /// <summary>
    /// Sets initial value hint for constant in solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="constant">Constant handle.</param>
    /// <param name="value">Initial value hint.</param>
    /// <remarks>
    /// Provides initial value suggestion for constant to guide solver search.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void SolverSetInitialValue(IntPtr ctx, IntPtr solver, IntPtr constant, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_set_initial_value");
        var func = Marshal.GetDelegateForFunctionPointer<SolverSetInitialValueDelegate>(funcPtr);
        func(ctx, solver, constant, value);
    }

    /// <summary>
    /// Checks satisfiability focused on specified variables.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <param name="numVars">Number of variables to focus on.</param>
    /// <param name="vars">Array of variable handles.</param>
    /// <returns>Satisfiability result: 1 (satisfiable), -1 (unsatisfiable), 0 (unknown).</returns>
    /// <remarks>
    /// Performs satisfiability check with search focused on given variables for model projection.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int SolverSolveFor(IntPtr ctx, IntPtr solver, uint numVars, IntPtr[] vars)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_solve_for");
        var func = Marshal.GetDelegateForFunctionPointer<SolverSolveForDelegate>(funcPtr);
        return func(ctx, solver, numVars, vars);
    }
}
