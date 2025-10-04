using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.IncRef" />
    public void IncRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeZ3Library.DecRef" />
    public void DecRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeZ3Library.MkBoolSort" />
    public IntPtr MkBoolSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkBoolSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBoolSort));
    }

    /// <inheritdoc cref="NativeZ3Library.MkIntSort" />
    public IntPtr MkIntSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkIntSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIntSort));
    }

    /// <inheritdoc cref="NativeZ3Library.MkRealSort" />
    public IntPtr MkRealSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkRealSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRealSort));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSort" />
    public IntPtr MkBvSort(IntPtr ctx, uint size)
    {
        var result = nativeLibrary.MkBvSort(ctx, size);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSort));
    }

    /// <inheritdoc cref="NativeZ3Library.MkArraySort" />
    public IntPtr MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = nativeLibrary.MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkArraySort));
    }
}
