using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkInt2Real" />
    public IntPtr MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkInt2Real(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Real));
    }

    /// <inheritdoc cref="NativeLibrary.MkReal2Int" />
    public IntPtr MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkReal2Int(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkReal2Int));
    }

    /// <inheritdoc cref="NativeLibrary.MkInt2Bv" />
    public IntPtr MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = nativeLibrary.MkInt2Bv(ctx, size, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Bv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBv2Int" />
    public IntPtr MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = nativeLibrary.MkBv2Int(ctx, term, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBv2Int));
    }
}
