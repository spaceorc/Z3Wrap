# Changelog

All notable changes to Z3Wrap will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Pseudo-Boolean constraints (`AtMost`, `AtLeast`, `Exactly`, `PbLe`, `PbGe`, `PbEq`) for cardinality and weighted constraints
- IEEE 754 floating-point arithmetic (`FpExpr<TFormat>`) with Half, Single, and Double precision formats, rounding modes, and special values
- Floating-point component API (`FpFromComponents`, `GetFpComponents`) for constructing and deconstructing FP values from raw IEEE 754 bit components
- `LocalZ3Handle` utility class for managing temporary Z3 AST handles with automatic reference counting

## [0.0.5] - 2025-11-23

### Added
- Proof generation (`Z3Solver.GetProof()`) for retrieving S-expression proofs of unsatisfiability
- String theory support (`StringExpr`, `CharExpr`)
- Regular expression support (`RegexExpr`) with factory methods, operations (Star, Plus, Option, Complement, Union, Concat, Intersect, Diff, Loop, Power), and operators (+, |, ~, &, -)
- Sequence theory support (`SeqExpr<T>`) with generic sequences for any element type
- Sequence higher-order functions (`Map`, `Mapi`, `Foldl`, `Foldli`) for functional programming over sequences
- Lambda expressions (`LambdaExpr<T1, TResult>`, `LambdaExpr<T1, T2, TResult>`, `LambdaExpr<T1, T2, T3, TResult>`) for anonymous functions in Z3
- Quantifiers (`ForAll`, `Exists`)
- Uninterpreted functions (`FuncDecl`)
- Optimization solver (`Z3Optimize`)
- Unsatisfiable cores (`CheckAssumptions`, `GetUnsatCore`)
- Multi-dimensional arrays (`ArrayExpr<T1,T2,TValue>`, `ArrayExpr<T1,T2,T3,TValue>`) for 2D and 3D arrays
- Example documentation (docs/examples/)
- Arithmetic functions: `Power` (exponentiation), `Divides` (divisibility check), `IsInt` (check if real is integer)
- Generic arithmetic function extensions for `Abs`, `Min`, `Max` in `ArithmeticFunctionsExprExtensions`

### Changed
- CI/CD upgraded to Z3 4.15.4 on Ubuntu 24.04
- **BREAKING**: Expression type renames: `Z3BoolExpr` → `BoolExpr`, `Z3IntExpr` → `IntExpr`, `Z3RealExpr` → `RealExpr`, `Z3ArrayExpr` → `ArrayExpr`
- **BREAKING**: Runtime-sized `Z3BitVecExpr` → compile-time `BvExpr<TSize>`
- **BREAKING**: Namespace reorganization to category-based structure
- `Abs` now uses native `Z3_mk_abs` instead of composite expression for better performance
- `Power` always returns `RealExpr` (Z3's design - power operation returns Real sort even for integer inputs)

### Deprecated
- `SeqExpr<T>.LastIndexOf` marked as obsolete due to unstable behavior in Z3

### Fixed
- Type validation error messages now report correct expected vs actual sorts
- Code generation script now correctly detects input array parameters vs output parameters
- Error handler delegate now stored to prevent garbage collection crashes

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