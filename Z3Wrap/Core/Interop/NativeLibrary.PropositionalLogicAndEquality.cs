// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 PropositionalLogicAndEquality API - P/Invoke bindings for Z3 propositional logic and equality functions
//
// Source: z3_api.h (PropositionalLogicAndEquality section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's PropositionalLogicAndEquality API (11 functions):
// - Boolean constants (true, false)
// - Boolean operations (and, or, not, implies, iff, xor)
// - Equality and distinctness (eq, distinct)
// - Conditional expression (if-then-else)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsPropositionalLogicAndEquality(
        IntPtr handle,
        Dictionary<string, IntPtr> functionPointers
    )
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_true");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_false");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_and");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_or");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_not");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_implies");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_iff");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_xor");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_eq");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_distinct");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_ite");
    }

    // Delegates
    private delegate IntPtr MkTrueDelegate(IntPtr ctx);
    private delegate IntPtr MkFalseDelegate(IntPtr ctx);
    private delegate IntPtr MkAndDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkOrDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkNotDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr MkImpliesDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkIffDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkXorDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkEqDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkDistinctDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkIteDelegate(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr);

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant true.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created true Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTrue(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_true");
        var func = Marshal.GetDelegateForFunctionPointer<MkTrueDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant false.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created false Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFalse(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_false");
        var func = Marshal.GetDelegateForFunctionPointer<MkFalseDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 logical AND expression over multiple Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of Boolean expressions in the array.</param>
    /// <param name="args">Array of Boolean expressions to combine with AND.</param>
    /// <returns>Handle to the created AND expression.</returns>
    /// <remarks>
    /// All arguments must be Boolean expressions. Returns true if all arguments are true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_and");
        var func = Marshal.GetDelegateForFunctionPointer<MkAndDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 logical OR expression over multiple Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of Boolean expressions in the array.</param>
    /// <param name="args">Array of Boolean expressions to combine with OR.</param>
    /// <returns>Handle to the created OR expression.</returns>
    /// <remarks>
    /// All arguments must be Boolean expressions. Returns true if any argument is true.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_or");
        var func = Marshal.GetDelegateForFunctionPointer<MkOrDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 logical NOT expression that negates a Boolean term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The Boolean expression to negate.</param>
    /// <returns>Handle to the created NOT expression.</returns>
    /// <remarks>
    /// The argument must be a Boolean expression. Returns the logical negation of the input.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkNot(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_not");
        var func = Marshal.GetDelegateForFunctionPointer<MkNotDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates a Z3 logical implication expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The antecedent (premise) Boolean expression.</param>
    /// <param name="right">The consequent (conclusion) Boolean expression.</param>
    /// <returns>Handle to the created implication expression (left =&gt; right).</returns>
    /// <remarks>
    /// Logical implication is false only when the antecedent is true and the consequent is false.
    /// Equivalent to (!left || right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkImplies(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_implies");
        var func = Marshal.GetDelegateForFunctionPointer<MkImpliesDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 logical biconditional (if-and-only-if) expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side Boolean expression.</param>
    /// <param name="right">The right-hand side Boolean expression.</param>
    /// <returns>Handle to the created biconditional expression (left &lt;=&gt; right).</returns>
    /// <remarks>
    /// Biconditional is true when both sides have the same truth value.
    /// Equivalent to (left &amp;&amp; right) || (!left &amp;&amp; !right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIff(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_iff");
        var func = Marshal.GetDelegateForFunctionPointer<MkIffDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 logical exclusive-or (XOR) expression between two Boolean terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side Boolean expression.</param>
    /// <param name="right">The right-hand side Boolean expression.</param>
    /// <returns>Handle to the created XOR expression (left XOR right).</returns>
    /// <remarks>
    /// XOR is true when exactly one of the two operands is true.
    /// Equivalent to (left &amp;&amp; !right) || (!left &amp;&amp; right).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_xor");
        var func = Marshal.GetDelegateForFunctionPointer<MkXorDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 equality expression between two terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side expression.</param>
    /// <param name="right">The right-hand side expression.</param>
    /// <returns>Handle to the created equality expression (left == right).</returns>
    /// <remarks>
    /// Both expressions must have the same sort for the equality to be valid.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_eq");
        var func = Marshal.GetDelegateForFunctionPointer<MkEqDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 constraint that all terms are pairwise distinct.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of expressions in the array.</param>
    /// <param name="args">Array of expressions that must be pairwise different.</param>
    /// <returns>Handle to the created distinct constraint.</returns>
    /// <remarks>
    /// All arguments must have the same sort. The constraint is true when no two
    /// arguments have the same value (all pairwise inequalities hold).
    /// Commonly used in combinatorial problems and constraint modeling.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDistinct(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_distinct");
        var func = Marshal.GetDelegateForFunctionPointer<MkDistinctDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 if-then-else (conditional) expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="condition">The Boolean condition expression.</param>
    /// <param name="thenExpr">The expression to return when condition is true.</param>
    /// <param name="elseExpr">The expression to return when condition is false.</param>
    /// <returns>Handle to the created conditional expression (condition ? thenExpr : elseExpr).</returns>
    /// <remarks>
    /// The condition must be Boolean, and both branches must have the same sort.
    /// Used for conditional logic and piecewise function definitions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ite");
        var func = Marshal.GetDelegateForFunctionPointer<MkIteDelegate>(funcPtr);
        return func(ctx, condition, thenExpr, elseExpr);
    }
}
