using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public abstract class Z3FuncDeclBase<TResult>
    where TResult : Z3Expr
{
    internal IntPtr Handle { get; }

    public Z3Context Context { get; }

    public string Name { get; }

    public uint Arity { get; }

    internal Z3FuncDeclBase(Z3Context context, IntPtr handle, string name, uint arity)
    {
        Context = context;
        Handle =
            handle != IntPtr.Zero
                ? handle
                : throw new ArgumentException("Invalid function declaration handle", nameof(handle));
        Name = name;
        Arity = arity;

        // Track this AST node for proper memory management
        context.TrackAstNode(handle);
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

    public override bool Equals(object? obj)
    {
        if (obj is Z3FuncDeclBase<TResult> funcDecl)
            return Handle.Equals(funcDecl.Handle);
        return false;
    }

    public override int GetHashCode() => Handle.GetHashCode();
}
