using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public abstract class Z3Handle
{
    protected Z3Handle(Z3Context context, IntPtr handle)
    {
        Handle = handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));
        context.TrackHandle(handle);
        Context = context;
    }

    internal IntPtr Handle { get; }

    public Z3Context Context { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Z3Handle expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    public override int GetHashCode()
    {
        return Handle.GetHashCode();
    }

    public override string ToString()
    {
        try
        {
            var stringPtr = SafeNativeMethods.Z3AstToString(Context.Handle, Handle);
            return Marshal.PtrToStringAnsi(stringPtr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
    }
}
