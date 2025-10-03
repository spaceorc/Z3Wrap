// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Solver API - P/Invoke bindings for Z3 basic solver operations
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's basic solver API (13 of 14 functions - 92.9% complete):
// - Solver creation (Z3_mk_solver, Z3_mk_simple_solver, Z3_mk_solver_from_tactic)
// - Reference counting (inc_ref, dec_ref)
// - Assertions and satisfiability checking
// - Backtracking stack operations (push, pop, reset)
// - Model extraction
// - Parameter configuration
//
// Missing: Z3_mk_solver_for_logic (logic-specific solver creation for performance optimization)
//
// Advanced solver features (assumptions, cores, proofs, statistics, etc.) are in NativeLibrary.SolverExtensions.cs
// See COMPARISON_Solver.md for complete API coverage analysis.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSolver(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_solver");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_simple_solver");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_inc_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_dec_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_assert");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_check");
        LoadFunctionInternal(handle, functionPointers, "Z3_solver_get_model");

        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_solver_from_tactic");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_push");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_pop");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_reset");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_get_reason_unknown");
        LoadFunctionOrNull(handle, functionPointers, "Z3_solver_set_params");
    }

    // Delegates
    private delegate IntPtr MkSolverDelegate(IntPtr ctx);
    private delegate IntPtr MkSimpleSolverDelegate(IntPtr ctx);
    private delegate IntPtr MkSolverFromTacticDelegate(IntPtr ctx, IntPtr tactic);
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
}
