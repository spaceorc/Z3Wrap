# Z3 Parsing API Comparison Report

## Overview
**NativeLibrary.Parsing.cs**: 10 functions
**Z3 C API (z3_api.h)**: 10 functions

## Complete Function Mapping

### ✅ Functions in Both (10/10 in NativeLibrary match Z3 API - 100%)

| Our Method | Z3 C API | Parameters | Purpose |
|------------|----------|------------|---------|
| `ParseSmtlib2String` | `Z3_parse_smtlib2_string` | `(ctx, str, numSorts, sortNames, sorts, numDecls, declNames, decls)` | Parses SMT-LIB2 formatted string with custom symbol table |
| `ParseSmtlib2File` | `Z3_parse_smtlib2_file` | `(ctx, fileName, numSorts, sortNames, sorts, numDecls, declNames, decls)` | Parses SMT-LIB2 file with custom symbol table |
| `EvalSmtlib2String` | `Z3_eval_smtlib2_string` | `(ctx, str)` | Evaluates and executes SMT-LIB2 commands from string |
| `MkParserContext` | `Z3_mk_parser_context` | `(ctx)` | Creates reusable parser context for incremental parsing |
| `ParserContextIncRef` | `Z3_parser_context_inc_ref` | `(ctx, parserCtx)` | Increments parser context reference count |
| `ParserContextDecRef` | `Z3_parser_context_dec_ref` | `(ctx, parserCtx)` | Decrements parser context reference count |
| `ParserContextAddSort` | `Z3_parser_context_add_sort` | `(ctx, parserCtx, sort)` | Registers sort in parser context symbol table |
| `ParserContextAddDecl` | `Z3_parser_context_add_decl` | `(ctx, parserCtx, decl)` | Registers function declaration in parser context |
| `ParserContextFromString` | `Z3_parser_context_from_string` | `(ctx, parserCtx, str)` | Parses string using existing parser context |
| `BenchmarkToSmtlibString` | `Z3_benchmark_to_smtlib_string` | `(ctx, name, logic, status, attributes, numAssumptions, assumptions, formula)` | Converts Z3 benchmark to SMT-LIB2 string format |

### ❌ Functions in Z3 but NOT in NativeLibrary

**None** - All 10 Z3 SMT-LIB parsing functions are implemented.

### ⚠️ Functions in NativeLibrary but NOT in Z3

**None** - All 10 functions match Z3 C API exactly.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 10 | 100% |
| Functions in NativeLibrary | 10 | **100%** |
| Missing Functions | 0 | 0% |
| Extra Functions | 0 | 0% |

## Function Categories

### SMT-LIB2 Parsing (3 functions)
- `Z3_parse_smtlib2_string` - Parse SMT-LIB2 string with custom symbol table
- `Z3_parse_smtlib2_file` - Parse SMT-LIB2 file with custom symbol table
- `Z3_eval_smtlib2_string` - Parse and execute SMT-LIB2 commands

### Parser Context Management (6 functions)
- `Z3_mk_parser_context` - Create reusable parser context
- `Z3_parser_context_inc_ref` - Reference counting (increment)
- `Z3_parser_context_dec_ref` - Reference counting (decrement)
- `Z3_parser_context_add_sort` - Extend symbol table with sort
- `Z3_parser_context_add_decl` - Extend symbol table with function declaration
- `Z3_parser_context_from_string` - Parse using existing context

### SMT-LIB2 Export (1 function)
- `Z3_benchmark_to_smtlib_string` - Convert Z3 formulas to SMT-LIB2 text

## Completeness Assessment

✅ **COMPLETE** - NativeLibrary.Parsing.cs provides 100% coverage of Z3's SMT-LIB parsing API.

### Strengths
- All 10 parsing functions are implemented
- Comprehensive XML documentation for each function
- Proper delegate signatures matching Z3 C API exactly
- Well-organized into logical categories (parsing, context, export)
- Documentation includes usage guidance for performance optimization

### Quality Notes
- Excellent documentation explaining use cases (file loading, batch processing, incremental parsing)
- Performance notes included (parser context reuse for efficiency)
- Proper use of IntPtr for all Z3 handle types
- Return types correctly mapped (IntPtr for AST handles, void for side effects)
- Array parameters properly handled (IntPtr[] for Z3 arrays)

## API Notes

### SMT-LIB Version Support
These functions support **SMT-LIB2** format, which is the current standard (SMT-LIB 2.6).

**Historical Note**: Z3 previously supported SMT-LIB 1.0 parsing via deprecated functions:
- `Z3_parse_smtlib_string` (deprecated, for SMT-LIB 1.0)
- `Z3_parse_smtlib_file` (deprecated, for SMT-LIB 1.0)

These deprecated functions are **NOT** included in our implementation, as SMT-LIB2 is the modern standard.

### Performance Considerations
From Z3 documentation:
- `Z3_eval_smtlib2_string` re-initializes a parser every call (overhead)
- `Z3_mk_parser_context` + `Z3_parser_context_from_string` is more efficient for repeated parsing
- Parser contexts maintain state across multiple parsing operations

### Use Cases
1. **Loading Benchmarks**: Parse constraint problems from SMT-LIB2 files
2. **Batch Processing**: Execute multiple SMT-LIB2 commands sequentially
3. **Incremental Parsing**: Efficiently parse multiple related formulas
4. **Exporting**: Convert Z3 internal representations to SMT-LIB2 format

## Verification

- **Source**: Z3 C API header `z3_api.h` from [Z3 GitHub repository](https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h)
- **Our implementation**: `Z3Wrap/Core/Interop/NativeLibrary.Parsing.cs`
- **Verification date**: 2025-01-03
- **Z3 version compatibility**: All modern versions (SMT-LIB2 standard)

## Recommendations

**No action required** - This file is complete and well-implemented. The implementation correctly focuses on SMT-LIB2 (current standard) and excludes deprecated SMT-LIB 1.0 functions.

### Future Considerations
None identified. This is a stable, complete API section.
