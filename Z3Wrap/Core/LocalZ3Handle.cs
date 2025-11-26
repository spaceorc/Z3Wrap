namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a temporary Z3 AST handle that is automatically cleaned up when disposed.
/// Use this for intermediate Z3 objects that don't need expression wrappers.
/// </summary>
public sealed class LocalZ3Handle : IDisposable
{
    private readonly Z3Context context;
    private bool disposed;

    /// <summary>
    /// Creates a local Z3 handle that will be reference-counted and cleaned up on disposal.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="handle">The Z3 AST handle.</param>
    public LocalZ3Handle(Z3Context context, IntPtr handle)
    {
        this.context = context;
        Handle = handle;
        context.Library.IncRef(context.Handle, handle);
    }

    /// <summary>
    /// Gets the underlying Z3 AST handle.
    /// </summary>
    public IntPtr Handle { get; }

    /// <summary>
    /// Releases the Z3 AST handle reference.
    /// </summary>
    public void Dispose()
    {
        if (!disposed)
        {
            context.Library.DecRef(context.Handle, Handle);
            disposed = true;
        }
    }
}
