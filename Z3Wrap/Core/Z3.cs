using Spaceorc.Z3Wrap.Core.Interop;

namespace Spaceorc.Z3Wrap.Core;

public static class Z3
{
    public static void LoadLibrary(string libraryPath)
    {
        NativeMethods.LoadLibrary(libraryPath);
    }

    public static void LoadLibraryAuto()
    {
        NativeMethods.LoadLibraryAuto();
    }
}
