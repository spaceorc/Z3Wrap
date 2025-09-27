namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Defines the contract for Z3 expression types that can be created from native handles.
/// </summary>
/// <typeparam name="T">The specific Z3 expression type that implements this interface.</typeparam>
public interface IExprType<out T>
    where T : Z3Expr
{
    internal static abstract T Create(Z3Context context, IntPtr handle);
    internal static abstract IntPtr Sort(Z3Context context);
}
