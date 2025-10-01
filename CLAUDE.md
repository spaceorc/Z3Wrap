# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with this repository.

## Project Overview

**Z3Wrap** is a complete, production-ready C# wrapper for Microsoft's Z3 theorem prover featuring unlimited precision arithmetic, type-safe API design, and natural mathematical syntax.

**Current Status**: Mature .NET 9.0 library with 1137+ tests achieving 80%+ coverage requirement in CI.

## Development Workflow

**CRITICAL**: This project uses a comprehensive Makefile for ALL development tasks. NEVER use direct `dotnet` commands.

### Essential Commands
```bash
make help         # Show all available commands
make build        # Build library (includes restore)
make test         # Run all 1137+ tests
make coverage     # Generate coverage report and open in browser
make format       # Format code (CSharpier) - REQUIRED before commits
make lint         # Check formatting (used in CI)
make ci           # Full CI pipeline (lint, build, test, coverage)
make clean        # Clean all build artifacts
```

### Why Makefile-First?
- **Consistency**: Same commands across all platforms and environments
- **Dependencies**: Automatic prerequisite handling (restore → build → test)
- **CI Integration**: Identical commands locally and in GitHub Actions
- **Tool Management**: Handles installation checks and provides helpful error messages

**Rule**: Always use `make [command]` instead of `dotnet [command]` to ensure consistent behavior.

## Project Structure

```
Z3Wrap.sln                    # Solution with library + tests
├── Z3Wrap/                   # Main library (48 source files)
│   ├── DataTypes/               # Real.cs, BitVec.cs
│   ├── Expressions/             # Z3Expr hierarchy (7 classes)
│   ├── Interop/                 # P/Invoke bindings (4 classes)
│   ├── Z3Context.cs             # Main context with scoped setup
│   ├── Z3Solver.cs              # Solver with push/pop operations
│   ├── Z3Model.cs               # Model extraction
│   └── Z3ContextExtensions.*.cs # 20+ extension files by functionality
├── Z3Wrap.Tests/             # NUnit tests (46 test files, 1137+ tests)
│   ├── Unit/Core/               # Core functionality tests
│   ├── Unit/DataTypes/          # Real, BitVec tests
│   ├── Unit/Expressions/        # Expression type tests (organized by type)
│   ├── Integration/             # Cross-component integration tests
│   └── Unit/ReadmeExamplesTests.cs # Validates all README examples
├── Makefile                  # Development workflow automation
├── scripts/                  # Build and release automation
└── .github/workflows/ci.yml  # GitHub Actions CI pipeline
```

## Key Features

- **Unlimited Precision**: BigInteger integers, exact rational arithmetic via Real class
- **Type Safety**: Strongly typed expressions, generic constraints, compile-time checking
- **Natural Syntax**: Mathematical operators (`x + y == 10`) via scoped context pattern
- **Complete Z3 Coverage**: Booleans, Integers, Reals, BitVectors, Arrays with full operator support
- **Memory Safety**: Reference-counted contexts, automatic cleanup, no resource leaks
- **Cross-Platform**: Auto-discovery of Z3 native library on Windows, macOS, Linux

## API Usage Examples

### Basic Solving
```csharp
using var context = new Z3Context();
using var scope = context.SetUp(); // Enable natural syntax

var x = context.IntConst("x");
var y = context.RealConst("y");

using var solver = context.CreateSolver();
solver.Assert(x + 5 == 10);              // BigInteger arithmetic
solver.Assert(x.ToReal() + y == 15.5m);  // Type conversions
solver.Assert(y == new Real(1, 3));      // Exact rationals

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");        // BigInteger
    Console.WriteLine($"y = {model.GetRealValueAsString(y)}"); // "1/3"
}
```

### Generic Arrays and BitVectors
```csharp
// Type-safe generic arrays
var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
solver.Assert(prices[0] == 10.5m);
solver.Assert(prices[1] > prices[0]);

// BitVectors with natural operators
var bv = context.BitVecConst("bv", 32);
solver.Assert(bv + context.BitVec(5, 32) == context.BitVec(15, 32));
```

