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

    // Signed comparison methods (for explicit signed comparisons)
    public Z3BoolExpr SignedLt(Z3BitVecExpr other) => Context.Slt(this, other);
    public Z3BoolExpr SignedLe(Z3BitVecExpr other) => Context.Sle(this, other);
    public Z3BoolExpr SignedGt(Z3BitVecExpr other) => Context.Sgt(this, other);
    public Z3BoolExpr SignedGe(Z3BitVecExpr other) => Context.Sge(this, other);

    // Signed division methods (for explicit signed operations)
    public Z3BitVecExpr SignedDiv(Z3BitVecExpr other) => Context.SDiv(this, other);
    public Z3BitVecExpr SignedRem(Z3BitVecExpr other) => Context.SRem(this, other);
    public Z3BitVecExpr SignedMod(Z3BitVecExpr other) => Context.SMod(this, other);

    // Shift methods (for explicit shift types)
    public Z3BitVecExpr LogicalShiftLeft(Z3BitVecExpr amount) => Context.Shl(this, amount);
    public Z3BitVecExpr LogicalShiftRight(Z3BitVecExpr amount) => Context.Lshr(this, amount);
    public Z3BitVecExpr ArithmeticShiftRight(Z3BitVecExpr amount) => Context.Ashr(this, amount);

    // Arithmetic operators (unsigned by default)
    public static Z3BitVecExpr operator +(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Add(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.UDiv(left, right);
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.URem(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr operand) => operand.Context.Neg(operand);

    // Bitwise operators
    public static Z3BitVecExpr operator &(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.And(left, right);
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Xor(left, right);
    public static Z3BitVecExpr operator ~(Z3BitVecExpr operand) => operand.Context.Not(operand);

    // Shift operators (logical by default)
    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Shl(left, right);
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Lshr(left, right);

    // Comparison operators (unsigned by default)
    public static Z3BoolExpr operator <(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Ult(left, right);
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Ule(left, right);
    public static Z3BoolExpr operator >(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Ugt(left, right);
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Uge(left, right);
}