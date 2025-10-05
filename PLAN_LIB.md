# Z3Library2 Refactoring Plan

## Goal
Create a modular Z3Library2 with generated partial class files organized by Z3 API categories, while keeping the core functionality (library loading, error handling, configuration/context management) in a single main file.

## Current Status
**Phase 3 Complete** - Full code generation pipeline working. Generator produces enums and methods with proper error handling, type conversion, and symbol overloads.

## Completed Work

### Phase 1: Core Infrastructure ✅
- [x] Create `Z3Wrap/Core/Library/` directory
- [x] Create `Z3Library2.cs` with core functionality:
  - Library loading (Load, LoadAuto)
  - Error handling (CheckError, CheckHandle, OnZ3ErrorSafe)
  - Configuration management (MkConfig, DelConfig, SetParamValue)
  - Context management (MkContextRc, DelContext, UpdateParamValue)
  - Version information (GetFullVersion)
  - IDisposable implementation
- [x] Verify Z3Library2 compiles with zero warnings
- [x] All 904 tests pass

### Phase 2: Enum Generation ✅
- [x] Create `scripts/generate_library.py` - enum and method generation tool
- [x] Parse `NativeZ3Library.Enums.generated.cs` with full documentation
- [x] Extract all 10 enum definitions (345 total values)
  - AstKind (7 values) - AST node types
  - AstPrintMode (3 values) - Print modes
  - DeclKind (281 values) - Function declaration kinds
  - ErrorCode (13 values) - Error codes
  - GoalPrec (4 values) - Goal precision
  - Lbool (3 values) - Three-valued boolean
  - ParamKind (7 values) - Parameter types
  - ParameterKind (9 values) - Parameter categories
  - SortKind (16 values) - Sort types
  - SymbolKind (2 values) - Symbol types
- [x] Generate `Z3Library2.Enums.generated.cs` with public enums (2875 lines)
- [x] Preserve complete XML documentation (summary, remarks, seealso)
- [x] Remove seealso references (they point to internal NativeZ3Library methods)
- [x] Verify compilation with zero enum-related warnings
- [x] `make generate-library ENUMS_ONLY=1` command working

### Phase 3: Method Generation ✅
- [x] Parse NativeZ3Library function files for signatures
- [x] Extract parameter information with C type metadata (ctype attributes)
- [x] Filter functions: only include those with Z3_context as first parameter
- [x] Generate proper method wrappers with error handling:
  - [x] Call `CheckError(context)` after every native call
  - [x] Use `CheckHandle(result, nameof(Method))` for IntPtr returns
  - [x] Cast enum return types from internal to public enums
- [x] String parameter conversion:
  - [x] Detect `Z3_string` parameters via ctype attribute
  - [x] Convert `Z3_string` → `string` in public signature
  - [x] Use `AnsiStringPtr` for marshalling
- [x] Symbol parameter handling:
  - [x] Generate string overloads for `Z3_symbol` parameters
  - [x] Call `MkStringSymbol` with `CheckError` for each symbol
  - [x] Generate IntPtr overload for advanced use cases
  - [x] Configuration to skip overloads for getters (GetSymbolKind, etc.)
- [x] XML documentation preservation from NativeZ3Library
- [x] Generate files for all 32 categories
- [x] `make generate-library` command working

## Architecture Decisions

### What Goes Where

**Z3Library2.cs** (Main file - manually maintained):
- Core infrastructure (library loading, disposal)
- Error handling helpers (CheckError, CheckHandle, OnZ3ErrorSafe)
- Configuration management (no context required)
- Context creation/deletion (no context required)
- Version information
- Static factory methods (Load, LoadAuto)

**Z3Library2.Enums.generated.cs** (Generated enum file):
- All 10 Z3 enum types (AstKind, DeclKind, ErrorCode, etc.)
- Public visibility (vs internal in NativeZ3Library)
- Complete XML documentation preserved
- 345 total enum values

**Z3Library2.{Category}.generated.cs** (Generated method files - TODO):
- All methods that accept `IntPtr c` (context) as first parameter
- Automatic error checking via `CheckError(ctx)`
- 32 categories matching NativeZ3Library organization
- 679 methods total across all categories

### Key Design Principles

1. **Context Parameter Rule**: Only methods accepting `IntPtr c` as first parameter go into generated files
   - Rationale: These can benefit from automatic `CheckError(ctx)` calls
   - Exceptions already in Z3Library2.cs: MkContextRc, DelContext, UpdateParamValue (manually managed)

