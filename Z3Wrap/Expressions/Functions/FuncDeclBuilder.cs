using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

public class FuncDeclBuilder<TResult>(Z3Context context, string name)
    where TResult : Z3Expr, IExprType<TResult>
{
    private readonly List<Func<IntPtr>> ranges = [];

    public FuncDeclBuilder<TResult> WithArg<TArg>()
        where TArg : Z3Expr, IExprType<TArg>
    {
        ranges.Add(context.GetSortForType<TArg>);
        return this;
    }

    public FuncDeclDynamic<TResult> Build()
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = ranges.Select(r => r()).ToArray();
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = SafeNativeMethods.Z3MkFuncDecl(
            context.Handle,
            symbol,
            (uint)domainSorts.Length,
            domainSorts,
            rangeSort
        );

        return new FuncDeclDynamic<TResult>(context, funcDeclHandle, name, (uint)domainSorts.Length);
    }
}
