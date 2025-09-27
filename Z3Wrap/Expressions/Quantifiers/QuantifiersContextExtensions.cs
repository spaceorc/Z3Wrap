using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Quantifiers;

public static class QuantifiersContextExtensions
{
    public static BoolExpr ForAll(
        this Z3Context context,
        uint weight,
        IEnumerable<Z3Expr> boundVars,
        IEnumerable<Z3Expr> patterns,
        BoolExpr body
    )
    {
        var bound = boundVars.Select(v => v.Handle).ToArray();
        var patternHandles = patterns.Select(p => p.Handle).ToArray();
        var handle = SafeNativeMethods.Z3MkForallConst(
            context.Handle,
            weight,
            (uint)bound.Length,
            bound,
            (uint)patternHandles.Length,
            patternHandles,
            body.Handle
        );
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.ForAll(0, [boundVar], [], body);
    }

    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.ForAll(0, [boundVar1, boundVar2], [], body);
    }

    public static BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        BoolExpr body
    )
    {
        return context.ForAll(0, [boundVar1, boundVar2, boundVar3], [], body);
    }

    public static BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3Expr boundVar4,
        BoolExpr body
    )
    {
        return context.ForAll(0, [boundVar1, boundVar2, boundVar3, boundVar4], [], body);
    }

    public static BoolExpr Exists(
        this Z3Context context,
        uint weight,
        IEnumerable<Z3Expr> boundVars,
        IEnumerable<Z3Expr> patterns,
        BoolExpr body
    )
    {
        var bound = boundVars.Select(v => v.Handle).ToArray();
        var patternHandles = patterns.Select(p => p.Handle).ToArray();
        var handle = SafeNativeMethods.Z3MkExistsConst(
            context.Handle,
            weight,
            (uint)bound.Length,
            bound,
            (uint)patternHandles.Length,
            patternHandles,
            body.Handle
        );
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.Exists(0, [boundVar], [], body);
    }

    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.Exists(0, [boundVar1, boundVar2], [], body);
    }

    public static BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        BoolExpr body
    )
    {
        return context.Exists(0, [boundVar1, boundVar2, boundVar3], [], body);
    }

    public static BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3Expr boundVar4,
        BoolExpr body
    )
    {
        return context.Exists(0, [boundVar1, boundVar2, boundVar3, boundVar4], [], body);
    }
}
