# Z3 AST Containers API Comparison Report

## Overview

**NativeLibrary.AstCollections.cs**: 21 functions
**Z3 C API (z3_ast_containers.h)**: 21 functions

**Completeness**: 21/21 functions (100%)

## Complete Function Mapping

### ✅ Functions in Both (21/21 in NativeLibrary match Z3 API)

#### AST Vector Operations (10 functions)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| MkAstVector | Z3_mk_ast_vector | Create empty AST vector |
| AstVectorIncRef | Z3_ast_vector_inc_ref | Increment vector reference count |
| AstVectorDecRef | Z3_ast_vector_dec_ref | Decrement vector reference count |
| AstVectorSize | Z3_ast_vector_size | Get number of elements |
| AstVectorGet | Z3_ast_vector_get | Get element at index |
| AstVectorSet | Z3_ast_vector_set | Set element at index |
| AstVectorResize | Z3_ast_vector_resize | Change vector size |
| AstVectorPush | Z3_ast_vector_push | Append element to end |
| AstVectorTranslate | Z3_ast_vector_translate | Translate vector to another context |
| AstVectorToString | Z3_ast_vector_to_string | Get string representation |

#### AST Map Operations (11 functions)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| MkAstMap | Z3_mk_ast_map | Create empty AST map |
| AstMapIncRef | Z3_ast_map_inc_ref | Increment map reference count |
| AstMapDecRef | Z3_ast_map_dec_ref | Decrement map reference count |
| AstMapContains | Z3_ast_map_contains | Check if key exists |
| AstMapFind | Z3_ast_map_find | Get value for key |
| AstMapInsert | Z3_ast_map_insert | Insert or update key-value pair |
| AstMapErase | Z3_ast_map_erase | Remove key-value pair |
| AstMapReset | Z3_ast_map_reset | Remove all entries |
| AstMapSize | Z3_ast_map_size | Get number of entries |
| AstMapKeys | Z3_ast_map_keys | Get all keys as vector |
| AstMapToString | Z3_ast_map_to_string | Get string representation |

### ❌ Functions in Z3 but NOT in NativeLibrary

None - all 21 functions are present.

### ⚠️ Functions in NativeLibrary but NOT in Z3

None - all 21 functions match the Z3 C API.

## Function Categories

### AST Vector (10 functions)
Resizable array container for AST nodes:
- **Creation**: Z3_mk_ast_vector
- **Reference Counting**: inc_ref, dec_ref
- **Access**: size, get, set
- **Modification**: resize, push
- **Context Management**: translate
- **Introspection**: to_string

### AST Map (11 functions)
Hash map container for AST key-value pairs:
- **Creation**: Z3_mk_ast_map
- **Reference Counting**: inc_ref, dec_ref
- **Lookup**: contains, find
- **Modification**: insert, erase, reset
- **Introspection**: size, keys, to_string

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API AST Container Functions | 21 | 100% |
| Functions in NativeLibrary | 21 | 100% |
| Missing Functions | 0 | 0% |
| Extra Functions | 0 | 0% |

## Completeness Assessment

✅ **COMPLETE (100%)**

The NativeLibrary.AstCollections.cs file provides complete coverage of Z3's AST Containers API with all 21 functions present.

### Quality Assessment

- ✅ All function signatures match Z3 C API
- ✅ Comprehensive XML documentation for all methods
- ✅ Proper organization (vectors then maps)
- ✅ Reference counting properly implemented
- ✅ All container operations covered
- ✅ Context translation supported

### Use Cases

**AST Vectors** are used for:
- Collecting multiple assertions
- Building function argument lists
- Storing solver results (unsat cores, consequences)
- Gathering quantifier-bound variables

**AST Maps** are used for:
- Expression substitution tables
- Memoization/caching
- Variable renaming
- Model extraction

## Recommendations

1. ✅ **Complete coverage**: All functions present
2. ✅ **Add source header**: Add standardized header comment referencing z3_ast_containers.h
3. ✅ **Documentation**: Current XML documentation is excellent and comprehensive
4. ✅ **100% coverage achieved**: All AST container functions are present

## Verification

- **Source**: Z3 C API header (z3_ast_containers.h)
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_ast_containers.h
- **Our implementation**: Z3Wrap/Core/Interop/NativeLibrary.AstCollections.cs
- **Verification method**:
  - Extracted all Z3_mk_ast_*, Z3_ast_vector_*, and Z3_ast_map_* functions from z3_ast_containers.h
  - Compared with LoadFunctionsAstCollections method in our implementation
  - Verified delegate signatures match Z3 C API

## Notes

The AST Containers API provides efficient collection types for Z3:
- **Type Safety**: Both containers hold AST nodes (expressions, sorts, function declarations)
- **Reference Counting**: Proper memory management via inc_ref/dec_ref
- **Context Isolation**: Vectors support translation between contexts
- **Performance**: Optimized native implementations for common operations
- **Integration**: Used extensively by other Z3 APIs (solvers, tactics, models)

Our implementation provides complete and correct bindings for this essential infrastructure.
