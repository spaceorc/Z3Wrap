using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents static utilities for Z3 library management.
/// </summary>
public static class Z3
{
    private static Z3Library? defaultLibrary;

    /// <summary>
    /// Gets or sets the default Z3 library instance.
    /// <para>
    /// When getting: If no library has been set, automatically initializes using <see cref="Z3Library.LoadAuto"/>.
    /// </para>
    /// <para>
    /// When setting: Ownership of the <see cref="Z3Library"/> instance is transferred to this class.
    /// The previous library (if any) is automatically disposed. The caller must NOT call
    /// <see cref="IDisposable.Dispose"/> on the library after setting it here. Thread-safe using atomic exchange.
    /// </para>
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when attempting to set a null library.</exception>
    public static Z3Library DefaultLibrary
    {
        get
        {
            if (defaultLibrary != null)
                return defaultLibrary;
            var newLibrary = Z3Library.LoadAuto();
            var original = Interlocked.CompareExchange(ref defaultLibrary, newLibrary, null);
            if (original != null)
            {
                // Another thread won the race, dispose our library
                newLibrary.Dispose();
            }
            return defaultLibrary;
        }
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot set DefaultLibrary to null");

            // Atomically exchange the library and dispose the old one
            var oldLibrary = Interlocked.Exchange(ref defaultLibrary, value);
            oldLibrary?.Dispose();
        }
    }

    /// <summary>
    /// Loads Z3 native library from the specified path and sets it as the default.
    /// <para>
    /// This method transfers ownership to the <see cref="Z3"/> class. The previous library
    /// (if any) is automatically disposed. Thread-safe.
    /// </para>
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 native library.</param>
    /// <exception cref="ArgumentException">Thrown when the library path is null, empty, or whitespace.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the library file is not found at the specified path.</exception>
    public static void LoadLibrary(string libraryPath)
    {
        DefaultLibrary = Z3Library.Load(libraryPath);
    }

    /// <summary>
    /// Loads Z3 native library using automatic discovery and sets it as the default.
    /// <para>
    /// This method transfers ownership to the <see cref="Z3"/> class. The previous library
    /// (if any) is automatically disposed. Thread-safe.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the Z3 library cannot be automatically located.</exception>
    public static void LoadLibraryAuto()
    {
        DefaultLibrary = Z3Library.LoadAuto();
    }
}
