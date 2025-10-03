// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Solver API - P/Invoke bindings for Z3 solver operations
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides complete bindings for Z3's solver API (31 functions):
// - Solver creation (Z3_mk_solver, Z3_mk_simple_solver, Z3_mk_solver_from_tactic, Z3_mk_solver_for_logic)
// - Reference counting (inc_ref, dec_ref)
// - Assertions and satisfiability checking (assert, check, check_assumptions)
// - Backtracking stack operations (push, pop, reset)
// - Model extraction
// - Parameter configuration
// - Assumption-based checking with unsat core extraction
// - Proof generation and extraction
// - SMT-LIB2 format I/O (file and string parsing)
// - Solver statistics and diagnostics
// - Context translation (moving solvers between contexts)
// - Advanced solving modes (cubes, consequences)
// - Solver control and interruption
//
// Missing Functions (25 functions):
// - Z3_get_implied_equalities
// - Z3_solver_congruence_explain
// - Z3_solver_congruence_next
// - Z3_solver_congruence_root
// - Z3_solver_get_levels
// - Z3_solver_get_non_units
// - Z3_solver_get_trail
// - Z3_solver_get_units
// - Z3_solver_import_model_converter
// - Z3_solver_next_split
// - Z3_solver_propagate_consequence
// - Z3_solver_propagate_created
// - Z3_solver_propagate_decide
// - Z3_solver_propagate_declare
// - Z3_solver_propagate_diseq
// - Z3_solver_propagate_eq
// - Z3_solver_propagate_final
// - Z3_solver_propagate_fixed
// - Z3_solver_propagate_init
// - Z3_solver_propagate_on_binding
// - Z3_solver_propagate_register
// - Z3_solver_propagate_register_cb
// - Z3_solver_register_on_clause
// - Z3_solver_set_initial_value
// - Z3_solver_solve_for

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
}
