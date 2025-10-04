# Z3Library Refactoring Plan

## Goal
Reorganize Z3Library into a modular structure with generated partial class files organized by Z3 API categories.

## Current Structure
- Single file: `Z3Wrap/Core/Z3Library.cs` (complete wrapper around native Z3 API)
- All P/Invoke wrappers defined in one partial class file
- Core functionality: error handling, disposal, library loading

## Target Structure
```
Z3Wrap/Core/
├── Z3Library.cs                    # Main class (to be replaced by Z3Library2)
└── Library/                        # New directory for modular implementation
    ├── Z3Library2.cs               # Core class (copy of current Z3Library.cs)
    ├── Z3Library.Contexts.cs       # GENERATED - Context management functions
    ├── Z3Library.Sorts.cs          # GENERATED - Sort/type functions
    ├── Z3Library.Numerals.cs       # GENERATED - Integer, Real, BitVector creation
    ├── Z3Library.Boolean.cs        # GENERATED - Boolean logic functions
    ├── Z3Library.Arithmetic.cs     # GENERATED - Arithmetic operations
    ├── Z3Library.BitVectors.cs     # GENERATED - BitVector operations
    ├── Z3Library.Arrays.cs         # GENERATED - Array operations
    ├── Z3Library.Functions.cs      # GENERATED - Function declarations
    ├── Z3Library.Quantifiers.cs    # GENERATED - ForAll, Exists
    ├── Z3Library.Solvers.cs        # GENERATED - Solver operations
    ├── Z3Library.Models.cs         # GENERATED - Model operations
    └── Z3Library.Misc.cs           # GENERATED - Other utilities
```

## Implementation Steps

### Phase 1: Setup (Current)
- [x] Create `Z3Wrap/Core/Library/` directory
- [x] Copy `Z3Library.cs` to `Z3Library2.cs` in new directory
- [ ] Verify Z3Library2 compiles and tests pass

### Phase 2: Analysis
- [ ] Analyze current Z3Library partial files to identify all P/Invoke methods
- [ ] Categorize methods by Z3 API area (contexts, sorts, numerals, etc.)
- [ ] Document method signatures and return types for code generation

### Phase 3: Code Generation
- [ ] Create Python/script to generate partial class files
- [ ] Generate each category file (Z3Library.Contexts.cs, etc.)
- [ ] Ensure all methods have proper error checking and handle validation
- [ ] Preserve XML documentation from original

### Phase 4: Integration
- [ ] Replace `Z3Library.cs` with reference to `Z3Library2.cs`
- [ ] Update namespace references if needed
- [ ] Run `make build` - verify zero warnings
- [ ] Run `make test` - verify all 837 tests pass

### Phase 5: Verification
- [ ] Run `make ci` - full pipeline must pass
- [ ] Verify coverage remains ≥90%
- [ ] Manual review of generated code quality
- [ ] Update CLAUDE.md with new structure

## Benefits
- **Modularity**: Easier to navigate and maintain Z3 API bindings
- **Generation**: Automated creation from Z3 API documentation
- **Consistency**: Uniform error handling and validation patterns
- **Discoverability**: Related functions grouped together
- **Future-proof**: Easy to update when Z3 API changes

## Notes
- Z3Library2 will become the permanent implementation
- Original Z3Library.cs may be kept temporarily for reference
- All generated files must follow project coding standards
- Use `make format` after generation to ensure proper formatting
