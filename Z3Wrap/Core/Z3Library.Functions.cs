using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <summary>
    ///     Creates a function declaration with the given name, domain, and range.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the function.</param>
    /// <param name="domainSize">Number of arguments.</param>
    /// <param name="domain">Array of argument sorts.</param>
    /// <param name="range">Return sort.</param>
    /// <returns>Function declaration handle.</returns>
    public IntPtr MkFuncDecl(IntPtr ctx, string name, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.MkFuncDecl(ctx, symbol, domainSize, domain, range);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkFuncDecl));
    }

    /// <inheritdoc cref="NativeLibrary.MkApp" />
    public IntPtr MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkApp(ctx, funcDecl, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkApp));
    }
}
