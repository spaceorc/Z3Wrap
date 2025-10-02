using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsOptimization(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Creation and Management
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_optimize");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_dec_ref");

        // Assertions and Objectives
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_assert");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_assert_soft");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_assert_and_track");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_maximize");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_minimize");

        // Solving and Results
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_check");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_model");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_upper");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_lower");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_reason_unknown");

        // Utilities
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_from_file");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_from_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_help");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_set_params");
        LoadFunctionOrNull(handle, functionPointers, "Z3_optimize_get_param_descrs");
    }

    // Delegates

    // Creation and Management
    private delegate IntPtr MkOptimizeDelegate(IntPtr ctx);
    private delegate void OptimizeIncRefDelegate(IntPtr ctx, IntPtr optimize);
    private delegate void OptimizeDecRefDelegate(IntPtr ctx, IntPtr optimize);

    // Assertions and Objectives
    private delegate void OptimizeAssertDelegate(IntPtr ctx, IntPtr optimize, IntPtr constraint);
    private delegate uint OptimizeAssertSoftDelegate(
        IntPtr ctx,
        IntPtr optimize,
        IntPtr constraint,
        IntPtr weight,
        IntPtr id
    );
    private delegate void OptimizeAssertAndTrackDelegate(
        IntPtr ctx,
        IntPtr optimize,
        IntPtr constraint,
        IntPtr trackingLiteral
    );
    private delegate uint OptimizeMaximizeDelegate(IntPtr ctx, IntPtr optimize, IntPtr objective);
    private delegate uint OptimizeMinimizeDelegate(IntPtr ctx, IntPtr optimize, IntPtr objective);

    // Solving and Results
    private delegate int OptimizeCheckDelegate(IntPtr ctx, IntPtr optimize, uint numAssumptions, IntPtr[] assumptions);
    private delegate IntPtr OptimizeGetModelDelegate(IntPtr ctx, IntPtr optimize);
    private delegate IntPtr OptimizeGetUpperDelegate(IntPtr ctx, IntPtr optimize, uint idx);
    private delegate IntPtr OptimizeGetLowerDelegate(IntPtr ctx, IntPtr optimize, uint idx);
    private delegate IntPtr OptimizeGetReasonUnknownDelegate(IntPtr ctx, IntPtr optimize);

    // Utilities
    private delegate IntPtr OptimizeToStringDelegate(IntPtr ctx, IntPtr optimize);
    private delegate void OptimizeFromFileDelegate(IntPtr ctx, IntPtr optimize, IntPtr fileName);
    private delegate void OptimizeFromStringDelegate(IntPtr ctx, IntPtr optimize, IntPtr str);
    private delegate IntPtr OptimizeGetHelpDelegate(IntPtr ctx, IntPtr optimize);
    private delegate void OptimizeSetParamsDelegate(IntPtr ctx, IntPtr optimize, IntPtr paramsHandle);
    private delegate IntPtr OptimizeGetParamDescrsDelegate(IntPtr ctx, IntPtr optimize);

    // Methods

    // Creation and Management
    /// <summary>
    /// Creates optimization context for MaxSMT and multi-objective optimization.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Optimization context handle.</returns>
    /// <remarks>
    /// Optimization context extends solver with objective functions (maximize/minimize)
    /// and soft constraints (assertions with weights). Supports MaxSMT, multi-objective
    /// optimization, and Pareto-optimal solution finding.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkOptimize(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_optimize");
        var func = Marshal.GetDelegateForFunctionPointer<MkOptimizeDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments reference count for optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <remarks>
    /// Prevents optimization context from being garbage collected by Z3.
    /// Must be balanced with OptimizeDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeIncRef(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeIncRefDelegate>(funcPtr);
        func(ctx, optimize);
    }

    /// <summary>
    /// Decrements reference count for optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <remarks>
    /// When reference count reaches zero, optimization context is freed by Z3.
    /// Must be balanced with OptimizeIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeDecRef(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeDecRefDelegate>(funcPtr);
        func(ctx, optimize);
    }

    // Assertions and Objectives
    /// <summary>
    /// Adds hard constraint to optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="constraint">Boolean constraint expression.</param>
    /// <remarks>
    /// Hard constraints must be satisfied in all solutions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeAssert(IntPtr ctx, IntPtr optimize, IntPtr constraint)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_assert");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeAssertDelegate>(funcPtr);
        func(ctx, optimize, constraint);
    }

    /// <summary>
    /// Adds soft constraint with weight to optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="constraint">Boolean constraint expression.</param>
    /// <param name="weight">Constraint weight (string representation of rational or decimal).</param>
    /// <param name="id">Constraint identifier symbol.</param>
    /// <returns>Constraint identifier for tracking.</returns>
    /// <remarks>
    /// Soft constraints are preferred but not required. MaxSMT solver maximizes
    /// weighted sum of satisfied soft constraints while satisfying all hard constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint OptimizeAssertSoft(IntPtr ctx, IntPtr optimize, IntPtr constraint, IntPtr weight, IntPtr id)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_assert_soft");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeAssertSoftDelegate>(funcPtr);
        return func(ctx, optimize, constraint, weight, id);
    }

    /// <summary>
    /// Adds constraint with tracking literal to optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="constraint">Boolean constraint expression.</param>
    /// <param name="trackingLiteral">Boolean literal for tracking constraint.</param>
    /// <remarks>
    /// Tracking literals enable constraint retraction and unsat core extraction.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeAssertAndTrack(IntPtr ctx, IntPtr optimize, IntPtr constraint, IntPtr trackingLiteral)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_assert_and_track");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeAssertAndTrackDelegate>(funcPtr);
        func(ctx, optimize, constraint, trackingLiteral);
    }

    /// <summary>
    /// Adds maximize objective to optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="objective">Arithmetic expression to maximize.</param>
    /// <returns>Objective identifier for retrieving bounds.</returns>
    /// <remarks>
    /// Multiple objectives create multi-objective optimization using lexicographic ordering.
    /// First objective has highest priority, subsequent objectives break ties.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint OptimizeMaximize(IntPtr ctx, IntPtr optimize, IntPtr objective)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_maximize");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeMaximizeDelegate>(funcPtr);
        return func(ctx, optimize, objective);
    }

    /// <summary>
    /// Adds minimize objective to optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="objective">Arithmetic expression to minimize.</param>
    /// <returns>Objective identifier for retrieving bounds.</returns>
    /// <remarks>
    /// Multiple objectives create multi-objective optimization using lexicographic ordering.
    /// First objective has highest priority, subsequent objectives break ties.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint OptimizeMinimize(IntPtr ctx, IntPtr optimize, IntPtr objective)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_minimize");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeMinimizeDelegate>(funcPtr);
        return func(ctx, optimize, objective);
    }

    // Solving and Results
    /// <summary>
    /// Solves optimization problem with optional assumptions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="numAssumptions">Number of assumptions.</param>
    /// <param name="assumptions">Array of boolean assumption literals.</param>
    /// <returns>Status code (1=satisfiable, -1=unsatisfiable, 0=unknown).</returns>
    /// <remarks>
    /// Finds solution satisfying all constraints while optimizing objectives.
    /// Assumptions are temporary additional constraints for this check only.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int OptimizeCheck(IntPtr ctx, IntPtr optimize, uint numAssumptions, IntPtr[] assumptions)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_check");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeCheckDelegate>(funcPtr);
        return func(ctx, optimize, numAssumptions, assumptions);
    }

    /// <summary>
    /// Gets optimal model from optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <returns>Model handle representing optimal solution.</returns>
    /// <remarks>
    /// Only valid after successful OptimizeCheck call (status=satisfiable).
    /// Model assigns values satisfying constraints and optimizing objectives.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetModel(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_model");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetModelDelegate>(funcPtr);
        return func(ctx, optimize);
    }

    /// <summary>
    /// Gets upper bound for objective value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="idx">Objective identifier returned from OptimizeMaximize/OptimizeMinimize.</param>
    /// <returns>Expression representing upper bound (may be infinity).</returns>
    /// <remarks>
    /// Returns upper bound on objective value in optimal solution.
    /// For maximize objectives, this is the achieved maximum value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetUpper(IntPtr ctx, IntPtr optimize, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_upper");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetUpperDelegate>(funcPtr);
        return func(ctx, optimize, idx);
    }

    /// <summary>
    /// Gets lower bound for objective value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="idx">Objective identifier returned from OptimizeMaximize/OptimizeMinimize.</param>
    /// <returns>Expression representing lower bound (may be negative infinity).</returns>
    /// <remarks>
    /// Returns lower bound on objective value in optimal solution.
    /// For minimize objectives, this is the achieved minimum value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetLower(IntPtr ctx, IntPtr optimize, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_lower");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetLowerDelegate>(funcPtr);
        return func(ctx, optimize, idx);
    }

    /// <summary>
    /// Gets reason for unknown optimization result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <returns>String describing why optimization result is unknown.</returns>
    /// <remarks>
    /// Only meaningful when OptimizeCheck returns unknown status (0).
    /// Provides diagnostic information about solver limitations or timeouts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetReasonUnknown(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_reason_unknown");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetReasonUnknownDelegate>(funcPtr);
        return func(ctx, optimize);
    }

    // Utilities
    /// <summary>
    /// Gets string representation of optimization context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <returns>SMTLIB2 format string representing optimization problem.</returns>
    /// <remarks>
    /// Returns all constraints, objectives, and assertions in SMTLIB2 format.
    /// Useful for debugging and exporting optimization problems.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeToString(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeToStringDelegate>(funcPtr);
        return func(ctx, optimize);
    }

    /// <summary>
    /// Parses optimization problem from SMTLIB2 file.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="fileName">Path to SMTLIB2 file.</param>
    /// <remarks>
    /// Reads constraints and objectives from file in SMTLIB2 format.
    /// Adds parsed formulas to optimization context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeFromFile(IntPtr ctx, IntPtr optimize, IntPtr fileName)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_from_file");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeFromFileDelegate>(funcPtr);
        func(ctx, optimize, fileName);
    }

    /// <summary>
    /// Parses optimization problem from SMTLIB2 string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="str">SMTLIB2 format string.</param>
    /// <remarks>
    /// Parses constraints and objectives from string in SMTLIB2 format.
    /// Adds parsed formulas to optimization context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeFromString(IntPtr ctx, IntPtr optimize, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_from_string");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeFromStringDelegate>(funcPtr);
        func(ctx, optimize, str);
    }

    /// <summary>
    /// Gets help text for optimization solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <returns>String containing help text and available options.</returns>
    /// <remarks>
    /// Returns documentation of optimization solver capabilities and parameters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetHelp(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_help");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetHelpDelegate>(funcPtr);
        return func(ctx, optimize);
    }

    /// <summary>
    /// Sets parameters for optimization solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <param name="paramsHandle">Parameters object handle.</param>
    /// <remarks>
    /// Configures optimization solver behavior (timeouts, strategies, etc.).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void OptimizeSetParams(IntPtr ctx, IntPtr optimize, IntPtr paramsHandle)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_set_params");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeSetParamsDelegate>(funcPtr);
        func(ctx, optimize, paramsHandle);
    }

    /// <summary>
    /// Gets parameter descriptors for optimization solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="optimize">Optimization context handle.</param>
    /// <returns>Parameter descriptors object handle.</returns>
    /// <remarks>
    /// Returns metadata describing available optimization solver parameters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr OptimizeGetParamDescrs(IntPtr ctx, IntPtr optimize)
    {
        var funcPtr = GetFunctionPointer("Z3_optimize_get_param_descrs");
        var func = Marshal.GetDelegateForFunctionPointer<OptimizeGetParamDescrsDelegate>(funcPtr);
        return func(ctx, optimize);
    }
}
