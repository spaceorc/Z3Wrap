# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **Robust Z3 error handling system with process safety**
  - `Z3Exception` class with structured error reporting and `Z3ErrorCode` enum
  - Safe wrapper layer (`SafeNativeMethods`) with automatic Z3 error detection after each operation
  - Converts Z3 crashes into managed exceptions preventing process termination
- **Universal and existential quantifier support**
  - `ForAll()` and `Exists()` extension methods for Z3Context with 1-3 variable overloads
  - Natural mathematical syntax with nested quantifier support and type mixing
- **BitVector boundary check API**
  - `BitVecBoundaryCheck()` extension method with BigInteger support
  - Overflow and underflow detection for all BitVector operations
  - Corrected signed/unsigned negation logic

### Changed
- **BREAKING**: Made Handle properties and factory methods internal
- **BREAKING**: Renamed `Z3SortKind.BV` to `Z3SortKind.Bv`
- **Migrated to SafeNativeMethods** for all Z3 P/Invoke calls with automatic error checking

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
- **Complete BitVector theory support** with arithmetic, bitwise, and comparison operations
- Cross-platform Z3 library auto-discovery
- Memory-safe reference-counted contexts

### Changed
- Updated to .NET 9.0 target framework

### Fixed
- Platform-specific Z3 behavior in signed multiplication overflow tests
- Streamlined test output by removing redundant error logging