namespace z3lib;

public abstract class Z3Expr(Z3Context context, IntPtr handle)
{
    public IntPtr Handle { get; } = handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));
    public Z3Context Context { get; } = context ?? throw new ArgumentNullException(nameof(context));

    public static Z3BoolExpr operator ==(Z3Expr left, Z3Expr right) => left.Eq(right);
    public static Z3BoolExpr operator !=(Z3Expr left, Z3Expr right) => left.Neq(right);
    
    public Z3BoolExpr Eq(Z3Expr other) => Context.Eq(this, other);
    public Z3BoolExpr Neq(Z3Expr other) => !Eq(other);

    public override bool Equals(object? obj)
    {
        if (obj is Z3Expr expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    public override int GetHashCode() => handle.GetHashCode();

    public override string ToString()
    {
        try
        {
            Context.ThrowIfDisposed();
            var stringPtr = NativeMethods.Z3AstToString(Context.Handle, Handle);
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(stringPtr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
        catch
        {
            return "<error>";
        }
    }
}