# Z3 Functions API Comparison Report

## Overview

**NativeLibrary.Functions.cs**: 3 functions
**Z3 C API (z3_api.h, Functions section)**: 7 functions
**Coverage**: 3/7 = 42.9%

## Complete Function Mapping

### ✅ Functions in Both (3/7 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| `MkFuncDecl` | `Z3_mk_func_decl` | Create function declaration with specified domain and range sorts |
| `MkApp` | `Z3_mk_app` | Create function application by applying arguments to a function declaration |
| `AstToString` | `Z3_ast_to_string` | Convert AST node to string representation |

### ✅ Functions in Other NativeLibrary Files (2/7)

| Z3 C API | Location | Purpose |
|----------|----------|---------|
| `Z3_mk_const` | NativeLibrary.Expressions.cs | Declare and create a constant (shorthand for Z3_mk_func_decl + Z3_mk_app) |
| `Z3_mk_fresh_func_decl` | NativeLibrary.SpecialTheories.cs | Declare a fresh function with a generated unique name |
| `Z3_mk_fresh_const` | NativeLibrary.SpecialTheories.cs | Declare and create a fresh constant with a generated unique name |

### ❌ Functions in Z3 but NOT in NativeLibrary (2/7 missing)

| Z3 C API | Signature | Purpose |
|----------|-----------|---------|
| `Z3_mk_rec_func_decl` | `Z3_func_decl Z3_API Z3_mk_rec_func_decl(Z3_context c, Z3_symbol s, unsigned domain_size, Z3_sort const domain[], Z3_sort range)` | Declare a recursive function (requires Z3_add_rec_def to define its body) |
| `Z3_add_rec_def` | `void Z3_API Z3_add_rec_def(Z3_context c, Z3_func_decl f, unsigned n, Z3_ast args[], Z3_ast body)` | Define the body of a recursive function declared with Z3_mk_rec_func_decl |

### ⚠️ Functions in NativeLibrary but NOT in Z3 (0 extra)

None. All functions in NativeLibrary.Functions.cs correspond to Z3 C API functions.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions (Functions category) | 7 | 100% |
| Functions in NativeLibrary.Functions.cs | 3 | 42.9% |
| Functions in Other NativeLibrary Files | 3 | 42.9% |
| Missing Functions | 2 | 28.6% |
| **Total Coverage** | **5/7** | **71.4%** |

## Function Categories

### Function Declaration Creation (3/4 = 75%)
- ✅ `Z3_mk_func_decl` - Declare a constant or function
- ⚠️ `Z3_mk_fresh_func_decl` - Declare a fresh function (in NativeLibrary.SpecialTheories.cs)
- ❌ `Z3_mk_rec_func_decl` - Declare a recursive function (MISSING)

### Function Application (3/3 = 100%)
- ✅ `Z3_mk_app` - Create function application
- ⚠️ `Z3_mk_const` - Declare and create constant (in NativeLibrary.Expressions.cs)
- ⚠️ `Z3_mk_fresh_const` - Declare and create fresh constant (in NativeLibrary.SpecialTheories.cs)

### Recursive Function Support (0/1 = 0%)
- ❌ `Z3_add_rec_def` - Define recursive function body (MISSING)

### Utility Functions (1/1 = 100%)
- ✅ `Z3_ast_to_string` - Convert AST to string

Note: Z3_ast_to_string was previously listed as MISSING in NativeLibrary.Utilities.cs but is actually implemented in NativeLibrary.Functions.cs.

## Completeness Assessment

**Status**: ⚠️ **71.4% Complete** - Missing recursive function support

### What's Implemented
- ✅ Basic function declaration (Z3_mk_func_decl)
- ✅ Function application (Z3_mk_app)
- ✅ Constant creation (Z3_mk_const in Expressions.cs)
- ✅ Fresh name generation (Z3_mk_fresh_func_decl, Z3_mk_fresh_const in SpecialTheories.cs)
- ✅ AST to string conversion (Z3_ast_to_string)

### What's Missing
- ❌ Recursive function declaration (Z3_mk_rec_func_decl)
- ❌ Recursive function definition (Z3_add_rec_def)

### Recommendations

1. **Add Recursive Function Support**: Implement Z3_mk_rec_func_decl and Z3_add_rec_def to enable recursive function definitions. These are important for defining recursive datatypes and recursive predicates.

2. **Consider File Organization**:
   - Z3_ast_to_string could arguably be moved to NativeLibrary.Utilities.cs (where it was expected)
   - Z3_mk_fresh_func_decl and Z3_mk_fresh_const could be moved from SpecialTheories.cs to Functions.cs for better semantic grouping
   - However, current organization is functional and changing it would be a refactoring decision

