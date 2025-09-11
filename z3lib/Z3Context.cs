namespace z3lib;

public partial class Z3Context : IDisposable
{
    private readonly HashSet<IntPtr> trackedExpressions = [];
    private readonly HashSet<Z3Solver> trackedSolvers = [];
    private IntPtr configHandle;
    private IntPtr contextHandle;
    private bool disposed;

    public Z3Context()
    {
        configHandle = NativeMethods.Z3MkConfig();
        if (configHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 configuration");

        contextHandle = NativeMethods.Z3MkContextRc(configHandle);
        if (contextHandle == IntPtr.Zero)
        {
            NativeMethods.Z3DelConfig(configHandle);
            throw new InvalidOperationException("Failed to create Z3 context");
        }
    }

    public Z3Context(Dictionary<string, string> parameters) : this()
    {
        foreach (var param in parameters) SetParameter(param.Key, param.Value);
    }

    public IntPtr Handle
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
        NativeMethods.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
    }

    internal void TrackExpression(IntPtr handle)
    {
        ThrowIfDisposed();
        NativeMethods.Z3IncRef(contextHandle, handle);
        trackedExpressions.Add(handle);
    }

    internal void TrackSolver(Z3Solver solver)
    {
        ThrowIfDisposed();
        trackedSolvers.Add(solver);
    }

    internal void UntrackSolver(Z3Solver solver)
    {
        trackedSolvers.Remove(solver);
    }

    internal void DisposeSolver(Z3Solver solver)
    {
        if (disposed)
            return; // Context is already disposed, nothing to do

        UntrackSolver(solver);
        
        var solverHandle = solver.InternalHandle;
        if (solverHandle != IntPtr.Zero)
        {
            NativeMethods.Z3SolverDecRef(contextHandle, solverHandle);
        }
        
        solver.InternalDispose();
    }

    internal void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(Z3Context));
    }

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

            // Then clean up all tracked expressions
            foreach (var exprHandle in trackedExpressions)
                NativeMethods.Z3DecRef(contextHandle, exprHandle);

            trackedExpressions.Clear();

            // Finally dispose the context itself
            NativeMethods.Z3DelContext(contextHandle);
            contextHandle = IntPtr.Zero;
        }

        if (configHandle != IntPtr.Zero)
        {
            NativeMethods.Z3DelConfig(configHandle);
            configHandle = IntPtr.Zero;
        }

        disposed = true;
    }

    ~Z3Context()
    {
        DisposeCore();
    }
}