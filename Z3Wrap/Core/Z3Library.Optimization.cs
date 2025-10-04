using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="Interop.NativeZ3Library.MkOptimize" />
    public IntPtr MkOptimize(IntPtr ctx)
    {
        var result = nativeLibrary.MkOptimize(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkOptimize));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeIncRef" />
    public void OptimizeIncRef(IntPtr ctx, IntPtr optimize)
    {
        nativeLibrary.OptimizeIncRef(ctx, optimize);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeDecRef" />
    public void OptimizeDecRef(IntPtr ctx, IntPtr optimize)
    {
        nativeLibrary.OptimizeDecRef(ctx, optimize);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeAssert" />
    public void OptimizeAssert(IntPtr ctx, IntPtr optimize, IntPtr constraint)
    {
        nativeLibrary.OptimizeAssert(ctx, optimize, constraint);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeMaximize" />
    public uint OptimizeMaximize(IntPtr ctx, IntPtr optimize, IntPtr objective)
    {
        var result = nativeLibrary.OptimizeMaximize(ctx, optimize, objective);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeMinimize" />
    public uint OptimizeMinimize(IntPtr ctx, IntPtr optimize, IntPtr objective)
    {
        var result = nativeLibrary.OptimizeMinimize(ctx, optimize, objective);
        CheckError(ctx);
        return result;
    }

    /// <summary>
    ///     Checks the satisfiability of the assertions in the optimizer.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="optimize">Optimizer handle.</param>
    /// <returns>The satisfiability result (Satisfiable, Unsatisfiable, or Unknown).</returns>
    public Z3Status OptimizeCheck(IntPtr ctx, IntPtr optimize)
    {
        var result = nativeLibrary.OptimizeCheck(ctx, optimize, 0, []);
        CheckError(ctx);
        return (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException(
                $"Unexpected boolean value result {result} from Z3_optimize_check"
            ),
        };
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeGetModel" />
    public IntPtr OptimizeGetModel(IntPtr ctx, IntPtr optimize)
    {
        var result = nativeLibrary.OptimizeGetModel(ctx, optimize);
        CheckError(ctx);
        return CheckHandle(result, nameof(OptimizeGetModel));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeGetUpper" />
    public IntPtr OptimizeGetUpper(IntPtr ctx, IntPtr optimize, uint idx)
    {
        var result = nativeLibrary.OptimizeGetUpper(ctx, optimize, idx);
        CheckError(ctx);
        return CheckHandle(result, nameof(OptimizeGetUpper));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeGetLower" />
    public IntPtr OptimizeGetLower(IntPtr ctx, IntPtr optimize, uint idx)
    {
        var result = nativeLibrary.OptimizeGetLower(ctx, optimize, idx);
        CheckError(ctx);
        return CheckHandle(result, nameof(OptimizeGetLower));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeSetParams" />
    public void OptimizeSetParams(IntPtr ctx, IntPtr optimize, IntPtr paramsHandle)
    {
        nativeLibrary.OptimizeSetParams(ctx, optimize, paramsHandle);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizeToString" />
    public string? OptimizeToString(IntPtr ctx, IntPtr optimize)
    {
        var result = nativeLibrary.OptimizeToString(ctx, optimize);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(OptimizeToString)));
    }

    /// <summary>
    ///     Gets the reason why the optimizer returned unknown status.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="optimize">Optimizer handle.</param>
    /// <returns>String describing the reason for unknown status, or null if not available.</returns>
    public string? OptimizeGetReasonUnknown(IntPtr ctx, IntPtr optimize)
    {
        var result = nativeLibrary.OptimizeGetReasonUnknown(ctx, optimize);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(OptimizeGetReasonUnknown)));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizePush" />
    public void OptimizePush(IntPtr ctx, IntPtr optimize)
    {
        nativeLibrary.OptimizePush(ctx, optimize);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.OptimizePop" />
    public void OptimizePop(IntPtr ctx, IntPtr optimize)
    {
        nativeLibrary.OptimizePop(ctx, optimize);
        CheckError(ctx);
    }
}
