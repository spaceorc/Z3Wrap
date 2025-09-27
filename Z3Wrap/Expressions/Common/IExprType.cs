using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.Common;

public interface IExprType<out T>
    where T : Z3Expr
{
    internal static abstract T Create(Z3Context context, IntPtr handle);
    internal static abstract IntPtr Sort(Z3Context context);
}
