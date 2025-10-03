// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 IntegersAndReals API - P/Invoke bindings for Z3 integers_and_reals functions
//
// Source: z3_api.h (IntegersAndReals section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's IntegersAndReals API (17 functions):
// - Arithmetic operations: Z3_mk_add, Z3_mk_sub, Z3_mk_mul, Z3_mk_div, Z3_mk_mod, Z3_mk_rem
// - Unary operations: Z3_mk_unary_minus, Z3_mk_abs
// - Power operation: Z3_mk_power
// - Comparisons: Z3_mk_lt, Z3_mk_le, Z3_mk_gt, Z3_mk_ge
// - Type conversions: Z3_mk_int2real, Z3_mk_real2int
// - Predicates: Z3_mk_is_int, Z3_mk_divides
//
// Missing Functions (0 functions):
// None - all functions implemented ✓

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    // Delegates
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
    private delegate IntPtr MkInt2RealDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkReal2IntDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkIsIntDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkDividesDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    private static void LoadFunctionsIntegersAndReals(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_add");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_sub");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_mul");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_div");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_mod");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_rem");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_unary_minus");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_power");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_abs");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_lt");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_le");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_gt");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_ge");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_int2real");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_real2int");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_is_int");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_divides");
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
