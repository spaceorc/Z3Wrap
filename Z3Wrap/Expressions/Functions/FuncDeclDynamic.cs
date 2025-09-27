using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

public class FuncDeclDynamic<TResult> : Z3FuncDecl<TResult>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDeclDynamic(Z3Context context, IntPtr handle, string name, uint arity)
        : base(context, handle, name, arity) { }

    public TResult Apply(params Z3Expr[] args)
    {
        if (args.Length != Arity)
            throw new ArgumentException($"Expected {Arity} arguments, but got {args.Length}.");

        return Context.Apply(this, args);
    }
}
