using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeLibrary.MkBvAdd" />
    public IntPtr MkBvAdd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAdd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAdd));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSub" />
    public IntPtr MkBvSub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSub(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSub));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMul" />
    public IntPtr MkBvMul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvMul(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMul));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSDiv" />
    public IntPtr MkBvSDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSDiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUDiv" />
    public IntPtr MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSRem" />
    public IntPtr MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSRem));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvURem" />
    public IntPtr MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvURem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvURem));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSMod" />
    public IntPtr MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSMod));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNeg" />
    public IntPtr MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNeg(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNeg));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAnd" />
    public IntPtr MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAnd));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvOr" />
    public IntPtr MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvOr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvOr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvXor" />
    public IntPtr MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvXor(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvXor));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNot" />
    public IntPtr MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNot));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvShl" />
    public IntPtr MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvShl(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvShl));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAShr" />
    public IntPtr MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAShr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvLShr" />
    public IntPtr MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvLShr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSLt" />
    public IntPtr MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvULt" />
    public IntPtr MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSLe" />
    public IntPtr MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvULe" />
    public IntPtr MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSGt" />
    public IntPtr MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUGt" />
    public IntPtr MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSGe" />
    public IntPtr MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUGe" />
    public IntPtr MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGe));
    }

    /// <inheritdoc cref="NativeLibrary.MkSignExt" />
    public IntPtr MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSignExt));
    }

    /// <inheritdoc cref="NativeLibrary.MkZeroExt" />
    public IntPtr MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkZeroExt));
    }

    /// <inheritdoc cref="NativeLibrary.MkExtract" />
    public IntPtr MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = nativeLibrary.MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkExtract));
    }

    /// <inheritdoc cref="NativeLibrary.MkRepeat" />
    public IntPtr MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = nativeLibrary.MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRepeat));
    }

    /// <inheritdoc cref="NativeLibrary.GetBvSortSize" />
    public uint GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAddNoOverflow" />
    public IntPtr MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSubNoOverflow" />
    public IntPtr MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSubNoUnderflow" />
    public IntPtr MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMulNoOverflow" />
    public IntPtr MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMulNoUnderflow" />
    public IntPtr MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAddNoUnderflow" />
    public IntPtr MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSDivNoOverflow" />
    public IntPtr MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDivNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNegNoOverflow" />
    public IntPtr MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNegNoOverflow));
    }
}
