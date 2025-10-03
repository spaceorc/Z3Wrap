# Z3 Error Handling API Comparison Report

## Overview
**NativeLibrary.ErrorHandling.cs**: 3 functions
**Z3 C API (z3_api.h)**: 4 functions (error handling section)

## Complete Function Mapping

### ✅ Functions in Both (3/4 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| GetErrorCode | Z3_get_error_code | Retrieves error code from last Z3 operation on context |
| GetErrorMsg | Z3_get_error_msg | Retrieves human-readable error message for given error code |
| SetErrorHandler | Z3_set_error_handler | Sets custom error handler callback for Z3 context |

### ❌ Functions in Z3 but NOT in NativeLibrary (1 missing)

| Z3 C API | Signature | Purpose |
|----------|-----------|---------|
| Z3_set_error | `void Z3_API Z3_set_error(Z3_context c, Z3_error_code e)` | Manually set error code on context (typically for internal use) |

**Analysis**: Z3_set_error is primarily used internally by Z3 to set error states. It's rarely needed in client code since Z3 automatically sets error codes when operations fail. Most applications only need to read error codes (Z3_get_error_code) and messages (Z3_get_error_msg).

### ⚠️ Functions in NativeLibrary but NOT in Z3

None - all our functions map directly to Z3 C API functions.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions (Error Handling) | 4 | 100% |
| Functions in NativeLibrary | 3 | 75% |
| Missing Functions | 1 | 25% |

## Function Categories

### Error Code Retrieval (1 function - ✅ Complete)
- ✅ Z3_get_error_code - Get last error code

### Error Messages (1 function - ✅ Complete)
- ✅ Z3_get_error_msg - Get human-readable error message

### Error Handler Management (1 function - ✅ Complete)
- ✅ Z3_set_error_handler - Set custom error callback

### Error Code Manipulation (1 function - ❌ Missing)
- ❌ Z3_set_error - Manually set error code (typically internal use)

## Error Code Enumeration

The Z3_error_code enum includes:
- Z3_OK - No error
- Z3_SORT_ERROR - Sort mismatch error
- Z3_IOB - Index out of bounds
- Z3_INVALID_ARG - Invalid argument
- Z3_PARSER_ERROR - Parser error
- Z3_NO_PARSER - No parser available
- Z3_INVALID_PATTERN - Invalid pattern
- Z3_MEMOUT_FAIL - Memory allocation failure
- Z3_FILE_ACCESS_ERROR - File access error
- Z3_INTERNAL_FATAL - Internal fatal error
- Z3_INVALID_USAGE - Invalid API usage
- Z3_DEC_REF_ERROR - Reference counting error
- Z3_EXCEPTION - General exception

## Completeness Assessment

✅ **FUNCTIONALLY COMPLETE** - All essential error handling functions are present.

The missing Z3_set_error function is primarily used internally by Z3 to set error states. For typical client usage, the three implemented functions provide complete error handling capabilities:
1. GetErrorCode - Check if an error occurred
2. GetErrorMsg - Get detailed error information
3. SetErrorHandler - Customize error handling behavior

**Recommendation**: The current implementation is sufficient for production use. Z3_set_error could be added for completeness, but it's rarely needed in client code. If added, it should be documented as primarily for internal/advanced use cases.

## Usage Pattern

```csharp
// Typical error handling pattern
var errorCode = nativeLibrary.GetErrorCode(ctx);
if (errorCode != Z3ErrorCode.OK)
{
    var errorMsg = nativeLibrary.GetErrorMsg(ctx, errorCode);
    // Handle error...
}

// Custom error handler
nativeLibrary.SetErrorHandler(ctx, (ctx, code) => {
    // Custom error handling logic
});
```

## Verification

- **Source**: Z3 C API (z3_api.h, implemented in api_context.cpp)
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/api_context.cpp
- **Our Implementation**: Z3Wrap/Core/Interop/NativeLibrary.ErrorHandling.cs
- **Documentation**: https://z3prover.github.io/api/html/group__capi.html

## Notes

1. **Error Handler Callback**: The ErrorHandlerDelegate uses UnmanagedFunctionPointer with Cdecl calling convention for proper interop.

2. **Error Code Enum**: The Z3ErrorCode enum is defined elsewhere in the codebase and maps to Z3's error code values.

3. **Thread Safety**: Error codes are context-specific, so multiple threads can use different contexts without error code interference.

4. **Error Propagation**: Z3 automatically sets error codes when operations fail. Client code doesn't need to call Z3_set_error.

5. **Default Behavior**: Without a custom error handler, Z3's default behavior is to print errors and continue execution (non-throwing mode) or throw exceptions (throwing mode, depending on context configuration).
