using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Base class for Z3 function declarations that can be used to create function applications.
/// Function declarations define the signature (domain and range sorts) of uninterpreted functions.
/// These are not expressions themselves, but are used to create expressions via function application.
/// </summary>
/// <typeparam name="TResult">The result type of the function.</typeparam>
public abstract class Z3FuncDeclBase<TResult>
    where TResult : Z3Expr
{
    internal IntPtr Handle { get; }

    /// <summary>
    /// Gets the Z3 context that owns this function declaration.
    /// </summary>
    public Z3Context Context { get; }

    /// <summary>
    /// Gets the name of this function declaration.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the arity (number of parameters) of this function.
    /// </summary>
    public uint Arity { get; }

    internal Z3FuncDeclBase(Z3Context context, IntPtr handle, string name, uint arity)
    {
        Context = context;
        Handle =
            handle != IntPtr.Zero
                ? handle
                : throw new ArgumentException(
                    "Invalid function declaration handle",
                    nameof(handle)
                );
        Name = name;
        Arity = arity;

        // Track this AST node for proper memory management
        context.TrackAstNode(handle);
    }

    /// <summary>
    /// Returns a string representation of this function declaration showing its signature.
    /// </summary>
    /// <returns>A string representation of the function declaration.</returns>
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

    /// <summary>
    /// Determines whether this function declaration is equal to the specified object.
    /// Uses handle-based equality for Z3 function declarations.
    /// </summary>
    /// <param name="obj">The object to compare with this function declaration.</param>
    /// <returns>true if the object is a Z3FuncDeclBase with the same handle; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Z3FuncDeclBase<TResult> funcDecl)
            return Handle.Equals(funcDecl.Handle);
        return false;
    }

    /// <summary>
    /// Returns the hash code for this function declaration based on its Z3 handle.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Handle.GetHashCode();
}
