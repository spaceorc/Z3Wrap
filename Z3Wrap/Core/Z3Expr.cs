using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents the base class for all Z3 expressions in the theorem prover.
/// </summary>
public abstract class Z3Expr(Z3Context context, IntPtr handle) : Z3Handle(context, handle)
{
    internal static T Create<T>(Z3Context context, IntPtr handle)
        where T : Z3Expr, IExprType<T>
    {
        return T.Create(context, handle);
    }
}
