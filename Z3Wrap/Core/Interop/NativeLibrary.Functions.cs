using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Z3 native library P/Invoke wrapper - partial class for specific API functions.
/// </summary>
internal sealed partial class NativeLibrary
{
    /// <summary>
    /// Load function pointers for this group of Z3 API functions.
    /// </summary>
    private static void LoadFunctionsFunctions(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_func_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_app");
        LoadFunctionOrNull(handle, functionPointers, "Z3_ast_to_string");
    }

    // Delegates
    private delegate IntPtr MkFuncDeclDelegate(
        IntPtr ctx,
        IntPtr symbol,
        uint domainSize,
        IntPtr[] domain,
        IntPtr range
    );
    private delegate IntPtr MkAppDelegate(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args);
    private delegate IntPtr AstToStringDelegate(IntPtr ctx, IntPtr ast);

    // Methods
    /// <summary>
    /// Creates a function declaration with specified domain and range sorts.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The function name symbol.</param>
    /// <param name="domainSize">The number of argument sorts.</param>
    /// <param name="domain">Array of argument sorts.</param>
    /// <param name="range">The return sort of the function.</param>
    /// <returns>Handle to the created function declaration.</returns>
    /// <remarks>
    /// Function declarations define the signature of uninterpreted functions.
    /// Used with MkApp to create function application expressions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkFuncDecl(IntPtr ctx, IntPtr symbol, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<MkFuncDeclDelegate>(funcPtr);
        return func(ctx, symbol, domainSize, domain, range);
    }

    /// <summary>
    /// Creates a function application expression by applying arguments to a function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcDecl">The function declaration to apply.</param>
    /// <param name="numArgs">The number of arguments.</param>
    /// <param name="args">Array of argument expressions.</param>
    /// <returns>Handle to the created function application expression.</returns>
    /// <remarks>
    /// Creates f(args) where f is the function declaration. Arguments must match
    /// the function's domain sorts in number and type.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_app");
        var func = Marshal.GetDelegateForFunctionPointer<MkAppDelegate>(funcPtr);
        return func(ctx, funcDecl, numArgs, args);
    }

    /// <summary>
    /// Converts a Z3 AST node to its string representation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node handle to convert.</param>
    /// <returns>Handle to a null-terminated string representation of the AST.</returns>
    /// <remarks>
    /// Provides a human-readable representation of expressions, formulas, and other AST nodes.
    /// The string is managed by Z3 and valid until the context is deleted.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AstToString(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_ast_to_string");
        var func = Marshal.GetDelegateForFunctionPointer<AstToStringDelegate>(funcPtr);
        return func(ctx, ast);
    }
}
