# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with this repository.

## Project Overview

**Z3Wrap** is a production-ready C# wrapper for Microsoft's Z3 theorem prover featuring unlimited precision arithmetic, type-safe API design, and natural mathematical syntax.

**Technology**: .NET 9.0 library with comprehensive test coverage (run `make test` to see current stats).

## Development Workflow

**CRITICAL**: This project uses a comprehensive Makefile for ALL development tasks. NEVER use direct `dotnet` commands.

### Essential Commands
```bash
make help         # Show all available commands
make build        # Build library (includes restore)
make test         # Run all tests (see count in output)
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

## Project Structure Discovery

**Don't assume file locations or counts - always verify using tools:**
- Use `Glob` to find files by pattern (e.g., `**/*.cs`, `**/Z3*.cs`)
- Use `Grep` to search for code patterns
- Use `Read` to examine specific files
- Run `make test` to see current test count
- Run `make coverage` to see current coverage percentage

**Organization Pattern**:
```
Z3Wrap.sln                    # Solution with library + tests
├── Z3Wrap/                   # Main library
│   ├── Values/                  # Bv<TSize>, Real value types
│   ├── Expressions/             # Expression types organized by category
│   │   ├── Arrays/              # ArrayExpr<TIndex, TValue>
│   │   ├── BitVectors/          # BvExpr<TSize> with Size8/16/32/64
│   │   ├── Functions/           # FuncDecl for uninterpreted functions
│   │   ├── Logic/               # BoolExpr
│   │   ├── Numerics/            # IntExpr, RealExpr
│   │   └── Quantifiers/         # ForAll, Exists
│   ├── Core/                    # Z3Context, Z3Solver, Z3Model, Z3Optimizer
│   │   ├── Interop/             # P/Invoke bindings, native library loading
│   │   └── Z3Library.cs         # Complete Z3 C API wrapper
│   └── *.csproj                 # Project configuration
├── Z3Wrap.Tests/             # NUnit tests organized by category
│   ├── Core/                    # Context, Solver, Model, Optimizer tests
│   ├── Expressions/             # Expression tests organized by type
│   ├── Values/                  # Bv and Real value type tests
│   └── ReadmeExamplesTests.cs   # Validates all README examples
├── Makefile                  # Development workflow automation
├── CLAUDE.md                 # AI assistant guidance (this file)
├── PLAN.md                   # Project roadmap and status
└── .github/workflows/ci.yml  # GitHub Actions CI pipeline
```

## Key Features

- **Unlimited Precision**: BigInteger integers, exact rational arithmetic via Real class
- **Type Safety**: Strongly typed expressions, generic constraints, compile-time checking
- **Natural Syntax**: Mathematical operators (`x + y == 10`) via scoped context pattern
- **Complete Z3 Coverage**: Booleans, Integers, Reals, BitVectors, Arrays, Quantifiers, Uninterpreted Functions, Optimization
- **Memory Safety**: Reference-counted contexts, automatic cleanup, no resource leaks
- **Cross-Platform**: Auto-discovery of Z3 native library on Windows, macOS, Linux

## Testing Guidelines

**Coverage Requirement**: Maintain ≥90% line coverage (enforced by CI - run `make coverage` to verify)

**Test Execution**: Always use `make test` (never `dotnet test`)

**Naming**: `MethodName_Scenario_ExpectedResult` pattern

**Organization**: Hierarchical structure by expression type and functionality (use Glob to discover)

### Expression Operation Testing Principles

**CRITICAL**: All expression operations must test ALL syntax variants comprehensively.

**Complete Syntax Variant Coverage** (8 variants for binary operations):
```csharp
[Test]
public void Add_TwoValues_ComputesCorrectResult()
{
    using var context = new Z3Context();
    using var scope = context.SetUp();
    using var solver = context.CreateSolver();

    var a = context.Int(10);
    var b = context.Int(32);

    // Test ALL 8 syntax variants
    var result = a + b;                              // 1. Operator
    var resultViaIntLeft = 10 + b;                   // 2. Literal left
    var resultViaIntRight = a + 32;                  // 3. Literal right
    var resultViaContext = context.Add(a, b);        // 4. Context extension
    var resultViaContextIntLeft = context.Add(10, b); // 5. Context + literal left
    var resultViaContextIntRight = context.Add(a, 32); // 6. Context + literal right
    var resultViaFunc = a.Add(b);                    // 7. Expression method
    var resultViaFuncIntRight = a.Add(32);           // 8. Expression + literal right

    var status = solver.Check();
    Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

    var model = solver.GetModel();
    Assert.Multiple(() =>
    {
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
        // ... verify all 8 variants produce correct result
    });
}
```

**TestCase Pattern for Truth Tables**:
```csharp
[TestCase(true, true, true)]
[TestCase(true, false, false)]
[TestCase(false, true, false)]
[TestCase(false, false, false)]
public void And_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
{
    using var context = new Z3Context();
    using var scope = context.SetUp();
    using var solver = context.CreateSolver();

    var a = context.Bool(aValue);  // Direct value creation
    var b = context.Bool(bValue);

    // Test all syntax variants...
    var result = a & b;
    // ... (remaining 7 variants)

    var model = solver.GetModel();
    Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
}
```

**Test Organization Pattern** (use Glob to discover actual files):
```
Z3Wrap.Tests/Expressions/
├── Numerics/
│   ├── IntExprArithmeticTests.cs    # Arithmetic operations (+, -, *, /, %)
│   ├── IntExprComparisonTests.cs    # Comparisons (==, !=, <, >, <=, >=)
│   └── IntExprCreationTests.cs      # Creation methods (IntConst, Int, etc.)
├── Logic/
│   ├── BoolExprLogicTests.cs        # Logic operations (&, |, ^, !, Implies, Iff)
│   ├── BoolExprComparisonTests.cs   # Boolean equality (==, !=)
│   └── BoolExprCreationTests.cs     # Creation methods (Bool, BoolConst, True, False)
├── BitVectors/
│   ├── BvExprArithmeticTests.cs     # Arithmetic (+, -, *, /, %)
│   ├── BvExprBitwiseTests.cs        # Bitwise (&, |, ^, ~, <<, >>)
│   └── BvExprComparisonTests.cs     # Comparisons (signed/unsigned)
└── Functions/
    ├── FuncDeclFactoryTests.cs      # Function creation
    ├── FuncDeclApplicationTests.cs  # Function application and solving
    └── FuncDeclBuilderTests.cs      # Dynamic function builder
