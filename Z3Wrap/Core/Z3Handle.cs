using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents base class for Z3 resource management and handle tracking.
/// </summary>
public abstract class Z3Handle
{
    /// <summary>
    /// Initializes a new Z3Handle with context and handle.
    /// </summary>
    /// <param name="context">Z3 context to track this handle.</param>
    /// <param name="handle">Native Z3 handle.</param>
    protected Z3Handle(Z3Context context, IntPtr handle)
    {
        Handle = handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));
        context.TrackHandle(handle);
        Context = context;
    }

    internal IntPtr Handle { get; }

    /// <summary>
    /// Gets the Z3 context that owns this handle.
    /// </summary>
    public Z3Context Context { get; }

    /// <summary>
    /// Determines whether this handle equals another object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if objects are equal; otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Z3Handle expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    /// <summary>
    /// Gets the hash code for this handle.
    /// </summary>
    /// <returns>The hash code of the native handle.</returns>
    public override int GetHashCode()
    {
        return Handle.GetHashCode();
    }

    /// <summary>
    /// Returns the string representation of this Z3 object.
    /// </summary>
    /// <returns>The Z3 string representation.</returns>
    public override string ToString()
    {
        try
        {
            return Context.Library.Z3AstToString(Context.Handle, Handle) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
    }
}
