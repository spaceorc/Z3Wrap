using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Extensions;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 constant (0-arity function) declaration.
/// </summary>
/// <typeparam name="TResult">The result type of the constant.</typeparam>
public sealed class Z3FuncDecl<TResult> : Z3FuncDeclBase<TResult>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 0) { }

    /// <summary>
    /// Applies this constant function (creates an expression).
    /// </summary>
    /// <returns>A Z3 expression of type TResult.</returns>
    public TResult Apply()
    {
        return Context.Apply(this);
    }
}

/// <summary>
/// Represents a Z3 unary function declaration.
/// </summary>
/// <typeparam name="T1">The type of the function parameter.</typeparam>
/// <typeparam name="TResult">The result type of the function.</typeparam>
public sealed class Z3FuncDecl<T1, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 1) { }

    /// <summary>
    /// Applies this unary function to the given argument.
    /// </summary>
    /// <param name="arg">The argument to the function.</param>
    /// <returns>A Z3 expression of type TResult.</returns>
    public TResult Apply(T1 arg)
    {
        return Context.Apply(this, arg);
    }
}

/// <summary>
/// Represents a Z3 binary function declaration.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="TResult">The result type of the function.</typeparam>
public sealed class Z3FuncDecl<T1, T2, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 2) { }

    /// <summary>
    /// Applies this binary function to the given arguments.
    /// </summary>
    /// <param name="arg1">The first argument to the function.</param>
    /// <param name="arg2">The second argument to the function.</param>
    /// <returns>A Z3 expression of type TResult.</returns>
    public TResult Apply(T1 arg1, T2 arg2)
    {
        return Context.Apply(this, arg1, arg2);
    }
}

/// <summary>
/// Represents a Z3 ternary function declaration.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
/// <typeparam name="TResult">The result type of the function.</typeparam>
public sealed class Z3FuncDecl<T1, T2, T3, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where T3 : Z3Expr, IExprType<T3>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal Z3FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 3) { }

    /// <summary>
    /// Applies this ternary function to the given arguments.
    /// </summary>
    /// <param name="arg1">The first argument to the function.</param>
    /// <param name="arg2">The second argument to the function.</param>
    /// <param name="arg3">The third argument to the function.</param>
    /// <returns>A Z3 expression of type TResult.</returns>
    public TResult Apply(T1 arg1, T2 arg2, T3 arg3)
    {
        return Context.Apply(this, arg1, arg2, arg3);
    }
}
