using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

/// <summary>
/// Provides extension methods for Z3Context to work with uninterpreted functions and function applications.
/// Supports creating function declarations with various arities and applying them to create expressions.
/// </summary>
public static class Z3ContextFunctionExtensions
{
    /// <summary>
    /// Creates a function declaration for a constant (0-arity function).
    /// </summary>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the function.</param>
    /// <returns>A new Z3FuncDecl representing the constant function.</returns>
    public static Z3FuncDecl<TResult> Func<TResult>(this Z3Context context, string name)
        where TResult : Z3Expr
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = SafeNativeMethods.Z3MkFuncDecl(
            context.Handle,
            symbol,
            0, // domain size (0 for constants)
            [], // empty domain array
            rangeSort
        );

        return new Z3FuncDecl<TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates a function declaration for a unary function.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the function.</param>
    /// <returns>A new Z3FuncDecl representing the unary function.</returns>
    public static Z3FuncDecl<T1, TResult> Func<T1, TResult>(this Z3Context context, string name)
        where T1 : Z3Expr
        where TResult : Z3Expr
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[] { context.GetSortForType<T1>() };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = SafeNativeMethods.Z3MkFuncDecl(
            context.Handle,
            symbol,
            1, // domain size
            domainSorts,
            rangeSort
        );

        return new Z3FuncDecl<T1, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates a function declaration for a binary function.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the function.</param>
    /// <returns>A new Z3FuncDecl representing the binary function.</returns>
    public static Z3FuncDecl<T1, T2, TResult> Func<T1, T2, TResult>(
        this Z3Context context,
        string name
    )
        where T1 : Z3Expr
        where T2 : Z3Expr
        where TResult : Z3Expr
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[] { context.GetSortForType<T1>(), context.GetSortForType<T2>() };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = SafeNativeMethods.Z3MkFuncDecl(
            context.Handle,
            symbol,
            2, // domain size
            domainSorts,
            rangeSort
        );

        return new Z3FuncDecl<T1, T2, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates a function declaration for a ternary function.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the function.</param>
    /// <returns>A new Z3FuncDecl representing the ternary function.</returns>
    public static Z3FuncDecl<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(
        this Z3Context context,
        string name
    )
        where T1 : Z3Expr
        where T2 : Z3Expr
        where T3 : Z3Expr
        where TResult : Z3Expr
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var domainSorts = new[]
        {
            context.GetSortForType<T1>(),
            context.GetSortForType<T2>(),
            context.GetSortForType<T3>(),
        };
        var rangeSort = context.GetSortForType<TResult>();

        var funcDeclHandle = SafeNativeMethods.Z3MkFuncDecl(
            context.Handle,
            symbol,
            3, // domain size
            domainSorts,
            rangeSort
        );

        return new Z3FuncDecl<T1, T2, T3, TResult>(context, funcDeclHandle, name);
    }

    /// <summary>
    /// Creates a function application from a function declaration with variable arguments.
    /// </summary>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="funcDecl">The function declaration.</param>
    /// <param name="args">The arguments to the function.</param>
    /// <returns>A Z3 expression of type TResult representing the function application.</returns>
    public static TResult Apply<TResult>(
        this Z3Context context,
        Z3FuncDeclBase<TResult> funcDecl,
        params Z3Expr[] args
    )
        where TResult : Z3Expr
    {
        if (args.Length != funcDecl.Arity)
            throw new ArgumentException(
                $"Function has arity {funcDecl.Arity}, but {args.Length} arguments provided",
                nameof(args)
            );

        var argHandles = args.Select(arg => arg.Handle).ToArray();
        var appHandle = SafeNativeMethods.Z3MkApp(
            context.Handle,
            funcDecl.Handle,
            (uint)args.Length,
            argHandles
        );

        return (TResult)Z3Expr.Create(context, appHandle);
    }
}
