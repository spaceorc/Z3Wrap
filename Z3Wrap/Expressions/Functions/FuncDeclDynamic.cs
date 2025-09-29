using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

/// <summary>
/// Represents a dynamically typed function declaration that can be applied to arguments.
/// </summary>
/// <typeparam name="TResult">Return type of the function.</typeparam>
public sealed class FuncDeclDynamic<TResult> : Z3FuncDecl<TResult>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDeclDynamic(Z3Context context, IntPtr handle, string name, uint arity)
        : base(context, handle, name, arity) { }

    /// <summary>
    /// Applies this function declaration to the specified arguments.
    /// </summary>
    /// <param name="args">Arguments to apply to the function.</param>
    /// <returns>Function application expression.</returns>
    /// <exception cref="ArgumentException">Thrown when argument count doesn't match function arity.</exception>
    public TResult Apply(params Z3Expr[] args)
    {
        if (args.Length != Arity)
            throw new ArgumentException($"Expected {Arity} arguments, but got {args.Length}.");

        return Context.Apply(this, args);
    }
}
