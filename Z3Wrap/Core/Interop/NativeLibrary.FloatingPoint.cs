using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for IEEE 754 floating-point theory functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for floating-point Z3 API functions.
    /// </summary>
    private static void LoadFunctionsFloatingPoint(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Sort creation
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rounding_mode_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_half");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_16");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_single");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_32");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_double");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_quadruple");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sort_128");

        // Rounding mode constants
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_nearest_ties_to_even");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rne");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_nearest_ties_to_away");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rna");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_toward_positive");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rtp");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_toward_negative");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rtn");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_toward_zero");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rtz");

        // Numeral creation
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_numeral_float");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_numeral_double");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_numeral_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_numeral_int_uint");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_numeral_int64_uint");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_nan");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_inf");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_zero");

        // Arithmetic operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_abs");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_neg");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_add");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sub");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_mul");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_div");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_fma");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_sqrt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_rem");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_round_to_integral");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_min");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_max");

        // Comparison operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_leq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_lt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_geq");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_gt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_eq");

        // Predicates
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_normal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_subnormal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_zero");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_infinite");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_nan");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_negative");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_is_positive");

        // Conversions
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_fp_bv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_fp_float");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_fp_real");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_fp_signed");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_fp_unsigned");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_ubv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_sbv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_real");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_fpa_to_ieee_bv");

        // Query functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_ebits");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_sbits");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_nan");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_inf");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_zero");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_normal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_subnormal");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_positive");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_is_numeral_negative");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_sign");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_significand_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_significand_uint64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_exponent_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_exponent_int64");
        LoadFunctionOrNull(handle, functionPointers, "Z3_fpa_get_numeral_exponent_bv");
    }

    // Delegates

    // Sort creation delegates
    private delegate IntPtr MkFpaRoundingModeSortDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaSortDelegate(IntPtr ctx, uint ebits, uint sbits);
    private delegate IntPtr MkFpaSortHalfDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaSort16Delegate(IntPtr ctx);
    private delegate IntPtr MkFpaSortSingleDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaSort32Delegate(IntPtr ctx);
    private delegate IntPtr MkFpaSortDoubleDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaSort64Delegate(IntPtr ctx);
    private delegate IntPtr MkFpaSortQuadrupleDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaSort128Delegate(IntPtr ctx);

    // Rounding mode constant delegates
    private delegate IntPtr MkFpaRoundNearestTiesToEvenDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRneDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRoundNearestTiesToAwayDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRnaDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRoundTowardPositiveDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRtpDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRoundTowardNegativeDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRtnDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRoundTowardZeroDelegate(IntPtr ctx);
    private delegate IntPtr MkFpaRtzDelegate(IntPtr ctx);

    // Numeral creation delegates
    private delegate IntPtr MkFpaNumeralFloatDelegate(IntPtr ctx, float v, IntPtr ty);
    private delegate IntPtr MkFpaNumeralDoubleDelegate(IntPtr ctx, double v, IntPtr ty);
    private delegate IntPtr MkFpaNumeralIntDelegate(IntPtr ctx, int v, IntPtr ty);
    private delegate IntPtr MkFpaNumeralIntUintDelegate(IntPtr ctx, bool sgn, int exp, uint sig, IntPtr ty);
    private delegate IntPtr MkFpaNumeralInt64UintDelegate(IntPtr ctx, bool sgn, long exp, ulong sig, IntPtr ty);
    private delegate IntPtr MkFpaNanDelegate(IntPtr ctx, IntPtr ty);
    private delegate IntPtr MkFpaInfDelegate(IntPtr ctx, IntPtr ty, bool negative);
    private delegate IntPtr MkFpaZeroDelegate(IntPtr ctx, IntPtr ty, bool negative);

    // Arithmetic operation delegates
    private delegate IntPtr MkFpaAbsDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaNegDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaAddDelegate(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaSubDelegate(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaMulDelegate(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaDivDelegate(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaFmaDelegate(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2, IntPtr t3);
    private delegate IntPtr MkFpaSqrtDelegate(IntPtr ctx, IntPtr rm, IntPtr t);
    private delegate IntPtr MkFpaRemDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaRoundToIntegralDelegate(IntPtr ctx, IntPtr rm, IntPtr t);
    private delegate IntPtr MkFpaMinDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaMaxDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Comparison operation delegates
    private delegate IntPtr MkFpaLeqDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaLtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaGeqDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaGtDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate IntPtr MkFpaEqDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);

    // Predicate delegates
    private delegate IntPtr MkFpaIsNormalDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsSubnormalDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsZeroDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsInfiniteDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsNanDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsNegativeDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaIsPositiveDelegate(IntPtr ctx, IntPtr t);

    // Conversion delegates
    private delegate IntPtr MkFpaToFpBvDelegate(IntPtr ctx, IntPtr bv, IntPtr s);
    private delegate IntPtr MkFpaToFpFloatDelegate(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s);
    private delegate IntPtr MkFpaToFpRealDelegate(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s);
    private delegate IntPtr MkFpaToFpSignedDelegate(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s);
    private delegate IntPtr MkFpaToFpUnsignedDelegate(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s);
    private delegate IntPtr MkFpaToUbvDelegate(IntPtr ctx, IntPtr rm, IntPtr t, uint sz);
    private delegate IntPtr MkFpaToSbvDelegate(IntPtr ctx, IntPtr rm, IntPtr t, uint sz);
    private delegate IntPtr MkFpaToRealDelegate(IntPtr ctx, IntPtr t);
    private delegate IntPtr MkFpaToIeeeBvDelegate(IntPtr ctx, IntPtr t);

    // Query function delegates
    private delegate uint FpaGetEbitsDelegate(IntPtr ctx, IntPtr s);
    private delegate uint FpaGetSbitsDelegate(IntPtr ctx, IntPtr s);
    private delegate bool FpaIsNumeralNanDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralInfDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralZeroDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralNormalDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralSubnormalDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralPositiveDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaIsNumeralNegativeDelegate(IntPtr ctx, IntPtr t);
    private delegate bool FpaGetNumeralSignDelegate(IntPtr ctx, IntPtr t, out int sgn);
    private delegate bool FpaGetNumeralSignificandStringDelegate(IntPtr ctx, IntPtr t, out IntPtr result);
    private delegate bool FpaGetNumeralSignificandUint64Delegate(IntPtr ctx, IntPtr t, out ulong result);
    private delegate bool FpaGetNumeralExponentStringDelegate(IntPtr ctx, IntPtr t, bool biased, out IntPtr result);
    private delegate bool FpaGetNumeralExponentInt64Delegate(IntPtr ctx, IntPtr t, out long result);
    private delegate bool FpaGetNumeralExponentBvDelegate(IntPtr ctx, IntPtr t, bool biased, out IntPtr result);

    // Methods

    // Sort creation methods

    /// <summary>
    /// Creates floating-point rounding mode sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>Rounding mode sort.</returns>
    internal IntPtr MkFpaRoundingModeSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rounding_mode_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundingModeSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates floating-point sort with specified exponent and significand bit widths.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="ebits">Exponent bit width (must be greater than 1).</param>
    /// <param name="sbits">Significand bit width (must be greater than 2).</param>
    /// <returns>Floating-point sort.</returns>
    internal IntPtr MkFpaSort(IntPtr ctx, uint ebits, uint sbits)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSortDelegate>(funcPtr);
        return func(ctx, ebits, sbits);
    }

    /// <summary>
    /// Creates half-precision (16-bit) floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>Half-precision FP sort (5-bit exponent, 11-bit significand).</returns>
    internal IntPtr MkFpaSortHalf(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_half");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSortHalfDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates 16-bit floating-point sort (alias for half-precision).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>16-bit FP sort.</returns>
    internal IntPtr MkFpaSort16(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_16");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSort16Delegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates single-precision (32-bit) floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>Single-precision FP sort (8-bit exponent, 24-bit significand).</returns>
    internal IntPtr MkFpaSortSingle(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_single");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSortSingleDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates 32-bit floating-point sort (alias for single-precision).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>32-bit FP sort.</returns>
    internal IntPtr MkFpaSort32(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_32");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSort32Delegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates double-precision (64-bit) floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>Double-precision FP sort (11-bit exponent, 53-bit significand).</returns>
    internal IntPtr MkFpaSortDouble(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_double");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSortDoubleDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates 64-bit floating-point sort (alias for double-precision).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>64-bit FP sort.</returns>
    internal IntPtr MkFpaSort64(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_64");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSort64Delegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates quadruple-precision (128-bit) floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>Quadruple-precision FP sort (15-bit exponent, 113-bit significand).</returns>
    internal IntPtr MkFpaSortQuadruple(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_quadruple");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSortQuadrupleDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates 128-bit floating-point sort (alias for quadruple-precision).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>128-bit FP sort.</returns>
    internal IntPtr MkFpaSort128(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sort_128");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSort128Delegate>(funcPtr);
        return func(ctx);
    }

    // Rounding mode constant methods

    /// <summary>
    /// Creates round-nearest-ties-to-even rounding mode constant (RNA).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RNA rounding mode.</returns>
    internal IntPtr MkFpaRoundNearestTiesToEven(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_nearest_ties_to_even");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundNearestTiesToEvenDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates RNE rounding mode constant (alias for round-nearest-ties-to-even).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RNE rounding mode.</returns>
    internal IntPtr MkFpaRne(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rne");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRneDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates round-nearest-ties-to-away rounding mode constant (RTA).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTA rounding mode.</returns>
    internal IntPtr MkFpaRoundNearestTiesToAway(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_nearest_ties_to_away");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundNearestTiesToAwayDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates RNA rounding mode constant (alias for round-nearest-ties-to-away).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RNA rounding mode.</returns>
    internal IntPtr MkFpaRna(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rna");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRnaDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates round-toward-positive rounding mode constant (RTP, toward +infinity).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTP rounding mode.</returns>
    internal IntPtr MkFpaRoundTowardPositive(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_toward_positive");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundTowardPositiveDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates RTP rounding mode constant (alias for round-toward-positive).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTP rounding mode.</returns>
    internal IntPtr MkFpaRtp(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rtp");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRtpDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates round-toward-negative rounding mode constant (RTN, toward -infinity).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTN rounding mode.</returns>
    internal IntPtr MkFpaRoundTowardNegative(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_toward_negative");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundTowardNegativeDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates RTN rounding mode constant (alias for round-toward-negative).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTN rounding mode.</returns>
    internal IntPtr MkFpaRtn(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rtn");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRtnDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates round-toward-zero rounding mode constant (RTZ).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTZ rounding mode.</returns>
    internal IntPtr MkFpaRoundTowardZero(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_toward_zero");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundTowardZeroDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates RTZ rounding mode constant (alias for round-toward-zero).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <returns>RTZ rounding mode.</returns>
    internal IntPtr MkFpaRtz(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rtz");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRtzDelegate>(funcPtr);
        return func(ctx);
    }

    // Numeral creation methods

    /// <summary>
    /// Creates floating-point numeral from C float value.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="v">Float value.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>FP numeral expression.</returns>
    internal IntPtr MkFpaNumeralFloat(IntPtr ctx, float v, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_numeral_float");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNumeralFloatDelegate>(funcPtr);
        return func(ctx, v, ty);
    }

    /// <summary>
    /// Creates floating-point numeral from C double value.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="v">Double value.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>FP numeral expression.</returns>
    internal IntPtr MkFpaNumeralDouble(IntPtr ctx, double v, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_numeral_double");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNumeralDoubleDelegate>(funcPtr);
        return func(ctx, v, ty);
    }

    /// <summary>
    /// Creates floating-point numeral from signed integer value.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="v">Integer value.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>FP numeral expression.</returns>
    internal IntPtr MkFpaNumeralInt(IntPtr ctx, int v, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_numeral_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNumeralIntDelegate>(funcPtr);
        return func(ctx, v, ty);
    }

    /// <summary>
    /// Creates floating-point numeral from sign, exponent, and significand components.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="sgn">Sign bit (true for negative).</param>
    /// <param name="exp">Exponent value.</param>
    /// <param name="sig">Significand value.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>FP numeral expression.</returns>
    internal IntPtr MkFpaNumeralIntUint(IntPtr ctx, bool sgn, int exp, uint sig, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_numeral_int_uint");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNumeralIntUintDelegate>(funcPtr);
        return func(ctx, sgn, exp, sig, ty);
    }

    /// <summary>
    /// Creates floating-point numeral from sign, 64-bit exponent, and 64-bit significand.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="sgn">Sign bit (true for negative).</param>
    /// <param name="exp">64-bit exponent value.</param>
    /// <param name="sig">64-bit significand value.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>FP numeral expression.</returns>
    internal IntPtr MkFpaNumeralInt64Uint(IntPtr ctx, bool sgn, long exp, ulong sig, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_numeral_int64_uint");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNumeralInt64UintDelegate>(funcPtr);
        return func(ctx, sgn, exp, sig, ty);
    }

    /// <summary>
    /// Creates NaN (Not-a-Number) floating-point constant.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <returns>NaN constant.</returns>
    internal IntPtr MkFpaNan(IntPtr ctx, IntPtr ty)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_nan");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNanDelegate>(funcPtr);
        return func(ctx, ty);
    }

    /// <summary>
    /// Creates infinity floating-point constant (signed).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <param name="negative">True for negative infinity, false for positive.</param>
    /// <returns>Infinity constant.</returns>
    internal IntPtr MkFpaInf(IntPtr ctx, IntPtr ty, bool negative)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_inf");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaInfDelegate>(funcPtr);
        return func(ctx, ty, negative);
    }

    /// <summary>
    /// Creates zero floating-point constant (signed).
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="ty">Floating-point sort.</param>
    /// <param name="negative">True for negative zero, false for positive.</param>
    /// <returns>Zero constant.</returns>
    internal IntPtr MkFpaZero(IntPtr ctx, IntPtr ty, bool negative)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_zero");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaZeroDelegate>(funcPtr);
        return func(ctx, ty, negative);
    }

    // Arithmetic operation methods

    /// <summary>
    /// Creates floating-point absolute value expression.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Absolute value expression.</returns>
    internal IntPtr MkFpaAbs(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_abs");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaAbsDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates floating-point negation expression.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Negation expression.</returns>
    internal IntPtr MkFpaNeg(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_neg");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaNegDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates floating-point addition expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Addition expression.</returns>
    internal IntPtr MkFpaAdd(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_add");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaAddDelegate>(funcPtr);
        return func(ctx, rm, t1, t2);
    }

    /// <summary>
    /// Creates floating-point subtraction expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t1">Minuend operand.</param>
    /// <param name="t2">Subtrahend operand.</param>
    /// <returns>Subtraction expression.</returns>
    internal IntPtr MkFpaSub(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sub");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSubDelegate>(funcPtr);
        return func(ctx, rm, t1, t2);
    }

    /// <summary>
    /// Creates floating-point multiplication expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Multiplication expression.</returns>
    internal IntPtr MkFpaMul(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_mul");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaMulDelegate>(funcPtr);
        return func(ctx, rm, t1, t2);
    }

    /// <summary>
    /// Creates floating-point division expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t1">Dividend operand.</param>
    /// <param name="t2">Divisor operand.</param>
    /// <returns>Division expression.</returns>
    internal IntPtr MkFpaDiv(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_div");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaDivDelegate>(funcPtr);
        return func(ctx, rm, t1, t2);
    }

    /// <summary>
    /// Creates floating-point fused multiply-add expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t1">First multiplication operand.</param>
    /// <param name="t2">Second multiplication operand.</param>
    /// <param name="t3">Addition operand.</param>
    /// <returns>Fused multiply-add expression (t1 * t2 + t3).</returns>
    internal IntPtr MkFpaFma(IntPtr ctx, IntPtr rm, IntPtr t1, IntPtr t2, IntPtr t3)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_fma");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaFmaDelegate>(funcPtr);
        return func(ctx, rm, t1, t2, t3);
    }

    /// <summary>
    /// Creates floating-point square root expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Operand.</param>
    /// <returns>Square root expression.</returns>
    internal IntPtr MkFpaSqrt(IntPtr ctx, IntPtr rm, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_sqrt");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaSqrtDelegate>(funcPtr);
        return func(ctx, rm, t);
    }

    /// <summary>
    /// Creates floating-point remainder expression.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">Dividend operand.</param>
    /// <param name="t2">Divisor operand.</param>
    /// <returns>Remainder expression.</returns>
    internal IntPtr MkFpaRem(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_rem");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRemDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point round-to-integral expression with rounding mode.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Operand.</param>
    /// <returns>Round-to-integral expression.</returns>
    internal IntPtr MkFpaRoundToIntegral(IntPtr ctx, IntPtr rm, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_round_to_integral");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaRoundToIntegralDelegate>(funcPtr);
        return func(ctx, rm, t);
    }

    /// <summary>
    /// Creates floating-point minimum expression.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Minimum expression.</returns>
    internal IntPtr MkFpaMin(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_min");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaMinDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point maximum expression.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Maximum expression.</returns>
    internal IntPtr MkFpaMax(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_max");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaMaxDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    // Comparison operation methods

    /// <summary>
    /// Creates floating-point less-or-equal comparison.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Less-or-equal comparison expression.</returns>
    internal IntPtr MkFpaLeq(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_leq");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaLeqDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point less-than comparison.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Less-than comparison expression.</returns>
    internal IntPtr MkFpaLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_lt");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaLtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point greater-or-equal comparison.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Greater-or-equal comparison expression.</returns>
    internal IntPtr MkFpaGeq(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_geq");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaGeqDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point greater-than comparison.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Greater-than comparison expression.</returns>
    internal IntPtr MkFpaGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_gt");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaGtDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Creates floating-point equality comparison.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t1">First operand.</param>
    /// <param name="t2">Second operand.</param>
    /// <returns>Equality comparison expression.</returns>
    internal IntPtr MkFpaEq(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_eq");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaEqDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    // Predicate methods

    /// <summary>
    /// Creates predicate checking if FP value is normal.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsNormal(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_normal");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsNormalDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is subnormal.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsSubnormal(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_subnormal");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsSubnormalDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is zero.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsZero(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_zero");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsZeroDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is infinite.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsInfinite(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_infinite");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsInfiniteDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is NaN.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsNan(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_nan");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsNanDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is negative.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsNegative(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_negative");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsNegativeDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates predicate checking if FP value is positive.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Boolean predicate expression.</returns>
    internal IntPtr MkFpaIsPositive(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_is_positive");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaIsPositiveDelegate>(funcPtr);
        return func(ctx, t);
    }

    // Conversion methods

    /// <summary>
    /// Creates floating-point from IEEE bitvector representation.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="bv">IEEE bitvector expression.</param>
    /// <param name="s">Target FP sort.</param>
    /// <returns>FP expression.</returns>
    internal IntPtr MkFpaToFpBv(IntPtr ctx, IntPtr bv, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_fp_bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToFpBvDelegate>(funcPtr);
        return func(ctx, bv, s);
    }

    /// <summary>
    /// Creates floating-point from another FP with different precision and rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Source FP expression.</param>
    /// <param name="s">Target FP sort.</param>
    /// <returns>Converted FP expression.</returns>
    internal IntPtr MkFpaToFpFloat(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_fp_float");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToFpFloatDelegate>(funcPtr);
        return func(ctx, rm, t, s);
    }

    /// <summary>
    /// Creates floating-point from real number with rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Real expression.</param>
    /// <param name="s">Target FP sort.</param>
    /// <returns>FP expression.</returns>
    internal IntPtr MkFpaToFpReal(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_fp_real");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToFpRealDelegate>(funcPtr);
        return func(ctx, rm, t, s);
    }

    /// <summary>
    /// Creates floating-point from signed bitvector with rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Signed bitvector expression.</param>
    /// <param name="s">Target FP sort.</param>
    /// <returns>FP expression.</returns>
    internal IntPtr MkFpaToFpSigned(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_fp_signed");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToFpSignedDelegate>(funcPtr);
        return func(ctx, rm, t, s);
    }

    /// <summary>
    /// Creates floating-point from unsigned bitvector with rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">Unsigned bitvector expression.</param>
    /// <param name="s">Target FP sort.</param>
    /// <returns>FP expression.</returns>
    internal IntPtr MkFpaToFpUnsigned(IntPtr ctx, IntPtr rm, IntPtr t, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_fp_unsigned");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToFpUnsignedDelegate>(funcPtr);
        return func(ctx, rm, t, s);
    }

    /// <summary>
    /// Creates unsigned bitvector from floating-point with rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">FP expression.</param>
    /// <param name="sz">Target bitvector width.</param>
    /// <returns>Unsigned bitvector expression.</returns>
    internal IntPtr MkFpaToUbv(IntPtr ctx, IntPtr rm, IntPtr t, uint sz)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_ubv");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToUbvDelegate>(funcPtr);
        return func(ctx, rm, t, sz);
    }

    /// <summary>
    /// Creates signed bitvector from floating-point with rounding.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="rm">Rounding mode.</param>
    /// <param name="t">FP expression.</param>
    /// <param name="sz">Target bitvector width.</param>
    /// <returns>Signed bitvector expression.</returns>
    internal IntPtr MkFpaToSbv(IntPtr ctx, IntPtr rm, IntPtr t, uint sz)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_sbv");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToSbvDelegate>(funcPtr);
        return func(ctx, rm, t, sz);
    }

    /// <summary>
    /// Creates real number from floating-point.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>Real expression.</returns>
    internal IntPtr MkFpaToReal(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_real");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToRealDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Creates IEEE bitvector representation from floating-point.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP expression.</param>
    /// <returns>IEEE bitvector expression.</returns>
    internal IntPtr MkFpaToIeeeBv(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_fpa_to_ieee_bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkFpaToIeeeBvDelegate>(funcPtr);
        return func(ctx, t);
    }

    // Query function methods

    /// <summary>
    /// Gets exponent bit width from floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="s">FP sort.</param>
    /// <returns>Exponent bit width.</returns>
    internal uint FpaGetEbits(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_ebits");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetEbitsDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Gets significand bit width from floating-point sort.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="s">FP sort.</param>
    /// <returns>Significand bit width.</returns>
    internal uint FpaGetSbits(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_sbits");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetSbitsDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if FP numeral is NaN.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if NaN.</returns>
    internal bool FpaIsNumeralNan(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_nan");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralNanDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is infinity.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if infinity.</returns>
    internal bool FpaIsNumeralInf(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_inf");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralInfDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is zero.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if zero.</returns>
    internal bool FpaIsNumeralZero(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_zero");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralZeroDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is normal.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if normal.</returns>
    internal bool FpaIsNumeralNormal(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_normal");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralNormalDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is subnormal.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if subnormal.</returns>
    internal bool FpaIsNumeralSubnormal(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_subnormal");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralSubnormalDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is positive.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if positive.</returns>
    internal bool FpaIsNumeralPositive(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_positive");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralPositiveDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if FP numeral is negative.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <returns>True if negative.</returns>
    internal bool FpaIsNumeralNegative(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_is_numeral_negative");
        var func = Marshal.GetDelegateForFunctionPointer<FpaIsNumeralNegativeDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Gets sign bit from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="sgn">Output sign (0 for positive, 1 for negative).</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralSign(IntPtr ctx, IntPtr t, out int sgn)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_sign");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralSignDelegate>(funcPtr);
        return func(ctx, t, out sgn);
    }

    /// <summary>
    /// Gets significand as string from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="result">Output significand string pointer.</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralSignificandString(IntPtr ctx, IntPtr t, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_significand_string");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralSignificandStringDelegate>(funcPtr);
        return func(ctx, t, out result);
    }

    /// <summary>
    /// Gets significand as 64-bit unsigned integer from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="result">Output significand value.</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralSignificandUint64(IntPtr ctx, IntPtr t, out ulong result)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_significand_uint64");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralSignificandUint64Delegate>(funcPtr);
        return func(ctx, t, out result);
    }

    /// <summary>
    /// Gets exponent as string from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="biased">True for biased representation.</param>
    /// <param name="result">Output exponent string pointer.</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralExponentString(IntPtr ctx, IntPtr t, bool biased, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_exponent_string");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralExponentStringDelegate>(funcPtr);
        return func(ctx, t, biased, out result);
    }

    /// <summary>
    /// Gets exponent as 64-bit signed integer from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="result">Output exponent value.</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralExponentInt64(IntPtr ctx, IntPtr t, out long result)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_exponent_int64");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralExponentInt64Delegate>(funcPtr);
        return func(ctx, t, out result);
    }

    /// <summary>
    /// Gets exponent as bitvector from FP numeral.
    /// </summary>
    /// <param name="ctx">The context.</param>
    /// <param name="t">FP numeral.</param>
    /// <param name="biased">True for biased representation.</param>
    /// <param name="result">Output exponent bitvector expression.</param>
    /// <returns>True if successful.</returns>
    internal bool FpaGetNumeralExponentBv(IntPtr ctx, IntPtr t, bool biased, out IntPtr result)
    {
        var funcPtr = GetFunctionPointer("Z3_fpa_get_numeral_exponent_bv");
        var func = Marshal.GetDelegateForFunctionPointer<FpaGetNumeralExponentBvDelegate>(funcPtr);
        return func(ctx, t, biased, out result);
    }
}
