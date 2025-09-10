using System.Runtime.InteropServices;

namespace z3lib;

public partial class Z3Context : IDisposable
{
    private IntPtr configHandle;
    private IntPtr contextHandle;
    private bool disposed;
    private readonly HashSet<IntPtr> trackedExpressions = new();

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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void SetParameter(string paramName, string paramValue)
    {
        ThrowIfDisposed();

        var paramNamePtr = Marshal.StringToHGlobalAnsi(paramName);
        var paramValuePtr = Marshal.StringToHGlobalAnsi(paramValue);

        try
        {
            NativeMethods.Z3UpdateParamValue(contextHandle, paramNamePtr, paramValuePtr);
        }
        finally
        {
            Marshal.FreeHGlobal(paramNamePtr);
            Marshal.FreeHGlobal(paramValuePtr);
        }
    }

    private void TrackExpression(IntPtr handle)
    {
        ThrowIfDisposed();
        NativeMethods.Z3IncRef(contextHandle, handle);
        trackedExpressions.Add(handle);
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(Z3Context));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (contextHandle != IntPtr.Zero)
            {
                // Clean up all tracked expressions first
                foreach (var exprHandle in trackedExpressions)
                {
                    NativeMethods.Z3DecRef(contextHandle, exprHandle);
                }
                trackedExpressions.Clear();

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
    }

    ~Z3Context()
    {
        Dispose(false);
    }
}