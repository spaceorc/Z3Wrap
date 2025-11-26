using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Extension methods for creating floating-point expressions.
/// </summary>
public static class FpContextExtensions
{
    /// <summary>
    /// Creates floating-point constant with specified name and format.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Floating-point constant expression.</returns>
    public static FpExpr<TFormat> FpConst<TFormat>(this Z3Context context, string name)
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkConst(context.Handle, name, sort);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Creates floating-point value from Half.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The Half value.</param>
    /// <returns>Floating-point expression representing the value.</returns>
    public static FpExpr<Float16> Fp(this Z3Context context, Half value)
    {
        var sort = context.Library.MkFpaSort(context.Handle, Float16.ExponentBits, Float16.SignificandBits);
        // Convert Half to float since Z3 doesn't have native Half support
        var handle = context.Library.MkFpaNumeralFloat(context.Handle, (float)value, sort);
        return new FpExpr<Float16>(context, handle);
    }

    /// <summary>
    /// Creates floating-point value from float.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The float value.</param>
    /// <returns>Floating-point expression representing the value.</returns>
    public static FpExpr<Float32> Fp(this Z3Context context, float value)
    {
        var sort = context.Library.MkFpaSort(context.Handle, Float32.ExponentBits, Float32.SignificandBits);
        var handle = context.Library.MkFpaNumeralFloat(context.Handle, value, sort);
        return new FpExpr<Float32>(context, handle);
    }

    /// <summary>
    /// Creates floating-point value from double.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The double value.</param>
    /// <returns>Floating-point expression representing the value.</returns>
    public static FpExpr<Float64> Fp(this Z3Context context, double value)
    {
        var sort = context.Library.MkFpaSort(context.Handle, Float64.ExponentBits, Float64.SignificandBits);
        var handle = context.Library.MkFpaNumeralDouble(context.Handle, value, sort);
        return new FpExpr<Float64>(context, handle);
    }

    /// <summary>
    /// Creates floating-point value from double with explicit format.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The double value.</param>
    /// <returns>Floating-point expression representing the value.</returns>
    public static FpExpr<TFormat> Fp<TFormat>(this Z3Context context, double value)
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkFpaNumeralDouble(context.Handle, value, sort);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Creates NaN (Not a Number) floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <returns>NaN floating-point expression.</returns>
    public static FpExpr<TFormat> FpNaN<TFormat>(this Z3Context context)
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkFpaNan(context.Handle, sort);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Creates infinity floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="negative">True for negative infinity, false for positive.</param>
    /// <returns>Infinity floating-point expression.</returns>
    public static FpExpr<TFormat> FpInfinity<TFormat>(this Z3Context context, bool negative = false)
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = negative
            ? context.Library.MkFpaInf(context.Handle, sort, true)
            : context.Library.MkFpaInf(context.Handle, sort, false);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Creates zero floating-point value.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="negative">True for negative zero, false for positive.</param>
    /// <returns>Zero floating-point expression.</returns>
    public static FpExpr<TFormat> FpZero<TFormat>(this Z3Context context, bool negative = false)
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = negative
            ? context.Library.MkFpaZero(context.Handle, sort, true)
            : context.Library.MkFpaZero(context.Handle, sort, false);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Creates floating-point value from IEEE 754 bit components.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="sign">Sign bit (true for negative, false for positive).</param>
    /// <param name="exponent">Biased exponent value.</param>
    /// <param name="significand">Significand bits (without implicit leading 1).</param>
    /// <returns>Floating-point expression constructed from components.</returns>
    public static FpExpr<TFormat> FpFromComponents<TFormat>(
        this Z3Context context,
        bool sign,
        ulong exponent,
        ulong significand
    )
        where TFormat : IFloatFormat
    {
        using var signBv = new LocalZ3Handle(
            context,
            context.Library.MkUnsignedInt64(
                context.Handle,
                sign ? 1UL : 0UL,
                context.Library.MkBvSort(context.Handle, 1)
            )
        );

        using var expBv = new LocalZ3Handle(
            context,
            context.Library.MkUnsignedInt64(
                context.Handle,
                exponent,
                context.Library.MkBvSort(context.Handle, TFormat.ExponentBits)
            )
        );

        var sigBits = TFormat.SignificandBits - 1;
        using var sigBv = new LocalZ3Handle(
            context,
            context.Library.MkUnsignedInt64(
                context.Handle,
                significand,
                context.Library.MkBvSort(context.Handle, sigBits)
            )
        );

        var fpAst = context.Library.MkFpaFp(context.Handle, signBv.Handle, expBv.Handle, sigBv.Handle);

        return new FpExpr<TFormat>(context, fpAst);
    }
}
