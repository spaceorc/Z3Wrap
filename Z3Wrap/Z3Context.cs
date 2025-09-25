using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

/// <summary>
/// Represents a Z3 theorem prover context that manages expressions, solvers, and memory.
/// Provides thread-local scoped setup for natural mathematical syntax and automatic resource cleanup.
/// </summary>
public class Z3Context : IDisposable
{
    private static readonly ThreadLocal<Z3Context?> currentContext = new(() => null);

    private readonly HashSet<IntPtr> trackedAstNodes = [];
    private readonly HashSet<Z3Solver> trackedSolvers = [];
    private IntPtr configHandle;
    private IntPtr contextHandle;
    private bool disposed;

    /// <summary>
    /// Initializes a new Z3 context with default configuration.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when Z3 context creation fails.</exception>
    public Z3Context()
    {
        configHandle = SafeNativeMethods.Z3MkConfig();
        if (configHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 configuration");

        contextHandle = SafeNativeMethods.Z3MkContextRc(configHandle);
        if (contextHandle == IntPtr.Zero)
        {
            SafeNativeMethods.Z3DelConfig(configHandle);
            throw new InvalidOperationException("Failed to create Z3 context");
        }
    }

    /// <summary>
    /// Initializes a new Z3 context with the specified parameters.
    /// </summary>
    /// <param name="parameters">Configuration parameters as key-value pairs.</param>
    /// <exception cref="InvalidOperationException">Thrown when Z3 context creation fails.</exception>
    public Z3Context(Dictionary<string, string> parameters)
        : this()
    {
        foreach (var param in parameters)
            SetParameter(param.Key, param.Value);
    }

    internal IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return contextHandle;
        }
    }

    /// <summary>
    /// Releases all resources used by the Z3 context, including tracked expressions and solvers.
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
    /// <exception cref="ObjectDisposedException">Thrown when the context has been disposed.</exception>
    public void SetParameter(string paramName, string paramValue)
    {
        ThrowIfDisposed();

        using var paramNamePtr = new AnsiStringPtr(paramName);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        SafeNativeMethods.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
    }

    /// <summary>
    /// Creates a new general-purpose solver that supports all Z3 theories and tactics.
    /// </summary>
    /// <returns>A new Z3 solver instance.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the context has been disposed.</exception>
    public Z3Solver CreateSolver()
    {
        var solver = new Z3Solver(this, false);
        TrackSolver(solver);
        return solver;
    }

    /// <summary>
    /// Creates a new simplified solver optimized for basic satisfiability checking.
    /// Faster than the general solver but with limited functionality.
    /// </summary>
    /// <returns>A new simplified Z3 solver instance.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the context has been disposed.</exception>
    public Z3Solver CreateSimpleSolver()
    {
        var solver = new Z3Solver(this, true);
        TrackSolver(solver);
        return solver;
    }

    internal void TrackAstNode(IntPtr handle)
    {
        ThrowIfDisposed();

        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid AST node handle", nameof(handle));

        SafeNativeMethods.Z3IncRef(contextHandle, handle);
        trackedAstNodes.Add(handle);
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
            SafeNativeMethods.Z3SolverDecRef(contextHandle, solverHandle);

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

            // Then clean up all tracked AST nodes
            foreach (var astHandle in trackedAstNodes)
                SafeNativeMethods.Z3DecRef(contextHandle, astHandle);

            trackedAstNodes.Clear();

            // Finally dispose the context itself
            SafeNativeMethods.Z3DelContext(contextHandle);
            contextHandle = IntPtr.Zero;
        }

        if (configHandle != IntPtr.Zero)
        {
            SafeNativeMethods.Z3DelConfig(configHandle);
            configHandle = IntPtr.Zero;
        }

        disposed = true;
    }

    /// <summary>
    /// Finalizes the Z3Context instance, ensuring proper cleanup of native resources.
    /// </summary>
    ~Z3Context()
    {
        DisposeCore();
    }

    /// <summary>
    /// Gets the current thread-local Z3 context used for implicit conversions and natural syntax.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no context is currently set up.</exception>
    public static Z3Context Current =>
        currentContext.Value
        ?? throw new InvalidOperationException(
            "No Z3Context is currently set. Use 'using var scope = context.SetUp()' to enable implicit conversions."
        );

    /// <summary>
    /// Gets a value indicating whether a Z3 context is currently set up for this thread.
    /// </summary>
    public static bool IsCurrentContextSet => currentContext.Value != null;

    /// <summary>
    /// Sets up this context as the current thread-local context, enabling natural mathematical syntax.
    /// Use within a using statement to automatically restore the previous context.
    /// </summary>
    /// <returns>A disposable scope that restores the previous context when disposed.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the context has been disposed.</exception>
    public SetUpScope SetUp()
    {
        ThrowIfDisposed();
        return new SetUpScope(this);
    }

    /// <summary>
    /// Represents a scoped context setup that enables natural mathematical syntax within its lifetime.
    /// Automatically restores the previous thread-local context when disposed.
    /// </summary>
    public sealed class SetUpScope : IDisposable
    {
        private readonly Z3Context? previousContext;

        internal SetUpScope(Z3Context context)
        {
            previousContext = currentContext.Value;
            currentContext.Value = context;
        }

        /// <summary>
        /// Restores the previous thread-local Z3 context.
        /// </summary>
        public void Dispose()
        {
            currentContext.Value = previousContext;
        }
    }

    internal IntPtr GetSortForType<T>()
        where T : Z3Expr, IZ3ExprType<T> => T.GetSort(this);
}
