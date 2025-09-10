using System.Runtime.InteropServices;

namespace z3lib;

public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private IntPtr solverHandle;
    private bool disposed;

    internal Z3Solver(Z3Context context, bool useSimpleSolver = false)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        
        // Check if context is disposed before using it
        var contextHandle = context.Handle; // This will throw if context is disposed
        
        solverHandle = useSimpleSolver 
            ? NativeMethods.Z3MkSimpleSolver(contextHandle)
            : NativeMethods.Z3MkSolver(contextHandle);
            
        if (solverHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 solver");
        
        NativeMethods.Z3SolverIncRef(contextHandle, solverHandle);
    }

    public IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return solverHandle;
        }
    }

    public void Assert(Z3BoolExpr constraint)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(constraint);
        
        var contextHandle = context.Handle; // This will throw if context is disposed
        NativeMethods.Z3SolverAssert(contextHandle, solverHandle, constraint.Handle);
    }

    public Z3Status Check()
    {
        ThrowIfDisposed();
        
        var contextHandle = context.Handle; // This will throw if context is disposed
        var result = NativeMethods.Z3SolverCheck(contextHandle, solverHandle);
        return (Z3Status)result;
    }

    public string GetReasonUnknown()
    {
        ThrowIfDisposed();
        
        var contextHandle = context.Handle; // This will throw if context is disposed
        var reasonPtr = NativeMethods.Z3SolverGetReasonUnknown(contextHandle, solverHandle);
        return Marshal.PtrToStringAnsi(reasonPtr) ?? "Unknown reason";
    }

    public void Push()
    {
        ThrowIfDisposed();
        
        var contextHandle = context.Handle; // This will throw if context is disposed
        NativeMethods.Z3SolverPush(contextHandle, solverHandle);
    }

    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        
        var contextHandle = context.Handle; // This will throw if context is disposed
        NativeMethods.Z3SolverPop(contextHandle, solverHandle, numScopes);
    }


    public void Dispose()
    {
        if (disposed)
            return;

        if (solverHandle != IntPtr.Zero)
        {
            // Only try to decrement reference if context is still alive
            try
            {
                var contextHandle = context.Handle;
                NativeMethods.Z3SolverDecRef(contextHandle, solverHandle);
            }
            catch (ObjectDisposedException)
            {
                // Context is already disposed, can't safely decrement reference
                // This is okay - Z3 will clean up when context is disposed
            }
            
            solverHandle = IntPtr.Zero;
        }

        disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(Z3Solver));
    }
}