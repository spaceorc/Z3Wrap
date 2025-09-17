# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.0.3] - TBD

### Fixed
- Fixed incorrect package name in GitHub release installation instructions
- Corrected NuGet package release notes to use GitHub release notes automatically
- Fixed changelog extraction bug in GitHub Actions release workflow
- Workflow now correctly prioritizes version-specific sections over [Unreleased]

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