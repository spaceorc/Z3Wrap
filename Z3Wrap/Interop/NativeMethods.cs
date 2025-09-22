using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Interop;

/// <summary>
/// Provides native method bindings and library management for the Z3 theorem prover.
/// Handles dynamic loading of the Z3 library and provides type-safe access to Z3 C API functions.
/// </summary>
public static class NativeMethods
{
    private record LoadedLibrary(Dictionary<string, IntPtr> FunctionPointers, IntPtr LibraryHandle);

    private static LoadedLibrary? loadedLibrary;

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
        LoadLibrarySafe(libraryPath);
    }

    /// <summary>
    /// Automatically discovers and loads the Z3 library for the current platform.
    /// Searches common installation paths for each platform.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when library cannot be found or loaded.</exception>
    public static void LoadLibraryAuto()
    {
        if (loadedLibrary != null)
            return; // Fast path - already loaded

        var searchPaths = GetPlatformSearchPaths();
        var loadAttempts = new List<(string path, Exception exception)>();

        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    LoadLibrarySafe(path);
                    return; // Success - library is now loaded
                }
                catch (Exception ex)
                {
                    loadAttempts.Add((path, ex));
                }
            }
        }

        var searchedPaths = string.Join(", ", searchPaths);
        var attemptDetails =
            loadAttempts.Count != 0
                ? "\n\nLoad attempts:\n"
                    + string.Join(
                        "\n",
                        loadAttempts.Select(a => $"  {a.path}: {a.exception.Message}")
                    )
                : "";

        throw new InvalidOperationException(
            $"Could not automatically locate Z3 library. "
                + $"Searched paths: {searchedPaths}"
                + attemptDetails
                + "\n\nPlease ensure Z3 is installed or use LoadLibrary(path) to specify the library path explicitly."
        );
    }

    // Thread-safe library loading helper
    private static void LoadLibrarySafe(string libraryPath)
    {
        if (loadedLibrary != null)
            return; // Fast path - already loaded

        var newLibrary = LoadLibraryInternal(libraryPath);
        // Atomically set if still null, otherwise another thread already loaded
        if (Interlocked.CompareExchange(ref loadedLibrary, newLibrary, null) != null)
        {
            // Another thread already loaded - clean up our library handle
            NativeLibrary.Free(newLibrary.LibraryHandle);
        }
    }

    // Internal Library Loading
    private static LoadedLibrary LoadLibraryInternal(string libraryPath)
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
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvadd_no_underflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsub_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsub_no_underflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvmul_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvmul_no_underflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvsdiv_no_overflow");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_bvneg_no_overflow");
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

            // Quantifier functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_forall_const");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_exists_const");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_pattern");

            return new LoadedLibrary(functionPointers, handle);
        }
        catch
        {
            // If anything fails, clean up the loaded library
            NativeLibrary.Free(handle);
            throw;
        }
    }

    private static void LoadFunctionInternal(
        IntPtr libraryHandle,
        Dictionary<string, IntPtr> functionPointers,
        string functionName
    )
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
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Z3",
                    "bin",
                    "libz3.dll"
                ),
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Z3",
                    "bin",
                    "z3.dll"
                ),
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Z3",
                    "bin",
                    "libz3.dll"
                ),
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Z3",
                    "bin",
                    "z3.dll"
                ),
            ];
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return
            [
                "libz3.dylib",
                "z3.dylib",
                "/opt/homebrew/opt/z3/lib/libz3.dylib", // Apple Silicon Homebrew
                "/usr/local/opt/z3/lib/libz3.dylib", // Intel Homebrew
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
                "/usr/lib/x86_64-linux-gnu/libz3.so", // Ubuntu/Debian
                "/usr/lib/libz3.so", // General Linux
                "/usr/lib64/libz3.so", // 64-bit systems
                "/usr/local/lib/libz3.so", // User installations
                "/opt/z3/lib/libz3.so", // Custom installations
                "/snap/z3/current/lib/libz3.so", // Snap packages
            ];
        }

        // Fallback for other platforms
        return ["libz3.so", "libz3.dylib", "libz3.dll", "z3.so", "z3.dylib", "z3.dll"];
    }

    // Function Pointer Management
    private static IntPtr GetFunctionPointer(string functionName)
    {
        // Ensure library is loaded before trying to get function pointer
        if (loadedLibrary == null)
            LoadLibraryAuto();

        if (!loadedLibrary!.FunctionPointers.TryGetValue(functionName, out var ptr))
            throw new InvalidOperationException($"Function {functionName} not loaded.");
        return ptr;
    }

    internal static IntPtr Z3MkConfig()
    {
        var funcPtr = GetFunctionPointer("Z3_mk_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConfigDelegate>(funcPtr);
        return func();
    }

    internal static void Z3DelConfig(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_del_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelConfigDelegate>(funcPtr);
        func(cfg);
    }

    internal static IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context_rc");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextRcDelegate>(funcPtr);
        return func(cfg);
    }

    internal static void Z3DelContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_del_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelContextDelegate>(funcPtr);
        func(ctx);
    }

    internal static void Z3UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_update_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3UpdateParamValueDelegate>(funcPtr);
        func(ctx, paramId, paramValue);
    }

    internal static void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IncRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    internal static void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DecRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    // Sort functions
    internal static IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bool_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBoolSortDelegate>(funcPtr);
        return func(ctx);
    }

    internal static IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIntSortDelegate>(funcPtr);
        return func(ctx);
    }

    internal static IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRealSortDelegate>(funcPtr);
        return func(ctx);
    }

    // Expression functions
    internal static IntPtr Z3MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstDelegate>(funcPtr);
        return func(ctx, symbol, sort);
    }

    internal static IntPtr Z3MkStringSymbol(IntPtr ctx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_symbol");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStringSymbolDelegate>(funcPtr);
        return func(ctx, str);
    }

    internal static IntPtr Z3MkTrue(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_true");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkTrueDelegate>(funcPtr);
        return func(ctx);
    }

    internal static IntPtr Z3MkFalse(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_false");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkFalseDelegate>(funcPtr);
        return func(ctx);
    }

    internal static IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_eq");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkEqDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_and");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAndDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    internal static IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_or");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkOrDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    internal static IntPtr Z3MkNot(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_not");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNotDelegate>(funcPtr);
        return func(ctx, arg);
    }

    internal static IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_add");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAddDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    internal static IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSubDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    internal static IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkMulDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    internal static IntPtr Z3MkDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_div");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkDivDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_le");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_gt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkNumeral(IntPtr ctx, IntPtr numeral, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_numeral");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNumeralDelegate>(funcPtr);
        return func(ctx, numeral, sort);
    }

    // Extended boolean operations
    internal static IntPtr Z3MkImplies(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_implies");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkImpliesDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkIff(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_iff");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIffDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_xor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkXorDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    // Extended arithmetic operations
    internal static IntPtr Z3MkMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkModDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    internal static IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unary_minus");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkUnaryMinusDelegate>(funcPtr);
        return func(ctx, arg);
    }

    internal static IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ite");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIteDelegate>(funcPtr);
        return func(ctx, condition, thenExpr, elseExpr);
    }

    // Type conversion functions
    internal static IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2real");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2RealDelegate>(funcPtr);
        return func(ctx, t1);
    }

    internal static IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkReal2IntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    // Array theory functions
    internal static IntPtr Z3MkArraySort(IntPtr ctx, IntPtr domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkArraySortDelegate>(funcPtr);
        return func(ctx, domain, range);
    }

    internal static IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_select");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSelectDelegate>(funcPtr);
        return func(ctx, array, index);
    }

    internal static IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_store");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStoreDelegate>(funcPtr);
        return func(ctx, array, index, value);
    }

    internal static IntPtr Z3MkConstArray(IntPtr ctx, IntPtr domain, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const_array");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstArrayDelegate>(funcPtr);
        return func(ctx, domain, value);
    }

    internal static IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_domain");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortDomainDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    internal static IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_range");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortRangeDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    // Bitvector theory methods
    internal static IntPtr Z3MkBvSort(IntPtr ctx, uint sz)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSortDelegate>(funcPtr);
        return func(ctx, sz);
    }

    internal static IntPtr Z3MkBvAdd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSub(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvMul(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvUDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvudiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvURem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvurem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvURemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSRem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsrem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSRemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSMod(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsmod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSModDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvAnd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvand");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAndDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvOr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvOrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvxor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvXorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvNot(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnot");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNotDelegate>(funcPtr);
        return func(ctx, t1);
    }

    internal static IntPtr Z3MkBvNeg(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNegDelegate>(funcPtr);
        return func(ctx, t1);
    }

    internal static IntPtr Z3MkBvShl(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvshl");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvShlDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvLShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvlshr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvLShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvAShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvashr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvULt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvult");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvULtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvslt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSLtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvULe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvule");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvULeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsle");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSLeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvUGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvugt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsgt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvUGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvuge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkSignExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sign_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSignExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    internal static IntPtr Z3MkZeroExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_zero_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkZeroExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    internal static IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_extract");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkExtractDelegate>(funcPtr);
        return func(ctx, high, low, t1);
    }

    internal static IntPtr Z3MkRepeat(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_repeat");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRepeatDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    internal static IntPtr Z3MkBv2Int(IntPtr ctx, IntPtr t1, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBv2IntDelegate>(funcPtr);
        return func(ctx, t1, signed);
    }

    internal static IntPtr Z3MkInt2Bv(IntPtr ctx, uint n, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2bv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2BvDelegate>(funcPtr);
        return func(ctx, n, t1);
    }

    internal static IntPtr Z3MkBvAddNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    internal static IntPtr Z3MkBvSubNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSubNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    internal static IntPtr Z3MkBvMulNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    internal static IntPtr Z3MkBvMulNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvAddNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvSDivNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvDivNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    internal static IntPtr Z3MkBvNegNoOverflow(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNegNoOverflowDelegate>(funcPtr);
        return func(ctx, t1);
    }

    internal static uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bv_sort_size");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBvSortSizeDelegate>(funcPtr);
        return func(ctx, sort);
    }

    // Solver functions
    internal static IntPtr Z3MkSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSolverDelegate>(funcPtr);
        return func(ctx);
    }

    internal static IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_simple_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSimpleSolverDelegate>(funcPtr);
        return func(ctx);
    }

    internal static void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverIncRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    internal static void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverDecRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    internal static void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr formula)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_assert");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverAssertDelegate>(funcPtr);
        func(ctx, solver, formula);
    }

    internal static int Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_check");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverCheckDelegate>(funcPtr);
        return func(ctx, solver);
    }

    internal static void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_push");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPushDelegate>(funcPtr);
        func(ctx, solver);
    }

    internal static void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_pop");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPopDelegate>(funcPtr);
        func(ctx, solver, numScopes);
    }

    internal static void Z3SolverReset(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_reset");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverResetDelegate>(funcPtr);
        func(ctx, solver);
    }

    internal static IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_model");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetModelDelegate>(funcPtr);
        return func(ctx, solver);
    }

    internal static IntPtr Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_reason_unknown");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetReasonUnknownDelegate>(funcPtr);
        return func(ctx, solver);
    }

    // Model functions
    internal static void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelIncRefDelegate>(funcPtr);
        func(ctx, model);
    }

    internal static void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelDecRefDelegate>(funcPtr);
        func(ctx, model);
    }

    internal static IntPtr Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelToStringDelegate>(funcPtr);
        return func(ctx, model);
    }

    internal static IntPtr Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3AstToStringDelegate>(funcPtr);
        return func(ctx, ast);
    }

    internal static bool Z3ModelEval(
        IntPtr ctx,
        IntPtr model,
        IntPtr expr,
        bool modelCompletion,
        out IntPtr result
    )
    {
        var funcPtr = GetFunctionPointer("Z3_model_eval");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelEvalDelegate>(funcPtr);
        return func(ctx, model, expr, modelCompletion ? 1 : 0, out result) != 0;
    }

    internal static IntPtr Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetNumeralStringDelegate>(funcPtr);
        return func(ctx, expr);
    }

    internal static int Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bool_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBoolValueDelegate>(funcPtr);
        return func(ctx, expr);
    }

    internal static bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_is_numeral_ast");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IsNumeralAstDelegate>(funcPtr);
        return func(ctx, expr) != 0;
    }

    internal static IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortDelegate>(funcPtr);
        return func(ctx, expr);
    }

    internal static int Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_kind");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortKindDelegate>(funcPtr);
        return func(ctx, sort);
    }

    // Quantifier functions
    internal static IntPtr Z3MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_forall_const");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkForallConstDelegate>(funcPtr);
        return func(ctx, weight, numBound, bound, numPatterns, patterns, body);
    }

    internal static IntPtr Z3MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var funcPtr = GetFunctionPointer("Z3_mk_exists_const");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkExistsConstDelegate>(funcPtr);
        return func(ctx, weight, numBound, bound, numPatterns, patterns, body);
    }

    internal static IntPtr Z3MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pattern");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkPatternDelegate>(funcPtr);
        return func(ctx, numPatterns, terms);
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
    private delegate IntPtr Z3MkIteDelegate(
        IntPtr ctx,
        IntPtr condition,
        IntPtr thenExpr,
        IntPtr elseExpr
    );

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
    private delegate IntPtr Z3MkBvAddDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSubDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvMulDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvUDivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSDivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvURemDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSRemDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSModDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvAndDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvOrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvXorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvNotDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr Z3MkBvNegDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr Z3MkBvShlDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvLShrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvAShrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvULtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSLtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvULeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSLeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvUGtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSGtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvUGeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSGeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkSignExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkZeroExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkExtractDelegate(IntPtr ctx, uint high, uint low, IntPtr t1);
    private delegate IntPtr Z3MkRepeatDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr Z3MkBv2IntDelegate(IntPtr ctx, IntPtr t1, bool signed);
    private delegate IntPtr Z3MkInt2BvDelegate(IntPtr ctx, uint n, IntPtr t1);
    private delegate IntPtr Z3MkBvAddNoOverflowDelegate(
        IntPtr ctx,
        IntPtr t1,
        IntPtr t2,
        bool signed
    );
    private delegate IntPtr Z3MkBvAddNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSubNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSubNoUnderflowDelegate(
        IntPtr ctx,
        IntPtr t1,
        IntPtr t2,
        bool signed
    );
    private delegate IntPtr Z3MkBvMulNoOverflowDelegate(
        IntPtr ctx,
        IntPtr t1,
        IntPtr t2,
        bool signed
    );
    private delegate IntPtr Z3MkBvMulNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvDivNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvNegNoOverflowDelegate(IntPtr ctx, IntPtr t1);
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
    private delegate int Z3ModelEvalDelegate(
        IntPtr ctx,
        IntPtr model,
        IntPtr expr,
        int modelCompletion,
        out IntPtr result
    );
    private delegate IntPtr Z3GetNumeralStringDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3GetBoolValueDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3IsNumeralAstDelegate(IntPtr ctx, IntPtr expr);
    private delegate IntPtr Z3GetSortDelegate(IntPtr ctx, IntPtr expr);
    private delegate int Z3GetSortKindDelegate(IntPtr ctx, IntPtr sort);

    // Quantifier delegates
    private delegate IntPtr Z3MkForallConstDelegate(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    );
    private delegate IntPtr Z3MkExistsConstDelegate(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    );
    private delegate IntPtr Z3MkPatternDelegate(IntPtr ctx, uint numPatterns, IntPtr[] terms);
}
