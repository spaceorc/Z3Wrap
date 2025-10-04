// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Accessors API - P/Invoke bindings for Z3 accessor functions
//
// Source: z3_api.h (Accessors section) from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's Accessors API (35 functions):
// - AST accessors (GetAstKind, GetAstHash, GetAstId)
// - Application accessors (GetAppArg, GetAppDecl, GetAppNumArgs)
// - Declaration accessors (GetDeclName, GetDeclKind, GetDecl* parameters)
// - Domain/range accessors (GetDomain, GetDomainSize, GetRange, GetArity)
// - Quantifier accessors (GetQuantifierNumBound, GetQuantifierBoundName, GetQuantifierBoundSort, GetQuantifierBody, GetQuantifierNumPatterns, GetQuantifierPatternAst)
// - Pattern accessors (GetPatternNumTerms, GetPattern)
// - Numeral accessors (GetNumerator, GetDenominator)
// - Symbol accessors (GetSymbolString, GetSymbolKind)
// - Sort accessor (GetSortName)
//
// Missing Functions (67 functions):
// - Z3_app_to_ast
// - Z3_datatype_update_field
// - Z3_func_decl_to_ast
// - Z3_get_algebraic_number_lower
// - Z3_get_algebraic_number_upper
// - Z3_get_array_arity
// - Z3_get_array_sort_domain
// - Z3_get_array_sort_domain_n
// - Z3_get_array_sort_range
// - Z3_get_bool_value
// - Z3_get_bv_sort_size
// - Z3_get_datatype_sort_constructor
// - Z3_get_datatype_sort_constructor_accessor
// - Z3_get_datatype_sort_num_constructors
// - Z3_get_datatype_sort_recognizer
// - Z3_get_depth
// - Z3_get_finite_domain_sort_size
// - Z3_get_func_decl_id
// - Z3_get_index_value
// - Z3_get_numeral_binary_string
// - Z3_get_numeral_decimal_string
// - Z3_get_numeral_double
// - Z3_get_numeral_int
// - Z3_get_numeral_int64
// - Z3_get_numeral_rational_int64
// - Z3_get_numeral_small
// - Z3_get_numeral_string
// - Z3_get_numeral_uint
// - Z3_get_numeral_uint64
// - Z3_get_quantifier_id
// - Z3_get_quantifier_no_pattern_ast
// - Z3_get_quantifier_num_no_patterns
// - Z3_get_quantifier_skolem_id
// - Z3_get_quantifier_weight
// - Z3_get_relation_arity
// - Z3_get_relation_column
// - Z3_get_sort
// - Z3_get_sort_id
// - Z3_get_sort_kind
// - Z3_get_tuple_sort_field_decl
// - Z3_get_tuple_sort_mk_decl
// - Z3_get_tuple_sort_num_fields
// - Z3_is_algebraic_number
// - Z3_is_app
// - Z3_is_eq_ast
// - Z3_is_eq_func_decl
// - Z3_is_eq_sort
// - Z3_is_ground
// - Z3_is_lambda
// - Z3_is_numeral_ast
// - Z3_is_quantifier_exists
// - Z3_is_quantifier_forall
// - Z3_is_recursive_datatype_sort
// - Z3_is_well_sorted
// - Z3_mk_atleast
// - Z3_mk_atmost
// - Z3_mk_pbeq
// - Z3_mk_pbge
// - Z3_mk_pble
// - Z3_pattern_to_ast
// - Z3_simplify
// - Z3_simplify_ex
// - Z3_simplify_get_help
// - Z3_simplify_get_param_descrs
// - Z3_sort_to_ast
// - Z3_to_app
// - Z3_to_func_decl

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsAccessors(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Type cast functions
        LoadFunctionOrNull(handle, functionPointers, "Z3_app_to_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_decl_to_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_pattern_to_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_sort_to_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_to_app");
        LoadFunctionOrNull(handle, functionPointers, "Z3_to_func_decl");

        // Application accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_app_arg");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_app_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_app_num_args");

        // AST accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_ast_hash");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_ast_id");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_ast_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_depth");

        // Declaration accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_arity");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_num_parameters");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_parameter_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_int_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_double_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_symbol_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_sort_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_ast_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_func_decl_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_decl_rational_parameter");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_func_decl_id");

        // Domain/range accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_domain");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_domain_size");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_range");

        // Sort accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_sort_id");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_sort_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_arity");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_finite_domain_sort_size");

        // Symbol accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_symbol_kind");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_symbol_string");
        LoadFunctionInternal(handle, functionPointers, "Z3_get_symbol_int");

        // Numeral accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_denominator");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_numerator");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_index_value");

        // Quantifier accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_id");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_num_bound");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_bound_name");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_bound_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_body");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_weight");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_skolem_id");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_num_patterns");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_pattern_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_num_no_patterns");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_quantifier_no_pattern_ast");

        // Pattern accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_pattern_num_terms");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_pattern");

        // Datatype accessor
        LoadFunctionOrNull(handle, functionPointers, "Z3_datatype_update_field");
    }

    // Delegates

    // Type cast delegates
    private delegate IntPtr AppToAstDelegate(IntPtr ctx, IntPtr app);
    private delegate IntPtr FuncDeclToAstDelegate(IntPtr ctx, IntPtr decl);
    private delegate IntPtr PatternToAstDelegate(IntPtr ctx, IntPtr pattern);
    private delegate IntPtr SortToAstDelegate(IntPtr ctx, IntPtr sort);
    private delegate IntPtr ToAppDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr ToFuncDeclDelegate(IntPtr ctx, IntPtr ast);

    // Application delegates
    private delegate IntPtr GetAppArgDelegate(IntPtr ctx, IntPtr app, uint idx);
    private delegate IntPtr GetAppDeclDelegate(IntPtr ctx, IntPtr app);
    private delegate uint GetAppNumArgsDelegate(IntPtr ctx, IntPtr app);

    // AST delegates
    private delegate uint GetAstHashDelegate(IntPtr ctx, IntPtr ast);
    private delegate uint GetAstIdDelegate(IntPtr ctx, IntPtr ast);
    private delegate int GetAstKindDelegate(IntPtr ctx, IntPtr ast);
    private delegate uint GetDepthDelegate(IntPtr ctx, IntPtr ast);

    // Declaration delegates
    private delegate uint GetArityDelegate(IntPtr ctx, IntPtr decl);
    private delegate IntPtr GetDeclNameDelegate(IntPtr ctx, IntPtr decl);
    private delegate int GetDeclKindDelegate(IntPtr ctx, IntPtr decl);
    private delegate uint GetDeclNumParametersDelegate(IntPtr ctx, IntPtr decl);
    private delegate uint GetFuncDeclIdDelegate(IntPtr ctx, IntPtr decl);
    private delegate int GetDeclParameterKindDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate int GetDeclIntParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate double GetDeclDoubleParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDeclSymbolParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDeclSortParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDeclAstParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDeclFuncDeclParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDeclRationalParameterDelegate(IntPtr ctx, IntPtr decl, uint idx);
    private delegate IntPtr GetDenominatorDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetNumeratorDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetDomainDelegate(IntPtr ctx, IntPtr decl, uint i);
    private delegate uint GetDomainSizeDelegate(IntPtr ctx, IntPtr decl);
    private delegate IntPtr GetRangeDelegate(IntPtr ctx, IntPtr decl);
    private delegate IntPtr GetSortNameDelegate(IntPtr ctx, IntPtr sort);
    private delegate int GetSymbolKindDelegate(IntPtr ctx, IntPtr symbol);
    private delegate int GetSymbolIntDelegate(IntPtr ctx, IntPtr symbol);
    private delegate IntPtr GetSymbolStringDelegate(IntPtr ctx, IntPtr symbol);
    private delegate uint GetQuantifierNumBoundDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetQuantifierBoundNameDelegate(IntPtr ctx, IntPtr ast, uint i);
    private delegate IntPtr GetQuantifierBoundSortDelegate(IntPtr ctx, IntPtr ast, uint i);
    private delegate IntPtr GetQuantifierBodyDelegate(IntPtr ctx, IntPtr ast);
    private delegate uint GetQuantifierWeightDelegate(IntPtr ctx, IntPtr ast);
    private delegate int GetQuantifierSkolemIdDelegate(IntPtr ctx, IntPtr ast);
    private delegate int GetQuantifierIdDelegate(IntPtr ctx, IntPtr ast);
    private delegate uint GetQuantifierNumPatternsDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetQuantifierPatternAstDelegate(IntPtr ctx, IntPtr ast, uint i);
    private delegate uint GetQuantifierNumNoPatternsDelegate(IntPtr ctx, IntPtr ast);
    private delegate IntPtr GetQuantifierNoPatternAstDelegate(IntPtr ctx, IntPtr ast, uint i);
    private delegate uint GetPatternNumTermsDelegate(IntPtr ctx, IntPtr pattern);
    private delegate IntPtr GetPatternDelegate(IntPtr ctx, IntPtr pattern, uint idx);

    // Sort accessors delegates
    private delegate uint GetSortIdDelegate(IntPtr ctx, IntPtr sort);
    private delegate uint GetArrayArityDelegate(IntPtr ctx, IntPtr sort);
    private delegate ulong GetFiniteDomainSortSizeDelegate(IntPtr ctx, IntPtr sort, out bool success);

    // Numeral accessor delegates
    private delegate ulong GetIndexValueDelegate(IntPtr ctx, IntPtr ast);

    // Datatype accessor delegate
    private delegate IntPtr DatatypeUpdateFieldDelegate(IntPtr ctx, IntPtr func_decl, IntPtr arg, IntPtr value);

    // Methods

    // Type cast methods (IntPtr identity functions in C# - Z3 C API type system helpers)

    /// <summary>
    /// Casts application to AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="app">The application node.</param>
    /// <returns>AST node handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr AppToAst(IntPtr ctx, IntPtr app)
    {
        var funcPtr = GetFunctionPointer("Z3_app_to_ast");
        var func = Marshal.GetDelegateForFunctionPointer<AppToAstDelegate>(funcPtr);
        return func(ctx, app);
    }

    /// <summary>
    /// Casts function declaration to AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration.</param>
    /// <returns>AST node handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncDeclToAst(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_func_decl_to_ast");
        var func = Marshal.GetDelegateForFunctionPointer<FuncDeclToAstDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Casts pattern to AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">The pattern node.</param>
    /// <returns>AST node handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr PatternToAst(IntPtr ctx, IntPtr pattern)
    {
        var funcPtr = GetFunctionPointer("Z3_pattern_to_ast");
        var func = Marshal.GetDelegateForFunctionPointer<PatternToAstDelegate>(funcPtr);
        return func(ctx, pattern);
    }

    /// <summary>
    /// Casts sort to AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The sort node.</param>
    /// <returns>AST node handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr SortToAst(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_sort_to_ast");
        var func = Marshal.GetDelegateForFunctionPointer<SortToAstDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Casts AST node to application.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node.</param>
    /// <returns>Application node handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// Use with IsApp predicate for safety.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ToApp(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_to_app");
        var func = Marshal.GetDelegateForFunctionPointer<ToAppDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Casts AST node to function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node.</param>
    /// <returns>Function declaration handle.</returns>
    /// <remarks>
    /// Identity function in C# (IntPtr-based). Exists for Z3 C API type system compatibility.
    /// Use with appropriate type checking for safety.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr ToFuncDecl(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_to_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<ToFuncDeclDelegate>(funcPtr);
        return func(ctx, ast);
    }

    // Application accessor methods

    /// <summary>
    /// Gets argument at index from application.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="app">The application handle.</param>
    /// <param name="idx">The argument index.</param>
    /// <returns>AST node representing the argument.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetAppArg(IntPtr ctx, IntPtr app, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_app_arg");
        var func = Marshal.GetDelegateForFunctionPointer<GetAppArgDelegate>(funcPtr);
        return func(ctx, app, idx);
    }

    /// <summary>
    /// Gets function declaration from application.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="app">The application handle.</param>
    /// <returns>Function declaration of the application.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetAppDecl(IntPtr ctx, IntPtr app)
    {
        var funcPtr = GetFunctionPointer("Z3_get_app_decl");
        var func = Marshal.GetDelegateForFunctionPointer<GetAppDeclDelegate>(funcPtr);
        return func(ctx, app);
    }

    /// <summary>
    /// Gets number of arguments in application.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="app">The application handle.</param>
    /// <returns>Number of arguments.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetAppNumArgs(IntPtr ctx, IntPtr app)
    {
        var funcPtr = GetFunctionPointer("Z3_get_app_num_args");
        var func = Marshal.GetDelegateForFunctionPointer<GetAppNumArgsDelegate>(funcPtr);
        return func(ctx, app);
    }

    /// <summary>
    /// Gets arity of function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Number of parameters (arity).</returns>
    /// <remarks>Alias for GetDomainSize.</remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetArity(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_arity");
        var func = Marshal.GetDelegateForFunctionPointer<GetArityDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets hash code of AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node handle.</param>
    /// <returns>Hash code for the AST node.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetAstHash(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_ast_hash");
        var func = Marshal.GetDelegateForFunctionPointer<GetAstHashDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets unique identifier of AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node handle.</param>
    /// <returns>Unique ID for the AST node.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetAstId(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_ast_id");
        var func = Marshal.GetDelegateForFunctionPointer<GetAstIdDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets kind discriminator of AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node handle.</param>
    /// <returns>AST kind enumeration value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetAstKind(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_ast_kind");
        var func = Marshal.GetDelegateForFunctionPointer<GetAstKindDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets name symbol of function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Symbol representing the declaration name.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclName(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclNameDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets kind of function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Declaration kind enumeration value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetDeclKind(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_kind");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclKindDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets number of parameters in declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Number of parameters.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetDeclNumParameters(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_num_parameters");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclNumParametersDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets kind of declaration parameter at index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Parameter kind enumeration value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetDeclParameterKind(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_parameter_kind");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclParameterKindDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets integer parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Integer parameter value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetDeclIntParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_int_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclIntParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets double parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Double parameter value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal double GetDeclDoubleParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_double_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclDoubleParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets symbol parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Symbol parameter handle.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclSymbolParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_symbol_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclSymbolParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets sort parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Sort parameter handle.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclSortParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_sort_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclSortParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets AST parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>AST parameter handle.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclAstParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_ast_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclAstParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets function declaration parameter value from declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>Function declaration parameter handle.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclFuncDeclParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_func_decl_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclFuncDeclParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets rational parameter value from declaration as string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="idx">The parameter index.</param>
    /// <returns>String representation of rational parameter.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDeclRationalParameter(IntPtr ctx, IntPtr decl, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_decl_rational_parameter");
        var func = Marshal.GetDelegateForFunctionPointer<GetDeclRationalParameterDelegate>(funcPtr);
        return func(ctx, decl, idx);
    }

    /// <summary>
    /// Gets denominator of rational number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The rational number AST node.</param>
    /// <returns>AST node representing the denominator.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDenominator(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_denominator");
        var func = Marshal.GetDelegateForFunctionPointer<GetDenominatorDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets numerator of rational number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The rational number AST node.</param>
    /// <returns>AST node representing the numerator.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetNumerator(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_numerator");
        var func = Marshal.GetDelegateForFunctionPointer<GetNumeratorDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets domain sort at index from function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <param name="i">The domain index.</param>
    /// <returns>Sort of the domain at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetDomain(IntPtr ctx, IntPtr decl, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_domain");
        var func = Marshal.GetDelegateForFunctionPointer<GetDomainDelegate>(funcPtr);
        return func(ctx, decl, i);
    }

    /// <summary>
    /// Gets number of domain parameters in function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Number of domain parameters (arity).</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetDomainSize(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_domain_size");
        var func = Marshal.GetDelegateForFunctionPointer<GetDomainSizeDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets range sort from function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Sort of the function range (return type).</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetRange(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_range");
        var func = Marshal.GetDelegateForFunctionPointer<GetRangeDelegate>(funcPtr);
        return func(ctx, decl);
    }

    /// <summary>
    /// Gets name symbol of sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The sort handle.</param>
    /// <returns>Symbol representing the sort name.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetSortName(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetSortNameDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets string value from string symbol.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="symbol">The string symbol handle.</param>
    /// <returns>String value of the symbol.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetSymbolString(IntPtr ctx, IntPtr symbol)
    {
        var funcPtr = GetFunctionPointer("Z3_get_symbol_string");
        var func = Marshal.GetDelegateForFunctionPointer<GetSymbolStringDelegate>(funcPtr);
        return func(ctx, symbol);
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

    /// <summary>
    /// Gets number of bound variables in quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Number of bound variables.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetQuantifierNumBound(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_num_bound");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierNumBoundDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets name symbol of bound variable in quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <param name="i">The bound variable index.</param>
    /// <returns>Symbol representing the bound variable name.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetQuantifierBoundName(IntPtr ctx, IntPtr ast, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_bound_name");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierBoundNameDelegate>(funcPtr);
        return func(ctx, ast, i);
    }

    /// <summary>
    /// Gets sort of bound variable in quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <param name="i">The bound variable index.</param>
    /// <returns>Sort of the bound variable.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetQuantifierBoundSort(IntPtr ctx, IntPtr ast, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_bound_sort");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierBoundSortDelegate>(funcPtr);
        return func(ctx, ast, i);
    }

    /// <summary>
    /// Gets body expression of quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>AST node representing the quantifier body.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetQuantifierBody(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_body");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierBodyDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets number of patterns in quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Number of patterns.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetQuantifierNumPatterns(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_num_patterns");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierNumPatternsDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets pattern at index from quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <param name="i">The pattern index.</param>
    /// <returns>Pattern handle at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetQuantifierPatternAst(IntPtr ctx, IntPtr ast, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_pattern_ast");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierPatternAstDelegate>(funcPtr);
        return func(ctx, ast, i);
    }

    /// <summary>
    /// Gets number of terms in pattern.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">The pattern handle.</param>
    /// <returns>Number of terms in the pattern.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetPatternNumTerms(IntPtr ctx, IntPtr pattern)
    {
        var funcPtr = GetFunctionPointer("Z3_get_pattern_num_terms");
        var func = Marshal.GetDelegateForFunctionPointer<GetPatternNumTermsDelegate>(funcPtr);
        return func(ctx, pattern);
    }

    /// <summary>
    /// Gets term at index from pattern.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="pattern">The pattern handle.</param>
    /// <param name="idx">The term index.</param>
    /// <returns>AST node representing the pattern term.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetPattern(IntPtr ctx, IntPtr pattern, uint idx)
    {
        var funcPtr = GetFunctionPointer("Z3_get_pattern");
        var func = Marshal.GetDelegateForFunctionPointer<GetPatternDelegate>(funcPtr);
        return func(ctx, pattern, idx);
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

    // AST depth accessor

    /// <summary>
    /// Gets depth of AST node.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The AST node.</param>
    /// <returns>Depth of the AST node in expression tree.</returns>
    /// <remarks>
    /// Depth is maximum distance from root to leaf. Useful for complexity analysis.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetDepth(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_depth");
        var func = Marshal.GetDelegateForFunctionPointer<GetDepthDelegate>(funcPtr);
        return func(ctx, ast);
    }

    // Sort accessors

    /// <summary>
    /// Gets unique identifier for sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The sort handle.</param>
    /// <returns>Unique identifier for the sort.</returns>
    /// <remarks>
    /// Sort IDs are unique within a context. Useful for sort comparison and caching.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetSortId(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_sort_id");
        var func = Marshal.GetDelegateForFunctionPointer<GetSortIdDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets array arity (number of index sorts).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The array sort handle.</param>
    /// <returns>Number of index sorts for the array.</returns>
    /// <remarks>
    /// For multi-dimensional arrays, returns the number of dimensions.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetArrayArity(IntPtr ctx, IntPtr sort)
    {
        var funcPtr = GetFunctionPointer("Z3_get_array_arity");
        var func = Marshal.GetDelegateForFunctionPointer<GetArrayArityDelegate>(funcPtr);
        return func(ctx, sort);
    }

    /// <summary>
    /// Gets size of finite domain sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="sort">The finite domain sort handle.</param>
    /// <param name="success">Output parameter indicating if sort is finite domain.</param>
    /// <returns>Size of the finite domain if success is true.</returns>
    /// <remarks>
    /// Returns size of finite domain sort. Sets success to false if sort is not finite domain.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal ulong GetFiniteDomainSortSize(IntPtr ctx, IntPtr sort, out bool success)
    {
        var funcPtr = GetFunctionPointer("Z3_get_finite_domain_sort_size");
        var func = Marshal.GetDelegateForFunctionPointer<GetFiniteDomainSortSizeDelegate>(funcPtr);
        return func(ctx, sort, out success);
    }

    // Function declaration accessor

    /// <summary>
    /// Gets unique identifier for function declaration.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="decl">The function declaration handle.</param>
    /// <returns>Unique identifier for the function declaration.</returns>
    /// <remarks>
    /// Function declaration IDs are unique within a context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetFuncDeclId(IntPtr ctx, IntPtr decl)
    {
        var funcPtr = GetFunctionPointer("Z3_get_func_decl_id");
        var func = Marshal.GetDelegateForFunctionPointer<GetFuncDeclIdDelegate>(funcPtr);
        return func(ctx, decl);
    }

    // Numeral accessor

    /// <summary>
    /// Gets integer value from de Bruijn index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The de Bruijn index AST node.</param>
    /// <returns>Integer value of the de Bruijn index.</returns>
    /// <remarks>
    /// Retrieves index value from de Bruijn indexed variables used in quantifiers.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal ulong GetIndexValue(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_index_value");
        var func = Marshal.GetDelegateForFunctionPointer<GetIndexValueDelegate>(funcPtr);
        return func(ctx, ast);
    }

    // Quantifier accessors

    /// <summary>
    /// Gets unique identifier for quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Unique identifier for the quantifier.</returns>
    /// <remarks>
    /// Quantifier IDs are unique within a context.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetQuantifierId(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_id");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierIdDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets weight of quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Weight assigned to the quantifier.</returns>
    /// <remarks>
    /// Quantifier weight affects instantiation priority during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetQuantifierWeight(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_weight");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierWeightDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets skolem identifier for quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Skolem identifier for the quantifier.</returns>
    /// <remarks>
    /// Used in quantifier skolemization during solving.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal int GetQuantifierSkolemId(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_skolem_id");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierSkolemIdDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets number of no-patterns in quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <returns>Number of no-patterns attached to quantifier.</returns>
    /// <remarks>
    /// No-patterns guide solver to avoid certain instantiations.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetQuantifierNumNoPatterns(IntPtr ctx, IntPtr ast)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_num_no_patterns");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierNumNoPatternsDelegate>(funcPtr);
        return func(ctx, ast);
    }

    /// <summary>
    /// Gets no-pattern at index from quantifier.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ast">The quantifier AST node.</param>
    /// <param name="i">The no-pattern index.</param>
    /// <returns>No-pattern AST node at specified index.</returns>
    /// <remarks>
    /// Retrieves specific no-pattern used to restrict instantiation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetQuantifierNoPatternAst(IntPtr ctx, IntPtr ast, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_get_quantifier_no_pattern_ast");
        var func = Marshal.GetDelegateForFunctionPointer<GetQuantifierNoPatternAstDelegate>(funcPtr);
        return func(ctx, ast, i);
    }

    // Datatype accessor

    /// <summary>
    /// Updates field value in datatype term.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="func_decl">The accessor function declaration for the field.</param>
    /// <param name="arg">The datatype term to update.</param>
    /// <param name="value">The new value for the field.</param>
    /// <returns>New datatype term with updated field value.</returns>
    /// <remarks>
    /// Creates functional update - returns new term rather than modifying original.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr DatatypeUpdateField(IntPtr ctx, IntPtr func_decl, IntPtr arg, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_datatype_update_field");
        var func = Marshal.GetDelegateForFunctionPointer<DatatypeUpdateFieldDelegate>(funcPtr);
        return func(ctx, func_decl, arg, value);
    }
}
