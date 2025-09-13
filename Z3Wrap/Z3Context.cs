using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public class Z3Context : IDisposable
{
    private static readonly ThreadLocal<Z3Context?> currentContext = new(() => null);

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
        NativeMethods.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
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

    private void TrackExpression(IntPtr handle)
    {
        ThrowIfDisposed();
        NativeMethods.Z3IncRef(contextHandle, handle);
        trackedExpressions.Add(handle);
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
            NativeMethods.Z3SolverDecRef(contextHandle, solverHandle);
        
        solver.InternalDispose();
    }

    internal Z3Expr WrapExpr(IntPtr handle)
    {
        ThrowIfDisposed();
        
        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid expression handle", nameof(handle));
        
        var sort = NativeMethods.Z3GetSort(contextHandle, handle);
        var sortKind = NativeMethods.Z3GetSortKind(contextHandle, sort);
        
        TrackExpression(handle);
        
        return (Z3SortKind)sortKind switch
        {
            Z3SortKind.Bool => new Z3BoolExpr(this, handle),
            Z3SortKind.Int => new Z3IntExpr(this, handle),
            Z3SortKind.Real => new Z3RealExpr(this, handle),
            Z3SortKind.Array => throw new InvalidOperationException("Array expressions must be wrapped with specific generic types. Use WrapArrayExpr<TIndex, TValue>() instead."),
            _ => throw new InvalidOperationException($"Unsupported sort kind: {sortKind}")
        };
    }

    internal Z3IntExpr WrapIntExpr(IntPtr handle)
    {
        TrackExpression(handle);
        return new Z3IntExpr(this, handle);
    }

    internal Z3RealExpr WrapRealExpr(IntPtr handle)
    {
        TrackExpression(handle);
        return new Z3RealExpr(this, handle);
    }

    internal Z3BoolExpr WrapBoolExpr(IntPtr handle)
    {
        TrackExpression(handle);
        return new Z3BoolExpr(this, handle);
    }

    internal Z3ArrayExpr<TIndex, TValue> WrapArrayExpr<TIndex, TValue>(IntPtr handle)
        where TIndex : Z3Expr
        where TValue : Z3Expr
    {
        TrackExpression(handle);
        return new Z3ArrayExpr<TIndex, TValue>(this, handle);
    }

    internal void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, typeof(Z3Context));

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

    public static Z3Context Current => currentContext.Value ?? throw new InvalidOperationException(
        "No Z3Context is currently set. Use 'using var scope = context.SetUp()' to enable implicit conversions.");

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
}