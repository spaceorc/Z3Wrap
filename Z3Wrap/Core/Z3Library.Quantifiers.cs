namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkForallConst" />
    public IntPtr MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.MkForallConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkForallConst));
    }

    /// <inheritdoc cref="NativeLibrary.MkExistsConst" />
    public IntPtr MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.MkExistsConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkExistsConst));
    }

    /// <inheritdoc cref="NativeLibrary.MkPattern" />
    public IntPtr MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var result = nativeLibrary.MkPattern(ctx, numPatterns, terms);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkPattern));
    }
}
