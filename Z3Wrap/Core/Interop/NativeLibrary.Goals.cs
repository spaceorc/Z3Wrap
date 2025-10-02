// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Goals API - P/Invoke bindings for Z3 goal-based solving
//
// Source: z3_api.h from Z3 C API (Goals section)
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's goals API (17 functions):
// - Goal creation and reference counting
// - Goal assertion and manipulation
// - Goal state queries (inconsistent, decided sat/unsat)
// - Goal translation and conversion
// - DIMACS export

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsGoals(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_goal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_precision");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_assert");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_inconsistent");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_depth");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_reset");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_formula");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_num_exprs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_is_decided_sat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_is_decided_unsat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_translate");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_convert_model");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_to_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_goal_to_dimacs_string");
    }

    // Delegates
    private delegate IntPtr MkGoalDelegate(IntPtr ctx, bool models, bool unsatCores, bool proofs);
    private delegate void GoalIncRefDelegate(IntPtr ctx, IntPtr goal);
    private delegate void GoalDecRefDelegate(IntPtr ctx, IntPtr goal);
    private delegate uint GoalPrecisionDelegate(IntPtr ctx, IntPtr goal);
    private delegate void GoalAssertDelegate(IntPtr ctx, IntPtr goal, IntPtr formula);
    private delegate bool GoalInconsistentDelegate(IntPtr ctx, IntPtr goal);
    private delegate uint GoalDepthDelegate(IntPtr ctx, IntPtr goal);
    private delegate void GoalResetDelegate(IntPtr ctx, IntPtr goal);
    private delegate uint GoalSizeDelegate(IntPtr ctx, IntPtr goal);
    private delegate IntPtr GoalFormulaDelegate(IntPtr ctx, IntPtr goal, uint index);
    private delegate uint GoalNumExprsDelegate(IntPtr ctx, IntPtr goal);
    private delegate bool GoalIsDecidedSatDelegate(IntPtr ctx, IntPtr goal);
    private delegate bool GoalIsDecidedUnsatDelegate(IntPtr ctx, IntPtr goal);
    private delegate IntPtr GoalTranslateDelegate(IntPtr sourceCtx, IntPtr goal, IntPtr targetCtx);
    private delegate IntPtr GoalConvertModelDelegate(IntPtr ctx, IntPtr goal, IntPtr model);
    private delegate IntPtr GoalToStringDelegate(IntPtr ctx, IntPtr goal);
    private delegate IntPtr GoalToDimacsStringDelegate(IntPtr ctx, IntPtr goal);

    // Methods
    /// <summary>
    /// Creates a goal for tactic-based solving.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="models">If true, model generation is enabled.</param>
    /// <param name="unsatCores">If true, unsat core generation is enabled.</param>
    /// <param name="proofs">If true, proof generation is enabled.</param>
    /// <returns>Goal handle.</returns>
    /// <remarks>
    /// A goal is a set of formulas. Goals are used to represent intermediate states
    /// in tactic-based solving. The Boolean flags control which auxiliary information
    /// should be tracked.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkGoal(IntPtr ctx, bool models, bool unsatCores, bool proofs)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_goal");
        var func = Marshal.GetDelegateForFunctionPointer<MkGoalDelegate>(funcPtr);
        return func(ctx, models, unsatCores, proofs);
    }

    /// <summary>
    /// Increments the reference counter of goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Increment the reference count
    /// to prevent premature deallocation. Must be paired with GoalDecRef.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GoalIncRef(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<GoalIncRefDelegate>(funcPtr);
        func(ctx, goal);
    }

    /// <summary>
    /// Decrements the reference counter of goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <remarks>
    /// Must be paired with GoalIncRef. When reference count reaches zero,
    /// the goal may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GoalDecRef(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<GoalDecRefDelegate>(funcPtr);
        func(ctx, goal);
    }

    /// <summary>
    /// Retrieves the precision level of the goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>Precision level (0=precise, 1=under, 2=over, 3=under_over).</returns>
    /// <remarks>
    /// Returns the precision of the given goal. Four precision levels are defined:
    /// PRECISE (0), UNDER (1), OVER (2), UNDER_OVER (3).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GoalPrecision(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_precision");
        var func = Marshal.GetDelegateForFunctionPointer<GoalPrecisionDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Adds a formula to the goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <param name="formula">The formula to add.</param>
    /// <remarks>
    /// Assert a formula into the given goal. The formula must be a Boolean expression.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GoalAssert(IntPtr ctx, IntPtr goal, IntPtr formula)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_assert");
        var func = Marshal.GetDelegateForFunctionPointer<GoalAssertDelegate>(funcPtr);
        func(ctx, goal, formula);
    }

    /// <summary>
    /// Checks if goal is inconsistent.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>True if goal is known to be inconsistent.</returns>
    /// <remarks>
    /// Returns true if the goal contains false (i.e., it is trivially unsatisfiable).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GoalInconsistent(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_inconsistent");
        var func = Marshal.GetDelegateForFunctionPointer<GoalInconsistentDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Retrieves the depth of the goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>Depth level.</returns>
    /// <remarks>
    /// Returns the depth of the given goal. The depth corresponds to the number of
    /// tactics applied to create the goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GoalDepth(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_depth");
        var func = Marshal.GetDelegateForFunctionPointer<GoalDepthDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Removes all formulas from the goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <remarks>
    /// Erases all formulas from the given goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GoalReset(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_reset");
        var func = Marshal.GetDelegateForFunctionPointer<GoalResetDelegate>(funcPtr);
        func(ctx, goal);
    }

    /// <summary>
    /// Retrieves the size of the goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>Size (number of formulas).</returns>
    /// <remarks>
    /// Returns the number of formulas in the goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GoalSize(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_size");
        var func = Marshal.GetDelegateForFunctionPointer<GoalSizeDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Retrieves formula at given index from goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <param name="index">Formula index (0-based).</param>
    /// <returns>Formula AST at the given index.</returns>
    /// <remarks>
    /// Returns the formula at position index in the given goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GoalFormula(IntPtr ctx, IntPtr goal, uint index)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_formula");
        var func = Marshal.GetDelegateForFunctionPointer<GoalFormulaDelegate>(funcPtr);
        return func(ctx, goal, index);
    }

    /// <summary>
    /// Retrieves number of expressions in goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>Number of expressions.</returns>
    /// <remarks>
    /// Returns the total number of AST nodes in the formulas of the goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GoalNumExprs(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_num_exprs");
        var func = Marshal.GetDelegateForFunctionPointer<GoalNumExprsDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Checks if goal is decided to be satisfiable.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>True if goal is decided SAT.</returns>
    /// <remarks>
    /// Returns true if the goal is empty or contains only true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GoalIsDecidedSat(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_is_decided_sat");
        var func = Marshal.GetDelegateForFunctionPointer<GoalIsDecidedSatDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Checks if goal is decided to be unsatisfiable.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>True if goal is decided UNSAT.</returns>
    /// <remarks>
    /// Returns true if the goal contains false.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GoalIsDecidedUnsat(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_is_decided_unsat");
        var func = Marshal.GetDelegateForFunctionPointer<GoalIsDecidedUnsatDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Translates goal from one context to another.
    /// </summary>
    /// <param name="sourceCtx">The source Z3 context handle.</param>
    /// <param name="goal">The goal handle in source context.</param>
    /// <param name="targetCtx">The target Z3 context handle.</param>
    /// <returns>Goal handle in target context.</returns>
    /// <remarks>
    /// Copy a goal from one context to another context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GoalTranslate(IntPtr sourceCtx, IntPtr goal, IntPtr targetCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_translate");
        var func = Marshal.GetDelegateForFunctionPointer<GoalTranslateDelegate>(funcPtr);
        return func(sourceCtx, goal, targetCtx);
    }

    /// <summary>
    /// Converts model to be valid for goal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <param name="model">The model handle.</param>
    /// <returns>Converted model handle.</returns>
    /// <remarks>
    /// Convert a model for the goal returned by a tactic into a model for the original
    /// goal. This function allows the user to convert a model for a transformed goal back
    /// to the original goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GoalConvertModel(IntPtr ctx, IntPtr goal, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_convert_model");
        var func = Marshal.GetDelegateForFunctionPointer<GoalConvertModelDelegate>(funcPtr);
        return func(ctx, goal, model);
    }

    /// <summary>
    /// Converts goal to string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>String representation of the goal.</returns>
    /// <remarks>
    /// Returns a string representing the goal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GoalToString(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<GoalToStringDelegate>(funcPtr);
        return func(ctx, goal);
    }

    /// <summary>
    /// Converts goal to DIMACS format string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="goal">The goal handle.</param>
    /// <returns>String in DIMACS format.</returns>
    /// <remarks>
    /// Returns a string representation of the goal in DIMACS format. Only works for
    /// pure Boolean formulas.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GoalToDimacsString(IntPtr ctx, IntPtr goal)
    {
        var funcPtr = GetFunctionPointer("Z3_goal_to_dimacs_string");
        var func = Marshal.GetDelegateForFunctionPointer<GoalToDimacsStringDelegate>(funcPtr);
        return func(ctx, goal);
    }
}
