# Z3 Function Interpretations API Comparison Report

## Overview

**NativeLibrary.FunctionInterpretations.cs**: 9 functions
**Z3 C API (z3_api.h, Function Interpretation section)**: 9 functions
**Reference counting**: 4 functions in NativeLibrary.ReferenceCountingExtra.cs
**Coverage**: 9/9 = 100%

## Complete Function Mapping

### ✅ Functions in Both (9/9 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| `FuncInterpGetNumEntries` | `Z3_func_interp_get_num_entries` | Get number of entries in function interpretation's finite map |
| `FuncInterpGetEntry` | `Z3_func_interp_get_entry` | Get entry at specific index in function interpretation |
| `FuncInterpGetElse` | `Z3_func_interp_get_else` | Get 'else' value (default) of function interpretation |
| `FuncInterpSetElse` | `Z3_func_interp_set_else` | Set 'else' value (default) of function interpretation |
| `FuncInterpGetArity` | `Z3_func_interp_get_arity` | Get arity (number of arguments) of function interpretation |
| `FuncInterpAddEntry` | `Z3_func_interp_add_entry` | Add entry to function interpretation's finite map |
| `FuncEntryGetValue` | `Z3_func_entry_get_value` | Get value (output) of a function entry |
| `FuncEntryGetNumArgs` | `Z3_func_entry_get_num_args` | Get number of arguments in function entry |
| `FuncEntryGetArg` | `Z3_func_entry_get_arg` | Get argument at specific index in function entry |

### ✅ Reference Counting Functions (in NativeLibrary.ReferenceCountingExtra.cs)

| Our Method | Z3 C API | Location | Purpose |
|------------|----------|----------|---------|
| `FuncInterpIncRef` | `Z3_func_interp_inc_ref` | ReferenceCountingExtra.cs | Increment reference count for function interpretation |
| `FuncInterpDecRef` | `Z3_func_interp_dec_ref` | ReferenceCountingExtra.cs | Decrement reference count for function interpretation |
| `FuncEntryIncRef` | `Z3_func_entry_inc_ref` | ReferenceCountingExtra.cs | Increment reference count for function entry |
| `FuncEntryDecRef` | `Z3_func_entry_dec_ref` | ReferenceCountingExtra.cs | Decrement reference count for function entry |

### ❌ Functions in Z3 but NOT in NativeLibrary (0 missing)

None. All Z3 function interpretation functions are implemented.

### ⚠️ Functions in NativeLibrary but NOT in Z3 (0 extra)

None. All functions in NativeLibrary.FunctionInterpretations.cs correspond to Z3 C API functions.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions (Function Interpretation category) | 9 | 100% |
| Functions in NativeLibrary.FunctionInterpretations.cs | 9 | 100% |
| Reference Counting Functions (ReferenceCountingExtra.cs) | 4 | 100% |
| Missing Functions | 0 | 0% |
| **Total Coverage** | **13/13** | **100%** |

## Function Categories

### Function Interpretation Queries (3/3 = 100%)
- ✅ `Z3_func_interp_get_num_entries` - Count entries in finite map
- ✅ `Z3_func_interp_get_entry` - Retrieve specific entry by index
- ✅ `Z3_func_interp_get_arity` - Get function arity (argument count)

### Function Interpretation Values (2/2 = 100%)
- ✅ `Z3_func_interp_get_else` - Get default value for unmapped inputs
- ✅ `Z3_func_interp_set_else` - Set default value for unmapped inputs

### Function Interpretation Modification (1/1 = 100%)
- ✅ `Z3_func_interp_add_entry` - Add mapping to finite map

### Function Entry Inspection (3/3 = 100%)
- ✅ `Z3_func_entry_get_value` - Get entry's output value
- ✅ `Z3_func_entry_get_num_args` - Get number of input arguments
- ✅ `Z3_func_entry_get_arg` - Get specific input argument by index

### Reference Counting (4/4 = 100% in ReferenceCountingExtra.cs)
- ✅ `Z3_func_interp_inc_ref` - Increment function interpretation reference count
- ✅ `Z3_func_interp_dec_ref` - Decrement function interpretation reference count
- ✅ `Z3_func_entry_inc_ref` - Increment function entry reference count
- ✅ `Z3_func_entry_dec_ref` - Decrement function entry reference count

