// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Expressions API - P/Invoke bindings for Z3 expression creation and manipulation
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's basic expression API (30 functions):
// - Sort creation (bool, int, real)
// - Symbol and constant creation
// - Boolean operations (true, false, and, or, not, implies, iff, xor)
// - Equality and distinctness (eq, distinct)
// - Arithmetic operations (add, sub, mul, div, mod, rem, unary minus, power, abs)
// - Numeric comparisons (lt, le, gt, ge)
// - Numeric predicates (is_int, divides)
// - Numeral creation
// - Conditional expressions (if-then-else)
// - Type conversions (int↔real)
//
// Coverage: 30/30 functions (100%) - See COMPARISON_Expressions.md for details
// Note: Z3_mk_int2bv is in NativeLibrary.BitVectors.cs (cross-theory conversion)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsExpressions(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_bool_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_int_sort");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_const");

        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_real_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_eq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_distinct");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_and");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_or");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_not");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_implies");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_iff");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_xor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_add");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_sub");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_mul");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_div");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_mod");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_rem");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_unary_minus");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_power");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_abs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_lt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_le");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_gt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ge");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_numeral");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ite");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int2real");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_real2int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_is_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_divides");
    }

    // Delegates
    private delegate IntPtr MkBoolSortDelegate(IntPtr ctx);
    private delegate IntPtr MkIntSortDelegate(IntPtr ctx);
    private delegate IntPtr MkRealSortDelegate(IntPtr ctx);
    private delegate IntPtr MkConstDelegate(IntPtr ctx, IntPtr symbol, IntPtr sort);
    private delegate IntPtr MkTrueDelegate(IntPtr ctx);
    private delegate IntPtr MkFalseDelegate(IntPtr ctx);
    private delegate IntPtr MkEqDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkDistinctDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkAndDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkOrDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkNotDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr MkImpliesDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkIffDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkXorDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkAddDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkSubDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkMulDelegate(IntPtr ctx, uint numArgs, IntPtr[] args);
    private delegate IntPtr MkDivDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkModDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkRemDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkUnaryMinusDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr MkPowerDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkAbsDelegate(IntPtr ctx, IntPtr arg);
    private delegate IntPtr MkLtDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkLeDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkGtDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkGeDelegate(IntPtr ctx, IntPtr left, IntPtr right);
    private delegate IntPtr MkNumeralDelegate(IntPtr ctx, IntPtr numeral, IntPtr sort);
    private delegate IntPtr MkIteDelegate(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr);
    private delegate IntPtr MkInt2RealDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkReal2IntDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkIsIntDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkDividesDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Methods
    /// <summary>
    /// Creates a Boolean sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created Boolean sort.</returns>
    /// <remarks>
    /// Boolean sorts are used for creating Boolean expressions and constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBoolSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bool_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkBoolSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates an integer sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created integer sort.</returns>
    /// <remarks>
    /// Integer sorts are used for creating integer expressions and arithmetic constraints.
    /// Z3 integers have unlimited precision (BigInteger semantics).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIntSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkIntSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a real number sort (type) for Z3 expressions.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created real sort.</returns>
    /// <remarks>
    /// Real sorts are used for creating real number expressions and arithmetic constraints.
    /// Z3 reals support exact rational arithmetic with unlimited precision.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRealSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkRealSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 constant expression with the specified name and sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The symbol name for the constant.</param>
    /// <param name="sort">The sort (type) of the constant.</param>
    /// <returns>Handle to the created constant expression.</returns>
    /// <remarks>
    /// Constants are free variables that can be assigned values during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkConstDelegate>(funcPtr);
        return func(ctx, symbol, sort);
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
    /// Creates a Z3 addition expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions to add together.</param>
    /// <returns>Handle to the created addition expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// Supports unlimited precision arithmetic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_add");
        var func = Marshal.GetDelegateForFunctionPointer<MkAddDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 subtraction expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions for subtraction (left-associative).</param>
    /// <returns>Handle to the created subtraction expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// With multiple args, performs left-associative subtraction: ((a - b) - c) - d.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sub");
        var func = Marshal.GetDelegateForFunctionPointer<MkSubDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 multiplication expression over multiple numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numArgs">The number of numeric expressions in the array.</param>
    /// <param name="args">Array of numeric expressions to multiply together.</param>
    /// <returns>Handle to the created multiplication expression.</returns>
    /// <remarks>
    /// All arguments must have the same numeric sort (integer or real).
    /// Supports unlimited precision arithmetic.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mul");
        var func = Marshal.GetDelegateForFunctionPointer<MkMulDelegate>(funcPtr);
        return func(ctx, numArgs, args);
    }

    /// <summary>
    /// Creates a Z3 division expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The dividend (numerator) expression.</param>
    /// <param name="right">The divisor (denominator) expression.</param>
    /// <returns>Handle to the created division expression (left / right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort. For integers, performs
    /// real division returning a rational result. Division by zero is undefined.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_div");
        var func = Marshal.GetDelegateForFunctionPointer<MkDivDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 modulo expression between two integer terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The dividend integer expression.</param>
    /// <param name="right">The divisor integer expression.</param>
    /// <returns>Handle to the created modulo expression (left mod right).</returns>
    /// <remarks>
    /// Both expressions must be integers. Returns the remainder of integer division.
    /// The result has the same sign as the divisor in Z3's modulo semantics.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_mod");
        var func = Marshal.GetDelegateForFunctionPointer<MkModDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 remainder expression between two integer terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The dividend integer expression.</param>
    /// <param name="right">The divisor integer expression.</param>
    /// <returns>Handle to the created remainder expression (left rem right).</returns>
    /// <remarks>
    /// Both expressions must be integers. Returns the remainder of integer division.
    /// The result has the same sign as the dividend (different from mod semantics).
    /// Example: -5 rem 3 = -2, whereas -5 mod 3 = 1.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_rem");
        var func = Marshal.GetDelegateForFunctionPointer<MkRemDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 unary minus (negation) expression for a numeric term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The numeric expression to negate.</param>
    /// <returns>Handle to the created unary minus expression (-arg).</returns>
    /// <remarks>
    /// The argument must be a numeric expression (integer or real).
    /// Returns the arithmetic negation of the input.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unary_minus");
        var func = Marshal.GetDelegateForFunctionPointer<MkUnaryMinusDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates a Z3 exponentiation expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The base numeric expression.</param>
    /// <param name="right">The exponent numeric expression.</param>
    /// <returns>Handle to the created power expression (left ^ right).</returns>
    /// <remarks>
    /// Both expressions must be numeric (integer or real). Used for non-linear
    /// arithmetic constraints involving powers and polynomials.
    /// Example: x^2 + y^2 = 25 for circle equations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkPower(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_power");
        var func = Marshal.GetDelegateForFunctionPointer<MkPowerDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 absolute value expression for a numeric term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="arg">The numeric expression to take absolute value of.</param>
    /// <returns>Handle to the created absolute value expression (|arg|).</returns>
    /// <remarks>
    /// The argument must be a numeric expression (integer or real).
    /// Returns the absolute value of the input.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkAbs(IntPtr ctx, IntPtr arg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_abs");
        var func = Marshal.GetDelegateForFunctionPointer<MkAbsDelegate>(funcPtr);
        return func(ctx, arg);
    }

    /// <summary>
    /// Creates a Z3 less-than comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &lt; right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lt");
        var func = Marshal.GetDelegateForFunctionPointer<MkLtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 less-than-or-equal comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &lt;= right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_le");
        var func = Marshal.GetDelegateForFunctionPointer<MkLeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 greater-than comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &gt; right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_gt");
        var func = Marshal.GetDelegateForFunctionPointer<MkGtDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 greater-than-or-equal comparison expression between two numeric terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="left">The left-hand side numeric expression.</param>
    /// <param name="right">The right-hand side numeric expression.</param>
    /// <returns>Handle to the created Boolean expression (left &gt;= right).</returns>
    /// <remarks>
    /// Both expressions must have the same numeric sort (integer or real).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ge");
        var func = Marshal.GetDelegateForFunctionPointer<MkGeDelegate>(funcPtr);
        return func(ctx, left, right);
    }

    /// <summary>
    /// Creates a Z3 numeric literal expression from a string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numeral">Pointer to the null-terminated string representation of the number.</param>
    /// <param name="sort">The numeric sort (integer or real) for the literal.</param>
    /// <returns>Handle to the created numeric literal expression.</returns>
    /// <remarks>
    /// The numeral string format depends on the sort. Integers use decimal notation,
    /// reals can use decimal or fractional notation (e.g., "3.14" or "22/7").
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkNumeral(IntPtr ctx, IntPtr numeral, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_numeral");
        var func = Marshal.GetDelegateForFunctionPointer<MkNumeralDelegate>(funcPtr);
        return func(ctx, numeral, sort);
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

    /// <summary>
    /// Creates a Z3 type conversion expression from integer to real.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The integer expression to convert.</param>
    /// <returns>Handle to the created real expression with the same numeric value.</returns>
    /// <remarks>
    /// Converts an integer expression to its real number equivalent.
    /// The numeric value is preserved in the conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkInt2Real(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2real");
        var func = Marshal.GetDelegateForFunctionPointer<MkInt2RealDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 type conversion expression from real to integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The real expression to convert.</param>
    /// <returns>Handle to the created integer expression truncated towards zero.</returns>
    /// <remarks>
    /// Converts a real expression to integer by truncating towards zero.
    /// For example, 3.7 becomes 3, and -2.9 becomes -2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReal2Int(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real2int");
        var func = Marshal.GetDelegateForFunctionPointer<MkReal2IntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 integer test predicate for a real term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The real expression to test.</param>
    /// <returns>Handle to the created Boolean predicate testing if t1 has an integer value.</returns>
    /// <remarks>
    /// Returns true if the real-valued expression t1 has an integer value.
    /// Used to test whether a real number is actually an integer.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIsInt(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_is_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkIsIntDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 divisibility predicate for two integer terms.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The divisor integer expression.</param>
    /// <param name="t2">The dividend integer expression.</param>
    /// <returns>Handle to the created Boolean predicate testing if t1 divides t2.</returns>
    /// <remarks>
    /// Both expressions must be integers. Returns true if t2 is divisible by t1,
    /// i.e., t2 mod t1 = 0. For linear integer arithmetic, t1 must be a non-zero integer.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkDivides(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_divides");
        var func = Marshal.GetDelegateForFunctionPointer<MkDividesDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }
}
