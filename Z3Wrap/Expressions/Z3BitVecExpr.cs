namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3BitVecExpr : Z3Expr
{
    public uint Size { get; }

    internal Z3BitVecExpr(Z3Context context, IntPtr handle, uint size) : base(context, handle)
    {
        Size = size;
    }

    public new static Z3BitVecExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3BitVecExpr)Z3Expr.Create(context, handle);
    }
    
    public override string ToString() => $"BitVec[{Size}]({base.ToString()})";
}