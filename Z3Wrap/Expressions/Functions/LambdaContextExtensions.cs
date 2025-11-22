using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Functions;

/// <summary>
/// Provides lambda expression creation methods for Z3Context.
/// </summary>
public static class LambdaContextExtensions
{
    /// <summary>
    /// Creates lambda expression with single parameter.
    /// </summary>
    /// <typeparam name="T1">Parameter type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="param">The bound parameter.</param>
    /// <param name="body">The lambda body expression.</param>
    /// <returns>Lambda expression.</returns>
    public static LambdaExpr<T1, TResult> Lambda<T1, TResult>(this Z3Context context, T1 param, TResult body)
        where T1 : Z3Expr, IExprType<T1>
        where TResult : Z3Expr, IExprType<TResult>
    {
        // Create the lambda using Z3_mk_lambda_const
        var handle = context.Library.MkLambdaConst(context.Handle, 1, [param.Handle], body.Handle);

        return Z3Expr.Create<LambdaExpr<T1, TResult>>(context, handle);
    }

    /// <summary>
    /// Creates lambda expression with two parameters.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="param1">The first bound parameter.</param>
    /// <param name="param2">The second bound parameter.</param>
    /// <param name="body">The lambda body expression.</param>
    /// <returns>Lambda expression.</returns>
    public static LambdaExpr<T1, T2, TResult> Lambda<T1, T2, TResult>(
        this Z3Context context,
        T1 param1,
        T2 param2,
        TResult body
    )
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where TResult : Z3Expr, IExprType<TResult>
    {
        // Create the lambda using Z3_mk_lambda_const
        var handle = context.Library.MkLambdaConst(context.Handle, 2, [param1.Handle, param2.Handle], body.Handle);

        return Z3Expr.Create<LambdaExpr<T1, T2, TResult>>(context, handle);
    }

    /// <summary>
    /// Creates lambda expression with three parameters.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="T3">Third parameter type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="param1">The first bound parameter.</param>
    /// <param name="param2">The second bound parameter.</param>
    /// <param name="param3">The third bound parameter.</param>
    /// <param name="body">The lambda body expression.</param>
    /// <returns>Lambda expression.</returns>
    public static LambdaExpr<T1, T2, T3, TResult> Lambda<T1, T2, T3, TResult>(
        this Z3Context context,
        T1 param1,
        T2 param2,
        T3 param3,
        TResult body
    )
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where T3 : Z3Expr, IExprType<T3>
        where TResult : Z3Expr, IExprType<TResult>
    {
        // Create the lambda using Z3_mk_lambda_const
        var handle = context.Library.MkLambdaConst(
            context.Handle,
            3,
            [param1.Handle, param2.Handle, param3.Handle],
            body.Handle
        );

        return Z3Expr.Create<LambdaExpr<T1, T2, T3, TResult>>(context, handle);
    }

    /// <summary>
    /// Applies lambda expression to single argument.
    /// </summary>
    /// <typeparam name="T1">Lambda parameter type.</typeparam>
    /// <typeparam name="TResult">Lambda result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="lambda">The lambda expression.</param>
    /// <param name="arg">The argument to apply.</param>
    /// <returns>Result of applying lambda to argument.</returns>
    public static TResult Apply<T1, TResult>(this Z3Context context, LambdaExpr<T1, TResult> lambda, T1 arg)
        where T1 : Z3Expr, IExprType<T1>
        where TResult : Z3Expr, IExprType<TResult>
    {
        var handle = context.Library.MkSelect(context.Handle, lambda.Handle, arg.Handle);
        return Z3Expr.Create<TResult>(context, handle);
    }

    /// <summary>
    /// Applies lambda expression to two arguments.
    /// </summary>
    /// <typeparam name="T1">First lambda parameter type.</typeparam>
    /// <typeparam name="T2">Second lambda parameter type.</typeparam>
    /// <typeparam name="TResult">Lambda result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="lambda">The lambda expression.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <returns>Result of applying lambda to arguments.</returns>
    public static TResult Apply<T1, T2, TResult>(
        this Z3Context context,
        LambdaExpr<T1, T2, TResult> lambda,
        T1 arg1,
        T2 arg2
    )
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where TResult : Z3Expr, IExprType<TResult>
    {
        var handle = context.Library.MkSelectN(context.Handle, lambda.Handle, 2, [arg1.Handle, arg2.Handle]);
        return Z3Expr.Create<TResult>(context, handle);
    }

    /// <summary>
    /// Applies lambda expression to three arguments.
    /// </summary>
    /// <typeparam name="T1">First lambda parameter type.</typeparam>
    /// <typeparam name="T2">Second lambda parameter type.</typeparam>
    /// <typeparam name="T3">Third lambda parameter type.</typeparam>
    /// <typeparam name="TResult">Lambda result type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="lambda">The lambda expression.</param>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <returns>Result of applying lambda to arguments.</returns>
    public static TResult Apply<T1, T2, T3, TResult>(
        this Z3Context context,
        LambdaExpr<T1, T2, T3, TResult> lambda,
        T1 arg1,
        T2 arg2,
        T3 arg3
    )
        where T1 : Z3Expr, IExprType<T1>
        where T2 : Z3Expr, IExprType<T2>
        where T3 : Z3Expr, IExprType<T3>
        where TResult : Z3Expr, IExprType<TResult>
    {
        var handle = context.Library.MkSelectN(
            context.Handle,
            lambda.Handle,
            3,
            [arg1.Handle, arg2.Handle, arg3.Handle]
        );
        return Z3Expr.Create<TResult>(context, handle);
    }
}
