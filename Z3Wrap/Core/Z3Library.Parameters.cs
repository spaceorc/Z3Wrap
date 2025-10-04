using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="Interop.NativeZ3Library.MkParams" />
    public IntPtr MkParams(IntPtr ctx)
    {
        var result = nativeLibrary.MkParams(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkParams));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.ParamsIncRef" />
    public void ParamsIncRef(IntPtr ctx, IntPtr paramsHandle)
    {
        nativeLibrary.ParamsIncRef(ctx, paramsHandle);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.ParamsDecRef" />
    public void ParamsDecRef(IntPtr ctx, IntPtr paramsHandle)
    {
        nativeLibrary.ParamsDecRef(ctx, paramsHandle);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a boolean parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The boolean value to set.</param>
    public void ParamsSetBool(IntPtr ctx, IntPtr paramsHandle, string name, bool value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetBool(ctx, paramsHandle, symbol, value);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets an unsigned integer parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The unsigned integer value to set.</param>
    public void ParamsSetUInt(IntPtr ctx, IntPtr paramsHandle, string name, uint value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetUint(ctx, paramsHandle, symbol, value);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a double parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The double value to set.</param>
    public void ParamsSetDouble(IntPtr ctx, IntPtr paramsHandle, string name, double value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetDouble(ctx, paramsHandle, symbol, value);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a symbol parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The symbol value as a string.</param>
    public void ParamsSetSymbol(IntPtr ctx, IntPtr paramsHandle, string name, string value)
    {
        using var namePtr = new AnsiStringPtr(name);
        var nameSymbol = nativeLibrary.MkStringSymbol(ctx, namePtr);
        CheckError(ctx);

        using var valuePtr = new AnsiStringPtr(value);
        var valueSymbol = nativeLibrary.MkStringSymbol(ctx, valuePtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetSymbol(ctx, paramsHandle, nameSymbol, valueSymbol);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.ParamsToString" />
    public string? ParamsToString(IntPtr ctx, IntPtr paramsHandle)
    {
        var result = nativeLibrary.ParamsToString(ctx, paramsHandle);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(ParamsToString)));
    }
}
