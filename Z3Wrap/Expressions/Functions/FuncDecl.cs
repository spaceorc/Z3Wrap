using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Extensions;

namespace Spaceorc.Z3Wrap.Core;

public sealed class FuncDecl<TResult> : Z3FuncDeclBase<TResult>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 0) { }

    public TResult Apply()
    {
        return Context.Apply(this);
    }
}

public sealed class FuncDecl<T1, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 1) { }

    public TResult Apply(T1 arg)
    {
        return Context.Apply(this, arg);
    }
}

public sealed class FuncDecl<T1, T2, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 2) { }

    public TResult Apply(T1 arg1, T2 arg2)
    {
        return Context.Apply(this, arg1, arg2);
    }
}

public sealed class FuncDecl<T1, T2, T3, TResult> : Z3FuncDeclBase<TResult>
    where T1 : Z3Expr, IExprType<T1>
    where T2 : Z3Expr, IExprType<T2>
    where T3 : Z3Expr, IExprType<T3>
    where TResult : Z3Expr, IExprType<TResult>
{
    internal FuncDecl(Z3Context context, IntPtr handle, string name)
        : base(context, handle, name, 3) { }

    public TResult Apply(T1 arg1, T2 arg2, T3 arg3)
    {
        return Context.Apply(this, arg1, arg2, arg3);
    }
}
