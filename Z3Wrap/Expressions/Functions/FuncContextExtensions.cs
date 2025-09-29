using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

/// <summary>
/// Provides function declaration and application methods for Z3Context.
/// </summary>
public static class FuncContextExtensions
{
    /// <summary>
    /// Creates function declaration builder for constructing functions with complex signatures.
    /// </summary>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The function name.</param>
    /// <returns>Function declaration builder for the specified result type.</returns>
    public static FuncDeclBuilder<TResult> FuncBuilder<TResult>(this Z3Context context, string name)
        where TResult : Z3Expr, IExprType<TResult>
    {
        return new FuncDeclBuilder<TResult>(context, name);
    }

    /// <summary>
    /// Creates function declaration with no parameters (constant function).
    /// </summary>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The function name.</param>
    /// <returns>Function declaration with no parameters.</returns>
    public static FuncDecl<TResult> Func<TResult>(this Z3Context context, string name)
        where TResult : Z3Expr, IExprType<TResult>
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = context.Library.Z3MkStringSymbol(context.Handle, namePtr);
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = context.Library.Z3MkFuncDecl(
            context.Handle,
            symbol,
            0, // domain size (0 for constants)
            [], // empty domain array
            rangeSort
        );

        return new FuncDecl<TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates function declaration with one parameter.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The function name.</param>
    /// <returns>Function declaration with one parameter.</returns>
    public static FuncDecl<T1, TResult> Func<T1, TResult>(this Z3Context context, string name)
        where T1 : Z3Expr, IExprType<T1>
        where TResult : Z3Expr, IExprType<TResult>
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = context.Library.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[] { context.GetSortForType<T1>() };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = context.Library.Z3MkFuncDecl(
            context.Handle,
            symbol,
            1, // domain size
            domainSorts,
            rangeSort
        );

        return new FuncDecl<T1, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates function declaration with two parameters.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The function name.</param>
    /// <returns>Function declaration with two parameters.</returns>
    public static FuncDecl<T1, T2, TResult> Func<T1, T2, TResult>(this Z3Context context, string name)
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where TResult : Z3Expr, IExprType<TResult>
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = context.Library.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[] { context.GetSortForType<T1>(), context.GetSortForType<T2>() };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = context.Library.Z3MkFuncDecl(
            context.Handle,
            symbol,
            2, // domain size
            domainSorts,
            rangeSort
        );

        return new FuncDecl<T1, T2, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates function declaration with three parameters.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="T3">Third parameter type.</typeparam>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The function name.</param>
    /// <returns>Function declaration with three parameters.</returns>
    public static FuncDecl<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(this Z3Context context, string name)
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where T3 : Z3Expr, IExprType<T3>
        where TResult : Z3Expr, IExprType<TResult>
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = context.Library.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[]
        {
            context.GetSortForType<T1>(),
            context.GetSortForType<T2>(),
            context.GetSortForType<T3>(),
        };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = context.Library.Z3MkFuncDecl(
            context.Handle,
            symbol,
            3, // domain size
            domainSorts,
            rangeSort
        );

        return new FuncDecl<T1, T2, T3, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates function application expression.
    /// </summary>
    /// <typeparam name="TResult">Function return type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="funcDecl">The function declaration to apply.</param>
    /// <param name="args">The arguments to apply to the function.</param>
    /// <returns>Expression representing function application.</returns>
    public static TResult Apply<TResult>(this Z3Context context, Z3FuncDecl<TResult> funcDecl, params Z3Expr[] args)
        where TResult : Z3Expr, IExprType<TResult>
    {
        if (args.Length != funcDecl.Arity)
            throw new ArgumentException(
                $"Function has arity {funcDecl.Arity}, but {args.Length} arguments provided",
                nameof(args)
            );

        var argHandles = args.Select(arg => arg.Handle).ToArray();
        var appHandle = context.Library.Z3MkApp(context.Handle, funcDecl.Handle, (uint)args.Length, argHandles);

        return Z3Expr.Create<TResult>(context, appHandle);
    }
}
