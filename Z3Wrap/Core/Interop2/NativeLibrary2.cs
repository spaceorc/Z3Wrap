using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop2;

[ExcludeFromCodeCoverage] // P/Invoke wrappers are mechanical delegates - no logic to test
internal sealed partial class NativeLibrary2 : IDisposable
{
    private readonly LoadedLibrary loadedLibrary;
    private bool disposed;

    static NativeLibrary2()
    {
        // Discover all Z3 functions via reflection
        var methods = typeof(NativeLibrary2)
            .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(Z3FunctionAttribute), false).Length > 0);

        allFunctions = methods
            .Select(m => ((Z3FunctionAttribute)m.GetCustomAttributes(typeof(Z3FunctionAttribute), false)[0]).Name)
            .ToArray();
    }

    private NativeLibrary2(string libraryPath, LoadedLibrary loadedLibrary)
    {
        LibraryPath = libraryPath;
        this.loadedLibrary = loadedLibrary;
    }

    internal string LibraryPath { get; }

    public void Dispose()
    {
        if (disposed)
            return;

        NativeLibrary.Free(loadedLibrary.LibraryHandle);
        disposed = true;
        GC.SuppressFinalize(this);
    }

    internal static NativeLibrary2 Load(string libraryPath)
    {
        if (string.IsNullOrWhiteSpace(libraryPath))
            throw new ArgumentException("Library path cannot be null, empty, or whitespace", nameof(libraryPath));

        if (!File.Exists(libraryPath))
            throw new FileNotFoundException($"Z3 library not found at path: {libraryPath}", libraryPath);

        var loadedLib = LoadLibraryInternal(libraryPath);
        return new NativeLibrary2(libraryPath, loadedLib);
    }

    internal static NativeLibrary2 LoadAuto()
    {
        var searchPaths = GetPlatformSearchPaths();
        var loadAttempts = new List<(string path, Exception exception)>();

        foreach (var path in searchPaths)
            if (File.Exists(path))
                try
                {
                    var loadedLib = LoadLibraryInternal(path);
                    return new NativeLibrary2(path, loadedLib);
                }
                catch (Exception ex)
                {
                    loadAttempts.Add((path, ex));
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

    ~NativeLibrary2()
    {
        Dispose();
    }

    // Internal Library Loading
    private static LoadedLibrary LoadLibraryInternal(string libraryPath)
    {
        var handle = NativeLibrary.Load(libraryPath);
        var functionPointers = new Dictionary<string, IntPtr>();

        try
        {
            // Load all functions discovered via reflection
            foreach (var functionName in allFunctions)
            {
                LoadFunctionOrNull(handle, functionPointers, functionName);
            }

            // Validate that all required core functions were loaded
            ValidateRequiredFunctions(functionPointers);

            return new LoadedLibrary(functionPointers, handle);
        }
        catch (Exception ex)
        {
            // If anything fails, clean up the loaded library
            NativeLibrary.Free(handle);
            throw new DllNotFoundException(
                $"Incompatible Z3 library at '{libraryPath}'.\n"
                    + "Missing one or more crucial functions required for basic operation.\n"
                    + "Please ensure you have a compatible version of Z3 installed.\n"
                    + $"Original error: {ex.Message}",
                ex
            );
        }
    }

    private static readonly string[] allFunctions;

    /// <summary>
    /// Core Z3 functions that must be present in any compatible Z3 library.
    /// These are validated after library loading to ensure basic functionality.
    /// </summary>
    private static readonly string[] requiredFunctions =
    [
        // Context management
        "Z3_mk_context",
        "Z3_mk_context_rc",
        "Z3_del_context",
        "Z3_inc_ref",
        "Z3_dec_ref",
        // Basic sorts
        "Z3_mk_bool_sort",
        "Z3_mk_int_sort",
        "Z3_mk_real_sort",
        // Basic expressions
        "Z3_mk_true",
        "Z3_mk_false",
        "Z3_mk_eq",
        "Z3_mk_not",
        "Z3_mk_and",
        "Z3_mk_or",
        // Constants
        "Z3_mk_const",
        "Z3_mk_int_symbol",
        "Z3_mk_string_symbol",
        // Numerals
        "Z3_mk_int",
        "Z3_mk_int64",
        "Z3_mk_unsigned_int",
        "Z3_mk_unsigned_int64",
        // Solver
        "Z3_mk_solver",
        "Z3_solver_inc_ref",
        "Z3_solver_dec_ref",
        "Z3_solver_assert",
        "Z3_solver_check",
        "Z3_solver_get_model",
        // Model
        "Z3_model_inc_ref",
        "Z3_model_dec_ref",
        "Z3_model_eval",
        // AST operations
        "Z3_ast_to_string",
        "Z3_get_sort",
    ];

    private static void ValidateRequiredFunctions(Dictionary<string, IntPtr> functionPointers)
    {
        var missingFunctions = requiredFunctions
            .Where(name => !functionPointers.TryGetValue(name, out var ptr) || ptr == IntPtr.Zero)
            .ToList();

        if (missingFunctions.Count > 0)
        {
            throw new DllNotFoundException(
                $"Incompatible Z3 library: missing {missingFunctions.Count} required function(s).\n"
                    + "Missing functions:\n"
                    + string.Join("\n", missingFunctions.Select(f => $"  - {f}"))
                    + "\n\nPlease ensure you have a compatible version of Z3 installed (4.8.0 or newer recommended)."
            );
        }
    }

    private static void LoadFunctionOrNull(
        IntPtr libraryHandle,
        Dictionary<string, IntPtr> functionPointers,
        string functionName
    )
    {
        if (NativeLibrary.TryGetExport(libraryHandle, functionName, out var functionPtr))
            functionPointers[functionName] = functionPtr;
        else
            functionPointers[functionName] = IntPtr.Zero; // Mark as unavailable
    }

    private static string[] GetPlatformSearchPaths()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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

        // Fallback for other platforms
        return ["libz3.so", "libz3.dylib", "libz3.dll", "z3.so", "z3.dylib", "z3.dll"];
    }

    // Function Pointer Management
    private IntPtr GetFunctionPointer(string functionName)
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(NativeLibrary2));

        if (!loadedLibrary.FunctionPointers.TryGetValue(functionName, out var ptr))
            throw new InvalidOperationException($"Function {functionName} not loaded.");

        if (ptr == IntPtr.Zero)
            throw new NotSupportedException(
                $"Function '{functionName}' is not available in this Z3 version.\n"
                    + $"Library: {LibraryPath}\n"
                    + "This feature may require a newer version of Z3."
            );

        return ptr;
    }

    private record LoadedLibrary(Dictionary<string, IntPtr> FunctionPointers, IntPtr LibraryHandle);

    private class Z3FunctionAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
