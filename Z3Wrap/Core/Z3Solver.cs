using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 solver for checking satisfiability of logical constraints.
/// </summary>
public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private IntPtr solverHandle;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Model? cachedModel;
    private Z3Status? lastCheckResult;

    internal Z3Solver(Z3Context context, bool useSimpleSolver)
    {
        this.context = context;

        solverHandle = useSimpleSolver
            ? context.Library.Z3MkSimpleSolver(context.Handle)
            : context.Library.Z3MkSolver(context.Handle);

        if (solverHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 solver");

        context.Library.Z3SolverIncRef(context.Handle, solverHandle);
    }

    internal IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return solverHandle;
        }
    }

    internal IntPtr InternalHandle => solverHandle;

    /// <summary>
    /// Adds a constraint to the solver.
    /// </summary>
    /// <param name="constraint">The boolean constraint to add.</param>
    public void Assert(BoolExpr constraint)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after assertion

        context.Library.Z3SolverAssert(context.Handle, solverHandle, constraint.Handle);
    }

    /// <summary>
    /// Resets the solver by removing all constraints.
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after reset

        context.Library.Z3SolverReset(context.Handle, solverHandle);
        lastCheckResult = null;
    }

    /// <summary>
    /// Checks the satisfiability of the current constraints.
    /// </summary>
    /// <returns>The satisfiability status.</returns>
    public Z3Status Check()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Clear any previous model

        var result = context.Library.Z3SolverCheck(context.Handle, solverHandle);
        lastCheckResult = (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected boolean value result {result} from Z3_solver_check"),
        };
        return lastCheckResult.Value;
    }

    /// <summary>
    /// Gets the reason why the solver returned unknown status.
    /// </summary>
    /// <returns>The reason for unknown status.</returns>
    public string GetReasonUnknown()
    {
        ThrowIfDisposed();

        var reasonPtr = context.Library.Z3SolverGetReasonUnknown(context.Handle, solverHandle);
        return Marshal.PtrToStringAnsi(reasonPtr) ?? "Unknown reason";
    }

    /// <summary>
    /// Pushes a new scope onto the solver stack.
    /// </summary>
    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push

        context.Library.Z3SolverPush(context.Handle, solverHandle);
    }

    /// <summary>
    /// Pops scopes from the solver stack, removing constraints added in those scopes.
    /// </summary>
    /// <param name="numScopes">Number of scopes to pop.</param>
    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop

        context.Library.Z3SolverPop(context.Handle, solverHandle, numScopes);
    }

    /// <summary>
    /// Gets the model from the solver after a satisfiable check result.
    /// </summary>
    /// <returns>The satisfying model.</returns>
    public Z3Model GetModel()
    {
        ThrowIfDisposed();

        // Check if we have called Check() before
        if (lastCheckResult == null)
            throw new InvalidOperationException("Must call Check() before GetModel()");

        // Check if the result was satisfiable
        if (lastCheckResult != Z3Status.Satisfiable)
            throw new InvalidOperationException($"Cannot get model when solver status is {lastCheckResult}");

        // Return cached model if we have one
        if (cachedModel == null)
        {
            var modelHandle = context.Library.Z3SolverGetModel(context.Handle, solverHandle);
            if (modelHandle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to get model from solver");

            cachedModel = new Z3Model(context, modelHandle);
        }

        return cachedModel;
    }

    private void InvalidateModel()
    {
        cachedModel?.Invalidate();
        cachedModel = null;
    }

    /// <summary>
    /// Releases all resources used by this solver.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        if (!isBeingDisposedByContext)
        {
            // Delegate disposal to context - it will call back to InternalDispose
            context.DisposeSolver(this);
        }

        disposed = true;
    }

    internal void InternalDispose()
    {
        if (disposed)
            return;

        // Clean up model before solver disposal
        InvalidateModel();

        isBeingDisposedByContext = true;
        solverHandle = IntPtr.Zero;
        disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, nameof(Z3Solver));
}
