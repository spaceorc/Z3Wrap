using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkAdd" />
    public IntPtr MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkAdd));
    }

    /// <inheritdoc cref="NativeLibrary.MkSub" />
    public IntPtr MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSub));
    }

    /// <inheritdoc cref="NativeLibrary.MkMul" />
    public IntPtr MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMul));
    }

    /// <inheritdoc cref="NativeLibrary.MkDiv" />
    public IntPtr MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkMod" />
    public IntPtr MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMod));
    }

    /// <inheritdoc cref="NativeLibrary.MkRem" />
    public IntPtr MkRem(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkRem(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRem));
    }

    /// <inheritdoc cref="NativeLibrary.MkUnaryMinus" />
    public IntPtr MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = nativeLibrary.MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkUnaryMinus));
    }
}
