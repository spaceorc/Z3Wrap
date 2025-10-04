namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkEq" />
    public IntPtr MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkEq(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkEq));
    }

    /// <inheritdoc cref="NativeLibrary.MkLt" />
    public IntPtr MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLt));
    }

    /// <inheritdoc cref="NativeLibrary.MkLe" />
    public IntPtr MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLe));
    }

    /// <inheritdoc cref="NativeLibrary.MkGt" />
    public IntPtr MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkGe" />
    public IntPtr MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGe));
    }
}
