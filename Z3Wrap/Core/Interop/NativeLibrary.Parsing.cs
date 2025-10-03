// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Parsing API - P/Invoke bindings for Z3 SMT-LIB parsing and export operations
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's SMT-LIB parsing API (10 functions):
// - SMT-LIB2 string and file parsing (parse_smtlib2_string, parse_smtlib2_file)
// - SMT-LIB2 command evaluation (eval_smtlib2_string)
// - Reusable parser context management for incremental parsing
// - SMT-LIB2 export and serialization (benchmark_to_smtlib_string)
//
// Performance Note: Parser contexts (mk_parser_context + parser_context_from_string)
// are more efficient than eval_smtlib2_string for repeated parsing operations.

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsParsing(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_parse_smtlib2_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parse_smtlib2_file");
        LoadFunctionOrNull(handle, functionPointers, "Z3_eval_smtlib2_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_parser_context");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parser_context_inc_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parser_context_dec_ref");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parser_context_add_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parser_context_add_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_parser_context_from_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_benchmark_to_smtlib_string");
    }

    // Delegates

    private delegate IntPtr ParseSmtlib2StringDelegate(
        IntPtr ctx,
        IntPtr str,
        uint numSorts,
        IntPtr[] sortNames,
        IntPtr[] sorts,
        uint numDecls,
        IntPtr[] declNames,
        IntPtr[] decls
    );
    private delegate IntPtr ParseSmtlib2FileDelegate(
        IntPtr ctx,
        IntPtr fileName,
        uint numSorts,
        IntPtr[] sortNames,
        IntPtr[] sorts,
        uint numDecls,
        IntPtr[] declNames,
        IntPtr[] decls
    );
    private delegate IntPtr EvalSmtlib2StringDelegate(IntPtr ctx, IntPtr str);
    private delegate IntPtr MkParserContextDelegate(IntPtr ctx);
    private delegate void ParserContextIncRefDelegate(IntPtr ctx, IntPtr parserCtx);
    private delegate void ParserContextDecRefDelegate(IntPtr ctx, IntPtr parserCtx);
    private delegate void ParserContextAddSortDelegate(IntPtr ctx, IntPtr parserCtx, IntPtr sort);
    private delegate void ParserContextAddDeclDelegate(IntPtr ctx, IntPtr parserCtx, IntPtr decl);
    private delegate IntPtr ParserContextFromStringDelegate(IntPtr ctx, IntPtr parserCtx, IntPtr str);
    private delegate IntPtr BenchmarkToSmtlibStringDelegate(
        IntPtr ctx,
        IntPtr name,
        IntPtr logic,
        IntPtr status,
        IntPtr attributes,
        uint numAssumptions,
        IntPtr[] assumptions,
        IntPtr formula
    );

    // Methods

    /// <summary>
    /// Parses SMTLIB2 formatted string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="str">SMTLIB2 string to parse.</param>
    /// <param name="numSorts">Number of sorts for symbol table.</param>
    /// <param name="sortNames">Array of sort names.</param>
    /// <param name="sorts">Array of sort handles.</param>
    /// <param name="numDecls">Number of declarations for symbol table.</param>
    /// <param name="declNames">Array of declaration names.</param>
    /// <param name="decls">Array of declaration handles.</param>
    /// <returns>AST formula from parsed string.</returns>
    /// <remarks>
    /// Parses SMTLIB2 format string and returns AST. Additional sorts and declarations
    /// can be provided to extend the default symbol table. Used for loading constraint
    /// problems and formulas from text format.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParseSmtlib2String(
        IntPtr ctx,
        IntPtr str,
        uint numSorts,
        IntPtr[] sortNames,
        IntPtr[] sorts,
        uint numDecls,
        IntPtr[] declNames,
        IntPtr[] decls
    )
    {
        var funcPtr = GetFunctionPointer("Z3_parse_smtlib2_string");
        var func = Marshal.GetDelegateForFunctionPointer<ParseSmtlib2StringDelegate>(funcPtr);
        return func(ctx, str, numSorts, sortNames, sorts, numDecls, declNames, decls);
    }

    /// <summary>
    /// Parses SMTLIB2 file.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="fileName">Path to SMTLIB2 file.</param>
    /// <param name="numSorts">Number of sorts for symbol table.</param>
    /// <param name="sortNames">Array of sort names.</param>
    /// <param name="sorts">Array of sort handles.</param>
    /// <param name="numDecls">Number of declarations for symbol table.</param>
    /// <param name="declNames">Array of declaration names.</param>
    /// <param name="decls">Array of declaration handles.</param>
    /// <returns>AST formula from parsed file.</returns>
    /// <remarks>
    /// Parses SMTLIB2 file and returns AST. Additional sorts and declarations
    /// can be provided to extend the default symbol table. Used for loading
    /// benchmark problems from files.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParseSmtlib2File(
        IntPtr ctx,
        IntPtr fileName,
        uint numSorts,
        IntPtr[] sortNames,
        IntPtr[] sorts,
        uint numDecls,
        IntPtr[] declNames,
        IntPtr[] decls
    )
    {
        var funcPtr = GetFunctionPointer("Z3_parse_smtlib2_file");
        var func = Marshal.GetDelegateForFunctionPointer<ParseSmtlib2FileDelegate>(funcPtr);
        return func(ctx, fileName, numSorts, sortNames, sorts, numDecls, declNames, decls);
    }

    /// <summary>
    /// Evaluates SMTLIB2 commands from string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="str">SMTLIB2 commands to evaluate.</param>
    /// <returns>Result AST from evaluation.</returns>
    /// <remarks>
    /// Executes SMTLIB2 commands (like assert, check-sat) from string and returns
    /// result. Used for batch processing of SMTLIB2 scripts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr EvalSmtlib2String(IntPtr ctx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_eval_smtlib2_string");
        var func = Marshal.GetDelegateForFunctionPointer<EvalSmtlib2StringDelegate>(funcPtr);
        return func(ctx, str);
    }

    /// <summary>
    /// Creates parser context for incremental parsing.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Parser context handle.</returns>
    /// <remarks>
    /// Creates reusable parser context that maintains symbol table across multiple
    /// parsing operations. Useful for parsing multiple related formulas efficiently.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkParserContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_parser_context");
        var func = Marshal.GetDelegateForFunctionPointer<MkParserContextDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Increments reference count for parser context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="parserCtx">The parser context handle.</param>
    /// <remarks>
    /// Increments reference count to prevent premature deletion. Must be balanced
    /// with ParserContextDecRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParserContextIncRef(IntPtr ctx, IntPtr parserCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_parser_context_inc_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParserContextIncRefDelegate>(funcPtr);
        func(ctx, parserCtx);
    }

    /// <summary>
    /// Decrements reference count for parser context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="parserCtx">The parser context handle.</param>
    /// <remarks>
    /// Decrements reference count. When count reaches zero, parser context is deleted.
    /// Must be balanced with ParserContextIncRef call.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParserContextDecRef(IntPtr ctx, IntPtr parserCtx)
    {
        var funcPtr = GetFunctionPointer("Z3_parser_context_dec_ref");
        var func = Marshal.GetDelegateForFunctionPointer<ParserContextDecRefDelegate>(funcPtr);
        func(ctx, parserCtx);
    }

    /// <summary>
    /// Adds sort to parser context symbol table.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="parserCtx">The parser context handle.</param>
    /// <param name="sort">The sort to add.</param>
    /// <remarks>
    /// Registers sort in parser's symbol table for subsequent parsing operations.
    /// Used when parsing formulas that reference custom sorts.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParserContextAddSort(IntPtr ctx, IntPtr parserCtx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_parser_context_add_sort");
        var func = Marshal.GetDelegateForFunctionPointer<ParserContextAddSortDelegate>(funcPtr);
        func(ctx, parserCtx, sort);
    }

    /// <summary>
    /// Adds function declaration to parser context symbol table.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="parserCtx">The parser context handle.</param>
    /// <param name="decl">The function declaration to add.</param>
    /// <remarks>
    /// Registers function declaration in parser's symbol table for subsequent parsing
    /// operations. Used when parsing formulas that reference custom functions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void ParserContextAddDecl(IntPtr ctx, IntPtr parserCtx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_parser_context_add_decl");
        var func = Marshal.GetDelegateForFunctionPointer<ParserContextAddDeclDelegate>(funcPtr);
        func(ctx, parserCtx, decl);
    }

    /// <summary>
    /// Parses string using parser context.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="parserCtx">The parser context handle.</param>
    /// <param name="str">String to parse.</param>
    /// <returns>Parsed AST formula.</returns>
    /// <remarks>
    /// Parses string using existing parser context with its symbol table.
    /// More efficient than ParseSmtlib2String for multiple related parsing operations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ParserContextFromString(IntPtr ctx, IntPtr parserCtx, IntPtr str)
    {
        var funcPtr = GetFunctionPointer("Z3_parser_context_from_string");
        var func = Marshal.GetDelegateForFunctionPointer<ParserContextFromStringDelegate>(funcPtr);
        return func(ctx, parserCtx, str);
    }

    /// <summary>
    /// Converts benchmark to SMTLIB2 string format.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="name">Benchmark name.</param>
    /// <param name="logic">Logic name (e.g., QF_UF, QF_LIA).</param>
    /// <param name="status">Expected status string (sat, unsat, unknown).</param>
    /// <param name="attributes">Additional attributes string.</param>
    /// <param name="numAssumptions">Number of assumption formulas.</param>
    /// <param name="assumptions">Array of assumption ASTs.</param>
    /// <param name="formula">Main formula AST.</param>
    /// <returns>SMTLIB2 formatted string.</returns>
    /// <remarks>
    /// Converts Z3 AST formulas to SMTLIB2 text format for export or sharing.
    /// Produces complete SMTLIB2 file with metadata and formulas.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr BenchmarkToSmtlibString(
        IntPtr ctx,
        IntPtr name,
        IntPtr logic,
        IntPtr status,
        IntPtr attributes,
        uint numAssumptions,
        IntPtr[] assumptions,
        IntPtr formula
    )
    {
        var funcPtr = GetFunctionPointer("Z3_benchmark_to_smtlib_string");
        var func = Marshal.GetDelegateForFunctionPointer<BenchmarkToSmtlibStringDelegate>(funcPtr);
        return func(ctx, name, logic, status, attributes, numAssumptions, assumptions, formula);
    }
}
