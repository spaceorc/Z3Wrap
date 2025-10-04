# Z3 Native Library Code Generation Plan

## Overview

This document tracks the plan and progress for generating NativeLibrary2 partial classes from Z3 C API header files.

## Project Goals

1. **Perfect 1-to-1 Mapping**: Each header group should map to exactly one partial class file
2. **Automated Generation**: Use Python script to generate all P/Invoke bindings automatically
3. **Clean Structure**: Generated files in `Interop2/` directory, separate from manual `Interop/`
4. **Source of Truth**: C API header files are the ultimate source of truth for organization

## Current Status

### ✅ Completed

1. **Infrastructure Setup**
   - Created `Z3Wrap/Core/Interop2/` directory for generated code
   - Created `scripts/` directory for generator scripts
   - Added `make generate-native` command to Makefile

2. **Root NativeLibrary2.cs**
   - Created base partial class with same structure as NativeLibrary.cs
   - Includes platform detection, library loading, function pointer management
   - Has placeholder for LoadFunctions calls (to be generated later)

3. **Header Analysis & Planning**
   - Downloaded 5 Z3 C API header files to `c_headers/`:
     - `z3_api.h` (main API)
     - `z3_algebraic.h` (algebraic numbers)
     - `z3_ast_containers.h` (AST vectors and maps)
     - `z3_fpa.h` (floating-point arithmetic)
     - `z3_optimization.h` (optimization)

4. **Python Generator Script** (`scripts/generate_native_library.py`)
   - Analyzes all header files in `c_headers/`
   - Extracts all `@name` groups (35 groups found)
   - Extracts all function names from each group (707 total functions)
   - Generates detailed plan file with:
     - Target file names (with `.generated.cs` suffix)
     - Function counts per group
     - Complete function lists per group

5. **Generated Plan File** (`Z3Wrap/Core/Interop2/generation_plan.txt`)
   - Complete mapping of 35 groups to 35 partial class files
   - All 707 Z3 functions categorized by group
   - Ready for code generation phase

## Header File Structure

### z3_api.h (29 groups)
- Global Parameters (3 functions)
- Create configuration (3 functions)
- Context and AST Reference Counting (9 functions)
- Parameters (9 functions)
- Parameter Descriptions (7 functions)
- Symbols (2 functions)
- Sorts (21 functions)
- Constants and Applications (7 functions)
- Propositional Logic and Equality (11 functions)
- Integers and Reals (17 functions)
- Bit-vectors (49 functions)
- Arrays (9 functions)
- Sets (12 functions)
- Numerals (8 functions)
- Sequences and regular expressions (65 functions)
- Special relations (5 functions)
- Quantifiers (12 functions)
- Accessors (102 functions - largest group)
- Modifiers (5 functions)
- Models (32 functions)
- Interaction logging (4 functions)
- String conversion (7 functions)
- Parser interface (9 functions)
- Error Handling (4 functions)
- Miscellaneous (6 functions)
- Goals (17 functions)
- Tactics, Simplifiers and Probes (55 functions)
- Solvers (56 functions)
- Statistics (10 functions)

### z3_algebraic.h (1 group)
- Algebraic Numbers (21 functions)

### z3_ast_containers.h (2 groups)
- AST vectors (10 functions)
- AST maps (11 functions)

### z3_fpa.h (2 groups)
- Floating-Point Arithmetic (61 functions)
- Z3-specific floating-point extensions (19 functions)

### z3_optimization.h (1 group)
- Optimization facilities (29 functions)

## File Naming Convention

All generated files use the `.generated.cs` suffix:
- Format: `NativeLibrary2.{GroupName}.generated.cs`
- Examples:
  - `NativeLibrary2.AlgebraicNumbers.generated.cs`
  - `NativeLibrary2.ContextAndAstReferenceCounting.generated.cs`
  - `NativeLibrary2.FloatingPointArithmetic.generated.cs`

## Group Name Conversion Rules

Header group names are converted to C# class names using these rules:
1. Remove special characters (keep letters, numbers, spaces, hyphens)
2. Split on spaces and hyphens
3. Capitalize each word
4. Join without separators

Examples:
- "Algebraic Numbers" → `AlgebraicNumbers`
- "Context and AST Reference Counting" → `ContextAndAstReferenceCounting`
- "Bit-vectors" → `BitVectors`

## Architecture Decisions

### LoadLibraryInternal Strategy (Option 1 - Selected)

The `LoadLibraryInternal` method will be **generated** as part of the code generation:
- Will be placed in a generated partial class file (e.g., `NativeLibrary2.LoaderMethods.generated.cs`)
- Contains all 35 `LoadFunctionsXXX(handle, functionPointers)` calls
- Regenerated automatically when headers change
- No reflection needed - direct method calls for best performance
- Main `NativeLibrary2.cs` stays clean with only infrastructure code

