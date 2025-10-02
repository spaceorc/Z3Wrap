using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

[ExcludeFromCodeCoverage] // P/Invoke wrappers are mechanical delegates - no logic to test
internal sealed partial class NativeLibrary : IDisposable
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
            LoadFunctionsContext(handle, functionPointers);
            LoadFunctionsSolver(handle, functionPointers);
            LoadFunctionsModel(handle, functionPointers);
            LoadFunctionsParameters(handle, functionPointers);
            LoadFunctionsQuantifiers(handle, functionPointers);
            LoadFunctionsExpressions(handle, functionPointers);
            LoadFunctionsArrays(handle, functionPointers);
            LoadFunctionsBitVectors(handle, functionPointers);
            LoadFunctionsFunctions(handle, functionPointers);
            LoadFunctionsErrorHandling(handle, functionPointers);
            LoadFunctionsPredicates(handle, functionPointers);
            LoadFunctionsFunctionInterpretations(handle, functionPointers);
            LoadFunctionsQueries(handle, functionPointers);
            LoadFunctionsSets(handle, functionPointers);
            LoadFunctionsConstraints(handle, functionPointers);
            LoadFunctionsStringTheory(handle, functionPointers);
            LoadFunctionsSolverExtensions(handle, functionPointers);
            LoadFunctionsTactics(handle, functionPointers);
            LoadFunctionsGoals(handle, functionPointers);
            LoadFunctionsProbes(handle, functionPointers);
            LoadFunctionsSimplifiers(handle, functionPointers);
            LoadFunctionsFloatingPoint(handle, functionPointers);
            LoadFunctionsDatatypes(handle, functionPointers);
            LoadFunctionsOptimization(handle, functionPointers);
            LoadFunctionsSpecialTheories(handle, functionPointers);
            LoadFunctionsAstCollections(handle, functionPointers);
            LoadFunctionsReferenceCountingExtra(handle, functionPointers);
            LoadFunctionsSimplify(handle, functionPointers);
            LoadFunctionsSubstitution(handle, functionPointers);
            LoadFunctionsAlgebraicNumbers(handle, functionPointers);
            LoadFunctionsStatistics(handle, functionPointers);
            LoadFunctionsParsing(handle, functionPointers);
            LoadFunctionsNumerals(handle, functionPointers);
            LoadFunctionsUtilities(handle, functionPointers);

            return new LoadedLibrary(functionPointers, handle);
        }
        catch (Exception ex)
        {
            // If anything fails, clean up the loaded library
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
            throw new DllNotFoundException(
                $"Incompatible Z3 library at '{libraryPath}'.\n"
                    + "Missing one or more crucial functions required for basic operation.\n"
                    + "Please ensure you have a compatible version of Z3 installed.\n"
                    + $"Original error: {ex.Message}",
                ex
            );
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

    private static void LoadFunctionOrNull(
        IntPtr libraryHandle,
        Dictionary<string, IntPtr> functionPointers,
        string functionName
    )
    {
        if (System.Runtime.InteropServices.NativeLibrary.TryGetExport(libraryHandle, functionName, out var functionPtr))
        {
            functionPointers[functionName] = functionPtr;
        }
        else
        {
            functionPointers[functionName] = IntPtr.Zero; // Mark as unavailable
        }
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

        if (ptr == IntPtr.Zero)
            throw new NotSupportedException(
                $"Function '{functionName}' is not available in this Z3 version.\n"
                    + $"Library: {LibraryPath}\n"
                    + "This feature may require a newer version of Z3."
            );

        return ptr;
    }
}
