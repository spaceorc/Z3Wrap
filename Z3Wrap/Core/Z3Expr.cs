using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Base class for all Z3 expressions, providing common functionality for equality, inequality, and string representation.
/// All Z3 expressions are immutable and associated with a specific Z3 context.
/// </summary>
public abstract class Z3Expr
{
    /// <summary>
    /// Base class for all Z3 expressions, providing common functionality for equality, inequality, and string representation.
    /// All Z3 expressions are immutable and associated with a specific Z3 context.
    /// </summary>
    protected Z3Expr(Z3Context context, IntPtr handle)
    {
        Handle =
            handle != IntPtr.Zero
                ? handle
                : throw new ArgumentException("Invalid handle", nameof(handle));
        context.TrackAstNode(handle);
        Context = context;
    }

    internal static T Create<T>(Z3Context context, IntPtr handle)
        where T : Z3Expr, IExprType<T>
    {
        return T.Create(context, handle);
    }

    internal IntPtr Handle { get; }

    /// <summary>
    /// Gets the Z3 context that owns this expression.
    /// </summary>
    public Z3Context Context { get; }

    /// <summary>
    /// Determines whether two Z3 expressions are equal using the == operator.
    /// Creates a Boolean expression representing the equality constraint.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(Z3Expr left, Z3Expr right) => left.Eq(right);

    /// <summary>
    /// Determines whether two Z3 expressions are not equal using the != operator.
    /// Creates a Boolean expression representing the inequality constraint.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(Z3Expr left, Z3Expr right) => left.Neq(right);

    /// <summary>
    /// Creates a Boolean expression representing equality with another expression.
    /// </summary>
    /// <param name="other">The expression to compare with.</param>
    /// <returns>A Boolean expression representing this == other.</returns>
    public BoolExpr Eq(Z3Expr other) => Context.Eq(this, other);

    /// <summary>
    /// Creates a Boolean expression representing inequality with another expression.
    /// </summary>
    /// <param name="other">The expression to compare with.</param>
    /// <returns>A Boolean expression representing this != other.</returns>
    public BoolExpr Neq(Z3Expr other) => Context.Neq(this, other);

    /// <summary>
    /// Determines whether this expression is equal to the specified object.
    /// Uses handle-based equality for Z3 expressions.
    /// </summary>
    /// <param name="obj">The object to compare with this expression.</param>
    /// <returns>true if the object is a Z3Expr with the same handle; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Z3Expr expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    /// <summary>
    /// Returns the hash code for this expression based on its Z3 handle.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Handle.GetHashCode();

    /// <summary>
    /// Returns a string representation of this expression in Z3's native format.
    /// </summary>
    /// <returns>A string representation of the expression, or status information if disposed/invalid.</returns>
    public override string ToString()
    {
        try
        {
            var stringPtr = SafeNativeMethods.Z3AstToString(Context.Handle, Handle);
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(stringPtr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
    }
}
