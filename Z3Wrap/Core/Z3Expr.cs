using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Core;

public abstract class Z3Expr
{
    protected Z3Expr(Z3Context context, IntPtr handle)
    {
        Handle = handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));
        context.TrackAstNode(handle);
        Context = context;
    }

    internal static T Create<T>(Z3Context context, IntPtr handle)
        where T : Z3Expr, IExprType<T>
    {
        return T.Create(context, handle);
    }

    internal IntPtr Handle { get; }

    public Z3Context Context { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Z3Expr expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    public override int GetHashCode() => Handle.GetHashCode();

    public override string ToString()
    {
        try
        {
            var stringPtr = SafeNativeMethods.Z3AstToString(Context.Handle, Handle);
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(stringPtr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
    }
}
