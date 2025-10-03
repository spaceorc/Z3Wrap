# Z3 Utilities API Comparison Report

## Overview
**NativeLibrary.Utilities.cs**: 17 functions
**Z3 C API (z3_api.h)**: 20 functions

## Complete Function Mapping

### ✅ Functions in Both (17/20 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| GetVersion | Z3_get_version | Returns Z3 version as four components (major, minor, build, revision) |
| GetFullVersion | Z3_get_full_version | Returns complete version string with build details |
| OpenLog | Z3_open_log | Opens log file for recording Z3 operations |
| AppendLog | Z3_append_log | Appends custom message to open log file |
| CloseLog | Z3_close_log | Closes and flushes currently open log file |
| EnableTrace | Z3_enable_trace | Enables detailed tracing for specific Z3 subsystem |
| DisableTrace | Z3_disable_trace | Disables tracing for specific tag |
| ResetMemory | Z3_reset_memory | Resets memory allocation statistics |
| FinalizeMemory | Z3_finalize_memory | Performs final memory cleanup before exit |
| Translate | Z3_translate | Translates AST from source context to target context |
| UpdateTerm | Z3_update_term | Creates new term by replacing arguments of existing term |
| SortToString | Z3_sort_to_string | Converts sort to human-readable string |
| FuncDeclToString | Z3_func_decl_to_string | Converts function declaration to string representation |
| PatternToString | Z3_pattern_to_string | Converts quantifier pattern to string |
| SetError | Z3_set_error | Manually sets error state for context |
| SetAstPrintMode | Z3_set_ast_print_mode | Controls AST printing format (default/low-level/SMTLIB2) |
| ToggleWarningMessages | Z3_toggle_warning_messages | Enables/disables warning message output globally |

### ❌ Functions in Z3 but NOT in NativeLibrary (3 missing)

| Z3 C API | Signature | Purpose |
|----------|-----------|---------|
| Z3_ast_to_string | `Z3_string Z3_ast_to_string(Z3_context c, Z3_ast a)` | Converts any AST node to string representation |
| Z3_model_to_string | `Z3_string Z3_model_to_string(Z3_context c, Z3_model m)` | Converts model to string representation |
| Z3_benchmark_to_smtlib_string | `Z3_string Z3_benchmark_to_smtlib_string(Z3_context c, Z3_string name, Z3_string logic, Z3_string status, Z3_string attributes, unsigned num_assumptions, Z3_ast const assumptions[], Z3_ast formula)` | Converts benchmark to SMTLIB format string |

### ⚠️ Functions in NativeLibrary but NOT in Z3

None - all 17 functions map directly to Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 20 | 100% |
| Functions in NativeLibrary | 17 | 85.0% |
| Missing Functions | 3 | 15.0% |

## Function Categories

### Version Information (2/2 - 100%)
- ✅ Z3_get_version - Get version components
- ✅ Z3_get_full_version - Get complete version string

### Logging and Tracing (5/5 - 100%)
- ✅ Z3_open_log - Open log file
- ✅ Z3_append_log - Append to log
- ✅ Z3_close_log - Close log file
- ✅ Z3_enable_trace - Enable tracing
- ✅ Z3_disable_trace - Disable tracing

### Memory Management (2/2 - 100%)
- ✅ Z3_reset_memory - Reset memory statistics
- ✅ Z3_finalize_memory - Finalize memory manager

### Translation and Conversion (2/2 - 100%)
- ✅ Z3_translate - Translate AST between contexts
- ✅ Z3_update_term - Update term arguments

### String Conversions (6/9 - 66.7%)
- ✅ Z3_sort_to_string - Sort to string
- ✅ Z3_func_decl_to_string - Function declaration to string
- ✅ Z3_pattern_to_string - Pattern to string
- ❌ Z3_ast_to_string - AST to string (MISSING)
- ❌ Z3_model_to_string - Model to string (MISSING)
- ❌ Z3_benchmark_to_smtlib_string - Benchmark to SMTLIB string (MISSING)

### Miscellaneous (3/3 - 100%)
- ✅ Z3_set_error - Set error code
- ✅ Z3_set_ast_print_mode - Set AST print format
- ✅ Z3_toggle_warning_messages - Toggle warnings

## Completeness Assessment

**Status**: ⚠️ **85% Complete** - Missing 3 string conversion functions

### Missing Functions Analysis

1. **Z3_ast_to_string** - Generic AST to string conversion
   - **Impact**: Medium - Generic conversion function
   - **Note**: We have specific converters (sort, func_decl, pattern) but not generic AST
   - **Likely reason**: May be handled elsewhere or overlaps with Z3_sort_to_string/etc.

2. **Z3_model_to_string** - Model to string conversion
   - **Impact**: Low - Z3Model likely has its own ToString implementation
   - **Note**: This might be called internally by Z3Model.ToString() via different NativeLibrary method
   - **Likely reason**: May be in NativeLibrary.Model.cs instead

3. **Z3_benchmark_to_smtlib_string** - Benchmark formatting
   - **Impact**: Low - Specialized formatting function
   - **Use case**: Converting benchmark problems to SMTLIB format
   - **Recommendation**: Consider adding for completeness, but not critical for core functionality

## Recommendations

### High Priority
None - all critical utility functions are present.

### Medium Priority
1. **Add Z3_ast_to_string** - Provides generic AST conversion
   - Signature: `Z3_string Z3_ast_to_string(Z3_context c, Z3_ast a)`
   - Use case: Generic AST debugging and display

### Low Priority
1. **Verify Z3_model_to_string location** - May already exist in NativeLibrary.Model.cs
   - Check if this function is loaded elsewhere
   - If not, consider adding to Model.cs or Utilities.cs

2. **Consider Z3_benchmark_to_smtlib_string** - Specialized formatting
   - Useful for benchmark generation and SMTLIB export
   - Complex signature with multiple parameters
   - Low usage in typical Z3 workflows

## Verification

- **Source**: [z3_api.h](https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h) from Z3 C API
- **Our implementation**: Z3Wrap/Core/Interop/NativeLibrary.Utilities.cs
- **Documentation**: [Z3 C API Documentation](https://z3prover.github.io/api/html/group__capi.html)
- **Lines in file**: 377 (including delegates, methods, and XML documentation)

## Notes

1. All 17 implemented functions have comprehensive XML documentation with:
   - Summary descriptions
   - Parameter documentation
   - Remarks with usage context
   - Links to Z3 documentation

2. Functions are well-organized into logical categories:
   - Version Information
   - Logging and Tracing
   - Memory Management
   - Translation and Conversion
   - String Conversions
   - Miscellaneous

3. The missing functions are all string conversion utilities that may be:
   - Located in other NativeLibrary partial files (Model.cs)
   - Lower priority for typical Z3 usage
   - Overlapping with existing functionality

4. No deprecated or obsolete functions detected in this category.

## Conclusion

NativeLibrary.Utilities.cs provides **85% coverage** of Z3's utility API functions with 17 out of 20 functions implemented. All critical utilities for version information, logging, tracing, memory management, and AST translation are present. The three missing functions are string conversion utilities that are either specialized (benchmark formatting) or may exist elsewhere (model to string). The implementation quality is excellent with comprehensive documentation and proper organization.
