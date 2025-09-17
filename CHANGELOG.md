# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Complete Z3 C API wrapper with P/Invoke bindings
- Unlimited precision arithmetic (BigInteger integers, exact rationals)
- Natural mathematical syntax with operator overloading
- Type-safe generic arrays and strongly typed expressions
- Cross-platform Z3 library auto-discovery
- Memory-safe reference-counted contexts
- 1000+ comprehensive tests with 95%+ coverage
- Automated NuGet publishing pipeline

### Changed
- Updated to .NET 9.0 target framework

### Fixed
- Platform-specific Z3 behavior in signed multiplication overflow tests

## [0.0.1] - TBD

### Added
- Initial pre-release version
- Core Z3Wrap functionality