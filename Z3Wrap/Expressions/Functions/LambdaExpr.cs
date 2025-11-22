using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)

/// <summary>
/// Represents lambda expression with single parameter for Z3 solving.
/// </summary>
/// <typeparam name="T1">The parameter type.</typeparam>
/// <typeparam name="TResult">The result type.</typeparam>
public sealed class LambdaExpr<T1, TResult> : Z3Expr, IExprType<LambdaExpr<T1, TResult>>
    where T1 : Z3Expr, IExprType<T1>
    where TResult : Z3Expr, IExprType<TResult>
{
    private LambdaExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static LambdaExpr<T1, TResult> IExprType<LambdaExpr<T1, TResult>>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<LambdaExpr<T1, TResult>>.Sort(Z3Context context) =>
        context.Library.MkArraySort(context.Handle, T1.Sort(context), TResult.Sort(context));

    /// <summary>
    /// Applies the lambda to an argument.
    /// </summary>
    /// <param name="arg">The argument to apply.</param>
    /// <returns>The result of applying the lambda.</returns>
    public TResult Apply(T1 arg) => Context.Apply(this, arg);

    /// <summary>
    /// Equality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator ==(LambdaExpr<T1, TResult> left, LambdaExpr<T1, TResult> right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator !=(LambdaExpr<T1, TResult> left, LambdaExpr<T1, TResult> right) => left.Neq(right);
}

/// <summary>
/// Represents lambda expression with two parameters for Z3 solving.
/// </summary>
/// <typeparam name="T1">The first parameter type.</typeparam>
/// <typeparam name="T2">The second parameter type.</typeparam>
/// <typeparam name="TResult">The result type.</typeparam>
public sealed class LambdaExpr<T1, T2, TResult> : Z3Expr, IExprType<LambdaExpr<T1, T2, TResult>>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where TResult : Z3Expr, IExprType<TResult>
{
    private LambdaExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static LambdaExpr<T1, T2, TResult> IExprType<LambdaExpr<T1, T2, TResult>>.Create(
        Z3Context context,
        IntPtr handle
    ) => new(context, handle);

    static IntPtr IExprType<LambdaExpr<T1, T2, TResult>>.Sort(Z3Context context)
    {
        var domains = new[] { T1.Sort(context), T2.Sort(context) };
        return context.Library.MkArraySortN(context.Handle, 2, domains, TResult.Sort(context));
    }

    /// <summary>
    /// Applies the lambda to arguments.
    /// </summary>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <returns>The result of applying the lambda.</returns>
    public TResult Apply(T1 arg1, T2 arg2) => Context.Apply(this, arg1, arg2);

    /// <summary>
    /// Equality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator ==(LambdaExpr<T1, T2, TResult> left, LambdaExpr<T1, T2, TResult> right) =>
        left.Eq(right);

    /// <summary>
    /// Inequality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator !=(LambdaExpr<T1, T2, TResult> left, LambdaExpr<T1, T2, TResult> right) =>
        left.Neq(right);
}

/// <summary>
/// Represents lambda expression with three parameters for Z3 solving.
/// </summary>
/// <typeparam name="T1">The first parameter type.</typeparam>
/// <typeparam name="T2">The second parameter type.</typeparam>
/// <typeparam name="T3">The third parameter type.</typeparam>
/// <typeparam name="TResult">The result type.</typeparam>
public sealed class LambdaExpr<T1, T2, T3, TResult> : Z3Expr, IExprType<LambdaExpr<T1, T2, T3, TResult>>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where T3 : Z3Expr, IExprType<T3>
    where TResult : Z3Expr, IExprType<TResult>
{
    private LambdaExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static LambdaExpr<T1, T2, T3, TResult> IExprType<LambdaExpr<T1, T2, T3, TResult>>.Create(
        Z3Context context,
        IntPtr handle
    ) => new(context, handle);

    static IntPtr IExprType<LambdaExpr<T1, T2, T3, TResult>>.Sort(Z3Context context)
    {
        var domains = new[] { T1.Sort(context), T2.Sort(context), T3.Sort(context) };
        return context.Library.MkArraySortN(context.Handle, 3, domains, TResult.Sort(context));
    }

    /// <summary>
    /// Applies the lambda to arguments.
    /// </summary>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <returns>The result of applying the lambda.</returns>
    public TResult Apply(T1 arg1, T2 arg2, T3 arg3) => Context.Apply(this, arg1, arg2, arg3);

    /// <summary>
    /// Equality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator ==(LambdaExpr<T1, T2, T3, TResult> left, LambdaExpr<T1, T2, T3, TResult> right) =>
        left.Eq(right);

    /// <summary>
    /// Inequality comparison of two lambda expressions.
    /// </summary>
    public static BoolExpr operator !=(LambdaExpr<T1, T2, T3, TResult> left, LambdaExpr<T1, T2, T3, TResult> right) =>
        left.Neq(right);
}
