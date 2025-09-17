using Spaceorc.Z3Wrap.BoundaryChecks;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates a fluent boundary check builder for bitvector operations.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A builder for constructing boundary check expressions.</returns>
    public static BitVecBoundaryCheckBuilder BitVecBoundaryCheck(this Z3Context context)
    {
        return new BitVecBoundaryCheckBuilder(context);
    }
}