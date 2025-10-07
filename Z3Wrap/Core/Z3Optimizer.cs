using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 optimizer for constraint optimization problems.
/// </summary>
public sealed class Z3Optimizer : IDisposable
{
    private readonly Z3Context context;
    private Z3Model? cachedModel;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Status? lastCheckResult;

    internal Z3Optimizer(Z3Context context)
    {
        this.context = context;

        InternalHandle = context.Library.MkOptimize(context.Handle);
        context.Library.OptimizeIncRef(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Gets the native Z3 optimizer handle.
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
    /// Releases all resources used by this optimizer.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        if (!isBeingDisposedByContext)
            // Delegate disposal to context - it will call back to InternalDispose
            context.DisposeOptimizer(this);

        disposed = true;
    }

    /// <summary>
    /// Adds a hard constraint to the optimizer.
    /// </summary>
    /// <param name="constraint">The boolean constraint to add.</param>
    public void Assert(BoolExpr constraint)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after assertion

        context.Library.OptimizeAssert(context.Handle, InternalHandle, constraint.Handle);
    }

    /// <summary>
    /// Checks the satisfiability and optimality of the current constraints.
    /// </summary>
    /// <returns>The satisfiability status.</returns>
    public Z3Status Check()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Clear any previous model

        lastCheckResult = context.Library.OptimizeCheck(context.Handle, InternalHandle, 0, []) switch
        {
            Z3Library.Lbool.Z3_L_FALSE => Z3Status.Unsatisfiable,
            Z3Library.Lbool.Z3_L_TRUE => Z3Status.Satisfiable,
            Z3Library.Lbool.Z3_L_UNDEF => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected optimizer result: {lastCheckResult}"),
        };
        return lastCheckResult.Value;
    }

    /// <summary>
    /// Gets the reason why the optimizer returned unknown status.
    /// </summary>
    /// <returns>The reason for unknown status.</returns>
    public string GetReasonUnknown()
    {
        ThrowIfDisposed();

        return context.Library.OptimizeGetReasonUnknown(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Pushes a new scope onto the optimizer stack.
    /// </summary>
    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push

        context.Library.OptimizePush(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Pops scopes from the optimizer stack, removing constraints added in those scopes.
    /// </summary>
    /// <param name="numScopes">Number of scopes to pop.</param>
    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop

        context.Library.OptimizePop(context.Handle, InternalHandle);
    }

    /// <summary>
    /// Gets the model from the optimizer after a satisfiable check result.
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
            throw new InvalidOperationException($"Cannot get model when optimizer status is {lastCheckResult}");

        // Return cached model if we have one
        if (cachedModel == null)
        {
            var modelHandle = context.Library.OptimizeGetModel(context.Handle, InternalHandle);
            cachedModel = new Z3Model(context, modelHandle);
        }

        return cachedModel;
    }

    /// <summary>
    /// Adds a maximization objective for the given expression.
    /// </summary>
    /// <typeparam name="TExpr">The expression type.</typeparam>
    /// <param name="expr">The expression to maximize.</param>
    /// <returns>A typed objective handle for retrieving optimal values.</returns>
    public OptimizeObjective<TExpr> Maximize<TExpr>(TExpr expr)
        where TExpr : Z3Expr, IOptimizableExpr
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after adding objective

        var objectiveId = context.Library.OptimizeMaximize(context.Handle, InternalHandle, expr.Handle);
        return new OptimizeObjective<TExpr>(objectiveId);
    }

    /// <summary>
    /// Adds a minimization objective for the given expression.
    /// </summary>
    /// <typeparam name="TExpr">The expression type.</typeparam>
    /// <param name="expr">The expression to minimize.</param>
    /// <returns>A typed objective handle for retrieving optimal values.</returns>
    public OptimizeObjective<TExpr> Minimize<TExpr>(TExpr expr)
        where TExpr : Z3Expr, IOptimizableExpr
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after adding objective

        var objectiveId = context.Library.OptimizeMinimize(context.Handle, InternalHandle, expr.Handle);
        return new OptimizeObjective<TExpr>(objectiveId);
    }

    /// <summary>
    /// Gets the upper bound (optimal value) for a maximization objective.
    /// </summary>
    /// <typeparam name="TExpr">The expression type.</typeparam>
    /// <param name="objective">The objective handle returned by Maximize.</param>
    /// <returns>The upper bound expression.</returns>
    public TExpr GetUpper<TExpr>(OptimizeObjective<TExpr> objective)
        where TExpr : Z3Expr, IOptimizableExpr, IExprType<TExpr>
    {
        ThrowIfDisposed();

        if (lastCheckResult != Z3Status.Satisfiable)
            throw new InvalidOperationException(
                $"Cannot get objective value when optimizer status is {lastCheckResult}"
            );

        var astHandle = context.Library.OptimizeGetUpper(context.Handle, InternalHandle, objective.ObjectiveId);
        return Z3Expr.Create<TExpr>(context, astHandle);
    }

    /// <summary>
    /// Gets the lower bound (optimal value) for a minimization objective.
    /// </summary>
    /// <typeparam name="TExpr">The expression type.</typeparam>
    /// <param name="objective">The objective handle returned by Minimize.</param>
    /// <returns>The lower bound expression.</returns>
    public TExpr GetLower<TExpr>(OptimizeObjective<TExpr> objective)
        where TExpr : Z3Expr, IOptimizableExpr, IExprType<TExpr>
    {
        ThrowIfDisposed();

        if (lastCheckResult != Z3Status.Satisfiable)
            throw new InvalidOperationException(
                $"Cannot get objective value when optimizer status is {lastCheckResult}"
            );

        var astHandle = context.Library.OptimizeGetLower(context.Handle, InternalHandle, objective.ObjectiveId);
        return Z3Expr.Create<TExpr>(context, astHandle);
    }

    /// <summary>
    /// Returns a string representation of the optimizer state.
    /// </summary>
    /// <returns>String representation in SMT-LIB2 format.</returns>
    public override string ToString()
    {
        ThrowIfDisposed();

        return context.Library.OptimizeToString(context.Handle, InternalHandle);
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

        // Clean up model before optimizer disposal
        InvalidateModel();

        context.Library.OptimizeDecRef(context.Handle, InternalHandle);
        isBeingDisposedByContext = true;

        disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, nameof(Z3Optimizer));
}
