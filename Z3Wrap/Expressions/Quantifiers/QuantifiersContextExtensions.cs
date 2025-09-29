using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Quantifiers;

/// <summary>
/// Provides universal and existential quantifier methods for Z3Context.
/// </summary>
public static class QuantifiersContextExtensions
{
    /// <summary>
    /// Creates universal quantifier with multiple trigger groups.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="weight">Quantifier weight for solver prioritization.</param>
    /// <param name="boundVars">Variables bound by the quantifier.</param>
    /// <param name="triggerGroups">Groups of trigger patterns for instantiation.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
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
                var pattern = context.Library.Z3MkPattern(context.Handle, (uint)handles.Length, handles);
                return pattern;
            })
            .ToArray();

        var handle = context.Library.Z3MkForallConst(
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

    /// <summary>
    /// Creates universal quantifier with single trigger group.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="weight">Quantifier weight for solver prioritization.</param>
    /// <param name="boundVars">Variables bound by the quantifier.</param>
    /// <param name="triggerGroup">Trigger patterns for instantiation.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
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

    /// <summary>
    /// Creates universal quantifier for single variable.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar">Variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.ForAll(0, [boundVar], Array.Empty<Z3Expr>(), body);
    }

    /// <summary>
    /// Creates universal quantifier for two variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
    public static BoolExpr ForAll(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.ForAll(0, [boundVar1, boundVar2], Array.Empty<Z3Expr>(), body);
    }

    /// <summary>
    /// Creates universal quantifier for three variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="boundVar3">Third variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
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

    /// <summary>
    /// Creates universal quantifier for four variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="boundVar3">Third variable bound by the quantifier.</param>
    /// <param name="boundVar4">Fourth variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing universal quantification.</returns>
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

    /// <summary>
    /// Creates existential quantifier with multiple trigger groups.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="weight">Quantifier weight for solver prioritization.</param>
    /// <param name="boundVars">Variables bound by the quantifier.</param>
    /// <param name="triggerGroups">Groups of trigger patterns for instantiation.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
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
                var pattern = context.Library.Z3MkPattern(context.Handle, (uint)handles.Length, handles);
                return pattern;
            })
            .ToArray();

        var handle = context.Library.Z3MkExistsConst(
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

    /// <summary>
    /// Creates existential quantifier with single trigger group.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="weight">Quantifier weight for solver prioritization.</param>
    /// <param name="boundVars">Variables bound by the quantifier.</param>
    /// <param name="triggerGroup">Trigger patterns for instantiation.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
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

    /// <summary>
    /// Creates existential quantifier for single variable.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar">Variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar, BoolExpr body)
    {
        return context.Exists(0, [boundVar], Array.Empty<Z3Expr>(), body);
    }

    /// <summary>
    /// Creates existential quantifier for two variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
    public static BoolExpr Exists(this Z3Context context, Z3Expr boundVar1, Z3Expr boundVar2, BoolExpr body)
    {
        return context.Exists(0, [boundVar1, boundVar2], Array.Empty<Z3Expr>(), body);
    }

    /// <summary>
    /// Creates existential quantifier for three variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="boundVar3">Third variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
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

    /// <summary>
    /// Creates existential quantifier for four variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">First variable bound by the quantifier.</param>
    /// <param name="boundVar2">Second variable bound by the quantifier.</param>
    /// <param name="boundVar3">Third variable bound by the quantifier.</param>
    /// <param name="boundVar4">Fourth variable bound by the quantifier.</param>
    /// <param name="body">The quantified formula body.</param>
    /// <returns>Boolean expression representing existential quantification.</returns>
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
