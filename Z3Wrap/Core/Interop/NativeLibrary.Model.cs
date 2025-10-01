using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for specific API functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for this group of Z3 API functions.
    /// </summary>
    private static void LoadFunctionsModel(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_model_inc_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_model_dec_ref");
        LoadFunctionInternal(handle, functionPointers, "Z3_model_to_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_model_eval");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_numeral_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_bool_value");
        LoadFunctionInternal(handle, functionPointers, "Z3_is_numeral_ast");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_sort_kind");
    }

    // Delegates
    private delegate void ModelIncRefDelegate(IntPtr ctx, IntPtr model);
    private delegate void ModelDecRefDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr ModelToStringDelegate(IntPtr ctx, IntPtr model);
    private delegate int ModelEvalDelegate(
        IntPtr ctx,
        IntPtr model,
        IntPtr expr,
        int modelCompletion,
        out IntPtr result
    );
    private delegate IntPtr GetNumeralStringDelegate(IntPtr ctx, IntPtr expr);
    private delegate int GetBoolValueDelegate(IntPtr ctx, IntPtr expr);
    private delegate int IsNumeralAstDelegate(IntPtr ctx, IntPtr expr);
    private delegate IntPtr GetSortDelegate(IntPtr ctx, IntPtr expr);
    private delegate int GetSortKindDelegate(IntPtr ctx, IntPtr sort);

    // Methods
    /// <summary>
    /// Increments the reference counter of the given model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Must be paired with
    /// ModelDecRef when the model is no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ModelIncRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ModelIncRefDelegate>(funcPtr);
        func(ctx, model);
    }

    /// <summary>
    /// Decrements the reference counter of the given model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle to decrement reference count for.</param>
    /// <remarks>
    /// Must be paired with ModelIncRef to properly manage memory. When reference
    /// count reaches zero, the model may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ModelDecRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ModelDecRefDelegate>(funcPtr);
        func(ctx, model);
    }

    /// <summary>
    /// Converts a Z3 model to its string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle to convert.</param>
    /// <returns>Handle to a null-terminated string representation of the model.</returns>
    /// <remarks>
    /// Provides a human-readable representation showing variable assignments.
    /// The string is managed by Z3 and valid until the context is deleted.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelToString(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<ModelToStringDelegate>(funcPtr);
        return func(ctx, model);
    }

    /// <summary>
    /// Evaluates an expression in the given model to obtain its value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle containing variable assignments.</param>
    /// <param name="expr">The expression to evaluate.</param>
    /// <param name="modelCompletion">Whether to use model completion for undefined values.</param>
    /// <param name="result">Output parameter receiving the evaluated expression result.</param>
    /// <returns>True if evaluation succeeded, false otherwise.</returns>
    /// <remarks>
    /// Model completion assigns default values to variables not explicitly defined in the model.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_model_eval");
        var func = Marshal.GetDelegateForFunctionPointer<ModelEvalDelegate>(funcPtr);
        return func(ctx, model, expr, modelCompletion ? 1 : 0, out result) != 0;
    }

    /// <summary>
    /// Retrieves the string representation of a numeric expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="expr">The numeric expression to convert.</param>
    /// <returns>Handle to a null-terminated string representation of the numeric value.</returns>
    /// <remarks>
    /// Works with integer and real number expressions. For rationals, returns fractional
    /// notation (e.g., "22/7"). The string is managed by Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralStringDelegate>(funcPtr);
        return func(ctx, expr);
    }

    /// <summary>
    /// Retrieves the Boolean value of a Boolean expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="expr">The Boolean expression to evaluate.</param>
    /// <returns>1 for true, -1 for false, 0 for unknown/undef.</returns>
    /// <remarks>
    /// Only valid for Boolean expressions that evaluate to concrete true or false values.
    /// Returns Z3_L_UNDEF (0) if the expression is not a concrete Boolean value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bool_value");
        var func = Marshal.GetDelegateForFunctionPointer<GetBoolValueDelegate>(funcPtr);
        return func(ctx, expr);
    }

    /// <summary>
    /// Checks whether an expression is a numeric literal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="expr">The expression to check.</param>
    /// <returns>True if the expression is a numeric literal, false otherwise.</returns>
    /// <remarks>
    /// Identifies concrete numeric values (integers and reals) as opposed to variables or operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_is_numeral_ast");
        var func = Marshal.GetDelegateForFunctionPointer<IsNumeralAstDelegate>(funcPtr);
        return func(ctx, expr) != 0;
    }

    /// <summary>
    /// Retrieves the sort (type) of a Z3 expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="expr">The expression to get the sort for.</param>
    /// <returns>Handle to the sort of the expression.</returns>
    /// <remarks>
    /// Used for type checking and determining the kind of expression (Boolean, integer, real, etc.).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetSort(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort");
        var func = Marshal.GetDelegateForFunctionPointer<GetSortDelegate>(funcPtr);
        return func(ctx, expr);
    }

    /// <summary>
    /// Retrieves the kind of a Z3 sort (type identifier).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The sort to get the kind for.</param>
    /// <returns>Integer representing the sort kind (e.g., Z3_BOOL_SORT, Z3_INT_SORT, Z3_REAL_SORT).</returns>
    /// <remarks>
    /// Used to determine the specific type of a sort for type checking and dispatch logic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_kind");
        var func = Marshal.GetDelegateForFunctionPointer<GetSortKindDelegate>(funcPtr);
        return func(ctx, sort);
    }
}
