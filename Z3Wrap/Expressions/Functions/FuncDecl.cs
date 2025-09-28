using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

/// <summary>
/// Represents function declaration with zero arguments for Z3 solving.
/// </summary>
/// <typeparam name="TResult">The function result type.</typeparam>
public sealed class FuncDecl<TResult> : Z3FuncDecl<TResult>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 0) { }

    /// <summary>
    /// Applies this function with no arguments.
    /// </summary>
    /// <returns>The result of applying the function.</returns>
    public TResult Apply()
    {
        return Context.Apply(this);
    }
}

/// <summary>
/// Represents function declaration with one argument for Z3 solving.
/// </summary>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="TResult">The function result type.</typeparam>
public sealed class FuncDecl<T1, TResult> : Z3FuncDecl<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 1) { }

    /// <summary>
    /// Applies this function with one argument.
    /// </summary>
    /// <param name="arg">The function argument.</param>
    /// <returns>The result of applying the function.</returns>
    public TResult Apply(T1 arg)
    {
        return Context.Apply(this, arg);
    }
}

/// <summary>
/// Represents function declaration with two arguments for Z3 solving.
/// </summary>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="TResult">The function result type.</typeparam>
public sealed class FuncDecl<T1, T2, TResult> : Z3FuncDecl<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 2) { }

    /// <summary>
    /// Applies this function with two arguments.
    /// </summary>
    /// <param name="arg1">The first function argument.</param>
    /// <param name="arg2">The second function argument.</param>
    /// <returns>The result of applying the function.</returns>
    public TResult Apply(T1 arg1, T2 arg2)
    {
        return Context.Apply(this, arg1, arg2);
    }
}

/// <summary>
/// Represents function declaration with three arguments for Z3 solving.
/// </summary>
/// <typeparam name="T1">The first argument type.</typeparam>
/// <typeparam name="T2">The second argument type.</typeparam>
/// <typeparam name="T3">The third argument type.</typeparam>
/// <typeparam name="TResult">The function result type.</typeparam>
public sealed class FuncDecl<T1, T2, T3, TResult> : Z3FuncDecl<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where T3 : Z3Expr, IExprType<T3>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 3) { }

    /// <summary>
    /// Applies this function with three arguments.
    /// </summary>
    /// <param name="arg1">The first function argument.</param>
    /// <param name="arg2">The second function argument.</param>
    /// <param name="arg3">The third function argument.</param>
    /// <returns>The result of applying the function.</returns>
    public TResult Apply(T1 arg1, T2 arg2, T3 arg3)
    {
        return Context.Apply(this, arg1, arg2, arg3);
    }
}
