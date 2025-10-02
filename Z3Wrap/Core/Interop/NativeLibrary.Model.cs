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
        LoadFunctionInternal(handle, functionPointers, "Z3_get_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_sort_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_num_consts");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_const_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_const_interp");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_num_funcs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_func_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_func_interp");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_has_interp");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_num_sorts");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_get_sort_universe");
        LoadFunctionOrNull(handle, functionPointers, "Z3_model_translate");
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
    private delegate IntPtr GetSortDelegate(IntPtr ctx, IntPtr expr);
    private delegate int GetSortKindDelegate(IntPtr ctx, IntPtr sort);
    private delegate uint ModelGetNumConstsDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr ModelGetConstDeclDelegate(IntPtr ctx, IntPtr model, uint i);
    private delegate IntPtr ModelGetConstInterpDelegate(IntPtr ctx, IntPtr model, IntPtr decl);
    private delegate uint ModelGetNumFuncsDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr ModelGetFuncDeclDelegate(IntPtr ctx, IntPtr model, uint i);
    private delegate IntPtr ModelGetFuncInterpDelegate(IntPtr ctx, IntPtr model, IntPtr decl);
    private delegate int ModelHasInterpDelegate(IntPtr ctx, IntPtr model, IntPtr decl);
    private delegate uint ModelGetNumSortsDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr ModelGetSortDelegate(IntPtr ctx, IntPtr model, uint i);
    private delegate IntPtr ModelGetSortUniverseDelegate(IntPtr ctx, IntPtr model, IntPtr sort);
    private delegate IntPtr ModelTranslateDelegate(IntPtr ctx, IntPtr model, IntPtr dstCtx);

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

    /// <summary>
    /// Retrieves the number of constant interpretations in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <returns>Number of constant declarations with interpretations in the model.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ModelGetNumConsts(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_num_consts");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetNumConstsDelegate>(funcPtr);
        return func(ctx, model);
    }

    /// <summary>
    /// Retrieves constant declaration at specified index in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="i">Index of the constant (must be less than ModelGetNumConsts).</param>
    /// <returns>Handle to the constant declaration at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetConstDecl(IntPtr ctx, IntPtr model, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_const_decl");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetConstDeclDelegate>(funcPtr);
        return func(ctx, model, i);
    }

    /// <summary>
    /// Retrieves the interpretation of a constant in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="decl">The constant declaration to get interpretation for.</param>
    /// <returns>Handle to the interpretation expression, or IntPtr.Zero if no interpretation.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetConstInterp(IntPtr ctx, IntPtr model, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_const_interp");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetConstInterpDelegate>(funcPtr);
        return func(ctx, model, decl);
    }

    /// <summary>
    /// Retrieves the number of function interpretations in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <returns>Number of function declarations with interpretations in the model.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ModelGetNumFuncs(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_num_funcs");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetNumFuncsDelegate>(funcPtr);
        return func(ctx, model);
    }

    /// <summary>
    /// Retrieves function declaration at specified index in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="i">Index of the function (must be less than ModelGetNumFuncs).</param>
    /// <returns>Handle to the function declaration at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetFuncDecl(IntPtr ctx, IntPtr model, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetFuncDeclDelegate>(funcPtr);
        return func(ctx, model, i);
    }

    /// <summary>
    /// Retrieves the interpretation of a function in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="decl">The function declaration to get interpretation for.</param>
    /// <returns>Handle to the function interpretation, or IntPtr.Zero if no interpretation.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetFuncInterp(IntPtr ctx, IntPtr model, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_func_interp");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetFuncInterpDelegate>(funcPtr);
        return func(ctx, model, decl);
    }

    /// <summary>
    /// Checks if a declaration has an interpretation in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="decl">The declaration to check for interpretation.</param>
    /// <returns>True if the declaration has an interpretation in the model, false otherwise.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool ModelHasInterp(IntPtr ctx, IntPtr model, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_model_has_interp");
        var func = Marshal.GetDelegateForFunctionPointer<ModelHasInterpDelegate>(funcPtr);
        return func(ctx, model, decl) != 0;
    }

    /// <summary>
    /// Retrieves the number of sorts with finite universes in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <returns>Number of uninterpreted sorts with finite interpretations in the model.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint ModelGetNumSorts(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_num_sorts");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetNumSortsDelegate>(funcPtr);
        return func(ctx, model);
    }

    /// <summary>
    /// Retrieves sort at specified index in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="i">Index of the sort (must be less than ModelGetNumSorts).</param>
    /// <returns>Handle to the sort at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetSort(IntPtr ctx, IntPtr model, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_sort");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetSortDelegate>(funcPtr);
        return func(ctx, model, i);
    }

    /// <summary>
    /// Retrieves the finite universe of a sort in the model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle.</param>
    /// <param name="sort">The sort to get the universe for.</param>
    /// <returns>Handle to an AST vector containing all elements in the sort's universe.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelGetSortUniverse(IntPtr ctx, IntPtr model, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_model_get_sort_universe");
        var func = Marshal.GetDelegateForFunctionPointer<ModelGetSortUniverseDelegate>(funcPtr);
        return func(ctx, model, sort);
    }

    /// <summary>
    /// Translates model from one context to another context.
    /// </summary>
    /// <param name="ctx">The source Z3 context handle.</param>
    /// <param name="model">The model handle to translate.</param>
    /// <param name="dstCtx">The destination Z3 context handle.</param>
    /// <returns>Handle to the translated model in the destination context.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ModelTranslate(IntPtr ctx, IntPtr model, IntPtr dstCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_model_translate");
        var func = Marshal.GetDelegateForFunctionPointer<ModelTranslateDelegate>(funcPtr);
        return func(ctx, model, dstCtx);
    }
}
