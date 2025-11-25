using Spaceorc.Z3Wrap.Core;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Extension methods for creating rounding mode expressions.
/// </summary>
public static class RoundingModeContextExtensions
{
    /// <summary>
    /// Creates rounding mode expression from specified mode.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="mode">The rounding mode.</param>
    /// <returns>Rounding mode expression.</returns>
    public static RoundingModeExpr RoundingMode(this Z3Context context, RoundingMode mode)
    {
        var handle = mode switch
        {
            FloatingPoint.RoundingMode.NearestTiesToEven => context.Library.MkFpaRoundNearestTiesToEven(context.Handle),
            FloatingPoint.RoundingMode.NearestTiesToAway => context.Library.MkFpaRoundNearestTiesToAway(context.Handle),
            FloatingPoint.RoundingMode.TowardPositive => context.Library.MkFpaRoundTowardPositive(context.Handle),
            FloatingPoint.RoundingMode.TowardNegative => context.Library.MkFpaRoundTowardNegative(context.Handle),
            FloatingPoint.RoundingMode.TowardZero => context.Library.MkFpaRoundTowardZero(context.Handle),
            _ => throw new ArgumentException($"Unknown rounding mode: {mode}", nameof(mode)),
        };

        return new RoundingModeExpr(context, handle);
    }
}