## Testing Guidelines

- **Framework**: NUnit with 1137+ tests organized by functionality
- **Coverage**: 90%+ requirement enforced in CI pipeline (fails below 90%)
- **Test Execution**: Always use `make test` (never `dotnet test`)
- **Naming**: `MethodName_Scenario_ExpectedResult` pattern
- **Organization**: Hierarchical structure by expression type and functionality

## AI Development Workflow

**CRITICAL**: Follow this exact workflow to avoid common AI mistakes:

1. **Before Changes**: Run `make help` to see available commands
2. **During Development**: Use `make test` frequently (not `dotnet test`)
3. **Adding Features**: Write tests first, then implementation
4. **Before Commits**: ALWAYS run `make format` and `make lint`
5. **Coverage Check**: Use `make coverage` to verify ≥90% coverage
6. **CI Verification**: Run `make ci` to ensure full pipeline passes
7. **Commit Process**: Ask "May I commit these changes?" and wait for approval

## Common AI Pitfalls to Avoid

### Project Structure Assumptions
- **File Counts**: 48 source files, 46 test files - don't assume what exists
- **Test Organization**: Unit/ vs Integration/ folders - check structure first
- **Extensions**: 20+ Z3ContextExtensions.*.cs files - use Glob/Grep to find
- **Coverage Critical**: 90%+ required by CI - new code must include comprehensive tests

### Command Usage Mistakes
- ❌ Using `dotnet test` instead of `make test`
- ❌ Using `dotnet format` instead of `make format`
- ❌ Using `dotnet build` instead of `make build`
- ❌ Forgetting to run `make format` before code changes
- ❌ Running git commands without explicit user permission

### Development Process Errors
- ❌ Adding new code without corresponding tests
- ❌ Modifying README examples without updating ReadmeExamplesTests.cs
- ❌ Committing without explicit user permission
- ❌ Assuming test counts or file structure without verification
- ❌ Ignoring coverage requirements (90% minimum)

## Quick AI Decision Guide

**Adding new functionality?**
→ Write tests first → Implement code → Run `make ci` → Verify ≥90% coverage

**Fixing bugs?**
→ Write failing test → Fix code → Run `make test` → Verify coverage maintained

**Changing README?**
→ Update README.md → Update ReadmeExamplesTests.cs to match exactly → Run `make test`

**Before any commit?**
→ Run `make format` → Run `make lint` → Ask user permission → Wait for approval

**Coverage below 90%?**
→ CI will fail → Add more tests → Run `make coverage` → Verify ≥90% before commit

**Unsure about project structure?**
→ Use `make help` → Use Glob/Grep tools → Don't assume file locations

### README Validation Framework

**CRITICAL**: All README.md examples are automatically validated in `ReadmeExamplesTests.cs`. This ensures 100% copy-paste reliability for users.

**Rules**:
- Test code must be IDENTICAL to README examples (zero modifications)
- Every README example must compile and run successfully
- Console output must be deterministically verified
- Use exact pattern: Preparation → Example → Assertions regions

**Update Process**:
1. Update README.md examples
2. Update corresponding test in ReadmeExamplesTests.cs to match exactly
3. Run `make test` to verify (all tests must pass)
4. README examples remain living, guaranteed-working documentation

## Memory Management

- **Z3Context**: Reference-counted (`Z3_mk_context_rc`) with automatic cleanup
- **Z3Expr classes**: Do NOT implement IDisposable - managed by context reference counting
- **Expressions**: Automatically cleaned up when context disposed
- **Solvers**: Implement IDisposable, tracked by context for proper cleanup
- **Thread Safety**: Context setup uses ThreadLocal for safe scoped operations

## Coding Standards

- **Naming**: Standard C# conventions (PascalCase public, camelCase private)
- **Style**: Enforced by CSharpier (run `make format` before commits)
- **Validation**: `make lint` checks formatting (used in CI)
- **Generics**: Extensive use with constraints for type safety
- **Operators**: Comprehensive overloading for natural mathematical syntax

