using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="NativeZ3Library.MkBvadd" />
    public IntPtr MkBvadd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvadd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvadd));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsub" />
    public IntPtr MkBvsub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsub(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvsub));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvmul" />
    public IntPtr MkBvmul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvmul(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvmul));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsdiv" />
    public IntPtr MkBvsdiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsdiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvsdiv));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvUDiv" />
    public IntPtr MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUDiv));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSRem" />
    public IntPtr MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSRem));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvURem" />
    public IntPtr MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvURem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvURem));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSMod" />
    public IntPtr MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSMod));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvNeg" />
    public IntPtr MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNeg(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNeg));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvAnd" />
    public IntPtr MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAnd));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvOr" />
    public IntPtr MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvOr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvOr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvXor" />
    public IntPtr MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvXor(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvXor));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvNot" />
    public IntPtr MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNot));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvShl" />
    public IntPtr MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvShl(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvShl));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvAShr" />
    public IntPtr MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAShr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvLShr" />
    public IntPtr MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvLShr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSLt" />
    public IntPtr MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvULt" />
    public IntPtr MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSLe" />
    public IntPtr MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvULe" />
    public IntPtr MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSGt" />
    public IntPtr MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvUGt" />
    public IntPtr MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSGe" />
    public IntPtr MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvUGe" />
    public IntPtr MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkSignExt" />
    public IntPtr MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSignExt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkZeroExt" />
    public IntPtr MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkZeroExt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkExtract" />
    public IntPtr MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = nativeLibrary.MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkExtract));
    }

    /// <inheritdoc cref="NativeZ3Library.MkRepeat" />
    public IntPtr MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = nativeLibrary.MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRepeat));
    }

    /// <inheritdoc cref="NativeZ3Library.GetBvSortSize" />
    public uint GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvAddNoOverflow" />
    public IntPtr MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSubNoOverflow" />
    public IntPtr MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSubNoUnderflow" />
    public IntPtr MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvMulNoOverflow" />
    public IntPtr MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvMulNoUnderflow" />
    public IntPtr MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvAddNoUnderflow" />
    public IntPtr MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvSDivNoOverflow" />
    public IntPtr MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDivNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvNegNoOverflow" />
    public IntPtr MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNegNoOverflow));
    }
}
