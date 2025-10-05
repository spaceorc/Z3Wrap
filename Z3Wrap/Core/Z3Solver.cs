using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 solver for checking satisfiability of logical constraints.
/// </summary>
public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private Z3Model? cachedModel;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Status? lastCheckResult;

    internal Z3Solver(Z3Context context, bool useSimpleSolver)
    {
        this.context = context;

        InternalHandle = useSimpleSolver
            ? context.Library.MkSimpleSolver(context.Handle)
            : context.Library.MkSolver(context.Handle);

        context.Library.SolverIncRef(context.Handle, InternalHandle);
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

    private IntPtr InternalHandle { get; }

    /// <summary>
    /// Releases all resources used by this solver.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        if (!isBeingDisposedByContext)
            // Delegate disposal to context - it will call back to InternalDispose
            context.DisposeSolver(this);

        disposed = true;
    }

    /// <summary>
    /// Adds a constraint to the solver.
    /// </summary>
    /// <param name="constraint">The boolean constraint to add.</param>
    public void Assert(BoolExpr constraint)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after assertion

        context.Library.SolverAssert(context.Handle, InternalHandle, constraint.Handle);
    }

    /// <summary>
    /// Resets the solver by removing all constraints.
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after reset

        context.Library.SolverReset(context.Handle, InternalHandle);
        lastCheckResult = null;
    }

    /// <summary>
    /// Applies a set of parameters to this solver.
    /// </summary>
    /// <param name="parameters">The parameters to apply.</param>
    public void SetParams(Z3Params parameters)
    {
        ThrowIfDisposed();
        parameters.ApplyTo(context, InternalHandle);
    }

    /// <summary>
    /// Checks the satisfiability of the current constraints.
    /// </summary>
    /// <returns>The satisfiability status.</returns>
    public Z3Status Check()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Clear any previous model

        lastCheckResult = context.Library.SolverCheck(context.Handle, InternalHandle) switch
        {
            Z3Library.Lbool.Z3_L_FALSE => Z3Status.Unsatisfiable,
            Z3Library.Lbool.Z3_L_TRUE => Z3Status.Satisfiable,
            Z3Library.Lbool.Z3_L_UNDEF => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected solver result: {lastCheckResult}"),
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

        return context.Library.SolverGetReasonUnknown(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Pushes a new scope onto the solver stack.
    /// </summary>
    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push

        context.Library.SolverPush(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Pops scopes from the solver stack, removing constraints added in those scopes.
    /// </summary>
    /// <param name="numScopes">Number of scopes to pop.</param>
    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop

        context.Library.SolverPop(context.Handle, InternalHandle, numScopes);
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
            var modelHandle = context.Library.SolverGetModel(context.Handle, InternalHandle);
            cachedModel = new Z3Model(context, modelHandle);
        }

        return cachedModel;
    }

    private void InvalidateModel()
    {
        cachedModel?.Invalidate();
        cachedModel = null;
    }

    internal void InternalDispose()
    {
        if (disposed)
            return;

        // Clean up model before solver disposal
        InvalidateModel();

        context.Library.SolverDecRef(context.Handle, InternalHandle);
        isBeingDisposedByContext = true;

        disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, nameof(Z3Solver));
}
