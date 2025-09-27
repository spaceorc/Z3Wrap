using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents static utilities for Z3 library management.
/// </summary>
public static class Z3
{
    /// <summary>
    /// Loads Z3 native library from the specified path.
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 native library.</param>
    public static void LoadLibrary(string libraryPath)
    {
        NativeMethods.LoadLibrary(libraryPath);
    }

    /// <summary>
    /// Loads Z3 native library using automatic discovery.
    /// </summary>
    public static void LoadLibraryAuto()
    {
        NativeMethods.LoadLibraryAuto();
    }
}
