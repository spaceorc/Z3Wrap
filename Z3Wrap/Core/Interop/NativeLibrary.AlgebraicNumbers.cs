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

    private delegate int GetAlgebraicNumberLowerDelegate(IntPtr ctx, IntPtr algebraic, uint precision);
    private delegate int GetAlgebraicNumberUpperDelegate(IntPtr ctx, IntPtr algebraic, uint precision);

    // Methods

    /// <summary>
    /// Gets lower bound approximation of algebraic number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="algebraic">The algebraic number AST.</param>
    /// <param name="precision">Decimal precision for approximation.</param>
    /// <returns>1 on success, 0 on failure.</returns>
    /// <remarks>
    /// Returns rational approximation that is less than or equal to the algebraic number.
    /// Algebraic numbers are roots of polynomials represented symbolically in Z3.
    /// The approximation is stored in the result parameter of the actual Z3 function.
    /// Precision determines number of decimal digits in approximation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetAlgebraicNumberLower(IntPtr ctx, IntPtr algebraic, uint precision)
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
    /// <returns>1 on success, 0 on failure.</returns>
    /// <remarks>
    /// Returns rational approximation that is greater than or equal to the algebraic number.
    /// Algebraic numbers are roots of polynomials represented symbolically in Z3.
    /// The approximation is stored in the result parameter of the actual Z3 function.
    /// Precision determines number of decimal digits in approximation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetAlgebraicNumberUpper(IntPtr ctx, IntPtr algebraic, uint precision)
    {
        var funcPtr = GetFunctionPointer("Z3_get_algebraic_number_upper");
        var func = Marshal.GetDelegateForFunctionPointer<GetAlgebraicNumberUpperDelegate>(funcPtr);
        return func(ctx, algebraic, precision);
    }
}
