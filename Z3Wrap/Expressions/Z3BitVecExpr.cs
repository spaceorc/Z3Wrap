namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3BitVecExpr : Z3NumericExpr
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

    // Fluent API for bitvector operations
    public Z3BitVecExpr Extend(uint additionalBits) => Context.Extend(this, additionalBits);
    public Z3BitVecExpr SignedExtend(uint additionalBits) => Context.SignedExtend(this, additionalBits);
    public Z3BitVecExpr Extract(uint high, uint low) => Context.Extract(this, high, low);
    public Z3BitVecExpr Resize(uint newSize) => Context.Resize(this, newSize);
    public Z3BitVecExpr SignedResize(uint newSize) => Context.SignedResize(this, newSize);
    public Z3IntExpr ToInt() => Context.ToInt(this);
    public Z3IntExpr ToSignedInt() => Context.ToSignedInt(this);
}