### XML Documentation

**CRITICAL**: The project has XML documentation warnings enabled. After any public API changes, run `make build` - it MUST produce ZERO warnings.

**Principles**: Document only `public` members. Be **concise, precise, and short**.

**Standard Patterns**:
```csharp
// Classes/Structs
/// <summary>
/// Represents [what it is] for [primary purpose].
/// </summary>

// Methods
/// <summary>
/// [Action verb] [what it does] [with/from/using what].
/// </summary>
/// <param name="paramName">The [description].</param>
/// <returns>[What it returns].</returns>

// Properties
/// <summary>
/// Gets [what it represents].
/// </summary>

// Operators
/// <summary>
/// [Operation] of two [type] values.
/// </summary>
```

**XML Encoding** (CRITICAL):
- Use `&lt;` instead of `<`
- Use `&gt;` instead of `>`
- Use `&amp;` instead of `&`
- Example: `List&lt;T&gt;` not `List<T>`

**Examples**:
```csharp
// ✅ GOOD - Concise and clear
/// <summary>
/// Creates integer constant with specified name.
/// </summary>
public IntExpr IntConst(string name)

// ✅ GOOD - Proper XML encoding
/// <summary>
/// Creates array expression of type TDomain to TRange.
/// </summary>
/// <typeparam name="TDomain">Array index type.</typeparam>
public ArrayExpr&lt;TDomain, TRange&gt; ArrayConst&lt;TDomain, TRange&gt;(string name)

// ❌ BAD - Too verbose
/// <summary>
/// This method creates a new integer constant expression which can be used
/// for mathematical operations and constraint solving...
/// </summary>

// ❌ BAD - Wrong XML encoding
/// <summary>
/// Creates List<T> with elements.
/// </summary>
```

**Quality Checks**:
- `make build` produces zero warnings
- XML comments appear correctly in IDE IntelliSense
- Use present tense ("Gets", "Creates", not "Get", "Create")
- Avoid implementation details - focus on functionality

## Architecture Notes

- **Target**: .NET 9.0 with implicit usings and nullable reference types
- **Interop**: Dynamic library loading for cross-platform Z3 discovery
- **P/Invoke**: Complete Z3 C API bindings with proper marshalling
- **Extensions**: 20+ organized extension files for clean API surface
- **Error Handling**: Proper exception handling and resource disposal patterns

## Git Workflow

**❌ CRITICAL RULE**: NEVER commit without explicit user permission.

**Process**:
1. Make changes using appropriate `make` commands
2. Run `make format` before any commit
3. Ask "May I commit these changes?" and wait for explicit approval
4. Only commit when explicitly authorized
5. Use descriptive commit messages with brief summary + detailed explanation

**Forbidden**: Automatic commits, running git commands without permission.

### Commit Message Rules

**❌ CRITICAL**: NEVER write commit messages based on assumptions or knowledge - ALWAYS examine actual changes first.

**MANDATORY Process**:
1. **Examine Changes**: Run `git diff --staged` to see exactly what changed
2. **Analyze Impact**: Understand what each file change actually does
3. **Write Accurately**: Commit message must reflect actual changes, not assumptions
4. **Be Specific**: Mention specific files, functions, or features that were modified

**Examples**:
- ❌ BAD: "fix: improve error handling" (without checking what actually changed)
- ✅ GOOD: "refactor: remove try-catch blocks from Z3Model.ToString() and Z3Expr.ToString()"

**Common Mistakes**:
- ❌ Assuming what changes were made without looking
- ❌ Using generic descriptions like "improve" or "enhance" without specifics
- ❌ Mentioning features that weren't actually modified
- ❌ Writing commit messages before examining the diff

## Development Setup

```bash
make dev-setup    # Install CSharpier, ReportGenerator, and other tools
make help         # See all available commands
make ci           # Verify full CI pipeline works locally
```

This project emphasizes consistent tooling, comprehensive testing, and maintainable code architecture for reliable theorem proving capabilities.