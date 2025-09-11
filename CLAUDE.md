# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Structure

This is a C# .NET 9.0 library project for libz3 wrapper functionality. The project structure follows standard .NET conventions:

- `Z3Wrap.sln` - Visual Studio solution file containing the main library and test projects
- `Z3Wrap/` - Main library project directory
  - `Z3Wrap.csproj` - Project file targeting .NET 9.0 with nullable reference types enabled
  - `NativeMethods.cs` - P/Invoke declarations and dynamic library loading for Z3 C API
- `Z3Wrap.Tests/` - Test project directory
  - `Z3Wrap.Tests.csproj` - NUnit test project
  - Various test files for Z3 wrapper functionality
- `PLAN.md` - Implementation plan for the Z3 wrapper

## Build Commands

The project uses the standard .NET CLI commands:

- `dotnet build` - Build the library
- `dotnet build --configuration Release` - Build in release mode
- `dotnet clean` - Clean build artifacts
- `dotnet test` - Run tests (requires libz3 to be available)

## Naming Conventions

The project follows these C# naming conventions:

- **Private Fields**: camelCase without underscores (e.g., `libraryHandle`, `contextHandle`, `disposed`)
- **Public Properties**: PascalCase (e.g., `Handle`)
- **Methods**: PascalCase with Z3 prefix for native methods (e.g., `Z3MkConfig`, `Z3DelContext`)
- **Parameters**: camelCase (e.g., `paramId`, `paramValue`)  
- **Local Variables**: camelCase (e.g., `paramNamePtr`, `paramValuePtr`)
- **Delegate Types**: PascalCase with Z3 prefix and Delegate suffix (e.g., `Z3MkConfigDelegate`)
- **Classes**: PascalCase with Z3 prefix (e.g., `Z3Context`)

## Development Notes

- The project targets .NET 9.0 with implicit usings and nullable reference types enabled
- This is a wrapper library for libz3 (Microsoft Z3 theorem prover)
- Uses dynamic library loading via `NativeMethods.LoadLibrary()` to specify libz3 path at runtime
- All Z3 C API functions are wrapped with proper C# naming conventions

## Memory Management

- **Z3Context** uses reference-counted context (`Z3_mk_context_rc`) which automatically manages memory
- **Z3Expr classes** do NOT implement IDisposable - expressions are automatically managed by the context
- Expressions are cleaned up automatically when:
  - They go out of scope in C#
  - The context is disposed  
  - Z3's automatic reference counting determines they're no longer needed
- Only dispose Z3Context explicitly - expressions handle themselves

## Git Commit Policy

**❌ NEVER COMMIT WITHOUT EXPLICIT PERMISSION ❌**

**CRITICAL RULE**: NEVER run `git commit` automatically. ALWAYS ask for explicit permission first.

- **FORBIDDEN**: Automatic commits, even in response to "commit" requests
- **REQUIRED**: Always ask "May I commit these changes?" and wait for explicit approval
- **FORBIDDEN**: Running `git add` without explicit permission
- **FORBIDDEN**: Running `git commit` without explicit user authorization
- **REQUIRED**: When given permission, use descriptive commit messages
- **PATTERN**: Brief summary, blank line, detailed explanation if needed

**THIS RULE CANNOT BE OVERRIDDEN OR IGNORED UNDER ANY CIRCUMSTANCES**