3. **Update Utilities.cs Comment**: The comment in NativeLibrary.Utilities.cs incorrectly lists Z3_ast_to_string as MISSING when it's actually in Functions.cs.

### Implementation Quality

- ✅ All 3 functions have correct signatures
- ✅ All functions have comprehensive XML documentation
- ✅ Proper delegate declarations for P/Invoke
- ✅ Consistent error handling via GetFunctionPointer
- ✅ Clear parameter descriptions and remarks

## Detailed Function Analysis

### Z3_mk_func_decl
**Status**: ✅ Implemented
**Purpose**: Creates a function declaration with specified domain and range sorts.
**Usage**: Primary API for declaring uninterpreted functions in Z3.

### Z3_mk_app
**Status**: ✅ Implemented
**Purpose**: Creates a function application expression by applying arguments to a function declaration.
**Usage**: Primary API for creating function application expressions (f(arg1, arg2, ...)).

### Z3_ast_to_string
**Status**: ✅ Implemented
**Purpose**: Converts a Z3 AST node to its string representation.
**Usage**: Debugging and display of Z3 expressions.
**Note**: Listed as MISSING in Utilities.cs but actually implemented here.

### Z3_mk_const (in Expressions.cs)
**Status**: ✅ Implemented (different file)
**Purpose**: Shorthand for declaring and creating a constant (0-arity function).
**Usage**: More convenient than calling Z3_mk_func_decl + Z3_mk_app for constants.

### Z3_mk_fresh_func_decl (in SpecialTheories.cs)
**Status**: ✅ Implemented (different file)
**Purpose**: Declares a fresh function with a generated unique name based on a prefix.
**Usage**: Useful for generating auxiliary functions with guaranteed unique names.

### Z3_mk_fresh_const (in SpecialTheories.cs)
**Status**: ✅ Implemented (different file)
**Purpose**: Declares and creates a fresh constant with a generated unique name.
**Usage**: Shorthand for Z3_mk_fresh_func_decl + Z3_mk_app for constants.

### Z3_mk_rec_func_decl
**Status**: ❌ NOT IMPLEMENTED
**Signature**: `Z3_func_decl Z3_API Z3_mk_rec_func_decl(Z3_context c, Z3_symbol s, unsigned domain_size, Z3_sort const domain[], Z3_sort range)`
**Purpose**: Declares a recursive function (must be followed by Z3_add_rec_def to define its body).
**Usage**: Required for defining recursive functions and recursive datatypes.
**Impact**: Without this, users cannot define recursive predicates or recursive datatype operations.

### Z3_add_rec_def
**Status**: ❌ NOT IMPLEMENTED
**Signature**: `void Z3_API Z3_add_rec_def(Z3_context c, Z3_func_decl f, unsigned n, Z3_ast args[], Z3_ast body)`
**Purpose**: Associates a recursive definition with a recursive function declared via Z3_mk_rec_func_decl.
**Usage**: Defines the body of the recursive function using the provided arguments and body expression.
**Impact**: Without this, Z3_mk_rec_func_decl is useless as the recursive function cannot be defined.

## Summary

**Partial Implementation**: NativeLibrary.Functions.cs provides 42.9% direct coverage (3/7 functions) with an additional 28.6% (2/7 functions) in other files, totaling 71.4% overall coverage of the Z3 function declaration API.

**Current State**:
- Core function declaration and application: ✅ Complete
- Fresh name generation: ✅ Complete (in SpecialTheories.cs)
- Recursive function support: ❌ Missing (2 functions)
- Utility functions: ✅ Complete (Z3_ast_to_string)

**Priority**: Medium - Recursive functions are important for advanced use cases but not required for basic Z3 usage. Many users can work without recursive function support, but it's valuable for:
- Recursive predicates
- Recursive datatype operations
- Fixed-point computations
- Advanced verification scenarios

## Verification

- **Source**: Z3 C API header (z3_api.h)
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
  - Section: Function declarations and applications
- **Our implementation**: Z3Wrap/Core/Interop/NativeLibrary.Functions.cs
- **Related files**:
  - Z3Wrap/Core/Interop/NativeLibrary.Expressions.cs (Z3_mk_const)
  - Z3Wrap/Core/Interop/NativeLibrary.SpecialTheories.cs (Z3_mk_fresh_func_decl, Z3_mk_fresh_const)
- **Verification method**: Direct comparison with Z3 C API documentation and source code
- **Date**: 2025-10-03
