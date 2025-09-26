namespace Spaceorc.Z3Wrap.Booleans;

public sealed partial class Z3Bool
{
    /// <summary>
    /// Implicitly converts a Boolean value to a Z3Bool using the current thread-local context.
    /// </summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>A Z3Bool representing the Boolean constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3Bool(bool value) => Z3Context.Current.Bool(value);
}