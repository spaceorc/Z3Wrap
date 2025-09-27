using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Quantifiers;

public static class QuantifiersContextExtensions
{
    public static BoolExpr ForAll(
        this Z3Context context,
        uint weight,
        IReadOnlyList<Z3Expr> boundVars,
        IReadOnlyList<IReadOnlyList<Z3Expr>> triggerGroups,
        BoolExpr body
    )
    {
        var bound = boundVars.Select(v => v.Handle).ToArray();

        var patternHandles = triggerGroups
            .Where(g => g.Count > 0)
            .Select(g =>
            {
                var handles = g.Select(p => p.Handle).ToArray();
                var pattern = SafeNativeMethods.Z3MkPattern(context.Handle, (uint)handles.Length, handles);
                return pattern;
            })
            .ToArray();

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

    public static BoolExpr ForAll(
        this Z3Context context,
        uint weight,
        IReadOnlyList<Z3Expr> boundVars,
        IReadOnlyList<Z3Expr> triggerGroup,
        BoolExpr body
    )
    {
        return context.ForAll(weight, boundVars, [triggerGroup], body);
    }

    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.ForAll(0, [boundVar], Array.Empty<Z3Expr>(), body);
    }

    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.ForAll(0, [boundVar1, boundVar2], Array.Empty<Z3Expr>(), body);
    }

    public static BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        BoolExpr body
    )
    {
        return context.ForAll(0, [boundVar1, boundVar2, boundVar3], Array.Empty<Z3Expr>(), body);
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
        return context.ForAll(0, [boundVar1, boundVar2, boundVar3, boundVar4], Array.Empty<Z3Expr>(), body);
    }

    public static BoolExpr Exists(
        this Z3Context context,
        uint weight,
        IReadOnlyList<Z3Expr> boundVars,
        IReadOnlyList<IReadOnlyList<Z3Expr>> triggerGroups,
        BoolExpr body
    )
    {
        var bound = boundVars.Select(v => v.Handle).ToArray();

        var patternHandles = triggerGroups
            .Where(g => g.Count > 0)
            .Select(g =>
            {
                var handles = g.Select(p => p.Handle).ToArray();
                var pattern = SafeNativeMethods.Z3MkPattern(context.Handle, (uint)handles.Length, handles);
                return pattern;
            })
            .ToArray();

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

    public static BoolExpr Exists(
        this Z3Context context,
        uint weight,
        IReadOnlyList<Z3Expr> boundVars,
        IReadOnlyList<Z3Expr> triggerGroup,
        BoolExpr body
    )
    {
        return context.Exists(weight, boundVars, [triggerGroup], body);
    }

    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.Exists(0, [boundVar], Array.Empty<Z3Expr>(), body);
    }

    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.Exists(0, [boundVar1, boundVar2], Array.Empty<Z3Expr>(), body);
    }

    public static BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        BoolExpr body
    )
    {
        return context.Exists(0, [boundVar1, boundVar2, boundVar3], Array.Empty<Z3Expr>(), body);
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
        return context.Exists(0, [boundVar1, boundVar2, boundVar3, boundVar4], Array.Empty<Z3Expr>(), body);
    }
}
