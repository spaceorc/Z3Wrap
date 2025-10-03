// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Numerals API - P/Invoke bindings for Z3 numeral creation and extraction
//
// Source: z3_api.h (Numerals section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Numerals API (16 functions):
// - Numeral creation from native types (int32, uint32, int64, uint64, real fractions)
// - Polymorphic string-based creation (Z3_mk_numeral)
// - Numeral string conversion (binary, decimal, generic)
// - Integer extraction (32-bit and 64-bit, signed and unsigned)
// - Rational number extraction as numerator/denominator pairs
// - Floating-point approximation extraction
// - Small numeral predicate checking
//
// Note: Rational decomposition (Z3_get_numerator/denominator) is in NativeLibrary.Accessors.cs
// Note: Z3_mk_bv_numeral is in NativeLibrary.BitVectors.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsNumerals(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Creation functions
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_numeral");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_unsigned_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_unsigned_int64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_real");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_real_int64");

        // Extraction and conversion functions
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

    // Creation delegates
    private delegate IntPtr MkNumeralDelegate(IntPtr ctx, IntPtr numeral, IntPtr sort);
    private delegate IntPtr MkIntDelegate(IntPtr ctx, int v, IntPtr sort);
    private delegate IntPtr MkUnsignedIntDelegate(IntPtr ctx, uint v, IntPtr sort);
    private delegate IntPtr MkInt64Delegate(IntPtr ctx, long v, IntPtr sort);
    private delegate IntPtr MkUnsignedInt64Delegate(IntPtr ctx, ulong v, IntPtr sort);
    private delegate IntPtr MkRealDelegate(IntPtr ctx, int num, int den);
    private delegate IntPtr MkRealInt64Delegate(IntPtr ctx, long num, long den);

    // Extraction and conversion delegates
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

    // Creation methods

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
    /// Creates numeral from 32-bit signed integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="v">Integer value.</param>
    /// <param name="sort">Target sort (integer, real, or bitvector).</param>
    /// <returns>AST node representing numeral with given value.</returns>
    /// <remarks>
    /// Direct creation from native int32 value. More efficient than string-based creation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkInt(IntPtr ctx, int v, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkIntDelegate>(funcPtr);
        return func(ctx, v, sort);
    }

    /// <summary>
    /// Creates numeral from 32-bit unsigned integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="v">Unsigned integer value.</param>
    /// <param name="sort">Target sort (integer, real, or bitvector).</param>
    /// <returns>AST node representing numeral with given value.</returns>
    /// <remarks>
    /// Direct creation from native uint32 value. More efficient than string-based creation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkUnsignedInt(IntPtr ctx, uint v, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unsigned_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkUnsignedIntDelegate>(funcPtr);
        return func(ctx, v, sort);
    }

    /// <summary>
    /// Creates numeral from 64-bit signed integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="v">Long integer value.</param>
    /// <param name="sort">Target sort (integer, real, or bitvector).</param>
    /// <returns>AST node representing numeral with given value.</returns>
    /// <remarks>
    /// Direct creation from native int64 value. More efficient than string-based creation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkInt64(IntPtr ctx, long v, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int64");
        var func = Marshal.GetDelegateForFunctionPointer<MkInt64Delegate>(funcPtr);
        return func(ctx, v, sort);
    }

    /// <summary>
    /// Creates numeral from 64-bit unsigned integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="v">Unsigned long integer value.</param>
    /// <param name="sort">Target sort (integer, real, or bitvector).</param>
    /// <returns>AST node representing numeral with given value.</returns>
    /// <remarks>
    /// Direct creation from native uint64 value. More efficient than string-based creation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkUnsignedInt64(IntPtr ctx, ulong v, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_unsigned_int64");
        var func = Marshal.GetDelegateForFunctionPointer<MkUnsignedInt64Delegate>(funcPtr);
        return func(ctx, v, sort);
    }

    /// <summary>
    /// Creates real numeral from 32-bit integer fraction.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="num">Numerator.</param>
    /// <param name="den">Denominator (must be non-zero).</param>
    /// <returns>AST node representing rational number num/den.</returns>
    /// <remarks>
    /// Creates exact rational representation. Result is automatically simplified (e.g., 2/4 becomes 1/2).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReal(IntPtr ctx, int num, int den)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real");
        var func = Marshal.GetDelegateForFunctionPointer<MkRealDelegate>(funcPtr);
        return func(ctx, num, den);
    }

    /// <summary>
    /// Creates real numeral from 64-bit integer fraction.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="num">Numerator.</param>
    /// <param name="den">Denominator (must be non-zero).</param>
    /// <returns>AST node representing rational number num/den.</returns>
    /// <remarks>
    /// Creates exact rational representation with 64-bit precision. Result is automatically simplified.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRealInt64(IntPtr ctx, long num, long den)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_real_int64");
        var func = Marshal.GetDelegateForFunctionPointer<MkRealInt64Delegate>(funcPtr);
        return func(ctx, num, den);
    }

    // Extraction and conversion methods

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
