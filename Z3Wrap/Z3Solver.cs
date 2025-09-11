using System.Runtime.InteropServices;

namespace Z3Wrap;

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
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        
        solverHandle = useSimpleSolver 
            ? NativeMethods.Z3MkSimpleSolver(context.Handle)
            : NativeMethods.Z3MkSolver(context.Handle);
            
        if (solverHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 solver");
        
        NativeMethods.Z3SolverIncRef(context.Handle, solverHandle);
    }

    public IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return solverHandle;
        }
    }

    internal IntPtr InternalHandle => solverHandle;

    public void Assert(Z3BoolExpr constraint)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(constraint);
        InvalidateModel(); // Model no longer valid after assertion
        
        NativeMethods.Z3SolverAssert(context.Handle, solverHandle, constraint.Handle);
    }

    public void Reset()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after reset
        
        NativeMethods.Z3SolverReset(context.Handle, solverHandle);
        lastCheckResult = null;
    }

    public Z3Status Check()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Clear any previous model
        
        var result = NativeMethods.Z3SolverCheck(context.Handle, solverHandle);
        lastCheckResult = (Z3Status)result;
        return lastCheckResult.Value;
    }

    public string GetReasonUnknown()
    {
        ThrowIfDisposed();
        
        var reasonPtr = NativeMethods.Z3SolverGetReasonUnknown(context.Handle, solverHandle);
        return Marshal.PtrToStringAnsi(reasonPtr) ?? "Unknown reason";
    }

    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push
        
        NativeMethods.Z3SolverPush(context.Handle, solverHandle);
    }

    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop
        
        NativeMethods.Z3SolverPop(context.Handle, solverHandle, numScopes);
    }

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
            var modelHandle = NativeMethods.Z3SolverGetModel(context.Handle, solverHandle);
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

    private void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(Z3Solver));
    }
}