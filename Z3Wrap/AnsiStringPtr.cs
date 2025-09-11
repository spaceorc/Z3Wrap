using System.Runtime.InteropServices;

namespace z3lib;

internal readonly struct AnsiStringPtr(string str) : IDisposable
{
    private readonly IntPtr pointer = Marshal.StringToHGlobalAnsi(str);

    public void Dispose()
    {
        if (pointer != IntPtr.Zero)
            Marshal.FreeHGlobal(pointer);
    }

    public static implicit operator IntPtr(AnsiStringPtr ansiString) => ansiString.pointer;
}