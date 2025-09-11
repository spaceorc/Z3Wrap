using System.Runtime.InteropServices;

namespace z3lib;

public sealed class Z3Model
{
    private readonly Z3Context context;
    private IntPtr modelHandle;
    private bool invalidated;

    internal Z3Model(Z3Context context, IntPtr handle)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        
        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid model handle", nameof(handle));
            
        modelHandle = handle;
        
        // Critical: increment ref count immediately to keep model alive
        NativeMethods.Z3ModelIncRef(context.Handle, handle);
    }

    public IntPtr Handle
    {
        get
        {
            ThrowIfInvalidated();
            return modelHandle;
        }
    }

    public override string ToString()
    {
        if (invalidated) 
            return "<invalidated>";
        
        try
        {
            context.ThrowIfDisposed();
            var ptr = NativeMethods.Z3ModelToString(context.Handle, modelHandle);
            return Marshal.PtrToStringAnsi(ptr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
        catch
        {
            return "<error>";
        }
    }

    internal void Invalidate()
    {
        if (!invalidated && modelHandle != IntPtr.Zero)
        {
            try
            {
                // Only dec ref if context is still alive
                context.ThrowIfDisposed(); // This will throw if disposed
                NativeMethods.Z3ModelDecRef(context.Handle, modelHandle);
            }
            catch
            {
                // Context might be disposed, ignore cleanup errors
                // The native Z3 context cleanup will handle the model cleanup
            }
            
            modelHandle = IntPtr.Zero;
            invalidated = true;
        }
    }

    private void ThrowIfInvalidated()
    {
        if (invalidated)
            throw new ObjectDisposedException(nameof(Z3Model), "Model has been invalidated due to solver state change");
    }
}