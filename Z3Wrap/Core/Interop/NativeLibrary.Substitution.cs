// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Substitution API - P/Invoke bindings (MOVED TO NativeLibrary.Modifiers.cs)
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file is now EMPTY - all substitution functions have been moved to NativeLibrary.Modifiers.cs
// per c_headers/z3_api_modifiers.txt (the ULTIMATE SOURCE OF TRUTH).
//
// Previously contained (now in Modifiers.cs):
// - Z3_substitute: General subexpression replacement
// - Z3_substitute_vars: Bound variable (de Bruijn index) replacement
// - Z3_substitute_funs: Function declaration replacement with expressions
//
// This file can be safely deleted after verifying all references are removed.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsSubstitution(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // All functions moved to NativeLibrary.Modifiers.cs
    }
}
