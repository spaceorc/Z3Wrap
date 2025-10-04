// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 String Theory API - P/Invoke bindings for Z3 strings, sequences, and regular expressions
//
// Source: z3_api.h from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
//
// This file provides bindings for Z3's string theory API (59 functions):
// - String and character sorts (4 functions)
// - Sort basis accessors (2 functions) - predicates in NativeLibrary.Predicates.cs
// - String literals and character values (4 functions)
// - String value accessors (3 functions) - IsString in NativeLibrary.Predicates.cs
// - Character operations: conversions, comparisons, predicates (5 functions)
// - Sequence operations: concatenation, extraction, indexing, length (13 functions)
// - Sequence higher-order operations: map, fold (4 functions)
// - String-specific operations: lexicographic comparisons (2 functions)
// - String conversions: int, code, bitvector (6 functions)
// - Regular expressions: patterns, matching, combinators, loops (16 functions)
//
// Total String Theory Coverage: 60/60 Z3 C API functions (100%)
// Note: 4 sort predicates + IsString are in NativeLibrary.Predicates.cs
// See COMPARISON_StringTheory.md for detailed API mapping and function categorization

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsStringTheory(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        // Sort constructors
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_string_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_sort");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_sort");

        // Sort accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_seq_sort_basis");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_re_sort_basis");

        // String/char literals
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_lstring");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_u32string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char");

        // String value accessors
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_string");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_lstring");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_string_length");
        LoadFunctionOrNull(handle, functionPointers, "Z3_get_string_contents");

        // Character operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_from_bv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_to_bv");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_to_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_le");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_char_is_digit");

        // Sequence operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_empty");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_unit");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_concat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_prefix");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_suffix");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_contains");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_extract");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_replace");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_at");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_nth");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_length");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_index");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_last_index");

        // Sequence higher-order operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_map");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_mapi");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_foldl");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_foldli");

        // String operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_str_lt");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_str_le");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_str_to_int");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_int_to_str");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_string_to_code");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_string_from_code");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_ubv_to_str");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_sbv_to_str");

        // Regular expression operations
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_to_re");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_seq_in_re");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_plus");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_star");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_option");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_union");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_concat");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_range");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_allchar");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_loop");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_power");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_intersect");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_complement");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_diff");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_empty");
        LoadFunctionOrNull(handle, functionPointers, "Z3_mk_re_full");
    }

    // Sort constructor delegates
    private delegate IntPtr MkStringSortDelegate(IntPtr ctx);
    private delegate IntPtr MkSeqSortDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr MkReSortDelegate(IntPtr ctx, IntPtr seq);
    private delegate IntPtr MkCharSortDelegate(IntPtr ctx);

    // Sort accessor delegates
    private delegate IntPtr GetSeqSortBasisDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr GetReSortBasisDelegate(IntPtr ctx, IntPtr s);

    // String/char literal delegates
    private delegate IntPtr MkStringDelegate(IntPtr ctx, [MarshalAs(UnmanagedType.LPStr)] string s);
    private delegate IntPtr MkLStringDelegate(IntPtr ctx, uint len, [MarshalAs(UnmanagedType.LPStr)] string s);
    private delegate IntPtr MkU32StringDelegate(IntPtr ctx, uint len, uint[] chars);
    private delegate IntPtr MkCharDelegate(IntPtr ctx, uint ch);

    // String value accessor delegates
    [return: MarshalAs(UnmanagedType.LPStr)]
    private delegate string GetStringDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr GetLStringDelegate(IntPtr ctx, IntPtr s, out uint length);
    private delegate uint GetStringLengthDelegate(IntPtr ctx, IntPtr s);
    private delegate void GetStringContentsDelegate(IntPtr ctx, IntPtr s, uint length, uint[] contents);

    // Character operation delegates
    private delegate IntPtr MkCharFromBvDelegate(IntPtr ctx, IntPtr bv);
    private delegate IntPtr MkCharToBvDelegate(IntPtr ctx, IntPtr ch);
    private delegate IntPtr MkCharToIntDelegate(IntPtr ctx, IntPtr ch);
    private delegate IntPtr MkCharLeDelegate(IntPtr ctx, IntPtr ch1, IntPtr ch2);
    private delegate IntPtr MkCharIsDigitDelegate(IntPtr ctx, IntPtr ch);

    // Sequence operation delegates
    private delegate IntPtr MkSeqEmptyDelegate(IntPtr ctx, IntPtr seq);
    private delegate IntPtr MkSeqUnitDelegate(IntPtr ctx, IntPtr a);
    private delegate IntPtr MkSeqConcatDelegate(IntPtr ctx, uint n, IntPtr[] args);
    private delegate IntPtr MkSeqPrefixDelegate(IntPtr ctx, IntPtr prefix, IntPtr s);
    private delegate IntPtr MkSeqSuffixDelegate(IntPtr ctx, IntPtr suffix, IntPtr s);
    private delegate IntPtr MkSeqContainsDelegate(IntPtr ctx, IntPtr container, IntPtr containee);
    private delegate IntPtr MkSeqExtractDelegate(IntPtr ctx, IntPtr s, IntPtr offset, IntPtr length);
    private delegate IntPtr MkSeqReplaceDelegate(IntPtr ctx, IntPtr s, IntPtr src, IntPtr dst);
    private delegate IntPtr MkSeqAtDelegate(IntPtr ctx, IntPtr s, IntPtr index);
    private delegate IntPtr MkSeqNthDelegate(IntPtr ctx, IntPtr s, IntPtr index);
    private delegate IntPtr MkSeqLengthDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr MkSeqIndexDelegate(IntPtr ctx, IntPtr s, IntPtr substr, IntPtr offset);
    private delegate IntPtr MkSeqLastIndexDelegate(IntPtr ctx, IntPtr s, IntPtr substr);

    // Sequence higher-order operation delegates
    private delegate IntPtr MkSeqMapDelegate(IntPtr ctx, IntPtr f, IntPtr s);
    private delegate IntPtr MkSeqMapiDelegate(IntPtr ctx, IntPtr f, IntPtr i, IntPtr s);
    private delegate IntPtr MkSeqFoldlDelegate(IntPtr ctx, IntPtr f, IntPtr a, IntPtr s);
    private delegate IntPtr MkSeqFoldliDelegate(IntPtr ctx, IntPtr f, IntPtr i, IntPtr a, IntPtr s);

    // String operation delegates
    private delegate IntPtr MkStrLtDelegate(IntPtr ctx, IntPtr prefix, IntPtr s);
    private delegate IntPtr MkStrLeDelegate(IntPtr ctx, IntPtr prefix, IntPtr s);
    private delegate IntPtr MkStrToIntDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr MkIntToStrDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr MkStringToCodeDelegate(IntPtr ctx, IntPtr a);
    private delegate IntPtr MkStringFromCodeDelegate(IntPtr ctx, IntPtr a);
    private delegate IntPtr MkUbvToStrDelegate(IntPtr ctx, IntPtr s);
    private delegate IntPtr MkSbvToStrDelegate(IntPtr ctx, IntPtr s);

    // Regular expression delegates
    private delegate IntPtr MkSeqToReDelegate(IntPtr ctx, IntPtr seq);
    private delegate IntPtr MkSeqInReDelegate(IntPtr ctx, IntPtr seq, IntPtr re);
    private delegate IntPtr MkRePlusDelegate(IntPtr ctx, IntPtr re);
    private delegate IntPtr MkReStarDelegate(IntPtr ctx, IntPtr re);
    private delegate IntPtr MkReOptionDelegate(IntPtr ctx, IntPtr re);
    private delegate IntPtr MkReUnionDelegate(IntPtr ctx, uint n, IntPtr[] args);
    private delegate IntPtr MkReConcatDelegate(IntPtr ctx, uint n, IntPtr[] args);
    private delegate IntPtr MkReRangeDelegate(IntPtr ctx, IntPtr lo, IntPtr hi);
    private delegate IntPtr MkReAllcharDelegate(IntPtr ctx, IntPtr regexSort);
    private delegate IntPtr MkReLoopDelegate(IntPtr ctx, IntPtr r, uint lo, uint hi);
    private delegate IntPtr MkRePowerDelegate(IntPtr ctx, IntPtr re, uint n);
    private delegate IntPtr MkReIntersectDelegate(IntPtr ctx, uint n, IntPtr[] args);
    private delegate IntPtr MkReComplementDelegate(IntPtr ctx, IntPtr re);
    private delegate IntPtr MkReDiffDelegate(IntPtr ctx, IntPtr re1, IntPtr re2);
    private delegate IntPtr MkReEmptyDelegate(IntPtr ctx, IntPtr re);
    private delegate IntPtr MkReFullDelegate(IntPtr ctx, IntPtr re);

    // Sort constructor methods
    /// <summary>
    /// Creates string sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Sort handle representing string type.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStringSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkStringSortDelegate>(funcPtr);
        return func(ctx);
    }

    /// <summary>
    /// Creates sequence sort with element type.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Element sort.</param>
    /// <returns>Sort handle representing sequence of elements.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqSort(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqSortDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Creates regular expression sort over sequences.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="seq">Sequence sort.</param>
    /// <returns>Sort handle representing regular expressions over sequences.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReSort(IntPtr ctx, IntPtr seq)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkReSortDelegate>(funcPtr);
        return func(ctx, seq);
    }

    /// <summary>
    /// Creates character sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <returns>Sort handle representing Unicode character type.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharSort(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_sort");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharSortDelegate>(funcPtr);
        return func(ctx);
    }

    // Sort accessor methods
    /// <summary>
    /// Retrieves basis sort for sequence sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence sort handle.</param>
    /// <returns>Element sort of sequence.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetSeqSortBasis(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_get_seq_sort_basis");
        var func = Marshal.GetDelegateForFunctionPointer<GetSeqSortBasisDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Retrieves basis sort for regular expression sort.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Regular expression sort handle.</param>
    /// <returns>Sequence sort for regular expression.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetReSortBasis(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_get_re_sort_basis");
        var func = Marshal.GetDelegateForFunctionPointer<GetReSortBasisDelegate>(funcPtr);
        return func(ctx, s);
    }

    // String/char literal methods
    /// <summary>
    /// Creates string literal from C string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">C string value.</param>
    /// <returns>AST node representing string literal.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkString(IntPtr ctx, string s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string");
        var func = Marshal.GetDelegateForFunctionPointer<MkStringDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Creates string literal from buffer with length.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="len">String length.</param>
    /// <param name="s">String buffer.</param>
    /// <returns>AST node representing string literal.</returns>
    /// <remarks>
    /// Handles strings containing null characters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkLString(IntPtr ctx, uint len, string s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_lstring");
        var func = Marshal.GetDelegateForFunctionPointer<MkLStringDelegate>(funcPtr);
        return func(ctx, len, s);
    }

    /// <summary>
    /// Creates string literal from UTF-32 code points.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="len">Number of characters.</param>
    /// <param name="chars">Array of UTF-32 code points.</param>
    /// <returns>AST node representing string literal.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkU32String(IntPtr ctx, uint len, uint[] chars)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_u32string");
        var func = Marshal.GetDelegateForFunctionPointer<MkU32StringDelegate>(funcPtr);
        return func(ctx, len, chars);
    }

    /// <summary>
    /// Creates character literal.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ch">Character code point.</param>
    /// <returns>AST node representing character literal.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkChar(IntPtr ctx, uint ch)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharDelegate>(funcPtr);
        return func(ctx, ch);
    }

    // String value accessor methods
    /// <summary>
    /// Retrieves string value from string constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">String constant AST node.</param>
    /// <returns>String value with escape encoding for non-ASCII characters.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal string GetString(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_get_string");
        var func = Marshal.GetDelegateForFunctionPointer<GetStringDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Retrieves string value and length from string constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">String constant AST node.</param>
    /// <param name="length">Output parameter for string length.</param>
    /// <returns>Pointer to string buffer.</returns>
    /// <remarks>
    /// Returns raw string buffer along with its length. Unlike GetString, does not escape non-ASCII characters.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr GetLString(IntPtr ctx, IntPtr s, out uint length)
    {
        var funcPtr = GetFunctionPointer("Z3_get_lstring");
        var func = Marshal.GetDelegateForFunctionPointer<GetLStringDelegate>(funcPtr);
        return func(ctx, s, out length);
    }

    /// <summary>
    /// Retrieves unescaped length of string constant.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">String constant AST node.</param>
    /// <returns>Number of characters in unescaped string.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint GetStringLength(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_get_string_length");
        var func = Marshal.GetDelegateForFunctionPointer<GetStringLengthDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Retrieves unescaped string contents as UTF-32 code points.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">String constant AST node.</param>
    /// <param name="length">Number of characters in string.</param>
    /// <param name="contents">Array to receive UTF-32 code points.</param>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void GetStringContents(IntPtr ctx, IntPtr s, uint length, uint[] contents)
    {
        var funcPtr = GetFunctionPointer("Z3_get_string_contents");
        var func = Marshal.GetDelegateForFunctionPointer<GetStringContentsDelegate>(funcPtr);
        func(ctx, s, length, contents);
    }

    // Character operation methods
    /// <summary>
    /// Creates character from bitvector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="bv">Bitvector expression.</param>
    /// <returns>AST node representing character from bitvector value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharFromBv(IntPtr ctx, IntPtr bv)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_from_bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharFromBvDelegate>(funcPtr);
        return func(ctx, bv);
    }

    /// <summary>
    /// Converts character to bitvector.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ch">Character expression.</param>
    /// <returns>AST node representing bitvector encoding of character.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharToBv(IntPtr ctx, IntPtr ch)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_to_bv");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharToBvDelegate>(funcPtr);
        return func(ctx, ch);
    }

    /// <summary>
    /// Converts character to integer code point.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ch">Character expression.</param>
    /// <returns>AST node representing integer code point of character.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharToInt(IntPtr ctx, IntPtr ch)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_to_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharToIntDelegate>(funcPtr);
        return func(ctx, ch);
    }

    /// <summary>
    /// Creates character less-than-or-equal comparison.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ch1">First character expression.</param>
    /// <param name="ch2">Second character expression.</param>
    /// <returns>AST node representing Boolean predicate ch1 &lt;= ch2.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharLe(IntPtr ctx, IntPtr ch1, IntPtr ch2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_le");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharLeDelegate>(funcPtr);
        return func(ctx, ch1, ch2);
    }

    /// <summary>
    /// Creates character digit test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="ch">Character expression.</param>
    /// <returns>AST node representing Boolean predicate testing if character is digit.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkCharIsDigit(IntPtr ctx, IntPtr ch)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_char_is_digit");
        var func = Marshal.GetDelegateForFunctionPointer<MkCharIsDigitDelegate>(funcPtr);
        return func(ctx, ch);
    }

    // Sequence operation methods
    /// <summary>
    /// Creates empty sequence.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="seq">Sequence sort.</param>
    /// <returns>AST node representing empty sequence.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqEmpty(IntPtr ctx, IntPtr seq)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_empty");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqEmptyDelegate>(funcPtr);
        return func(ctx, seq);
    }

    /// <summary>
    /// Creates unit sequence containing single element.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">Element expression.</param>
    /// <returns>AST node representing sequence with single element.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqUnit(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_unit");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqUnitDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Creates sequence concatenation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">Number of sequences.</param>
    /// <param name="args">Array of sequence expressions.</param>
    /// <returns>AST node representing concatenation of sequences.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqConcat(IntPtr ctx, uint n, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_concat");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqConcatDelegate>(funcPtr);
        return func(ctx, n, args);
    }

    /// <summary>
    /// Creates sequence prefix test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="prefix">Prefix sequence expression.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing Boolean predicate testing if prefix is prefix of s.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqPrefix(IntPtr ctx, IntPtr prefix, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_prefix");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqPrefixDelegate>(funcPtr);
        return func(ctx, prefix, s);
    }

    /// <summary>
    /// Creates sequence suffix test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="suffix">Suffix sequence expression.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing Boolean predicate testing if suffix is suffix of s.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqSuffix(IntPtr ctx, IntPtr suffix, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_suffix");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqSuffixDelegate>(funcPtr);
        return func(ctx, suffix, s);
    }

    /// <summary>
    /// Creates sequence containment test.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="container">Container sequence expression.</param>
    /// <param name="containee">Subsequence expression.</param>
    /// <returns>AST node representing Boolean predicate testing if container contains containee.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqContains(IntPtr ctx, IntPtr container, IntPtr containee)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_contains");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqContainsDelegate>(funcPtr);
        return func(ctx, container, containee);
    }

    /// <summary>
    /// Creates sequence extraction (substring).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="offset">Start offset expression.</param>
    /// <param name="length">Length expression.</param>
    /// <returns>AST node representing subsequence from offset with specified length.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqExtract(IntPtr ctx, IntPtr s, IntPtr offset, IntPtr length)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_extract");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqExtractDelegate>(funcPtr);
        return func(ctx, s, offset, length);
    }

    /// <summary>
    /// Creates sequence replacement.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="src">Subsequence to replace.</param>
    /// <param name="dst">Replacement subsequence.</param>
    /// <returns>AST node representing sequence with first occurrence of src replaced by dst.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqReplace(IntPtr ctx, IntPtr s, IntPtr src, IntPtr dst)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_replace");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqReplaceDelegate>(funcPtr);
        return func(ctx, s, src, dst);
    }

    /// <summary>
    /// Creates sequence character access.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="index">Index expression.</param>
    /// <returns>AST node representing unit sequence containing character at index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqAt(IntPtr ctx, IntPtr s, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_at");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqAtDelegate>(funcPtr);
        return func(ctx, s, index);
    }

    /// <summary>
    /// Creates sequence nth element access.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="index">Index expression.</param>
    /// <returns>AST node representing element at index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqNth(IntPtr ctx, IntPtr s, IntPtr index)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_nth");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqNthDelegate>(funcPtr);
        return func(ctx, s, index);
    }

    /// <summary>
    /// Creates sequence length.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing integer length of sequence.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqLength(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_length");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqLengthDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Creates sequence index-of operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="substr">Subsequence to find.</param>
    /// <param name="offset">Start offset expression.</param>
    /// <returns>AST node representing index of first occurrence of substr starting from offset.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqIndex(IntPtr ctx, IntPtr s, IntPtr substr, IntPtr offset)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_index");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqIndexDelegate>(funcPtr);
        return func(ctx, s, substr, offset);
    }

    /// <summary>
    /// Creates sequence last-index-of operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Sequence expression.</param>
    /// <param name="substr">Subsequence to find.</param>
    /// <returns>AST node representing index of last occurrence of substr.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqLastIndex(IntPtr ctx, IntPtr s, IntPtr substr)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_last_index");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqLastIndexDelegate>(funcPtr);
        return func(ctx, s, substr);
    }

    // Sequence higher-order operation methods
    /// <summary>
    /// Creates sequence map operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Function to map over sequence.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing sequence with function applied to each element.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqMap(IntPtr ctx, IntPtr f, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_map");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqMapDelegate>(funcPtr);
        return func(ctx, f, s);
    }

    /// <summary>
    /// Creates sequence map with index operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Function to map over sequence.</param>
    /// <param name="i">Starting index expression.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing sequence with indexed function applied to each element.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqMapi(IntPtr ctx, IntPtr f, IntPtr i, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_mapi");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqMapiDelegate>(funcPtr);
        return func(ctx, f, i, s);
    }

    /// <summary>
    /// Creates sequence left fold operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Fold function.</param>
    /// <param name="a">Initial accumulator value.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing folded result.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqFoldl(IntPtr ctx, IntPtr f, IntPtr a, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_foldl");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqFoldlDelegate>(funcPtr);
        return func(ctx, f, a, s);
    }

    /// <summary>
    /// Creates sequence left fold with index operation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="f">Fold function.</param>
    /// <param name="i">Starting index expression.</param>
    /// <param name="a">Initial accumulator value.</param>
    /// <param name="s">Sequence expression.</param>
    /// <returns>AST node representing folded result with index tracking.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqFoldli(IntPtr ctx, IntPtr f, IntPtr i, IntPtr a, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_foldli");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqFoldliDelegate>(funcPtr);
        return func(ctx, f, i, a, s);
    }

    // String operation methods
    /// <summary>
    /// Creates string less-than comparison.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="prefix">First string expression.</param>
    /// <param name="s">Second string expression.</param>
    /// <returns>AST node representing Boolean predicate prefix &lt; s (lexicographic).</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStrLt(IntPtr ctx, IntPtr prefix, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_str_lt");
        var func = Marshal.GetDelegateForFunctionPointer<MkStrLtDelegate>(funcPtr);
        return func(ctx, prefix, s);
    }

    /// <summary>
    /// Creates string less-than-or-equal comparison.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="prefix">First string expression.</param>
    /// <param name="s">Second string expression.</param>
    /// <returns>AST node representing Boolean predicate prefix &lt;= s (lexicographic).</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStrLe(IntPtr ctx, IntPtr prefix, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_str_le");
        var func = Marshal.GetDelegateForFunctionPointer<MkStrLeDelegate>(funcPtr);
        return func(ctx, prefix, s);
    }

    /// <summary>
    /// Converts string to integer.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">String expression.</param>
    /// <returns>AST node representing integer value of string.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStrToInt(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_str_to_int");
        var func = Marshal.GetDelegateForFunctionPointer<MkStrToIntDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Converts integer to string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Integer expression.</param>
    /// <returns>AST node representing string representation of integer.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkIntToStr(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_int_to_str");
        var func = Marshal.GetDelegateForFunctionPointer<MkIntToStrDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Converts string to code point.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">String expression.</param>
    /// <returns>AST node representing code point of first character or -1 if empty.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStringToCode(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_to_code");
        var func = Marshal.GetDelegateForFunctionPointer<MkStringToCodeDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Converts code point to string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="a">Code point expression.</param>
    /// <returns>AST node representing single-character string from code point.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkStringFromCode(IntPtr ctx, IntPtr a)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_string_from_code");
        var func = Marshal.GetDelegateForFunctionPointer<MkStringFromCodeDelegate>(funcPtr);
        return func(ctx, a);
    }

    /// <summary>
    /// Converts unsigned bitvector to string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Bitvector expression.</param>
    /// <returns>AST node representing string representation of unsigned bitvector value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkUbvToStr(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_ubv_to_str");
        var func = Marshal.GetDelegateForFunctionPointer<MkUbvToStrDelegate>(funcPtr);
        return func(ctx, s);
    }

    /// <summary>
    /// Converts signed bitvector to string.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="s">Bitvector expression.</param>
    /// <returns>AST node representing string representation of signed bitvector value.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSbvToStr(IntPtr ctx, IntPtr s)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_sbv_to_str");
        var func = Marshal.GetDelegateForFunctionPointer<MkSbvToStrDelegate>(funcPtr);
        return func(ctx, s);
    }

    // Regular expression methods
    /// <summary>
    /// Converts sequence to regular expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="seq">Sequence expression.</param>
    /// <returns>AST node representing regex matching exactly the sequence.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqToRe(IntPtr ctx, IntPtr seq)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_to_re");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqToReDelegate>(funcPtr);
        return func(ctx, seq);
    }

    /// <summary>
    /// Creates sequence membership test in regular expression.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="seq">Sequence expression.</param>
    /// <param name="re">Regular expression.</param>
    /// <returns>AST node representing Boolean predicate testing if seq matches re.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkSeqInRe(IntPtr ctx, IntPtr seq, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_seq_in_re");
        var func = Marshal.GetDelegateForFunctionPointer<MkSeqInReDelegate>(funcPtr);
        return func(ctx, seq, re);
    }

    /// <summary>
    /// Creates regex plus (one or more).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regular expression.</param>
    /// <returns>AST node representing regex matching re one or more times.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRePlus(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_plus");
        var func = Marshal.GetDelegateForFunctionPointer<MkRePlusDelegate>(funcPtr);
        return func(ctx, re);
    }

    /// <summary>
    /// Creates regex star (zero or more).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regular expression.</param>
    /// <returns>AST node representing regex matching re zero or more times.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReStar(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_star");
        var func = Marshal.GetDelegateForFunctionPointer<MkReStarDelegate>(funcPtr);
        return func(ctx, re);
    }

    /// <summary>
    /// Creates regex option (zero or one).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regular expression.</param>
    /// <returns>AST node representing regex matching re zero or one time.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReOption(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_option");
        var func = Marshal.GetDelegateForFunctionPointer<MkReOptionDelegate>(funcPtr);
        return func(ctx, re);
    }

    /// <summary>
    /// Creates regex union.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">Number of regex expressions.</param>
    /// <param name="args">Array of regex expressions.</param>
    /// <returns>AST node representing regex matching any of the argument regexes.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReUnion(IntPtr ctx, uint n, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_union");
        var func = Marshal.GetDelegateForFunctionPointer<MkReUnionDelegate>(funcPtr);
        return func(ctx, n, args);
    }

    /// <summary>
    /// Creates regex concatenation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">Number of regex expressions.</param>
    /// <param name="args">Array of regex expressions.</param>
    /// <returns>AST node representing regex matching sequential concatenation of regexes.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReConcat(IntPtr ctx, uint n, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_concat");
        var func = Marshal.GetDelegateForFunctionPointer<MkReConcatDelegate>(funcPtr);
        return func(ctx, n, args);
    }

    /// <summary>
    /// Creates regex character range.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="lo">Lower bound character.</param>
    /// <param name="hi">Upper bound character.</param>
    /// <returns>AST node representing regex matching characters from lo to hi.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReRange(IntPtr ctx, IntPtr lo, IntPtr hi)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_range");
        var func = Marshal.GetDelegateForFunctionPointer<MkReRangeDelegate>(funcPtr);
        return func(ctx, lo, hi);
    }

    /// <summary>
    /// Creates regex matching all characters.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="regexSort">Regular expression sort.</param>
    /// <returns>AST node representing regex matching any single character.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReAllchar(IntPtr ctx, IntPtr regexSort)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_allchar");
        var func = Marshal.GetDelegateForFunctionPointer<MkReAllcharDelegate>(funcPtr);
        return func(ctx, regexSort);
    }

    /// <summary>
    /// Creates regex loop with bounded repetition.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="r">Regular expression to repeat.</param>
    /// <param name="lo">Minimum number of repetitions.</param>
    /// <param name="hi">Maximum number of repetitions (0 for unbounded).</param>
    /// <returns>AST node representing regex repeated between lo and hi times.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReLoop(IntPtr ctx, IntPtr r, uint lo, uint hi)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_loop");
        var func = Marshal.GetDelegateForFunctionPointer<MkReLoopDelegate>(funcPtr);
        return func(ctx, r, lo, hi);
    }

    /// <summary>
    /// Creates regex power (exact repetition).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regular expression to repeat.</param>
    /// <param name="n">Number of repetitions.</param>
    /// <returns>AST node representing regex repeated exactly n times.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkRePower(IntPtr ctx, IntPtr re, uint n)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_power");
        var func = Marshal.GetDelegateForFunctionPointer<MkRePowerDelegate>(funcPtr);
        return func(ctx, re, n);
    }

    /// <summary>
    /// Creates regex intersection.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="n">Number of regex expressions.</param>
    /// <param name="args">Array of regex expressions.</param>
    /// <returns>AST node representing regex matching all argument regexes simultaneously.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReIntersect(IntPtr ctx, uint n, IntPtr[] args)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_intersect");
        var func = Marshal.GetDelegateForFunctionPointer<MkReIntersectDelegate>(funcPtr);
        return func(ctx, n, args);
    }

    /// <summary>
    /// Creates regex complement.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regular expression.</param>
    /// <returns>AST node representing regex matching all sequences not matched by re.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReComplement(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_complement");
        var func = Marshal.GetDelegateForFunctionPointer<MkReComplementDelegate>(funcPtr);
        return func(ctx, re);
    }

    /// <summary>
    /// Creates regex difference.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re1">First regular expression.</param>
    /// <param name="re2">Second regular expression.</param>
    /// <returns>AST node representing regex matching sequences in re1 but not in re2.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReDiff(IntPtr ctx, IntPtr re1, IntPtr re2)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_diff");
        var func = Marshal.GetDelegateForFunctionPointer<MkReDiffDelegate>(funcPtr);
        return func(ctx, re1, re2);
    }

    /// <summary>
    /// Creates empty regex.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regex sort.</param>
    /// <returns>AST node representing regex matching no sequences.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReEmpty(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_empty");
        var func = Marshal.GetDelegateForFunctionPointer<MkReEmptyDelegate>(funcPtr);
        return func(ctx, re);
    }

    /// <summary>
    /// Creates full regex (matches all sequences).
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="re">Regex sort.</param>
    /// <returns>AST node representing regex matching all sequences.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr MkReFull(IntPtr ctx, IntPtr re)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_re_full");
        var func = Marshal.GetDelegateForFunctionPointer<MkReFullDelegate>(funcPtr);
        return func(ctx, re);
    }
}
