using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Extension methods for floating-point conversion operations.
/// </summary>
public static class FpConversionExprExtensions
{
    /// <summary>
    /// Converts floating-point expression to different format.
    /// </summary>
    /// <typeparam name="TSource">The source format.</typeparam>
    /// <typeparam name="TTarget">The target format.</typeparam>
    /// <param name="expr">The expression to convert.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Converted floating-point expression.</returns>
    public static FpExpr<TTarget> ToFormat<TSource, TTarget>(this FpExpr<TSource> expr, RoundingModeExpr roundingMode)
        where TSource : IFloatFormat
        where TTarget : IFloatFormat
    {
        var targetSort = expr.Context.Library.MkFpaSort(
            expr.Context.Handle,
            TTarget.ExponentBits,
            TTarget.SignificandBits
        );
        var handle = expr.Context.Library.MkFpaToFpFloat(
            expr.Context.Handle,
            roundingMode.Handle,
            expr.Handle,
            targetSort
        );
        return new FpExpr<TTarget>(expr.Context, handle);
    }

    /// <summary>
    /// Converts floating-point expression to Float16 format.
    /// </summary>
    /// <typeparam name="TFormat">The source format.</typeparam>
    /// <param name="expr">The expression to convert.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Float16 expression.</returns>
    public static FpExpr<Float16> ToFloat16<TFormat>(this FpExpr<TFormat> expr, RoundingModeExpr roundingMode)
        where TFormat : IFloatFormat
    {
        return expr.ToFormat<TFormat, Float16>(roundingMode);
    }

    /// <summary>
    /// Converts floating-point expression to Float32 format.
    /// </summary>
    /// <typeparam name="TFormat">The source format.</typeparam>
    /// <param name="expr">The expression to convert.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Float32 expression.</returns>
    public static FpExpr<Float32> ToFloat32<TFormat>(this FpExpr<TFormat> expr, RoundingModeExpr roundingMode)
        where TFormat : IFloatFormat
    {
        return expr.ToFormat<TFormat, Float32>(roundingMode);
    }

    /// <summary>
    /// Converts floating-point expression to Float64 format.
    /// </summary>
    /// <typeparam name="TFormat">The source format.</typeparam>
    /// <param name="expr">The expression to convert.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Float64 expression.</returns>
    public static FpExpr<Float64> ToFloat64<TFormat>(this FpExpr<TFormat> expr, RoundingModeExpr roundingMode)
        where TFormat : IFloatFormat
    {
        return expr.ToFormat<TFormat, Float64>(roundingMode);
    }

    // TODO: Add ToSignedBv and ToUnsignedBv methods
    // These require typed BvExpr<TSize> return types which need size type parameters
    // to be properly implemented

    /// <summary>
    /// Converts floating-point expression to real number.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The expression to convert.</param>
    /// <returns>Real expression.</returns>
    public static RealExpr ToReal<TFormat>(this FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var handle = expr.Context.Library.MkFpaToReal(expr.Context.Handle, expr.Handle);
        return Z3Expr.Create<RealExpr>(expr.Context, handle);
    }

    /// <summary>
    /// Converts signed bit-vector to floating-point format.
    /// </summary>
    /// <typeparam name="TFormat">The target floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="bvExpr">The signed bit-vector expression.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Floating-point expression.</returns>
    public static FpExpr<TFormat> FpFromSignedBv<TFormat>(
        this Z3Context context,
        Z3Expr bvExpr,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkFpaToFpSigned(context.Handle, roundingMode.Handle, bvExpr.Handle, sort);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Converts unsigned bit-vector to floating-point format.
    /// </summary>
    /// <typeparam name="TFormat">The target floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="bvExpr">The unsigned bit-vector expression.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Floating-point expression.</returns>
    public static FpExpr<TFormat> FpFromUnsignedBv<TFormat>(
        this Z3Context context,
        Z3Expr bvExpr,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkFpaToFpUnsigned(context.Handle, roundingMode.Handle, bvExpr.Handle, sort);
        return new FpExpr<TFormat>(context, handle);
    }

    /// <summary>
    /// Converts real expression to floating-point format.
    /// </summary>
    /// <typeparam name="TFormat">The target floating-point format.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="realExpr">The real expression.</param>
    /// <param name="roundingMode">The rounding mode.</param>
    /// <returns>Floating-point expression.</returns>
    public static FpExpr<TFormat> FpFromReal<TFormat>(
        this Z3Context context,
        RealExpr realExpr,
        RoundingModeExpr roundingMode
    )
        where TFormat : IFloatFormat
    {
        var sort = context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);
        var handle = context.Library.MkFpaToFpReal(context.Handle, roundingMode.Handle, realExpr.Handle, sort);
        return new FpExpr<TFormat>(context, handle);
    }
}
