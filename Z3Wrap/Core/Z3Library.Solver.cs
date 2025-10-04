using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="Interop.NativeZ3Library.MkSolver" />
    public IntPtr MkSolver(IntPtr ctx)
    {
        var result = nativeLibrary.MkSolver(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSolver));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.MkSimpleSolver" />
    public IntPtr MkSimpleSolver(IntPtr ctx)
    {
        var result = nativeLibrary.MkSimpleSolver(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSimpleSolver));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverIncRef" />
    public void SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverIncRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverDecRef" />
    public void SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverDecRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverAssert" />
    public void SolverAssert(IntPtr ctx, IntPtr solver, IntPtr expr)
    {
        nativeLibrary.SolverAssert(ctx, solver, expr);
        CheckError(ctx);
    }

    /// <summary>
    ///     Checks the satisfiability of the assertions in the solver.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>The satisfiability result (Satisfiable, Unsatisfiable, or Unknown).</returns>
    public Z3Status SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverCheck(ctx, solver);
        CheckError(ctx);
        return (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected boolean value result {result} from Z3_solver_check"),
        };
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverPush" />
    public void SolverPush(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverPush(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverPop" />
    public void SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        nativeLibrary.SolverPop(ctx, solver, numScopes);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverGetModel" />
    public IntPtr SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverGetModel(ctx, solver);
        CheckError(ctx);
        return CheckHandle(result, nameof(SolverGetModel));
    }

    /// <summary>
    ///     Gets the reason why the solver returned unknown status.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>String describing the reason for unknown status, or null if not available.</returns>
    public string? SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverGetReasonUnknown(ctx, solver);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(SolverGetReasonUnknown)));
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverReset" />
    public void SolverReset(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverReset(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="Interop.NativeZ3Library.SolverSetParams" />
    public void SolverSetParams(IntPtr ctx, IntPtr solver, IntPtr paramsHandle)
    {
        nativeLibrary.SolverSetParams(ctx, solver, paramsHandle);
        CheckError(ctx);
    }
}