2. **No Global Parameters**: Excluded Z3_global_param_* functions
   - Not context-specific (affect entire Z3 system)
   - Not thread-safe
   - Should be in separate Z3Global class if needed
   - Rationale: Official Z3 APIs (Java, .NET) keep them separate

3. **MkContextRc Only**: Only reference-counted context creation
   - MkContext (non-RC) is legacy from Z3 3.x
   - MkContextRc is official API for Z3 4.x
   - All new features require reference-counted contexts

4. **Generated File Extensions**: All generated files use `.generated.cs` suffix
   - Clearly marks auto-generated code
   - Matches project convention (see NativeZ3Library.*.generated.cs)

## Current Structure
```
Z3Wrap/Core/
├── Z3Library.cs                              # Original (to be replaced)
└── Library/                                  # New modular implementation
    ├── Z3Library2.cs                         # Core class (manual)
    ├── Z3Library2.Enums.generated.cs         # Generated enums (10 enums, 345 values) ✅
    ├── generator_plan.txt                    # Generation plan (679 methods, 32 categories)
    └── [Future method files]
        ├── Z3Library2.Accessors.generated.cs              # 102 methods
        ├── Z3Library2.AlgebraicNumbers.generated.cs       # 21 methods
        ├── Z3Library2.Arrays.generated.cs                 # 9 methods
        ├── Z3Library2.AstMaps.generated.cs                # 11 methods
        ├── Z3Library2.AstVectors.generated.cs             # 9 methods
        ├── Z3Library2.BitVectors.generated.cs             # 49 methods
        ├── Z3Library2.ConstantsAndApplications.generated.cs # 7 methods
        ├── Z3Library2.ContextAndAstReferenceCounting.generated.cs # 9 methods
        ├── Z3Library2.CreateConfiguration.generated.cs    # 2 methods
        ├── Z3Library2.ErrorHandling.generated.cs          # 4 methods
        ├── Z3Library2.FloatingPointArithmetic.generated.cs # 61 methods
        ├── Z3Library2.Goals.generated.cs                  # 16 methods
        ├── Z3Library2.IntegersAndReals.generated.cs       # 17 methods
        ├── Z3Library2.Models.generated.cs                 # 32 methods
        ├── Z3Library2.Modifiers.generated.cs              # 4 methods
        ├── Z3Library2.Numerals.generated.cs               # 8 methods
        ├── Z3Library2.OptimizationFacilities.generated.cs # 29 methods
        ├── Z3Library2.ParameterDescriptions.generated.cs  # 7 methods
        ├── Z3Library2.Parameters.generated.cs             # 9 methods
        ├── Z3Library2.ParserInterface.generated.cs        # 9 methods
        ├── Z3Library2.PropositionalLogicAndEquality.generated.cs # 11 methods
        ├── Z3Library2.Quantifiers.generated.cs            # 12 methods
        ├── Z3Library2.SequencesAndRegularExpressions.generated.cs # 65 methods
        ├── Z3Library2.Sets.generated.cs                   # 12 methods
        ├── Z3Library2.Solvers.generated.cs                # 55 methods
        ├── Z3Library2.Sorts.generated.cs                  # 21 methods
        ├── Z3Library2.SpecialRelations.generated.cs       # 5 methods
        ├── Z3Library2.Statistics.generated.cs             # 9 methods
        ├── Z3Library2.StringConversion.generated.cs       # 7 methods
        ├── Z3Library2.Symbols.generated.cs                # 2 methods
        ├── Z3Library2.TacticsSimplifiersAndProbes.generated.cs # 46 methods
        └── Z3Library2.Z3SpecificFloatingPointExtensions.generated.cs # 19 methods
```

## Next Steps

### Phase 4: Integration & Testing (IN PROGRESS)
- [ ] Generate all 32 partial class files
- [ ] Verify compilation with zero warnings
- [ ] Run `make test` - verify all 904 tests pass
- [ ] Update Z3Context to use Z3Library2 instead of Z3Library
- [ ] Update any other references to Z3Library

### Phase 5: Verification (TODO)
- [ ] Run `make ci` - full pipeline must pass
- [ ] Verify coverage remains ≥90%
- [ ] Manual review of generated code quality
- [ ] Compare generated methods with original Z3Library for correctness
- [ ] Update CLAUDE.md with new structure
- [ ] Update this plan with lessons learned

## Generation Script Details

**Tool**: `scripts/generate_library.py`

