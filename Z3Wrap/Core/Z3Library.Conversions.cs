using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkInt2real" />
    public IntPtr MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkInt2real(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Real));
    }

    /// <inheritdoc cref="NativeZ3Library.MkReal2int" />
    public IntPtr MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkReal2int(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkReal2Int));
    }

    /// <inheritdoc cref="NativeZ3Library.MkInt2bv" />
    public IntPtr MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = nativeLibrary.MkInt2bv(ctx, size, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Bv));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBv2int" />
    public IntPtr MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = nativeLibrary.MkBv2int(ctx, term, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBv2Int));
    }
}
