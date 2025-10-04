// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 Predicates API - P/Invoke bindings for Z3 type checking predicates
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's type checking predicates API (18 functions):
// - AST comparison predicates (equality checks for AST nodes, sorts, function declarations)
// - AST type predicates (app, numeral, algebraic number, string, as-array, lambda, ground)
// - Sort type predicates (string, sequence, regular expression, character, recursive datatype)
// - Quantifier predicates (forall, exists)
// - Well-sortedness checking

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsPredicates(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_eq_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_eq_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_eq_func_decl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_well_sorted");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_app");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_numeral_ast");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_algebraic_number");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_string_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_seq_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_re_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_char_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_as_array");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_lambda");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_quantifier_forall");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_quantifier_exists");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_ground");
        LoadFunctionOrNull(handle, functionPointers, "Z3_is_recursive_datatype_sort");
    }

    // Delegates
    private delegate bool IsEqAstDelegate(IntPtr ctx, IntPtr t1, IntPtr t2);
    private delegate bool IsEqSortDelegate(IntPtr ctx, IntPtr s1, IntPtr s2);
    private delegate bool IsEqFuncDeclDelegate(IntPtr ctx, IntPtr f1, IntPtr f2);
    private delegate bool IsWellSortedDelegate(IntPtr ctx, IntPtr t);
    private delegate bool IsAppDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsNumeralAstDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsAlgebraicNumberDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsStringDelegate(IntPtr ctx, IntPtr s);
    private delegate bool IsStringSortDelegate(IntPtr ctx, IntPtr s);
    private delegate bool IsSeqSortDelegate(IntPtr ctx, IntPtr s);
    private delegate bool IsReSortDelegate(IntPtr ctx, IntPtr s);
    private delegate bool IsCharSortDelegate(IntPtr ctx, IntPtr s);
    private delegate bool IsAsArrayDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsLambdaDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsQuantifierForallDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsQuantifierExistsDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsGroundDelegate(IntPtr ctx, IntPtr a);
    private delegate bool IsRecursiveDatatypeSortDelegate(IntPtr ctx, IntPtr s);

    // Methods

    /// <summary>
    /// Checks if two AST nodes are structurally equal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t1">First AST node.</param>
    /// <param name="t2">Second AST node.</param>
    /// <returns>True if the AST nodes are equal, false otherwise.</returns>
    /// <remarks>
    /// Compares AST structure, not semantic equivalence. Two expressions
    /// that are logically equivalent may not be structurally equal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsEqAst(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var funcPtr = GetFunctionPointer("Z3_is_eq_ast");
        var func = Marshal.GetDelegateForFunctionPointer<IsEqAstDelegate>(funcPtr);
        return func(ctx, t1, t2);
    }

    /// <summary>
    /// Checks if two sorts are equal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s1">First sort.</param>
    /// <param name="s2">Second sort.</param>
    /// <returns>True if the sorts are equal, false otherwise.</returns>
    /// <remarks>
    /// Sort equality is structural - two sorts with the same definition are equal.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsEqSort(IntPtr ctx, IntPtr s1, IntPtr s2)
    {
        var funcPtr = GetFunctionPointer("Z3_is_eq_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsEqSortDelegate>(funcPtr);
        return func(ctx, s1, s2);
    }

    /// <summary>
    /// Checks if two function declarations are equal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f1">First function declaration.</param>
    /// <param name="f2">Second function declaration.</param>
    /// <returns>True if the function declarations are equal, false otherwise.</returns>
    /// <remarks>
    /// Compares function declarations by their definitions, not by reference.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsEqFuncDecl(IntPtr ctx, IntPtr f1, IntPtr f2)
    {
        var funcPtr = GetFunctionPointer("Z3_is_eq_func_decl");
        var func = Marshal.GetDelegateForFunctionPointer<IsEqFuncDeclDelegate>(funcPtr);
        return func(ctx, f1, f2);
    }

    /// <summary>
    /// Checks if an AST node is well-sorted.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="t">The AST node to check.</param>
    /// <returns>True if the AST is well-sorted, false otherwise.</returns>
    /// <remarks>
    /// Well-sorted means all type constraints are satisfied. This is useful
    /// for debugging and validation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsWellSorted(IntPtr ctx, IntPtr t)
    {
        var funcPtr = GetFunctionPointer("Z3_is_well_sorted");
        var func = Marshal.GetDelegateForFunctionPointer<IsWellSortedDelegate>(funcPtr);
        return func(ctx, t);
    }

    /// <summary>
    /// Checks if an AST node is an application.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is an application, false otherwise.</returns>
    /// <remarks>
    /// Applications are function applications, including constants (nullary functions).
    /// Most expressions are applications.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsApp(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_app");
        var func = Marshal.GetDelegateForFunctionPointer<IsAppDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is a numeral literal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is a numeral, false otherwise.</returns>
    /// <remarks>
    /// Numerals include integers, reals, and bitvector literals.
    /// Use this before attempting to extract numeric values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsNumeralAst(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_numeral_ast");
        var func = Marshal.GetDelegateForFunctionPointer<IsNumeralAstDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is an algebraic number.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is an algebraic number, false otherwise.</returns>
    /// <remarks>
    /// Algebraic numbers are represented in Z3's real closed field (RCF) format.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsAlgebraicNumber(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_algebraic_number");
        var func = Marshal.GetDelegateForFunctionPointer<IsAlgebraicNumberDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is a string literal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The AST node to check.</param>
    /// <returns>True if the AST is a string literal, false otherwise.</returns>
    /// <remarks>
    /// String theory support in Z3 includes string literals, operations, and constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsString(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_string");
        var func = Marshal.GetDelegateForFunctionPointer<IsStringDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if a sort is a string sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The sort to check.</param>
    /// <returns>True if the sort is a string sort, false otherwise.</returns>
    /// <remarks>
    /// String sorts are used in Z3's string theory for text processing constraints.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsStringSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_string_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsStringSortDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if a sort is a sequence sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The sort to check.</param>
    /// <returns>True if the sort is a sequence sort, false otherwise.</returns>
    /// <remarks>
    /// Sequences are generalizations of strings over arbitrary element types.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsSeqSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_seq_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsSeqSortDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if a sort is a regular expression sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The sort to check.</param>
    /// <returns>True if the sort is a regular expression sort, false otherwise.</returns>
    /// <remarks>
    /// Regular expressions in Z3 can be used for string constraints and matching.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsReSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_re_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsReSortDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if a sort is a character sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The sort to check.</param>
    /// <returns>True if the sort is a character sort, false otherwise.</returns>
    /// <remarks>
    /// Character sorts represent Unicode characters in Z3's string theory.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsCharSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_char_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsCharSortDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Checks if an AST node is an as-array expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is an as-array expression, false otherwise.</returns>
    /// <remarks>
    /// As-array expressions represent arrays defined by function interpretations in models.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsAsArray(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_as_array");
        var func = Marshal.GetDelegateForFunctionPointer<IsAsArrayDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is a lambda expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is a lambda expression, false otherwise.</returns>
    /// <remarks>
    /// Lambda expressions define anonymous functions in Z3.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsLambda(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_lambda");
        var func = Marshal.GetDelegateForFunctionPointer<IsLambdaDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is a universal quantifier (forall).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is a universal quantifier, false otherwise.</returns>
    /// <remarks>
    /// Universal quantifiers express that a property holds for all values.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsQuantifierForall(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_quantifier_forall");
        var func = Marshal.GetDelegateForFunctionPointer<IsQuantifierForallDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is an existential quantifier (exists).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is an existential quantifier, false otherwise.</returns>
    /// <remarks>
    /// Existential quantifiers express that a property holds for at least one value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsQuantifierExists(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_quantifier_exists");
        var func = Marshal.GetDelegateForFunctionPointer<IsQuantifierExistsDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if an AST node is ground (contains no variables).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">The AST node to check.</param>
    /// <returns>True if the AST is ground, false otherwise.</returns>
    /// <remarks>
    /// Ground expressions contain no free variables and can be fully evaluated.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsGround(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_is_ground");
        var func = Marshal.GetDelegateForFunctionPointer<IsGroundDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Checks if a sort is a recursive datatype sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">The sort to check.</param>
    /// <returns>True if the sort is a recursive datatype, false otherwise.</returns>
    /// <remarks>
    /// Recursive datatypes allow defining structures like lists and trees.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal bool IsRecursiveDatatypeSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_is_recursive_datatype_sort");
        var func = Marshal.GetDelegateForFunctionPointer<IsRecursiveDatatypeSortDelegate>(funcPtr);
        return func(ctx, s);
    }
}
