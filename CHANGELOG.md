# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- String theory support (`StringExpr`, `CharExpr`)
- Regular expression support (`RegexExpr`) with factory methods, operations (Star, Plus, Option, Complement, Union, Concat, Intersect, Diff, Loop, Power), and operators (+, |, ~, &, -)
- Sequence theory support (`SeqExpr<T>`) with generic sequences for any element type
- Quantifiers (`ForAll`, `Exists`)
- Uninterpreted functions (`FuncDecl`)
- Optimization solver (`Z3Optimize`)
- Unsatisfiable cores (`CheckAssumptions`, `GetUnsatCore`)
- Example documentation (docs/examples/)

### Changed
- CI/CD upgraded to Z3 4.15.4 on Ubuntu 24.04
- **BREAKING**: Expression type renames: `Z3BoolExpr` → `BoolExpr`, `Z3IntExpr` → `IntExpr`, `Z3RealExpr` → `RealExpr`, `Z3ArrayExpr` → `ArrayExpr`
- **BREAKING**: Runtime-sized `Z3BitVecExpr` → compile-time `BvExpr<TSize>`
- **BREAKING**: Namespace reorganization to category-based structure

### Fixed
- Type validation error messages now report correct expected vs actual sorts

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