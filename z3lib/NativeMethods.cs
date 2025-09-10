using System.Runtime.InteropServices;

namespace z3lib;

public static class NativeMethods
{
    private static IntPtr libraryHandle;

    private static readonly Dictionary<string, IntPtr> FunctionPointers = new();

    public static void LoadLibrary(string libraryPath)
    {
        if (libraryHandle != IntPtr.Zero)
        {
            NativeLibrary.Free(libraryHandle);
            FunctionPointers.Clear();
        }

        libraryHandle = NativeLibrary.Load(libraryPath);

        // Load function pointers
        LoadFunction("Z3_mk_config");
        LoadFunction("Z3_del_config");
        LoadFunction("Z3_mk_context");
        LoadFunction("Z3_mk_context_rc");
        LoadFunction("Z3_del_context");
        LoadFunction("Z3_update_param_value");
        LoadFunction("Z3_inc_ref");
        LoadFunction("Z3_dec_ref");
        
        // Sort functions
        LoadFunction("Z3_mk_bool_sort");
        LoadFunction("Z3_mk_int_sort");
        LoadFunction("Z3_mk_real_sort");
        
        // Expression functions
        LoadFunction("Z3_mk_const");
        LoadFunction("Z3_mk_string_symbol");
        LoadFunction("Z3_mk_true");
        LoadFunction("Z3_mk_false");
        LoadFunction("Z3_mk_eq");
        LoadFunction("Z3_mk_and");
        LoadFunction("Z3_mk_or");
        LoadFunction("Z3_mk_not");
        LoadFunction("Z3_mk_add");
        LoadFunction("Z3_mk_sub");
        LoadFunction("Z3_mk_mul");
        LoadFunction("Z3_mk_div");
        LoadFunction("Z3_mk_lt");
        LoadFunction("Z3_mk_le");
        LoadFunction("Z3_mk_gt");
        LoadFunction("Z3_mk_ge");
        LoadFunction("Z3_mk_numeral");
        
        // Solver functions
        LoadFunction("Z3_mk_solver");
        LoadFunction("Z3_mk_simple_solver");
        LoadFunction("Z3_solver_inc_ref");
        LoadFunction("Z3_solver_dec_ref");
        LoadFunction("Z3_solver_assert");
        LoadFunction("Z3_solver_check");
        LoadFunction("Z3_solver_push");
        LoadFunction("Z3_solver_pop");
        LoadFunction("Z3_solver_get_model");
        LoadFunction("Z3_solver_get_reason_unknown");
        
        // Model functions
        LoadFunction("Z3_model_inc_ref");
        LoadFunction("Z3_model_dec_ref");
        LoadFunction("Z3_model_eval");
        LoadFunction("Z3_get_numeral_string");
        LoadFunction("Z3_get_numeral_int");
        LoadFunction("Z3_ast_to_string");
    }

    private static void LoadFunction(string functionName)
    {
        if (libraryHandle == IntPtr.Zero)
            throw new InvalidOperationException("Library not loaded. Call LoadLibrary first.");

        var functionPtr = NativeLibrary.GetExport(libraryHandle, functionName);
        FunctionPointers[functionName] = functionPtr;
    }

    private static IntPtr GetFunctionPointer(string functionName)
    {
        if (!FunctionPointers.TryGetValue(functionName, out var ptr))
            throw new InvalidOperationException($"Function {functionName} not loaded.");
        return ptr;
    }

    public static IntPtr Z3MkConfig()
    {
        var funcPtr = GetFunctionPointer("Z3_mk_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConfigDelegate>(funcPtr);
        return func();
    }

    public static void Z3DelConfig(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_del_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelConfigDelegate>(funcPtr);
        func(cfg);
    }

    public static IntPtr Z3MkContext(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextDelegate>(funcPtr);
        return func(cfg);
    }

    public static IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context_rc");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextRcDelegate>(funcPtr);
        return func(cfg);
    }

