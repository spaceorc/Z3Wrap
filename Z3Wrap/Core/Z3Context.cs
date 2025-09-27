using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Core;

public class Z3Context : IDisposable
{
    private static readonly ThreadLocal<Z3Context?> currentContext = new(() => null);

    private readonly HashSet<IntPtr> trackedAstNodes = [];
    private readonly HashSet<Z3Solver> trackedSolvers = [];
    private IntPtr configHandle;
    private IntPtr contextHandle;
    private bool disposed;

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

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }

    public void SetParameter(string paramName, string paramValue)
    {
        ThrowIfDisposed();

        using var paramNamePtr = new AnsiStringPtr(paramName);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        SafeNativeMethods.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
    }

    public Z3Solver CreateSolver()
    {
        var solver = new Z3Solver(this, false);
        TrackSolver(solver);
        return solver;
    }

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

    ~Z3Context()
    {
        DisposeCore();
    }

    public static Z3Context Current =>
        currentContext.Value
        ?? throw new InvalidOperationException(
            "No Z3Context is currently set. Use 'using var scope = context.SetUp()' to enable implicit conversions."
        );

    public static bool IsCurrentContextSet => currentContext.Value != null;

    public SetUpScope SetUp()
    {
        ThrowIfDisposed();
        return new SetUpScope(this);
    }

    public sealed class SetUpScope : IDisposable
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
