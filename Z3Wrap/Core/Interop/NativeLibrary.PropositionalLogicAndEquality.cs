// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 PropositionalLogicAndEquality API - P/Invoke bindings for Z3 propositional_logic_and_equality functions
//
// Source: z3_api.h (PropositionalLogicAndEquality section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's PropositionalLogicAndEquality API (0 functions - placeholder):
// - TODO: Add functions from c_headers/z3_api_propositional_logic_and_equality.txt

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsPropositionalLogicAndEquality(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_true");
        LoadFunctionInternal(handle, functionPointers, "Z3_mk_false");
    }

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant true.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created true Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkTrue(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_true");
        var func = Marshal.GetDelegateForFunctionPointer<MkTrueDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates a Z3 Boolean expression representing the constant false.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Handle to the created false Boolean expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFalse(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_false");
        var func = Marshal.GetDelegateForFunctionPointer<MkFalseDelegate>(funcPtr);
        return func(ctx);
    }

}
