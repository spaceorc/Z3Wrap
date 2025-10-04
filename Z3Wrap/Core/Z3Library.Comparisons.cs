using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkEq" />
    public IntPtr MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkEq(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkEq));
    }

    /// <inheritdoc cref="NativeZ3Library.MkLt" />
    public IntPtr MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkLe" />
    public IntPtr MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkGt" />
    public IntPtr MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkGe" />
    public IntPtr MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGe));
    }
}
