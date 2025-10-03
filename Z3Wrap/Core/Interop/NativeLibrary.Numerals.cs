// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Numerals API - P/Invoke bindings for Z3 numeral value extraction
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Numerals API (9 functions):
// - Numeral string conversion (binary, decimal, generic)
// - Integer extraction (32-bit and 64-bit, signed and unsigned)
// - Rational number extraction as numerator/denominator pairs
// - Floating-point approximation extraction
// - Small numeral predicate checking
//
// Note: Numeral creation (Z3_mk_numeral) is in NativeLibrary.Expressions.cs
// Note: Rational decomposition (Z3_get_numerator/denominator) is in NativeLibrary.Queries.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsNumerals(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_binary_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_decimal_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_uint");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_int64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_uint64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_rational_int64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_small");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_double");
    }

    // Delegates

    private delegate IntPtr GetNumeralBinaryStringDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetNumeralDecimalStringDelegate(IntPtr ctx, IntPtr ast, uint precision);
    private delegate bool GetNumeralIntDelegate(IntPtr ctx, IntPtr ast, out int value);
    private delegate bool GetNumeralUintDelegate(IntPtr ctx, IntPtr ast, out uint value);
    private delegate bool GetNumeralInt64Delegate(IntPtr ctx, IntPtr ast, out long value);
    private delegate bool GetNumeralUint64Delegate(IntPtr ctx, IntPtr ast, out ulong value);
    private delegate bool GetNumeralRationalInt64Delegate(IntPtr ctx, IntPtr ast, out long num, out long den);
    private delegate bool GetNumeralSmallDelegate(IntPtr ctx, IntPtr ast, out long num, out long den);
    private delegate bool GetNumeralDoubleDelegate(IntPtr ctx, IntPtr ast, out double value);

    // Methods

    /// <summary>
    /// Gets binary string representation of numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <returns>Binary string (e.g., "1010" for 10).</returns>
    /// <remarks>
    /// Converts numeric value to binary string representation. Works for integers
    /// and bitvectors. Useful for low-level bit manipulation analysis.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetNumeralBinaryString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_binary_string");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralBinaryStringDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets decimal string representation of numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="precision">Number of decimal places for reals.</param>
    /// <returns>Decimal string (e.g., "3.14159").</returns>
    /// <remarks>
    /// Converts numeric value to decimal string. For rationals, returns fraction.
    /// For reals, uses specified precision. For integers, precision is ignored.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetNumeralDecimalString(IntPtr ctx, IntPtr ast, uint precision)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_decimal_string");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralDecimalStringDelegate>(funcPtr);
        return func(ctx, ast, precision);
    }

    /// <summary>
    /// Extracts integer value from numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="value">Output parameter for extracted value.</param>
    /// <returns>True if extraction succeeded, false if out of range.</returns>
    /// <remarks>
    /// Extracts int value from numeral. Returns false if value doesn't fit in
    /// 32-bit signed integer. Use GetNumeralInt64 for larger values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralInt(IntPtr ctx, IntPtr ast, out int value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_int");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralIntDelegate>(funcPtr);
        return func(ctx, ast, out value);
    }

    /// <summary>
    /// Extracts unsigned integer value from numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="value">Output parameter for extracted value.</param>
    /// <returns>True if extraction succeeded, false if out of range.</returns>
    /// <remarks>
    /// Extracts uint value from numeral. Returns false if value doesn't fit in
    /// 32-bit unsigned integer. Use GetNumeralUint64 for larger values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralUint(IntPtr ctx, IntPtr ast, out uint value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_uint");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralUintDelegate>(funcPtr);
        return func(ctx, ast, out value);
    }

    /// <summary>
    /// Extracts 64-bit integer value from numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="value">Output parameter for extracted value.</param>
    /// <returns>True if extraction succeeded, false if out of range.</returns>
    /// <remarks>
    /// Extracts long value from numeral. Returns false if value doesn't fit in
    /// 64-bit signed integer. For arbitrary precision, use string conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralInt64(IntPtr ctx, IntPtr ast, out long value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_int64");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralInt64Delegate>(funcPtr);
        return func(ctx, ast, out value);
    }

    /// <summary>
    /// Extracts 64-bit unsigned integer value from numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="value">Output parameter for extracted value.</param>
    /// <returns>True if extraction succeeded, false if out of range.</returns>
    /// <remarks>
    /// Extracts ulong value from numeral. Returns false if value doesn't fit in
    /// 64-bit unsigned integer. For arbitrary precision, use string conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralUint64(IntPtr ctx, IntPtr ast, out ulong value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_uint64");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralUint64Delegate>(funcPtr);
        return func(ctx, ast, out value);
    }

    /// <summary>
    /// Extracts rational number as 64-bit numerator and denominator.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The rational numeral AST.</param>
    /// <param name="num">Output parameter for numerator.</param>
    /// <param name="den">Output parameter for denominator.</param>
    /// <returns>True if extraction succeeded, false if out of range.</returns>
    /// <remarks>
    /// Extracts rational as num/den where both fit in 64-bit signed integers.
    /// Returns false if numerator or denominator too large. For arbitrary precision
    /// rationals, use string conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralRationalInt64(IntPtr ctx, IntPtr ast, out long num, out long den)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_rational_int64");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralRationalInt64Delegate>(funcPtr);
        return func(ctx, ast, out num, out den);
    }

    /// <summary>
    /// Checks if numeral fits in small integer representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="num">Output parameter for numerator.</param>
    /// <param name="den">Output parameter for denominator.</param>
    /// <returns>True if numeral fits in small representation, false otherwise.</returns>
    /// <remarks>
    /// Tests if numeral can be represented as num/den in 64-bit integers.
    /// Useful for determining whether to use integer extraction or string conversion.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralSmall(IntPtr ctx, IntPtr ast, out long num, out long den)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_small");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralSmallDelegate>(funcPtr);
        return func(ctx, ast, out num, out den);
    }

    /// <summary>
    /// Extracts double-precision floating-point approximation of numeral.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The numeral AST.</param>
    /// <param name="value">Output parameter for extracted value.</param>
    /// <returns>True if extraction succeeded, false if not numeric.</returns>
    /// <remarks>
    /// Converts numeral to double approximation. May lose precision for large
    /// integers or exact rationals. Use for approximate numeric analysis only.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool GetNumeralDouble(IntPtr ctx, IntPtr ast, out double value)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numeral_double");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeralDoubleDelegate>(funcPtr);
        return func(ctx, ast, out value);
    }
}
