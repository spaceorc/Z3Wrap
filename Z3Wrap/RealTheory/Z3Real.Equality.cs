using Spaceorc.Z3Wrap.BoolTheory;

namespace Spaceorc.Z3Wrap.RealTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class Z3Real
{
    /// <summary>
    /// Checks equality between a real expression and a Real value using the == operator.
    /// </summary>
    /// <param name="left">The real expression.</param>
    /// <param name="right">The Real value.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Z3Real left, Real right) =>
        left.Eq(left.Context.Real(right));

    /// <summary>
    /// Checks inequality between a real expression and a Real value using the != operator.
    /// </summary>
    /// <param name="left">The real expression.</param>
    /// <param name="right">The Real value.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Z3Real left, Real right) =>
        left.Neq(left.Context.Real(right));

    /// <summary>
    /// Checks equality between a Real value and a real expression using the == operator.
    /// </summary>
    /// <param name="left">The Real value.</param>
    /// <param name="right">The real expression.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Real left, Z3Real right) =>
        right.Context.Real(left).Eq(right);

    /// <summary>
    /// Checks inequality between a Real value and a real expression using the != operator.
    /// </summary>
    /// <param name="left">The Real value.</param>
    /// <param name="right">The real expression.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Real left, Z3Real right) =>
        right.Context.Real(left).Neq(right);
}