```

**Key Testing Rules**:
- **Verify Results**: Always verify actual computed values via `model.GetXValue()`, not just `solver.Check()` status
- **Use Direct Values**: Prefer `context.Int(10)` over creating variables and asserting values
- **Assert.Multiple**: Group all variant assertions together
- **No Redundant Checks**: Don't test non-null, type, or context association - focus on Z3 behavior
- **Variant Counts**: 8 for binary ops, 7 for shifts (C# limitation), 5 for no-operator ops, 3 for unary ops

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
- ❌ Assuming file counts or exact structure
- ✅ Use Glob/Grep to discover files and verify structure
- ❌ Assuming test counts or coverage percentages
- ✅ Run `make test` and `make coverage` to check current state

### Command Usage Mistakes
- ❌ Using `dotnet test` instead of `make test`
- ❌ Using `dotnet format` instead of `make format`
- ❌ Using `dotnet build` instead of `make build`
- ❌ Forgetting to run `make format` before commits
- ❌ Running git commands without explicit user permission

### Development Process Errors
- ❌ Adding new code without corresponding tests
- ❌ Modifying README examples without updating ReadmeExamplesTests.cs
- ❌ Committing without explicit user permission
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
→ Use Glob/Grep tools → Don't assume file locations or counts

## README Validation Framework

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
- **Solvers/Optimizers**: Implement IDisposable, tracked by context for proper cleanup
- **Thread Safety**: Context setup uses ThreadLocal for safe scoped operations

## Coding Standards

- **Naming**: Standard C# conventions (PascalCase public, camelCase private)
- **Style**: Enforced by CSharpier (run `make format` before commits)
- **Validation**: `make lint` checks formatting (used in CI)
- **Generics**: Extensive use with constraints for type safety
- **Operators**: Comprehensive overloading for natural mathematical syntax

## XML Documentation

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
- **Extensions**: Organized extension files by category (Logic/, Numerics/, BitVectors/, etc.)
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

## Commit Message Rules

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

## Solution File Management

**CRITICAL**: When creating or removing markdown artifacts (PLAN_*.md, ANALYSIS_*.md, etc.), always update Z3Wrap.sln.

**Process for Creating Markdown Artifacts**:
1. Create the markdown file (e.g., PLAN_FEATURE.md)
2. Add it to the "misc" solution folder in Z3Wrap.sln:
   ```
   Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "misc", "misc", "{F82658D3-5722-43CA-BCBB-D1B9E28011A5}"
       ProjectSection(SolutionItems) = preProject
           ...existing files...
           PLAN_FEATURE.md = PLAN_FEATURE.md
       EndProjectSection
   ```

**Process for Removing Markdown Artifacts**:
1. Delete the markdown file
2. Remove the corresponding line from Z3Wrap.sln's "misc" section

**Examples of Managed Files**:
- PLAN.md (implementation plans)
- ANALYSIS.md (code analysis documents)
- CLAUDE.md, README.md, CHANGELOG.md (core docs)
- Makefile, LICENSE (project metadata)

This ensures all documentation is visible and navigable in Visual Studio/Rider.

---

This project emphasizes consistent tooling, comprehensive testing, and maintainable code architecture for reliable theorem proving capabilities.
