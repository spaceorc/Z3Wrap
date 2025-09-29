using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed class NativeLibrary : IDisposable
{
    private record LoadedLibrary(Dictionary<string, IntPtr> FunctionPointers, IntPtr LibraryHandle);

    private readonly LoadedLibrary loadedLibrary;
    private bool disposed;

    internal string LibraryPath { get; }

    private NativeLibrary(string libraryPath, LoadedLibrary loadedLibrary)
    {
        LibraryPath = libraryPath;
        this.loadedLibrary = loadedLibrary;
    }

    internal static NativeLibrary Load(string libraryPath)
    {
        if (string.IsNullOrWhiteSpace(libraryPath))
            throw new ArgumentException("Library path cannot be null, empty, or whitespace", nameof(libraryPath));

        if (!File.Exists(libraryPath))
            throw new FileNotFoundException($"Z3 library not found at path: {libraryPath}", libraryPath);

        var loadedLib = LoadLibraryInternal(libraryPath);
        return new NativeLibrary(libraryPath, loadedLib);
    }

    internal static NativeLibrary LoadAuto()
    {
        var searchPaths = GetPlatformSearchPaths();
        var loadAttempts = new List<(string path, Exception exception)>();

        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                try
                {
                    var loadedLib = LoadLibraryInternal(path);
                    return new NativeLibrary(path, loadedLib);
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
                    + string.Join("\n", loadAttempts.Select(a => $"  {a.path}: {a.exception.Message}"))
                : "";

        throw new InvalidOperationException(
            $"Could not automatically locate Z3 library. "
                + $"Searched paths: {searchedPaths}"
                + attemptDetails
                + "\n\nPlease ensure Z3 is installed or use Load(path) to specify the library path explicitly."
        );
    }

    ~NativeLibrary()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (disposed)
            return;

        System.Runtime.InteropServices.NativeLibrary.Free(loadedLibrary.LibraryHandle);
        disposed = true;
        GC.SuppressFinalize(this);
    }

    // Internal Library Loading
    private static LoadedLibrary LoadLibraryInternal(string libraryPath)
    {
        var handle = System.Runtime.InteropServices.NativeLibrary.Load(libraryPath);
        var functionPointers = new Dictionary<string, IntPtr>();

        try
        {
            // Load all function pointers
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_config");
            LoadFunctionInternal(handle, functionPointers, "Z3_del_config");
            LoadFunctionInternal(handle, functionPointers, "Z3_set_param_value");
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

            // Function declaration and application
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_func_decl");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_app");

            // Quantifier functions
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_forall_const");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_exists_const");
            LoadFunctionInternal(handle, functionPointers, "Z3_mk_pattern");

            // Error handling functions
            LoadFunctionInternal(handle, functionPointers, "Z3_set_error_handler");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_error_code");
            LoadFunctionInternal(handle, functionPointers, "Z3_get_error_msg");

            return new LoadedLibrary(functionPointers, handle);
        }
        catch
        {
            // If anything fails, clean up the loaded library
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
            throw;
        }
    }

    private static void LoadFunctionInternal(
        IntPtr libraryHandle,
        Dictionary<string, IntPtr> functionPointers,
        string functionName
    )
    {
        var functionPtr = System.Runtime.InteropServices.NativeLibrary.GetExport(libraryHandle, functionName);
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
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Z3", "bin", "z3.dll"),
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
    private IntPtr GetFunctionPointer(string functionName)
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(NativeLibrary));

        if (!loadedLibrary.FunctionPointers.TryGetValue(functionName, out var ptr))
            throw new InvalidOperationException($"Function {functionName} not loaded.");
        return ptr;
    }

    /// <summary>
    /// Creates a Z3 configuration object that can be used to configure various solver settings.
    /// </summary>
    /// <returns>Handle to the created Z3 configuration object.</returns>
    /// <remarks>
    /// The configuration object must be deleted using Z3DelConfig when no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkConfig()
    {
        var funcPtr = GetFunctionPointer("Z3_mk_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConfigDelegate>(funcPtr);
        return func();
    }

    /// <summary>
    /// Deletes a Z3 configuration object and releases its memory.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle to delete.</param>
    /// <remarks>
    /// Should be called for every configuration object created with Z3MkConfig.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3DelConfig(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_del_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelConfigDelegate>(funcPtr);
        func(cfg);
    }

    /// <summary>
    /// Sets a configuration parameter value on a Z3 configuration object.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle.</param>
    /// <param name="paramId">The ANSI string pointer for parameter name.</param>
    /// <param name="paramValue">The ANSI string pointer for parameter value.</param>
    /// <remarks>
    /// Must be called before creating a context with the configuration.
    /// Some parameters can only be set at context creation time.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3SetParamValue(IntPtr cfg, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_set_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SetParamValueDelegate>(funcPtr);
        func(cfg, paramId, paramValue);
    }

    /// <summary>
    /// Creates a Z3 context with reference counting enabled for automatic memory management.
    /// </summary>
    /// <param name="cfg">The Z3 configuration handle to use for context creation.</param>
    /// <returns>Handle to the created Z3 context with reference counting.</returns>
    /// <remarks>
    /// Reference counting automatically manages memory for AST nodes created within this context.
    /// The context must be deleted using Z3DelContext when no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context_rc");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextRcDelegate>(funcPtr);
        return func(cfg);
    }

    /// <summary>
    /// Deletes a Z3 context and releases all associated memory.
    /// </summary>
    /// <param name="ctx">The Z3 context handle to delete.</param>
    /// <remarks>
    /// All objects created within this context become invalid after deletion.
    /// Should be called for every context created with Z3MkContextRc.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3DelContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_del_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelContextDelegate>(funcPtr);
        func(ctx);
    }

    /// <summary>
    /// Updates a parameter value in the Z3 context configuration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="paramId">The parameter identifier to update.</param>
    /// <param name="paramValue">The new parameter value to set.</param>
    /// <remarks>
    /// Used to dynamically modify Z3 solver behavior and optimization settings.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_update_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3UpdateParamValueDelegate>(funcPtr);
        func(ctx, paramId, paramValue);
    }

    /// <summary>
    /// Increments the reference counter of a Z3 AST node to prevent premature deallocation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The Z3 AST node handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Must be paired with Z3DecRef
    /// when the AST node is no longer needed to prevent memory leaks.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IncRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    /// <summary>
    /// Decrements the reference counter of a Z3 AST node, potentially allowing its deallocation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The Z3 AST node handle to decrement reference count for.</param>
    /// <remarks>
    /// Should be called for every Z3IncRef to properly manage memory and prevent leaks.
    /// When reference count reaches zero, the AST node may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DecRefDelegate>(funcPtr);
        func(ctx, ast);
    }

    // Sort functions
    /// <summary>
    /// Creates a Boolean sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Boolean sort.</returns>
    /// <remarks>
    /// Boolean sorts are used for creating Boolean expressions and constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bool_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBoolSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates an integer sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created integer sort.</returns>
    /// <remarks>
    /// Integer sorts are used for creating integer expressions and arithmetic constraints.
    /// Z3 integers have unlimited precision (BigInteger semantics).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIntSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a real number sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created real sort.</returns>
    /// <remarks>
    /// Real sorts are used for creating real number expressions and arithmetic constraints.
    /// Z3 reals support exact rational arithmetic with unlimited precision.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRealSortDelegate>(funcPtr);
        return func(ctx);
    }

    // Expression functions
    /// <summary>
    /// Creates a Z3 constant expression with the specified name and sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The symbol name for the constant.</param>
    /// <param name="sort">The sort (type) of the constant.</param>
    /// <returns>Handle to the created constant expression.</returns>
    /// <remarks>
    /// Constants are free variables that can be assigned values during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstDelegate>(funcPtr);
        return func(ctx, symbol, sort);
    }

    /// <summary>
    /// Creates a Z3 symbol from a string name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="str">Pointer to the null-terminated string name.</param>
    /// <returns>Handle to the created Z3 symbol.</returns>
    /// <remarks>
    /// Symbols are used to name constants, functions, and other Z3 objects.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkStringSymbol(IntPtr ctx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_symbol");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStringSymbolDelegate>(funcPtr);
        return func(ctx, str);
    }

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant true.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created true Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkTrue(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_true");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkTrueDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant false.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created false Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkFalse(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_false");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkFalseDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 equality expression between two terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side expression.</param>
    /// <param name="right">The right-hand side expression.</param>
    /// <returns>Handle to the created equality expression (left == right).</returns>
    /// <remarks>
    /// Both expressions must have the same sort for the equality to be valid.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_eq");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkEqDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 logical AND expression over multiple Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of Boolean expressions in the array.</param>
    /// <param name="args">Array of Boolean expressions to combine with AND.</param>
    /// <returns>Handle to the created AND expression.</returns>
    /// <remarks>
    /// All arguments must be Boolean expressions. Returns true if all arguments are true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_and");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAndDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 logical OR expression over multiple Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of Boolean expressions in the array.</param>
    /// <param name="args">Array of Boolean expressions to combine with OR.</param>
    /// <returns>Handle to the created OR expression.</returns>
    /// <remarks>
    /// All arguments must be Boolean expressions. Returns true if any argument is true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_or");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkOrDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 logical NOT expression that negates a Boolean term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The Boolean expression to negate.</param>
    /// <returns>Handle to the created NOT expression.</returns>
    /// <remarks>
    /// The argument must be a Boolean expression. Returns the logical negation of the input.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkNot(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_not");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNotDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates a Z3 addition expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions to add together.</param>
    /// <returns>Handle to the created addition expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// Supports unlimited precision arithmetic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_add");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAddDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 subtraction expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions for subtraction (left-associative).</param>
    /// <returns>Handle to the created subtraction expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// With multiple args, performs left-associative subtraction: ((a - b) - c) - d.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSubDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 multiplication expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions to multiply together.</param>
    /// <returns>Handle to the created multiplication expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// Supports unlimited precision arithmetic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkMulDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 division expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The dividend (numerator) expression.</param>
    /// <param name="right">The divisor (denominator) expression.</param>
    /// <returns>Handle to the created division expression (left / right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort. For integers, performs
    /// real division returning a rational result. Division by zero is undefined.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_div");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkDivDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 less-than comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &lt; right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 less-than-or-equal comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &lt;= right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_le");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkLeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 greater-than comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &gt; right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_gt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 greater-than-or-equal comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &gt;= right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkGeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 numeric literal expression from a string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numeral">Pointer to the null-terminated string representation of the number.</param>
    /// <param name="sort">The numeric sort (integer or real) for the literal.</param>
    /// <returns>Handle to the created numeric literal expression.</returns>
    /// <remarks>
    /// The numeral string format depends on the sort. Integers use decimal notation,
    /// reals can use decimal or fractional notation (e.g., "3.14" or "22/7").
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkNumeral(IntPtr ctx, IntPtr numeral, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_numeral");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkNumeralDelegate>(funcPtr);
        return func(ctx, numeral, sort);
    }

    // Extended boolean operations
    /// <summary>
    /// Creates a Z3 logical implication expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The antecedent (premise) Boolean expression.</param>
    /// <param name="right">The consequent (conclusion) Boolean expression.</param>
    /// <returns>Handle to the created implication expression (left =&gt; right).</returns>
    /// <remarks>
    /// Logical implication is false only when the antecedent is true and the consequent is false.
    /// Equivalent to (!left || right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkImplies(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_implies");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkImpliesDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 logical biconditional (if-and-only-if) expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side Boolean expression.</param>
    /// <param name="right">The right-hand side Boolean expression.</param>
    /// <returns>Handle to the created biconditional expression (left &lt;=&gt; right).</returns>
    /// <remarks>
    /// Biconditional is true when both sides have the same truth value.
    /// Equivalent to (left &amp;&amp; right) || (!left &amp;&amp; !right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkIff(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_iff");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIffDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 logical exclusive-or (XOR) expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side Boolean expression.</param>
    /// <param name="right">The right-hand side Boolean expression.</param>
    /// <returns>Handle to the created XOR expression (left XOR right).</returns>
    /// <remarks>
    /// XOR is true when exactly one of the two operands is true.
    /// Equivalent to (left &amp;&amp; !right) || (!left &amp;&amp; right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_xor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkXorDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    // Extended arithmetic operations
    /// <summary>
    /// Creates a Z3 modulo expression between two integer terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The dividend integer expression.</param>
    /// <param name="right">The divisor integer expression.</param>
    /// <returns>Handle to the created modulo expression (left mod right).</returns>
    /// <remarks>
    /// Both expressions must be integers. Returns the remainder of integer division.
    /// The result has the same sign as the divisor in Z3's modulo semantics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkModDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 unary minus (negation) expression for a numeric term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The numeric expression to negate.</param>
    /// <returns>Handle to the created unary minus expression (-arg).</returns>
    /// <remarks>
    /// The argument must be a numeric expression (integer or real).
    /// Returns the arithmetic negation of the input.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unary_minus");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkUnaryMinusDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates a Z3 if-then-else (conditional) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="condition">The Boolean condition expression.</param>
    /// <param name="thenExpr">The expression to return when condition is true.</param>
    /// <param name="elseExpr">The expression to return when condition is false.</param>
    /// <returns>Handle to the created conditional expression (condition ? thenExpr : elseExpr).</returns>
    /// <remarks>
    /// The condition must be Boolean, and both branches must have the same sort.
    /// Used for conditional logic and piecewise function definitions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ite");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkIteDelegate>(funcPtr);
        return func(ctx, condition, thenExpr, elseExpr);
    }

    // Type conversion functions
    /// <summary>
    /// Creates a Z3 type conversion expression from integer to real.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The integer expression to convert.</param>
    /// <returns>Handle to the created real expression with the same numeric value.</returns>
    /// <remarks>
    /// Converts an integer expression to its real number equivalent.
    /// The numeric value is preserved in the conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2real");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2RealDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 type conversion expression from real to integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The real expression to convert.</param>
    /// <returns>Handle to the created integer expression truncated towards zero.</returns>
    /// <remarks>
    /// Converts a real expression to integer by truncating towards zero.
    /// For example, 3.7 becomes 3, and -2.9 becomes -2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkReal2IntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    // Array theory functions
    /// <summary>
    /// Creates a Z3 array sort with specified domain and range sorts.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The sort for array indices (keys).</param>
    /// <param name="range">The sort for array values.</param>
    /// <returns>Handle to the created array sort.</returns>
    /// <remarks>
    /// Array sorts define mappings from domain elements to range elements.
    /// Used for creating array expressions and constants.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkArraySort(IntPtr ctx, IntPtr domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_array_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkArraySortDelegate>(funcPtr);
        return func(ctx, domain, range);
    }

    /// <summary>
    /// Creates a Z3 array select expression that reads a value at a given index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The array expression to read from.</param>
    /// <param name="index">The index expression specifying the position to read.</param>
    /// <returns>Handle to the created select expression (array[index]).</returns>
    /// <remarks>
    /// The index must match the array's domain sort, and the result has the array's range sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_select");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSelectDelegate>(funcPtr);
        return func(ctx, array, index);
    }

    /// <summary>
    /// Creates a Z3 array store expression that writes a value at a given index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="array">The original array expression.</param>
    /// <param name="index">The index expression specifying where to write.</param>
    /// <param name="value">The value expression to store at the index.</param>
    /// <returns>Handle to the created array expression with updated value (array[index := value]).</returns>
    /// <remarks>
    /// Creates a new array identical to the original except at the specified index.
    /// The index must match the domain sort and value must match the range sort.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_store");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkStoreDelegate>(funcPtr);
        return func(ctx, array, index, value);
    }

    /// <summary>
    /// Creates a Z3 constant array expression where all elements have the same value.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="domain">The sort for array indices (keys).</param>
    /// <param name="value">The constant value for all array elements.</param>
    /// <returns>Handle to the created constant array expression.</returns>
    /// <remarks>
    /// Creates an array where every possible index maps to the same value.
    /// Useful for initializing arrays with default values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkConstArray(IntPtr ctx, IntPtr domain, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const_array");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConstArrayDelegate>(funcPtr);
        return func(ctx, domain, value);
    }

    /// <summary>
    /// Retrieves the domain sort (index type) of an array sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arraySort">The array sort to query.</param>
    /// <returns>Handle to the domain sort of the array.</returns>
    /// <remarks>
    /// Returns the sort used for array indices. Used for type checking and sort queries.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_domain");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortDomainDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    /// <summary>
    /// Retrieves the range sort (value type) of an array sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arraySort">The array sort to query.</param>
    /// <returns>Handle to the range sort of the array.</returns>
    /// <remarks>
    /// Returns the sort used for array values. Used for type checking and sort queries.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr arraySort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_sort_range");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetArraySortRangeDelegate>(funcPtr);
        return func(ctx, arraySort);
    }

    // Bitvector theory methods
    /// <summary>
    /// Creates a Z3 bitvector sort with the specified bit width.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sz">The bit width of the bitvector (must be greater than 0).</param>
    /// <returns>Handle to the created bitvector sort.</returns>
    /// <remarks>
    /// Bitvector sorts represent fixed-width binary numbers used for modeling
    /// machine arithmetic and bitwise operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSort(IntPtr ctx, uint sz)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSortDelegate>(funcPtr);
        return func(ctx, sz);
    }

    /// <summary>
    /// Creates a Z3 bitvector addition expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitvector addition expression (t1 + t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Addition uses modular arithmetic
    /// with overflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvAdd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector subtraction expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The minuend bitvector operand.</param>
    /// <param name="t2">The subtrahend bitvector operand.</param>
    /// <returns>Handle to the created bitvector subtraction expression (t1 - t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Subtraction uses modular arithmetic
    /// with underflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSub(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector multiplication expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitvector multiplication expression (t1 * t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Multiplication uses modular arithmetic
    /// with overflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvMul(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned division expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created unsigned division expression (t1 /u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// Division by zero returns all 1s (maximum unsigned value).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvUDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvudiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed division expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed division expression (t1 /s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation. Division by zero has undefined behavior.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned remainder expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created unsigned remainder expression (t1 %u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// Remainder by zero returns the dividend unchanged.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvURem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvurem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvURemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed remainder expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed remainder expression (t1 %s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers.
    /// The result has the same sign as the dividend.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSRem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsrem");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSRemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed modulo expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed modulo expression (t1 mod t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. The result has the same sign as the divisor.
    /// Different from signed remainder in how negative numbers are handled.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSMod(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsmod");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSModDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise AND expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise AND expression (t1 &amp; t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise AND operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvAnd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvand");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAndDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise OR expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise OR expression (t1 | t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise OR operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvOr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvOrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise XOR expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise XOR expression (t1 ^ t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise XOR operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvxor");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvXorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise NOT expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to the created bitwise NOT expression (~t1).</returns>
    /// <remarks>
    /// Performs bitwise complement, flipping all bits in the bitvector.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvNot(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnot");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNotDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector arithmetic negation expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to the created arithmetic negation expression (-t1).</returns>
    /// <remarks>
    /// Performs two's complement negation, equivalent to (~t1 + 1).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvNeg(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNegDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector left shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift left.</param>
    /// <returns>Handle to the created left shift expression (t1 &lt;&lt; t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Fills with zeros from the right.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvShl(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvshl");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvShlDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector logical right shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift right.</param>
    /// <returns>Handle to the created logical right shift expression (t1 &gt;&gt;u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Fills with zeros from the left.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvLShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvlshr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvLShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector arithmetic right shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift right.</param>
    /// <returns>Handle to the created arithmetic right shift expression (t1 &gt;&gt;s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Preserves the sign bit when shifting.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvAShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvashr");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned less-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvULt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvult");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvULtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed less-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvslt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSLtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned less-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;=u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvULe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvule");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvULeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed less-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;=s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsle");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSLeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned greater-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 >u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvUGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvugt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed greater-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 >s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsgt");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned greater-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &gt;=u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvUGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvuge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvUGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed greater-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &gt;=s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsge");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector sign extension expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of bits to extend.</param>
    /// <param name="t1">The bitvector operand to extend.</param>
    /// <returns>Handle to the created sign-extended bitvector expression.</returns>
    /// <remarks>
    /// Extends the bitvector by replicating the sign bit (most significant bit) i times.
    /// The resulting bitvector has width = original_width + i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkSignExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sign_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSignExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector zero extension expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of bits to extend.</param>
    /// <param name="t1">The bitvector operand to extend.</param>
    /// <returns>Handle to the created zero-extended bitvector expression.</returns>
    /// <remarks>
    /// Extends the bitvector by adding i zero bits at the most significant positions.
    /// The resulting bitvector has width = original_width + i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkZeroExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_zero_ext");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkZeroExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector bit extraction expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="high">The highest bit index to extract (inclusive).</param>
    /// <param name="low">The lowest bit index to extract (inclusive).</param>
    /// <param name="t1">The bitvector operand to extract bits from.</param>
    /// <returns>Handle to the created bit extraction expression (t1[high:low]).</returns>
    /// <remarks>
    /// Extracts bits from position low to high (inclusive). The resulting bitvector
    /// has width = high - low + 1. Bit indexing starts from 0.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_extract");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkExtractDelegate>(funcPtr);
        return func(ctx, high, low, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector repeat expression that concatenates a bitvector with itself.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of times to repeat the bitvector.</param>
    /// <param name="t1">The bitvector operand to repeat.</param>
    /// <returns>Handle to the created repeat expression.</returns>
    /// <remarks>
    /// The resulting bitvector has width = original_width * i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkRepeat(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_repeat");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkRepeatDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector to integer conversion expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to convert.</param>
    /// <param name="signed">True for signed interpretation, false for unsigned.</param>
    /// <returns>Handle to the created integer conversion expression.</returns>
    /// <remarks>
    /// Converts a bitvector to its integer representation. When signed is true,
    /// uses two's complement interpretation for negative values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBv2Int(IntPtr ctx, IntPtr t1, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv2int");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBv2IntDelegate>(funcPtr);
        return func(ctx, t1, signed);
    }

    /// <summary>
    /// Creates a Z3 integer to bitvector conversion expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">The bit width of the resulting bitvector.</param>
    /// <param name="t1">The integer operand to convert.</param>
    /// <returns>Handle to the created bitvector conversion expression.</returns>
    /// <remarks>
    /// Converts an integer to a bitvector of width n. The integer value is taken modulo 2^n.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkInt2Bv(IntPtr ctx, uint n, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2bv");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkInt2BvDelegate>(funcPtr);
        return func(ctx, n, t1);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector addition does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <param name="signed">If true, checks signed overflow; if false, checks unsigned overflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 + t2 does not overflow.</returns>
    /// <remarks>
    /// For signed arithmetic, overflow occurs when the result exceeds the maximum or minimum representable value.
    /// For unsigned arithmetic, overflow occurs when the result cannot fit in the bitvector width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvAddNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector subtraction does not overflow in signed arithmetic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand (minuend).</param>
    /// <param name="t2">The second bitvector operand (subtrahend).</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 - t2 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// This predicate is useful for verification of arithmetic properties in signed bitvector operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSubNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector subtraction does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand (minuend).</param>
    /// <param name="t2">The second bitvector operand (subtrahend).</param>
    /// <param name="signed">If true, checks signed underflow; if false, checks unsigned underflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 - t2 does not underflow.</returns>
    /// <remarks>
    /// For signed arithmetic, underflow occurs when the result is less than the minimum representable value.
    /// For unsigned arithmetic, underflow occurs when t1 &lt; t2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSubNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvSubNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector multiplication does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <param name="signed">If true, checks signed overflow; if false, checks unsigned overflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 * t2 does not overflow.</returns>
    /// <remarks>
    /// For signed arithmetic, overflow occurs when the result exceeds the representable range.
    /// For unsigned arithmetic, overflow occurs when the result cannot fit in the bitvector width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvMulNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector multiplication does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 * t2 does not underflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed multiplication underflow occurs when the result is less than the minimum representable signed value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvMulNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvMulNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector addition does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 + t2 does not underflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed addition underflow occurs when the result is less than the minimum representable signed value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvAddNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvAddNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector division does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 / t2 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed division overflow occurs when dividing the minimum signed value by -1,
    /// which would result in a value that cannot be represented in the same bit width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvSDivNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvDivNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector negation does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if -t1 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed negation overflow occurs when negating the minimum signed value,
    /// as the positive equivalent cannot be represented in the same bit width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkBvNegNoOverflow(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkBvNegNoOverflowDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Retrieves the bit width of a bitvector sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The bitvector sort to query.</param>
    /// <returns>The bit width of the bitvector sort.</returns>
    /// <remarks>
    /// Used to determine the size of bitvector expressions for type checking and operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bv_sort_size");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBvSortSizeDelegate>(funcPtr);
        return func(ctx, sort);
    }

    // Solver functions
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
    internal IntPtr Z3MkSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSolverDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 simple solver instance with reduced functionality for basic satisfiability checking.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Z3 simple solver.</returns>
    /// <remarks>
    /// Simple solvers have fewer features but may be more efficient for basic use cases.
    /// Prefer Z3MkSolver for full functionality.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_simple_solver");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkSimpleSolverDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments the reference counter of the given solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. When you receive a solver object,
    /// increment its reference count to prevent premature deallocation. Must be paired
    /// with Z3SolverDecRef when the solver is no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverIncRefDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Decrements the reference counter of the given solver.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle to decrement reference count for.</param>
    /// <remarks>
    /// Must be paired with Z3SolverIncRef to properly manage memory. When reference
    /// count reaches zero, the solver may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverDecRefDelegate>(funcPtr);
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
    internal void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr formula)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_assert");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverAssertDelegate>(funcPtr);
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
    internal int Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_check");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverCheckDelegate>(funcPtr);
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
    internal void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_push");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPushDelegate>(funcPtr);
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
    internal void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_pop");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverPopDelegate>(funcPtr);
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
    internal void Z3SolverReset(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_reset");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverResetDelegate>(funcPtr);
        func(ctx, solver);
    }

    /// <summary>
    /// Retrieves a satisfying model from the solver after a successful satisfiability check.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Handle to the model, or null if no model is available.</returns>
    /// <remarks>
    /// Only valid after Z3SolverCheck returns satisfiable (1). The model contains
    /// variable assignments that satisfy all asserted constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_model");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetModelDelegate>(funcPtr);
        return func(ctx, solver);
    }

    /// <summary>
    /// Retrieves the reason why the solver returned an unknown result.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="solver">The solver handle.</param>
    /// <returns>Handle to a string describing the reason for unknown result.</returns>
    /// <remarks>
    /// Only valid after Z3SolverCheck returns unknown (0). Provides information
    /// about why the solver could not determine satisfiability (e.g., timeout, incomplete theory).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var funcPtr = GetFunctionPointer("Z3_solver_get_reason_unknown");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SolverGetReasonUnknownDelegate>(funcPtr);
        return func(ctx, solver);
    }

    // Model functions
    /// <summary>
    /// Increments the reference counter of the given model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle to increment reference count for.</param>
    /// <remarks>
    /// Z3 uses reference counting for memory management. Must be paired with
    /// Z3ModelDecRef when the model is no longer needed.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelIncRefDelegate>(funcPtr);
        func(ctx, model);
    }

    /// <summary>
    /// Decrements the reference counter of the given model.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="model">The model handle to decrement reference count for.</param>
    /// <remarks>
    /// Must be paired with Z3ModelIncRef to properly manage memory. When reference
    /// count reaches zero, the model may be garbage collected.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelDecRefDelegate>(funcPtr);
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
    internal IntPtr Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var funcPtr = GetFunctionPointer("Z3_model_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelToStringDelegate>(funcPtr);
        return func(ctx, model);
    }

    /// <summary>
    /// Converts a Z3 AST node to its string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node handle to convert.</param>
    /// <returns>Handle to a null-terminated string representation of the AST.</returns>
    /// <remarks>
    /// Provides a human-readable representation of expressions, formulas, and other AST nodes.
    /// The string is managed by Z3 and valid until the context is deleted.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3AstToStringDelegate>(funcPtr);
        return func(ctx, ast);
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
    internal bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_model_eval");
        var func = Marshal.GetDelegateForFunctionPointer<Z3ModelEvalDelegate>(funcPtr);
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
    internal IntPtr Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetNumeralStringDelegate>(funcPtr);
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
    internal int Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bool_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetBoolValueDelegate>(funcPtr);
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
    internal bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_is_numeral_ast");
        var func = Marshal.GetDelegateForFunctionPointer<Z3IsNumeralAstDelegate>(funcPtr);
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
    internal IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortDelegate>(funcPtr);
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
    internal int Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_kind");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetSortKindDelegate>(funcPtr);
        return func(ctx, sort);
    }

    // Quantifier functions
    /// <summary>
    /// Creates a universal quantifier (for-all) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="weight">Quantifier weight for instantiation heuristics (0 for default).</param>
    /// <param name="numBound">The number of bound variables.</param>
    /// <param name="bound">Array of bound variable constants.</param>
    /// <param name="numPatterns">The number of patterns for instantiation.</param>
    /// <param name="patterns">Array of pattern expressions (can be null).</param>
    /// <param name="body">The Boolean formula body of the quantifier.</param>
    /// <returns>Handle to the created universal quantifier expression.</returns>
    /// <remarks>
    /// Creates ∀ bound_vars : body. Patterns help guide quantifier instantiation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkForallConst(
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

    /// <summary>
    /// Creates an existential quantifier (there-exists) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="weight">Quantifier weight for instantiation heuristics (0 for default).</param>
    /// <param name="numBound">The number of bound variables.</param>
    /// <param name="bound">Array of bound variable constants.</param>
    /// <param name="numPatterns">The number of patterns for instantiation.</param>
    /// <param name="patterns">Array of pattern expressions (can be null).</param>
    /// <param name="body">The Boolean formula body of the quantifier.</param>
    /// <returns>Handle to the created existential quantifier expression.</returns>
    /// <remarks>
    /// Creates ∃ bound_vars : body. Patterns help guide quantifier instantiation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkExistsConst(
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

    /// <summary>
    /// Creates a pattern for quantifier instantiation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numPatterns">The number of terms in the pattern.</param>
    /// <param name="terms">Array of expressions forming the pattern.</param>
    /// <returns>Handle to the created pattern.</returns>
    /// <remarks>
    /// Patterns guide Z3's quantifier instantiation by specifying which terms
    /// should trigger instantiation of the quantified formula.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_pattern");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkPatternDelegate>(funcPtr);
        return func(ctx, numPatterns, terms);
    }

    // Function declaration and application methods
    /// <summary>
    /// Creates a function declaration with specified domain and range sorts.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The function name symbol.</param>
    /// <param name="domainSize">The number of argument sorts.</param>
    /// <param name="domain">Array of argument sorts.</param>
    /// <param name="range">The return sort of the function.</param>
    /// <returns>Handle to the created function declaration.</returns>
    /// <remarks>
    /// Function declarations define the signature of uninterpreted functions.
    /// Used with Z3MkApp to create function application expressions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkFuncDecl(IntPtr ctx, IntPtr symbol, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkFuncDeclDelegate>(funcPtr);
        return func(ctx, symbol, domainSize, domain, range);
    }

    /// <summary>
    /// Creates a function application expression by applying arguments to a function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcDecl">The function declaration to apply.</param>
    /// <param name="numArgs">The number of arguments.</param>
    /// <param name="args">Array of argument expressions.</param>
    /// <returns>Handle to the created function application expression.</returns>
    /// <remarks>
    /// Creates f(args) where f is the function declaration. Arguments must match
    /// the function's domain sorts in number and type.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr Z3MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_app");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkAppDelegate>(funcPtr);
        return func(ctx, funcDecl, numArgs, args);
    }

    private delegate IntPtr Z3MkConfigDelegate();
    private delegate void Z3DelConfigDelegate(IntPtr cfg);
    private delegate void Z3SetParamValueDelegate(IntPtr cfg, IntPtr paramId, IntPtr paramValue);
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
    private delegate IntPtr Z3MkBvAddNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
    private delegate IntPtr Z3MkBvAddNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSubNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr Z3MkBvSubNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
    private delegate IntPtr Z3MkBvMulNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
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

    // Function declaration and application delegates
    private delegate IntPtr Z3MkFuncDeclDelegate(
        IntPtr ctx,
        IntPtr symbol,
        uint domainSize,
        IntPtr[] domain,
        IntPtr range
    );
    private delegate IntPtr Z3MkAppDelegate(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args);

    // Error handling delegates
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void Z3ErrorHandlerDelegate(IntPtr ctx, int errorCode);

    private delegate void Z3SetErrorHandlerDelegate(IntPtr ctx, Z3ErrorHandlerDelegate? handler);
    private delegate int Z3GetErrorCodeDelegate(IntPtr ctx);
    private delegate IntPtr Z3GetErrorMsgDelegate(IntPtr ctx, int errorCode);

    // Error handling methods
    /// <summary>
    /// Sets a custom error handler for the Z3 context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="handler">The error handler delegate, or null to remove current handler.</param>
    /// <remarks>
    /// The error handler is called when Z3 encounters internal errors.
    /// Provides custom error handling instead of default Z3 behavior.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void Z3SetErrorHandler(IntPtr ctx, Z3ErrorHandlerDelegate? handler)
    {
        var funcPtr = GetFunctionPointer("Z3_set_error_handler");
        var func = Marshal.GetDelegateForFunctionPointer<Z3SetErrorHandlerDelegate>(funcPtr);
        func(ctx, handler);
    }

    /// <summary>
    /// Retrieves the error code from the last Z3 operation on the context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>The error code from the last operation.</returns>
    /// <remarks>
    /// Returns Z3_OK if no error occurred. Use Z3GetErrorMsg to get
    /// a human-readable description of the error.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal Z3ErrorCode Z3GetErrorCode(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_error_code");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetErrorCodeDelegate>(funcPtr);
        return (Z3ErrorCode)func(ctx);
    }

    /// <summary>
    /// Retrieves a human-readable error message for the given error code.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="errorCode">The error code to get a message for.</param>
    /// <returns>String description of the error, or "Unknown error" if unavailable.</returns>
    /// <remarks>
    /// Provides detailed information about Z3 errors for debugging and user feedback.
    /// The returned string is managed by Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal string Z3GetErrorMsg(IntPtr ctx, Z3ErrorCode errorCode)
    {
        var funcPtr = GetFunctionPointer("Z3_get_error_msg");
        var func = Marshal.GetDelegateForFunctionPointer<Z3GetErrorMsgDelegate>(funcPtr);
        var msgPtr = func(ctx, (int)errorCode);
        return Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
    }
}
