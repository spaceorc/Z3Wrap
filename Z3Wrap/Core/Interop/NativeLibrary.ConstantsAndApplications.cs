// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 ConstantsAndApplications API - P/Invoke bindings for Z3 constants_and_applications functions
//
// Source: z3_api.h (ConstantsAndApplications section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's ConstantsAndApplications API (0 functions):
// - All 7 functions from c_headers/z3_api_constants_and_applications.txt already exist in other files
//
// Note: Z3_mk_func_decl, Z3_mk_app, Z3_mk_rec_func_decl, Z3_add_rec_def are in NativeLibrary.Functions.cs
// Note: Z3_mk_const is in NativeLibrary.Expressions.cs
// Note: Z3_mk_fresh_func_decl, Z3_mk_fresh_const are in NativeLibrary.SpecialTheories.cs

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsConstantsAndApplications(
        IntPtr handle,
        Dictionary<string, IntPtr> functionPointers
    )
    {
        // All functions from z3_api_constants_and_applications.txt are implemented in other files:
        // - Functions.cs: Z3_mk_func_decl, Z3_mk_app, Z3_mk_rec_func_decl, Z3_add_rec_def
        // - Expressions.cs: Z3_mk_const
        // - SpecialTheories.cs: Z3_mk_fresh_func_decl, Z3_mk_fresh_const
    }
}
