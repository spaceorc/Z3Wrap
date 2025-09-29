using Spaceorc.Z3Wrap.Core.Interop;
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
    private readonly Z3Library library;
    private IntPtr configHandle;
    private IntPtr contextHandle;
    private bool disposed;

    /// <summary>
    /// Initializes a new Z3 context with optional configuration parameters and library.
    /// </summary>
    /// <param name="library">The Z3Library to use for Z3 operations. If null, uses <see cref="Z3.DefaultLibrary"/>.</param>
    /// <param name="parameters">Configuration parameters to set. If null, uses default configuration.</param>
    public Z3Context(Z3Library? library = null, Dictionary<string, string>? parameters = null)
    {
        this.library = library ?? Z3.DefaultLibrary;

        configHandle = this.library.Z3MkConfig();
        if (configHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 configuration");

        contextHandle = this.library.Z3MkContextRc(configHandle);
        if (contextHandle == IntPtr.Zero)
        {
            this.library.Z3DelConfig(configHandle);
            throw new InvalidOperationException("Failed to create Z3 context");
        }

        if (parameters != null)
        {
            foreach (var param in parameters)
                SetParameter(param.Key, param.Value);
        }
    }

    internal IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return contextHandle;
        }
    }

    internal Z3Library Library
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
    /// Sets a configuration parameter for this Z3 context.
    /// </summary>
    /// <param name="paramName">The parameter name.</param>
    /// <param name="paramValue">The parameter value.</param>
    public void SetParameter(string paramName, string paramValue)
    {
        ThrowIfDisposed();

        using var paramNamePtr = new AnsiStringPtr(paramName);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        library.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
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

    internal void TrackHandle(IntPtr handle)
    {
        ThrowIfDisposed();

        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid handle", nameof(handle));

        library.Z3IncRef(contextHandle, handle);
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

        var solverHandle = solver.InternalHandle;
        if (solverHandle != IntPtr.Zero)
            library.Z3SolverDecRef(contextHandle, solverHandle);

        solver.InternalDispose();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, typeof(Z3Context));

    private void DisposeCore()
    {
        if (disposed)
            return;

        if (contextHandle != IntPtr.Zero)
        {
            // First dispose all tracked solvers
            foreach (var solver in trackedSolvers.ToArray())
            {
                DisposeSolver(solver);
            }

            trackedSolvers.Clear();

            // Then clean up all tracked handles
            foreach (var handle in trackedHandles)
                library.Z3DecRef(contextHandle, handle);

            trackedHandles.Clear();

            // Finally dispose the context itself
            library.Z3DelContext(contextHandle);
            contextHandle = IntPtr.Zero;
        }

        if (configHandle != IntPtr.Zero)
        {
            library.Z3DelConfig(configHandle);
            configHandle = IntPtr.Zero;
        }

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
