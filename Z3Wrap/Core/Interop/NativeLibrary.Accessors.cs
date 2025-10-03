// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Accessors API - P/Invoke bindings for Z3 accessors functions
//
// Source: z3_api.h (Accessors section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Accessors API (0 functions - placeholder):
// - TODO: Add functions from c_headers/z3_api_accessors.txt

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsAccessors(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_get_symbol_kind");
    }


    /// <summary>
    /// Gets kind of symbol.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The symbol handle.</param>
    /// <returns>Symbol kind enumeration value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetSymbolKind(IntPtr ctx, IntPtr symbol)
    {
        var funcPtr = GetFunctionPointer("Z3_get_symbol_kind");
        var func = Marshal.GetDelegateForFunctionPointer<GetSymbolKindDelegate>(funcPtr);
        return func(ctx, symbol);
    }
}
