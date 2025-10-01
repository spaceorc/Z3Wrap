# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **Compile-time sized BitVectors with generic type parameters**
  - `BvExpr<TSize>` with `Size8`, `Size16`, `Size32`, `Size64` for type-safe bit-width enforcement
  - `Bv<TSize>` value type for constant bitvector values with arithmetic and bitwise operations
  - Comprehensive bitvector tests (120+ tests covering arithmetic, bitwise, comparison, overflow detection)
- **Value types for exact arithmetic**
  - `Real` struct with unlimited precision rational arithmetic (fraction representation)
  - Factory methods, conversions, arithmetic, comparison operations
- **Complete expression type hierarchy reorganization**
  - Organized by category: Arrays/, BitVectors/, Functions/, Logic/, Numerics/, Quantifiers/
  - Type-safe expression classes: `BoolExpr`, `IntExpr`, `RealExpr`, `BvExpr<TSize>`, `ArrayExpr<TIndex, TValue>`
  - Natural syntax support via extension methods for all operators
- **Uninterpreted Functions support**
  - `FuncDecl` for function declarations with dynamic builder pattern
  - Full function application and solving tests (30+ tests)
- **Thread safety and comprehensive disposal testing**
  - ThreadLocal context isolation with stress testing (10 threads × 100 cycles)
  - Comprehensive disposal scenarios (12 tests) including GC finalizer safety
  - Exception recovery and nested scope restoration tests
- **Production-ready documentation**
  - All README examples validated in `ReadmeExamplesTests.cs` (10 tests, 100% copy-paste reliability)
  - Complete XML documentation for all public APIs (zero warnings enforced)
  - Updated project documentation (PLAN.md, CLAUDE.md) reflecting 93.3% coverage

### Changed
- **BREAKING**: Renamed `BitVecConst` → `BvConst`, `BitVec` → `Bv`, `ToBitVec` → `ToBv` for consistency
- **BREAKING**: Changed from runtime-sized to compile-time sized bitvectors (`BvExpr<Size32>` vs old `BitVecConst("x", 32)`)
- **BREAKING**: Reorganized namespaces from flat structure to organized by expression category
- **BREAKING**: Renamed expression types: `Z3Bool` → `BoolExpr`, `Z3IntExpr` → `IntExpr`, `Z3Real` → `RealExpr`
- **BREAKING**: Made Handle properties and factory methods internal
- **BREAKING**: Renamed `Z3SortKind.BV` to `Z3SortKind.Bv`
- Migrated to `SafeNativeMethods` for all Z3 P/Invoke calls with automatic error checking
- Refactored project structure: 79 source files, 37 test files (removed 100+ outdated tests)
- Improved test organization with hierarchical structure by expression type
- Enhanced operator overloading for natural mathematical syntax across all types

### Removed
- **BREAKING**: Removed runtime-sized BitVector API (replaced with compile-time sized generics)
- **BREAKING**: Removed `__old` test directory (2,392 lines of outdated/superseded tests)
- Removed BitVector boundary check builder API (replaced with overflow detection in expressions)
- Removed redundant integration tests (superseded by modern parameterized tests)

## [0.0.4] - 2025-09-17

### Fixed
- Fixed incorrect package name in GitHub release installation instructions
- Corrected NuGet package release notes to use GitHub release notes automatically
- Fixed changelog extraction bug in GitHub Actions release workflow

## [0.0.3] - 2025-09-17

## [0.0.2] - 2025-09-17

### Fixed
- Updated changelog to properly reflect 0.0.1 release history

## [0.0.1] - 2025-09-17

### Added
- Complete Z3 C API wrapper with P/Invoke bindings
- Unlimited precision arithmetic (BigInteger integers, exact rationals)
- Natural mathematical syntax with operator overloading
- Type-safe generic arrays and strongly typed expressions
- Complete BitVector theory support with arithmetic, bitwise, and comparison operations
- Cross-platform Z3 library auto-discovery
- Memory-safe reference-counted contexts