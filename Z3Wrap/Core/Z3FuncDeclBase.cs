namespace Spaceorc.Z3Wrap.Core;

public abstract class Z3FuncDeclBase<TResult> : Z3Handle
    where TResult : Z3Expr
{
    internal Z3FuncDeclBase(Z3Context context, IntPtr handle, string name, uint arity)
        : base(context, handle)
    {
        Name = name;
        Arity = arity;
    }

    public string Name { get; }

    public uint Arity { get; }
}
