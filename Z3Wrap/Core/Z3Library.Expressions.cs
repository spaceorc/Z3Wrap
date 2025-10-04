using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <summary>
    ///     Creates a constant expression with the given name and sort.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the constant.</param>
    /// <param name="sort">Sort of the constant.</param>
    /// <returns>AST handle for the constant expression.</returns>
    public IntPtr MkConst(IntPtr ctx, string name, IntPtr sort)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.MkConst(ctx, symbol, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkConst));
    }

    /// <inheritdoc cref="NativeZ3Library.MkTrue" />
    public IntPtr MkTrue(IntPtr ctx)
    {
        var result = nativeLibrary.MkTrue(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkTrue));
    }

    /// <inheritdoc cref="NativeZ3Library.MkFalse" />
    public IntPtr MkFalse(IntPtr ctx)
    {
        var result = nativeLibrary.MkFalse(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkFalse));
    }

    /// <summary>
    ///     Creates a numeral expression from a string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="numeral">String representation of the numeral.</param>
    /// <param name="sort">Sort of the numeral.</param>
    /// <returns>AST handle for the numeral expression.</returns>
    public IntPtr MkNumeral(IntPtr ctx, string numeral, IntPtr sort)
    {
        using var numeralPtr = new AnsiStringPtr(numeral);
        var result = nativeLibrary.MkNumeral(ctx, numeralPtr, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkNumeral));
    }
}
