using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for solver extension functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for solver extension Z3 API functions.
    /// </summary>
    private static void LoadFunctionsSolverExtensions(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
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
