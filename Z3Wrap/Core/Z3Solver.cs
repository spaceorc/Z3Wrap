using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 solver for checking satisfiability of logical constraints.
/// </summary>
public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Model? cachedModel;
    private Z3Status? lastCheckResult;

    internal Z3Solver(Z3Context context, bool useSimpleSolver)
    {
        this.context = context;

        InternalHandle = useSimpleSolver
            ? context.Library.Z3MkSimpleSolver(context.Handle)
            : context.Library.Z3MkSolver(context.Handle);

        if (InternalHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 solver");

        context.Library.Z3SolverIncRef(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Gets the native Z3 solver handle.
    /// </summary>
    public IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return InternalHandle;
        }
    }

    internal IntPtr InternalHandle { get; private set; }

    /// <summary>
    /// Adds a constraint to the solver.
    /// </summary>
    /// <param name="constraint">The boolean constraint to add.</param>
    public void Assert(BoolExpr constraint)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after assertion

        context.Library.Z3SolverAssert(context.Handle, InternalHandle, constraint.Handle);
    }

    /// <summary>
    /// Resets the solver by removing all constraints.
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after reset

        context.Library.Z3SolverReset(context.Handle, InternalHandle);
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

        lastCheckResult = context.Library.Z3SolverCheck(context.Handle, InternalHandle);
        return lastCheckResult.Value;
    }

    /// <summary>
    /// Gets the reason why the solver returned unknown status.
    /// </summary>
    /// <returns>The reason for unknown status.</returns>
    public string GetReasonUnknown()
    {
        ThrowIfDisposed();

        return context.Library.Z3SolverGetReasonUnknown(context.Handle, InternalHandle) ?? "Unknown reason";
    }

    /// <summary>
    /// Pushes a new scope onto the solver stack.
    /// </summary>
    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push

        context.Library.Z3SolverPush(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Pops scopes from the solver stack, removing constraints added in those scopes.
    /// </summary>
    /// <param name="numScopes">Number of scopes to pop.</param>
    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop

        context.Library.Z3SolverPop(context.Handle, InternalHandle, numScopes);
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
            var modelHandle = context.Library.Z3SolverGetModel(context.Handle, InternalHandle);
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
        InternalHandle = IntPtr.Zero;
        disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, nameof(Z3Solver));
}
