# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Complete XML documentation for BitVec class

### Changed
- Completely reinvented release process with MinVer for automatic semantic versioning
- Migrated from manual version management to Git tag-based versioning
- Enhanced CI/CD pipeline with improved coverage requirements (90% minimum)
- Streamlined release workflow with automatic changelog integration

## [0.0.5] - 2025-09-17

### Added
- Fluent BitVector boundary check API with comprehensive BigInteger support
  - New `BitVecBoundaryCheck()` extension method for natural boundary constraint syntax
  - Support for Add, Sub, Mul, Div, Neg operations with full BigInteger overloads
  - Both positive (`NoOverflow`, `NoUnderflow`) and negative (`Overflow`, `Underflow`) boundary checks
  - Corrected negation boundary logic for proper signed/unsigned behavior
  - 43 comprehensive unit tests covering all operations and edge cases
  - Complete API consistency with existing Z3Wrap BigInteger patterns

### Changed
- Enhanced NuGet package publishing workflow to automatically include GitHub release notes

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
- Cross-platform Z3 library auto-discovery
- Memory-safe reference-counted contexts
- 553+ comprehensive tests with 80%+ coverage requirement
- Automated CI/CD pipeline with GitHub Actions
- Coverage badges and test metrics extraction

### Changed
- Updated to .NET 9.0 target framework

### Fixed
- Platform-specific Z3 behavior in signed multiplication overflow tests
- Streamlined test output by removing redundant error logging