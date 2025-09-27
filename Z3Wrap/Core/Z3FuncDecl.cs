using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Core;

public abstract class Z3FuncDecl<TResult> : Z3Handle
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name, uint arity)
        : base(context, handle)
    {
        Name = name;
        Arity = arity;
    }

    public string Name { get; }

    public uint Arity { get; }
}
