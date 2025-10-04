using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkConfig" />
    public IntPtr MkConfig()
    {
        return nativeLibrary.MkConfig();
    }

    /// <inheritdoc cref="NativeZ3Library.DelConfig" />
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

    /// <inheritdoc cref="NativeZ3Library.MkContextRc" />
    public IntPtr MkContextRc(IntPtr cfg)
    {
        var result = CheckHandle(nativeLibrary.MkContextRc(cfg), nameof(MkContextRc));

        // No error check for context creation
        // Set up safe error handler (prevents crashes)
        nativeLibrary.SetErrorHandler(result, OnZ3ErrorSafe);
        return result;
    }

    /// <inheritdoc cref="NativeZ3Library.DelContext" />
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
}