Alternative approaches considered:
- Option 2: Use reflection to find and call LoadFunctions methods (slower, more flexible)
- Option 3: Registration pattern where each generated file registers itself (complex)

### Directory Structure

```
Z3Wrap/Core/
├── Interop/                          # Original manual implementation
│   ├── NativeLibrary.cs
│   ├── NativeLibrary.*.cs           # Original partials
│   └── NativeLibrary.*_Invalid.cs   # Renamed invalid files
└── Interop2/                         # New generated implementation
    ├── NativeLibrary2.cs            # Root partial (manual)
    ├── generation_plan.txt          # Generated plan
    └── NativeLibrary2.*.generated.cs # Generated partials (to be created)
```

## Next Steps

### Phase 1: Generate Partial Class Files
1. Extend Python script to generate actual C# code:
   - Parse function signatures from headers
   - Extract parameter types and return types
   - Generate P/Invoke delegate declarations
   - Generate wrapper methods with proper marshalling
   - Generate `LoadFunctionsXXX` method per file

2. Generate template structure:
```csharp
// NativeLibrary2.{GroupName}.generated.cs
// <auto-generated>
// This file was generated by scripts/generate_native_library.py
// Source: {header_file} / {group_name}
// DO NOT EDIT - Changes will be overwritten
// </auto-generated>

namespace Spaceorc.Z3Wrap.Core.Interop2;

internal sealed partial class NativeLibrary2
{
    private static void LoadFunctions{GroupName}(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionInternal(handle, functionPointers, "Z3_function_name");
        // ... more functions
    }

    // Delegates
    private delegate ReturnType FunctionNameDelegate(params);

    // Wrapper methods
    internal ReturnType FunctionName(params)
    {
        var funcPtr = GetFunctionPointer("Z3_function_name");
        var func = Marshal.GetDelegateForFunctionPointer<FunctionNameDelegate>(funcPtr);
        return func(params);
    }
}
```

### Phase 2: Generate LoadLibraryInternal
Generate `NativeLibrary2.LoaderMethods.generated.cs` with:
- All 35 `LoadFunctionsXXX` calls in correct order
- Auto-generated comment header
- Proper exception handling

### Phase 3: Type Mapping
Create type mapping for C to C# types:
- `Z3_context` → `IntPtr`
- `Z3_ast` → `IntPtr`
- `Z3_sort` → `IntPtr`
- `Z3_symbol` → `IntPtr`
- `bool` → `bool` (with `[return: MarshalAs(UnmanagedType.I1)]`)
- `int` → `int`
- `unsigned int` → `uint`
- `int64_t` → `long`
- `uint64_t` → `ulong`
- `double` → `double`
- `char*` → `IntPtr` (manually marshaled strings)
- Arrays → `IntPtr` with length parameter

### Phase 4: Documentation
Generate XML documentation from header comments:
- Extract `\brief` descriptions
- Extract `\param` descriptions
- Extract `\pre`, `\post` conditions
- Add `<seealso href="...">` links to Z3 docs

### Phase 5: Validation
1. Run `make build` - should compile without errors
2. Compare function count with `c_headers/all_methods.txt`
3. Verify all 707 functions are present
4. Test basic functionality with simple Z3 operations

## Tools and Commands

### Make Commands
- `make generate-native` - Run generator script
- `make build` - Build the library
- `make test` - Run tests
- `make format` - Format generated code

### Python Script Location
- `scripts/generate_native_library.py`
- Run directly: `python3 scripts/generate_native_library.py`

### Key Files
- Header files: `c_headers/*.h`
- All methods list: `c_headers/all_methods.txt` (707 functions)
- Groups mapping: `c_headers/groups_by_header.txt`
- Final mapping: `c_headers/final_mapping.txt`
- Generation plan: `Z3Wrap/Core/Interop2/generation_plan.txt`

## Notes

### Why Start From Scratch (Interop2)?
- Original `Interop/` had misaligned partial classes (not matching header groups)
- Easier to generate cleanly than to refactor existing code
- Can validate generated code against original implementation
- Original code preserved for reference (with `_Invalid.cs` suffix where needed)

### File Naming Rationale
- `.generated.cs` suffix is standard in .NET for auto-generated code
- Clear distinction from manually-written code
- Git ignore patterns can target generated files
- Developers know not to manually edit these files

### Function Count Verification
- Total in headers: 707 functions
- Total extracted: 707 functions ✓
- This matches the `c_headers/all_methods.txt` file

### Skipped Groups
- "Types" group in z3_api.h - only contains type declarations, no functions

## References

- Z3 C API Documentation: https://z3prover.github.io/api/html/group__capi.html
- Z3 GitHub Repository: https://github.com/Z3Prover/z3
- Header files source: https://github.com/Z3Prover/z3/tree/master/src/api