**Current Capabilities**:
- **Enum Generation**:
  - Parses `NativeZ3Library.Enums.generated.cs` with regex patterns
  - Extracts enum definitions with full documentation (summary, remarks)
  - Extracts enum values with their numeric values and documentation
  - Generates `Z3Library2.Enums.generated.cs` with public enums
  - Preserves multi-line XML documentation
  - Removes seealso references (point to internal methods)

- **Method Generation**:
  - Parses all `NativeZ3Library.*.generated.cs` files
  - Extracts function signatures with ctype attributes
  - Filters functions: only includes those with `Z3_context` as first parameter
  - Generates wrapper methods with proper error handling
  - Converts `Z3_string` parameters to `string` with `AnsiStringPtr`
  - Generates string overloads for `Z3_symbol` parameters (configurable skip list)
  - Casts enum return types from internal to public enums
  - Preserves XML documentation (summary, param, returns)
  - Auto-generates default parameter descriptions from C types

**Commands**:
- `make generate-library` - Generate all files (enums + methods)
- `make generate-library ENUMS_ONLY=1` - Generate only enums (faster)

**Configuration**:
- `SKIP_SYMBOL_STRING_OVERLOAD` - Set of function names to skip string overloads for symbols
  - Currently: `GetSymbolKind`, `GetSymbolString`, `GetSymbolInt`

## Code Generation Examples

### Basic Method with IntPtr Return
```csharp
public IntPtr MkConst(IntPtr c, IntPtr symbol, IntPtr sort)
{
    var result = nativeLibrary.MkConst(c, symbol, sort);
    CheckError(c);
    return CheckHandle(result, nameof(MkConst));
}
```

### Method with String Parameters
```csharp
public IntPtr MkStringSymbol(IntPtr c, string s)
{
    using var sAnsi = new AnsiStringPtr(s);
    var result = nativeLibrary.MkStringSymbol(c, sAnsi);
    CheckError(c);
    return CheckHandle(result, nameof(MkStringSymbol));
}
```

### Method with Symbol Parameter (Two Overloads)
```csharp
// String overload (preferred)
public IntPtr MkConst(IntPtr c, string symbol, IntPtr sort)
{
    using var symbolAnsi = new AnsiStringPtr(symbol);
    var symbolSymbol = nativeLibrary.MkStringSymbol(c, symbolAnsi);
    CheckError(c);
    var result = nativeLibrary.MkConst(c, symbolSymbol, sort);
    CheckError(c);
    return CheckHandle(result, nameof(MkConst));
}

// IntPtr overload (advanced)
public IntPtr MkConst(IntPtr c, IntPtr symbol, IntPtr sort)
{
    var result = nativeLibrary.MkConst(c, symbol, sort);
    CheckError(c);
    return CheckHandle(result, nameof(MkConst));
}
```

### Method with Enum Return Type
```csharp
public SymbolKind GetSymbolKind(IntPtr c, IntPtr s)
{
    var result = nativeLibrary.GetSymbolKind(c, s);
    CheckError(c);
    return (SymbolKind)result;  // Cast from internal to public enum
}
```

### Void Method
```csharp
public void IncRef(IntPtr c, IntPtr ast)
{
    nativeLibrary.IncRef(c, ast);
    CheckError(c);
}
```

## Benefits
- **Public API**: Enums are public (vs internal in NativeZ3Library)
- **Modularity**: 679 methods organized into 32 logical categories (TODO)
- **Consistency**: Uniform error handling via `CheckError(ctx)` (TODO)
- **Maintainability**: Generated code easy to update when Z3 API changes
- **Discoverability**: Related functions grouped together by category
- **Safety**: Automatic error checking for all context-based operations (TODO)
- **Documentation**: Complete XML docs preserved from NativeZ3Library

## Testing Strategy
- All existing 904 tests must continue to pass
- No new tests required (Z3Library2 is internal wrapper)
- Coverage must remain ≥90%
- Integration testing via existing test suite
- Manual verification of generated code quality

## Migration Path
1. Complete Z3Library2 generation
2. Update Z3Context to use Z3Library2
3. Run full test suite
4. Keep Z3Library.cs temporarily for reference
5. After verification period, remove Z3Library.cs
6. Update documentation and CLAUDE.md

## Notes
- Z3Library2 is internal infrastructure, not exposed to library users
- All user-facing APIs (Z3Context, Z3Solver, etc.) remain unchanged
- Generated files use `.generated.cs` suffix for clarity
- Use `make format` after generation to ensure code style compliance
- Follow project XML documentation standards (concise, clear, no fluff)
