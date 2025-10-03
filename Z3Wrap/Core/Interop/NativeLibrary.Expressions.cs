// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Expressions API - P/Invoke bindings for Z3 expression creation and manipulation
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's basic expression API (3 functions):
// - Constant creation (Z3_mk_const)
// - Type conversions (Z3_mk_int2real, Z3_mk_real2int)
//
// Note: Sort creation (bool, int, real) moved to NativeLibrary.Sorts.cs
// Note: Boolean operations, equality, and conditionals moved to NativeLibrary.PropositionalLogicAndEquality.cs
// Note: Z3_mk_int2bv is in NativeLibrary.BitVectors.cs (cross-theory conversion)
// Note: Numeral creation (Z3_mk_numeral) moved to NativeLibrary.Numerals.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsExpressions(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_const");

        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int2real");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_real2int");
    }

    // Delegates
    private delegate IntPtr MkConstDelegate(IntPtr ctx, IntPtr symbol, IntPtr sort);

    // Methods
    /// <summary>
    /// Creates a Z3 constant expression with the specified name and sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The symbol name for the constant.</param>
    /// <param name="sort">The sort (type) of the constant.</param>
    /// <returns>Handle to the created constant expression.</returns>
    /// <remarks>
    /// Constants are free variables that can be assigned values during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_const");
        var func = Marshal.GetDelegateForFunctionPointer<MkConstDelegate>(funcPtr);
        return func(ctx, symbol, sort);
    }
}