## Architecture Notes

### Function Interpretation Structure

Function interpretations in Z3 represent how uninterpreted functions behave in a model. They are represented as:

**Finite Map + Else Value**:
- **Finite Map**: Explicit mappings for specific argument tuples → output values
  - Stored as entries (Z3_func_entry objects)
  - Each entry contains: argument values → output value
  - Retrieved via `get_num_entries` and `get_entry(i)`
- **Else Value**: Default value for all other inputs not in the finite map
  - Retrieved via `get_else`
  - Can be modified via `set_else`

### Use Cases

1. **Extracting Function Values from Models**:
   - After solving, extract function interpretations from models
   - Iterate through finite map entries to see explicit mappings
   - Get else value for default behavior

2. **Building Custom Function Interpretations**:
   - Create function interpretation objects
   - Add entries for specific input/output mappings
   - Set else value for unmapped cases

3. **Function Analysis**:
   - Check arity (number of arguments)
   - Inspect how many distinct mappings exist (num_entries)
   - Extract specific argument-value pairs

### Example Usage Pattern

```csharp
// Get function interpretation from model
var funcInterp = model.GetFuncInterp(funcDecl);

// Check arity
var arity = nativeLib.FuncInterpGetArity(ctx, funcInterp);

// Iterate through finite map entries
var numEntries = nativeLib.FuncInterpGetNumEntries(ctx, funcInterp);
for (uint i = 0; i < numEntries; i++)
{
    var entry = nativeLib.FuncInterpGetEntry(ctx, funcInterp, i);

    // Get entry's argument values
    var numArgs = nativeLib.FuncEntryGetNumArgs(ctx, entry);
    for (uint j = 0; j < numArgs; j++)
    {
        var arg = nativeLib.FuncEntryGetArg(ctx, entry, j);
        // Process argument...
    }

    // Get entry's output value
    var value = nativeLib.FuncEntryGetValue(ctx, entry);
    // Process value...
}

// Get default value for unmapped inputs
var elseValue = nativeLib.FuncInterpGetElse(ctx, funcInterp);
```

## Completeness Assessment

**Status**: ✅ **100% Complete** - All Z3 function interpretation API functions are implemented!

### Implementation Quality

- ✅ All 9 core functions have correct signatures
- ✅ All 4 reference counting functions properly located in ReferenceCountingExtra.cs
- ✅ All functions have comprehensive XML documentation
- ✅ Proper delegate declarations for P/Invoke
- ✅ Consistent error handling via GetFunctionPointer
- ✅ Complete coverage of Z3 function interpretation API
- ✅ Clear header comment documenting source and organization

### Documentation Quality

Each function includes:
- Clear summary of purpose
- Parameter descriptions
- Return value documentation
- Usage remarks explaining behavior
- Links to Z3 C API documentation

## Summary

**Complete Implementation**: NativeLibrary.FunctionInterpretations.cs provides 100% coverage of the Z3 function interpretation API:

- **9 core functions** for working with function interpretations and entries
- **4 reference counting functions** properly organized in ReferenceCountingExtra.cs
- **Well-organized structure**: Queries, values, modification, and entry inspection
- **Production-ready**: Comprehensive documentation and proper error handling

The implementation supports full extraction and manipulation of function interpretations from Z3 models, enabling users to:
- Inspect how uninterpreted functions behave in satisfying models
- Extract explicit argument → value mappings
- Get default behavior for unmapped inputs
- Build custom function interpretations programmatically

## Verification

- **Source**: Z3 C API header (z3_api.h)
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
  - Section: Function interpretations (model extraction)
- **Our implementation**:
  - Z3Wrap/Core/Interop/NativeLibrary.FunctionInterpretations.cs (9 functions)
  - Z3Wrap/Core/Interop/NativeLibrary.ReferenceCountingExtra.cs (4 ref counting functions)
- **Verification method**: Direct comparison against Z3 source code
  - Command: `curl -s "https://raw.githubusercontent.com/Z3Prover/z3/master/src/api/z3_api.h" | grep -E "Z3_API.*Z3_func_(interp|entry)"`
  - Result: 13 total functions (9 core + 4 ref counting), all implemented
- **Date**: 2025-10-03
