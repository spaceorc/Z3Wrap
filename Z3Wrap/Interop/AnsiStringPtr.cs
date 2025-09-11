using System.Runtime.InteropServices;

namespace Z3Wrap.Interop;

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