# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **Complete code generation infrastructure for Z3 C API wrapper**
  - Automated generation of all Z3Library methods from Z3 C API using Doxygen XML
  - 34 category-organized generated files (Arrays, BitVectors, Solvers, etc.)
  - Callback delegates with C# naming conventions and nullable reference type annotations
  - XML documentation auto-generated from Z3 API comments
  - Proper handling of out parameters and reference counting methods
- **Z3Library safe wrapper with cross-platform support**
  - Public API for loading and managing Z3 native library
  - `Load(path)` and `LoadAuto()` methods for manual and automatic library discovery
  - Safe error checking and handle validation for all operations
  - Cross-platform native library discovery (Windows, macOS, Linux)
- **Type-safe value types for compile-time guarantees**
  - `BvExpr<TSize>` and `Bv<TSize>` with compile-time sized types (`Size8`, `Size16`, `Size32`, `Size64`)
  - `Real` struct for unlimited precision rational arithmetic (exact fractions)
  - Full support for arithmetic, bitwise, and comparison operations
- **Complete expression type hierarchy**
  - Type-safe expression classes: `BoolExpr`, `IntExpr`, `RealExpr`, `BvExpr<TSize>`, `ArrayExpr<TIndex, TValue>`
  - Category-based organization: Arrays/, BitVectors/, Functions/, Logic/, Numerics/, Quantifiers/
  - Natural mathematical syntax via operator overloading and extension methods
- **Uninterpreted functions and solver enhancements**
  - `FuncDecl` for function declarations with dynamic builder pattern
  - Solver parameter extensions: `SetParam` for bool/uint/double/string and `SetTimeout(TimeSpan)`
- **Production-ready documentation**
  - Complete XML documentation for all public APIs (zero warnings enforced)

### Changed
- **BREAKING**: Renamed expression types: `Z3Bool` → `BoolExpr`, `Z3IntExpr` → `IntExpr`, `Z3Real` → `RealExpr`
- **BREAKING**: Changed from runtime-sized to compile-time sized bitvectors (`BvExpr<Size32>` vs old `BitVecConst("x", 32)`)
- **BREAKING**: Renamed BitVector identifiers: `BitVecConst` → `BvConst`, `BitVec` → `Bv`, `ToBitVec` → `ToBv`, `Z3SortKind.BV` → `Z3SortKind.Bv`
- **BREAKING**: Reorganized namespaces from flat structure to category-based organization
- **BREAKING**: Made Handle properties and factory methods internal

### Removed
- **BREAKING**: Removed runtime-sized BitVector API (replaced with compile-time sized generics)
- Removed BitVector boundary check builder API (replaced with overflow detection in expressions)

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