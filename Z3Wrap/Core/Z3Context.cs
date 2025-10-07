using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents the main entry point for Z3 theorem prover operations and expression creation.
/// </summary>
public sealed class Z3Context : IDisposable
{
    private static readonly ThreadLocal<Z3Context?> currentContext = new(() => null);

    private readonly HashSet<IntPtr> trackedHandles = [];
    private readonly HashSet<Z3Solver> trackedSolvers = [];
    private readonly HashSet<Z3Optimizer> trackedOptimizers = [];
    private readonly Z3Library library;
    private IntPtr contextHandle;
    private bool disposed;

    /// <summary>
    /// Initializes a new Z3 context with optional configuration parameters and library.
    /// </summary>
    /// <param name="parameters">Configuration parameters to set. If null, uses default configuration.
    ///     Parameters must be set at context creation time as some can only be configured this way.</param>
    /// <param name="library">The Z3Library to use for Z3 operations. If null, uses <see cref="Z3.Library"/>.</param>
    public Z3Context(Dictionary<string, string>? parameters = null, Z3Library? library = null)
    {
        this.library = library ?? Z3.Library;

        // Create temporary config object
        var configHandle = this.library.MkConfig();
        try
        {
            // Set parameters on config before creating context
            if (parameters != null)
            {
                foreach (var param in parameters)
                    this.library.SetParamValue(configHandle, param.Key, param.Value);
            }

            // Create context from configured config
            contextHandle = this.library.MkContextRc(configHandle);
        }
        finally
        {
            // Always delete config after context creation
            this.library.DelConfig(configHandle);
        }
    }

    /// <summary>
    /// Gets the native Z3 context handle.
    /// </summary>
    public IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return contextHandle;
        }
    }

    /// <summary>
    /// Gets the Z3 library instance used by this context.
    /// </summary>
    public Z3Library Library
    {
        get
        {
            ThrowIfDisposed();
            return library;
        }
    }

    /// <summary>
    /// Releases all resources used by this Z3 context.
    /// </summary>
    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates a configuration parameter on this context after creation.
    /// </summary>
    /// <param name="paramName">The parameter name.</param>
    /// <param name="paramValue">The parameter value.</param>
    /// <remarks>
    /// Some parameters can only be set at context creation time via constructor.
    /// This method is for parameters that can be updated dynamically.
    /// </remarks>
    public void SetParameter(string paramName, string paramValue)
    {
        ThrowIfDisposed();

        library.UpdateParamValue(contextHandle, paramName, paramValue);
    }

    /// <summary>
    /// Creates a new solver instance for this context.
    /// </summary>
    /// <returns>A new solver instance.</returns>
    public Z3Solver CreateSolver()
    {
        var solver = new Z3Solver(this, false);
        TrackSolver(solver);
        return solver;
    }

    /// <summary>
    /// Creates a new simple solver instance for this context.
    /// </summary>
    /// <returns>A new simple solver instance.</returns>
    public Z3Solver CreateSimpleSolver()
    {
        var solver = new Z3Solver(this, true);
        TrackSolver(solver);
        return solver;
    }

    /// <summary>
    /// Creates a new optimizer instance for this context.
    /// </summary>
    /// <returns>A new optimizer instance.</returns>
    public Z3Optimizer CreateOptimizer()
    {
        var optimizer = new Z3Optimizer(this);
        TrackOptimizer(optimizer);
        return optimizer;
    }

    internal void TrackHandle(IntPtr handle)
    {
        ThrowIfDisposed();
        library.IncRef(contextHandle, handle);
        trackedHandles.Add(handle);
    }

    private void TrackSolver(Z3Solver solver)
    {
        ThrowIfDisposed();
        trackedSolvers.Add(solver);
    }

    private void UntrackSolver(Z3Solver solver)
    {
        trackedSolvers.Remove(solver);
    }

    internal void DisposeSolver(Z3Solver solver)
    {
        if (disposed)
            return;

        UntrackSolver(solver);
        solver.InternalDispose();
    }

    private void TrackOptimizer(Z3Optimizer optimizer)
    {
        ThrowIfDisposed();
        trackedOptimizers.Add(optimizer);
    }

    private void UntrackOptimizer(Z3Optimizer optimizer)
    {
        trackedOptimizers.Remove(optimizer);
    }

    internal void DisposeOptimizer(Z3Optimizer optimizer)
    {
        if (disposed)
            return;

        UntrackOptimizer(optimizer);
        optimizer.InternalDispose();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, typeof(Z3Context));

    private void DisposeCore()
    {
        if (disposed)
            return;

        // First dispose all tracked solvers
        foreach (var solver in trackedSolvers.ToArray())
            DisposeSolver(solver);

        trackedSolvers.Clear();

        // Dispose all tracked optimizers
        foreach (var optimizer in trackedOptimizers.ToArray())
            DisposeOptimizer(optimizer);

        trackedOptimizers.Clear();

        // Then clean up all tracked handles
        foreach (var handle in trackedHandles)
            library.DecRef(contextHandle, handle);

        trackedHandles.Clear();

        // Finally dispose the context itself
        library.DelContext(contextHandle);

        disposed = true;
    }

    /// <summary>
    /// Finalizer that ensures proper resource cleanup.
    /// </summary>
    ~Z3Context()
    {
        DisposeCore();
    }

    /// <summary>
    /// Gets the currently active Z3 context for implicit operations.
    /// </summary>
    public static Z3Context Current =>
        currentContext.Value
        ?? throw new InvalidOperationException(
            "No Z3Context is currently set. Use 'using var scope = context.SetUp()' to enable implicit conversions."
        );

    /// <summary>
    /// Gets whether a current context is set for implicit operations.
    /// </summary>
    public static bool IsCurrentContextSet => currentContext.Value != null;

    /// <summary>
    /// Sets up this context as the current context for natural syntax operations.
    /// </summary>
    /// <returns>A scope that restores the previous context when disposed.</returns>
    public IDisposable SetUp()
    {
        ThrowIfDisposed();
        return new SetUpScope(this);
    }

    private sealed class SetUpScope : IDisposable
    {
        private readonly Z3Context? previousContext;

        internal SetUpScope(Z3Context context)
        {
            previousContext = currentContext.Value;
            currentContext.Value = context;
        }

        public void Dispose()
        {
            currentContext.Value = previousContext;
        }
    }

    internal IntPtr GetSortForType<T>()
        where T : Z3Expr, IExprType<T> => T.Sort(this);
}
