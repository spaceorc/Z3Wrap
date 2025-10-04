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

    /// <inheritdoc cref="NativeZ3Library.MkBvudiv" />
    public IntPtr MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvudiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUDiv));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsrem" />
    public IntPtr MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsrem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSRem));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvurem" />
    public IntPtr MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvurem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvURem));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsmod" />
    public IntPtr MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsmod(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSMod));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvneg" />
    public IntPtr MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvneg(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNeg));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvand" />
    public IntPtr MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvand(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAnd));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvor" />
    public IntPtr MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvor(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvOr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvxor" />
    public IntPtr MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvxor(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvXor));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvnot" />
    public IntPtr MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvnot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNot));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvshl" />
    public IntPtr MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvshl(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvShl));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvashr" />
    public IntPtr MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvashr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAShr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvlshr" />
    public IntPtr MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvlshr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvLShr));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvslt" />
    public IntPtr MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvslt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvult" />
    public IntPtr MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvult(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsle" />
    public IntPtr MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsle(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvule" />
    public IntPtr MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvule(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsgt" />
    public IntPtr MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsgt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvugt" />
    public IntPtr MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvugt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGt));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsge" />
    public IntPtr MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsge(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGe));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvuge" />
    public IntPtr MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvuge(ctx, left, right);
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

    /// <inheritdoc cref="NativeZ3Library.MkBvaddNoOverflow" />
    public IntPtr MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvaddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsubNoOverflow" />
    public IntPtr MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsubNoUnderflow" />
    public IntPtr MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvsubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvmulNoOverflow" />
    public IntPtr MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvmulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvmulNoUnderflow" />
    public IntPtr MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvmulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvaddNoUnderflow" />
    public IntPtr MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvaddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoUnderflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvsdivNoOverflow" />
    public IntPtr MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvsdivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDivNoOverflow));
    }

    /// <inheritdoc cref="NativeZ3Library.MkBvnegNoOverflow" />
    public IntPtr MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvnegNoOverflow(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNegNoOverflow));
    }
}
