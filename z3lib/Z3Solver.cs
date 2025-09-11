using System.Runtime.InteropServices;

namespace z3lib;

public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private IntPtr solverHandle;
    private bool disposed;
    private bool isBeingDisposedByContext;

    internal Z3Solver(Z3Context context, bool useSimpleSolver = false)
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
        
        NativeMethods.Z3SolverAssert(context.Handle, solverHandle, constraint.Handle);
    }

    public Z3Status Check()
    {
        ThrowIfDisposed();
        
        var result = NativeMethods.Z3SolverCheck(context.Handle, solverHandle);
        return (Z3Status)result;
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
        
        NativeMethods.Z3SolverPush(context.Handle, solverHandle);
    }

    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        
        NativeMethods.Z3SolverPop(context.Handle, solverHandle, numScopes);
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