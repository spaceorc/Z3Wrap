using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

/// <summary>
/// Provides extension methods for Z3Context to work with quantified expressions including universal (ForAll)
/// and existential (Exists) quantifiers. Supports quantification over one to four bound variables.
/// </summary>
public static partial class Z3ContextQuantifiersExtensions
{
    /// <summary>
    /// Creates a universal quantifier (ForAll) expression with one bound variable.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar">The variable to be universally quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the universal quantifier.</returns>
    public static Z3BoolExpr ForAll(this Z3Context context, Z3Expr boundVar, Z3BoolExpr body)
    {
        var bound = new[] { boundVar.Handle };
        var handle = SafeNativeMethods.Z3MkForallConst(
            context.Handle,
            0, // weight
            1, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates an existential quantifier (Exists) expression with one bound variable.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar">The variable to be existentially quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the existential quantifier.</returns>
    public static Z3BoolExpr Exists(this Z3Context context, Z3Expr boundVar, Z3BoolExpr body)
    {
        var bound = new[] { boundVar.Handle };
        var handle = SafeNativeMethods.Z3MkExistsConst(
            context.Handle,
            0, // weight
            1, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a universal quantifier (ForAll) expression with two bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be universally quantified.</param>
    /// <param name="boundVar2">The second variable to be universally quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the universal quantifier.</returns>
    public static Z3BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3BoolExpr body
    )
    {
        var bound = new[] { boundVar1.Handle, boundVar2.Handle };
        var handle = SafeNativeMethods.Z3MkForallConst(
            context.Handle,
            0, // weight
            2, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates an existential quantifier (Exists) expression with two bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be existentially quantified.</param>
    /// <param name="boundVar2">The second variable to be existentially quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the existential quantifier.</returns>
    public static Z3BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3BoolExpr body
    )
    {
        var bound = new[] { boundVar1.Handle, boundVar2.Handle };
        var handle = SafeNativeMethods.Z3MkExistsConst(
            context.Handle,
            0, // weight
            2, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a universal quantifier (ForAll) expression with three bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be universally quantified.</param>
    /// <param name="boundVar2">The second variable to be universally quantified.</param>
    /// <param name="boundVar3">The third variable to be universally quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the universal quantifier.</returns>
    public static Z3BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3BoolExpr body
    )
    {
        var bound = new[] { boundVar1.Handle, boundVar2.Handle, boundVar3.Handle };
        var handle = SafeNativeMethods.Z3MkForallConst(
            context.Handle,
            0, // weight
            3, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates an existential quantifier (Exists) expression with three bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be existentially quantified.</param>
    /// <param name="boundVar2">The second variable to be existentially quantified.</param>
    /// <param name="boundVar3">The third variable to be existentially quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the existential quantifier.</returns>
    public static Z3BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3BoolExpr body
    )
    {
        var bound = new[] { boundVar1.Handle, boundVar2.Handle, boundVar3.Handle };
        var handle = SafeNativeMethods.Z3MkExistsConst(
            context.Handle,
            0, // weight
            3, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a universal quantifier (ForAll) expression with four bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be universally quantified.</param>
    /// <param name="boundVar2">The second variable to be universally quantified.</param>
    /// <param name="boundVar3">The third variable to be universally quantified.</param>
    /// <param name="boundVar4">The fourth variable to be universally quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the universal quantifier.</returns>
    public static Z3BoolExpr ForAll(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3Expr boundVar4,
        Z3BoolExpr body
    )
    {
        var bound = new[]
        {
            boundVar1.Handle,
            boundVar2.Handle,
            boundVar3.Handle,
            boundVar4.Handle,
        };
        var handle = SafeNativeMethods.Z3MkForallConst(
            context.Handle,
            0, // weight
            4, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates an existential quantifier (Exists) expression with four bound variables.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="boundVar1">The first variable to be existentially quantified.</param>
    /// <param name="boundVar2">The second variable to be existentially quantified.</param>
    /// <param name="boundVar3">The third variable to be existentially quantified.</param>
    /// <param name="boundVar4">The fourth variable to be existentially quantified.</param>
    /// <param name="body">The body of the quantifier.</param>
    /// <returns>A new Z3BoolExpr representing the existential quantifier.</returns>
    public static Z3BoolExpr Exists(
        this Z3Context context,
        Z3Expr boundVar1,
        Z3Expr boundVar2,
        Z3Expr boundVar3,
        Z3Expr boundVar4,
        Z3BoolExpr body
    )
    {
        var bound = new[]
        {
            boundVar1.Handle,
            boundVar2.Handle,
            boundVar3.Handle,
            boundVar4.Handle,
        };
        var handle = SafeNativeMethods.Z3MkExistsConst(
            context.Handle,
            0, // weight
            4, // numBound
            bound,
            0, // numPatterns
            [], // patterns
            body.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }
}
