using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents abstract base class for Z3 function declarations with typed result.
/// </summary>
/// <typeparam name="TResult">The result type of the function.</typeparam>
public abstract class Z3FuncDecl<TResult> : Z3Handle
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name, uint arity)
        : base(context, handle)
    {
        Name = name;
        Arity = arity;
    }

    /// <summary>
    /// Gets the function name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the number of function arguments.
    /// </summary>
    public uint Arity { get; }
}
