using System.Runtime.InteropServices;

namespace Z3Wrap.Interop;

public static class NativeMethods
{
    private static Dictionary<string, IntPtr>? loadedFunctionPointers;

    // Public API Methods
    /// <summary>
    /// Loads the Z3 library from the specified path.
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 library file.</param>
    /// <exception cref="DllNotFoundException">Thrown when the library cannot be found at the specified path.</exception>
    /// <exception cref="BadImageFormatException">Thrown when the library is not a valid native library.</exception>
    /// <exception cref="InvalidOperationException">Thrown when required Z3 functions cannot be loaded from the library.</exception>
    public static void LoadLibrary(string libraryPath)
    {
        if (loadedFunctionPointers != null)
            return; // Already loaded

        loadedFunctionPointers = LoadLibraryInternal(libraryPath);
    }

    /// <summary>
    /// Automatically discovers and loads the Z3 library for the current platform.
    /// Searches common installation paths for each platform.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when library cannot be found or loaded.</exception>
    public static void LoadLibraryAuto()
    {
        if (loadedFunctionPointers != null)
            return; // Already loaded
            
        var searchPaths = GetPlatformSearchPaths();
        
        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    loadedFunctionPointers = LoadLibraryInternal(path);
                    return; // Success
                }
                catch
                {
                    // Try next path if this one fails
                }
            }
        }
        
        throw new InvalidOperationException(
            "Could not automatically locate Z3 library. " +
            "Please ensure Z3 is installed or use LoadLibrary(path) to specify the library path explicitly.");
    }

    // Internal Library Loading
    private static Dictionary<string, IntPtr> LoadLibraryInternal(string libraryPath)
    {
        var handle = NativeLibrary.Load(libraryPath);
        var functionPointers = new Dictionary<string, IntPtr>();
        
        try
        {
            // Load all function pointers
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_config");
            LoadFunctionInternal(handle, functionPointers, "Z3_del_config");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_context_rc");
            LoadFunctionInternal(handle, functionPointers, "Z3_del_context");
            LoadFunctionInternal(handle, functionPointers, "Z3_update_param_value");
            LoadFunctionInternal(handle, functionPointers, "Z3_inc_ref");
            LoadFunctionInternal(handle, functionPointers, "Z3_dec_ref");
            
            // Sort functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bool_sort");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_int_sort");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_real_sort");
            
            // Expression functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_const");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_string_symbol");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_true");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_false");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_eq");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_and");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_or");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_not");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_add");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_sub");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_mul");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_div");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_lt");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_le");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_gt");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_ge");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_numeral");
            
            // Extended boolean operations
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_implies");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_iff");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_xor");
            
            // Extended arithmetic operations
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_mod");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_unary_minus");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_ite");
            
            // Type conversion functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_int2real");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_real2int");
            
            // Array theory functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_array_sort");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_select");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_store");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_const_array");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_array_sort_domain");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_array_sort_range");

            // Bitvector functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bv_sort");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvadd");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsub");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvmul");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvudiv");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsdiv");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvurem");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsrem");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsmod");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvand");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvor");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvxor");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvnot");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvneg");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvshl");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvlshr");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvashr");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvult");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvslt");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvule");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsle");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvugt");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsgt");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvuge");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsge");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_sign_ext");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_zero_ext");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_extract");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_repeat");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bv2int");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_int2bv");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvadd_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsub_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsub_no_underflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvmul_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvmul_no_underflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_bv_sort_size");

            // Solver functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_solver");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_simple_solver");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_inc_ref");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_dec_ref");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_assert");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_check");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_push");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_pop");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_reset");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_get_model");
            LoadFunctionInternal(handle, functionPointers, "Z3_solver_get_reason_unknown");
            
            // Model functions
            LoadFunctionInternal(handle, functionPointers, "Z3_model_inc_ref");
            LoadFunctionInternal(handle, functionPointers, "Z3_model_dec_ref");
            LoadFunctionInternal(handle, functionPointers, "Z3_model_to_string");
            LoadFunctionInternal(handle, functionPointers, "Z3_ast_to_string");
            LoadFunctionInternal(handle, functionPointers, "Z3_model_eval");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_numeral_string");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_bool_value");
            LoadFunctionInternal(handle, functionPointers, "Z3_is_numeral_ast");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_sort");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_sort_kind");
            
            // Once all functions are loaded successfully, we can return the pointers
            // The library handle is no longer needed - function pointers keep the library loaded
            return functionPointers;
        }
        catch
        {
            // If anything fails, clean up the loaded library
            NativeLibrary.Free(handle);
            throw;
        }
    }
    
    private static void LoadFunctionInternal(IntPtr libraryHandle, Dictionary<string, IntPtr> functionPointers, string functionName)
    {
        var functionPtr = NativeLibrary.GetExport(libraryHandle, functionName);
        functionPointers[functionName] = functionPtr;
    }

    private static string[] GetPlatformSearchPaths()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return
            [
                "libz3.dll",
                "z3.dll",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Z3", "bin", "libz3.dll"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Z3", "bin", "z3.dll"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Z3", "bin", "libz3.dll"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Z3", "bin", "z3.dll"),
            ];
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return
            [
                "libz3.dylib",
                "z3.dylib",
                "/opt/homebrew/opt/z3/lib/libz3.dylib",  // Apple Silicon Homebrew
                "/usr/local/opt/z3/lib/libz3.dylib",     // Intel Homebrew
                "/opt/homebrew/lib/libz3.dylib",
                "/usr/local/lib/libz3.dylib",
                "/usr/lib/libz3.dylib",
                "/System/Library/Frameworks/libz3.dylib",
            ];
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return
            [
                "libz3.so",
                "z3.so",
                "/usr/lib/x86_64-linux-gnu/libz3.so",    // Ubuntu/Debian
                "/usr/lib/libz3.so",                     // General Linux
                "/usr/lib64/libz3.so",                   // 64-bit systems
                "/usr/local/lib/libz3.so",               // User installations
                "/opt/z3/lib/libz3.so",                  // Custom installations
                "/snap/z3/current/lib/libz3.so",         // Snap packages
            ];
        }
        
        // Fallback for other platforms
        return ["libz3.so", "libz3.dylib", "libz3.dll", "z3.so", "z3.dylib", "z3.dll"];
    }

    // Function Pointer Management
    private static IntPtr GetFunctionPointer(string functionName)
    {
        // Ensure library is loaded before trying to get function pointer
        if (loadedFunctionPointers == null) 
            LoadLibraryAuto();
        
        if (!loadedFunctionPointers!.TryGetValue(functionName, out var ptr))
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

    // Extended boolean operations
    public static IntPtr Z3MkImplies(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_implies");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkImpliesDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkIff(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_iff");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIffDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_xor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkXorDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    // Extended arithmetic operations
    public static IntPtr Z3MkMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkModDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    public static IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unary_minus");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkUnaryMinusDelegate>(funcPtr);
        return func(ctx, arg);
    }

    public static IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ite");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIteDelegate>(funcPtr);
        return func(ctx, condition, thenExpr, elseExpr);
    }

    // Type conversion functions
    public static IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2real");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2RealDelegate>(funcPtr);
        return func(ctx, t1);
    }

    public static IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkReal2IntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    // Array theory functions
    public static IntPtr Z3MkArraySort(IntPtr ctx, IntPtr domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkArraySortDelegate>(funcPtr);
        return func(ctx, domain, range);
    }

    public static IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_select");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSelectDelegate>(funcPtr);
        return func(ctx, array, index);
    }

    public static IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_store");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStoreDelegate>(funcPtr);
        return func(ctx, array, index, value);
    }

    public static IntPtr Z3MkConstArray(IntPtr ctx, IntPtr domain, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const_array");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstArrayDelegate>(funcPtr);
        return func(ctx, domain, value);
    }

    public static IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_domain");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortDomainDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    public static IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_range");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortRangeDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    // Bitvector theory methods
    public static IntPtr Z3MkBvSort(IntPtr ctx, uint sz)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSortDelegate>(funcPtr);
        return func(ctx, sz);
    }

    public static IntPtr Z3MkBvadd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvaddDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsub(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsubDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvmul(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvmulDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvudiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvudiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvudivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsdiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsdivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvurem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvurem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvuremDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsrem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsrem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsremDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsmod(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsmod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsmodDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvand(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvand");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvandDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvxor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvxor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvxorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvnot(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnot");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvnotDelegate>(funcPtr);
        return func(ctx, t1);
    }

    public static IntPtr Z3MkBvneg(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvnegDelegate>(funcPtr);
        return func(ctx, t1);
    }

    public static IntPtr Z3MkBvshl(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvshl");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvshlDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvlshr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvlshr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvlshrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvashr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvashr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvashrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvult(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvult");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvultDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvslt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvslt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsltDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvule(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvule");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvuleDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsle(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsle");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsleDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvugt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvugt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvugtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsgt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsgt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsgtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvuge(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvuge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvugeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsge(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsgeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkSignExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sign_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSignExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    public static IntPtr Z3MkZeroExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_zero_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkZeroExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    public static IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_extract");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkExtractDelegate>(funcPtr);
        return func(ctx, high, low, t1);
    }

    public static IntPtr Z3MkRepeat(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_repeat");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRepeatDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    public static IntPtr Z3MkBv2int(IntPtr ctx, IntPtr t1, bool isSigned)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBv2intDelegate>(funcPtr);
        return func(ctx, t1, isSigned);
    }

    public static IntPtr Z3MkInt2bv(IntPtr ctx, uint n, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2bv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2bvDelegate>(funcPtr);
        return func(ctx, n, t1);
    }

    public static IntPtr Z3MkBvaddNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvaddNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, isSigned);
    }

    public static IntPtr Z3MkBvsubNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsubNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static IntPtr Z3MkBvsubNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvsubNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2, isSigned);
    }

    public static IntPtr Z3MkBvmulNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvmulNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, isSigned);
    }

    public static IntPtr Z3MkBvmulNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvmulNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    public static uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bv_sort_size");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBvSortSizeDelegate>(funcPtr);
        return func(ctx, sort);
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

    public static void Z3SolverReset(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_reset");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverResetDelegate>(funcPtr);
        func(ctx, solver);
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

    public static IntPtr Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelToStringDelegate>(funcPtr);
        return func(ctx, model);
    }

    public static IntPtr Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3AstToStringDelegate>(funcPtr);
        return func(ctx, ast);
    }

    public static bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_model_eval");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelEvalDelegate>(funcPtr);
        return func(ctx, model, expr, modelCompletion ? 1 : 0, out result) != 0;
    }

    public static IntPtr Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetNumeralStringDelegate>(funcPtr);
        return func(ctx, expr);
    }


    public static int Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bool_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBoolValueDelegate>(funcPtr);
        return func(ctx, expr);
    }

    public static bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_is_numeral_ast");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IsNumeralAstDelegate>(funcPtr);
        return func(ctx, expr) != 0;
    }

    public static IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortDelegate>(funcPtr);
        return func(ctx, expr);
    }

    public static int Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_kind");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortKindDelegate>(funcPtr);
        return func(ctx, sort);
    }

    private delegate IntPtr Z3MkConfigDelegate();
    private delegate void Z3DelConfigDelegate(IntPtr cfg);
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
    
    // Extended boolean operation delegates
    private delegate IntPtr Z3MkImpliesDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkIffDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkXorDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    
    // Extended arithmetic operation delegates
    private delegate IntPtr Z3MkModDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr Z3MkUnaryMinusDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr Z3MkIteDelegate(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr);
    
    // Type conversion delegates
    private delegate IntPtr Z3MkInt2RealDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr Z3MkReal2IntDelegate(IntPtr ctx, IntPtr t1);
    
    // Array theory delegates
    private delegate IntPtr Z3MkArraySortDelegate(IntPtr ctx, IntPtr domain, IntPtr range);
    private delegate IntPtr Z3MkSelectDelegate(IntPtr ctx, IntPtr array, IntPtr index);
    private delegate IntPtr Z3MkStoreDelegate(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value);
    private delegate IntPtr Z3MkConstArrayDelegate(IntPtr ctx, IntPtr domain, IntPtr value);
    private delegate IntPtr Z3GetArraySortDomainDelegate(IntPtr ctx, IntPtr arraySort);
    private delegate IntPtr Z3GetArraySortRangeDelegate(IntPtr ctx, IntPtr arraySort);

    // Bitvector theory delegates
    private delegate IntPtr Z3MkBvSortDelegate(IntPtr ctx, uint sz);
    private delegate IntPtr Z3MkBvaddDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsubDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvmulDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvudivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsdivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvuremDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsremDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsmodDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvandDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvxorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvnotDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr Z3MkBvnegDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr Z3MkBvshlDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvlshrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvashrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvultDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsltDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvuleDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsleDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvugtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsgtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvugeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsgeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkSignExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkZeroExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkExtractDelegate(IntPtr ctx, uint high, uint low, IntPtr t1);
    private delegate IntPtr Z3MkRepeatDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkBv2intDelegate(IntPtr ctx, IntPtr t1, bool isSigned);
    private delegate IntPtr Z3MkInt2bvDelegate(IntPtr ctx, uint n, IntPtr t1);
    private delegate IntPtr Z3MkBvaddNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned);
    private delegate IntPtr Z3MkBvsubNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvsubNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned);
    private delegate IntPtr Z3MkBvmulNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool isSigned);
    private delegate IntPtr Z3MkBvmulNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate uint Z3GetBvSortSizeDelegate(IntPtr ctx, IntPtr sort);
    
    // Solver delegates
    private delegate IntPtr Z3MkSolverDelegate(IntPtr ctx);
    private delegate IntPtr Z3MkSimpleSolverDelegate(IntPtr ctx);
    private delegate void Z3SolverIncRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverDecRefDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverAssertDelegate(IntPtr ctx, IntPtr solver, IntPtr formula);
    private delegate int Z3SolverCheckDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverPushDelegate(IntPtr ctx, IntPtr solver);
    private delegate void Z3SolverPopDelegate(IntPtr ctx, IntPtr solver, uint numScopes);
    private delegate void Z3SolverResetDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr Z3SolverGetModelDelegate(IntPtr ctx, IntPtr solver);
    private delegate IntPtr Z3SolverGetReasonUnknownDelegate(IntPtr ctx, IntPtr solver);
    
    // Model delegates
    private delegate void Z3ModelIncRefDelegate(IntPtr ctx, IntPtr model);
    private delegate void Z3ModelDecRefDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr Z3ModelToStringDelegate(IntPtr ctx, IntPtr model);
    private delegate IntPtr Z3AstToStringDelegate(IntPtr ctx, IntPtr ast);
    private delegate int Z3ModelEvalDelegate(IntPtr ctx, IntPtr model, IntPtr expr, int modelCompletion, out IntPtr result);
    private delegate IntPtr Z3GetNumeralStringDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3GetBoolValueDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3IsNumeralAstDelegate(IntPtr ctx, IntPtr expr);
    private delegate IntPtr Z3GetSortDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3GetSortKindDelegate(IntPtr ctx, IntPtr sort);
}