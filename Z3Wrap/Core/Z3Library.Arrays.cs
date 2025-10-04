using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkConstArray" />
    public IntPtr MkConstArray(IntPtr ctx, IntPtr sort, IntPtr value)
    {
        var result = nativeLibrary.MkConstArray(ctx, sort, value);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkConstArray));
    }

    /// <inheritdoc cref="NativeZ3Library.MkStore" />
    public IntPtr MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var result = nativeLibrary.MkStore(ctx, array, index, value);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkStore));
    }

    /// <inheritdoc cref="NativeZ3Library.MkSelect" />
    public IntPtr MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var result = nativeLibrary.MkSelect(ctx, array, index);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSelect));
    }

    /// <inheritdoc cref="NativeZ3Library.GetArraySortDomain" />
    public IntPtr GetArraySortDomain(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetArraySortDomain(ctx, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetArraySortDomain));
    }

    /// <inheritdoc cref="NativeZ3Library.GetArraySortRange" />
    public IntPtr GetArraySortRange(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetArraySortRange(ctx, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetArraySortRange));
    }
}
