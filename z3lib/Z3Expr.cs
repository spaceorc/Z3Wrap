namespace z3lib;

public abstract class Z3Expr(Z3Context context, IntPtr handle)
{
    protected readonly Z3Context context = context ?? throw new ArgumentNullException(nameof(context));
    protected readonly IntPtr handle = handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));

    public IntPtr Handle => handle;

    public Z3Context Context => context;

    public Z3BoolExpr Equals(Z3Expr other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        return context.MkEq(this, other);
    }

    public static Z3BoolExpr operator ==(Z3Expr? left, Z3Expr? right)
    {
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            throw new ArgumentNullException();
        return left.Equals(right);
    }

    public static Z3BoolExpr operator !=(Z3Expr? left, Z3Expr? right)
    {
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            throw new ArgumentNullException();
        var eq = left.Equals(right);
        return left.context.MkNot(eq);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Z3Expr expr)
        {
            return Handle.Equals(expr.Handle);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return handle.GetHashCode();
    }
}