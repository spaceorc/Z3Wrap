using System.Diagnostics;
using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core.Library;

/// <summary>
///     Provides a safe wrapper around the native Z3 library with error checking and crash prevention.
///     <para>
///         This class implements <see cref="IDisposable" /> and must be disposed when no longer needed,
///         unless ownership is transferred to the <see cref="Z3" /> static class by setting
///         <see cref="Z3.Library" />. When set as the default library, ownership transfers
///         to the <see cref="Z3" /> class and you must NOT call <see cref="Dispose()" /> manually.
///         The <see cref="Z3" /> class will automatically dispose the previous library when a new
///         one is set or when the application exits.
///     </para>
/// </summary>
public sealed partial class Z3Library2 : IDisposable
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void ErrorHandler(IntPtr ctx, NativeZ3Library.ErrorCode errorCode);

    private readonly NativeZ3Library nativeLibrary;
    private readonly ErrorHandler errorHandlerDelegate;
    private bool disposed;

    private Z3Library2(NativeZ3Library nativeLibrary)
    {
        this.nativeLibrary = nativeLibrary ?? throw new ArgumentNullException(nameof(nativeLibrary));
        errorHandlerDelegate = OnZ3ErrorSafe;
    }

    /// <summary>
    ///     Gets the path to the native library that was loaded.
    /// </summary>
    public string LibraryPath => nativeLibrary.LibraryPath;

    /// <summary>
    ///     Finalizer that ensures cleanup of Z3 library resources.
    /// </summary>
    ~Z3Library2()
    {
        Dispose();
    }

    // Static Factory Methods

    /// <summary>
    ///     Loads the Z3 native library from the specified path.
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 native library file.</param>
    /// <returns>A new <see cref="Z3Library2" /> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="ArgumentException">Thrown when the library path is null, empty, or whitespace.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the library file is not found at the specified path.</exception>
    public static Z3Library2 Load(string libraryPath)
    {
        var nativeLib = NativeZ3Library.Load(libraryPath);
        return new Z3Library2(nativeLib);
    }

    /// <summary>
    ///     Loads the Z3 native library using automatic discovery based on the current platform.
    /// </summary>
    /// <returns>A new <see cref="Z3Library2" /> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the Z3 library cannot be automatically located.</exception>
    public static Z3Library2 LoadAuto()
    {
        var nativeLib = NativeZ3Library.LoadAuto();
        return new Z3Library2(nativeLib);
    }

    // IDisposable Implementation

    /// <summary>
    ///     Releases all resources used by the Z3Library.
    ///     <para>
    ///         IMPORTANT: Do not call this method if you have transferred ownership by setting
    ///         this instance as <see cref="Z3.Library" />. The <see cref="Z3" /> class
    ///         will handle disposal automatically.
    ///     </para>
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        nativeLibrary.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    // Version Information

    /// <summary>
    ///     Gets the full Z3 version string.
    /// </summary>
    /// <returns>A string describing the Z3 version.</returns>
    public string GetFullVersion()
    {
        var versionPtr = nativeLibrary.GetFullVersion();
        return Marshal.PtrToStringAnsi(versionPtr) ?? "Unknown version";
    }

    // Configuration Management

    /// <summary>
    ///     Creates a configuration object for the Z3 context.
    /// </summary>
    /// <returns>Configuration handle.</returns>
    /// <remarks>
    ///     Configurations are created to assign parameters prior to creating contexts for Z3 interaction.
    ///     Common parameters include: proof, debug_ref_count, trace, timeout, model, unsat_core.
    /// </remarks>
    public IntPtr MkConfig()
    {
        return nativeLibrary.MkConfig();
    }

    /// <summary>
    ///     Deletes the given configuration object.
    /// </summary>
    /// <param name="cfg">Configuration handle.</param>
    public void DelConfig(IntPtr cfg)
    {
        nativeLibrary.DelConfig(cfg);
    }

    /// <summary>
    ///     Sets a configuration parameter value.
    /// </summary>
    /// <param name="cfg">Configuration handle.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void SetParamValue(IntPtr cfg, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.SetParamValue(cfg, paramIdPtr, paramValuePtr);
    }

    // Context Management

    /// <summary>
    ///     Creates a reference-counted context using the given configuration.
    /// </summary>
    /// <param name="cfg">Configuration handle.</param>
    /// <returns>Context handle.</returns>
    /// <remarks>
    ///     This is the recommended method for creating Z3 contexts in Z3 4.x.
    ///     After creation, the configuration cannot be changed, though some parameters
    ///     can be updated using <see cref="UpdateParamValue" />.
    /// </remarks>
    public IntPtr MkContextRc(IntPtr cfg)
    {
        var result = CheckHandle(nativeLibrary.MkContextRc(cfg), nameof(MkContextRc));

        // No error check for context creation
        // Set up safe error handler (prevents crashes)
        var errorHandlerPtr = Marshal.GetFunctionPointerForDelegate(errorHandlerDelegate);
        nativeLibrary.SetErrorHandler(result, errorHandlerPtr);
        return result;
    }

    /// <summary>
    ///     Deletes the given logical context.
    /// </summary>
    /// <param name="ctx">Context handle.</param>
    public void DelContext(IntPtr ctx)
    {
        nativeLibrary.DelContext(ctx);
        // No error check needed for deletion
    }

    /// <summary>
    ///     Updates a parameter value for an existing context.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void UpdateParamValue(IntPtr ctx, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.UpdateParamValue(ctx, paramIdPtr, paramValuePtr);
        CheckError(ctx);
    }

    // Private Helper Methods

    private static IntPtr CheckHandle(IntPtr handle, string methodName)
    {
        if (handle == IntPtr.Zero)
            throw new InvalidOperationException($"{methodName} returned null handle");

        return handle;
    }

    private void CheckError(IntPtr ctx)
    {
        var errorCode = nativeLibrary.GetErrorCode(ctx);
        if (errorCode == NativeZ3Library.ErrorCode.Z3_OK)
            return;
        var msgPtr = nativeLibrary.GetErrorMsg(ctx, errorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        throw new Z3Exception((Z3ErrorCode)errorCode, message);
    }

    private void OnZ3ErrorSafe(IntPtr ctx, NativeZ3Library.ErrorCode errorCode)
    {
        // DO NOT THROW EXCEPTIONS HERE - this is called from native Z3 code!
        var msgPtr = nativeLibrary.GetErrorMsg(ctx, errorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        Debug.WriteLine($"Z3 Error: {errorCode}: {message}");
    }
}
