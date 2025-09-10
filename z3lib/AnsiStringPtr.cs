using System.Runtime.InteropServices;

namespace z3lib;

internal readonly struct AnsiStringPtr(string str) : IDisposable
{
    public IntPtr Pointer { get; } = Marshal.StringToHGlobalAnsi(str);

    public void Dispose()
    {
        if (Pointer != IntPtr.Zero)
            Marshal.FreeHGlobal(Pointer);
    }

    public static implicit operator IntPtr(AnsiStringPtr ansiString) => ansiString.Pointer;
}