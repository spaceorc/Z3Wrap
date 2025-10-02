using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkAnd" />
    public IntPtr MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkAnd(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkAnd));
    }

    /// <inheritdoc cref="NativeLibrary.MkOr" />
    public IntPtr MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkOr(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkOr));
    }

    /// <inheritdoc cref="NativeLibrary.MkNot" />
    public IntPtr MkNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkNot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkNot));
    }

    /// <inheritdoc cref="NativeLibrary.MkImplies" />
    public IntPtr MkImplies(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkImplies(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkImplies));
    }

    /// <inheritdoc cref="NativeLibrary.MkIff" />
    public IntPtr MkIff(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkIff(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIff));
    }

    /// <inheritdoc cref="NativeLibrary.MkXor" />
    public IntPtr MkXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkXor(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkXor));
    }

    /// <inheritdoc cref="NativeLibrary.MkIte" />
    public IntPtr MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var result = nativeLibrary.MkIte(ctx, condition, thenExpr, elseExpr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIte));
    }
}
