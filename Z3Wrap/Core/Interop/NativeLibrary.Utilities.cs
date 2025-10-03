// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Utilities API - P/Invoke bindings (MOVED TO NativeLibrary.Modifiers.cs)
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file is now EMPTY - Z3_translate and Z3_update_term have been moved to NativeLibrary.Modifiers.cs
// per c_headers/z3_api_modifiers.txt (the ULTIMATE SOURCE OF TRUTH).
//
// Previously contained (now in Modifiers.cs):
// - Z3_translate (copy AST between contexts)
// - Z3_update_term (replace term arguments)
//
// Note: Other utility functions have been distributed to specialized files:
// - Version info: NativeLibrary.Miscellaneous.cs
// - Logging: NativeLibrary.InteractionLogging.cs
// - String conversion: NativeLibrary.StringConversion.cs
// - Error handling: NativeLibrary.ErrorHandling.cs
//
// This file can be safely deleted after verifying all references are removed.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsUtilities(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // All functions moved to NativeLibrary.Modifiers.cs
    }
}
