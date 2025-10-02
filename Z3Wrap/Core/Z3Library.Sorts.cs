using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.IncRef" />
    public void IncRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.DecRef" />
    public void DecRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.MkBoolSort" />
    public IntPtr MkBoolSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkBoolSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBoolSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkIntSort" />
    public IntPtr MkIntSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkIntSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIntSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkRealSort" />
    public IntPtr MkRealSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkRealSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRealSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSort" />
    public IntPtr MkBvSort(IntPtr ctx, uint size)
    {
        var result = nativeLibrary.MkBvSort(ctx, size);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkArraySort" />
    public IntPtr MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = nativeLibrary.MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkArraySort));
    }
}
