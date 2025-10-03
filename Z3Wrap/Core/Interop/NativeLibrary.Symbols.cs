// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Symbols API - P/Invoke bindings for Z3 symbols functions
//
// Source: z3_api.h (Symbols section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Symbols API (0 functions - placeholder):
// - TODO: Add functions from c_headers/z3_api_symbols.txt

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSymbols(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_string_symbol");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_symbol_int");
    }

    private delegate IntPtr MkStringSymbolDelegate(IntPtr ctx, IntPtr str);

    /// <summary>
    /// Creates a Z3 symbol from a string name.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="str">Pointer to the null-terminated string name.</param>
    /// <returns>Handle to the created Z3 symbol.</returns>
    /// <remarks>
    /// Symbols are used to name constants, functions, and other Z3 objects.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStringSymbol(IntPtr ctx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_symbol");
        var func = Marshal.GetDelegateForFunctionPointer<MkStringSymbolDelegate>(funcPtr);
        return func(ctx, str);
    }


    /// <summary>
    /// Gets integer value from integer symbol.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The integer symbol handle.</param>
    /// <returns>Integer value of the symbol.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetSymbolInt(IntPtr ctx, IntPtr symbol)
    {
        var funcPtr = GetFunctionPointer("Z3_get_symbol_int");
        var func = Marshal.GetDelegateForFunctionPointer<GetSymbolIntDelegate>(funcPtr);
        return func(ctx, symbol);
    }

}
