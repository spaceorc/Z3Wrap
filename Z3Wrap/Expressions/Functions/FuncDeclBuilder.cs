using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

/// <summary>
/// Builder for creating function declarations with specified argument types.
/// </summary>
/// <typeparam name="TResult">Return type of the function declaration.</typeparam>
public sealed class FuncDeclBuilder<TResult>(Z3Context context, string name)
    where TResult : Z3Expr, IExprType<TResult>
{
    private readonly List<Func<IntPtr>> ranges = [];

    /// <summary>
    /// Adds an argument type to the function declaration.
    /// </summary>
    /// <typeparam name="TArg">Argument type to add.</typeparam>
    /// <returns>This builder for method chaining.</returns>
    public FuncDeclBuilder<TResult> WithArg<TArg>()
        where TArg : Z3Expr, IExprType<TArg>
    {
        ranges.Add(context.GetSortForType<TArg>);
        return this;
    }

    /// <summary>
    /// Builds the function declaration with configured argument types.
    /// </summary>
    /// <returns>Dynamic function declaration with specified signature.</returns>
    public FuncDeclDynamic<TResult> Build()
    {
        var symbol = context.Library.Z3MkStringSymbol(context.Handle, name);
        var domainSorts = ranges.Select(r => r()).ToArray();
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = context.Library.Z3MkFuncDecl(
            context.Handle,
            symbol,
            (uint)domainSorts.Length,
            domainSorts,
            rangeSort
        );

        return new FuncDeclDynamic<TResult>(context, funcDeclHandle, name, (uint)domainSorts.Length);
    }
}
