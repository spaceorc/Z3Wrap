using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkAdd" />
    public IntPtr MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkAdd));
    }

    /// <inheritdoc cref="NativeZ3Library.MkSub" />
    public IntPtr MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSub));
    }

    /// <inheritdoc cref="NativeZ3Library.MkMul" />
    public IntPtr MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMul));
    }

    /// <inheritdoc cref="NativeZ3Library.MkDiv" />
    public IntPtr MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkDiv));
    }

    /// <inheritdoc cref="NativeZ3Library.MkMod" />
    public IntPtr MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMod));
    }

    /// <inheritdoc cref="NativeZ3Library.MkRem" />
    public IntPtr MkRem(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkRem(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRem));
    }

    /// <inheritdoc cref="NativeZ3Library.MkUnaryMinus" />
    public IntPtr MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = nativeLibrary.MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkUnaryMinus));
    }
}
