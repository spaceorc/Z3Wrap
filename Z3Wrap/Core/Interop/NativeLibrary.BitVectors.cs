// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 BitVector API - P/Invoke bindings for fixed-width binary arithmetic
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's bitvector theory API (53 functions):
// - Sort creation (2 functions): Bitvector sorts and size queries
// - Numeral creation (2 functions): Create bitvector constants from strings or bit arrays
// - Bit manipulation (6 functions): Concat, extract, sign/zero extend, repeat, bit2bool
// - Arithmetic operations (9 functions): Add, sub, mul, div (signed/unsigned), rem, mod, neg
// - Bitwise operations (7 functions): AND, OR, XOR, NOT, NAND, NOR, XNOR
// - Bitwise reduction (2 functions): Reduction AND/OR
// - Shift operations (3 functions): Left shift, logical/arithmetic right shift
// - Rotate operations (4 functions): Const and variable rotate left/right
// - Comparison operations (8 functions): Signed and unsigned comparisons
// - Overflow detection (8 functions): Detect arithmetic overflow/underflow
// - Conversion functions (2 functions): Between bitvectors and integers
//
// Complete coverage of Z3's bitvector reasoning API for fixed-width machine arithmetic.
// See COMPARISON_BitVectors.md for detailed function mapping documentation.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsBitVectors(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Sort creation and queries
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_bv_sort_size");

        // Numeral creation
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bv_numeral");

        // Bit manipulation
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_concat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_extract");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_sign_ext");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_zero_ext");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_repeat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bit2bool");

        // Arithmetic operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvadd");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsub");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvmul");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvudiv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsdiv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvurem");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsrem");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsmod");

        // Bitwise operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvand");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvxor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvnot");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvneg");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvnand");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvnor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvxnor");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvredand");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvredor");

        // Shift operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvshl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvlshr");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvashr");

        // Rotate operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_rotate_left");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_rotate_right");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ext_rotate_left");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ext_rotate_right");

        // Comparison operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvult");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvslt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvule");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsle");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvugt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsgt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvuge");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsge");

        // Overflow detection
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvadd_no_overflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvadd_no_underflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsub_no_overflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsub_no_underflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvmul_no_overflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvmul_no_underflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvsdiv_no_overflow");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bvneg_no_overflow");

        // Conversion functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_bv2int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int2bv");
    }

    // Delegates

    // Sort and numeral creation delegates
    private delegate uint GetBvSortSizeDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr MkBvDelegate(
        IntPtr ctx,
        int numSize,
        [MarshalAs(UnmanagedType.LPStr)] string numString,
        int base_
    );
    private delegate IntPtr MkBvNumeralDelegate(
        IntPtr ctx,
        uint sz,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] bool[] bits
    );

    // Bit manipulation delegates
    private delegate IntPtr MkConcatDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkExtractDelegate(IntPtr ctx, uint high, uint low, IntPtr t1);
    private delegate IntPtr MkSignExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr MkZeroExtDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr MkRepeatDelegate(IntPtr ctx, uint i, IntPtr t1);
    private delegate IntPtr MkBit2BoolDelegate(IntPtr ctx, uint i, IntPtr t1);

    // Arithmetic operation delegates
    private delegate IntPtr MkBvAddDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSubDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvMulDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvUDivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSDivDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvURemDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSRemDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSModDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Bitwise operation delegates
    private delegate IntPtr MkBvAndDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvOrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvXorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvNotDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkBvNegDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkBvNandDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvNorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvXnorDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvRedAndDelegate(IntPtr ctx, IntPtr t1);
    private delegate IntPtr MkBvRedOrDelegate(IntPtr ctx, IntPtr t1);

    // Shift and rotate operation delegates
    private delegate IntPtr MkBvShlDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvLShrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvAShrDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkRotateLeftDelegate(IntPtr ctx, uint i, IntPtr t);
    private delegate IntPtr MkRotateRightDelegate(IntPtr ctx, uint i, IntPtr t);
    private delegate IntPtr MkExtRotateLeftDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkExtRotateRightDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Comparison operation delegates
    private delegate IntPtr MkBvULtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSLtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvULeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSLeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvUGtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSGtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvUGeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSGeDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Overflow detection delegates
    private delegate IntPtr MkBvAddNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
    private delegate IntPtr MkBvAddNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSubNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvSubNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
    private delegate IntPtr MkBvMulNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed);
    private delegate IntPtr MkBvMulNoUnderflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvDivNoOverflowDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkBvNegNoOverflowDelegate(IntPtr ctx, IntPtr t1);

    // Conversion delegates
    private delegate IntPtr MkBv2IntDelegate(IntPtr ctx, IntPtr t1, bool signed);
    private delegate IntPtr MkInt2BvDelegate(IntPtr ctx, uint n, IntPtr t1);

    // Methods
    /// <summary>
    /// Retrieves the bit width of a bitvector sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The bitvector sort to query.</param>
    /// <returns>The bit width of the bitvector sort.</returns>
    /// <remarks>
    /// Used to determine the size of bitvector expressions for type checking and operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_bv_sort_size");
        var func = Marshal.GetDelegateForFunctionPointer<GetBvSortSizeDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Creates bitvector numeral from string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="numSize">Size of the bitvector in bits.</param>
    /// <param name="numString">String representation of the number.</param>
    /// <param name="base_">Numeric base (2, 10, or 16).</param>
    /// <returns>Bitvector numeral AST node.</returns>
    internal IntPtr MkBv(IntPtr ctx, int numSize, string numString, int base_)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvDelegate>(funcPtr);
        return func(ctx, numSize, numString, base_);
    }

    /// <summary>
    /// Creates bitvector numeral from bit array.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sz">Number of bits in the bitvector.</param>
    /// <param name="bits">Array of Boolean values representing bits (LSB first).</param>
    /// <returns>AST node representing bitvector constant with specified bit pattern.</returns>
    /// <remarks>
    /// Creates bitvector constant from Boolean array where bits[0] is LSB.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNumeral(IntPtr ctx, uint sz, bool[] bits)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv_numeral");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNumeralDelegate>(funcPtr);
        return func(ctx, sz, bits);
    }

    /// <summary>
    /// Creates concatenation of two bitvectors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First bitvector (higher-order bits).</param>
    /// <param name="t2">Second bitvector (lower-order bits).</param>
    /// <returns>Bitvector concatenation expression.</returns>
    internal IntPtr MkConcat(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_concat");
        var func = Marshal.GetDelegateForFunctionPointer<MkConcatDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector addition expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitvector addition expression (t1 + t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Addition uses modular arithmetic
    /// with overflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvAdd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvAddDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector subtraction expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The minuend bitvector operand.</param>
    /// <param name="t2">The subtrahend bitvector operand.</param>
    /// <returns>Handle to the created bitvector subtraction expression (t1 - t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Subtraction uses modular arithmetic
    /// with underflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSub(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSubDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector multiplication expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitvector multiplication expression (t1 * t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Multiplication uses modular arithmetic
    /// with overflow wrapping around.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvMul(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvMulDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned division expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created unsigned division expression (t1 /u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// Division by zero returns all 1s (maximum unsigned value).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvUDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvudiv");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvUDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed division expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed division expression (t1 /s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation. Division by zero has undefined behavior.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSDiv(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSDivDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned remainder expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created unsigned remainder expression (t1 %u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// Remainder by zero returns the dividend unchanged.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvURem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvurem");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvURemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed remainder expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed remainder expression (t1 %s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers.
    /// The result has the same sign as the dividend.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSRem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsrem");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSRemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed modulo expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to the created signed modulo expression (t1 mod t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. The result has the same sign as the divisor.
    /// Different from signed remainder in how negative numbers are handled.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSMod(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsmod");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSModDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise AND expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise AND expression (t1 &amp; t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise AND operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvAnd(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvand");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvAndDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise OR expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise OR expression (t1 | t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise OR operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvOr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvor");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvOrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise XOR expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to the created bitwise XOR expression (t1 ^ t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Performs bitwise XOR operation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvxor");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvXorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector bitwise NOT expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to the created bitwise NOT expression (~t1).</returns>
    /// <remarks>
    /// Performs bitwise complement, flipping all bits in the bitvector.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNot(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnot");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNotDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector arithmetic negation expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to the created arithmetic negation expression (-t1).</returns>
    /// <remarks>
    /// Performs two's complement negation, equivalent to (~t1 + 1).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNeg(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNegDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector left shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift left.</param>
    /// <returns>Handle to the created left shift expression (t1 &lt;&lt; t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Fills with zeros from the right.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvShl(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvshl");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvShlDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector sign extension expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of bits to extend.</param>
    /// <param name="t1">The bitvector operand to extend.</param>
    /// <returns>Handle to the created sign-extended bitvector expression.</returns>
    /// <remarks>
    /// Extends the bitvector by replicating the sign bit (most significant bit) i times.
    /// The resulting bitvector has width = original_width + i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSignExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sign_ext");
        var func = Marshal.GetDelegateForFunctionPointer<MkSignExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector zero extension expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of bits to extend.</param>
    /// <param name="t1">The bitvector operand to extend.</param>
    /// <returns>Handle to the created zero-extended bitvector expression.</returns>
    /// <remarks>
    /// Extends the bitvector by adding i zero bits at the most significant positions.
    /// The resulting bitvector has width = original_width + i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkZeroExt(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_zero_ext");
        var func = Marshal.GetDelegateForFunctionPointer<MkZeroExtDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector bit extraction expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="high">The highest bit index to extract (inclusive).</param>
    /// <param name="low">The lowest bit index to extract (inclusive).</param>
    /// <param name="t1">The bitvector operand to extract bits from.</param>
    /// <returns>Handle to the created bit extraction expression (t1[high:low]).</returns>
    /// <remarks>
    /// Extracts bits from position low to high (inclusive). The resulting bitvector
    /// has width = high - low + 1. Bit indexing starts from 0.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkExtract(IntPtr ctx, uint high, uint low, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_extract");
        var func = Marshal.GetDelegateForFunctionPointer<MkExtractDelegate>(funcPtr);
        return func(ctx, high, low, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector repeat expression that concatenates a bitvector with itself.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The number of times to repeat the bitvector.</param>
    /// <param name="t1">The bitvector operand to repeat.</param>
    /// <returns>Handle to the created repeat expression.</returns>
    /// <remarks>
    /// The resulting bitvector has width = original_width * i.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRepeat(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_repeat");
        var func = Marshal.GetDelegateForFunctionPointer<MkRepeatDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Boolean expression from a specific bit of a bitvector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">The bit index to extract (0 is LSB).</param>
    /// <param name="t1">The bitvector operand.</param>
    /// <returns>Handle to a Boolean expression representing the bit value.</returns>
    /// <remarks>
    /// Extracts a single bit and converts it to a Boolean value (true if 1, false if 0).
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBit2Bool(IntPtr ctx, uint i, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bit2bool");
        var func = Marshal.GetDelegateForFunctionPointer<MkBit2BoolDelegate>(funcPtr);
        return func(ctx, i, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector to integer conversion expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to convert.</param>
    /// <param name="signed">True for signed interpretation, false for unsigned.</param>
    /// <returns>Handle to the created integer conversion expression.</returns>
    /// <remarks>
    /// Converts a bitvector to its integer representation. When signed is true,
    /// uses two's complement interpretation for negative values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBv2Int(IntPtr ctx, IntPtr t1, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bv2int");
        var func = Marshal.GetDelegateForFunctionPointer<MkBv2IntDelegate>(funcPtr);
        return func(ctx, t1, signed);
    }

    /// <summary>
    /// Creates a Z3 integer to bitvector conversion expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">The bit width of the resulting bitvector.</param>
    /// <param name="t1">The integer operand to convert.</param>
    /// <returns>Handle to the created bitvector conversion expression.</returns>
    /// <remarks>
    /// Converts an integer to a bitvector of width n. The integer value is taken modulo 2^n.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkInt2Bv(IntPtr ctx, uint n, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int2bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkInt2BvDelegate>(funcPtr);
        return func(ctx, n, t1);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector addition does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <param name="signed">If true, checks signed overflow; if false, checks unsigned overflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 + t2 does not overflow.</returns>
    /// <remarks>
    /// For signed arithmetic, overflow occurs when the result exceeds the maximum or minimum representable value.
    /// For unsigned arithmetic, overflow occurs when the result cannot fit in the bitvector width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvAddNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvAddNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector addition does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 + t2 does not underflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed addition underflow occurs when the result is less than the minimum representable signed value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvAddNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvadd_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvAddNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector subtraction does not overflow in signed arithmetic.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand (minuend).</param>
    /// <param name="t2">The second bitvector operand (subtrahend).</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 - t2 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// This predicate is useful for verification of arithmetic properties in signed bitvector operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSubNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSubNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector subtraction does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand (minuend).</param>
    /// <param name="t2">The second bitvector operand (subtrahend).</param>
    /// <param name="signed">If true, checks signed underflow; if false, checks unsigned underflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 - t2 does not underflow.</returns>
    /// <remarks>
    /// For signed arithmetic, underflow occurs when the result is less than the minimum representable value.
    /// For unsigned arithmetic, underflow occurs when t1 &lt; t2.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSubNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsub_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSubNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if bitvector multiplication does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <param name="signed">If true, checks signed overflow; if false, checks unsigned overflow.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 * t2 does not overflow.</returns>
    /// <remarks>
    /// For signed arithmetic, overflow occurs when the result exceeds the representable range.
    /// For unsigned arithmetic, overflow occurs when the result cannot fit in the bitvector width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvMulNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2, bool signed)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvMulNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2, signed);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector multiplication does not underflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The first bitvector operand.</param>
    /// <param name="t2">The second bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 * t2 does not underflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed multiplication underflow occurs when the result is less than the minimum representable signed value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvMulNoUnderflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvmul_no_underflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvMulNoUnderflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector division does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The dividend bitvector operand.</param>
    /// <param name="t2">The divisor bitvector operand.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if t1 / t2 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed division overflow occurs when dividing the minimum signed value by -1,
    /// which would result in a value that cannot be represented in the same bit width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSDivNoOverflow(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsdiv_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvDivNoOverflowDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Boolean expression checking if signed bitvector negation does not overflow.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector operand to negate.</param>
    /// <returns>Handle to a Boolean Z3 expression that is true if -t1 does not overflow in signed arithmetic.</returns>
    /// <remarks>
    /// Signed negation overflow occurs when negating the minimum signed value,
    /// as the positive equivalent cannot be represented in the same bit width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNegNoOverflow(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvneg_no_overflow");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNegNoOverflowDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates a Z3 bitvector logical right shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift right.</param>
    /// <returns>Handle to the created logical right shift expression (t1 &gt;&gt;u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Fills with zeros from the left.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvLShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvlshr");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvLShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector arithmetic right shift expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The bitvector to shift.</param>
    /// <param name="t2">The number of positions to shift right.</param>
    /// <returns>Handle to the created arithmetic right shift expression (t1 &gt;&gt;s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Preserves the sign bit when shifting.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvAShr(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvashr");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvAShrDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector rotate left by constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">Number of positions to rotate.</param>
    /// <param name="t">Bitvector to rotate.</param>
    /// <returns>Rotated bitvector expression.</returns>
    internal IntPtr MkRotateLeft(IntPtr ctx, uint i, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_rotate_left");
        var func = Marshal.GetDelegateForFunctionPointer<MkRotateLeftDelegate>(funcPtr);
        return func(ctx, i, t);
    }

    /// <summary>
    /// Creates bitvector rotate right by constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="i">Number of positions to rotate.</param>
    /// <param name="t">Bitvector to rotate.</param>
    /// <returns>Rotated bitvector expression.</returns>
    internal IntPtr MkRotateRight(IntPtr ctx, uint i, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_rotate_right");
        var func = Marshal.GetDelegateForFunctionPointer<MkRotateRightDelegate>(funcPtr);
        return func(ctx, i, t);
    }

    /// <summary>
    /// Creates bitvector rotate left with variable shift amount.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">Bitvector to rotate.</param>
    /// <param name="t2">Shift amount as bitvector.</param>
    /// <returns>Rotated bitvector expression.</returns>
    internal IntPtr MkExtRotateLeft(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ext_rotate_left");
        var func = Marshal.GetDelegateForFunctionPointer<MkExtRotateLeftDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector rotate right with variable shift amount.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">Bitvector to rotate.</param>
    /// <param name="t2">Shift amount as bitvector.</param>
    /// <returns>Rotated bitvector expression.</returns>
    internal IntPtr MkExtRotateRight(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ext_rotate_right");
        var func = Marshal.GetDelegateForFunctionPointer<MkExtRotateRightDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned less-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvULt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvult");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvULtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed less-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvslt");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSLtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned less-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;=u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvULe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvule");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvULeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed less-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &lt;=s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsle");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSLeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned greater-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 >u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvUGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvugt");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvUGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed greater-than comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 >s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsgt");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector unsigned greater-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &gt;=u t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as unsigned integers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvUGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvuge");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvUGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates a Z3 bitvector signed greater-than-or-equal comparison expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">The left-hand side bitvector operand.</param>
    /// <param name="t2">The right-hand side bitvector operand.</param>
    /// <returns>Handle to the created Boolean expression (t1 &gt;=s t2).</returns>
    /// <remarks>
    /// Both operands must be bitvectors of the same width. Treats bitvectors as signed integers
    /// using two's complement representation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvSGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvsge");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvSGeDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector NAND of two bitvectors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First bitvector expression.</param>
    /// <param name="t2">Second bitvector expression.</param>
    /// <returns>AST node representing bitwise NAND of t1 and t2.</returns>
    /// <remarks>
    /// Computes ~(t1 &amp; t2). Both arguments must have the same bit-width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNand(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnand");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNandDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector NOR of two bitvectors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First bitvector expression.</param>
    /// <param name="t2">Second bitvector expression.</param>
    /// <returns>AST node representing bitwise NOR of t1 and t2.</returns>
    /// <remarks>
    /// Computes ~(t1 | t2). Both arguments must have the same bit-width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvNor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvnor");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvNorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector XNOR of two bitvectors.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First bitvector expression.</param>
    /// <param name="t2">Second bitvector expression.</param>
    /// <returns>AST node representing bitwise XNOR of t1 and t2.</returns>
    /// <remarks>
    /// Computes ~(t1 ^ t2). Both arguments must have the same bit-width.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvXnor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvxnor");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvXnorDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates bitvector reduction AND.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">Bitvector expression.</param>
    /// <returns>AST node representing Boolean result of ANDing all bits.</returns>
    /// <remarks>
    /// Returns true (bit 1) if all bits are 1, false (bit 0) otherwise.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvRedAnd(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvredand");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvRedAndDelegate>(funcPtr);
        return func(ctx, t1);
    }

    /// <summary>
    /// Creates bitvector reduction OR.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">Bitvector expression.</param>
    /// <returns>AST node representing Boolean result of ORing all bits.</returns>
    /// <remarks>
    /// Returns true (bit 1) if any bit is 1, false (bit 0) otherwise.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkBvRedOr(IntPtr ctx, IntPtr t1)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_bvredor");
        var func = Marshal.GetDelegateForFunctionPointer<MkBvRedOrDelegate>(funcPtr);
        return func(ctx, t1);
    }
}