    public static void Z3DelContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_del_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelContextDelegate>(funcPtr);
        func(ctx);
    }

    public static void Z3UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_update_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3UpdateParamValueDelegate>(funcPtr);
        func(ctx, paramId, paramValue);
    }

    public static void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IncRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    public static void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DecRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    // Sort functions
    public static IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bool_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBoolSortDelegate>(funcPtr);
        return func(ctx);
    }

    public static IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIntSortDelegate>(funcPtr);
        return func(ctx);
    }

    public static IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRealSortDelegate>(funcPtr);
        return func(ctx);
    }

    // Expression functions
    public static IntPtr Z3MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstDelegate>(funcPtr);
        return func(ctx, symbol, sort);
    }

    public static IntPtr Z3MkStringSymbol(IntPtr ctx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_symbol");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStringSymbolDelegate>(funcPtr);
        return func(ctx, str);
    }

    public static IntPtr Z3MkTrue(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_true");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkTrueDelegate>(funcPtr);
        return func(ctx);
    }

    public static IntPtr Z3MkFalse(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_false");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkFalseDelegate>(funcPtr);
        return func(ctx);
    }

    public static IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_eq");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkEqDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_and");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAndDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    public static IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_or");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkOrDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    public static IntPtr Z3MkNot(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_not");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNotDelegate>(funcPtr);
        return func(ctx, arg);
    }

    public static IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_add");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAddDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    public static IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSubDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    public static IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkMulDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    public static IntPtr Z3MkDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_div");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkDivDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_le");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_gt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkNumeral(IntPtr ctx, IntPtr numeral, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_numeral");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNumeralDelegate>(funcPtr);
        return func(ctx, numeral, sort);
    }

    // Solver functions
    public static IntPtr Z3MkSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSolverDelegate>(funcPtr);
        return func(ctx);
    }

    public static IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_simple_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSimpleSolverDelegate>(funcPtr);
        return func(ctx);
    }

    public static void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverIncRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    public static void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverDecRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    public static void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr formula)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_assert");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverAssertDelegate>(funcPtr);
        func(ctx, solver, formula);
    }

    public static int Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_check");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverCheckDelegate>(funcPtr);
        return func(ctx, solver);
    }

    public static void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_push");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPushDelegate>(funcPtr);
        func(ctx, solver);
    }

    public static void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_pop");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPopDelegate>(funcPtr);
        func(ctx, solver, numScopes);
    }

    public static IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_model");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetModelDelegate>(funcPtr);
        return func(ctx, solver);
    }

    public static IntPtr Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_reason_unknown");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetReasonUnknownDelegate>(funcPtr);
        return func(ctx, solver);
    }

    // Model functions
    public static void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelIncRefDelegate>(funcPtr);
        func(ctx, model);
    }

    public static void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelDecRefDelegate>(funcPtr);
        func(ctx, model);
    }

    public static bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_model_eval");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelEvalDelegate>(funcPtr);
        return func(ctx, model, expr, modelCompletion, out result);
    }

    public static IntPtr Z3GetNumeralString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetNumeralStringDelegate>(funcPtr);
        return func(ctx, ast);
    }

    public static bool Z3GetNumeralInt(IntPtr ctx, IntPtr ast, out int value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetNumeralIntDelegate>(funcPtr);
        return func(ctx, ast, out value);
    }

    public static IntPtr Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3AstToStringDelegate>(funcPtr);
        return func(ctx, ast);
    }

    private delegate IntPtr Z3MkConfigDelegate();
    private delegate void Z3DelConfigDelegate(IntPtr cfg);
    private delegate IntPtr Z3MkContextDelegate(IntPtr cfg);
    private delegate IntPtr Z3MkContextRcDelegate(IntPtr cfg);
    private delegate void Z3DelContextDelegate(IntPtr ctx);
    private delegate void Z3UpdateParamValueDelegate(IntPtr ctx, IntPtr paramId, IntPtr paramValue);
    private delegate void Z3IncRefDelegate(IntPtr ctx, IntPtr ast);
    private delegate void Z3DecRefDelegate(IntPtr ctx, IntPtr ast);
    
    // Sort delegates
    private delegate IntPtr Z3MkBoolSortDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkIntSortDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkRealSortDelegate(IntPtr ctx);
    
    // Expression delegates
    private delegate IntPtr Z3MkConstDelegate(IntPtr ctx, IntPtr symbol, IntPtr sort);
    private delegate IntPtr Z3MkStringSymbolDelegate(IntPtr ctx, IntPtr str);
    private delegate IntPtr Z3MkTrueDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkFalseDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkEqDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkAndDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr Z3MkOrDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr Z3MkNotDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr Z3MkAddDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr Z3MkSubDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr Z3MkMulDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr Z3MkDivDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkLtDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkLeDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkGtDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkGeDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkNumeralDelegate(IntPtr ctx, IntPtr numeral, IntPtr sort);
    
    // Solver delegates
    private delegate IntPtr Z3MkSolverDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkSimpleSolverDelegate(IntPtr ctx);
    private delegate void Z3SolverIncRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverDecRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverAssertDelegate(IntPtr ctx, IntPtr solver, IntPtr formula);
    private delegate int Z3SolverCheckDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverPushDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverPopDelegate(IntPtr ctx, IntPtr solver, uint numScopes);
    private delegate IntPtr Z3SolverGetModelDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr Z3SolverGetReasonUnknownDelegate(IntPtr ctx, IntPtr solver);
    
    // Model delegates
    private delegate void Z3ModelIncRefDelegate(IntPtr ctx, IntPtr model);
    private delegate void Z3ModelDecRefDelegate(IntPtr ctx, IntPtr model);
    private delegate bool Z3ModelEvalDelegate(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result);
    private delegate IntPtr Z3GetNumeralStringDelegate(IntPtr ctx, IntPtr ast);
    private delegate bool Z3GetNumeralIntDelegate(IntPtr ctx, IntPtr ast, out int value);
    private delegate IntPtr Z3AstToStringDelegate(IntPtr ctx, IntPtr ast);
}