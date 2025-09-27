using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed class AnsiStringPtr : SafeHandleZeroOrMinusOneIsInvalid
{
    public AnsiStringPtr(string str)
        : base(true)
    {
        SetHandle(Marshal.StringToHGlobalAnsi(str));
    }

    protected override bool ReleaseHandle()
    {
        Marshal.FreeHGlobal(handle);
        return true;
    }

    public static implicit operator IntPtr(AnsiStringPtr ansiString) => ansiString.DangerousGetHandle();
}
