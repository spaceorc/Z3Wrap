// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Algebraic Numbers API - P/Invoke bindings for Z3 algebraic number operations
//
// Source: z3_api.h from Z3 C API (bound approximation functions)
//         z3_algebraic.h from Z3 C API (extended algebraic operations - NOT YET BOUND)
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//      https://github.com/Z3Prover/z3/blob/master/src/api/z3_algebraic.h
//
// This file provides bindings for Z3's algebraic number API (2 functions from z3_api.h):
// - Rational bound approximations for algebraic numbers (lower/upper bounds with precision)
//
// NOTE: Extended algebraic operations from z3_algebraic.h (~20 functions) are NOT yet bound:
// - Algebraic predicates (is_value, is_pos, is_neg, is_zero)
// - Sign operations (algebraic_sign)
// - Arithmetic operations (add, sub, mul, div, root, power)
// - Comparison operations (lt, gt, le, ge, eq, neq)
// - Polynomial operations (roots, eval, get_poly)

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsAlgebraicNumbers(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_algebraic_number_lower");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_algebraic_number_upper");
    }

    // Delegates

    private delegate IntPtr GetAlgebraicNumberLowerDelegate(IntPtr ctx, IntPtr algebraic, uint precision);
    private delegate IntPtr GetAlgebraicNumberUpperDelegate(IntPtr ctx, IntPtr algebraic, uint precision);

    // Methods

    /// <summary>
    /// Gets lower bound approximation of algebraic number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="algebraic">The algebraic number AST.</param>
    /// <param name="precision">Decimal precision for approximation.</param>
    /// <returns>Z3_ast handle representing rational approximation (lower bound).</returns>
    /// <remarks>
    /// Returns rational approximation that is less than or equal to the algebraic number.
    /// The interval isolating the algebraic number is smaller than 1/10^precision.
    /// The result is a numeral AST of sort Real.
    /// Algebraic numbers are roots of polynomials represented symbolically in Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetAlgebraicNumberLower(IntPtr ctx, IntPtr algebraic, uint precision)
    {
        var funcPtr = GetFunctionPointer("Z3_get_algebraic_number_lower");
        var func = Marshal.GetDelegateForFunctionPointer<GetAlgebraicNumberLowerDelegate>(funcPtr);
        return func(ctx, algebraic, precision);
    }

    /// <summary>
    /// Gets upper bound approximation of algebraic number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="algebraic">The algebraic number AST.</param>
    /// <param name="precision">Decimal precision for approximation.</param>
    /// <returns>Z3_ast handle representing rational approximation (upper bound).</returns>
    /// <remarks>
    /// Returns rational approximation that is greater than or equal to the algebraic number.
    /// The interval isolating the algebraic number is smaller than 1/10^precision.
    /// The result is a numeral AST of sort Real.
    /// Algebraic numbers are roots of polynomials represented symbolically in Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetAlgebraicNumberUpper(IntPtr ctx, IntPtr algebraic, uint precision)
    {
        var funcPtr = GetFunctionPointer("Z3_get_algebraic_number_upper");
        var func = Marshal.GetDelegateForFunctionPointer<GetAlgebraicNumberUpperDelegate>(funcPtr);
        return func(ctx, algebraic, precision);
    }
}
